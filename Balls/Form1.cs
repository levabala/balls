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

namespace Balls
{
    public partial class Form1 : Form
    {
        Stopwatch stopWatch = new Stopwatch();
        Room room;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            panelDraw.Paint += PanelDraw_Paint;
            
            long lastTime = stopWatch.ElapsedMilliseconds;

            Timer t = new Timer();
            t.Interval = 16;
            t.Tick += (s, a) =>
            {
                
                panelDraw.Invalidate();

                long currentTime = stopWatch.ElapsedMilliseconds;
                long delta = currentTime - lastTime;                

                lastTime = currentTime;
            };

            t.Start();
            stopWatch.Start();

            room = new Room(
                new State(
                    new List<Ball>()
                    {
                        new Ball(new Vector(50, 50, 0, 210), 15),
                        new Ball(new Vector(50, 50, 0, 220), 15),
                        new Ball(new Vector(50, 50, 0, 230), 15),
                        new Ball(new Vector(50, 50, 0, 240), 15),
                        new Ball(new Vector(50, 50, 100, 400), 15),
                        new Ball(new Vector(50, 150, 400, 100), 15),
                        new Ball(new Vector(450, 150, -400, 200), 15),
                        new Ball(new Vector(250, 350, -200, -100), 15),
                        new Ball(new Vector(250, 350, 100, -400), 15),
                        new Ball(new Vector(50, 50, 200, 1), 15),
                        new Ball(new Vector(50, 50, 100, 200), 5),
                        new Ball(new Vector(40, 30, 100, 70), 35),
                    },
                    new List<Wall>()
                    {
                        new Wall(0, 0, 700, 0),
                        new Wall(700, 0, 700, 500),
                        new Wall(700, 500, 0, 500),
                        new Wall(0, 500, 0, 0),

                        //new Wall(100, 180, 400, 350),
                        //new Wall(100, 450, 500, 460),
                        //new Wall(310, 10, 510, 410),
                    }, 
                    stopWatch.ElapsedMilliseconds,
                    true
                )
            );
        }

        private void PanelDraw_Paint(object sender, PaintEventArgs e)
        {
            room.render(e.Graphics, stopWatch.ElapsedMilliseconds);
        }
    }
}
