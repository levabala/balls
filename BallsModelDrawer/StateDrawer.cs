using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BallsModelDrawer
{
    public static class StateDrawer
    {
        public static void Draw(
            Graphics g, 
            BallsModel.State state,
            double time,
            float scale = 1, 
            float scaleVectors = 1,
            bool showCenter = true,
            bool showFrame = false,
            bool showVector = true
        )
        {
            Pen wallPen = new Pen(Color.Black, 3f);
            foreach (BallsModel.Wall wall in state.walls)                            
            {
                PointF[] points =
                    wall.frame.points
                    .Select(p => new PointF((float)p.x * scale, (float)p.y * scale))
                    .ToArray();

                HatchBrush hBrush = new HatchBrush(
                   HatchStyle.ForwardDiagonal,
                   Color.White,
                   Color.Gray
                );

                g.FillPolygon(
                    hBrush,
                    points
                );

                g.DrawPolygon(
                    wallPen,
                    points
                );
            }



            Pen ballPen = new Pen(Color.DarkRed, 2f);
            Brush ballCenterBrush = Brushes.Red;
            Pen ballVectorPen = new Pen(Color.DarkGreen, 3f);
            ballVectorPen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;

            double timeDelta = time - state.timeStamp;            
            foreach (BallsModel.Ball ball in state.balls)
            {
                double currentX = ball.frame.x + ball.ph.speed.dx * timeDelta;
                double currentY = ball.frame.y + ball.ph.speed.dy * timeDelta;

                float r = (float)ball.frame.radius * scale * 2;
                float x = (float)(currentX * scale);
                float y = (float)(currentY * scale);

                if (showCenter)
                {
                    float cr = scale / 2;
                    g.DrawRectangle(ballPen, x - cr / 2, y - cr / 2, cr, cr);
                }

                if (showFrame)
                {
                    // draw circle
                    g.DrawEllipse(ballPen, x - r / 2, y - r / 2, r, r);

                }

                if (showVector)
                {
                    // draw speed vector
                    float dx = (float)ball.ph.speed.dx * scaleVectors;
                    float dy = (float)ball.ph.speed.dy * scaleVectors;
                    g.DrawLine(ballVectorPen, x, y, x + dx, y + dy);

                    // draw center circle
                    g.FillEllipse(
                        ballCenterBrush,
                        x - 5,
                        y - 5,
                        10,
                        10
                    );
                }                    
            }

        }
    }
}

