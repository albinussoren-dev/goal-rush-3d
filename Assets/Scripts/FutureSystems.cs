using System;
using UnityEngine;

[Serializable] public class PlayerUpgradeData {
    public int speedLevel=1, jumpLevel=1, shieldLevel=1, magnetLevel=1;
    public int selectedCharacter=0;
    public bool[] unlockedCharacters=new bool[5]{true,false,false,false,false};
}

[Serializable] public class DailyRewardData {
    public string lastClaimUtc="";
    public int streak=0;
}

[Serializable] public class MissionData {
    public int totalCoins=0, totalGates=0, totalLevels=0, bestScore=0;
    public int claimedMask=0;
}

public static class PlayerProgression {
    public static PlayerUpgradeData Data => SaveSystem.Data.upgrades;
    public static bool SpendMoney(int amount) {
        if(amount<0 || SaveSystem.Data.money<amount) return false;
        SaveSystem.Data.money-=amount; SaveSystem.Save(); return true;
    }
    public static int UpgradeCost(int currentLevel) => 100 + currentLevel*75;
    public static bool UpgradeSpeed() => Upgrade(ref Data.speedLevel);
    public static bool UpgradeJump() => Upgrade(ref Data.jumpLevel);
    public static bool UpgradeShield() => Upgrade(ref Data.shieldLevel);
    public static bool UpgradeMagnet() => Upgrade(ref Data.magnetLevel);
    static bool Upgrade(ref int level) {
        int cost=UpgradeCost(level);
        if(!SpendMoney(cost)) return false;
        level=Mathf.Clamp(level+1,1,10); SaveSystem.Save(); return true;
    }
    public static bool UnlockCharacter(int index) {
        if(index<0 || index>=Data.unlockedCharacters.Length || Data.unlockedCharacters[index]) return false;
        int cost=500+index*500;
        if(!SpendMoney(cost)) return false;
        Data.unlockedCharacters[index]=true; SaveSystem.Save(); return true;
    }
    public static bool SelectCharacter(int index) {
        if(index<0 || index>=Data.unlockedCharacters.Length || !Data.unlockedCharacters[index]) return false;
        Data.selectedCharacter=index; SaveSystem.Save(); return true;
    }
}

public static class DailyRewardSystem {
    public static bool CanClaim {
        get {
            if(string.IsNullOrEmpty(SaveSystem.Data.daily.lastClaimUtc)) return true;
            if(!DateTime.TryParse(SaveSystem.Data.daily.lastClaimUtc,null,System.Globalization.DateTimeStyles.RoundtripKind,out var last)) return true;
            return DateTime.UtcNow.Date>last.ToUniversalTime().Date;
        }
    }
    public static int CurrentReward => 25 + Mathf.Min(SaveSystem.Data.daily.streak,6)*25;
    public static bool Claim(out int reward) {
        reward=0; if(!CanClaim) return false;
        var d=SaveSystem.Data.daily;
        if(DateTime.TryParse(d.lastClaimUtc,null,System.Globalization.DateTimeStyles.RoundtripKind,out var last)) {
            int days=(DateTime.UtcNow.Date-last.ToUniversalTime().Date).Days;
            d.streak=days==1 ? (d.streak+1)%7 : 0;
        } else d.streak=0;
        reward=CurrentReward; SaveSystem.Data.money+=reward; d.lastClaimUtc=DateTime.UtcNow.ToString("O");
        SaveSystem.Save(); return true;
    }
}

public enum MissionType { CollectCoins, FinishLevels, PassGates, ReachScore }

[Serializable] public class MissionDefinition {
    public MissionType type; public int target; public int rewardMoney;
    public MissionDefinition(MissionType t,int target,int reward){type=t;this.target=target;rewardMoney=reward;}
}

public static class MissionSystem {
    public static readonly MissionDefinition[] Definitions = {
        new MissionDefinition(MissionType.CollectCoins,50,150),
        new MissionDefinition(MissionType.FinishLevels,5,250),
        new MissionDefinition(MissionType.PassGates,20,200),
        new MissionDefinition(MissionType.ReachScore,1000,300)
    };
    public static int Progress(MissionType type) {
        var d=SaveSystem.Data.missions;
        return type==MissionType.CollectCoins?d.totalCoins:type==MissionType.FinishLevels?d.totalLevels:type==MissionType.PassGates?d.totalGates:d.bestScore;
    }
    public static bool IsClaimed(int index)=>index>=0&&index<32&&(SaveSystem.Data.missions.claimedMask&(1<<index))!=0;
    public static bool Claim(int index) {
        if(index<0||index>=Definitions.Length||IsClaimed(index)) return false;
        var m=Definitions[index]; if(Progress(m.type)<m.target) return false;
        SaveSystem.Data.missions.claimedMask|=1<<index; SaveSystem.Data.money+=m.rewardMoney; SaveSystem.Save(); return true;
    }
}

public static class ShopSystem {
    public static bool BuyShield(int price=150) {
        if(!PlayerProgression.SpendMoney(price)) return false;
        SaveSystem.Data.shieldCharges=Mathf.Clamp(SaveSystem.Data.shieldCharges+1,0,99); SaveSystem.Save(); return true;
    }
    public static bool BuyMagnet(int price=120) {
        if(!PlayerProgression.SpendMoney(price)) return false;
        SaveSystem.Data.magnetCharges=Mathf.Clamp(SaveSystem.Data.magnetCharges+1,0,99); SaveSystem.Save(); return true;
    }
}
