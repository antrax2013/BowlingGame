namespace BowlingGame;
public interface IBowling
{
    void Play();

    int? Score { get; }

    string DisplayScore();
}