namespace BowlingGame;

public sealed class AmericanTenPinBowling(IRollResult rollResult, List<Frame> frames) : IBowling
{
#pragma warning disable CS9124 // Le paramètre est capturé dans l'état du type englobant et sa valeur est également utilisée pour initialiser un champ, une propriété ou un événement.
#pragma warning disable IDE0052 // Supprimer les membres privés non lus
    private readonly IRollResult _rollResult = rollResult;
#pragma warning restore IDE0052 // Supprimer les membres privés non lus
#pragma warning restore CS9124 // Le paramètre est capturé dans l'état du type englobant et sa valeur est également utilisée pour initialiser un champ, une propriété ou un événement.
    private readonly List<Frame> _frames = frames;

    public int? Score {
        get {
            var completedFrame = _frames.Where(f => f.State == FrameState.Completed).ToList();

            return completedFrame.Count > 0 ? completedFrame.Select(f => f.Score).Sum() : null;
        }
    }

    public void Play()
    {
        var endOfGame = false;
        List<Frame> unresolvedFrames = [];
        for (int i = 0; i < _frames.Count; i++)
        {
            var frame = _frames[i];

            var result = rollResult.GenerateResult();
            frame.SaveRollResult(result);

            unresolvedFrames = ManageNotSolvedFrames(unresolvedFrames, result, endOfGame);

            if (frame.State == FrameState.Strike)
            {
                unresolvedFrames.Add(frame);
                continue;
            }

            result = rollResult.GenerateResult();
            frame.SaveRollResult(result);

            if (frame.State == FrameState.Spare)
                unresolvedFrames.Add(frame);
        }

        endOfGame = true;
        while (unresolvedFrames.Count > 0)
            unresolvedFrames = ManageNotSolvedFrames(unresolvedFrames, rollResult.GenerateResult(), endOfGame);
    }

    private static List<Frame> ManageNotSolvedFrames(List<Frame> unresolvedFrames, int result, bool endOfGame) {
        List<Frame> uncompletedFrame = [];
        var isLastRoll = endOfGame && unresolvedFrames.Count == 1;

        foreach (var unresolved in unresolvedFrames)
        {
            unresolved.SaveRollResult(result);

            if (unresolved.State != FrameState.Completed)
            {
                if (isLastRoll)
                    while(unresolved.State != FrameState.Completed)
                        unresolved.SaveRollResult(0);
                else
                    uncompletedFrame.Add(unresolved);
            }
                
        }

        return uncompletedFrame;
    }
}
