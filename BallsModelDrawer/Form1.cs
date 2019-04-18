using Microsoft.FSharp.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BallsModelDrawer
{
    public partial class Form1 : Form
    {
        float scaleGraphics = 0.4f;
        float scaleModel = 50;
        float scaleFinal = 1;
        float scaleVectors = 10;
        float translateX = 30;
        float translateY = 30;

        double lastTime = 0;
        List<double> cpuUsageBuffer = new List<double>();

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

            Paint += Form1_Paint;
            Resize += Form1_Resize;
            MouseDown += Form1_MouseDown;
            KeyDown += Form1_KeyDown;

            start();
        }

        private void printState()
        {
            double cpu = cpuUsageBuffer.Average();
            Text =
                string.Format(
                    "Solved: {0}, Time: {1}, CPU: {2}",
                    room.solved,
                    Math.Round(lastTime, 3).ToString(),
                    Math.Round(cpu, 1)
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
            BallsModel.Ball[] balls = new BallsModel.Ball[150].Select(_ =>
            {
                const double speed = 3;
                double dx = rnd.NextDouble() * 5 * speed;
                double dy = rnd.NextDouble() * 5 * speed;
                //double dx = 3;
                //double dy = 0.1;

                return new BallsModel.Ball(
                    new BallsModel.Vector(dx, dy),
                    1.0,
                    new BallsModel.Circle(8.0, 5.0, 1.0),
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
                        new BallsModel.Point(38.0, 0.0),
                        new BallsModel.Point(41.0, 3.0),
                        new BallsModel.Point(41.0, 0.0),
                    },
                    wallWeight,
                    FSharpOption<Guid>.None
                ),
                new BallsModel.Wall(
                    new BallsModel.Point[]
                    {
                        new BallsModel.Point(41.0, 38.0),
                        new BallsModel.Point(38.0, 41.0),
                        new BallsModel.Point(38.0, 41.0),
                    },
                    wallWeight,
                    FSharpOption<Guid>.None
                ),
                new BallsModel.Wall(
                    new BallsModel.Point[]
                    {
                        new BallsModel.Point(-1.0, 38.0),
                        new BallsModel.Point(2.0, 41.0),
                        new BallsModel.Point(-1.0, 41.0),
                    },
                    wallWeight,
                    FSharpOption<Guid>.None
                ),

                // square walls
                new BallsModel.Wall(
                    new BallsModel.Point[]
                    {
                        new BallsModel.Point(38.0, 0.0),
                        //new BallsModel.Point(39.0, 3.0),
                        new BallsModel.Point(41.0, 3.0),
                        new BallsModel.Point(41.0, 0.0),
                    },
                    wallWeight,
                    FSharpOption<Guid>.None
                ),
                new BallsModel.Wall(
                    new BallsModel.Point[]
                    {
                        new BallsModel.Point(-1.0, 0.0),
                        new BallsModel.Point(41.0, 0.0),
                        new BallsModel.Point(41.0, 1.0),
                        new BallsModel.Point(-1.0, 1.0),
                    },
                    wallWeight,
                    FSharpOption<Guid>.None
                ),
                new BallsModel.Wall(
                    new BallsModel.Point[]
                    {
                        new BallsModel.Point(40.0, 0.0),
                        new BallsModel.Point(40.0, 41.0),
                        new BallsModel.Point(41.0, 41.0),
                        new BallsModel.Point(41.0, 0.0),
                    },
                    wallWeight,
                    FSharpOption<Guid>.None
                ),
                new BallsModel.Wall(
                    new BallsModel.Point[]
                    {
                        new BallsModel.Point(41.0, 40.0),
                        new BallsModel.Point(-1.0, 40.0),
                        new BallsModel.Point(-1.0, 41.0),
                        new BallsModel.Point(41.0, 41.0),
                    },
                    wallWeight,
                    FSharpOption<Guid>.None
                ),
                new BallsModel.Wall(
                    new BallsModel.Point[]
                    {
                        new BallsModel.Point(0.0, 0.0),
                        new BallsModel.Point(0.0, 41.0),
                        new BallsModel.Point(-1.0, 41.0),
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
                        new BallsModel.Point(24.0, 30.0),
                        new BallsModel.Point(14.0, 13.0),
                    },
                    wallWeight,
                    FSharpOption<Guid>.None
                ),
            };

            BallsModel.State initialState = new BallsModel.State(balls, walls, 0.0);
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
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.R:
                    Invalidate();
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
            if (ModifierKeys != Keys.Control)
                return;

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

                g.DrawPolygon(
                    newWallPen,
                    points
                );
            }

            if (room == null)
                return;

            // draw current state
            double time = (double)stopWatch.ElapsedMilliseconds / 1000;
            BallsModel.State currentState = room.getActualState(time);

            lastTime = time;

            double cpu = myAppCpu.NextValue();
            cpuUsageBuffer.Add(cpu);
            if (cpuUsageBuffer.Count > 55)
                cpuUsageBuffer.RemoveAt(0);

            StateDrawer.Draw(g, currentState, time, scaleModel, scaleVectors);
        }
    }    
}
