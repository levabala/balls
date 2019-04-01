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
        public bool supportGeometry = false;
        public bool drawNextState = false;

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
            State newState = statesStack.Last().calcNextState();
            statesStack.Add(newState);

            return newState;
        }

        public State takeActualState(double time)
        {
            int index = statesStack.FindIndex((state) => state.startTime > time);

            if (index == -1)
            {
                State state = statesStack.Last().calcNextState();
                while (state.startTime < time)
                    state = state.calcNextState();

                statesStack.Clear();
                statesStack.Add(state);
                return state;
            }

            State actualState = statesStack[index];
            statesStack = statesStack.Skip(index).ToList();

            return actualState;
        }
    }
}
