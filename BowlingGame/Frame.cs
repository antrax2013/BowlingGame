namespace BowlingGame;

public sealed class Frame(int numberOfRollsToCompleted = 2, int maxRollResult=10)
{
    private int numberOfRollsToCompleted = numberOfRollsToCompleted;
    private readonly int maxRollResult = maxRollResult;
    private int CurrentScore { get { return values.Sum(); } }

    private int nbRolls = 0;
    private readonly List<int> values = [];

    public Boolean IsStrike { get; private set; } = false;
    public Boolean IsSpare { get; private set; } = false;
    public Boolean IsCompleted { get; private set; } = false;

    public int? Score { get => IsCompleted ? CurrentScore : null; }

    public void SaveRollResult(int result)
    {
        if (IsCompleted)
            throw new InvalidOperationException("Frame is full.");

        values.Add(result);
        nbRolls++;

        if (CurrentScore == maxRollResult) {

            numberOfRollsToCompleted++;
            if (nbRolls == 1)
                IsStrike = true;
            else
                IsSpare = true;
            return;
        }
      
        if (nbRolls == numberOfRollsToCompleted)
            IsCompleted = true;
    }

    public string DisplayScore()
    {
        if (!IsCompleted)
            return string.Empty;

        if (IsStrike)
            return "X";

        if (IsSpare)
            return values[0]+"/";

        return (values[0].ToString()+values[1].ToString()).Replace("0","-");
    }
}
