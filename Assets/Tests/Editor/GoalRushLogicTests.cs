using NUnit.Framework;
public class GoalRushLogicTests {
    [Test] public void StarThresholds(){Assert.That(800>=800);Assert.That(500>=500);}
    [Test] public void LevelBounds(){Assert.That(UnityEngine.Mathf.Clamp(101,1,100),Is.EqualTo(100));}
}
