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
            
            Random rnd = new Random();

            room = new Room(
                new State(
                    new List<Ball>(
                    new Ball[13].Select(
                        b => new Ball(
                            new Vector(
                                50, 50,
                                rnd.Next(-50, 50),
                                rnd.Next(-50, 50)
                            ),
                            rnd.Next(10, 30)
                        )
                    )
                    ),
                    //)
                    //{
                    //    new Ball(
                    //        new Vector(
                    //            100, 200,
                    //            150,
                    //            150
                    //        ),
                    //        15
                    //    )
                    //},

                    new List<Wall>()
                    {
                        new Wall(0, 0, 700, 0),
                        new Wall(700, 0, 700, 500),
                        new Wall(700, 500, 0, 500),
                        new Wall(0, 500, 0, 0),

                        new Wall(100, 180, 400, 350),
                        new Wall(100, 650, 610, 460),
                        new Wall(310, 10, 510, 410),
                        new Wall(110, 110, 110, 410),
                        new Wall(210, 110, 100, 410),
                    }, 
                    stopWatch.ElapsedMilliseconds,
                    0,
                    true
                )
            );

            room.calcMultipleStates(100);

            long lastTime = stopWatch.ElapsedMilliseconds;

            Timer t = new Timer();
            t.Interval = 4;
            t.Tick += (s, a) =>
            {

                panelDraw.Invalidate();

                long currentTime = stopWatch.ElapsedMilliseconds;
                long delta = currentTime - lastTime;

                lastTime = currentTime;
            };

            t.Start();
            stopWatch.Start();



            Timer t2 = new Timer();
            t2.Interval = 300;
            t2.Tick += (s, a) =>
            {

                Text = room.statesStack.Count.ToString();
            };

            t2.Start();
        }

        private void PanelDraw_Paint(object sender, PaintEventArgs e)
        {
            room.render(e.Graphics, stopWatch.ElapsedMilliseconds);
        }
    }
}
