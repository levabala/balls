using Microsoft.FSharp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BallsModelDrawer
{
    public class Room
    {
        BallsModel.State currentState;
        List<BallsModel.State> statesBuffer = new List<BallsModel.State>();
        public bool solved = false;
       
        public Room(BallsModel.State initialState)
        {
            currentState = initialState;
        }

        public BallsModel.State calcNextState()
        {            
            BallsModel.State latestState = 
                statesBuffer.Count > 0 ? statesBuffer.Last() : currentState;

            FSharpOption <BallsModel.State> state = latestState.nextState;
            solved = FSharpOption<BallsModel.State>.get_IsNone(state);

            if (!solved)
            {
                statesBuffer.Add(state.Value);
                return state.Value;
            }
            else
                return latestState;
        }

        public BallsModel.State getActualState(double time)
        {
            bool currentStateIsOutdated()
            {
                if (statesBuffer.Count == 0)
                    calcNextState();

                if (solved)
                    return false;

                double currentEndTime = statesBuffer.First().timeStamp;

                return time > currentEndTime;
            }            

            bool currentStateOutdated = currentStateIsOutdated();

            if (solved)
                return currentState;

            if (!currentStateOutdated)
                return currentState;

            do
            {
                if (statesBuffer.Count == 0)
                    currentState = calcNextState();
                else
                {
                    currentState = statesBuffer[0];
                    statesBuffer = statesBuffer.Skip(1).ToList();
                }
                    
            } while (!solved && currentStateIsOutdated());

            return currentState;
        }

        public void addWall(BallsModel.Wall wall)
        {
            BallsModel.State newState = statesBuffer.Last().addWall(wall);
            statesBuffer.Add(newState);
        }
    }
}
