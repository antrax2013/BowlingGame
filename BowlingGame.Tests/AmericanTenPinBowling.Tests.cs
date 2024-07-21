using NFluent;
using NSubstitute;

namespace BowlingGame.Tests;

internal class AmericanTenPinBowlingTests
{
    private readonly IRollResult rollResult = Substitute.For<IRollResult>();
    private AmericanTenPinBowling game;

    [SetUp]
    public void Setup(){
        List<Frame> frames = [];
        for (int i = 0; i < 10; i++) {
            frames.Add(new Frame());
        }

        game = new AmericanTenPinBowling(rollResult, frames);
    }

    [Test]
    public void When_A_New_Games_Is_Set_Then_The_Number_Of_Pins_Is_Ten_And_The_Score_Is_Null() {
        Check.That(game.Score).IsNull();
    }

    [Test]
    public void When_The_Game_Is_Completed_After_12_Strikes_The_Score_Should_Be_300() {
        var expectedResult = 300;
        rollResult.GenerateResult().Returns(10);

        game.Play();

        Check.That(game.Score).IsEqualTo(expectedResult);
    }

    [Test]
    public void When_The_Game_Is_Completed_With_9_points_rolls_and_9_failed_The_Score_Should_Be_90()
    {
        var expectedResult = 90;
        var i = 0;
        rollResult.GenerateResult().Returns(x =>
                {
            var res = i % 2 == 0 ? 9 : 0;
            i++;
            return res;
        });

        game.Play();

        Check.That(game.Score).IsEqualTo(expectedResult);
    }

    [Test]
    public void When_The_Game_Is_Completed_By_10_Spares_and_5_points_spare_reroll_The_Score_Should_Be_150()
    {
        var expectedResult = 150;
        rollResult.GenerateResult().Returns(5);

        game.Play();

        Check.That(game.Score).IsEqualTo(expectedResult);
    }
}
