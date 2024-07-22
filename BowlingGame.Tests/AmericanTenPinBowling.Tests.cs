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
    public void When_A_New_Games_Starts_Then_The_Score_Is_Null() {
        Check.That(game.Score).IsNull();
        Check.That(game.DisplayScore()).IsEqualTo(string.Empty);
    }

    [Test]
    public void When_The_Game_Is_Completed_After_12_Strikes_The_Score_Should_Be_300_And_Display_Is_The_Expected_Display()
    {
        var expectedResult = 300;
        var expectedDisplay = "X X X X X X X X X X X X";
        rollResult.GenerateResult().Returns(10);

        game.Play();

        Check.That(game.Score).IsEqualTo(expectedResult);
        Check.That(game.DisplayScore()).IsEqualTo(expectedDisplay);
    }

    [Test]
    public void When_The_Game_Is_Completed_With_9_points_rolls_and_9_failed_The_Score_Should_Be_90_And_Display_Is_The_Expected_Display()
    {
        var expectedResult = 90;
        var expectedDisplay = "9- 9- 9- 9- 9- 9- 9- 9- 9- 9-";
        var i = 0;
        rollResult.GenerateResult().Returns(x =>
                {
            var res = i % 2 == 0 ? 9 : 0;
            i++;
            return res;
        });

        game.Play();

        Check.That(game.Score).IsEqualTo(expectedResult);
        Check.That(game.DisplayScore()).IsEqualTo(expectedDisplay);
    }

    [Test]
    public void When_The_Game_Is_Completed_By_10_Spares_and_5_points_spare_reroll_The_Score_Should_Be_150_And_Display_Is_The_Expected_Display()
    {
        var expectedResult = 150;
        rollResult.GenerateResult().Returns(5);
        var expectedDisplay = "5/ 5/ 5/ 5/ 5/ 5/ 5/ 5/ 5/ 5/5";

        game.Play();

        Check.That(game.Score).IsEqualTo(expectedResult);
        Check.That(game.DisplayScore()).IsEqualTo(expectedDisplay);
    }
}
