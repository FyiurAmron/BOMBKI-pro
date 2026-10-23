
namespace Bombki.Tests;

public class UnitsCombatTests {
    readonly Game game = new();
    void WalkaSetup(int maxE, int wrogE, int sil, int wrogSil,
        int zre, int wrogZre, int par, int kop) {
        UnitsSeed.Reseed(game);
        game.Player.Energy = 100;
        game.Skills.Values[TSkill.Parry] = par;
        game.Skills.Values[TSkill.Kick] = kop;
        game.Skills.Values[TSkill.KickMinMana] = 0;
        game.Skills.Values[TSkill.KickMaxEnergy] = 0;
        game.Player.Mana = 0;
        game.Combat.FireballCharges = 0;
        game.Combat.PoisonCharges = 0;
        game.Combat.PoisonRounds = 0;
        game.Skills.Values[TSkill.Fleeing] = 0;
        game.Skills.Values[TSkill.FleeEnergy] = 0;
        game.Combat.Fled = false;
        game.Player.Experience = 0;
        game.Gear.Values[TGearItem.LuckRoll] = 0;
        game.Gear.Values[TGearItem.Armor] = 0;
        game.Gear.Values[TGearItem.WornCount] = 0;
        game.Player.MaxEnergy = maxE;
        game.Combat.EnemyEnergy = wrogE;
        game.Player.Stats[TStat.Strength] = sil;
        game.Combat.EnemyStrength = wrogSil;
        game.Player.Stats[TStat.Dexterity] = zre;
        game.Combat.EnemyDexterity = wrogZre;
    }

    void RunWalka(int maxE, int wrogE, int sil, int wrogSil,
        int zre, int wrogZre, int par, int kop, int expectedKunszt) {
        WalkaSetup(maxE, wrogE, sil, wrogSil, zre, wrogZre, par, kop);
        game.DoFight();
        Assert.Equal(expectedKunszt, game.Player.Experience);
    }

    void RunWalkaRange(int maxE, int wrogE, int sil, int wrogSil,
        int zre, int wrogZre, int par, int kop, int kunsztLo, int kunsztHi) {
        WalkaSetup(maxE, wrogE, sil, wrogSil, zre, wrogZre, par, kop);
        game.DoFight();
        Assert.InRange(game.Player.Experience, kunsztLo, kunsztHi);
    }

    void RunWalkaShielded(int maxE, int wrogE, int sil, int wrogSil,
        int zre, int wrogZre, int par, int kop, int expectedKunszt) {
        WalkaSetup(maxE, wrogE, sil, wrogSil, zre, wrogZre, par, kop);
        game.Gear.Values[TGearItem.Armor] = 100;
        game.Gear.Values[TGearItem.WornCount] = 127;
        game.DoFight();
        Assert.Equal(expectedKunszt, game.Player.Experience);
    }

    [Fact]
    public void WalkaEqualStats() =>
        RunWalka(0, 0, 1, 1, 10, 10, 0, 0, 34);
    [Fact]
    public void WalkaMaxePlusOne() =>
        RunWalka(1, 0, 1, 1, 10, 10, 0, 0, 33);
    [Fact]
    public void WalkaMaxePlusThreeOverlap() =>
        RunWalka(3, 0, 1, 1, 10, 10, 0, 0, 42);
    [Fact]
    public void WalkaMaxePlusFive() =>
        RunWalka(5, 0, 1, 1, 10, 10, 0, 0, 32);
    [Fact]
    public void WalkaMaxePlusEleven() =>
        RunWalka(11, 0, 1, 1, 10, 10, 0, 0, 30);
    [Fact]
    public void WalkaMaxePlusTwenty() =>
        RunWalka(20, 0, 1, 1, 10, 10, 0, 0, 27);
    [Fact]
    public void WalkaMaxePlusTwentyNine() =>
        RunWalka(29, 0, 1, 1, 10, 10, 0, 0, 24);
    [Fact]
    public void WalkaMaxePlusThirtyOne() =>
        RunWalka(31, 0, 1, 1, 10, 10, 0, 0, 23);
    [Fact]
    public void WalkaMaxeBandAdjustment() =>
        RunWalka(76, 0, 1, 1, 10, 10, 0, 0, 21);
    [Fact]
    public void WalkaMaxeHugeAdjustment() =>
        RunWalka(116, 0, 1, 1, 10, 10, 0, 0, 18);
    [Fact]
    public void WalkaMaxeMinusTwo() =>
        RunWalkaRange(0, 2, 61, 1, 10, 10, 0, 0, 24, 26);
    [Fact]
    public void WalkaMaxeMinusTen() =>
        RunWalkaRange(0, 10, 61, 1, 10, 10, 0, 0, 28, 31);
    [Fact]
    public void WalkaMaxeMinusTwentyOne() =>
        RunWalkaRange(0, 21, 61, 1, 10, 10, 0, 0, 33, 36);
    [Fact]
    public void WalkaSilPlusOne() =>
        RunWalkaShielded(0, 0, 11, 10, 10, 10, 0, 0, 33);
    [Fact]
    public void WalkaSilPlusTen() =>
        RunWalkaShielded(0, 0, 20, 10, 10, 10, 0, 0, 24);
    [Fact]
    public void WalkaSilMinusOne() =>
        RunWalkaShielded(0, 0, 9, 10, 10, 10, 0, 0, 35);
    [Fact]
    public void WalkaSilMinusTen() =>
        RunWalkaShielded(0, 0, 1, 11, 10, 10, 0, 0, 44);
    [Fact]
    public void WalkaParAdjustments() =>
        RunWalka(0, 0, 1, 1, 10, 10, 96, 0, 29);
    [Fact]
    public void WalkaKopAdjustments() =>
        RunWalka(0, 0, 1, 1, 10, 10, 0, 96, 27);
    [Fact]
    public void WalkaZrePlusOne() =>
        RunWalkaRange(0, 0, 1, 1, 11, 10, 0, 0, 32, 33);
    [Fact]
    public void WalkaZrePlusTwelve() =>
        RunWalkaRange(0, 0, 1, 1, 22, 10, 0, 0, 27, 28);
    [Fact]
    public void WalkaZreMinusOne() =>
        RunWalkaRange(0, 0, 1, 1, 9, 10, 0, 0, 34, 35);
    [Fact]
    public void WalkaZreMinusTwelve() =>
        RunWalkaRange(0, 0, 1, 1, 1, 13, 0, 0, 39, 40);

    [Fact]
    public void WalkaPoisoned() {
        WalkaSetup(100, 0, 1, 1, 10, 10, 0, 0);
        game.Combat.PoisonRounds = 10;
        game.DoFight();
        Assert.True(game.Combat.EnemyEnergy < 1);
        Assert.True(game.Player.Energy > 0);
    }

    [Fact]
    public void WalkaFireball() {
        WalkaSetup(0, 0, 1, 1, 10, 10, 0, 0);
        game.Combat.FireballCharges = 1;
        game.DoFight();
        Assert.True(game.Combat.EnemyEnergy < 1);
        Assert.True(game.Combat.FireballCharges == 0 || game.Combat.FireballCharges == 1);
    }

    [Fact]
    public void WalkaPoison() {
        WalkaSetup(0, 0, 1, 1, 10, 10, 0, 0);
        game.Combat.PoisonCharges = 1;
        game.DoFight();
        Assert.True(game.Combat.EnemyEnergy < 1);
        Assert.True(game.Combat.PoisonCharges == 0 || game.Combat.PoisonCharges == 1);
    }

    [Fact]
    public void WalkaParry() {
        WalkaSetup(100, 0, 1, 20, 10, 10, 139, 0);
        game.DoFight();
        Assert.True(game.Combat.EnemyEnergy < 1);
        Assert.True(game.Player.Energy > 0);
    }

    [Fact]
    public void WalkaParryLight() {
        WalkaSetup(10000, 10, 3, 3, 10, 10, 139, 0);
        game.Player.Energy = 10000;
        game.DoFight();
        Assert.True(game.Combat.EnemyEnergy < 1);
        Assert.True(game.Player.Energy > 0);
    }

    [Fact]
    public void WalkaSpecials() {
        WalkaSetup(10000, 20, 3, 20, 10, 10, 139, 0);
        game.Player.Energy = 10000;
        game.Combat.FireballCharges = 1;
        game.Combat.PoisonCharges = 1;
        game.Combat.PoisonRounds = 10;
        game.DoFight();
        Assert.True(game.Combat.EnemyEnergy < 1);
        Assert.True(game.Player.Energy > 0);
    }

    [Fact]
    public void WalkaSuperKopHit() {
        WalkaSetup(0, 0, 1, 1, 10, 10, 0, 100);
        game.Player.Mana = 100;
        game.Skills.Values[TSkill.KickMaxEnergy] = 100;
        game.Player.Energy = 50;
        game.DoFight();
        Assert.True(game.Combat.EnemyEnergy < 1);
        Assert.True(game.Player.Mana < 100);
    }

    [Fact]
    public void WalkaSuperKopHitAgain() {
        WalkaSetup(0, 0, 1, 1, 10, 10, 0, 100);
        game.Player.Mana = 100;
        game.Skills.Values[TSkill.KickMaxEnergy] = 100;
        game.Player.Energy = 50;
        game.DoFight();
        Assert.True(game.Combat.EnemyEnergy < 1);
        Assert.True(game.Player.Mana < 100);
    }

    [Fact]
    public void WalkaSuperKopChybia() {
        WalkaSetup(0, 0, 1, 1, 10, 10, 0, 11);
        game.Player.Mana = 100;
        game.Skills.Values[TSkill.KickMaxEnergy] = 100;
        game.Player.Energy = 50;
        game.DoFight();
        Assert.True(game.Combat.EnemyEnergy < 1);
        Assert.True(game.Player.Mana < 100);
    }

    [Fact]
    public void WalkaFlee() {
        WalkaSetup(0, 0, 1, 1, 10, 10, 0, 0);
        game.Skills.Values[TSkill.Fleeing] = 100;
        game.Skills.Values[TSkill.FleeEnergy] = 100;
        game.Player.Energy = 50;
        game.Player.Mana = 100;
        game.DoFight();
        Assert.True(game.Combat.Fled);
    }

    [Fact]
    public void WalkaFleeChybia() {
        WalkaSetup(0, 0, 1, 1, 10, 10, 0, 0);
        game.Skills.Values[TSkill.Fleeing] = 1;
        game.Skills.Values[TSkill.FleeEnergy] = 100;
        game.Player.Energy = 50;
        game.Player.Mana = 100;
        game.DoFight();
        Assert.True(game.Combat.EnemyEnergy < 1);
    }

    [Fact]
    public void WalkaPotrawki() {
        WalkaSetup(0, 0, 1, 1, 10, 10, 0, 0);
        game.Skills.Values[TSkill.Cooking] = 99;
        game.Bag.Items[TBagItem.Bigos] = 0;
        game.Bag.Count = 0;
        game.DoFight();
        Assert.True(game.Combat.EnemyEnergy < 1);
        Assert.True(game.Bag.Count == 0 || game.Bag.Count == 1);
    }

    [Fact]
    public void WalkaDeath() {
        WalkaSetup(0, 0, 1, 61, 10, 10, 0, 0);
        game.Player.Energy = 1;
        game.DoFight();
        Assert.True(game.Player.Energy < 1 || game.Combat.EnemyEnergy < 1);
    }

    [Fact]
    public void WalkaFukroll() {
        WalkaSetup(0, 10, 2, 1, 10, 10, 0, 0);
        game.Gear.Values[TGearItem.LuckRoll] = 1;
        game.DoFight();
        Assert.True(game.Combat.EnemyEnergy < 1);
    }

    void EncounterSetup() {
        WalkaSetup(127, 0, 127, 1, 127, 1, 0, 0);
        game.Bag.Items[TBagItem.SmallShield] = 0;
        game.Skills.Values[TSkill.Cooking] = 0;
        game.Combat.FireballCharges = 0;
        game.Combat.PoisonCharges = 0;
        game.Combat.PoisonRounds = 0;
        game.Bag.Items[TBagItem.Heart] = 0;
        game.Bag.ItemRooms[TBagItem.Heart].Clear();
    }

    [Fact]
    public void Encounters() {
        EncounterSetup();
        game.Command = "ZABIJ MROWKA";
        game.Fight(TFoe.WeakEnemy);
        Assert.True(game.Combat.EnemyEnergy < 1);
        EncounterSetup();
        game.FightDog();
        Assert.True(game.Combat.EnemyEnergy < 1);
        EncounterSetup();
        game.Fight(TFoe.WeakishEnemy);
        Assert.True(game.Combat.EnemyEnergy < 1);
        EncounterSetup();
        game.Fight(TFoe.MediumEnemy);
        Assert.True(game.Combat.EnemyEnergy < 1);
        EncounterSetup();
        game.Fight(TFoe.HardEnemy);
        Assert.True(game.Combat.EnemyEnergy < 1);
        EncounterSetup();
        game.Fight(TFoe.VeryEasyEnemy);
        Assert.True(game.Combat.EnemyEnergy < 1 && game.Player.Energy > 0);
        EncounterSetup();
        game.Fight(TFoe.EasyEnemy);
        Assert.True(game.Combat.EnemyEnergy < 1 && game.Player.Energy > 0);
        EncounterSetup();
        game.Fight(TFoe.NotEasyEnemy);
        Assert.True(game.Combat.EnemyEnergy < 1 && game.Player.Energy > 0);
        EncounterSetup();
        game.Fight(TFoe.VeryHardEnemy);
        Assert.True(game.Combat.EnemyEnergy < 1);
    }

    [Fact]
    public void EnergyClampsAtZero() {
        game.Player.MaxEnergy = 50;
        game.Player.Energy = 10;
        Assert.False(game.Player.ChangeEnergy(-40));
        Assert.Equal(0, game.Player.Energy);
    }

    [Fact]
    public void EnergyClampsAtMax() {
        game.Player.MaxEnergy = 50;
        game.Player.Energy = 10;
        Assert.True(game.Player.ChangeEnergy(100));
        Assert.Equal(50, game.Player.Energy);
    }

    [Fact]
    public void ManaClampsAtZero() {
        game.Player.MaxMana = 50;
        game.Player.Mana = 10;
        Assert.False(game.Player.ChangeMana(-40));
        Assert.Equal(0, game.Player.Mana);
    }

    [Fact]
    public void ManaClampsAtMax() {
        game.Player.MaxMana = 50;
        game.Player.Mana = 10;
        Assert.True(game.Player.ChangeMana(100));
        Assert.Equal(50, game.Player.Mana);
    }

    [Fact]
    public void ManaExactCostIsEnough() {
        game.Player.MaxMana = 50;
        game.Player.Mana = 10;
        Assert.True(game.Player.ChangeMana(-10));
        Assert.Equal(0, game.Player.Mana);
    }

    [Fact]
    public void PorownanieNieDzialaBezMany() {
        game.Player.MaxMana = 100;
        game.Player.Mana = 0;
        game.Skills.Values[TSkill.Comparing] = 1;
        game.CmdCompare();
        Assert.Equal(0, game.Player.Mana);
    }

    [Fact]
    public void WalkaDodge() {
        WalkaSetup(0, 30, 3, 1, 0, 2, 0, 0);
        game.Player.Energy = 100;
        game.DoFight();
        Assert.True(game.Combat.EnemyEnergy < 1);
        Assert.True(game.Player.Energy > 0);
    }
}
