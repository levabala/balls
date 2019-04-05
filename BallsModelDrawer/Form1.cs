using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BallsModelDrawer
{
    public partial class Form1 : Form
    {
        List<BallsModel.Point> localPoints = new List<BallsModel.Point>(){
            new BallsModel.Point(1, 1),
            new BallsModel.Point(3, 2),
            new BallsModel.Point(2, 5),
            new BallsModel.Point(5, 10),
            new BallsModel.Point(-2, 12),
        };
        BallsModel.Point[] points;
        BallsModel.Polygone poly;
        bool polyIsEmpty = false;
        float scale = 1;
        float translateX = 0;
        float translateY = 0;

        public Form1()
        {
            InitializeComponent();

            rebuildData();
        }

        private void rebuildData()
        {                           
            // get model data
            points = localPoints.ToArray();

            if (points.Length == 0)
                polyIsEmpty = true;
            else
            {
                poly = new BallsModel.Polygone(points);
                polyIsEmpty = false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Text = "Click - put point; R - reset";

            DoubleBuffered = true;
            BackColor = Color.White;

            Paint += Form1_Paint;
            Resize += Form1_Resize;
            MouseClick += Form1_MouseClick;
            KeyDown += Form1_KeyDown;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.R:
                    localPoints.Clear();
                    rebuildData();
                    Invalidate();
                    break;
            }
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            localPoints.Add(
                new BallsModel.Point(
                    (decimal)((e.X - translateX) / scale), 
                    (decimal)((e.Y - translateY) / scale)
                )
            );
            rebuildData();

            Invalidate();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            scale = ClientSize.Width / 60f;
            translateX = ClientSize.Width / 2;
            translateY = ClientSize.Height / 2;

            // scale to 10 and translate to the center
            g.TranslateTransform(translateX, translateY);
            g.ScaleTransform(scale, scale);

            // draw axises
            Pen axisPen = new Pen(Color.Black, 0.1f);
            g.DrawLine(axisPen, -ClientSize.Width, 0, ClientSize.Width, 0);
            g.DrawLine(axisPen, 0, -ClientSize.Height, 0, ClientSize.Height);

            if (polyIsEmpty)
                return;

            // draw lines
            Pen linePen = new Pen(Color.LightPink, 0.3f);
            poly.lines.ToList().ForEach(
                line => g.DrawLine(
                    linePen,
                    (float)line.x1,
                    (float)line.y1,
                    (float)line.x2,
                    (float)line.y2
                )
            );

            // draw points
            const float pointSize = 0.8f;
            poly.points.ToList().ForEach(
                point => g.FillEllipse(
                    Brushes.DarkRed,
                    (float)point.x - pointSize / 2,
                    (float)point.y - pointSize / 2,
                    pointSize,
                    pointSize
                )
            );

            // draw normals
            Pen normalPen = new Pen(Color.DarkViolet, 0.3f);
            normalPen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
            poly.normals.ToList().ForEach(
                normal =>
                {
                    BallsModel.Point vStart = normal.getStart;
                    BallsModel.Point vEnd = normal.getEnd;

                    g.DrawLine(
                        normalPen,
                        (float)normal.x,
                        (float)normal.y,
                        (float)normal.ex,
                        (float)normal.ey
                    );

                    g.FillEllipse(
                        Brushes.DarkSeaGreen,
                        (float)vStart.x - pointSize / 2,
                        (float)vStart.y - pointSize / 2,
                        pointSize,
                        pointSize
                    );
                }
            );
        }
    }
}
