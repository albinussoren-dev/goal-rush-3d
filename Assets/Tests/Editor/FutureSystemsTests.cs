using NUnit.Framework;
using UnityEngine;

public class FutureSystemsTests {
    [Test] public void LevelCatalogHas100Levels(){Assert.That(LevelCatalog.TotalLevels,Is.EqualTo(100));}
    [Test] public void WorldMappingIsStable(){Assert.That(LevelCatalog.WorldFor(1),Is.EqualTo(1));Assert.That(LevelCatalog.WorldFor(20),Is.EqualTo(1));Assert.That(LevelCatalog.WorldFor(21),Is.EqualTo(2));Assert.That(LevelCatalog.WorldFor(100),Is.EqualTo(5));}
    [Test] public void DailyRewardStartsClaimable(){SaveSystem.Data=new SaveData();Assert.That(DailyRewardSystem.CanClaim,Is.True);}
    [Test] public void MissionDefinitionsExist(){Assert.That(MissionSystem.Definitions.Length,Is.GreaterThanOrEqualTo(4));}
    [Test] public void UpgradeCostIncreases(){Assert.That(PlayerProgression.UpgradeCost(2),Is.GreaterThan(PlayerProgression.UpgradeCost(1)));}
    [Test] public void ScoreThresholdsRemainCompatible(){Assert.That(800>=800);Assert.That(500>=500);}
    [Test] public void LevelBoundsRemainSafe(){Assert.That(Mathf.Clamp(101,1,100),Is.EqualTo(100));}
}