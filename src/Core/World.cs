
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
        TMazeMove? maze = MazeMoves.FirstOrDefault(move => move?.room == number);
        if (maze == null) {
            return;
        }
        Out.WriteLine(TxtShared_03);
        foreach ((TMazeDir dir, string cmd, string label) in MazeExitLabels) {
            if (maze.exits.Contains(dir) && RoomInDirection(number, cmd) is int destination) {
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

    public void StartQuest(string cmd, string line, int quest, int kills) {
        if (!game.Matches(cmd)) {
            return;
        }

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

    public void DescribePlants() {
        for (int i = 0; i < PlantSpots.Count; i++) {
            if (Monsters[PlantSpots[i].where] == game.Session.Room) {
                Out.WriteLine(PlantSpots[i].text);
            }
        }

        if (game.Session.Room == Monsters[TMonster.Duncan]) {
            Out.WriteLine(TxtDescribePlants_12);
        }
    }

    public void DescribeStreet() {
        for (int i = 0; i < StreetSpots.Count; i++) {
            if (Monsters[StreetSpots[i].where] == game.Session.Room) {
                Out.WriteLine(StreetSpots[i].text);
            }
        }
    }

    public void DescribeStage() {
        for (int i = 0; i < StageSpots.Count; i++) {
            if (Monsters[StageSpots[i].where] == game.Session.Room) {
                Out.WriteLine(StageSpots[i].text);
            }
        }
    }

    public void DescribeCrowd() {
        for (int i = 0; i < CrowdSpots.Count; i++) {
            if (Monsters[CrowdSpots[i].where] == game.Session.Room) {
                Out.WriteLine(CrowdSpots[i].text);
            }
        }
    }

    public void DescribeBeasts() {
        for (int i = 0; i < BeastSpots.Count; i++) {
            if (Monsters[BeastSpots[i].where] == game.Session.Room) {
                Out.WriteLine(BeastSpots[i].text);
            }
        }
    }

    public string MazeDirectionCommand(TMazeDir dir) =>
        dir switch {
            TMazeDir.mdPolodnie => CmdPolodnie,
            TMazeDir.mdPolnoc => CmdPolnoc,
            TMazeDir.mdWschod => CmdWschod,
            TMazeDir.mdZachod => CmdZachod,
            _ => throw new ArgumentOutOfRangeException(nameof(dir)),
        };

    public void MazeMove(TMazeDir dir) {
        TMazeMove? maze = MazeMoves.FirstOrDefault(move => move.room == game.Session.Room);
        if (maze?.exits.Contains(dir) != true) {
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
        ShopOffers.FirstOrDefault(o => o.item == item);

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

    public void RunShopRules(List<TShopRule> rules) {
        TShopRule? rule = rules.FirstOrDefault(r => game.Matches(r.cmd));
        if (rule is null) {
            return;
        }

        if (rule.selling) {
            Sell(rule.item);
        } else {
            Buy(rule.item);
        }
    }

    public void ShowStock(List<string> lines) =>
        lines.ForEach(Out.WriteLine);

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
            for (int si2 = 0; si2 < ArmouryStock.Count; si2++) {
                ShowStock(ArmouryStock[si2].lines);
            }

            Out.WriteLine(TxtArmouryShop_03);
        }
        game.Combat.StreetFights();
        RunShopRules(ArmouryRules);
    }

    public void GeneralStoreShop() {
        if (game.Verb == CmdLista) {
            Out.WriteLine(TxtGeneralStoreShop_01);
            for (int si2 = 0; si2 < GeneralStoreStock.Count; si2++) {
                ShowStock(GeneralStoreStock[si2].lines);
            }
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

    public List<TQuestOffer> QuestOffers => _questOffers ??= [
        new($"{CmdZacznij}{CmdQuest} 1", TxtRoom100_09, 1, 75),
        new($"{CmdZacznij}{CmdQuest} 2", TxtRoom100_10, 2, 50),
        new($"{CmdZacznij}{CmdQuest} 3", TxtRoom100_11, 3, 50),
    ];
    List<TQuestOffer>? _questOffers;

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

    public List<TMazeMove> MazeMoves => _mazeMoves ??= [
        new() { room = RoomArena33, exits = [TMazeDir.mdPolodnie, TMazeDir.mdPolnoc, TMazeDir.mdWschod, TMazeDir.mdZachod] },
        new() { room = RoomArena34, exits = [TMazeDir.mdPolodnie, TMazeDir.mdPolnoc, TMazeDir.mdWschod, TMazeDir.mdZachod] },
        new() { room = RoomArena35, exits = [TMazeDir.mdPolnoc, TMazeDir.mdWschod, TMazeDir.mdZachod] },
        new() { room = RoomArena36, exits = [TMazeDir.mdPolnoc, TMazeDir.mdWschod, TMazeDir.mdZachod] },
        new() { room = RoomArena37, exits = [TMazeDir.mdPolodnie, TMazeDir.mdPolnoc, TMazeDir.mdWschod, TMazeDir.mdZachod] },
        new() { room = RoomArena38, exits = [TMazeDir.mdPolodnie, TMazeDir.mdPolnoc, TMazeDir.mdWschod, TMazeDir.mdZachod] },
        new() { room = RoomArena39, exits = [TMazeDir.mdPolodnie, TMazeDir.mdPolnoc, TMazeDir.mdWschod, TMazeDir.mdZachod] },
        new() { room = RoomArena40, exits = [TMazeDir.mdPolnoc, TMazeDir.mdZachod] },
        new() { room = RoomArena41, exits = [TMazeDir.mdPolnoc, TMazeDir.mdWschod] },
        new() { room = RoomArena42, exits = [TMazeDir.mdPolodnie, TMazeDir.mdPolnoc, TMazeDir.mdWschod, TMazeDir.mdZachod] },
        new() { room = RoomArena43, exits = [TMazeDir.mdPolodnie, TMazeDir.mdPolnoc, TMazeDir.mdWschod, TMazeDir.mdZachod] },
        new() { room = RoomArena44, exits = [TMazeDir.mdPolodnie, TMazeDir.mdPolnoc, TMazeDir.mdWschod, TMazeDir.mdZachod] },
        new() { room = RoomArena45, exits = [TMazeDir.mdPolodnie, TMazeDir.mdPolnoc, TMazeDir.mdZachod] },
        new() { room = RoomArena46, exits = [TMazeDir.mdPolodnie, TMazeDir.mdPolnoc, TMazeDir.mdWschod] },
        new() { room = RoomArena47, exits = [TMazeDir.mdPolodnie, TMazeDir.mdWschod, TMazeDir.mdZachod] },
        new() { room = RoomArena48, exits = [TMazeDir.mdPolodnie, TMazeDir.mdPolnoc, TMazeDir.mdWschod, TMazeDir.mdZachod] },
        new() { room = RoomArena49, exits = [TMazeDir.mdPolodnie, TMazeDir.mdPolnoc, TMazeDir.mdWschod, TMazeDir.mdZachod] },
        new() { room = RoomArena50, exits = [TMazeDir.mdPolodnie, TMazeDir.mdPolnoc, TMazeDir.mdZachod] },
        new() { room = RoomArena51, exits = [TMazeDir.mdPolodnie, TMazeDir.mdPolnoc, TMazeDir.mdWschod] },
        new() { room = RoomArena52, exits = [TMazeDir.mdPolodnie, TMazeDir.mdWschod, TMazeDir.mdZachod] },
        new() { room = RoomArena53, exits = [TMazeDir.mdPolodnie, TMazeDir.mdWschod, TMazeDir.mdZachod] },
        new() { room = RoomArena54, exits = [TMazeDir.mdPolodnie, TMazeDir.mdPolnoc, TMazeDir.mdZachod] },
        new() { room = RoomArena55, exits = [TMazeDir.mdPolodnie, TMazeDir.mdPolnoc, TMazeDir.mdWschod] },
        new() { room = RoomArena56, exits = [TMazeDir.mdPolodnie, TMazeDir.mdZachod] },
        new() { room = RoomArena57, exits = [TMazeDir.mdPolodnie, TMazeDir.mdWschod] },
    ];
    List<TMazeMove>? _mazeMoves;

    public List<TCheckpoint> CheckpointRules => _checkpointRules ??= [
        new(400, 0, 1, (-Game.MaxInt - 1), 19),
        new(180, 0, 1, (-Game.MaxInt - 1), 7),
        new(360, 1, 2, (-Game.MaxInt - 1), 7),
        new(540, 2, 3, (-Game.MaxInt - 1), 7),
        new(300, 0, 1, 8, 12),
        new(500, 1, 2, 8, 12),
    ];
    List<TCheckpoint>? _checkpointRules;

    public List<TShopOffer> ShopOffers => _shopOffers ??= [
        new() { item = TBagItem.OldSword, buyPrice = 70, buyName = TxtBuyOldSword_01,
            sellPrice = 30, sellLines = [TxtSellOldSword_01], },
        new() { item = TBagItem.SmallShield, buyPrice = 15, buyName = TxtBuySmallShield_01,
            sellPrice = 8, sellLines = [TxtSellSmallShield_01], },
        new() { item = TBagItem.Pipe, buyPrice = 480, buyName = TxtTakeDrop_05,
            sellPrice = 120, sellLines = [$"{TxtSellVerb}{TxtSellPipe_01}"], },
        new() { item = TBagItem.Tracksuit, buyPrice = 35, buyName = TxtBuyTracksuit_01,
            sellPrice = 15, sellLines = [$"{TxtSellVerb}{TxtSellTracksuit_01}"], },
        new() { item = TBagItem.Cassette, buyPrice = 1999, buyName = TxtBuyCassette_01,
            sellPrice = 1000,
            sellLines = [$"{TxtSellVerb}{TxtSellCassette_01}"], },
        new() { item = TBagItem.SpikedSuit, buyPrice = 1000, buyLine = TxtBuySpikedSuit_01,
            sellPrice = 500, sellLines = [$"{TxtSellVerb}{TxtSellSpikedSuit_01}"], },
        new() { item = TBagItem.ManaBottle, buyPrice = 20, buyGate = 15, buyLine = TxtBuyManaBottle_01 },
        new() { item = TBagItem.LuckyLeaf, buyPrice = 830, buyGate = 820, buyLine = TxtBuyLuckyLeaf_01,
            sellPrice = 400, sellLines = [$"{TxtSellVerb}{TxtSellLuckyLeaf_01}"], },
        new() { item = TBagItem.TeleportPill, buyPrice = 235, buyName = TxtBuyTeleportPill_01,
            sellPrice = 170, sellLines = [TxtSellTeleportPill_01], },
        new() { item = TBagItem.CompareScroll, buyPrice = 40, buyLine = TxtBuyCompareScroll_01,
            sellPrice = 18, sellLines = [TxtSellCompareScroll_01], },
        new() { item = TBagItem.Doughnut, buyPrice = 8, buyName = CmdPaczek },
        new() { item = TBagItem.Cookie, buyPrice = 12, buyName = CmdCiastko },
        new() { item = TBagItem.DryRation, buyPrice = 15, buyName = TxtItemSuchaRacje },
        new() { item = TBagItem.BreadRoll, buyPrice = 19, buyName = TxtItemBulke },
        new() { item = TBagItem.Bread, buyPrice = 24, buyName = CmdChleb },
        new() { item = TBagItem.Sausage, buyPrice = 29, buyName = TxtItemWeke },
    ];
    List<TShopOffer>? _shopOffers;

    public List<TShopRule> ArmouryRules => _armouryRules ??= [
        new(CmdKup + CmdStary, TBagItem.OldSword, false),
        new(CmdSprzedaj + CmdStary, TBagItem.OldSword, true),
        new(CmdKup + CmdMala, TBagItem.SmallShield, false),
        new(CmdSprzedaj + CmdMala, TBagItem.SmallShield, true),
    ];
    List<TShopRule>? _armouryRules;

    public List<TShopRule> GeneralStoreRules => _generalStoreRules ??= [
        new(CmdKup + CmdFajka, TBagItem.Pipe, false),
        new(CmdKup + CmdKomplet, TBagItem.Tracksuit, false),
        new(CmdSprzedaj + CmdKomplet, TBagItem.Tracksuit, true),
        new(CmdSprzedaj + CmdFajka, TBagItem.Pipe, true),
        new(CmdKup + CmdKaseta, TBagItem.Cassette, false),
        new(CmdKup + TxtWornGarnitur, TBagItem.SpikedSuit, false),
        new(CmdSprzedaj + TxtWornGarnitur, TBagItem.SpikedSuit, true),
        new(CmdSprzedaj + CmdKaseta, TBagItem.Cassette, true),
    ];
    List<TShopRule>? _generalStoreRules;

    public List<TShopRule> MagicShopRules => _magicShopRules ??= [
        new(CmdKup + CmdMalaButelkaMany, TBagItem.ManaBottle, false),
        new(CmdKup + CmdListek, TBagItem.LuckyLeaf, false),
        new(CmdSprzedaj + CmdListek, TBagItem.LuckyLeaf, true),
        new(CmdKup + CmdPigulkaTransportujaca, TBagItem.TeleportPill, false),
        new(CmdSprzedaj + CmdPigulkaTransportujaca, TBagItem.TeleportPill, true),
        new(CmdKup + CmdScrollPorownywanie, TBagItem.CompareScroll, false),
        new(CmdSprzedaj + CmdScrollPorownywanie, TBagItem.CompareScroll, true),
    ];
    List<TShopRule>? _magicShopRules;

    public List<TShopRule> BakeryRules => _bakeryRules ??= [
        new(CmdKup + CmdPaczek, TBagItem.Doughnut, false),
        new(CmdKup + CmdCiastko, TBagItem.Cookie, false),
        new(CmdKup + CmdSuchaRacja, TBagItem.DryRation, false),
        new(CmdKup + CmdBulka, TBagItem.BreadRoll, false),
        new(CmdKup + CmdChleb, TBagItem.Bread, false),
        new(CmdKup + CmdWeka, TBagItem.Sausage, false),
    ];
    List<TShopRule>? _bakeryRules;

    public List<TShopStock> ArmouryStock => _armouryStock ??= [
        new() { lines = [TxtArmouryShop_01] },
        new() { lines = [TxtArmouryShop_02] },
    ];
    List<TShopStock>? _armouryStock;

    public List<TShopStock> GeneralStoreStock => _generalStoreStock ??= [
        new() { lines = [TxtGeneralStoreShop_02] },
        new() { lines = [TxtGeneralStoreShop_03] },
    ];
    List<TShopStock>? _generalStoreStock;

}
