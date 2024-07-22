using NFluent;

namespace BowlingGame.Tests;

internal class FrameTests
{
    [Test]
    public void When_2_Rolls_Frame_Is_Initialized_Then_The_Frame_Score_Should_Be_Null_And_States_Are_Null()
    {
        var frame = new Frame();

        Check.That(frame.Score.HasValue).IsFalse();
        Check.That(frame.IsCompleted).IsFalse();
        Check.That(frame.IsStrike).IsFalse();
        Check.That(frame.IsSpare).IsFalse();
    }

    [Test]
    public void When_A_1_Roll_Frame_Is_Complted_Then_The_State_Of_Frame_Is_Completed()
    {
        var frame = new Frame(1);
        var aValue = 1;

        frame.SaveRollResult(aValue);

        Check.That(frame.IsCompleted).IsTrue();
        Check.That(frame.IsStrike).IsFalse();
        Check.That(frame.IsSpare).IsFalse();
    }

    [Test]
    public void When_A_1_Roll_Frame_Is_Completed_And_New_Score_Is_Saved_Then_An_InvalidOperationException_Should_Fire()
    {
        var frame = new Frame(1);
        var aValue = 1;

        frame.SaveRollResult(aValue);

        Check.ThatCode(() => frame.SaveRollResult(aValue))
             .Throws<InvalidOperationException>();
    }    

    [Test]
    public void When_2_Rolls_Frame_contains_2_rolls_results_Then_The_Frame_Score_Should_Be_The_Expected_Score()
    {
        var frame = new Frame();
        var score1 = 1;
        var score2 = 2;
        var expectedScore = score1 + score2;

        frame.SaveRollResult(score1);
        frame.SaveRollResult(score2);

        Check.That(frame.Score).IsEqualTo(expectedScore);
    }

    [Test]
    public void When_A_Spare_Is_Saved_In_2_Rolls_Frame_Then_The_Frame_State_Should_Be_Spare_And_The_Score_Should_Be_Null()
    {
        var frame = new Frame();
        var score1 = 5;
        var score2 = 5;

        frame.SaveRollResult(score1);
        frame.SaveRollResult(score2);

        Check.That(frame.Score.HasValue).IsFalse();
        Check.That(frame.IsCompleted).IsFalse();
        Check.That(frame.IsStrike).IsFalse();
        Check.That(frame.IsSpare).IsTrue();
    }

    [Test]
    public void When_A_Strike_Is_Saved_In_2_Rolls_Frame_Then_The_Frame_State_Should_Be_Strike_And_The_Score_Should_Be_Null()
    {
        var frame = new Frame();
        var score1 = 10;

        frame.SaveRollResult(score1);

        Check.That(frame.Score.HasValue).IsFalse();
        Check.That(frame.IsCompleted).IsFalse();
        Check.That(frame.IsStrike).IsTrue();
        Check.That(frame.IsSpare).IsFalse();
    }

}
