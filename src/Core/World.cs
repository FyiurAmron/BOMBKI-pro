
using static TMonster;
using static TFoe;
using static TStreetKind;
using static Texts;
using static Rooms;

public class World : TWorld {
    readonly Game game;
    readonly Rooms rooms;

    public World(Game game) {
        this.game = game;
        rooms = new(game);
        foreach (TRoom room in rooms.RoomDefs) {
            room.moves = [.. room.exits.Select(exit => rooms.ResolveMove(room, exit))];
        }
    }

    public List<TRoom> RoomDefs => rooms.RoomDefs;

    public int? RoomInDirection(int number, string cmd) =>
        rooms.RoomInDirection(number, cmd);

    readonly List<(TMazeDir dir, string cmd, string label)> MazeExitLabels = [
        (TMazeDir.mdPolnoc, CmdPolnoc, TxtExitPolnoc),
        (TMazeDir.mdWschod, CmdWschod, TxtExitWschod),
        (TMazeDir.mdPolodnie, CmdPolodnie, TxtExitPolodnie),
        (TMazeDir.mdZachod, CmdZachod, TxtExitZachod),
    ];

    string ExitLabel(string cmd) =>
        cmd switch {
            CmdPolnoc => TxtExitPolnoc,
            CmdPolodnie => TxtExitPolodnie,
            CmdWschod => TxtExitWschod,
            CmdZachod => TxtExitZachod,
            CmdGora => TxtExitGora,
            CmdDol => TxtExitDol,
            _ => TxtEmpty,
        };

    string RoomName(int number) =>
        rooms.RoomIndex.TryGetValue(number, out TRoom? room) ? room.name : TxtEmpty;

    void WriteMazeExits(int number) {
        if (!MazeMoves.TryGetValue(number, out List<TMazeDir>? exits)) {
            return;
        }
        Out.WriteLine(TxtShared_03);
        foreach ((TMazeDir dir, string cmd, string label) in MazeExitLabels) {
            if (exits.Contains(dir) && RoomInDirection(number, cmd) is int destination) {
                Out.Write(label);
                Out.WriteLine(RoomName(destination));
            }
        }
    }

    void WriteExits(TRoom room) {
        if (room.maze) {
            WriteMazeExits(room.number);
            return;
        }
        if (!room.moves.Any()) {
            return;
        }
        Out.WriteLine(TxtShared_03);
        foreach (TMovement move in room.moves) {
            Out.Write(ExitLabel(move.cmd));
            Out.WriteLine(RoomName(move.dest));
        }
    }

    public void DispatchRoom() {
        if (rooms.RoomIndex.TryGetValue(game.Session.Room, out TRoom? room)) {
            RunRoom(room);
        }
    }

    void RunRoom(TRoom room) {
        game.WriteLines(room.intro);
        room.turn?.Invoke(TRoomPhase.Enter);
        do {
            game.ReadCommand();

            if (game.Verb == CmdExit) {
                WriteExits(room);
            }

            if (game.Session.Room == room.number) {
                room.turn?.Invoke(TRoomPhase.Command);
            }

            if (MoveWhenOrQuit(room.moves)) {
                return;
            }

            for (int pi = 0; pi < room.posters.Count; pi++) {
                if (game.Matches(room.posters[pi].cmd)) {
                    game.WriteLines(room.posters[pi].lines);
                }
            }
        } while (game.Session.Room == room.number);
    }
    public void Load(TWorld save) {
        Monsters = save.Monsters;
        Door = save.Door;
        OldElf = save.OldElf;
        LiroyKilled = save.LiroyKilled;
        Quest = save.Quest;
        QuestKills = save.QuestKills;
        DuncanQuest = save.DuncanQuest;
    }

    public void MoveTo(int room) {
        game.Session.Room = room;
        game.Session.OuterRoom = room;
        GroundItemLines();
    }

    public void GroundItemLines() {
        for (int ti = 0; ti < GroundItems.Count; ti++) {
            TGroundItem t = GroundItems[ti];
            if (game.Bag.HasGroundItem(t.item, game.Session.OuterRoom)) {
                Out.WriteLine(t.line);
            }
        }
    }

    public void MoveWhen(List<TMovement> moves) {
        for (int i = 0; i < moves.Count; i++) {
            if (!game.Matches(moves[i].cmd)) {
                continue;
            }
            if (moves[i].predicate?.Invoke() == false) {
                string? blocked = moves[i].blocked;
                if (blocked != null) {
                    Out.WriteLine(blocked);
                }
                continue;
            }
            MoveTo(moves[i].dest);
        }
    }

    public bool MoveWhenOrQuit(List<TMovement> moves) {
        if (game.Verb == CmdWyjscie) {
            game.Session.ExitRequested = true;
            return true;
        }
        MoveWhen(moves);
        return false;
    }

    public void GrantQuestReward(int quest, int exp) {
        Out.WriteLine(TxtRoom100_12);
        Out.WriteLine(TxtQuestReward + exp + TxtSpaceKunsztDashes);
        game.Player.Experience += exp;
        game.World.Quest = 0;
        game.World.QuestKills = 0;
        game.Inventory.AcquirePermit();
        switch (quest) {
            case 1:
                break;
            case 2:
                game.Inventory.LoseItem(TBagItem.Diploma);
                break;
            case 3:
                game.Inventory.LoseItem(TBagItem.Pipe);
                break;
        }
    }

    public void StartQuest(string line, int quest, int kills) {
        if (Quest > 0) {
            Out.WriteLine(TxtRoom100_17);
            return;
        }

        Out.WriteLine(line);
        Quest = quest;
        QuestKills = kills;
        LiroyKilled = Monsters[TMonster.Liroy] == 0;
    }

    public void SpawnAt(List<TSpawnedAt> placements) {
        foreach (TSpawnedAt placement in placements) {
            Monsters[placement.monster] = placement.room;
        }
    }

    public void SpawnGroup(List<TMonster> targets, int spread, int Base) {
        foreach (TMonster target in targets) {
            Monsters[target] = game.Roll(spread) + Base;
        }
    }

    public bool PastLimit(List<TMonster> targets, int limit) =>
        targets.All(target => Monsters[target] > limit);

    public void SpawnMonsters() {
        do {
            SpawnGroup(MonstersOf(TMonster.Bacterium, TMonster.Snail, TMonster.Beetle, TMonster.Cockroach, TMonster.Ant,
                TMonster.Spider, TMonster.Boar, TMonster.Fox, TMonster.Rat, TMonster.Partridge), 38, RoomCityCentre);
        } while (!PastLimit(MonstersOf(TMonster.Bacterium, TMonster.Snail, TMonster.Beetle, TMonster.Cockroach, TMonster.Ant,
            TMonster.Spider, TMonster.Boar, TMonster.Fox, TMonster.Rat, TMonster.Partridge), RoomArenaEntrance));
        do {
            SpawnGroup(MonstersOf(TMonster.Deer, TMonster.Eagle, TMonster.Hare, TMonster.Mongrel, TMonster.Woodworm, TMonster.Fly), 38, RoomCityCentre);
        } while (!PastLimit(MonstersOf(TMonster.Deer, TMonster.Eagle, TMonster.Hare, TMonster.Mongrel, TMonster.Woodworm, TMonster.Fly), RoomArenaEntrance));
        do {
            SpawnGroup(MonstersOf(TMonster.Elephant, TMonster.Lion, TMonster.Giraffe, TMonster.Camel, TMonster.Ostrich,
                TMonster.Boa, TMonster.Wolf, TMonster.Bison, TMonster.Panther), 38, RoomCityCentre);
        } while (!PastLimit(MonstersOf(TMonster.Elephant, TMonster.Lion, TMonster.Giraffe, TMonster.Camel, TMonster.Ostrich,
            TMonster.Boa, TMonster.Wolf, TMonster.Bison, TMonster.Panther), RoomArenaEntrance));
        do {
            SpawnGroup(MonstersOf(TMonster.Gladiator, TMonster.Warrior, TMonster.Tamer), 38, RoomCityCentre);
        } while (!PastLimit(MonstersOf(TMonster.Gladiator, TMonster.Warrior, TMonster.Tamer), RoomArenaEntrance));
        do {
            SpawnGroup(MonstersOf(TMonster.Dachshund, TMonster.Shepherd, TMonster.Pooch, TMonster.Spaniel, TMonster.Poodle), 7, RoomCityCentre);
        } while (!PastLimit(MonstersOf(TMonster.Dachshund, TMonster.Shepherd, TMonster.Pooch, TMonster.Spaniel, TMonster.Poodle), 19));
        do {
            SpawnGroup(MonstersOf(TMonster.Cabman, TMonster.Vendor, TMonster.Sweeper, TMonster.Drunkard, TMonster.SickMan), 7, RoomCityCentre);
        } while (!PastLimit(MonstersOf(TMonster.Cabman, TMonster.Vendor, TMonster.Sweeper, TMonster.Drunkard, TMonster.SickMan), 19));
        do {
            SpawnGroup(MonstersOf(TMonster.Kid), 7, RoomEntertainmentCave);
            SpawnGroup(MonstersOf(TMonster.Lunatic, TMonster.Listener, TMonster.Fan, TMonster.Man), 8, RoomEntertainmentCave);
        } while (!PastLimit(MonstersOf(TMonster.Kid, TMonster.Lunatic, TMonster.Listener, TMonster.Fan, TMonster.Man), RoomEntertainmentCave));
        do {
            SpawnGroup(MonstersOf(TMonster.Policeman, TMonster.Bouncer, TMonster.Gorilla), 9, RoomEntertainmentCave);
            SpawnGroup(MonstersOf(TMonster.OldMan), 8, RoomEntertainmentCave);
            SpawnGroup(MonstersOf(TMonster.Reporter), 7, RoomEntertainmentCave);
        } while (!PastLimit(MonstersOf(TMonster.Policeman, TMonster.Bouncer, TMonster.Gorilla, TMonster.OldMan, TMonster.Reporter), RoomEntertainmentCave));
        SpawnGroup(MonstersOf(TMonster.Guitarist, TMonster.Drummer, TMonster.Keyboardist, TMonster.Liroy), 3, RoomStage1);
        SpawnAt(PubSpawns);
        Door = true;
        OldElf = true;
        do {
            SpawnGroup(MonstersOf(TMonster.Sorrel, TMonster.Clover, TMonster.Daisy, TMonster.SowThistle, TMonster.Dandelion,
                TMonster.Rose, TMonster.Thistle, TMonster.Raspberry, TMonster.Gooseberry, TMonster.Blackberry,
                TMonster.Grass), 8, RoomIvy1);
            SpawnGroup(MonstersOf(TMonster.Duncan), 8, RoomIvy1);
        } while (!((Monsters[TMonster.Sorrel] < RoomCavePath) && (Monsters[TMonster.Daisy] < RoomCavePath)
            && (Monsters[TMonster.Clover] < RoomCavePath) && (Monsters[TMonster.SowThistle] < RoomCavePath)
            && (Monsters[TMonster.Dandelion] < RoomCavePath) && (Monsters[TMonster.Rose] < RoomCavePath)
            && (Monsters[TMonster.Thistle] < RoomCavePath) && (Monsters[TMonster.Raspberry] < RoomCavePath)));
    }

    void Describe(List<TSpot> spots) =>
        spots.Where(spot => Monsters[spot.where] == game.Session.Room).Select(spot => spot.text).ToList().ForEach(Out.WriteLine);

    public void DescribePlants() {
        Describe(PlantSpots);
        if (game.Session.Room == Monsters[TMonster.Duncan]) {
            Out.WriteLine(TxtDescribePlants_12);
        }
    }

    public void DescribeStreet() =>
        Describe(StreetSpots);

    public void DescribeStage() =>
        Describe(StageSpots);

    public void DescribeCrowd() =>
        Describe(CrowdSpots);

    public void DescribeBeasts() =>
        Describe(BeastSpots);

    public string MazeDirectionCommand(TMazeDir dir) =>
        dir switch {
            TMazeDir.mdPolodnie => CmdPolodnie,
            TMazeDir.mdPolnoc => CmdPolnoc,
            TMazeDir.mdWschod => CmdWschod,
            TMazeDir.mdZachod => CmdZachod,
            _ => throw new ArgumentOutOfRangeException(nameof(dir)),
        };

    public void MazeMove(TMazeDir dir) {
        if (!MazeMoves.TryGetValue(game.Session.Room, out List<TMazeDir>? exits) || !exits.Contains(dir)) {
            return;
        }

        int? destination = rooms.RoomInDirection(game.Session.Room, MazeDirectionCommand(dir));
        if (destination is int room) {
            MoveTo(room);
        }
    }

    public void ReachCheckpoint(int exp, int stage, int next, int loStr, int hiStr) {
        if ((game.Player.Experience >= exp) && (game.Session.CheckpointStage == stage)
            && (game.Player.Stats[TStat.Strength] >= loStr) && (game.Player.Stats[TStat.Strength] <= hiStr)) {
            Out.WriteLine(TxtCheckpoints_01);
            game.Session.CheckpointStage = next;
            SpawnMonsters();
        }
    }

    public void Checkpoints() {
        for (int ci = 0; ci < CheckpointRules.Count; ci++) {
            TCheckpoint c = CheckpointRules[ci];
            ReachCheckpoint(c.exp, c.stage, c.next, c.loStr, c.hiStr);
        }
    }

    public TShopOffer? ShopOffer(TBagItem item) =>
        ShopOffers.GetValueOrDefault(item);

    public bool Buy(TBagItem item) {
        TShopOffer? offer = ShopOffer(item);
        if ((offer is null)
            || (game.Player.Money < (offer.buyGate >= 0 ? offer.buyGate : offer.buyPrice))) {
            return false;
        }

        Out.WriteLine(offer.buyLine != TxtEmpty
            ? offer.buyLine
            : $"{TxtShopVerb}{offer.buyName}{TxtSpaceZa}{offer.buyPrice}{TxtSpaceMonet}");
        game.Player.Money -= offer.buyPrice;
        game.Inventory.AcquireItem(item);
        return true;
    }

    public bool Sell(TBagItem item) {
        TShopOffer? offer = ShopOffer(item);
        if ((offer is null) || game.Inventory.IsEquipped(item) || !game.Inventory.LoseItem(item)) {
            return false;
        }

        offer.sellLines.ForEach(Out.WriteLine);
        game.Player.Money += offer.sellPrice;
        return true;
    }

    public void RunShopRules(Dictionary<string, TShopRule> rules) {
        if (!rules.TryGetValue(game.Command, out TShopRule? rule)) {
            return;
        }

        if (rule.selling) {
            Sell(rule.item);
        } else {
            Buy(rule.item);
        }
    }

    public void BakeryShop() {
        if (game.Verb == CmdLista) {
            game.WriteLines([
                TxtShared_24,
                TxtBakeryShop_01,
            ]);
        }

        game.Combat.StreetFights();
        RunShopRules(BakeryRules);
    }

    public void ArmouryShop() {
        if (game.Verb == CmdLista) {
            Out.WriteLine(TxtShared_24);
            game.WriteLines(ArmouryStock);

            Out.WriteLine(TxtArmouryShop_03);
        }
        game.Combat.StreetFights();
        RunShopRules(ArmouryRules);
    }

    public void GeneralStoreShop() {
        if (game.Verb == CmdLista) {
            Out.WriteLine(TxtGeneralStoreShop_01);
            game.WriteLines(GeneralStoreStock);
        }
        RunShopRules(GeneralStoreRules);
    }

    public void MagicShop() {
        if (game.Verb == CmdLista) {
            Out.WriteLine(TxtMagicShop_01);
        }

        RunShopRules(MagicShopRules);
    }

    List<TMonster> MonstersOf(params TMonster[] monsters) =>
        [.. monsters];

    public Dictionary<string, TQuestOffer> QuestOffers => _questOffers ??= new() {
        [$"{CmdZacznij}{CmdQuest} 1"] = new(TxtRoom100_09, 1, 75),
        [$"{CmdZacznij}{CmdQuest} 2"] = new(TxtRoom100_10, 2, 50),
        [$"{CmdZacznij}{CmdQuest} 3"] = new(TxtRoom100_11, 3, 50),
    };
    Dictionary<string, TQuestOffer>? _questOffers;

    public List<TGroundItem> GroundItems => _groundItems ??= [
        new(TBagItem.OldSword, TxtStart_07),
        new(TBagItem.SmallShield, TxtGroundItemLabel + TxtStart_08),
        new(TBagItem.Heart, TxtGroundItemLabel + TxtStart_09),
        new(TBagItem.Diploma, TxtGroundItemLabel + TxtStart_10),
        new(TBagItem.Pipe, TxtStart_11),
    ];
    List<TGroundItem>? _groundItems;

    public List<TSpawnedAt> CageInits => _cageInits ??= [
        new(TMonster.CageWeakling, RoomCageWeakling),
        new(TMonster.CageStrong, RoomCageStrong),
        new(TMonster.CageAgile, RoomCageAgile),
        new(TMonster.CageTough, RoomCageTough),
        new(TMonster.CageHuge, RoomCageHuge),
    ];
    List<TSpawnedAt>? _cageInits;

    public List<TSpawnedAt> PubSpawns => _pubSpawns ??= [
        new(TMonster.Minibarman, RoomPub),
        new(TMonster.Fatso, RoomPub),
        new(TMonster.DJ, RoomBackstage),
        new(TMonster.Pedal, RoomBar),
        new(TMonster.Couple, RoomBar),
        new(TMonster.Maciek, RoomBar),
    ];
    List<TSpawnedAt>? _pubSpawns;

    public List<TSpot> PlantSpots => _plantSpots ??= [
        new(TMonster.Sorrel, TxtDescribePlants_01),
        new(TMonster.Daisy, TxtDescribePlants_02),
        new(TMonster.Clover, TxtDescribePlants_03),
        new(TMonster.SowThistle, TxtDescribePlants_04),
        new(TMonster.Dandelion, TxtDescribePlants_05),
        new(TMonster.Rose, TxtDescribePlants_06),
        new(TMonster.Thistle, TxtDescribePlants_07),
        new(TMonster.Blackberry, TxtDescribePlants_08),
        new(TMonster.Gooseberry, TxtDescribePlants_09),
        new(TMonster.Raspberry, TxtDescribePlants_10),
        new(TMonster.Grass, TxtDescribePlants_11),
    ];
    List<TSpot>? _plantSpots;

    public List<TSpot> BeastSpots => _beastSpots ??= [
        new(TMonster.Woodworm, TxtDescribeBeasts_01),
        new(TMonster.Fly, TxtDescribeBeasts_02),
        new(TMonster.Bacterium, TxtDescribeBeasts_03),
        new(TMonster.Snail, TxtDescribeBeasts_04),
        new(TMonster.Beetle, TxtDescribeBeasts_05),
        new(TMonster.Cockroach, TxtDescribeBeasts_06),
        new(TMonster.Ant, TxtDescribeBeasts_07),
        new(TMonster.Spider, TxtDescribeBeasts_08),
        new(TMonster.Boar, TxtDescribeBeasts_09),
        new(TMonster.Rat, TxtDescribeBeasts_10),
        new(TMonster.Fox, TxtDescribeBeasts_11),
        new(TMonster.Partridge, TxtDescribeBeasts_12),
        new(TMonster.Hare, TxtDescribeBeasts_13),
        new(TMonster.Mongrel, TxtDescribeBeasts_14),
        new(TMonster.Eagle, TxtDescribeBeasts_15),
        new(TMonster.Deer, TxtDescribeBeasts_16),
        new(TMonster.Elephant, TxtDescribeBeasts_17),
        new(TMonster.Lion, TxtDescribeBeasts_18),
        new(TMonster.Giraffe, TxtDescribeBeasts_19),
        new(TMonster.Camel, TxtDescribeBeasts_20),
        new(TMonster.Ostrich, TxtDescribeBeasts_21),
        new(TMonster.Boa, TxtDescribeBeasts_22),
        new(TMonster.Wolf, TxtDescribeBeasts_23),
        new(TMonster.Bison, TxtDescribeBeasts_24),
        new(TMonster.Panther, TxtDescribeBeasts_25),
        new(TMonster.Gladiator, TxtDescribeBeasts_26),
        new(TMonster.Warrior, TxtDescribeBeasts_27),
        new(TMonster.Tamer, TxtDescribeBeasts_28),
    ];
    List<TSpot>? _beastSpots;

    public List<TSpot> StreetSpots => _streetSpots ??= [
        new(TMonster.Dachshund, TxtDescribeStreet_01),
        new(TMonster.Shepherd, TxtDescribeStreet_02),
        new(TMonster.Pooch, TxtDescribeStreet_03),
        new(TMonster.Spaniel, TxtDescribeStreet_04),
        new(TMonster.Poodle, TxtDescribeStreet_05),
        new(TMonster.Cabman, TxtDescribeStreet_06),
        new(TMonster.Vendor, TxtDescribeStreet_07),
        new(TMonster.Sweeper, TxtDescribeStreet_08),
        new(TMonster.Drunkard, TxtDescribeStreet_09),
        new(TMonster.SickMan, TxtDescribeStreet_10),
    ];
    List<TSpot>? _streetSpots;

    public List<TSpot> StageSpots => _stageSpots ??= [
        new(TMonster.Guitarist, TxtDescribeStage_01),
        new(TMonster.Drummer, TxtDescribeStage_02),
        new(TMonster.Keyboardist, TxtDescribeStage_03),
        new(TMonster.Liroy, TxtDescribeStage_04),
    ];
    List<TSpot>? _stageSpots;

    public List<TSpot> CrowdSpots => _crowdSpots ??= [
        new(TMonster.Kid, TxtDescribeCrowd_01),
        new(TMonster.Lunatic, TxtDescribeCrowd_02),
        new(TMonster.Listener, TxtDescribeCrowd_03),
        new(TMonster.Fan, TxtDescribeCrowd_04),
        new(TMonster.Man, TxtDescribeCrowd_05),
        new(TMonster.Policeman, TxtDescribeCrowd_06),
        new(TMonster.Bouncer, TxtDescribeCrowd_07),
        new(TMonster.Gorilla, TxtDescribeCrowd_08),
        new(TMonster.OldMan, TxtDescribeCrowd_09),
        new(TMonster.Reporter, TxtDescribeCrowd_10),
    ];
    List<TSpot>? _crowdSpots;

    public Dictionary<int, List<TMazeDir>> MazeMoves => _mazeMoves ??= new() {
        [RoomArena33] = [TMazeDir.mdPolodnie, TMazeDir.mdPolnoc, TMazeDir.mdWschod, TMazeDir.mdZachod],
        [RoomArena34] = [TMazeDir.mdPolodnie, TMazeDir.mdPolnoc, TMazeDir.mdWschod, TMazeDir.mdZachod],
        [RoomArena35] = [TMazeDir.mdPolnoc, TMazeDir.mdWschod, TMazeDir.mdZachod],
        [RoomArena36] = [TMazeDir.mdPolnoc, TMazeDir.mdWschod, TMazeDir.mdZachod],
        [RoomArena37] = [TMazeDir.mdPolodnie, TMazeDir.mdPolnoc, TMazeDir.mdWschod, TMazeDir.mdZachod],
        [RoomArena38] = [TMazeDir.mdPolodnie, TMazeDir.mdPolnoc, TMazeDir.mdWschod, TMazeDir.mdZachod],
        [RoomArena39] = [TMazeDir.mdPolodnie, TMazeDir.mdPolnoc, TMazeDir.mdWschod, TMazeDir.mdZachod],
        [RoomArena40] = [TMazeDir.mdPolnoc, TMazeDir.mdZachod],
        [RoomArena41] = [TMazeDir.mdPolnoc, TMazeDir.mdWschod],
        [RoomArena42] = [TMazeDir.mdPolodnie, TMazeDir.mdPolnoc, TMazeDir.mdWschod, TMazeDir.mdZachod],
        [RoomArena43] = [TMazeDir.mdPolodnie, TMazeDir.mdPolnoc, TMazeDir.mdWschod, TMazeDir.mdZachod],
        [RoomArena44] = [TMazeDir.mdPolodnie, TMazeDir.mdPolnoc, TMazeDir.mdWschod, TMazeDir.mdZachod],
        [RoomArena45] = [TMazeDir.mdPolodnie, TMazeDir.mdPolnoc, TMazeDir.mdZachod],
        [RoomArena46] = [TMazeDir.mdPolodnie, TMazeDir.mdPolnoc, TMazeDir.mdWschod],
        [RoomArena47] = [TMazeDir.mdPolodnie, TMazeDir.mdWschod, TMazeDir.mdZachod],
        [RoomArena48] = [TMazeDir.mdPolodnie, TMazeDir.mdPolnoc, TMazeDir.mdWschod, TMazeDir.mdZachod],
        [RoomArena49] = [TMazeDir.mdPolodnie, TMazeDir.mdPolnoc, TMazeDir.mdWschod, TMazeDir.mdZachod],
        [RoomArena50] = [TMazeDir.mdPolodnie, TMazeDir.mdPolnoc, TMazeDir.mdZachod],
        [RoomArena51] = [TMazeDir.mdPolodnie, TMazeDir.mdPolnoc, TMazeDir.mdWschod],
        [RoomArena52] = [TMazeDir.mdPolodnie, TMazeDir.mdWschod, TMazeDir.mdZachod],
        [RoomArena53] = [TMazeDir.mdPolodnie, TMazeDir.mdWschod, TMazeDir.mdZachod],
        [RoomArena54] = [TMazeDir.mdPolodnie, TMazeDir.mdPolnoc, TMazeDir.mdZachod],
        [RoomArena55] = [TMazeDir.mdPolodnie, TMazeDir.mdPolnoc, TMazeDir.mdWschod],
        [RoomArena56] = [TMazeDir.mdPolodnie, TMazeDir.mdZachod],
        [RoomArena57] = [TMazeDir.mdPolodnie, TMazeDir.mdWschod],
    };
    Dictionary<int, List<TMazeDir>>? _mazeMoves;

    public List<TCheckpoint> CheckpointRules => _checkpointRules ??= [
        new(400, 0, 1, (-Game.MaxInt - 1), 19),
        new(180, 0, 1, (-Game.MaxInt - 1), 7),
        new(360, 1, 2, (-Game.MaxInt - 1), 7),
        new(540, 2, 3, (-Game.MaxInt - 1), 7),
        new(300, 0, 1, 8, 12),
        new(500, 1, 2, 8, 12),
    ];
    List<TCheckpoint>? _checkpointRules;

    public Dictionary<TBagItem, TShopOffer> ShopOffers => _shopOffers ??= new() {
        [TBagItem.OldSword] = new() {
            buyPrice = 70,
            buyName = TxtBuyOldSword_01,
            sellPrice = 30,
            sellLines = [TxtSellOldSword_01],
        },
        [TBagItem.SmallShield] = new() {
            buyPrice = 15,
            buyName = TxtBuySmallShield_01,
            sellPrice = 8,
            sellLines = [TxtSellSmallShield_01],
        },
        [TBagItem.Pipe] = new() {
            buyPrice = 480,
            buyName = TxtTakeDrop_05,
            sellPrice = 120,
            sellLines = [$"{TxtSellVerb}{TxtSellPipe_01}"],
        },
        [TBagItem.Tracksuit] = new() {
            buyPrice = 35,
            buyName = TxtBuyTracksuit_01,
            sellPrice = 15,
            sellLines = [$"{TxtSellVerb}{TxtSellTracksuit_01}"],
        },
        [TBagItem.Cassette] = new() {
            buyPrice = 1999,
            buyName = TxtBuyCassette_01,
            sellPrice = 1000,
            sellLines = [$"{TxtSellVerb}{TxtSellCassette_01}"],
        },
        [TBagItem.SpikedSuit] = new() {
            buyPrice = 1000,
            buyLine = TxtBuySpikedSuit_01,
            sellPrice = 500,
            sellLines = [$"{TxtSellVerb}{TxtSellSpikedSuit_01}"],
        },
        [TBagItem.ManaBottle] = new() { buyPrice = 20, buyGate = 15, buyLine = TxtBuyManaBottle_01 },
        [TBagItem.LuckyLeaf] = new() {
            buyPrice = 830,
            buyGate = 820,
            buyLine = TxtBuyLuckyLeaf_01,
            sellPrice = 400,
            sellLines = [$"{TxtSellVerb}{TxtSellLuckyLeaf_01}"],
        },
        [TBagItem.TeleportPill] = new() {
            buyPrice = 235,
            buyName = TxtBuyTeleportPill_01,
            sellPrice = 170,
            sellLines = [TxtSellTeleportPill_01],
        },
        [TBagItem.CompareScroll] = new() {
            buyPrice = 40,
            buyLine = TxtBuyCompareScroll_01,
            sellPrice = 18,
            sellLines = [TxtSellCompareScroll_01],
        },
        [TBagItem.Doughnut] = new() { buyPrice = 8, buyName = CmdPaczek },
        [TBagItem.Cookie] = new() { buyPrice = 12, buyName = CmdCiastko },
        [TBagItem.DryRation] = new() { buyPrice = 15, buyName = TxtItemSuchaRacje },
        [TBagItem.BreadRoll] = new() { buyPrice = 19, buyName = TxtItemBulke },
        [TBagItem.Bread] = new() { buyPrice = 24, buyName = CmdChleb },
        [TBagItem.Sausage] = new() { buyPrice = 29, buyName = TxtItemWeke },
    };
    Dictionary<TBagItem, TShopOffer>? _shopOffers;

    public Dictionary<string, TShopRule> ArmouryRules => _armouryRules ??= new() {
        [$"{CmdKup}{CmdStary}"] = new(TBagItem.OldSword, false),
        [$"{CmdSprzedaj}{CmdStary}"] = new(TBagItem.OldSword, true),
        [$"{CmdKup}{CmdMala}"] = new(TBagItem.SmallShield, false),
        [$"{CmdSprzedaj}{CmdMala}"] = new(TBagItem.SmallShield, true),
    };
    Dictionary<string, TShopRule>? _armouryRules;

    public Dictionary<string, TShopRule> GeneralStoreRules => _generalStoreRules ??= new() {
        [$"{CmdKup}{CmdFajka}"] = new(TBagItem.Pipe, false),
        [$"{CmdKup}{CmdKomplet}"] = new(TBagItem.Tracksuit, false),
        [$"{CmdSprzedaj}{CmdKomplet}"] = new(TBagItem.Tracksuit, true),
        [$"{CmdSprzedaj}{CmdFajka}"] = new(TBagItem.Pipe, true),
        [$"{CmdKup}{CmdKaseta}"] = new(TBagItem.Cassette, false),
        [$"{CmdKup}{TxtWornGarnitur}"] = new(TBagItem.SpikedSuit, false),
        [$"{CmdSprzedaj}{TxtWornGarnitur}"] = new(TBagItem.SpikedSuit, true),
        [$"{CmdSprzedaj}{CmdKaseta}"] = new(TBagItem.Cassette, true),
    };
    Dictionary<string, TShopRule>? _generalStoreRules;

    public Dictionary<string, TShopRule> MagicShopRules => _magicShopRules ??= new() {
        [$"{CmdKup}{CmdMalaButelkaMany}"] = new(TBagItem.ManaBottle, false),
        [$"{CmdKup}{CmdListek}"] = new(TBagItem.LuckyLeaf, false),
        [$"{CmdSprzedaj}{CmdListek}"] = new(TBagItem.LuckyLeaf, true),
        [$"{CmdKup}{CmdPigulkaTransportujaca}"] = new(TBagItem.TeleportPill, false),
        [$"{CmdSprzedaj}{CmdPigulkaTransportujaca}"] = new(TBagItem.TeleportPill, true),
        [$"{CmdKup}{CmdScrollPorownywanie}"] = new(TBagItem.CompareScroll, false),
        [$"{CmdSprzedaj}{CmdScrollPorownywanie}"] = new(TBagItem.CompareScroll, true),
    };
    Dictionary<string, TShopRule>? _magicShopRules;

    public Dictionary<string, TShopRule> BakeryRules => _bakeryRules ??= new() {
        [$"{CmdKup}{CmdPaczek}"] = new(TBagItem.Doughnut, false),
        [$"{CmdKup}{CmdCiastko}"] = new(TBagItem.Cookie, false),
        [$"{CmdKup}{CmdSuchaRacja}"] = new(TBagItem.DryRation, false),
        [$"{CmdKup}{CmdBulka}"] = new(TBagItem.BreadRoll, false),
        [$"{CmdKup}{CmdChleb}"] = new(TBagItem.Bread, false),
        [$"{CmdKup}{CmdWeka}"] = new(TBagItem.Sausage, false),
    };
    Dictionary<string, TShopRule>? _bakeryRules;

    public List<string> ArmouryStock => _armouryStock ??= [TxtArmouryShop_01, TxtArmouryShop_02];
    List<string>? _armouryStock;

    public List<string> GeneralStoreStock => _generalStoreStock ??= [TxtGeneralStoreShop_02, TxtGeneralStoreShop_03];
    List<string>? _generalStoreStock;

}
