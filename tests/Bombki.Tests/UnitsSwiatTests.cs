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
    public void NettleThroneReturnsToCaveMouth() {
        var throne = Rooms.RoomDefs.Single(room => room.number == Rooms.RoomNettleThrone);
        Assert.Contains(throne.moves, move => move.cmd == Texts.CmdPolnoc && move.dest == Rooms.RoomCaveMouth);
        Assert.DoesNotContain(throne.moves, move => move.cmd == Texts.CmdPolodnie && move.dest == Rooms.RoomCaveMouth);
    }

    static string Opposite(string cmd) =>
        cmd switch {
            Texts.CmdPolnoc => Texts.CmdPolodnie,
            Texts.CmdPolodnie => Texts.CmdPolnoc,
            Texts.CmdWschod => Texts.CmdZachod,
            Texts.CmdZachod => Texts.CmdWschod,
            Texts.CmdGora => Texts.CmdDol,
            Texts.CmdDol => Texts.CmdGora,
            _ => "",
        };

    static TMazeDir Opposite(TMazeDir dir) =>
        dir switch {
            TMazeDir.mdPolnoc => TMazeDir.mdPolodnie,
            TMazeDir.mdPolodnie => TMazeDir.mdPolnoc,
            TMazeDir.mdWschod => TMazeDir.mdZachod,
            TMazeDir.mdZachod => TMazeDir.mdWschod,
            _ => throw new ArgumentOutOfRangeException(nameof(dir)),
        };

    [Fact]
    public void RoomMovesAreTwoWay() {
        var rooms = Rooms.RoomDefs.ToDictionary(room => room.number);
        foreach (var room in rooms.Values) {
            foreach (var move in room.moves) {
                if ((room.number == Rooms.RoomArenaEntrance) && (move.dest == Rooms.RoomArena33)) {
                    continue;
                }
                string reverse = Opposite(move.cmd);
                Assert.Contains(rooms[move.dest].moves,
                    back => (back.cmd == reverse) && (back.dest == room.number));
            }
        }

        var arena = MazeMoves.Single(move => move.room == Rooms.RoomArena33);
        Assert.Contains(TMazeDir.mdPolodnie, arena.exits);
        Assert.Equal(Rooms.RoomArenaEntrance,
            Rooms.RoomInDirection(Rooms.RoomArena33, MazeDirectionCommand(TMazeDir.mdPolodnie)));

        var mazeRooms = MazeMoves.ToDictionary(move => move.room);
        foreach (var maze in mazeRooms.Values) {
            foreach (TMazeDir dir in maze.exits) {
                int? destination = Rooms.RoomInDirection(maze.room, MazeDirectionCommand(dir));
                Assert.NotNull(destination);
                if (destination == Rooms.RoomArenaEntrance) {
                    Assert.Equal(Rooms.RoomArena33, maze.room);
                    Assert.Equal(TMazeDir.mdPolodnie, dir);
                    continue;
                }

                Assert.Contains(Opposite(dir), mazeRooms[destination.Value].exits);
            }
        }
    }

    [Fact]
    public void RoomNamesCoverConnectivity() {
        var rooms = Rooms.RoomDefs;
        Assert.All(rooms, room => Assert.False(string.IsNullOrWhiteSpace(room.name), $"room {room.number} has no name"));
        Assert.Equal(rooms.Count, rooms.Select(room => room.name).Distinct().Count());
        Assert.All(rooms, room => Assert.All(room.moves,
            move => Assert.Contains(rooms, target => target.number == move.dest)));
        Assert.All(rooms, room => Assert.All(room.moves,
            move => Assert.Equal(move.dest, Rooms.RoomInDirection(room.number, move.cmd))));
        Assert.All(MazeMoves, maze => Assert.All(maze.exits, dir => {
            int? target = Rooms.RoomInDirection(maze.room, MazeDirectionCommand(dir));
            Assert.NotNull(target);
            Assert.Contains(rooms, room => room.number == target);
        }));
    }

    [Fact]
    public void CageRewardConnectsEastOfSchoolLanding() {
        var landing = Rooms.RoomDefs.Single(room => room.number == Rooms.RoomSchoolLanding);
        var reward = Rooms.RoomDefs.Single(room => room.number == Rooms.RoomCageReward);
        var hugeCage = Rooms.RoomDefs.Single(room => room.number == Rooms.RoomCageHuge);

        Assert.Equal(new Rooms.TCoordinate(-2, -1, 1), reward.position);
        Assert.Contains(landing.moves, move => move.cmd == Texts.CmdWschod && move.dest == reward.number);
        Assert.Contains(reward.moves, move => move.cmd == Texts.CmdZachod && move.dest == landing.number);
        Assert.DoesNotContain(hugeCage.moves, move => move.cmd == Texts.CmdGora);
    }

    [Fact]
    public void MapCoordinatesAreUniqueAndPartitionLayoutIsFixed() {
        var rooms = Rooms.RoomDefs;
        Assert.Equal(rooms.Count, rooms.Select(room => room.position).Distinct().Count());

        Dictionary<int, Rooms.TCoordinate> fixedCells = new() {
            [Rooms.RoomCityCentre] = new(0, 0, 0),
            [Rooms.RoomSchoolHall] = new(-2, -1, 0),
            [Rooms.RoomSchoolGate] = new(-1, -1, 0),
            [Rooms.RoomArenaEntrance] = new(-1, 1, 1),
            [Rooms.RoomArenaGate] = new(-1, 0, 1),
            [Rooms.RoomEntertainmentCave] = new(-2, 1, -1),
            [Rooms.RoomConcertGate] = new(-2, 1, 0),
            [Rooms.RoomCageReward] = new(-2, -1, 1),
            [Rooms.RoomDarkStreet] = new(1, 0, 1),
            [Rooms.RoomIvyGate] = new(1, 0, 0),
            [Rooms.RoomRoadsGate] = new(0, 3, 0),
            [Rooms.RoomQuestMaster] = new(0, 4, 0),
        };

        Assert.All(fixedCells, entry =>
            Assert.Equal(entry.Value, rooms.Single(room => room.number == entry.Key).position));
        Assert.Equal(Rooms.RoomFutureRoad, Rooms.RoomInDirection(Rooms.RoomQuestMaster, Texts.CmdWschod));
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
        Assert.Equal(1, Bag.Count);
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
        Bag.Count = 1;
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
        Player.MaxEnergy = 55;
        Bag.Permit = 0;
        Player.Experience = 100;
        Bag.Count = 1;
        PokojAt("ZAKONCZ QUEST|WYJSCIE", 100);
        Assert.Equal(0, World.Quest);
        Assert.Equal(0, Bag.Items[TBagItem.Diploma]);
        Assert.Equal(50, Player.MaxEnergy);
        Assert.Equal(1, Bag.Permit);
        Assert.Equal(350, Player.Experience);
        Assert.Equal(1, Bag.Count);
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
        Player.Stats[TStat.Wisdom] = 6;
        Player.Stats[TStat.MaxWisdom] = 10;
        PokojAt("ZAKONCZ QUEST|WYJSCIE", 100);
        Assert.Equal(0, World.Quest);
        Assert.Equal(0, Bag.Items[TBagItem.Pipe]);
        Assert.Equal(1, Player.Practice);
        Assert.Equal(1, Bag.Permit);
        Assert.Equal(525, Player.Experience);
        Assert.Equal(1, Bag.Count);
        Assert.Equal(5, Player.Stats[TStat.Wisdom]);
        Player.Stats[TStat.Wisdom] = 0;
        Player.Stats[TStat.MaxWisdom] = 0;
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
