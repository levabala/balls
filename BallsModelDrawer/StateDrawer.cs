using System;
using System.Collections.Generic;
using System.Drawing;
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
            float scaleVectors = 1
        )
        {
            Pen wallPen = new Pen(Color.Black, 3f);
            foreach (BallsModel.Wall wall in state.walls)            
                foreach (BallsModel.Line line in wall.frame.lines)
                {
                    g.DrawLine(
                        wallPen, 
                        (float)line.x1 * scale,
                        (float)line.y1 * scale,
                        (float)line.x2 * scale,
                        (float)line.y2 * scale
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

                // draw circle
                g.DrawEllipse(ballPen, x - r / 2, y - r / 2, r, r);                

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

