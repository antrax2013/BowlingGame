namespace BowlingGame;

public sealed class AmericanTenPinBowling(IRollResult rollResult, List<Frame> frames) : IBowling
{
    private readonly IRollResult rollResult = rollResult;
    private readonly List<Frame> frames = frames;

    public int? Score {
        get {
            var completedFrame = frames.Where(f => f.IsCompleted).ToList();

            return completedFrame.Count > 0 ? completedFrame.Select(f => f.Score).Sum() : null;
        }
    }

    public void Play()
    {
        List<Frame> unresolvedFrames = [];
        for (int i = 0; i < frames.Count; i++)
        {
            var frame = frames[i];

            var result = rollResult.GenerateResult();
            frame.SaveRollResult(result);

            unresolvedFrames = ManageNotSolvedFrames(unresolvedFrames, result);

            if (frame.IsStrike)
            {
                unresolvedFrames.Add(frame);
                continue;
            }

            result = rollResult.GenerateResult();
            frame.SaveRollResult(result);

            if (frame.IsSpare)
                unresolvedFrames.Add(frame);
        }

        while (unresolvedFrames.Count > 0)
            unresolvedFrames = ManageNotSolvedFrames(unresolvedFrames, rollResult.GenerateResult());
    }

    private static List<Frame> ManageNotSolvedFrames(List<Frame> unresolvedFrames, int result) {
        List<Frame> uncompletedFrame = [];

        foreach (var unresolved in unresolvedFrames)
        {
            unresolved.SaveRollResult(result);

            if (!unresolved.IsCompleted)
                    uncompletedFrame.Add(unresolved);
                
        }

        return uncompletedFrame;
    }

    public string DisplayScore() {
        if(frames.Exists(f => !f.IsCompleted))
            return string.Empty;

        var famresWithoutLastFrame = frames.SkipLast(1);
        var startOfDisplay = string.Join(" ", famresWithoutLastFrame.Select(f => f.DisplayScore()).ToList());
        
        var lastFrame = frames.Last();
        var lastRollResult = lastFrame.Score switch {
            30 => "X X X",
            > 10 => lastFrame.DisplayScore()+(lastFrame.Score-10).ToString(),
            _ =>  lastFrame.DisplayScore()
        };

        return startOfDisplay +" "+ lastRollResult;
    }
}
