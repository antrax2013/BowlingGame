
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
    private int currentScore = 0;
    private int nbRolls = 0;

    public FrameState State { get; private set; } = FrameState.Running;

    public int? Score { get => State==FrameState.Completed? currentScore : null; }

    public void SaveRollResult(int result)
    {
        if (State == FrameState.Completed)
            throw new InvalidOperationException("Frame is full.");

        currentScore += result;
        nbRolls++;

        if (currentScore == maxRollResult) {

            numberOfRollsToCompleted++;
            State = nbRolls == 1 ? FrameState.Strike : FrameState.Spare;
            return;
        }

      
        if (nbRolls == numberOfRollsToCompleted) {
            State = FrameState.Completed;
        }
    }
}
