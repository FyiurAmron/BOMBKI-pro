
namespace Bombki.Tests;

public class UnitsSwiatTests {
    readonly Game game = new();
    TestStdin FeedStdin(string lines) =>
        TestStdin.Feed(lines);

    void RoomSetup() {
        game.Session.ExitRequested = false;
        UnitsSeed.Reseed(game);
        game.Player.Energy = 30000;
        game.Player.Experience = 100;
        game.Player.Stats[TStat.Strength] = 127;
        game.Player.Stats[TStat.Dexterity] = 127;
        game.Combat.Fled = false;
        game.Skills.Values[TSkill.Fleeing] = 0;
        game.Skills.Values[TSkill.FleeEnergy] = 0;
        game.Player.Mana = 0;
        game.Skills.Values[TSkill.Parry] = 0;
        game.Combat.FireballCharges = 0;
        game.Combat.PoisonCharges = 0;
        game.Combat.PoisonRounds = 0;
        game.Skills.Values[TSkill.Cooking] = 0;
        game.Skills.Values[TSkill.Kick] = 0;
        game.Skills.Values[TSkill.KickMinMana] = 0;
        game.Skills.Values[TSkill.KickMaxEnergy] = 0;
        game.Gear.Values[TGearItem.Armor] = 0;
        game.Gear.Values[TGearItem.WornCount] = 0;
        game.Gear.Values[TGearItem.LuckRoll] = 0;
        game.Player.MaxEnergy = 50;
        game.World.Monsters[TMonster.CageStrong] = 0;
        game.Bag.Count = 0;
        game.World.Quest = 0;
        game.World.QuestKills = 0;
        game.World.LiroyKilled = false;
        game.Player.Money = 0;
        game.Bag.Permit = 0;
        game.Bag.Items[TBagItem.Diploma] = 0;
        game.Bag.Items[TBagItem.Pipe] = 0;
        foreach (TBagItem item in Enum.GetValues<TBagItem>()) {
            game.Bag.ItemRooms[item].Clear();
        }
        game.Player.Practice = 1;
    }

    void PokojAt(string cmds, int room) {
        game.Session.ExitRequested = false;
        game.Session.Room = room;
        using (FeedStdin(cmds)) {
            game.World.DispatchRoom();
        }
    }

    [Fact]
    public void Pokoj5Poster() {
        RoomSetup();
        game.Session.Room = 5;
        using (FeedStdin("EXIT|PATRZ PLAKAT|WYJSCIE")) {
            game.World.DispatchRoom();
        }

        Assert.Equal(5, game.Session.Room);
    }

    [Fact]
    public void Pokoj1Poster() {
        RoomSetup();
        game.Session.Room = 1;
        using (FeedStdin("EXIT|PATRZ PLAKAT|WYJSCIE")) {
            game.World.DispatchRoom();
        }

        Assert.Equal(1, game.Session.Room);
    }

    [Fact]
    public void Pokoj4Poster() {
        RoomSetup();
        PokojAt("EXIT|PATRZ AFISZ|WYJSCIE", 4);
        Assert.Equal(4, game.Session.Room);
    }

    [Fact]
    public void Pokoj13Loot() {
        RoomSetup();
        game.Bag.Items[TBagItem.OldSword] = 0;
        game.Bag.Items[TBagItem.SmallShield] = 0;
        game.Bag.Items[TBagItem.Heart] = 0;
        bool sawSword = false, sawShield = false, sawHeart = false;
        for (int i = 1; i <= 80; i++) {
            game.Player.Energy = 30000;
            game.Player.Experience = 100;
            game.Player.Stats[TStat.Strength] = 127;
            game.Player.Stats[TStat.Dexterity] = 127;
            game.Combat.Fled = false;
            game.Skills.Values[TSkill.Fleeing] = 0;
            game.Skills.Values[TSkill.FleeEnergy] = 0;
            game.Player.Mana = 0;
            game.Skills.Values[TSkill.Parry] = 0;
            game.Combat.FireballCharges = 0;
            game.Combat.PoisonCharges = 0;
            game.Combat.PoisonRounds = 0;
            game.Skills.Values[TSkill.Cooking] = 0;
            game.Skills.Values[TSkill.Kick] = 0;
            game.Skills.Values[TSkill.KickMinMana] = 0;
            game.Skills.Values[TSkill.KickMaxEnergy] = 0;
            game.Gear.Values[TGearItem.LuckRoll] = 0;
            game.World.Monsters[TMonster.CageStrong] = 13;
            PokojAt("ZABIJ POTWOR|WYJSCIE", 13);
            if (game.Bag.HasGroundItem(TBagItem.OldSword)) {
                sawSword = true;
            }

            if (game.Bag.HasGroundItem(TBagItem.SmallShield)) {
                sawShield = true;
            }

            if (game.Bag.HasGroundItem(TBagItem.Heart)) {
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
        game.World.Monsters[TMonster.CageStrong] = 13;
        game.Player.Energy = 50;
        game.Skills.Values[TSkill.FleeEnergy] = 100;
        game.Player.Mana = 20;
        game.Skills.Values[TSkill.Fleeing] = 100;
        PokojAt("ZABIJ POTWOR|WYJSCIE", 13);
        Assert.Equal(13, game.World.Monsters[TMonster.CageStrong]);
        Assert.False(game.Combat.Fled);
    }

    [Fact]
    public void Pokoj13Exits() {
        RoomSetup();
        game.World.Monsters[TMonster.CageStrong] = 0;
        PokojAt("EXIT|MODE|WYJSCIE", 13);
        Assert.Equal(13, game.Session.Room);
    }

    [Fact]
    public void PokojeSmallRooms() {
        List<int> rooms = [6, 8, 7, 10];
        foreach (int room in rooms) {
            RoomSetup();
            PokojAt("EXIT|MODE|WYJSCIE", room);
            Assert.Equal(room, game.Session.Room);
            RoomSetup();
            PokojAt("WYJSCIE", room);
            Assert.Equal(room, game.Session.Room);
        }
    }

    [Fact]
    public void Pokoj11Poster() {
        RoomSetup();
        PokojAt("EXIT|PATRZ PLAKAT|WYJSCIE", 11);
        Assert.Equal(11, game.Session.Room);
    }

    [Fact]
    public void Pokoj30Exits() {
        RoomSetup();
        PokojAt("EXIT|WYJSCIE", 30);
        Assert.Equal(30, game.Session.Room);
    }

    [Fact]
    public void Pokoj60Exits() {
        RoomSetup();
        PokojAt("EXIT|WYJSCIE", 60);
        Assert.Equal(60, game.Session.Room);
    }

    [Fact]
    public void Pokoj75Exits() {
        RoomSetup();
        PokojAt("EXIT|WYJSCIE", 75);
        Assert.Equal(75, game.Session.Room);
    }

    [Fact]
    public void Pokoj83Exits() {
        RoomSetup();
        PokojAt("EXIT|WYJSCIE", 83);
        Assert.Equal(83, game.Session.Room);
    }

    [Fact]
    public void NettleThroneReturnsToCaveMouth() {
        var throne = game.World.RoomDefs.Single(room => room.number == Rooms.RoomNettleThrone);
        Assert.Contains(throne.moves, move => move.cmd == Texts.CmdPolnoc && move.dest == Rooms.RoomCaveMouth);
        Assert.DoesNotContain(throne.moves, move => move.cmd == Texts.CmdPolodnie && move.dest == Rooms.RoomCaveMouth);
    }

    string Opposite(string cmd) =>
        cmd switch {
            Texts.CmdPolnoc => Texts.CmdPolodnie,
            Texts.CmdPolodnie => Texts.CmdPolnoc,
            Texts.CmdWschod => Texts.CmdZachod,
            Texts.CmdZachod => Texts.CmdWschod,
            Texts.CmdGora => Texts.CmdDol,
            Texts.CmdDol => Texts.CmdGora,
            _ => "",
        };

    TMazeDir Opposite(TMazeDir dir) =>
        dir switch {
            TMazeDir.mdPolnoc => TMazeDir.mdPolodnie,
            TMazeDir.mdPolodnie => TMazeDir.mdPolnoc,
            TMazeDir.mdWschod => TMazeDir.mdZachod,
            TMazeDir.mdZachod => TMazeDir.mdWschod,
            _ => throw new ArgumentOutOfRangeException(nameof(dir)),
        };

    [Fact]
    public void RoomMovesAreTwoWay() {
        var rooms = game.World.RoomDefs.ToDictionary(room => room.number);
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

        var arena = game.World.MazeMoves.Single(move => move.room == Rooms.RoomArena33);
        Assert.Contains(TMazeDir.mdPolodnie, arena.exits);
        Assert.Equal(Rooms.RoomArenaEntrance,
            game.World.RoomInDirection(Rooms.RoomArena33, game.World.MazeDirectionCommand(TMazeDir.mdPolodnie)));

        var mazeRooms = game.World.MazeMoves.ToDictionary(move => move.room);
        foreach (var maze in mazeRooms.Values) {
            foreach (TMazeDir dir in maze.exits) {
                int? destination = game.World.RoomInDirection(maze.room, game.World.MazeDirectionCommand(dir));
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
        var rooms = game.World.RoomDefs;
        Assert.All(rooms, room => Assert.False(string.IsNullOrWhiteSpace(room.name), $"room {room.number} has no name"));
        Assert.Equal(rooms.Count, rooms.Select(room => room.name).Distinct().Count());
        Assert.All(rooms, room => Assert.All(room.moves,
            move => Assert.Contains(rooms, target => target.number == move.dest)));
        Assert.All(rooms, room => Assert.All(room.moves,
            move => Assert.Equal(move.dest, game.World.RoomInDirection(room.number, move.cmd))));
        Assert.All(game.World.MazeMoves, maze => Assert.All(maze.exits, dir => {
            int? target = game.World.RoomInDirection(maze.room, game.World.MazeDirectionCommand(dir));
            Assert.NotNull(target);
            Assert.Contains(rooms, room => room.number == target);
        }));
    }

    [Fact]
    public void CageRewardConnectsEastOfSchoolLanding() {
        var landing = game.World.RoomDefs.Single(room => room.number == Rooms.RoomSchoolLanding);
        var reward = game.World.RoomDefs.Single(room => room.number == Rooms.RoomCageReward);
        var hugeCage = game.World.RoomDefs.Single(room => room.number == Rooms.RoomCageHuge);

        Assert.Equal(new Rooms.TCoordinate(-2, -1, 1), reward.position);
        Assert.Contains(landing.moves, move => move.cmd == Texts.CmdWschod && move.dest == reward.number);
        Assert.Contains(reward.moves, move => move.cmd == Texts.CmdZachod && move.dest == landing.number);
        Assert.DoesNotContain(hugeCage.moves, move => move.cmd == Texts.CmdGora);
    }

    [Fact]
    public void MapCoordinatesAreUniqueAndPartitionLayoutIsFixed() {
        var rooms = game.World.RoomDefs;
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
        Assert.Equal(Rooms.RoomFutureRoad, game.World.RoomInDirection(Rooms.RoomQuestMaster, Texts.CmdWschod));
    }

    [Fact]
    public void Pokoj100QuestMaster() {
        RoomSetup();
        game.Player.Money = 10000;
        game.Bag.Permit = 0;
        PokojAt("EXIT|LISTA|ZABIJ QUEST-MASTER|ZACZNIJ QUEST 1|WYJSCIE", 100);
        Assert.Equal(1, game.World.Quest);
        Assert.Equal(75, game.World.QuestKills);
        Assert.Equal(10000, game.Player.Money);
        Assert.Equal(1, game.Bag.Permit);
        Assert.Equal(1, game.Bag.Count);
    }

    [Fact]
    public void Pokoj100BlockedRoad() {
        RoomSetup();
        game.Bag.Permit = 0;
        PokojAt("ZACHOD|WYJSCIE", 100);
        Assert.Equal(100, game.Session.Room);
    }

    [Fact]
    public void Pokoj100OpenRoad() {
        RoomSetup();
        game.Bag.Permit = 1;
        game.Bag.Count = 1;
        PokojAt("EXIT|ZACHOD", 100);
        Assert.Equal(101, game.Session.Room);
        Assert.Equal(101, game.Session.OuterRoom);
    }

    [Fact]
    public void Pokoj100QuestStart() {
        RoomSetup();
        game.Player.Money = 10000;
        PokojAt("ZACZNIJ QUEST 2|ZACZNIJ QUEST 3|WYJSCIE", 100);
        Assert.Equal(2, game.World.Quest);
        Assert.Equal(50, game.World.QuestKills);
        Assert.Equal(10000, game.Player.Money);
    }

    [Fact]
    public void Pokoj100QuestStartHard() {
        RoomSetup();
        PokojAt("ZACZNIJ QUEST 3|WYJSCIE", 100);
        Assert.Equal(3, game.World.Quest);
        Assert.Equal(50, game.World.QuestKills);
    }

    [Fact]
    public void Pokoj100QuestStartCountsDeadLiroy() {
        RoomSetup();
        game.World.Monsters[TMonster.Liroy] = 0;
        PokojAt("ZACZNIJ QUEST 2|WYJSCIE", 100);
        Assert.True(game.World.LiroyKilled);
    }

    [Fact]
    public void Pokoj100QuestStartRequiresLiveLiroy() {
        RoomSetup();
        game.World.Monsters[TMonster.Liroy] = Rooms.RoomStage1;
        game.World.LiroyKilled = true;
        PokojAt("ZACZNIJ QUEST 3|WYJSCIE", 100);
        Assert.False(game.World.LiroyKilled);
    }

    [Fact]
    public void Pokoj100FinishNoQuest() {
        RoomSetup();
        game.Bag.Permit = 0;
        game.Player.Experience = 100;
        PokojAt("ZAKONCZ QUEST|WYJSCIE", 100);
        Assert.Equal(0, game.World.Quest);
        Assert.Equal(100, game.Player.Experience);
        Assert.Equal(0, game.Bag.Permit);
    }

    [Fact]
    public void Pokoj100FinishEasy() {
        RoomSetup();
        game.World.Quest = 1;
        game.World.QuestKills = 0;
        game.Bag.Permit = 0;
        game.Player.Experience = 100;
        PokojAt("ZAKONCZ QUEST|WYJSCIE", 100);
        Assert.Equal(0, game.World.Quest);
        Assert.Equal(0, game.World.QuestKills);
        Assert.Equal(1, game.Bag.Permit);
        Assert.Equal(200, game.Player.Experience);
        Assert.Equal(1, game.Bag.Count);
    }

    [Fact]
    public void Pokoj100FinishMedium() {
        RoomSetup();
        game.World.Quest = 2;
        game.World.QuestKills = 0;
        game.World.LiroyKilled = true;
        game.Bag.Items[TBagItem.Diploma] = 1;
        game.Player.MaxEnergy = 55;
        game.Bag.Permit = 0;
        game.Player.Experience = 100;
        game.Bag.Count = 1;
        PokojAt("ZAKONCZ QUEST|WYJSCIE", 100);
        Assert.Equal(0, game.World.Quest);
        Assert.Equal(0, game.Bag.Items[TBagItem.Diploma]);
        Assert.Equal(50, game.Player.MaxEnergy);
        Assert.Equal(1, game.Bag.Permit);
        Assert.Equal(350, game.Player.Experience);
        Assert.Equal(1, game.Bag.Count);
    }

    [Fact]
    public void Pokoj100FinishNeedsLiroy() {
        RoomSetup();
        game.World.Quest = 2;
        game.World.QuestKills = 0;
        game.World.LiroyKilled = false;
        game.Bag.Items[TBagItem.Diploma] = 1;
        game.Bag.Permit = 0;
        game.Player.Experience = 100;
        PokojAt("ZAKONCZ QUEST|WYJSCIE", 100);
        Assert.Equal(2, game.World.Quest);
        Assert.Equal(100, game.Player.Experience);
        Assert.Equal(0, game.Bag.Permit);
    }

    [Fact]
    public void Pokoj100FinishHard() {
        RoomSetup();
        game.World.Quest = 3;
        game.World.QuestKills = 0;
        game.World.LiroyKilled = true;
        game.Bag.Items[TBagItem.Pipe] = 1;
        game.Player.Practice = 1;
        game.Bag.Permit = 0;
        game.Player.Experience = 100;
        game.Bag.Count = 1;
        game.Player.Stats[TStat.Wisdom] = 6;
        game.Player.Stats[TStat.MaxWisdom] = 10;
        PokojAt("ZAKONCZ QUEST|WYJSCIE", 100);
        Assert.Equal(0, game.World.Quest);
        Assert.Equal(0, game.Bag.Items[TBagItem.Pipe]);
        Assert.Equal(1, game.Player.Practice);
        Assert.Equal(1, game.Bag.Permit);
        Assert.Equal(525, game.Player.Experience);
        Assert.Equal(1, game.Bag.Count);
        Assert.Equal(5, game.Player.Stats[TStat.Wisdom]);
        game.Player.Stats[TStat.Wisdom] = 0;
        game.Player.Stats[TStat.MaxWisdom] = 0;
    }

    [Fact]
    public void LiroyKillCountsOnce() {
        RoomSetup();
        game.World.Quest = 2;
        game.World.QuestKills = 50;
        game.Session.Room = Rooms.RoomStage1;
        game.World.Monsters[TMonster.Liroy] = Rooms.RoomStage1;
        PokojAt("ZABIJ LIROY|WYJSCIE", Rooms.RoomStage1);
        Assert.Equal(49, game.World.QuestKills);
        Assert.True(game.World.LiroyKilled);
        Assert.Equal(0, game.World.Monsters[TMonster.Liroy]);
    }

    [Fact]
    public void StaruchTradeLeavesYouInTheCity() {
        RoomSetup();
        game.Session.Room = Rooms.RoomHut;
        game.Bag.Items[TBagItem.Sausage] = 4;
        game.Bag.Items[TBagItem.ReturnScroll] = 0;
        game.Bag.Items[TBagItem.Beer] = 0;
        game.Bag.Count = 4;
        game.Player.Money = 200;
        game.World.OldElf = false;
        using (FeedStdin("ROZMAWIAJ STARUCH")) {
            game.World.DispatchRoom();
        }

        Assert.Equal(Rooms.RoomCityCentre, game.Session.Room);
        Assert.Equal(1, game.Bag.Items[TBagItem.ReturnScroll]);
        Assert.Equal(1, game.Bag.Items[TBagItem.Beer]);
        Assert.Equal(0, game.Bag.Items[TBagItem.Sausage]);
        Assert.Equal(0, game.Player.Money);
        Assert.Equal(2, game.Bag.Count);
    }

    [Fact]
    public void KamienieMogaZabic() {
        RoomSetup();
        game.Player.MaxEnergy = 50;
        game.Player.Energy = 10;
        game.Player.Stats[TStat.Dexterity] = 10;
        for (int i = 1; i <= 20; i++) {
            PokojAt("PATRZ|POLNOC", 87);
            if (game.Session.Room == Rooms.RoomDead) {
                break;
            }
        }

        Assert.Equal(Rooms.RoomDead, game.Session.Room);
        Assert.Equal(0, game.Player.Energy);
    }

    [Fact]
    public void StudniaMozeZabic() {
        RoomSetup();
        game.Player.MaxEnergy = 50;
        game.Player.Energy = 50;
        for (int i = 1; i <= 100; i++) {
            PokojAt("PATRZ STUDNIA|POLNOC", 80);
            if (game.Session.Room == Rooms.RoomDead) {
                break;
            }
        }

        Assert.Equal(Rooms.RoomDead, game.Session.Room);
        Assert.Equal(0, game.Player.Energy);
    }
}
