using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Balls
{
    public class Room
    {
        public int statesPast = 0;
        public List<State> statesStack = new List<State>();
        public bool supportGeometry = true;
        public bool drawNextState = true;
        public double solveTime;

        public Room(State state)
        {
            statesStack.Add(state);
        } 

        public void render(Graphics g, long nowTime)
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            double secs = (double)nowTime / 1000;
            State actualState = takeActualState(secs);

            actualState.render(g, secs, supportGeometry);

            if (drawNextState && statesStack[1] != null)
                statesStack[1].render(g, secs, supportGeometry, Color.Red, true);
        }

        public void calcMultipleStates(int count)
        {
            while (count-- > 0)
                calcNextState();
        }

        public State calcNextState()
        {
            State lastState = statesStack.Last();
            State newState = lastState.calcNextState();

            statesStack.Add(newState);

            if (lastState.id == newState.id)
                solveTime = lastState.startTime;

            return newState;
        }

        public State takeActualState(double time)
        {
            if (time >= solveTime)
                return statesStack.Last();

            int index = statesStack.FindIndex((state) => state.startTime > time) - 1;

            if (index < 0)
            {
                calcMultipleStates(50);
                return takeActualState(time);
            }

            State actualState = statesStack[index];
            statesStack = statesStack.Skip(index).ToList();           

            return actualState;
        }
    }
}
