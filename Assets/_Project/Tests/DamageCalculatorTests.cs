using NUnit.Framework;
using UnityEngine;
using BiomeWar;

public class DamageCalculatorTests
{
    [Test]
    public void NoResistance_ReturnsFullDamage()
    {
        float result = DamageCalculator.Calculate(50f, 0f, 0f, 10f, 30f);
        Assert.AreEqual(50f, result, 0.01f);
    }

    [Test]
    public void FullResistance_ReturnsZeroDamage()
    {
        float result = DamageCalculator.Calculate(50f, 1f, 0f, 10f, 30f);
        Assert.AreEqual(0f, result, 0.01f);
    }

    [Test]
    public void HalfResistance_HalvesDamage()
    {
        float result = DamageCalculator.Calculate(100f, 0.5f, 0f, 10f, 30f);
        Assert.AreEqual(50f, result, 0.01f);
    }

    [Test]
    public void DamageNeverGoesNegative()
    {
        float result = DamageCalculator.Calculate(-20f, 0f, 0f, 10f, 30f);
        Assert.GreaterOrEqual(result, 0f);
    }

    [Test]
    public void WithinFalloffStart_NoReduction()
    {
        float f = DamageCalculator.CalculateFalloff(5f, 10f, 30f, 0.25f);
        Assert.AreEqual(1f, f, 0.01f);
    }

    [Test]
    public void BeyondFalloffEnd_ReturnsMinimumMultiplier()
    {
        float f = DamageCalculator.CalculateFalloff(50f, 10f, 30f, 0.25f);
        Assert.AreEqual(0.25f, f, 0.01f);
    }

    [Test]
    public void FrontalHit_AppliesResistance()
    {
        float r = DamageCalculator.DirectionalResistance(
            Vector3.forward, Vector3.back, 90f, 0.8f);
        Assert.AreEqual(0.8f, r, 0.01f);
    }

    [Test]
    public void RearHit_AppliesNoResistance()
    {
        float r = DamageCalculator.DirectionalResistance(
            Vector3.forward, Vector3.forward, 90f, 0.8f);
        Assert.AreEqual(0f, r, 0.01f);
    }
}
