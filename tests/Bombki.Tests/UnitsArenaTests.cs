
namespace Bombki.Tests;

public class UnitsArenaTests {
    readonly Game game = new();
    TestStdin FeedStdin(string lines) =>
        TestStdin.Feed(lines);

    [Fact]
    public void ScenaMessages() {
        game.Session.Room = 1;
        game.World.Monsters[TMonster.Guitarist] = 1;
        game.World.Monsters[TMonster.Drummer] = 1;
        game.World.Monsters[TMonster.Keyboardist] = 1;
        game.World.Monsters[TMonster.Liroy] = 1;
        game.World.DescribeStage();
        Assert.Equal(1, game.Session.Room);
    }

    [Fact]
    public void TlumMessages() {
        game.Session.Room = 1;
        game.World.Monsters[TMonster.Kid] = 1;
        game.World.Monsters[TMonster.Lunatic] = 1;
        game.World.Monsters[TMonster.Listener] = 1;
        game.World.Monsters[TMonster.Fan] = 1;
        game.World.Monsters[TMonster.Man] = 1;
        game.World.Monsters[TMonster.Policeman] = 1;
        game.World.Monsters[TMonster.Bouncer] = 1;
        game.World.Monsters[TMonster.Gorilla] = 1;
        game.World.Monsters[TMonster.OldMan] = 1;
        game.World.Monsters[TMonster.Reporter] = 1;
        game.World.DescribeCrowd();
        Assert.Equal(1, game.Session.Room);
    }

    [Fact]
    public void KtoMessages() {
        game.Session.Room = 1;
        game.World.Monsters[TMonster.Woodworm] = 1;
        game.World.Monsters[TMonster.Fly] = 1;
        game.World.Monsters[TMonster.Bacterium] = 1;
        game.World.Monsters[TMonster.Snail] = 1;
        game.World.Monsters[TMonster.Beetle] = 1;
        game.World.Monsters[TMonster.Cockroach] = 1;
        game.World.Monsters[TMonster.Ant] = 1;
        game.World.Monsters[TMonster.Spider] = 1;
        game.World.Monsters[TMonster.Boar] = 1;
        game.World.Monsters[TMonster.Rat] = 1;
        game.World.Monsters[TMonster.Fox] = 1;
        game.World.Monsters[TMonster.Partridge] = 1;
        game.World.Monsters[TMonster.Hare] = 1;
        game.World.Monsters[TMonster.Mongrel] = 1;
        game.World.Monsters[TMonster.Eagle] = 1;
        game.World.Monsters[TMonster.Deer] = 1;
        game.World.Monsters[TMonster.Elephant] = 1;
        game.World.Monsters[TMonster.Lion] = 1;
        game.World.Monsters[TMonster.Giraffe] = 1;
        game.World.Monsters[TMonster.Camel] = 1;
        game.World.Monsters[TMonster.Ostrich] = 1;
        game.World.Monsters[TMonster.Boa] = 1;
        game.World.Monsters[TMonster.Wolf] = 1;
        game.World.Monsters[TMonster.Bison] = 1;
        game.World.Monsters[TMonster.Panther] = 1;
        game.World.Monsters[TMonster.Gladiator] = 1;
        game.World.Monsters[TMonster.Warrior] = 1;
        game.World.Monsters[TMonster.Tamer] = 1;
        game.World.DescribeBeasts();
        Assert.Equal(1, game.Session.Room);
    }

    [Fact]
    public void UlSklepMessages() {
        game.Session.Room = 1;
        game.World.Monsters[TMonster.Dachshund] = 1;
        game.World.Monsters[TMonster.Shepherd] = 1;
        game.World.Monsters[TMonster.Pooch] = 1;
        game.World.Monsters[TMonster.Spaniel] = 1;
        game.World.Monsters[TMonster.Poodle] = 1;
        game.World.Monsters[TMonster.Cabman] = 1;
        game.World.Monsters[TMonster.Vendor] = 1;
        game.World.Monsters[TMonster.Sweeper] = 1;
        game.World.Monsters[TMonster.Drunkard] = 1;
        game.World.Monsters[TMonster.SickMan] = 1;
        game.World.DescribeStreet();
        Assert.Equal(1, game.Session.Room);
    }

    [Fact]
    public void BluszczMessages() {
        game.Session.Room = 1;
        game.World.Monsters[TMonster.Sorrel] = 1;
        game.World.Monsters[TMonster.Daisy] = 1;
        game.World.Monsters[TMonster.Clover] = 1;
        game.World.Monsters[TMonster.SowThistle] = 1;
        game.World.Monsters[TMonster.Dandelion] = 1;
        game.World.Monsters[TMonster.Rose] = 1;
        game.World.Monsters[TMonster.Thistle] = 1;
        game.World.Monsters[TMonster.Blackberry] = 1;
        game.World.Monsters[TMonster.Gooseberry] = 1;
        game.World.Monsters[TMonster.Raspberry] = 1;
        game.World.Monsters[TMonster.Grass] = 1;
        game.World.Monsters[TMonster.Duncan] = 1;
        game.World.DescribePlants();
        Assert.Equal(1, game.Session.Room);
    }

    [Fact]
    public void PierdolyTlo() {
        using (FeedStdin("1")) {
            game.Command = "ZMIEN TLO";
            game.DisplayPrefs();
        }
    }

    [Fact]
    public void Porownanie() {
        game.Player.Mana = 10;
        using (FeedStdin("DZIK")) {
            game.Combat.CompareFoe();
        }
    }

    [Fact]
    public void PorownanieKeepsTheTypedCommand() {
        game.Player.Mana = 10;
        game.Command = "POROWNAJ";
        using (FeedStdin("WSCHOD")) {
            game.Combat.CompareFoe();
        }
        Assert.Equal("POROWNAJ", game.Command);
    }

    void MiniarenaSetup() {
        game.Session.ExitRequested = false;
        UnitsSeed.Reseed(game);
        game.World.Monsters[TMonster.Bacterium] = 33;
        game.World.Monsters[TMonster.Snail] = 33;
        game.World.Monsters[TMonster.Woodworm] = 33;
        game.World.Monsters[TMonster.Fly] = 33;
        game.World.Monsters[TMonster.Beetle] = 33;
        game.World.Monsters[TMonster.Cockroach] = 33;
        game.World.Monsters[TMonster.Ant] = 33;
        game.World.Monsters[TMonster.Spider] = 33;
        game.World.Monsters[TMonster.Boar] = 33;
        game.World.Monsters[TMonster.Mongrel] = 33;
        game.World.Monsters[TMonster.Rat] = 33;
        game.World.Monsters[TMonster.Fox] = 33;
        game.World.Monsters[TMonster.Partridge] = 33;
        game.World.Monsters[TMonster.Hare] = 33;
        game.World.Monsters[TMonster.Eagle] = 33;
        game.World.Monsters[TMonster.Deer] = 33;
        game.World.Monsters[TMonster.Elephant] = 33;
        game.World.Monsters[TMonster.Lion] = 33;
        game.World.Monsters[TMonster.Giraffe] = 33;
        game.World.Monsters[TMonster.Wolf] = 33;
        game.World.Monsters[TMonster.Camel] = 33;
        game.World.Monsters[TMonster.Ostrich] = 33;
        game.World.Monsters[TMonster.Boa] = 33;
        game.World.Monsters[TMonster.Bison] = 33;
        game.World.Monsters[TMonster.Panther] = 33;
        game.World.Monsters[TMonster.Gladiator] = 33;
        game.World.Monsters[TMonster.Warrior] = 33;
        game.World.Monsters[TMonster.Tamer] = 33;
        game.Session.Room = 33;
        game.Player.Stats[TStat.Strength] = 127;
        game.Player.Stats[TStat.Dexterity] = 127;
        game.Player.Energy = 30000;
        game.Combat.Fled = false;
    }

    [Fact]
    public void MiniarenaSlabo() {
        MiniarenaSetup();
        using (FeedStdin("ZABIJ BAKTERIA|ZABIJ SLIMAK|ZABIJ KORNIK|ZABIJ MUCHA|ZABIJ ZUK|ZABIJ KARALUCH|ZABIJ MROWKA|ZABIJ PAJAK|WYJSCIE")) {
            game.World.DispatchRoom();
        }
    }

    [Fact]
    public void MiniarenaMniejslabo() {
        MiniarenaSetup();
        using (FeedStdin("ZABIJ DZIK|ZABIJ WILCZUR|ZABIJ SZCZUR|ZABIJ LIS|ZABIJ KUROPATWA|ZABIJ ZAJAC|ZABIJ ORZEL|ZABIJ SARNA|WYJSCIE")) {
            game.World.DispatchRoom();
        }
    }

    [Fact]
    public void MiniarenaSrednio() {
        MiniarenaSetup();
        using (FeedStdin("ZABIJ SLON|ZABIJ LEW|ZABIJ ZYRAFA|ZABIJ WILK|ZABIJ WIELBLAD|ZABIJ STRUS|ZABIJ BOA|ZABIJ BIZON|ZABIJ PANTERA|WYJSCIE")) {
            game.World.DispatchRoom();
        }
    }

    [Fact]
    public void MiniarenaTrudno() {
        MiniarenaSetup();
        using (FeedStdin("ZABIJ GLADIATOR|ZABIJ WOJOWNIK|ZABIJ TRENER|WYJSCIE")) {
            game.World.DispatchRoom();
        }
    }

    void MiniarenaAt(string cmd, int room) {
        game.Session.ExitRequested = false;
        game.Session.Room = room;
        using (FeedStdin(cmd)) {
            game.World.DispatchRoom();
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

    void FightSetup() {
        UnitsSeed.Reseed(game);
        game.Session.Room = 100;
        game.World.Monsters[TMonster.Drummer] = 100;
        game.World.Monsters[TMonster.Guitarist] = 100;
        game.World.Monsters[TMonster.Keyboardist] = 100;
        game.World.Monsters[TMonster.Liroy] = 100;
        game.World.Monsters[TMonster.Duncan] = 100;
        game.World.Monsters[TMonster.Sorrel] = 100;
        game.World.Monsters[TMonster.Daisy] = 100;
        game.World.Monsters[TMonster.Clover] = 100;
        game.World.Monsters[TMonster.SowThistle] = 100;
        game.World.Monsters[TMonster.Dandelion] = 100;
        game.World.Monsters[TMonster.Rose] = 100;
        game.World.Monsters[TMonster.Thistle] = 100;
        game.World.Monsters[TMonster.Raspberry] = 100;
        game.World.Monsters[TMonster.Gooseberry] = 100;
        game.World.Monsters[TMonster.Blackberry] = 100;
        game.World.Monsters[TMonster.Grass] = 100;
        game.Player.Stats[TStat.Strength] = 127;
        game.Player.Stats[TStat.Dexterity] = 127;
        game.Player.Energy = 30000;
        game.Combat.Fled = false;
        game.World.DuncanQuest = 0;
        game.Player.Money = 10000;
        game.Bag.Permit = 100;
        game.Bag.Items[TBagItem.Backpack] = 100;
        game.World.Quest = 0;
        game.Player.Experience = 0;
    }

    [Fact]
    public void Fightscena() {
        FightSetup();
        game.Command = "ZABIJ PERKUSISTA";
        game.Combat.StageFights();
        game.Command = "ZABIJ GITARZYSTA";
        game.Combat.StageFights();
        game.Command = "ZABIJ LIROY";
        game.Combat.StageFights();
        game.Command = "ZABIJ ORGANISTA";
        game.Combat.StageFights();
    }

    [Fact]
    public void FightbluszczZabij() {
        FightSetup();
        game.Command = "ZABIJ DUNCAN";
        game.Combat.PlantFights();
        game.Command = "ZABIJ SZCZAW";
        game.Combat.PlantFights();
        game.Command = "ZABIJ STOKROTKA";
        game.Combat.PlantFights();
        game.Command = "ZABIJ KONICZYNKA";
        game.Combat.PlantFights();
        game.Command = "ZABIJ MLECZ";
        game.Combat.PlantFights();
        game.Command = "ZABIJ DMUCHAWIEC";
        game.Combat.PlantFights();
        game.Command = "ZABIJ ROZA";
        game.Combat.PlantFights();
        game.Command = "ZABIJ OSET";
        game.Combat.PlantFights();
        game.Command = "ZABIJ MALINA";
        game.Combat.PlantFights();
        game.Command = "ZABIJ AGREST";
        game.Combat.PlantFights();
        game.Command = "ZABIJ JEZYNA";
        game.Combat.PlantFights();
        game.Command = "ZABIJ TRAWA";
        game.Combat.PlantFights();
    }

    [Fact]
    public void FightbluszczShop() {
        FightSetup();
        game.Command = "SECRET LISTA";
        game.Combat.PlantFights();
        game.Command = "KUP DOKUMENT";
        game.Combat.PlantFights();
        game.Command = "KUP PLECAK";
        game.Combat.PlantFights();
    }

    [Fact]
    public void FightbluszczDuncan() {
        FightSetup();
        game.World.DuncanQuest = 0;
        game.Command = "ROZMAWIAJ DUNCAN";
        game.Combat.PlantFights();
        game.World.DuncanQuest = -125;
        game.Command = "ROZMAWIAJ DUNCAN";
        game.Combat.PlantFights();
    }

    void PorownanieAsk(string monster,
        int poziom, int sil, int zre, int par, int kop) {
        using (FeedStdin(monster)) {
            game.Player.Mana = 10;
            game.Player.Level = poziom;
            game.Player.Stats[TStat.Strength] = sil;
            game.Player.Stats[TStat.Dexterity] = zre;
            game.Skills.Values[TSkill.Parry] = par;
            game.Skills.Values[TSkill.Kick] = kop;
            game.Combat.CompareFoe();
        }
    }

    [Fact]
    public void PorownanieBeasts() {
        PorownanieAsk("DZIK", 0, 0, 0, 0, 0);
        PorownanieAsk("KUROPATWA", 0, 16, 0, 0, 0);
        PorownanieAsk("SARNA", 0, 19, 0, 0, 0);
        PorownanieAsk("DZIADEK", 0, 30, 0, 0, 0);
        Assert.Equal(30, game.Player.Stats[TStat.Strength]);
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
        game.Player.MaxMana = 100;
        game.Player.Mana = 10;
        using (FeedStdin("BAKTERIA")) {
            game.Combat.CompareFoe();
        }

        Assert.Equal(15, game.Player.Mana);
    }

    void LootSetup() {
        UnitsSeed.Reseed(game);
        game.Bag.Items[TBagItem.Heart] = 0;
        game.Bag.ItemRooms[TBagItem.Heart].Clear();
        game.Player.MaxEnergy = 30000;
        game.Player.Energy = 30000;
        game.Player.Stats[TStat.Strength] = 127;
        game.Player.Stats[TStat.Dexterity] = 127;
        game.Combat.Fled = false;
        game.Skills.Values[TSkill.Fleeing] = 0;
        game.Skills.Values[TSkill.FleeEnergy] = 0;
        game.Player.Mana = 0;
        game.Combat.FireballCharges = 0;
        game.Combat.PoisonCharges = 0;
        game.Combat.PoisonRounds = 0;
        game.Skills.Values[TSkill.Kick] = 0;
        game.Skills.Values[TSkill.KickMinMana] = 0;
        game.Skills.Values[TSkill.KickMaxEnergy] = 0;
        game.Gear.Values[TGearItem.Armor] = 0;
        game.Gear.Values[TGearItem.WornCount] = 0;
        game.Gear.Values[TGearItem.LuckRoll] = 0;
        game.Bag.Count = 0;
    }

    [Fact]
    public void HeartLootIgnoresGroundHeart() {
        LootSetup();
        game.Bag.AddGroundItem(TBagItem.Heart, 7);
        game.Combat.CompositeFight(1, 0, 1, 0, 1, 0, 0, 1, 20, false, false);
        Assert.Equal(1, game.Bag.Items[TBagItem.Heart]);
    }

    [Fact]
    public void EncounterHeartVeasy() {
        LootSetup();
        for (int i = 1; i <= 160; i++) {
            if (game.Bag.Items[TBagItem.Heart] == 0) {
                game.Combat.Fight(TFoe.VeryEasyEnemy);
            }
        }

        Assert.Equal(1, game.Bag.Items[TBagItem.Heart]);
    }

    [Fact]
    public void EncounterHeartEasy() {
        LootSetup();
        for (int i = 1; i <= 160; i++) {
            if (game.Bag.Items[TBagItem.Heart] == 0) {
                game.Combat.Fight(TFoe.EasyEnemy);
            }
        }

        Assert.Equal(1, game.Bag.Items[TBagItem.Heart]);
    }

    [Fact]
    public void EncounterHeartNeasy() {
        LootSetup();
        for (int i = 1; i <= 160; i++) {
            if (game.Bag.Items[TBagItem.Heart] == 0) {
                game.Combat.Fight(TFoe.NotEasyEnemy);
            }
        }

        Assert.Equal(1, game.Bag.Items[TBagItem.Heart]);
    }

    [Fact]
    public void EncounterHeartTrudno() {
        LootSetup();
        for (int i = 1; i <= 110; i++) {
            if (game.Bag.Items[TBagItem.Heart] == 0) {
                game.Combat.Fight(TFoe.HardEnemy);
            }
        }

        Assert.Equal(1, game.Bag.Items[TBagItem.Heart]);
    }

    [Fact]
    public void EncounterHeartBtrudno() {
        LootSetup();
        for (int i = 1; i <= 150; i++) {
            if (game.Bag.Items[TBagItem.Heart] == 0) {
                game.Combat.Fight(TFoe.VeryHardEnemy);
            }
        }

        Assert.Equal(1, game.Bag.Items[TBagItem.Heart]);
    }

    [Fact]
    public void WalkaPotrawkiLearn() {
        UnitsSeed.Reseed(game);
        bool learned = false;
        for (int i = 1; i <= 800; i++) {
            game.Player.Energy = 100;
            game.Player.Stats[TStat.Strength] = 127;
            game.Player.Stats[TStat.Dexterity] = 127;
            game.Player.MaxEnergy = 100;
            game.Combat.EnemyEnergy = 0;
            game.Combat.EnemyStrength = 1;
            game.Combat.EnemyDexterity = 1;
            game.Skills.Values[TSkill.Parry] = 0;
            game.Skills.Values[TSkill.Kick] = 0;
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
            game.Player.Energy = 100;
            game.Skills.Values[TSkill.Cooking] = 1;
            game.Bag.Items[TBagItem.Bigos] = 0;
            game.Combat.DoFight();
            if (game.Skills.Values[TSkill.Cooking] > 1) {
                learned = true;
            }
        }
        Assert.True(learned);
    }

    [Fact]
    public void WalkaParryLearn() {
        UnitsSeed.Reseed(game);
        for (int i = 1; i <= 60; i++) {
            game.Player.Energy = 100;
            game.Player.Stats[TStat.Strength] = 127;
            game.Player.Stats[TStat.Dexterity] = 127;
            game.Player.MaxEnergy = 30000;
            game.Combat.EnemyEnergy = 3000;
            game.Combat.EnemyStrength = 5;
            game.Combat.EnemyDexterity = 1;
            game.Skills.Values[TSkill.Parry] = 99;
            game.Skills.Values[TSkill.Kick] = 0;
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
            game.Player.Energy = 30000;
            game.Combat.DoFight();
            if (game.Skills.Values[TSkill.Parry] > 99) {
                break;
            }
        }
        Assert.True(game.Skills.Values[TSkill.Parry] > 99);
    }
}
