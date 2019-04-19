using Microsoft.FSharp.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BallsModelDrawer
{
    public partial class Form1 : Form
    {
        float scaleGraphics = 0.3f;
        float scaleModel = 50;
        float scaleFinal = 1;
        float scaleVectors = 10;
        float translateX = 30;
        float translateY = 30;
        bool showCenter = false;
        bool showFrame = false;
        bool showVector = true;

        const double boxWidth = 100;
        const double boxHeight = 60;
        const int ballsCount = 3000;

        double bounceGate = 0.1;
        double bounceScalePower = 3.0;

        double lastSimTime = 0;
        double lastRealTime = 0;

        double timeScale = 0.5;
        double timeScalePower = 3;

        List<double> cpuUsageBuffer = new List<double>();
        List<double> frameDurationBuffer = new List<double>();

        Room room;
        Stopwatch stopWatch = new Stopwatch();
        Timer renderTimer = new Timer();
        Timer textStateTimer = new Timer();
        PerformanceCounter myAppCpu;

        List<BallsModel.Point> newWallBuffer = new List<BallsModel.Point>();

        public Form1()
        {                   
            InitializeComponent();

            myAppCpu =
                 new PerformanceCounter("Processor", "% Processor Time", "_Total");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Text = "Click - put point; R - reset";

            DoubleBuffered = true;
            BackColor = Color.White;
            KeyPreview = true;

            trackBarBounceGate.Value = (int)(Math.Pow(bounceGate, 1.0 / bounceScalePower) * 1000);
            trackBarTimeScale.Value = (int)(Math.Pow(timeScale, 1.0 / timeScalePower) * 100);
            labelTimeScale.Text = string.Format("TimeScale: x{0}", Math.Round(timeScale, 2));

            Paint += Form1_Paint;
            Resize += Form1_Resize;
            MouseDown += Form1_MouseDown;
            KeyDown += Form1_KeyDown;

            checkBoxShowCenter.Checked = showCenter;
            checkBoxShowCenter.CheckedChanged += CheckBoxShowCenter_CheckedChanged; ;

            checkBoxShowFrame.Checked = showFrame;
            checkBoxShowFrame.CheckedChanged += CheckBoxShowFrame_CheckedChanged;

            checkBoxShowVector.Checked = showVector;
            checkBoxShowVector.CheckedChanged += CheckBoxShowVector_CheckedChanged;

            trackBarTimeScale.ValueChanged += TrackBarTimeScale_ValueChanged;
            trackBarBounceGate.ValueChanged += TrackBarBounceGate_ValueChanged;

            start();
        }

        private void TrackBarBounceGate_ValueChanged(object sender, EventArgs e)
        {
            bounceGate = Math.Pow((double)trackBarBounceGate.Value / 1000, bounceScalePower);
            room.bounceGate = bounceGate;

            labelBounceGate.Text = string.Format("BounceGate: {0}s", Math.Round(bounceGate, 3));
        }

        private void CheckBoxShowCenter_CheckedChanged(object sender, EventArgs e)
        {
            showCenter = checkBoxShowCenter.Checked;
        }

        private void CheckBoxShowVector_CheckedChanged(object sender, EventArgs e)
        {
            showVector = checkBoxShowVector.Checked;
        }

        private void CheckBoxShowFrame_CheckedChanged(object sender, EventArgs e)
        {
            showFrame = checkBoxShowFrame.Checked;
        }

        private void TrackBarTimeScale_ValueChanged(object sender, EventArgs e)
        {
            double val = (double)trackBarTimeScale.Value / (trackBarTimeScale.Maximum / 2);
            double val2 = Math.Pow(val, timeScalePower);

            timeScale = val2;
            labelTimeScale.Text = string.Format("TimeScale: x{0}", Math.Round(val2, 2));
        }

        private void printState()
        {
            double impulse = room.currentState.balls.Sum((ball) => ball.ph.speed.length);
            double cpu = cpuUsageBuffer.Average();
            double frameDuration = frameDurationBuffer.Average() * 1000;
            int wallLinesCount = room.currentState.walls.Sum(wall => wall.frame.lines.Length);

            Text =
                string.Format(
                    "Solved: {0}, Time: {1}, CPU: {2, 3}, Room Impulse: {3}, LastFrame: {4}ms, InstancesCount: {5}, WallLinesCount: {6}",
                    room.solved,
                    Math.Round(lastRealTime),
                    Math.Round(cpu),
                    Math.Round(impulse),
                    Math.Round(frameDuration, 1),
                    room.currentState.balls.Length,
                    wallLinesCount
                );
        }

        private void start()
        {
            //BallsModel.Ball[] balls = new BallsModel.Ball[]
            //{
            //    new BallsModel.Ball(
            //        new BallsModel.Vector(20.0, 50.0),
            //        1.0,
            //        new BallsModel.Circle(5.0, 5.0, 1.0),
            //        FSharpOption<Guid>.None
            //    ),
            //    new BallsModel.Ball(
            //        new BallsModel.Vector(-20.0, 60.0),
            //        2.0,
            //        new BallsModel.Circle(5.0, 5.0, 2.0),
            //        FSharpOption<Guid>.None
            //    )
            //};

            Random rnd = new Random();
            BallsModel.Ball[] balls = new BallsModel.Ball[ballsCount].Select(_ =>
            {
                const double speed = 3;
                double dx = rnd.NextDouble() * 5 * speed;
                double dy = rnd.NextDouble() * 5 * speed;
                double r = rnd.NextDouble() * 2 + 0.1;
                //double dx = 3;
                //double dy = 0.1;

                return new BallsModel.Ball(
                    new BallsModel.Vector(dx, dy),
                    1.0,
                    new BallsModel.Circle(8.0, 5.0, r),
                    FSharpOption<Guid>.None
                );
            }).ToArray();

            double wallWeight = Math.Pow(10, 5);
            BallsModel.Wall[] walls = new BallsModel.Wall[]{
                //new BallsModel.Wall(
                //    new BallsModel.Point[]
                //    {
                //        new BallsModel.Point(20.0, 10.0),
                //        new BallsModel.Point(20.0, 45.0),
                //        new BallsModel.Point(25.0, 45.0),
                //        new BallsModel.Point(25.0, 10.0),
                //    },
                //    wallWeight,
                //    FSharpOption<Guid>.None
                //),
                //new BallsModel.Wall(
                //    new BallsModel.Point[]
                //    {
                //        new BallsModel.Point(20.0, 6.0),
                //        new BallsModel.Point(20.0, 7.0),
                //        new BallsModel.Point(25.0, 7.0),
                //        new BallsModel.Point(25.0, 6.0),
                //    },
                //    wallWeight,
                //    FSharpOption<Guid>.None
                //),

                // anchors
                new BallsModel.Wall(
                    new BallsModel.Point[]
                    {
                        new BallsModel.Point(2.0, 0.0),
                        new BallsModel.Point(-1.0, 3.0),
                        new BallsModel.Point(-1.0, 0.0),
                    },
                    wallWeight,
                    FSharpOption<Guid>.None
                ),
                new BallsModel.Wall(
                    new BallsModel.Point[]
                    {
                        new BallsModel.Point(boxWidth - 2, 0.0),
                        new BallsModel.Point(boxWidth + 1, 3.0),
                        new BallsModel.Point(boxWidth + 1, 0.0),
                    },
                    wallWeight,
                    FSharpOption<Guid>.None
                ),
                new BallsModel.Wall(
                    new BallsModel.Point[]
                    {
                        new BallsModel.Point(boxWidth + 1, boxHeight - 2),
                        new BallsModel.Point(boxWidth - 2, boxHeight + 1),
                        new BallsModel.Point(boxWidth - 2, boxHeight + 1),
                    },
                    wallWeight,
                    FSharpOption<Guid>.None
                ),
                new BallsModel.Wall(
                    new BallsModel.Point[]
                    {
                        new BallsModel.Point(-1.0, boxHeight - 2),
                        new BallsModel.Point(2.0, boxHeight + 1),
                        new BallsModel.Point(-1.0, boxHeight + 1),
                    },
                    wallWeight,
                    FSharpOption<Guid>.None
                ),

                // square walls
                new BallsModel.Wall(
                    new BallsModel.Point[]
                    {
                        new BallsModel.Point(boxWidth - 2, 0.0),
                        //new BallsModel.Point(39.0, 3.0),
                        new BallsModel.Point(boxWidth + 1, 3.0),
                        new BallsModel.Point(boxWidth + 1, 0.0),
                    },
                    wallWeight,
                    FSharpOption<Guid>.None
                ),
                new BallsModel.Wall(
                    new BallsModel.Point[]
                    {
                        new BallsModel.Point(-1.0, 0.0),
                        new BallsModel.Point(boxWidth + 1, 0.0),
                        new BallsModel.Point(boxWidth + 1, 1.0),
                        new BallsModel.Point(-1.0, 1.0),
                    },
                    wallWeight,
                    FSharpOption<Guid>.None
                ),
                new BallsModel.Wall(
                    new BallsModel.Point[]
                    {
                        new BallsModel.Point(boxWidth, 0.0),
                        new BallsModel.Point(boxWidth, boxHeight + 1),
                        new BallsModel.Point(boxWidth + 1, boxHeight + 1),
                        new BallsModel.Point(boxWidth + 1, 0.0),
                    },
                    wallWeight,
                    FSharpOption<Guid>.None
                ),
                new BallsModel.Wall(
                    new BallsModel.Point[]
                    {
                        new BallsModel.Point(boxWidth + 1, boxHeight),
                        new BallsModel.Point(-1.0, boxHeight),
                        new BallsModel.Point(-1.0, boxHeight + 1),
                        new BallsModel.Point(boxWidth + 1, boxHeight + 1),
                    },
                    wallWeight,
                    FSharpOption<Guid>.None
                ),
                new BallsModel.Wall(
                    new BallsModel.Point[]
                    {
                        new BallsModel.Point(0.0, 0.0),
                        new BallsModel.Point(0.0, boxHeight + 1),
                        new BallsModel.Point(-1.0, boxHeight + 1),
                        new BallsModel.Point(-1.0, 0.0),
                    },
                    wallWeight,
                    FSharpOption<Guid>.None
                ),

                // other walls
                new BallsModel.Wall(
                    new BallsModel.Point[]
                    {
                        new BallsModel.Point(20.0, 20.0),
                        new BallsModel.Point(30.0, 15.0),
                        new BallsModel.Point(35.0, 30.0),
                        new BallsModel.Point(25.0, 33.0),
                    },
                    wallWeight,
                    FSharpOption<Guid>.None
                ),
            };

            BallsModel.State initialState = 
                new BallsModel.State(
                    balls, 
                    walls,
                    (double)stopWatch.ElapsedMilliseconds / 1000, 
                    FSharpOption<
                        Microsoft.FSharp.Collections.FSharpMap<
                            Guid, 
                            BallsModel.Collision
                        >
                    >.None
                );
            room = new Room(initialState);

            renderTimer.Interval = 4;
            renderTimer.Tick += (s, a) =>
            {
                Invalidate();
            };

            textStateTimer.Interval = 100;
            textStateTimer.Tick += (s, a) =>
            {
                printState();
            };

            textStateTimer.Start();
            renderTimer.Start();
            stopWatch.Start();

            lastRealTime = (double)stopWatch.ElapsedMilliseconds / 1000;
            lastSimTime = lastRealTime;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.R:
                    start();
                    break;
                case Keys.F:
                    if (WindowState == FormWindowState.Maximized)
                        WindowState = FormWindowState.Normal;
                    else
                        WindowState = FormWindowState.Maximized;
                    break;
                case Keys.V:
                    showVector = !showVector;
                    break;
                case Keys.C:
                    showCenter = !showCenter;
                    break;
                case Keys.B:
                    showFrame = !showFrame;
                    break;
                case Keys.Space:
                    if (newWallBuffer.Count < 3)
                        break;

                    BallsModel.Wall wall = new BallsModel.Wall(
                        newWallBuffer.ToArray(),
                        1000,
                        FSharpOption<Guid>.None
                    );

                    newWallBuffer.Clear();

                    room.addWall(wall);
                    break;
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            //if (ModifierKeys != Keys.Control)
            //    return;

            labelFocusing.Focus();

            BallsModel.Point p = new BallsModel.Point(
                (e.X - translateX) / scaleFinal,
                (e.Y - translateY) / scaleFinal
                );
            newWallBuffer.Add(p);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            float scaleGraphics = this.scaleGraphics * Math.Min(ClientSize.Width, ClientSize.Height) / 1000f;
            scaleFinal = scaleGraphics * scaleModel;

            //float translateX = this.translateX + ClientSize.Width / 2;
            //float translateY = this.translateY + ClientSize.Height / 2;
            float translateX = this.translateX;
            float translateY = this.translateY;

            // scale to 10 and translate to the center
            g.TranslateTransform(translateX, translateY);
            g.ScaleTransform(scaleGraphics, scaleGraphics);

            // draw axises
            //Pen axisPen = new Pen(Color.Black, 1f);
            //g.DrawLine(axisPen, -ClientSize.Width, 0, ClientSize.Width, 0);
            //g.DrawLine(axisPen, 0, -ClientSize.Height, 0, ClientSize.Height);

            // draw temp polygon
            if (newWallBuffer.Count > 1)
            {
                Pen newWallPen = new Pen(Color.DarkCyan, 2);
                PointF[] points =
                    newWallBuffer
                    .Select(p => new PointF((float)p.x * scaleModel, (float)p.y * scaleModel))
                    .ToArray();

                HatchBrush hBrush = new HatchBrush(
                   HatchStyle.ForwardDiagonal,
                   Color.White,
                   Color.DarkCyan
                );

                g.FillPolygon(
                    hBrush,
                    points
                );

                g.DrawPolygon(
                    newWallPen,
                    points
                );
            }

            if (room == null)
                return;

            // draw current state
            double time = (double)stopWatch.ElapsedMilliseconds / 1000;            
            double frameDeltaReal = time - lastRealTime;

            double simTime = lastSimTime + frameDeltaReal * timeScale;
            //double scaledTime = time;
            lastRealTime = time;
            lastSimTime = simTime;

            BallsModel.State currentState = room.getActualState(simTime);

            double cpu = myAppCpu.NextValue();
            cpuUsageBuffer.Add(cpu);
            if (cpuUsageBuffer.Count > 55)
                cpuUsageBuffer.RemoveAt(0);

            frameDurationBuffer.Add(frameDeltaReal);
            if (frameDurationBuffer.Count > 55)
                frameDurationBuffer.RemoveAt(0);

            StateDrawer.Draw(
                g, 
                currentState, 
                simTime, 
                scaleModel, 
                scaleVectors, 
                showCenter, 
                showFrame, 
                showVector
            );
        }
    }    
}
