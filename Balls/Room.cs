using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Balls
{
    public class Room
    {
        public State currentState, nextState;

        public Room(State state)
        {
            currentState = state;
            nextState = currentState.calcNextState();
        } 

        public void render(Graphics g, long nowTime)
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            double secs = (double)nowTime / 1000;
            State actualState = getActualState(secs);

            actualState.render(g, secs);
            nextState.render(g, secs, Color.Red, true);
        }

        public void updateStates(double nowTime)
        {
            if (nowTime > nextState.startTime)
            {
                currentState = nextState;
                nextState = currentState.calcNextState();                
            }
        }

        public State getActualState(double nowTime)
        {
            updateStates(nowTime);

            return currentState;
        }
    }
}
