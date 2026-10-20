using static Game;

namespace Bombki.Tests;

public class UnitsSwiatTests {
    static TestStdin FeedStdin(string lines) =>
        TestStdin.Feed(lines);

    static void RoomSetup() {
        Session.ExitRequested = false;
        UnitsSeed.Reseed();
        Player.Energy = 30000;
        Player.Experience = 100;
        Player.Stats[TStat.Strength] = 127;
        Player.Stats[TStat.Dexterity] = 127;
        Combat.Fled = false;
        Skills.Values[TSkill.Fleeing] = 0;
        Skills.Values[TSkill.FleeEnergy] = 0;
        Player.Mana = 0;
        Skills.Values[TSkill.Parry] = 0;
        Combat.FireballCharges = 0;
        Combat.PoisonCharges = 0;
        Combat.PoisonRounds = 0;
        Skills.Values[TSkill.Cooking] = 0;
        Skills.Values[TSkill.Kick] = 0;
        Skills.Values[TSkill.KickMinMana] = 0;
        Skills.Values[TSkill.KickMaxEnergy] = 0;
        Gear.Values[TGearItem.Armor] = 0;
        Gear.Values[TGearItem.WornCount] = 0;
        Gear.Values[TGearItem.LuckRoll] = 0;
        Player.MaxEnergy = 50;
        World.Monsters[TMonster.CageStrong] = 0;
        Bag.Count = 0;
        World.Quest = 0;
        World.QuestKills = 0;
        World.LiroyKilled = false;
        Player.Money = 0;
        Bag.Permit = 0;
        Bag.Items[TBagItem.Diploma] = 0;
        Bag.Items[TBagItem.Pipe] = 0;
        foreach (TBagItem item in Enum.GetValues<TBagItem>()) {
            Bag.ItemRooms[item].Clear();
        }
        Player.Practice = 1;
    }

    static void PokojAt(string cmds, int room) {
        Session.ExitRequested = false;
        Session.Room = room;
        using (FeedStdin(cmds)) {
            Rooms.DispatchRoom();
        }
    }

    [Fact]
    public void Pokoj5Poster() {
        RoomSetup();
        Session.Room = 5;
        using (FeedStdin("EXIT|PATRZ PLAKAT|WYJSCIE")) {
            Rooms.DispatchRoom();
        }

        Assert.Equal(5, Session.Room);
    }

    [Fact]
    public void Pokoj1Poster() {
        RoomSetup();
        Session.Room = 1;
        using (FeedStdin("EXIT|PATRZ PLAKAT|WYJSCIE")) {
            Rooms.DispatchRoom();
        }

        Assert.Equal(1, Session.Room);
    }

    [Fact]
    public void Pokoj4Poster() {
        RoomSetup();
        PokojAt("EXIT|PATRZ AFISZ|WYJSCIE", 4);
        Assert.Equal(4, Session.Room);
    }

    [Fact]
    public void Pokoj13Loot() {
        RoomSetup();
        Bag.Items[TBagItem.OldSword] = 0;
        Bag.Items[TBagItem.SmallShield] = 0;
        Bag.Items[TBagItem.Heart] = 0;
        bool sawSword = false, sawShield = false, sawHeart = false;
        for (int i = 1; i <= 80; i++) {
            Player.Energy = 30000;
            Player.Experience = 100;
            Player.Stats[TStat.Strength] = 127;
            Player.Stats[TStat.Dexterity] = 127;
            Combat.Fled = false;
            Skills.Values[TSkill.Fleeing] = 0;
            Skills.Values[TSkill.FleeEnergy] = 0;
            Player.Mana = 0;
            Skills.Values[TSkill.Parry] = 0;
            Combat.FireballCharges = 0;
            Combat.PoisonCharges = 0;
            Combat.PoisonRounds = 0;
            Skills.Values[TSkill.Cooking] = 0;
            Skills.Values[TSkill.Kick] = 0;
            Skills.Values[TSkill.KickMinMana] = 0;
            Skills.Values[TSkill.KickMaxEnergy] = 0;
            Gear.Values[TGearItem.LuckRoll] = 0;
            World.Monsters[TMonster.CageStrong] = 13;
            PokojAt("ZABIJ POTWOR|WYJSCIE", 13);
            if (Bag.HasGroundItem(TBagItem.OldSword)) {
                sawSword = true;
            }

            if (Bag.HasGroundItem(TBagItem.SmallShield)) {
                sawShield = true;
            }

            if (Bag.HasGroundItem(TBagItem.Heart)) {
                sawHeart = true;
            }

            if (sawSword && sawShield && sawHeart) {
                break;
            }
        }
        Assert.True(sawSword);
        Assert.True(sawShield);
        Assert.True(sawHeart);
    }

    [Fact]
    public void Pokoj13Flee() {
        RoomSetup();
        World.Monsters[TMonster.CageStrong] = 13;
        Player.Energy = 50;
        Skills.Values[TSkill.FleeEnergy] = 100;
        Player.Mana = 20;
        Skills.Values[TSkill.Fleeing] = 100;
        PokojAt("ZABIJ POTWOR|WYJSCIE", 13);
        Assert.Equal(13, World.Monsters[TMonster.CageStrong]);
        Assert.False(Combat.Fled);
    }

    [Fact]
    public void Pokoj13Exits() {
        RoomSetup();
        World.Monsters[TMonster.CageStrong] = 0;
        PokojAt("EXIT|MODE|WYJSCIE", 13);
        Assert.Equal(13, Session.Room);
    }

    [Fact]
    public void PokojeSmallRooms() {
        List<int> rooms = [6, 8, 7, 10];
        foreach (int room in rooms) {
            RoomSetup();
            PokojAt("EXIT|MODE|WYJSCIE", room);
            Assert.Equal(room, Session.Room);
            RoomSetup();
            PokojAt("WYJSCIE", room);
            Assert.Equal(room, Session.Room);
        }
    }

    [Fact]
    public void Pokoj11Poster() {
        RoomSetup();
        PokojAt("EXIT|PATRZ PLAKAT|WYJSCIE", 11);
        Assert.Equal(11, Session.Room);
    }

    [Fact]
    public void Pokoj30Exits() {
        RoomSetup();
        PokojAt("EXIT|WYJSCIE", 30);
        Assert.Equal(30, Session.Room);
    }

    [Fact]
    public void Pokoj60Exits() {
        RoomSetup();
        PokojAt("EXIT|WYJSCIE", 60);
        Assert.Equal(60, Session.Room);
    }

    [Fact]
    public void Pokoj75Exits() {
        RoomSetup();
        PokojAt("EXIT|WYJSCIE", 75);
        Assert.Equal(75, Session.Room);
    }

    [Fact]
    public void Pokoj83Exits() {
        RoomSetup();
        PokojAt("EXIT|WYJSCIE", 83);
        Assert.Equal(83, Session.Room);
    }

    [Fact]
    public void RoomNamesCoverConnectivity() {
        var rooms = Rooms.RoomDefs;
        Assert.All(rooms, room => Assert.False(string.IsNullOrWhiteSpace(room.name), $"room {room.number} has no name"));
        Assert.Equal(rooms.Count, rooms.Select(room => room.name).Distinct().Count());
        Assert.All(rooms, room => Assert.All(room.moves,
            move => Assert.Contains(rooms, target => target.number == move.dest)));
        List<int> mazeTargets = [
            .. MazeMoves
                .Skip(1)
                .SelectMany(maze => Enum.GetValues<TMazeDir>().Select(dir => maze.dest[dir])),
        ];
        Assert.All(mazeTargets, target => Assert.Contains(rooms, room => room.number == target));
    }

    [Fact]
    public void Pokoj100QuestMaster() {
        RoomSetup();
        Player.Money = 10000;
        Bag.Permit = 0;
        PokojAt("EXIT|LISTA|ZABIJ QUEST-MASTER|ZACZNIJ QUEST 1|WYJSCIE", 100);
        Assert.Equal(1, World.Quest);
        Assert.Equal(75, World.QuestKills);
        Assert.Equal(10000, Player.Money);
        Assert.Equal(1, Bag.Permit);
    }

    [Fact]
    public void Pokoj100BlockedRoad() {
        RoomSetup();
        Bag.Permit = 0;
        PokojAt("ZACHOD|WYJSCIE", 100);
        Assert.Equal(100, Session.Room);
    }

    [Fact]
    public void Pokoj100OpenRoad() {
        RoomSetup();
        Bag.Permit = 1;
        PokojAt("EXIT|ZACHOD", 100);
        Assert.Equal(101, Session.Room);
        Assert.Equal(101, Session.OuterRoom);
    }

    [Fact]
    public void Pokoj100QuestStart() {
        RoomSetup();
        Player.Money = 10000;
        PokojAt("ZACZNIJ QUEST 2|ZACZNIJ QUEST 3|WYJSCIE", 100);
        Assert.Equal(2, World.Quest);
        Assert.Equal(50, World.QuestKills);
        Assert.Equal(10000, Player.Money);
    }

    [Fact]
    public void Pokoj100QuestStartHard() {
        RoomSetup();
        PokojAt("ZACZNIJ QUEST 3|WYJSCIE", 100);
        Assert.Equal(3, World.Quest);
        Assert.Equal(50, World.QuestKills);
    }

    [Fact]
    public void Pokoj100QuestStartCountsDeadLiroy() {
        RoomSetup();
        World.Monsters[TMonster.Liroy] = 0;
        PokojAt("ZACZNIJ QUEST 2|WYJSCIE", 100);
        Assert.True(World.LiroyKilled);
    }

    [Fact]
    public void Pokoj100QuestStartRequiresLiveLiroy() {
        RoomSetup();
        World.Monsters[TMonster.Liroy] = Rooms.RoomStage1;
        World.LiroyKilled = true;
        PokojAt("ZACZNIJ QUEST 3|WYJSCIE", 100);
        Assert.False(World.LiroyKilled);
    }

    [Fact]
    public void Pokoj100FinishNoQuest() {
        RoomSetup();
        Bag.Permit = 0;
        Player.Experience = 100;
        PokojAt("ZAKONCZ QUEST|WYJSCIE", 100);
        Assert.Equal(0, World.Quest);
        Assert.Equal(100, Player.Experience);
        Assert.Equal(0, Bag.Permit);
    }

    [Fact]
    public void Pokoj100FinishEasy() {
        RoomSetup();
        World.Quest = 1;
        World.QuestKills = 0;
        Bag.Permit = 0;
        Player.Experience = 100;
        PokojAt("ZAKONCZ QUEST|WYJSCIE", 100);
        Assert.Equal(0, World.Quest);
        Assert.Equal(0, World.QuestKills);
        Assert.Equal(1, Bag.Permit);
        Assert.Equal(200, Player.Experience);
        Assert.Equal(1, Bag.Count);
    }

    [Fact]
    public void Pokoj100FinishMedium() {
        RoomSetup();
        World.Quest = 2;
        World.QuestKills = 0;
        World.LiroyKilled = true;
        Bag.Items[TBagItem.Diploma] = 1;
        Player.MaxEnergy = 50;
        Bag.Permit = 0;
        Player.Experience = 100;
        Bag.Count = 1;
        PokojAt("ZAKONCZ QUEST|WYJSCIE", 100);
        Assert.Equal(0, World.Quest);
        Assert.Equal(0, Bag.Items[TBagItem.Diploma]);
        Assert.Equal(45, Player.MaxEnergy);
        Assert.Equal(1, Bag.Permit);
        Assert.Equal(350, Player.Experience);
        Assert.Equal(0, Bag.Count);
    }

    [Fact]
    public void Pokoj100FinishNeedsLiroy() {
        RoomSetup();
        World.Quest = 2;
        World.QuestKills = 0;
        World.LiroyKilled = false;
        Bag.Items[TBagItem.Diploma] = 1;
        Bag.Permit = 0;
        Player.Experience = 100;
        PokojAt("ZAKONCZ QUEST|WYJSCIE", 100);
        Assert.Equal(2, World.Quest);
        Assert.Equal(100, Player.Experience);
        Assert.Equal(0, Bag.Permit);
    }

    [Fact]
    public void Pokoj100FinishHard() {
        RoomSetup();
        World.Quest = 3;
        World.QuestKills = 0;
        World.LiroyKilled = true;
        Bag.Items[TBagItem.Pipe] = 1;
        Player.Practice = 1;
        Bag.Permit = 0;
        Player.Experience = 100;
        Bag.Count = 1;
        Player.Stats[TStat.Wisdom] = 5;
        PokojAt("ZAKONCZ QUEST|WYJSCIE", 100);
        Assert.Equal(0, World.Quest);
        Assert.Equal(0, Bag.Items[TBagItem.Pipe]);
        Assert.Equal(1, Player.Practice);
        Assert.Equal(1, Bag.Permit);
        Assert.Equal(525, Player.Experience);
        Assert.Equal(0, Bag.Count);
        Assert.Equal(4, Player.Stats[TStat.Wisdom]);
    }

    [Fact]
    public void LiroyKillCountsOnce() {
        RoomSetup();
        World.Quest = 2;
        World.QuestKills = 50;
        Session.Room = Rooms.RoomStage1;
        World.Monsters[TMonster.Liroy] = Rooms.RoomStage1;
        PokojAt("ZABIJ LIROY|WYJSCIE", Rooms.RoomStage1);
        Assert.Equal(49, World.QuestKills);
        Assert.True(World.LiroyKilled);
        Assert.Equal(0, World.Monsters[TMonster.Liroy]);
    }

    [Fact]
    public void StaruchTradeLeavesYouInTheCity() {
        RoomSetup();
        Session.Room = Rooms.RoomHut;
        Bag.Items[TBagItem.Sausage] = 4;
        Bag.Items[TBagItem.ReturnScroll] = 0;
        Bag.Items[TBagItem.Beer] = 0;
        Bag.Count = 4;
        Player.Money = 200;
        World.OldElf = false;
        using (FeedStdin("ROZMAWIAJ STARUCH")) {
            Rooms.DispatchRoom();
        }

        Assert.Equal(Rooms.RoomCityCentre, Session.Room);
        Assert.Equal(1, Bag.Items[TBagItem.ReturnScroll]);
        Assert.Equal(1, Bag.Items[TBagItem.Beer]);
        Assert.Equal(0, Bag.Items[TBagItem.Sausage]);
        Assert.Equal(0, Player.Money);
        Assert.Equal(2, Bag.Count);
    }

    [Fact]
    public void KamienieMogaZabic() {
        RoomSetup();
        Player.MaxEnergy = 50;
        Player.Energy = 10;
        Player.Stats[TStat.Dexterity] = 10;
        for (int i = 1; i <= 20; i++) {
            PokojAt("PATRZ|POLNOC", 87);
            if (Session.Room == Texts.DeadRoom) {
                break;
            }
        }

        Assert.Equal(Texts.DeadRoom, Session.Room);
        Assert.Equal(0, Player.Energy);
    }

    [Fact]
    public void StudniaMozeZabic() {
        RoomSetup();
        Player.MaxEnergy = 50;
        Player.Energy = 50;
        for (int i = 1; i <= 100; i++) {
            PokojAt("PATRZ STUDNIA|POLNOC", 80);
            if (Session.Room == Texts.DeadRoom) {
                break;
            }
        }

        Assert.Equal(Texts.DeadRoom, Session.Room);
        Assert.Equal(0, Player.Energy);
    }
}
