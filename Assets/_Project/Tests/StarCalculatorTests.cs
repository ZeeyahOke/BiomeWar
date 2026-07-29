using NUnit.Framework;
using BiomeWar;

public class StarCalculatorTests
{
    [Test]
    public void NoCollectablesFound_ReturnsZeroStars()
    {
        Assert.AreEqual(0, StarCalculator.Calculate(0, 3));
    }

    [Test]
    public void AllCollectablesFound_ReturnsThreeStars()
    {
        Assert.AreEqual(3, StarCalculator.Calculate(3, 3));
    }

    [Test]
    public void OneOfThreeFound_ReturnsOneStar()
    {
        Assert.AreEqual(1, StarCalculator.Calculate(1, 3));
    }

    [Test]
    public void TwoOfThreeFound_ReturnsTwoStars()
    {
        Assert.AreEqual(2, StarCalculator.Calculate(2, 3));
    }

    [Test]
    public void ZeroTotal_ReturnsZeroStars()
    {
        Assert.AreEqual(0, StarCalculator.Calculate(0, 0));
    }

    [Test]
    public void MoreFoundThanTotal_CapsAtThreeStars()
    {
        Assert.AreEqual(3, StarCalculator.Calculate(5, 3));
    }
}
