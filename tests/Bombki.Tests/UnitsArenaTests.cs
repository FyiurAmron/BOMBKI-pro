using static Game;

namespace Bombki.Tests;

public class UnitsArenaTests {
    static TestStdin FeedStdin(string lines) =>
        TestStdin.Feed(lines);

    [Fact]
    public void ScenaMessages() {
        Session.Room = 1;
        World.Monsters[TMonster.Guitarist] = 1;
        World.Monsters[TMonster.Drummer] = 1;
        World.Monsters[TMonster.Keyboardist] = 1;
        World.Monsters[TMonster.Liroy] = 1;
        DescribeStage();
        Assert.Equal(1, Session.Room);
    }

    [Fact]
    public void TlumMessages() {
        Session.Room = 1;
        World.Monsters[TMonster.Kid] = 1;
        World.Monsters[TMonster.Lunatic] = 1;
        World.Monsters[TMonster.Listener] = 1;
        World.Monsters[TMonster.Fan] = 1;
        World.Monsters[TMonster.Man] = 1;
        World.Monsters[TMonster.Policeman] = 1;
        World.Monsters[TMonster.Bouncer] = 1;
        World.Monsters[TMonster.Gorilla] = 1;
        World.Monsters[TMonster.OldMan] = 1;
        World.Monsters[TMonster.Reporter] = 1;
        DescribeCrowd();
        Assert.Equal(1, Session.Room);
    }

    [Fact]
    public void KtoMessages() {
        Session.Room = 1;
        World.Monsters[TMonster.Woodworm] = 1;
        World.Monsters[TMonster.Fly] = 1;
        World.Monsters[TMonster.Bacterium] = 1;
        World.Monsters[TMonster.Snail] = 1;
        World.Monsters[TMonster.Beetle] = 1;
        World.Monsters[TMonster.Cockroach] = 1;
        World.Monsters[TMonster.Ant] = 1;
        World.Monsters[TMonster.Spider] = 1;
        World.Monsters[TMonster.Boar] = 1;
        World.Monsters[TMonster.Rat] = 1;
        World.Monsters[TMonster.Fox] = 1;
        World.Monsters[TMonster.Partridge] = 1;
        World.Monsters[TMonster.Hare] = 1;
        World.Monsters[TMonster.Mongrel] = 1;
        World.Monsters[TMonster.Eagle] = 1;
        World.Monsters[TMonster.Deer] = 1;
        World.Monsters[TMonster.Elephant] = 1;
        World.Monsters[TMonster.Lion] = 1;
        World.Monsters[TMonster.Giraffe] = 1;
        World.Monsters[TMonster.Camel] = 1;
        World.Monsters[TMonster.Ostrich] = 1;
        World.Monsters[TMonster.Boa] = 1;
        World.Monsters[TMonster.Wolf] = 1;
        World.Monsters[TMonster.Bison] = 1;
        World.Monsters[TMonster.Panther] = 1;
        World.Monsters[TMonster.Gladiator] = 1;
        World.Monsters[TMonster.Warrior] = 1;
        World.Monsters[TMonster.Tamer] = 1;
        DescribeBeasts();
        Assert.Equal(1, Session.Room);
    }

    [Fact]
    public void UlSklepMessages() {
        Session.Room = 1;
        World.Monsters[TMonster.Dachshund] = 1;
        World.Monsters[TMonster.Shepherd] = 1;
        World.Monsters[TMonster.Pooch] = 1;
        World.Monsters[TMonster.Spaniel] = 1;
        World.Monsters[TMonster.Poodle] = 1;
        World.Monsters[TMonster.Cabman] = 1;
        World.Monsters[TMonster.Vendor] = 1;
        World.Monsters[TMonster.Sweeper] = 1;
        World.Monsters[TMonster.Drunkard] = 1;
        World.Monsters[TMonster.SickMan] = 1;
        DescribeStreet();
        Assert.Equal(1, Session.Room);
    }

    [Fact]
    public void BluszczMessages() {
        Session.Room = 1;
        World.Monsters[TMonster.Sorrel] = 1;
        World.Monsters[TMonster.Daisy] = 1;
        World.Monsters[TMonster.Clover] = 1;
        World.Monsters[TMonster.SowThistle] = 1;
        World.Monsters[TMonster.Dandelion] = 1;
        World.Monsters[TMonster.Rose] = 1;
        World.Monsters[TMonster.Thistle] = 1;
        World.Monsters[TMonster.Blackberry] = 1;
        World.Monsters[TMonster.Gooseberry] = 1;
        World.Monsters[TMonster.Raspberry] = 1;
        World.Monsters[TMonster.Grass] = 1;
        World.Monsters[TMonster.Duncan] = 1;
        DescribePlants();
        Assert.Equal(1, Session.Room);
    }

    [Fact]
    public void PierdolyTlo() {
        using (FeedStdin("1")) {
            Command = "ZMIEN TLO";
            DisplayPrefs();
        }
    }

    [Fact]
    public void Porownanie() {
        Player.Mana = 10;
        using (FeedStdin("DZIK")) {
            CompareFoe();
        }
    }

    static void MiniarenaSetup() {
        Session.ExitRequested = false;
        UnitsSeed.Reseed();
        World.Monsters[TMonster.Bacterium] = 33;
        World.Monsters[TMonster.Snail] = 33;
        World.Monsters[TMonster.Woodworm] = 33;
        World.Monsters[TMonster.Fly] = 33;
        World.Monsters[TMonster.Beetle] = 33;
        World.Monsters[TMonster.Cockroach] = 33;
        World.Monsters[TMonster.Ant] = 33;
        World.Monsters[TMonster.Spider] = 33;
        World.Monsters[TMonster.Boar] = 33;
        World.Monsters[TMonster.Mongrel] = 33;
        World.Monsters[TMonster.Rat] = 33;
        World.Monsters[TMonster.Fox] = 33;
        World.Monsters[TMonster.Partridge] = 33;
        World.Monsters[TMonster.Hare] = 33;
        World.Monsters[TMonster.Eagle] = 33;
        World.Monsters[TMonster.Deer] = 33;
        World.Monsters[TMonster.Elephant] = 33;
        World.Monsters[TMonster.Lion] = 33;
        World.Monsters[TMonster.Giraffe] = 33;
        World.Monsters[TMonster.Wolf] = 33;
        World.Monsters[TMonster.Camel] = 33;
        World.Monsters[TMonster.Ostrich] = 33;
        World.Monsters[TMonster.Boa] = 33;
        World.Monsters[TMonster.Bison] = 33;
        World.Monsters[TMonster.Panther] = 33;
        World.Monsters[TMonster.Gladiator] = 33;
        World.Monsters[TMonster.Warrior] = 33;
        World.Monsters[TMonster.Tamer] = 33;
        Session.Room = 33;
        Player.Stats[TStat.Strength] = 127;
        Player.Stats[TStat.Dexterity] = 127;
        Player.Energy = 30000;
        Combat.Fled = false;
    }

    [Fact]
    public void MiniarenaSlabo() {
        MiniarenaSetup();
        using (FeedStdin("ZABIJ BAKTERIA|ZABIJ SLIMAK|ZABIJ KORNIK|ZABIJ MUCHA|ZABIJ ZUK|ZABIJ KARALUCH|ZABIJ MROWKA|ZABIJ PAJAK|WYJSCIE")) {
            Rooms.DispatchRoom();
        }
    }

    [Fact]
    public void MiniarenaMniejslabo() {
        MiniarenaSetup();
        using (FeedStdin("ZABIJ DZIK|ZABIJ WILCZUR|ZABIJ SZCZUR|ZABIJ LIS|ZABIJ KUROPATWA|ZABIJ ZAJAC|ZABIJ ORZEL|ZABIJ SARNA|WYJSCIE")) {
            Rooms.DispatchRoom();
        }
    }

    [Fact]
    public void MiniarenaSrednio() {
        MiniarenaSetup();
        using (FeedStdin("ZABIJ SLON|ZABIJ LEW|ZABIJ ZYRAFA|ZABIJ WILK|ZABIJ WIELBLAD|ZABIJ STRUS|ZABIJ BOA|ZABIJ BIZON|ZABIJ PANTERA|WYJSCIE")) {
            Rooms.DispatchRoom();
        }
    }

    [Fact]
    public void MiniarenaTrudno() {
        MiniarenaSetup();
        using (FeedStdin("ZABIJ GLADIATOR|ZABIJ WOJOWNIK|ZABIJ TRENER|WYJSCIE")) {
            Rooms.DispatchRoom();
        }
    }

    static void MiniarenaAt(string cmd, int room) {
        Session.ExitRequested = false;
        Session.Room = room;
        using (FeedStdin(cmd)) {
            Rooms.DispatchRoom();
        }
    }

    [Fact]
    public void MiniarenaExitNav() {
        MiniarenaAt("EXIT|WYJSCIE", 33);
        MiniarenaAt("EXIT|WYJSCIE", 34);
        MiniarenaAt("EXIT|WYJSCIE", 35);
        MiniarenaAt("EXIT|WYJSCIE", 40);
        MiniarenaAt("EXIT|WYJSCIE", 41);
        MiniarenaAt("EXIT|WYJSCIE", 45);
        MiniarenaAt("EXIT|WYJSCIE", 46);
        MiniarenaAt("EXIT|WYJSCIE", 47);
        MiniarenaAt("EXIT|WYJSCIE", 56);
        MiniarenaAt("EXIT|WYJSCIE", 57);
        MiniarenaAt("POLUDNIE", 33);
        MiniarenaAt("POLNOC", 33);
        MiniarenaAt("WSCHOD", 33);
        MiniarenaAt("ZACHOD", 33);
    }

    static void FightSetup() {
        UnitsSeed.Reseed();
        Session.Room = 100;
        World.Monsters[TMonster.Drummer] = 100;
        World.Monsters[TMonster.Guitarist] = 100;
        World.Monsters[TMonster.Keyboardist] = 100;
        World.Monsters[TMonster.Liroy] = 100;
        World.Monsters[TMonster.Duncan] = 100;
        World.Monsters[TMonster.Sorrel] = 100;
        World.Monsters[TMonster.Daisy] = 100;
        World.Monsters[TMonster.Clover] = 100;
        World.Monsters[TMonster.SowThistle] = 100;
        World.Monsters[TMonster.Dandelion] = 100;
        World.Monsters[TMonster.Rose] = 100;
        World.Monsters[TMonster.Thistle] = 100;
        World.Monsters[TMonster.Raspberry] = 100;
        World.Monsters[TMonster.Gooseberry] = 100;
        World.Monsters[TMonster.Blackberry] = 100;
        World.Monsters[TMonster.Grass] = 100;
        Player.Stats[TStat.Strength] = 127;
        Player.Stats[TStat.Dexterity] = 127;
        Player.Energy = 30000;
        Combat.Fled = false;
        World.DuncanQuest = 0;
        Player.Money = 10000;
        Bag.Permit = 100;
        Bag.Items[TBagItem.Backpack] = 100;
        World.Quest = 0;
        Player.Experience = 0;
    }

    [Fact]
    public void Fightscena() {
        FightSetup();
        Command = "ZABIJ PERKUSISTA";
        StageFights();
        Command = "ZABIJ GITARZYSTA";
        StageFights();
        Command = "ZABIJ LIROY";
        StageFights();
        Command = "ZABIJ ORGANISTA";
        StageFights();
    }

    [Fact]
    public void FightbluszczZabij() {
        FightSetup();
        Command = "ZABIJ DUNCAN";
        PlantFights();
        Command = "ZABIJ SZCZAW";
        PlantFights();
        Command = "ZABIJ STOKROTKA";
        PlantFights();
        Command = "ZABIJ KONICZYNKA";
        PlantFights();
        Command = "ZABIJ MLECZ";
        PlantFights();
        Command = "ZABIJ DMUCHAWIEC";
        PlantFights();
        Command = "ZABIJ ROZA";
        PlantFights();
        Command = "ZABIJ OSET";
        PlantFights();
        Command = "ZABIJ MALINA";
        PlantFights();
        Command = "ZABIJ AGREST";
        PlantFights();
        Command = "ZABIJ JEZYNA";
        PlantFights();
        Command = "ZABIJ TRAWA";
        PlantFights();
    }

    [Fact]
    public void FightbluszczShop() {
        FightSetup();
        Command = "SECRET LISTA";
        PlantFights();
        Command = "KUP DOKUMENT";
        PlantFights();
        Command = "KUP PLECAK";
        PlantFights();
    }

    [Fact]
    public void FightbluszczDuncan() {
        FightSetup();
        World.DuncanQuest = 0;
        Command = "ROZMAWIAJ DUNCAN";
        PlantFights();
        World.DuncanQuest = -125;
        Command = "ROZMAWIAJ DUNCAN";
        PlantFights();
    }

    static void PorownanieAsk(string monster,
        int poziom, int sil, int zre, int par, int kop) {
        using (FeedStdin(monster)) {
            Player.Mana = 10;
            Player.Level = poziom;
            Player.Stats[TStat.Strength] = sil;
            Player.Stats[TStat.Dexterity] = zre;
            Skills.Values[TSkill.Parry] = par;
            Skills.Values[TSkill.Kick] = kop;
            CompareFoe();
        }
    }

    [Fact]
    public void PorownanieBeasts() {
        PorownanieAsk("DZIK", 0, 0, 0, 0, 0);
        PorownanieAsk("KUROPATWA", 0, 16, 0, 0, 0);
        PorownanieAsk("SARNA", 0, 19, 0, 0, 0);
        PorownanieAsk("DZIADEK", 0, 30, 0, 0, 0);
        Assert.Equal(30, Player.Stats[TStat.Strength]);
    }

    [Fact]
    public void PorownanieBigBeasts() {
        PorownanieAsk("SLON", 0, 0, 0, 0, 0);
        PorownanieAsk("LEW", 0, 16, 0, 0, 0);
        PorownanieAsk("ZYRAFA", 0, 21, 0, 0, 0);
        PorownanieAsk("REPORTER", 0, 23, 0, 0, 0);
    }

    [Fact]
    public void PorownanieFighters() {
        PorownanieAsk("GLADIATOR", 0, 0, 0, 0, 0);
        PorownanieAsk("WOJOWNIK", 0, 16, 0, 0, 0);
        PorownanieAsk("TRENER", 0, 20, 0, 0, 0);
        PorownanieAsk("POLICJANT", 0, 24, 0, 0, 0);
        PorownanieAsk("GORYL", 0, 27, 0, 0, 0);
        PorownanieAsk("OCHRONIARZ", 0, 30, 0, 0, 0);
    }

    [Fact]
    public void PorownanieDrunks() {
        PorownanieAsk("MINI-BARMAN", 0, 0, 0, 0, 0);
        PorownanieAsk("GRUBAS", 0, 25, 0, 0, 0);
        PorownanieAsk("D.J", 0, 27, 0, 0, 0);
        PorownanieAsk("GRUBAS", 0, 29, 0, 0, 0);
        PorownanieAsk("D.J", 0, 35, 0, 0, 0);
    }

    [Fact]
    public void PorownanieMusicians() {
        PorownanieAsk("TAKSOWKARZ", 0, 0, 0, 0, 0);
        PorownanieAsk("GITARZYSTA", 0, 23, 0, 0, 0);
        PorownanieAsk("PERKUSISTA", 0, 26, 0, 0, 0);
        PorownanieAsk("ORGANISTA", 0, 29, 0, 0, 0);
        PorownanieAsk("ZEBRAK", 0, 30, 0, 0, 0);
    }

    [Fact]
    public void PorownanieBluszcz() {
        PorownanieAsk("PEDAL", 0, 0, 0, 0, 0);
        PorownanieAsk("PARA", 0, 20, 0, 0, 0);
        PorownanieAsk("MACIEK", 0, 26, 0, 0, 0);
        PorownanieAsk("ROZA", 0, 30, 0, 0, 0);
        PorownanieAsk("MALINA", 0, 36, 0, 0, 0);
        PorownanieAsk("TRAWA", 0, 0, 9, 0, 0);
        PorownanieAsk("AGREST", 0, 0, 11, 0, 0);
        PorownanieAsk("JEZYNA", 0, 0, 13, 0, 0);
        PorownanieAsk("TRAWA", 0, 0, 15, 0, 0);
        PorownanieAsk("AGREST", 0, 0, 17, 0, 0);
        PorownanieAsk("JEZYNA", 0, 0, 19, 0, 0);
        PorownanieAsk("TRAWA", 0, 0, 21, 0, 0);
    }

    [Fact]
    public void PorownanieLiroyAndDogs() {
        PorownanieAsk("LIROY", 0, 0, 0, 0, 0);
        PorownanieAsk("LIROY", 0, 28, 0, 0, 0);
        PorownanieAsk("LIROY", 0, 33, 0, 0, 0);
        PorownanieAsk("LIROY", 0, 44, 0, 0, 0);
        PorownanieAsk("LIROY", 0, 48, 0, 0, 0);
        PorownanieAsk("POKRZYWA", 0, 0, 0, 0, 0);
        PorownanieAsk("POKRZYWA", 0, 41, 0, 0, 0);
        PorownanieAsk("SPANIEL", 0, 0, 0, 0, 0);
        PorownanieAsk("OWCZAREK", 0, 0, 0, 0, 0);
        PorownanieAsk("PIESEK", 0, 0, 0, 0, 0);
        PorownanieAsk("JAMNIK", 0, 0, 0, 0, 0);
        PorownanieAsk("PUDEL", 0, 0, 0, 0, 0);
    }

    [Fact]
    public void PorownanieBonusesAndBakteria() {
        PorownanieAsk("DZIK", 0, 10, 0, 51, 11);
        PorownanieAsk("DZIK", 0, 10, 0, 76, 71);
        Player.Mana = 10;
        using (FeedStdin("BAKTERIA")) {
            CompareFoe();
        }

        Assert.Equal(15, Player.Mana);
    }

    static void LootSetup() {
        UnitsSeed.Reseed();
        Bag.Items[TBagItem.Heart] = 0;
        Player.MaxEnergy = 30000;
        Player.Energy = 30000;
        Player.Stats[TStat.Strength] = 127;
        Player.Stats[TStat.Dexterity] = 127;
        Combat.Fled = false;
        Skills.Values[TSkill.Fleeing] = 0;
        Skills.Values[TSkill.FleeEnergy] = 0;
        Player.Mana = 0;
        Combat.FireballCharges = 0;
        Combat.PoisonCharges = 0;
        Combat.PoisonRounds = 0;
        Skills.Values[TSkill.Kick] = 0;
        Skills.Values[TSkill.KickMinMana] = 0;
        Skills.Values[TSkill.KickMaxEnergy] = 0;
        Gear.Values[TGearItem.Armor] = 0;
        Gear.Values[TGearItem.WornCount] = 0;
        Gear.Values[TGearItem.LuckRoll] = 0;
        Bag.Count = 0;
    }

    [Fact]
    public void EncounterHeartVeasy() {
        LootSetup();
        for (int i = 1; i <= 160; i++) {
            if (Bag.Items[TBagItem.Heart] == 0) {
                Fight(TFoe.VeryEasyEnemy);
            }
        }

        Assert.Equal(-10, Bag.Items[TBagItem.Heart]);
    }

    [Fact]
    public void EncounterHeartEasy() {
        LootSetup();
        for (int i = 1; i <= 160; i++) {
            if (Bag.Items[TBagItem.Heart] == 0) {
                Fight(TFoe.EasyEnemy);
            }
        }

        Assert.Equal(-10, Bag.Items[TBagItem.Heart]);
    }

    [Fact]
    public void EncounterHeartNeasy() {
        LootSetup();
        for (int i = 1; i <= 160; i++) {
            if (Bag.Items[TBagItem.Heart] == 0) {
                Fight(TFoe.NotEasyEnemy);
            }
        }

        Assert.Equal(-10, Bag.Items[TBagItem.Heart]);
    }

    [Fact]
    public void EncounterHeartTrudno() {
        LootSetup();
        for (int i = 1; i <= 110; i++) {
            if (Bag.Items[TBagItem.Heart] == 0) {
                Fight(TFoe.HardEnemy);
            }
        }

        Assert.Equal(-10, Bag.Items[TBagItem.Heart]);
    }

    [Fact]
    public void EncounterHeartBtrudno() {
        LootSetup();
        for (int i = 1; i <= 150; i++) {
            if (Bag.Items[TBagItem.Heart] == 0) {
                Fight(TFoe.VeryHardEnemy);
            }
        }

        Assert.Equal(-10, Bag.Items[TBagItem.Heart]);
    }

    [Fact]
    public void WalkaPotrawkiLearn() {
        UnitsSeed.Reseed();
        bool learned = false;
        for (int i = 1; i <= 800; i++) {
            Player.Energy = 100;
            Player.Stats[TStat.Strength] = 127;
            Player.Stats[TStat.Dexterity] = 127;
            Player.MaxEnergy = 100;
            Combat.EnemyEnergy = 0;
            Combat.EnemyStrength = 1;
            Combat.EnemyDexterity = 1;
            Skills.Values[TSkill.Parry] = 0;
            Skills.Values[TSkill.Kick] = 0;
            Skills.Values[TSkill.KickMinMana] = 0;
            Skills.Values[TSkill.KickMaxEnergy] = 0;
            Player.Mana = 0;
            Combat.FireballCharges = 0;
            Combat.PoisonCharges = 0;
            Combat.PoisonRounds = 0;
            Skills.Values[TSkill.Fleeing] = 0;
            Skills.Values[TSkill.FleeEnergy] = 0;
            Combat.Fled = false;
            Player.Experience = 0;
            Gear.Values[TGearItem.LuckRoll] = 0;
            Gear.Values[TGearItem.Armor] = 0;
            Gear.Values[TGearItem.WornCount] = 0;
            Player.Energy = 100;
            Skills.Values[TSkill.Cooking] = 1;
            Bag.Items[TBagItem.Bigos] = 0;
            DoFight();
            if (Skills.Values[TSkill.Cooking] > 1) {
                learned = true;
            }
        }
        Assert.True(learned);
    }

    [Fact]
    public void WalkaParryLearn() {
        UnitsSeed.Reseed();
        for (int i = 1; i <= 60; i++) {
            Player.Energy = 100;
            Player.Stats[TStat.Strength] = 127;
            Player.Stats[TStat.Dexterity] = 127;
            Player.MaxEnergy = 30000;
            Combat.EnemyEnergy = 3000;
            Combat.EnemyStrength = 5;
            Combat.EnemyDexterity = 1;
            Skills.Values[TSkill.Parry] = 99;
            Skills.Values[TSkill.Kick] = 0;
            Skills.Values[TSkill.KickMinMana] = 0;
            Skills.Values[TSkill.KickMaxEnergy] = 0;
            Player.Mana = 0;
            Combat.FireballCharges = 0;
            Combat.PoisonCharges = 0;
            Combat.PoisonRounds = 0;
            Skills.Values[TSkill.Fleeing] = 0;
            Skills.Values[TSkill.FleeEnergy] = 0;
            Combat.Fled = false;
            Player.Experience = 0;
            Gear.Values[TGearItem.LuckRoll] = 0;
            Gear.Values[TGearItem.Armor] = 0;
            Gear.Values[TGearItem.WornCount] = 0;
            Player.Energy = 30000;
            DoFight();
            if (Skills.Values[TSkill.Parry] > 99) {
                break;
            }
        }
        Assert.True(Skills.Values[TSkill.Parry] > 99);
    }
}
