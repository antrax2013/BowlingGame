
using System;

namespace BowlingGame;


public enum FrameState {
    Running,
    Strike,
    Spare,
    Completed,
}


public sealed class Frame(int numberOfRollsToCompleted = 2, int maxRollResult=10)
{
    private int numberOfRollsToCompleted = numberOfRollsToCompleted;
    private readonly int maxRollResult = maxRollResult;
    private int CurrentScore { get { return values.Sum(); } }

    private int nbRolls = 0;
    private readonly List<int> values = [];

    public FrameState State { get; private set; } = FrameState.Running;

    private Boolean IsStrike { get; set; } = false;
    private Boolean IsSpare { get; set; } = false;

    public int? Score { get => State==FrameState.Completed? CurrentScore : null; }

    public void SaveRollResult(int result)
    {
        if (State == FrameState.Completed)
            throw new InvalidOperationException("Frame is full.");

        values.Add(result);
        nbRolls++;

        if (CurrentScore == maxRollResult) {

            numberOfRollsToCompleted++;
            if (nbRolls == 1) {
                State = FrameState.Strike;
                IsStrike = true;
            }
            else {
                State = FrameState.Spare;
                IsSpare = true;
            }
            return;
        }

      
        if (nbRolls == numberOfRollsToCompleted) {
            State = FrameState.Completed;
        }
    }

    public override string ToString()
    {
        if (IsStrike) {
            return "X";
        }

        if (IsSpare)
        {
            return values[0]+"/";
        }

        return (values[0].ToString()+values[1].ToString()).Replace("0","-");
    }
}
