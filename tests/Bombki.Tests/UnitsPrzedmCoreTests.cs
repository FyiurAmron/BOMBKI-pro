
namespace Bombki.Tests;

public class UnitsPrzedmCoreTests {
    readonly Game game = new();
    [Fact]
    public void Komendy() {
        game.Command = "PN";
        game.ExpandAbbrev();
        Assert.Equal("POLNOC", game.Command);
        game.Command = "PD";
        game.ExpandAbbrev();
        Assert.Equal("POLUDNIE", game.Command);
        game.Command = "W";
        game.ExpandAbbrev();
        Assert.Equal("WSCHOD", game.Command);
        game.Command = "Z";
        game.ExpandAbbrev();
        Assert.Equal("ZACHOD", game.Command);
        game.Command = "G";
        game.ExpandAbbrev();
        Assert.Equal("GORA", game.Command);
        game.Command = "D";
        game.ExpandAbbrev();
        Assert.Equal("DOL", game.Command);
        game.Command = "E";
        game.ExpandAbbrev();
        Assert.Equal("EXIT", game.Command);
        game.Command = "M";
        game.ExpandAbbrev();
        Assert.Equal("MODE", game.Command);
        game.Command = "POLNOC";
        game.ExpandAbbrev();
        Assert.Equal("POLNOC", game.Command);
    }

    [Fact]
    public void CommandIgnoresExtraWhitespace() {
        game.Command = "  PATRZ   PLAKAT  ";
        Assert.Equal("PATRZ PLAKAT", game.Command);
        Assert.Equal("PATRZ ", game.Verb);
        Assert.Equal("PLAKAT", game.Rest);
        Assert.True(game.Matches("PATRZ PLAKAT"));

        game.Command = "  EXIT  ";
        Assert.Equal("EXIT", game.Command);
        Assert.Equal("EXIT", game.Verb);
        Assert.Equal("", game.Rest);
        Assert.True(game.Matches("EXIT"));

        game.Command = "  PN  ";
        game.ExpandAbbrev();
        Assert.Equal("POLNOC", game.Command);
    }

    [Fact]
    public void TrainSila() {
        game.Player.Stats[TStat.Strength] = 10;
        game.Player.Stats[TStat.MaxStrength] = 20;
        game.Player.Practice = 10;
        game.Command = "TRENUJ SILA";
        game.HandleTrain();
        Assert.Equal(11, game.Player.Stats[TStat.Strength]);
        Assert.Equal(7, game.Player.Practice);
    }

    [Fact]
    public void TrainZrecznosc() {
        game.Player.Stats[TStat.Dexterity] = 10;
        game.Player.Stats[TStat.MaxDexterity] = 20;
        game.Player.Practice = 10;
        game.Command = "TRENUJ ZRECZNOSC";
        game.HandleTrain();
        Assert.Equal(11, game.Player.Stats[TStat.Dexterity]);
        Assert.Equal(8, game.Player.Practice);
    }

    [Fact]
    public void TrainMadrosc() {
        game.Player.Stats[TStat.Wisdom] = 10;
        game.Player.Stats[TStat.MaxWisdom] = 20;
        game.Player.Practice = 10;
        game.Command = "TRENUJ MADROSC";
        game.HandleTrain();
        Assert.Equal(11, game.Player.Stats[TStat.Wisdom]);
        Assert.Equal(7, game.Player.Practice);
    }

    [Fact]
    public void TrainNeedsPracticePoints() {
        game.Player.Stats[TStat.Strength] = 10;
        game.Player.Stats[TStat.MaxStrength] = 20;
        game.Player.Practice = 2;
        game.Command = "TRENUJ SILA";
        game.HandleTrain();
        Assert.Equal(10, game.Player.Stats[TStat.Strength]);
        Assert.Equal(2, game.Player.Practice);
    }

    [Fact]
    public void TrainCapsAtMaximum() {
        game.Player.Stats[TStat.Strength] = 20;
        game.Player.Stats[TStat.MaxStrength] = 20;
        game.Player.Practice = 10;
        game.Command = "TRENUJ SILA";
        game.HandleTrain();
        Assert.Equal(20, game.Player.Stats[TStat.Strength]);
        Assert.Equal(10, game.Player.Practice);
    }

    [Fact]
    public void PotworyRanges() {
        UnitsSeed.Reseed(game);
        game.SpawnMonsters();
        List<TMonster> beasts = [
            TMonster.Bacterium, TMonster.Snail, TMonster.Beetle,
            TMonster.Cockroach, TMonster.Ant, TMonster.Spider,
            TMonster.Boar, TMonster.Fox, TMonster.Rat,
            TMonster.Partridge, TMonster.Hare, TMonster.Mongrel,
            TMonster.Eagle, TMonster.Deer, TMonster.Elephant,
            TMonster.Lion, TMonster.Giraffe, TMonster.Camel,
            TMonster.Ostrich, TMonster.Boa, TMonster.Wolf,
            TMonster.Bison, TMonster.Panther, TMonster.Gladiator,
            TMonster.Warrior, TMonster.Tamer,
        ];
        foreach (TMonster m in beasts) {
            Assert.InRange(game.World.Monsters[m], 33, 57);
        }

        Assert.InRange(game.World.Monsters[TMonster.Kid], 61, 66);
        Assert.InRange(game.World.Monsters[TMonster.Reporter], 61, 66);
        List<TMonster> maniacs = [
            TMonster.Lunatic, TMonster.Listener, TMonster.Fan,
            TMonster.Man, TMonster.OldMan,
        ];
        foreach (TMonster m in maniacs) {
            Assert.InRange(game.World.Monsters[m], 61, 67);
        }

        List<TMonster> heavies = [
            TMonster.Policeman, TMonster.Bouncer, TMonster.Gorilla,
        ];
        foreach (TMonster m in heavies) {
            Assert.InRange(game.World.Monsters[m], 61, 68);
        }

        List<TMonster> musicians = [
            TMonster.Guitarist, TMonster.Drummer, TMonster.Keyboardist,
            TMonster.Liroy,
        ];
        foreach (TMonster m in musicians) {
            Assert.InRange(game.World.Monsters[m], 70, 72);
        }

        Assert.Equal(67, game.World.Monsters[TMonster.Minibarman]);
        Assert.Equal(67, game.World.Monsters[TMonster.Fatso]);
        Assert.Equal(69, game.World.Monsters[TMonster.DJ]);
        Assert.Equal(76, game.World.Monsters[TMonster.Pedal]);
        Assert.Equal(76, game.World.Monsters[TMonster.Couple]);
        Assert.Equal(76, game.World.Monsters[TMonster.Maciek]);
        Assert.True(game.World.Door);
        Assert.True(game.World.OldElf);
        List<TMonster> dogs = [
            TMonster.Dachshund, TMonster.Shepherd, TMonster.Pooch,
            TMonster.Spaniel, TMonster.Poodle, TMonster.Cabman,
            TMonster.Vendor, TMonster.Sweeper, TMonster.Drunkard,
            TMonster.SickMan,
        ];
        foreach (TMonster m in dogs) {
            Assert.InRange(game.World.Monsters[m], 20, 26);
        }

        List<TMonster> flowers = [
            TMonster.Sorrel, TMonster.Daisy, TMonster.Clover,
            TMonster.SowThistle, TMonster.Dandelion, TMonster.Rose,
            TMonster.Thistle, TMonster.Raspberry,
        ];
        foreach (TMonster m in flowers) {
            Assert.InRange(game.World.Monsters[m], 77, 83);
        }

        List<TMonster> berries = [
            TMonster.Gooseberry, TMonster.Blackberry, TMonster.Grass,
            TMonster.Duncan,
        ];
        foreach (TMonster m in berries) {
            Assert.InRange(game.World.Monsters[m], 77, 84);
        }
    }

    [Fact]
    public void TarczaAppliesShield() {
        UnitsSeed.Reseed(game);
        game.Combat.Damage = 50;
        game.Gear.Values[TGearItem.WornCount] = 30;
        game.Gear.Values[TGearItem.Armor] = 100;
        game.ShieldBlock();
        Assert.Equal(20, game.Combat.Damage);
    }

    [Fact]
    public void TarczaClampsAtZero() {
        UnitsSeed.Reseed(game);
        game.Combat.Damage = 10;
        game.Gear.Values[TGearItem.WornCount] = 30;
        game.Gear.Values[TGearItem.Armor] = 100;
        game.ShieldBlock();
        Assert.Equal(0, game.Combat.Damage);
    }
}
