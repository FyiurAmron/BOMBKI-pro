
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using static Texts;
using static Rooms;

public class Game {

    public TPlayer Player = new();
    public TSkills Skills = new();
    public TGear Gear = new();
    public TBag Bag = new();
    public TCombat Combat = new();
    public TWorld World = new();
    public TSession Session = new();
    public Inventory Inventory { get; }
    public Rooms Rooms { get; }

    public Game() {
        Inventory = new Inventory(this);
        Rooms = new Rooms(this);
    }

    public void GainLoot1000(int threshold, string line1, string line2, TBagItem item) =>
        Inventory.GainLoot1000(threshold, line1, line2, item);

    public void GainLoot100(int threshold, string line1, string line2, TBagItem item) =>
        Inventory.GainLoot100(threshold, line1, line2, item);

    public void SuitGain() =>
        Inventory.SuitGain();

    public void PillGain() =>
        Inventory.PillGain();

    public void CassetteGain() =>
        Inventory.CassetteGain();

    public void LeafGain() =>
        Inventory.LeafGain();

    public void ScrollGain() =>
        Inventory.ScrollGain();

    public void TakeItem(string cmd, TBagItem item, string verb, string line) =>
        Inventory.TakeItem(cmd, item, verb, line);

    public bool IsEquipped(TBagItem item) =>
        Inventory.IsEquipped(item);

    public void AcquireItem(TBagItem item, int quantity = 1) =>
        Inventory.AcquireItem(item, quantity);

    public bool LoseItem(TBagItem item, int quantity = 1) =>
        Inventory.LoseItem(item, quantity);

    public void AcquirePermit() =>
        Inventory.AcquirePermit();

    public bool LosePermit() =>
        Inventory.LosePermit();

    public void DropItem(string cmd, TBagItem item, string verb, string line) =>
        Inventory.DropItem(cmd, item, verb, line);

    public bool Equip(TBagItem item) =>
        Inventory.Equip(item);

    public bool Unequip(TBagItem item) =>
        Inventory.Unequip(item);

    public void TakeDrop() =>
        Inventory.TakeDrop();

    public void ClampStat(ref int stat, int cap) =>
        Inventory.ClampStat(ref stat, cap);

    public void EatFood(string cmd, TBagItem item, string verb, string line, int gain, IntSlot stat, IntSlot cap) =>
        Inventory.EatFood(cmd, item, verb, line, gain, stat, cap);

    public void UseItems() =>
        Inventory.UseItems();

    public void RecalcBag() =>
        Inventory.RecalcBag();

    public string Command {
        get => _command;
        set {
            _command = NormalizeCommand(value);
            var (verb, rest) = SplitVerbRest(_command);
            Verb = verb;
            Rest = rest;
        }
    }

    string _command = "";

    public string Verb = "";
    public string Rest = "";

    string NormalizeCommand(string value) {
        value = value.Trim();
        if (!value.Contains("  ")) {
            return value;
        }
        StringBuilder normalized = new(value.Length);
        bool space = false;
        foreach (char c in value) {
            if (c == ' ') {
                if (!space) {
                    normalized.Append(c);
                }
                space = true;
            } else {
                normalized.Append(c);
                space = false;
            }
        }
        return normalized.ToString();
    }

    (string verb, string rest) SplitVerbRest(string command) {
        int split = command.IndexOf(' ');
        return split < 0 ? (command, TxtEmpty) : (command[..(split + 1)], command[(split + 1)..]);
    }

    public bool Matches(string cmd) =>
        Command == cmd;


    const ulong BombkiDefaultSeed = 0x9E3779B97F4A7C15;
    const ulong XorshiftMult = 2685821657736338717;
    ulong _bombkiState = BombkiDefaultSeed;

    ulong ParseSeed(string? text) {
        if (text is null) {
            return 0;
        }

        int i = 0;
        while (i < text.Length && (text[i] == ' ' || text[i] == '\t')) {
            i++;
        }

        ulong numberBase = 10;
        if (i + 1 < text.Length && text[i] == '0' && (text[i + 1] == 'x' || text[i + 1] == 'X')) {
            numberBase = 16;
            i += 2;
        }
        ulong value = 0;
        int digits = 0;
        for (; i < text.Length; i++) {
            char c = text[i];
            ulong digit;
            if (c >= '0' && c <= '9') {
                digit = (ulong)(c - '0');
            } else if (numberBase == 16 && c >= 'a' && c <= 'f') {
                digit = (ulong)(c - 'a') + 10;
            } else if (numberBase == 16 && c >= 'A' && c <= 'F') {
                digit = (ulong)(c - 'A') + 10;
            } else {
                break;
            }

            unchecked {
                value = value * numberBase + digit;
            }
            digits++;
        }
        return digits == 0 ? 0 : value;
    }

    public void Randomize() {
        if (!Crt.DebugEnabled()) {
            _bombkiState = (ulong)DateTime.UtcNow.Ticks;
            return;
        }
        string? raw = Environment.GetEnvironmentVariable("BOMBKI_SEED");
        if (raw is null) {
            _bombkiState = (ulong)DateTime.UtcNow.Ticks;
            return;
        }
        ulong parsed = ParseSeed(raw);
        _bombkiState = parsed == 0 ? BombkiDefaultSeed : parsed;
    }

    uint NextBelow(uint bound) {
        ulong x = _bombkiState;
        x ^= x >> 12;
        x ^= x << 25;
        x ^= x >> 27;
        _bombkiState = x;
        return (uint)(((x * XorshiftMult) >> 32) % bound);
    }

    public int Roll(int spread) =>
        spread <= 0 ? 0 : (int)NextBelow((uint)spread);

    public const int MaxInt = 32767;

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

    public void MoveTo(int room) {
        Session.Room = room;
        Session.OuterRoom = room;
        GroundItemLines();
    }

    public void GroundItemLines() {
        for (int ti = 0; ti < GroundItems.Count; ti++) {
            TGroundItem t = GroundItems[ti];
            if (Bag.HasGroundItem(t.item, Session.OuterRoom)) {
                Out.WriteLine(t.line);
            }
        }
    }

    public void MoveWhen(List<TMovement> moves) {
        for (int i = 0; i < moves.Count; i++) {
            if (!Matches(moves[i].cmd)) {
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
        if (Verb == CmdWyjscie) {
            Session.ExitRequested = true;
            return true;
        }
        MoveWhen(moves);
        return false;
    }

    public void WriteLines(List<string> lines) {
        for (int i = 0; i < lines.Count; i++) {
            Out.WriteLine(lines[i]);
        }
    }

    public void TryLoot(TBagItem item, int threshold, string foundMsg) {
        Combat.Roll = Roll(20);
        if (Combat.Roll < threshold) {
            Bag.AddGroundItem(item, Session.Room);
            Out.WriteLine(TxtShared_08 + foundMsg);
        }
    }

    public void TryCageLoot(int threshold) {
        TryLoot(TBagItem.OldSword, threshold, TxtShared_10);
        TryLoot(TBagItem.SmallShield, threshold, TxtShared_11);
        TryLoot(TBagItem.Heart, 6, TxtShared_12);
    }

    public void ShowIntro() {
        Crt.ClrScr();
        Player.MaxEnergy = 50;
        WriteLines(ReadBanner());
        WriteRainbow(TxtShowIntro_12);
        Out.WriteLine();
        Out.WriteLine(TxtShowIntro_13);
        Out.WriteLine(TxtShowIntro_14);
    }

    public List<string> ReadBanner() =>
        [.. File.ReadAllLines(Path.Combine(AppContext.BaseDirectory, "data", "banner.aa"), Encoding.Latin1)];

    static readonly List<int> RainbowColors = [91, 93, 92, 96, 94, 95];

    public void WriteRainbow(string text) {
        int step = 0;
        foreach (char c in text) {
            if (c == ' ') {
                Out.Write(" ");
            } else {
                Out.Write($"\x1b[{RainbowColors[step % RainbowColors.Count]}m{c}");
                step++;
            }
        }
        Out.Write("\x1b[m");
    }

    public void StartQuest(string cmd, string line, int quest, int kills) {
        if (!Matches(cmd)) {
            return;
        }

        if (World.Quest > 0) {
            Out.WriteLine(TxtRoom100_17);
            return;
        }

        Out.WriteLine(line);
        World.Quest = quest;
        World.QuestKills = kills;
        World.LiroyKilled = World.Monsters[TMonster.Liroy] == 0;
    }

    public List<TBand> PosExpBands => _posExpBands ??= [
        new(1, 2, 10, 0), new(3, 3, 19, 0), new(4, 6, 9, 0),
        new(7, 9, 8, 0), new(10, 12, 7, 0), new(13, 15, 6, 0),
        new(16, 18, 5, 0), new(19, 21, 4, 0), new(22, 24, 3, 0),
        new(25, 27, 2, 0), new(28, 30, 1, 0), new(31, MaxInt, 0, 0),
    ];
    List<TBand>? _posExpBands;

    public List<TBand> NegExpBands => _negExpBands ??= [
        new(2, 2, 12, 0), new(4, 4, 13, 0), new(6, 6, 14, 0),
        new(8, 8, 15, 0), new(10, 10, 16, 0), new(12, 12, 17, 0),
        new(14, 14, 18, 0), new(16, 16, 19, 0), new(18, 18, 20, 0),
        new(21, MaxInt, 21, 0),
    ];
    List<TBand>? _negExpBands;

    public int BandAmount(List<TBand> bands, int v) {
        for (int bi = 0; bi < bands.Count; bi++) {
            TBand band = bands[bi];
            if ((v >= band.lo) && (v <= band.hi)) {
                return (band.Base + band.perUnit * v);
            }
        }
        return 0;
    }

    public int FightExpForEnergy(int diff) =>
        diff > 0
            ? BandAmount(PosExpBands, diff)
            : diff < 0 ? BandAmount(NegExpBands, (-diff)) : 11;

    public int FightExpForStrength(int diff) {
        if (diff > 0) {
            return diff >= 10 ? 1 : 11 - diff;
        } else if (diff < 0) {
            diff = (-diff);
            return diff >= 10 ? 21 : 11 + diff;
        } else {
            return 11;
        }
    }

    public int FightExpForDexterity(int diff) {
        int step;
        if (diff > 0) {
            step = ((diff + 1) / 2);
            return step >= 10 ? 1 : 11 - step;
        } else if (diff < 0) {
            diff = (-diff);
            step = ((diff + 1) / 2);
            return step >= 10 ? 21 : 11 + step;
        } else {
            return 11;
        }
    }


    public List<TDodgeBound> DodgeBounds => _dodgeBounds ??= [
        new(30, 12, 60), new(27, 13, 58), new(24, 14, 56),
        new(20, 15, 54), new(18, 16, 52), new(16, 17, 51),
        new(12, 18, 48), new(9, 19, 46), new(6, 20, 42),
        new(4, 21, 40), new(2, 22, 38), new(0, 23, 35),
    ];
    List<TDodgeBound>? _dodgeBounds;

    public TDodgeBound DodgeBoundRow(int chance) {
        int i = 0;
        while ((i < DodgeBounds.Count - 1) && (chance <= DodgeBounds[i].limit)) {
            i++;
        }

        return DodgeBounds[i];
    }

    public int DodgeBound(int chance) =>
        DodgeBoundRow(chance).player;
    public int EnemyDodgeBound(int chance) =>
        DodgeBoundRow(chance).enemy;

    public int DexterityBonus(int value) =>
        value >= 8 ? value / 2 : 0;

    public int ParryBonus(int value) {
        int bonus = 0;
        if (value > 75) {
            bonus++;
        }

        if (value > 50) {
            bonus++;
        }

        return bonus;
    }

    public int KickBonus(int value) {
        int bonus = 0;
        if (value > 70) {
            bonus += 2;
        }

        if (value > 10) {
            bonus += 2;
        }

        return bonus;
    }

    public void FightDog() {
        Combat.EnemyEnergy = 10;
        Combat.EnemyDexterity = 10;
        Combat.EnemyStrength = 5;
        DoFight();
        Combat.Loot = Roll(15);
        Out.WriteLine(TxtShared_08 + Combat.Loot + TxtFightDog_01);
        Player.Money += Combat.Loot;
        TryLoot(TBagItem.Heart, 5, TxtFightDog_02);
    }

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

    public void SpawnAt(List<TSpawnedAt> placements) {
        foreach (TSpawnedAt placement in placements) {
            World.Monsters[placement.monster] = placement.room;
        }
    }

    public void SpawnGroup(List<TMonster> targets, int spread, int Base) {
        foreach (TMonster target in targets) {
            World.Monsters[target] = Roll(spread) + Base;
        }
    }

    public bool PastLimit(List<TMonster> targets, int limit) =>
        targets.All(target => World.Monsters[target] > limit);

    List<TMonster> MonstersOf(params TMonster[] monsters) =>
        [.. monsters];

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
        World.Door = true;
        World.OldElf = true;
        do {
            SpawnGroup(MonstersOf(TMonster.Sorrel, TMonster.Clover, TMonster.Daisy, TMonster.SowThistle, TMonster.Dandelion,
                TMonster.Rose, TMonster.Thistle, TMonster.Raspberry, TMonster.Gooseberry, TMonster.Blackberry,
                TMonster.Grass), 8, RoomIvy1);
            SpawnGroup(MonstersOf(TMonster.Duncan), 8, RoomIvy1);
        } while (!((World.Monsters[TMonster.Sorrel] < RoomCavePath) && (World.Monsters[TMonster.Daisy] < RoomCavePath)
            && (World.Monsters[TMonster.Clover] < RoomCavePath) && (World.Monsters[TMonster.SowThistle] < RoomCavePath)
            && (World.Monsters[TMonster.Dandelion] < RoomCavePath) && (World.Monsters[TMonster.Rose] < RoomCavePath)
            && (World.Monsters[TMonster.Thistle] < RoomCavePath) && (World.Monsters[TMonster.Raspberry] < RoomCavePath)));
    }

    public void TrainAttribute(IntSlot Attribute, IntSlot MaxAttribute, int MinPractice, int PracticeCost, string cmdForm, string nounForm) {
        if ((Player.Practice > MinPractice) && (Attribute.Get() < MaxAttribute.Get())) {
            Attribute.Set((Attribute.Get() + 1));
            Player.Practice -= PracticeCost;
            Out.WriteLine(TxtTrainAttribute_01 + cmdForm + TxtTrainAttribute_02 + Attribute.Get() + TxtTrainAttribute_03 + nounForm + TxtTrainAttribute_04 + Player.Practice + TxtShared_19);
        }
    }

    public List<TTrainable> Trainables => _trainables ??= [
        new() { cmd = CmdTrenuj2 + CmdSila, stat = TStat.Strength, maxStat = TStat.MaxStrength, minPractice = 2, cost = 3, cmdForm = CmdSile, nounForm = CmdSily },
        new() { cmd = CmdTrenuj2 + CmdZrecznosc, stat = TStat.Dexterity, maxStat = TStat.MaxDexterity, minPractice = 1, cost = 2, cmdForm = CmdZrecznosc, nounForm = CmdZrecznosci },
        new() { cmd = CmdTrenuj2 + CmdMadrosc, stat = TStat.Wisdom, maxStat = TStat.MaxWisdom, minPractice = 2, cost = 3, cmdForm = CmdMadrosc, nounForm = CmdMadrosci },
    ];
    List<TTrainable>? _trainables;

    public void HandleTrain() {
        if (Verb == CmdTrenuj) {
            Out.WriteLine(TxtHandleTrain_01);
        }

        for (int ti = 0; ti < Trainables.Count; ti++) {
            TTrainable t = Trainables[ti];
            if (Matches(t.cmd)) {
                TrainAttribute(IntSlot.Into(Player.Stats, t.stat), IntSlot.Into(Player.Stats, t.maxStat), t.minPractice, t.cost, t.cmdForm, t.nounForm);
            }
        }
    }

    public void DescribePlants() {
        for (int i = 0; i < PlantSpots.Count; i++) {
            if (World.Monsters[PlantSpots[i].where] == Session.Room) {
                Out.WriteLine(PlantSpots[i].text);
            }
        }

        if (Session.Room == World.Monsters[TMonster.Duncan]) {
            Out.WriteLine(TxtDescribePlants_12);
        }
    }

    public void CompositeFight(int EnemyEnergyBase, int EnemyEnergyRand, int EnemyDexterityBase, int EnemyDexterityRand, int EnemyStrengthBase, int EnemyStrengthRand,
        int LootBase, int LootRand, int HeartChance, bool RequireEnergy, bool AntDoughnut) {
        Combat.EnemyEnergy = EnemyEnergyRand > 0 ? Roll(EnemyEnergyRand) + EnemyEnergyBase : EnemyEnergyBase;

        Combat.EnemyDexterity = EnemyDexterityRand > 0 ? Roll(EnemyDexterityRand) + EnemyDexterityBase : EnemyDexterityBase;

        Combat.EnemyStrength = EnemyStrengthRand > 0 ? Roll(EnemyStrengthRand) + EnemyStrengthBase : EnemyStrengthBase;

        DoFight();
        if (!Combat.Fled && (!RequireEnergy || (Player.Energy > 0))) {
            Combat.Loot = (Roll(LootRand) + LootBase);
            Out.WriteLine(TxtShared_08 + Combat.Loot + TxtCompositeFight_01);
            Player.Money += Combat.Loot;
            if (AntDoughnut) {
                if (Verb == CmdZabij && Rest == CmdMrowka) {
                    Combat.Roll = Roll(10);
                    if (Combat.Roll < 7) {
                        Out.WriteLine(TxtShared_08 + TxtCompositeFight_02);
                        AcquireItem(TBagItem.Doughnut);
                    }
                }
            } else if (HeartChance > 0) {
                Combat.Roll = Roll(20);
                if (Combat.Roll < HeartChance) {
                    Out.WriteLine(TxtShared_08 + TxtCompositeFight_03);
                    AcquireItem(TBagItem.Heart);
                }
            }
        }
    }


    public TEnumList<TFoe, TFoeStats> Foes => _foes ??= new([
        new() { energyBase = 1, energyRand = 0, dexterityBase = 1, dexterityRand = 0,
            strengthBase = 2, strengthRand = 0, lootBase = 0, lootRand = 3,
            heartChance = 0, requireEnergy = false, antDoughnut = true, },
        new() { energyBase = 34, energyRand = 3, dexterityBase = 8, dexterityRand = 4,
            strengthBase = 10, strengthRand = 0, lootBase = 10, lootRand = 21,
            heartChance = 5, requireEnergy = false, antDoughnut = false, },
        new() { energyBase = 50, energyRand = 5, dexterityBase = 10, dexterityRand = 5,
            strengthBase = 13, strengthRand = 2, lootBase = 30, lootRand = 30,
            heartChance = 7, requireEnergy = false, antDoughnut = false, },
        new() { energyBase = 60, energyRand = 6, dexterityBase = 12, dexterityRand = 3,
            strengthBase = 15, strengthRand = 3, lootBase = 20, lootRand = 40,
            heartChance = 7, requireEnergy = false, antDoughnut = false, },
        new() { energyBase = 90, energyRand = 10, dexterityBase = 14, dexterityRand = 2,
            strengthBase = 17, strengthRand = 2, lootBase = 10, lootRand = 55,
            heartChance = 4, requireEnergy = true, antDoughnut = false, },
        new() { energyBase = 100, energyRand = 20, dexterityBase = 16, dexterityRand = 0,
            strengthBase = 18, strengthRand = 2, lootBase = 30, lootRand = 30,
            heartChance = 4, requireEnergy = true, antDoughnut = false, },
        new() { energyBase = 130, energyRand = 20, dexterityBase = 20, dexterityRand = 0,
            strengthBase = 19, strengthRand = 2, lootBase = 20, lootRand = 45,
            heartChance = 4, requireEnergy = true, antDoughnut = false, },
        new() { energyBase = 72, energyRand = 3, dexterityBase = 13, dexterityRand = 2,
            strengthBase = 16, strengthRand = 2, lootBase = 15, lootRand = 50,
            heartChance = 5, requireEnergy = false, antDoughnut = false, },
    ]);
    TEnumList<TFoe, TFoeStats>? _foes;


    public TCageStats CageWeaklingStats => _cageWeaklingStats ??= new() { energy = 20, dexterity = 3, strength = 3, lootRand = 10, lootThreshold = 4, };
    TCageStats? _cageWeaklingStats;
    public TCageStats CageStrongStats => _cageStrongStats ??= new() { energy = 20, dexterity = 3, strength = 10, lootRand = 15, lootThreshold = 7, };
    TCageStats? _cageStrongStats;
    public TCageStats CageAgileStats => _cageAgileStats ??= new() { energy = 20, dexterity = 30, strength = 3, lootRand = 15, lootThreshold = 7, };
    TCageStats? _cageAgileStats;
    public TCageStats CageToughStats => _cageToughStats ??= new() { energy = 40, dexterity = 3, strength = 3, lootRand = 15, lootThreshold = 7, };
    TCageStats? _cageToughStats;
    public TCageStats CageHugeStats => _cageHugeStats ??= new() { energy = 40, dexterity = 11, strength = 10, lootRand = 30, lootThreshold = 10, };
    TCageStats? _cageHugeStats;

    public void Fight(TFoe foe) {
        TFoeStats f = Foes[foe];
        CompositeFight(f.energyBase, f.energyRand, f.dexterityBase, f.dexterityRand,
            f.strengthBase, f.strengthRand, f.lootBase, f.lootRand, f.heartChance,
            f.requireEnergy, f.antDoughnut);
    }

    public void KillBeast(string cmd, IntSlot beast, TFoe foe) {
        if ((Matches(cmd)) && (Session.Room == beast.Get())) {
            Fight(foe);
            beast.Set(0);
        }
    }

    public void KillBeastUnlessFled(string cmd, IntSlot beast, TFoe foe) {
        if ((Matches(cmd)) && (Session.Room == beast.Get())) {
            Fight(foe);
            if (!Combat.Fled) {
                beast.Set(0);
            }
        }
    }


    public void CompareFoeRules(List<TCompareFoeRule> rules) {
        for (int i = 0; i < rules.Count; i++) {
            TCompareFoeRule rule = rules[i];
            if ((rule.stat.Get() >= rule.lo) && (rule.stat.Get() <= rule.hi)) {
                Out.WriteLine(rule.line);
            }
        }
    }

    IntSlot SessionOverall() =>
        new(() =>
            Session.Overall, v => Session.Overall = v);
    IntSlot PlayerStrength() =>
        IntSlot.Into(Player.Stats, TStat.Strength);

    public List<TCompareFoeRule> BeastCompareRules => _beastCompareRules ??= [
        new(SessionOverall(), -MaxInt - 1, 15, TxtCompareFoe_02),
        new(PlayerStrength(), 16, 18, TxtEnemy + TxtCompareFoe_03),
        new(PlayerStrength(), 19, MaxInt, TxtCompareFoe_04),
    ];
    List<TCompareFoeRule>? _beastCompareRules;

    public List<TCompareFoeRule> BigBeastCompareRules => _bigBeastCompareRules ??= [
        new(SessionOverall(), -MaxInt - 1, 15, TxtCompareFoe_05),
        new(PlayerStrength(), 16, 20, TxtCompareFoe_06),
        new(PlayerStrength(), 21, 22, TxtCompareFoe_07),
        new(PlayerStrength(), 23, MaxInt, TxtCompareFoe_08),
    ];
    List<TCompareFoeRule>? _bigBeastCompareRules;

    public List<TCompareFoeRule> FighterCompareRules => _fighterCompareRules ??= [
        new(SessionOverall(), -MaxInt - 1, 14, TxtCompareFoe_09),
        new(SessionOverall(), 15, 17, TxtCompareFoe_10),
        new(SessionOverall(), 18, 22, TxtCompareFoe_11),
        new(SessionOverall(), 23, 25, TxtCompareFoe_12),
        new(SessionOverall(), 26, MaxInt, TxtCompareFoe_13),
    ];
    List<TCompareFoeRule>? _fighterCompareRules;

    public List<TCompareFoeRule> DrunkCompareRules => _drunkCompareRules ??= [
        new(SessionOverall(), -MaxInt - 1, 23, TxtCompareFoe_14),
        new(SessionOverall(), 24, 25, TxtCompareFoe_15),
        new(SessionOverall(), 26, 28, TxtCompareFoe_16),
        new(SessionOverall(), 29, MaxInt, TxtCompareFoe_17),
        new(SessionOverall(), 35, MaxInt, TxtCompareFoe_18),
    ];
    List<TCompareFoeRule>? _drunkCompareRules;

    public List<TCompareFoeRule> MusicianCompareRules => _musicianCompareRules ??= [
        new(SessionOverall(), -MaxInt - 1, 20, TxtCompareFoe_19),
        new(SessionOverall(), 23, 27, TxtCompareFoe_20),
        new(SessionOverall(), 28, MaxInt, TxtCompareFoe_21),
    ];
    List<TCompareFoeRule>? _musicianCompareRules;

    public List<TCompareFoeRule> IvyCompareRules => _ivyCompareRules ??= [
        new(SessionOverall(), -MaxInt - 1, 19, TxtCompareFoe_22),
        new(SessionOverall(), 20, 25, TxtCompareFoe_23),
        new(SessionOverall(), 26, 29, TxtCompareFoe_24),
        new(SessionOverall(), 30, MaxInt, TxtCompareFoe_25),
        new(SessionOverall(), 36, MaxInt, TxtCompareFoe_26),
    ];
    List<TCompareFoeRule>? _ivyCompareRules;

    public List<TCompareFoeRule> LiroyCompareRules => _liroyCompareRules ??= [
        new(SessionOverall(), -MaxInt - 1, 27, TxtCompareFoe_27),
        new(SessionOverall(), 28, 32, TxtCompareFoe_28),
        new(SessionOverall(), 33, 43, TxtCompareFoe_29),
        new(SessionOverall(), 44, 47, TxtCompareFoe_30),
        new(SessionOverall(), 48, MaxInt, TxtCompareFoe_31),
    ];
    List<TCompareFoeRule>? _liroyCompareRules;

    public List<TCompareFoeRule> PokrzywaCompareRules => _pokrzywaCompareRules ??= [
        new(SessionOverall(), -MaxInt - 1, 39, TxtCompareFoe_32),
        new(SessionOverall(), 40, MaxInt, TxtCompareFoe_33),
    ];
    List<TCompareFoeRule>? _pokrzywaCompareRules;

    public void CompareFoe() {
        Combat.Roll = Roll(100);
        Out.WriteLine(TxtCompareFoe_01);
        Command = Crt.ReadLn();
        Session.Overall = (Player.Level + Player.Stats[TStat.Strength] + DexterityBonus(Player.Stats[TStat.Dexterity]) + ParryBonus(Skills.Values[TSkill.Parry]) + KickBonus(Skills.Values[TSkill.Kick]));
        switch (Verb) {
            case CmdDzik:
            case CmdSzczur:
            case CmdLis:
            case CmdKuropatwa:
            case CmdZajac:
            case CmdWilczur:
            case CmdOrzel:
            case CmdSarna:
            case CmdDziecko:
            case CmdDziadek:
                CompareFoeRules(BeastCompareRules);
                break;
            case CmdSlon:
            case CmdLew:
            case CmdZyrafa:
            case CmdWielblad:
            case CmdStrus:
            case CmdBoa:
            case CmdWilk:
            case CmdBizon:
            case CmdPantera:
            case CmdWariat:
            case CmdSluchacz:
            case CmdFan:
            case CmdCzlowiek:
            case CmdReporter:
                CompareFoeRules(BigBeastCompareRules);
                break;
            case CmdGladiator:
            case CmdWojownik:
            case CmdTrener:
            case CmdPolicjant:
            case CmdGoryl:
            case CmdOchroniarz:
                CompareFoeRules(FighterCompareRules);
                break;
            case CmdMiniBarman:
            case CmdGrubas:
            case CmdDJ:
                CompareFoeRules(DrunkCompareRules);
                break;
            case CmdTaksowkarz:
            case CmdSprzedawca:
            case CmdGitarzysta:
            case CmdPerkusista:
            case CmdOrganista:
            case CmdZebrak:
            case CmdPijak:
            case CmdZamiatacz:
            case CmdSzczaw:
            case CmdStokrotka:
            case CmdKoniczynka:
            case CmdMlecz:
            case CmdDmuchawiec:
                CompareFoeRules(MusicianCompareRules);
                break;
            case CmdPedal:
            case CmdPara:
            case CmdMaciek:
            case CmdRoza:
            case CmdMalina:
            case CmdTrawa:
            case CmdAgrest:
            case CmdJezyna:
                CompareFoeRules(IvyCompareRules);
                break;
            case CmdLiroy:
                CompareFoeRules(LiroyCompareRules);
                break;
            case CmdPokrzywa:
                CompareFoeRules(PokrzywaCompareRules);
                break;
            case CmdSpaniel:
            case CmdOwczarek:
            case CmdPiesek:
            case CmdJamnik:
            case CmdPudel:
                Out.WriteLine(TxtCompareFoe_34);
                break;
            case CmdBakteria:
                Out.WriteLine(TxtCompareFoe_36);
                Player.ChangeMana(5);
                break;
        }
        if ((Combat.Roll < 3) && (Skills.Values[TSkill.Comparing] < 100)) {
            Out.WriteLine(TxtFightStars17 + TxtLearnVerb + TxtCompareFoe_35);
            Skills.Values[TSkill.Comparing]++;
            Player.Experience += 5;
        }
    }

    public void DescribeStreet() {
        for (int i = 0; i < StreetSpots.Count; i++) {
            if (World.Monsters[StreetSpots[i].where] == Session.Room) {
                Out.WriteLine(StreetSpots[i].text);
            }
        }
    }




    public void DescribeStage() {
        for (int i = 0; i < StageSpots.Count; i++) {
            if (World.Monsters[StageSpots[i].where] == Session.Room) {
                Out.WriteLine(StageSpots[i].text);
            }
        }
    }

    public void DescribeCrowd() {
        for (int i = 0; i < CrowdSpots.Count; i++) {
            if (World.Monsters[CrowdSpots[i].where] == Session.Room) {
                Out.WriteLine(CrowdSpots[i].text);
            }
        }
    }

    public void DescribeBeasts() {
        for (int i = 0; i < BeastSpots.Count; i++) {
            if (World.Monsters[BeastSpots[i].where] == Session.Room) {
                Out.WriteLine(BeastSpots[i].text);
            }
        }
    }

    public void ShieldBlock() {
        Combat.Roll = Roll(100);
        if (Combat.Roll <= Gear.Values[TGearItem.Armor]) {
            Combat.Damage -= Gear.Values[TGearItem.WornCount];
            if (Combat.Damage < 0) {
                Combat.Damage = 0;
            }

            Out.WriteLine(TxtShieldBlock_01 + Combat.Damage + TxtShared_20);
        }
    }

    public void DoFight() {
        for (int i = 0; i < FightExpStats.Count; i++) {
            TFightExpStat s = FightExpStats[i];
            Combat.FightExperience += s.expFor((s.playerStat.Get() - s.enemyStat.Get()));
        }

        do {
            Combat.EnemyDodged = false;
            Combat.PlayerDodged = false;
            Out.WriteLine(TxtFightEngagedEnemy + Combat.EnemyEnergy + TxtFightEngagedYou + Player.Energy + TxtFightEngagedEnd);
            if (Player.Stats[TStat.Dexterity] > Combat.EnemyDexterity) {
                Combat.DodgeChance = (Player.Stats[TStat.Dexterity] - Combat.EnemyDexterity);
                Combat.Roll = Roll(DodgeBound(Combat.DodgeChance));
                if (Combat.Roll < 10) {
                    Out.WriteLine(TxtFightYouDodgeMocking);
                    Combat.PlayerDodged = true;
                }
                Combat.Roll = Roll(EnemyDodgeBound(Combat.DodgeChance));
                if (Combat.Roll < 10) {
                    Out.WriteLine(TxtEnemy + TxtFightEnemyDodgesLamersAttack);
                    Combat.EnemyDodged = true;
                }
            }
            if (Combat.EnemyDexterity > Player.Stats[TStat.Dexterity]) {
                Combat.DodgeChance = (Combat.EnemyDexterity - Player.Stats[TStat.Dexterity]);
                Combat.Roll = Roll(DodgeBound(Combat.DodgeChance));
                if (Combat.Roll < 10) {
                    Out.WriteLine(TxtEnemy + TxtFightEnemyDodgesWithGrace);
                    Combat.EnemyDodged = true;
                }
                Combat.Roll = Roll(EnemyDodgeBound(Combat.DodgeChance));
                if (Combat.Roll < 10) {
                    Out.WriteLine(TxtFightYouDodgeWithoutPardon);
                    Combat.PlayerDodged = true;
                }
            }
            if (!Combat.PlayerDodged) {
                Combat.Damage = Roll(Combat.EnemyStrength);
                switch (Combat.Damage) {
                    case >= 0 and <= 5:
                        Out.WriteLine(TxtEnemy + TxtFightScratchedLabel + Combat.Damage + TxtFightTailEnergii);
                        break;
                    case >= 6 and <= 20:
                        Out.WriteLine(TxtFightTakenLabel + Combat.Damage + TxtFightTailEnergii);
                        break;
                    case >= 21 and <= 50:
                        Out.WriteLine(TxtEnemy + TxtFightMassacredYouLabel + Combat.Damage + TxtFightTailEnergii);
                        break;
                    default:
                        Out.WriteLine(TxtEnemy + TxtFightGougedYouLabel + Combat.Damage + TxtFightTailEnergii);
                        break;
                }
                Combat.FightExperience += 1;
                ShieldBlock();
                if ((Skills.Values[TSkill.Parry] > 0) && (Combat.Damage > 0)) {
                    Combat.Roll = Roll(140);
                    if (Combat.Roll <= Skills.Values[TSkill.Parry]) {
                        switch (Combat.Damage) {
                            case >= 0 and <= 2:
                                Out.WriteLine(TxtFightParryLabel + TxtFightParryZeroTail);
                                Combat.Damage = 0;
                                break;
                            case >= 3 and <= 9:
                                Combat.Damage -= 2;
                                Out.WriteLine(TxtFightParryLabel + Combat.Damage + TxtFightTailEnergii);
                                break;
                            default:
                                Combat.Damage -= 3;
                                Out.WriteLine(TxtFightParryLabel + Combat.Damage + TxtFightTailEnergii);
                                break;
                        }
                        if ((Combat.Roll < 1) && (Skills.Values[TSkill.Parry] < 100)) {
                            Out.WriteLine(TxtFightStars15 + TxtLearnVerb + TxtFightParryLearn);
                            Skills.Values[TSkill.Parry]++;
                            Combat.FightExperience += 5;
                        }
                    }
                }
                if (Combat.Damage > 0) {
                    Player.ChangeEnergy(-Combat.Damage);
                }
                if (Combat.FireballCharges > 0) {
                    Combat.Roll = Roll(100);
                    if (Combat.Roll < 10) {
                        Crt.TextColor(14);
                        Combat.Roll = Roll(20);
                        Out.WriteLine(TxtEnemy + TxtFightFireballLabel + Combat.Roll + TxtFightTailEnergii);
                        Player.ChangeEnergy(-Combat.Roll);
                        Combat.FireballCharges -= 1;
                        Crt.TextColor(15);
                    }
                }
                if (Combat.PoisonCharges > 0) {
                    Combat.Roll = Roll(100);
                    if (Combat.Roll < 10) {
                        Crt.TextColor(4);
                        Out.WriteLine(TxtEnemy + TxtFightCastsPoison);
                        Combat.PoisonCharges -= 1;
                        Crt.TextColor(15);
                        Combat.PoisonRounds += 10;
                    }
                }
                if (Combat.PoisonRounds > 0) {
                    Combat.Roll = Roll(5);
                    Out.WriteLine(TxtFightPoisonedLabel + Combat.Roll + TxtFightTailEnergii);
                    Player.ChangeEnergy(-Combat.Roll);
                    Combat.PoisonRounds -= 1;
                }
            }
            if (!Combat.EnemyDodged) {
                Combat.Damage = Roll(Player.Stats[TStat.Strength]);
                if (Combat.Damage * 10 < Gear.Values[TGearItem.LuckRoll]) {
                    do {
                        Combat.Damage = Roll(Player.Stats[TStat.Strength]);
                        Out.WriteLine(TxtFightLuckRoll);
                    } while (!((Combat.Damage * 10) > Gear.Values[TGearItem.LuckRoll]));
                }
                Combat.EnemyEnergy -= Combat.Damage;
                switch (Combat.Damage) {
                    case >= 0 and <= 5:
                        Out.WriteLine(TxtFightPushedLabel + Combat.Damage + TxtFightTailEnergii);
                        break;
                    case >= 6 and <= 20:
                        Out.WriteLine(TxtFightHitLabel + Combat.Damage + TxtFightTailEnergii);
                        break;
                    case >= 21 and <= 50:
                        Out.WriteLine(TxtFightMassacredHimLabel + Combat.Damage + TxtFightTailEnergii);
                        break;
                    default:
                        Out.WriteLine(TxtFightGougedHimLabel + Combat.Damage + TxtFightTailEnergii);
                        break;
                }
            }
            Crt.Delay(2000);
            if ((Skills.Values[TSkill.Kick] > 0) && (Player.Mana > Skills.Values[TSkill.KickMinMana]) && (Player.Energy < Skills.Values[TSkill.KickMaxEnergy])) {
                Combat.Roll = Roll(100);
                if (Combat.Roll <= Skills.Values[TSkill.Kick] - 10) {
                    Combat.Roll = (Roll(10) + Roll(Player.Level));
                    Crt.TextColor(14);
                    Out.WriteLine(TxtFightKickVerb + TxtFightSuperKickLabel + Combat.Roll + TxtFightTailEnergii);
                    Crt.TextColor(15);
                    Player.ChangeMana(-(Roll(3) + 3));
                    Combat.EnemyEnergy -= Combat.Roll;
                } else {
                    Out.WriteLine(TxtFightKickVerb + TxtFightSuperKickMiss);
                }

                Player.ChangeMana(-(Roll(2) + 2));
            }
            if ((Skills.Values[TSkill.Fleeing] > 0) && (Player.Energy < Skills.Values[TSkill.FleeEnergy]) && (Player.Mana > 14)) {
                Player.ChangeMana(-15);
                Combat.Roll = Roll(100);
                if (Combat.Roll <= Skills.Values[TSkill.Fleeing]) {
                    Combat.Fled = true;
                    Out.WriteLine(TxtFightFled);
                    Player.Experience -= 20;
                } else {
                    Out.WriteLine(TxtFightFleeFailed);
                }
            }
        } while (!((Player.Energy < 1) || (Combat.EnemyEnergy < 1) || Combat.Fled));
        if ((Combat.EnemyEnergy < 1) && (Player.Energy > 0)) {
            if (Player.MaxEnergy > 75) {
                Combat.FightExperience -= 2;
            }

            if (Player.MaxEnergy > 115) {
                Combat.FightExperience -= 3;
            }

            if (Skills.Values[TSkill.Parry] > 50) {
                Combat.FightExperience -= 2;
            }

            if (Skills.Values[TSkill.Parry] > 75) {
                Combat.FightExperience -= 2;
            }

            if (Skills.Values[TSkill.Parry] > 95) {
                Combat.FightExperience -= 1;
            }

            if (Skills.Values[TSkill.Kick] > 50) {
                Combat.FightExperience -= 5;
            }

            if (Skills.Values[TSkill.Kick] > 95) {
                Combat.FightExperience -= 2;
            }

            if (Combat.FightExperience < 0) {
                Combat.FightExperience = 0;
            }

            Out.WriteLine(TxtFightVictoryLabel + Combat.FightExperience + TxtShared_22);
            Player.Experience = (Combat.FightExperience + Player.Experience);
            Combat.FireballCharges = 0;
            Combat.PoisonCharges = 0;

            if (World.Quest > 0) {
                World.QuestKills -= 1;
            }

            if (Skills.Values[TSkill.Cooking] > 0) {
                Combat.Roll = Roll(100);
                if (Skills.Values[TSkill.Cooking] > Combat.Roll) {
                    Out.WriteLine(TxtFightMadeBigos);
                    AcquireItem(TBagItem.Bigos);
                    if ((Combat.Roll < 1) && (Skills.Values[TSkill.Cooking] < 100)) {
                        Out.WriteLine(TxtFightCookingLearn);
                        Skills.Values[TSkill.Cooking]++;
                        Player.Experience += 10;
                    }
                }
            }
        }
        if (Player.Energy < 1) {
            Out.WriteLine(TxtFightKilled);
            Crt.Delay(3000);
            Session.Room = DeadRoom;
        }
        Combat.FightExperience = 0;
    }


    public List<TFightExpStat> FightExpStats => _fightExpStats ??= [
        new() { playerStat = new IntSlot(() => Player.MaxEnergy, v => Player.MaxEnergy = v), enemyStat = new IntSlot(() => Combat.EnemyEnergy, v => Combat.EnemyEnergy = v), expFor = FightExpForEnergy },
        new() { playerStat = PlayerStat(TStat.Strength), enemyStat = new IntSlot(() => Combat.EnemyStrength, v => Combat.EnemyStrength = v), expFor = FightExpForStrength },
        new() { playerStat = PlayerStat(TStat.Dexterity), enemyStat = new IntSlot(() => Combat.EnemyDexterity, v => Combat.EnemyDexterity = v), expFor = FightExpForDexterity },
    ];
    List<TFightExpStat>? _fightExpStats;


















    internal IntSlot PlayerEnergy() =>
        new(() => Player.Energy, v => Player.Energy = v);
    IntSlot PlayerStat(TStat stat) =>
        new(() => Player.Stats[stat], v => Player.Stats[stat] = v);
    IntSlot BagItemSlot(TBagItem item) =>
        new(() => Bag.Items[item], v => Bag.Items[item] = v);
    internal IntSlot PlayerMaxEnergy() =>
        new(() => Player.MaxEnergy, v => Player.MaxEnergy = v);
    internal IntSlot PlayerMana() =>
        new(() => Player.Mana, v => Player.Mana = v);
    internal IntSlot PlayerMaxMana() =>
        new(() => Player.MaxMana, v => Player.MaxMana = v);





    public void DisplayPrefs() {
        switch (Verb) {
            case CmdZmien:
                switch (Rest) {
                    case CmdKolor:
                        Out.WriteLine(TxtDisplayPrefs_01);
                        Combat.Roll = Crt.ReadInt();
                        Crt.TextColor(Combat.Roll);
                        break;
                    case CmdTlo:
                        Out.WriteLine(TxtDisplayPrefs_02);
                        Combat.Roll = Crt.ReadInt();
                        Crt.TextBackground(Combat.Roll);
                        break;
                }
                break;
        }
    }


    public List<TBeastKill> BeastKills => _beastKills ??= [
        new() { cmd = CmdZabij + CmdSlimak, beast = TMonster.Snail, foe = TFoe.WeakEnemy, unlessFled = true },
        new() { cmd = CmdZabij + CmdKornik, beast = TMonster.Woodworm, foe = TFoe.WeakEnemy, unlessFled = true },
        new() { cmd = CmdZabij + CmdMucha, beast = TMonster.Fly, foe = TFoe.WeakEnemy, unlessFled = true },
        new() { cmd = CmdZabij + CmdZuk, beast = TMonster.Beetle, foe = TFoe.WeakEnemy, unlessFled = true },
        new() { cmd = CmdZabij + CmdKaraluch, beast = TMonster.Cockroach, foe = TFoe.WeakEnemy, unlessFled = false },
        new() { cmd = CmdZabij + CmdMrowka, beast = TMonster.Ant, foe = TFoe.WeakEnemy, unlessFled = false },
        new() { cmd = CmdZabij + CmdPajak, beast = TMonster.Spider, foe = TFoe.WeakEnemy, unlessFled = false },
        new() { cmd = CmdZabij + CmdDzik, beast = TMonster.Boar, foe = TFoe.WeakishEnemy, unlessFled = false },
        new() { cmd = CmdZabij + CmdSzczur, beast = TMonster.Rat, foe = TFoe.WeakishEnemy, unlessFled = false },
        new() { cmd = CmdZabij + CmdLis, beast = TMonster.Fox, foe = TFoe.WeakishEnemy, unlessFled = false },
        new() { cmd = CmdZabij + CmdKuropatwa, beast = TMonster.Partridge, foe = TFoe.WeakishEnemy, unlessFled = false },
        new() { cmd = CmdZabij + CmdZajac, beast = TMonster.Hare, foe = TFoe.WeakishEnemy, unlessFled = false },
        new() { cmd = CmdZabij + CmdOrzel, beast = TMonster.Eagle, foe = TFoe.WeakishEnemy, unlessFled = false },
        new() { cmd = CmdZabij + CmdSarna, beast = TMonster.Deer, foe = TFoe.WeakishEnemy, unlessFled = false },
        new() { cmd = CmdZabij + CmdSlon, beast = TMonster.Elephant, foe = TFoe.MediumEnemy, unlessFled = false },
        new() { cmd = CmdZabij + CmdLew, beast = TMonster.Lion, foe = TFoe.MediumEnemy, unlessFled = false },
        new() { cmd = CmdZabij + CmdZyrafa, beast = TMonster.Giraffe, foe = TFoe.MediumEnemy, unlessFled = false },
        new() { cmd = CmdZabij + CmdWilk, beast = TMonster.Wolf, foe = TFoe.MediumEnemy, unlessFled = false },
        new() { cmd = CmdZabij + CmdWielblad, beast = TMonster.Camel, foe = TFoe.MediumEnemy, unlessFled = false },
        new() { cmd = CmdZabij + CmdStrus, beast = TMonster.Ostrich, foe = TFoe.MediumEnemy, unlessFled = false },
        new() { cmd = CmdZabij + CmdBoa, beast = TMonster.Boa, foe = TFoe.MediumEnemy, unlessFled = false },
        new() { cmd = CmdZabij + CmdBizon, beast = TMonster.Bison, foe = TFoe.MediumEnemy, unlessFled = false },
        new() { cmd = CmdZabij + CmdPantera, beast = TMonster.Panther, foe = TFoe.MediumEnemy, unlessFled = false },
    ];
    List<TBeastKill>? _beastKills;


    public List<TArenaKill> ArenaKills => _arenaKills ??= [
        new() { cmd = CmdZabij + CmdGladiator, monster = TMonster.Gladiator, pills = true },
        new() { cmd = CmdZabij + CmdWojownik, monster = TMonster.Warrior, pills = true },
        new() { cmd = CmdZabij + CmdTrener, monster = TMonster.Tamer, pills = false },
    ];
    List<TArenaKill>? _arenaKills;

    public void ArenaKill(string cmd, TMonster monster, bool pills) {
        if ((Matches(cmd)) && (Session.Room == World.Monsters[monster])) {
            Fight(TFoe.HardEnemy);
            World.Monsters[monster] = 0;
            if (pills) {
                PillGain();
            } else {
                SuitGain();
            }
        }
    }


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

    public string MazeDirectionCommand(TMazeDir dir) =>
        dir switch {
            TMazeDir.mdPolodnie => CmdPolodnie,
            TMazeDir.mdPolnoc => CmdPolnoc,
            TMazeDir.mdWschod => CmdWschod,
            TMazeDir.mdZachod => CmdZachod,
            _ => throw new ArgumentOutOfRangeException(nameof(dir)),
        };

    public void MazeMove(TMazeDir dir) {
        TMazeMove? maze = MazeMoves.FirstOrDefault(move => move.room == Session.Room);
        if (maze?.exits.Contains(dir) != true) {
            return;
        }

        int? destination = Rooms.RoomInDirection(Session.Room, MazeDirectionCommand(dir));
        if (destination is int room) {
            MoveTo(room);
        }
    }

    public void StageFights() {
        switch (Verb) {
            case CmdZabij:
                switch (Rest) {
                    case CmdPerkusista:
                        if (World.Monsters[TMonster.Drummer] == Session.Room) {
                            Fight(TFoe.VeryEasyEnemy);
                            World.Monsters[TMonster.Drummer] = 0;
                            CassetteGain();
                        }
                        break;
                    case CmdGitarzysta:
                        if (World.Monsters[TMonster.Guitarist] == Session.Room) {
                            Fight(TFoe.VeryEasyEnemy);
                            World.Monsters[TMonster.Guitarist] = 0;
                            CassetteGain();
                        }
                        break;
                    case CmdLiroy:
                        if (World.Monsters[TMonster.Liroy] == Session.Room) {
                            Fight(TFoe.NotEasyEnemy);
                            if ((Session.Room != DeadRoom) && (Player.Energy > 0) && !Combat.Fled) {
                                Out.WriteLine(TxtStageFights_01);
                                Player.Money += 30;
                                CassetteGain();
                                World.LiroyKilled = true;
                                World.Monsters[TMonster.Liroy] = 0;
                            }
                        }
                        break;
                    case CmdOrganista:
                        if (World.Monsters[TMonster.Keyboardist] == Session.Room) {
                            Fight(TFoe.VeryEasyEnemy);
                            World.Monsters[TMonster.Keyboardist] = 0;
                            CassetteGain();
                        }
                        break;
                }
                break;
        }
    }

    public void KillPlant(string cmd, IntSlot Plant) {
        if ((Matches(cmd)) && (Plant.Get() == Session.Room)) {
            Fight(TFoe.VeryEasyEnemy);
            Plant.Set(0);
        }
    }

    public void KillPlantIfAlive(string cmd, IntSlot Plant) {
        if ((Matches(cmd)) && (Plant.Get() == Session.Room)) {
            Fight(TFoe.VeryEasyEnemy);
            if (Player.Energy > 0) {
                Plant.Set(0);
            }
        }
    }

    public void KillPlantEasyIfAlive(string cmd, IntSlot Plant) {
        if ((Matches(cmd)) && (Plant.Get() == Session.Room)) {
            Fight(TFoe.EasyEnemy);
            if (Player.Energy > 0) {
                Plant.Set(0);
            }
        }
    }


    public List<TPlantKill> PlantKills => _plantKills ??= [
        new() { cmd = CmdZabij + CmdSzczaw, plant = TMonster.Sorrel },
        new() { cmd = CmdZabij + CmdKoniczynka, plant = TMonster.Clover },
        new() { cmd = CmdZabij + CmdMlecz, plant = TMonster.SowThistle },
        new() { cmd = CmdZabij + CmdDmuchawiec, plant = TMonster.Dandelion },
    ];
    List<TPlantKill>? _plantKills;

    public List<TPlantKill> EasyPlantKills => _easyPlantKills ??= [
        new() { cmd = CmdZabij + CmdRoza, plant = TMonster.Rose },
        new() { cmd = CmdZabij + CmdOset, plant = TMonster.Thistle },
        new() { cmd = CmdZabij + CmdMalina, plant = TMonster.Raspberry },
        new() { cmd = CmdZabij + CmdAgrest, plant = TMonster.Gooseberry },
        new() { cmd = CmdZabij + CmdJezyna, plant = TMonster.Blackberry },
    ];
    List<TPlantKill>? _easyPlantKills;

    public void PlantFights() {
        switch (Verb) {
            case CmdZabij:
                switch (Rest) {
                    case CmdDuncan:
                        if (World.Monsters[TMonster.Duncan] == Session.Room) {
                            Fight(TFoe.EasyEnemy);
                            if (Combat.EnemyEnergy < 1) {
                                World.Monsters[TMonster.Duncan] = 0;
                            }
                        }
                        break;
                    case CmdTrawa:
                        if (World.Monsters[TMonster.Grass] == Session.Room) {
                            Out.WriteLine(TxtPlantFights_17);
                            Fight(TFoe.HardEnemy);
                            for (int i = 2; i <= 4; i++) {
                                if (Player.Energy > 0) {
                                    Fight(TFoe.HardEnemy);
                                }
                            }

                            if ((Player.Energy > 0) && !Combat.Fled) {
                                Out.WriteLine(TxtPlantFights_18);
                            }
                        }
                        break;
                }
                break;
            case CmdSecret:
                if ((Rest == CmdLista) && (World.Monsters[TMonster.Duncan] == Session.Room)) {
                    WriteLines([
                        TxtPlantFights_01,
                        TxtPlantFights_02,
                        TxtPlantFights_03,
                        TxtPlantFights_04,
                    ]);
                }

                break;
            case CmdKup:
                switch (Rest) {
                    case CmdDokument:
                        if (Player.Money > 399) {
                            Out.WriteLine(TxtPlantFights_05);
                            AcquirePermit();
                            Player.Money -= 400;
                        }
                        break;
                    case CmdPlecak:
                        if ((World.Monsters[TMonster.Duncan] == Session.Room) && (Player.Money > 4799)) {
                            Out.WriteLine(TxtPlantFights_06);
                            Player.Money -= 4800;
                            AcquireItem(TBagItem.Backpack);
                        }
                        break;
                }
                break;
            case CmdRozmawiaj:
                if ((Rest == CmdDuncan) && (World.Monsters[TMonster.Duncan] == Session.Room)) {
                    if (World.DuncanQuest == 0) {
                        WriteLines([
                            TxtPlantFights_07,
                            TxtPlantFights_08,
                            TxtPlantFights_09,
                            TxtPlantFights_10,
                            TxtPlantFights_11,
                            TxtPlantFights_12,
                        ]);
                        World.DuncanQuest = 75;
                    }
                    if (World.DuncanQuest != 0) {
                        Out.WriteLine(TxtPlantFights_13);
                    }

                    if (World.DuncanQuest == -125) {
                        Out.WriteLine(TxtPlantFights_14);
                        Out.WriteLine(TxtPlantFights_15);
                        World.DuncanQuest = 0;
                        Player.Experience += 125;
                        Out.WriteLine(TxtPlantFights_16);
                    }
                }
                break;
        }

        for (int i = 0; i < PlantKills.Count; i++) {
            KillPlant(PlantKills[i].cmd, IntSlot.Into(World.Monsters, PlantKills[i].plant));
        }

        KillPlantIfAlive(CmdZabij + CmdStokrotka, IntSlot.Into(World.Monsters, TMonster.Daisy));

        for (int i = 0; i < EasyPlantKills.Count; i++) {
            KillPlantEasyIfAlive(EasyPlantKills[i].cmd, IntSlot.Into(World.Monsters, EasyPlantKills[i].plant));
        }
    }

    public void ReadCommand() {
        Out.Write(Player.Energy + TxtShared_01 + Player.Experience + TxtShared_02);
        Command = Crt.ReadLn();
        switch (Verb) {
            case CmdMode:
            case CmdM:
                Out.WriteLine(TxtModeEgg);
                break;
            case CmdUnmode:
            case CmdUm:
                Out.WriteLine(TxtUnmodeEgg);
                break;
        }
        RefuseCheats();
        HandleGround();
        ExpandAbbrev();
    }

    public void RefuseCheats() {
        bool dawaj = false;
        bool other = false;
        switch (Verb) {
            case CmdDawaj:
                dawaj = Rest == CmdEn || Rest == CmdKase || Rest == CmdKunszt;
                break;
            case CmdZabieraj:
                other = Rest == CmdKase;
                break;
            case CmdUstaw:
                other = Rest == CmdSz;
                break;
            case CmdZdejmuj:
                other = true;
                break;
            case CmdJestem:
                other = Rest == CmdSpeede2;
                break;
        }
        if (!dawaj && !other) {
            return;
        }

        if (!Crt.DebugEnabled()) {
            Out.WriteLine(dawaj ? TxtCheatRefusal : TxtCheatRefusalOther);
            Command = TxtEmpty;
        } else {
            Out.WriteLine($"\"{Command}\"... {(dawaj ? TxtCheatSuccess : TxtCheatSuccessOther)}");
        }
    }

    public void HandleGround() {
        for (int gi = 0; gi < GroundCommands.Count; gi++) {
            if ((GroundCommands[gi].cmd == TxtEmpty) || (Matches(GroundCommands[gi].cmd))) {
                GroundCommands[gi].run();
                if (Session.ExitRequested) {
                    return;
                }
            }
        }
    }

    static readonly List<(string from, string to)> Abbreviations = [
        (CmdPn, CmdPolnoc), (CmdPd, CmdPolodnie), (CmdW, CmdWschod), (CmdZ, CmdZachod),
        (CmdG, CmdGora), (CmdD, CmdDol), (CmdE, CmdExit), (CmdM, CmdMode),
    ];

    public void ExpandAbbrev() {
        for (int i = 0; i < Abbreviations.Count; i++) {
            if (Matches(Abbreviations[i].from)) {
                Command = Abbreviations[i].to;
                return;
            }
        }
    }


    public List<TRace> Races => _races ??= [
        new() { name = CmdCzlowiek, baseStats = new([ 10, 10, 10, 15, 15, 15 ]),
            MaxManaBonus = 0, MoneyBonus = 0, MaxEnergyBonus = 0, EnergyBonus = 0, },
        new() { name = CmdOlbrzym, baseStats = new([ 13, 13, 5, 20, 20, 7 ]),
            MaxManaBonus = -30, MoneyBonus = 0, MaxEnergyBonus = 0, EnergyBonus = 0, },
        new() { name = CmdNimfa, baseStats = new([ 10, 13, 7, 15, 20, 11 ]),
            MaxManaBonus = 0, MoneyBonus = 0, MaxEnergyBonus = 0, EnergyBonus = 0, },
        new() { name = CmdPolElf, baseStats = new([ 6, 16, 8, 10, 24, 12 ]),
            MaxManaBonus = -20, MoneyBonus = 30, MaxEnergyBonus = -5, EnergyBonus = -5, },
        new() { name = CmdUfok, baseStats = new([ 9, 9, 13, 13, 14, 20 ]),
            MaxManaBonus = 50, MoneyBonus = 0, MaxEnergyBonus = 0, EnergyBonus = 0, },
        new() { name = CmdCzaromil, baseStats = new([ 6, 7, 16, 9, 11, 25 ]),
            MaxManaBonus = 150, MoneyBonus = 0, MaxEnergyBonus = 0, EnergyBonus = 0, },
    ];
    List<TRace>? _races;


    public List<TDexterityLimit> DexterityItemLimits => _dexterityItemLimits ??= [
        new() { gate = 0, limit = 10 }, new() { gate = 11, limit = 11 },
        new() { gate = 13, limit = 12 }, new() { gate = 16, limit = 13 },
        new() { gate = 19, limit = 14 }, new() { gate = 21, limit = 15 },
        new() { gate = 25, limit = 16 },
    ];
    List<TDexterityLimit>? _dexterityItemLimits;

    public const int Lvl1_Threshold = 700;
    public const int Lvl2_Threshold = 725;
    public const int Lvl3_Threshold = 730;
    public const int Lvl4_Base = 735;
    public const int Lvl9_Base = 735;

    public List<TBand> LvlThresholds => _lvlThresholds ??= [
        new(1, 1, Lvl1_Threshold, 0),
        new(2, 2, Lvl2_Threshold, 0),
        new(3, 3, Lvl3_Threshold, 0),
        new(4, 8, Lvl4_Base, 1),
        new(9, MaxInt, Lvl9_Base, 2),
    ];
    List<TBand>? _lvlThresholds;

    public int LevelThreshold(int lvl) =>
        BandAmount(LvlThresholds, lvl);


    public List<TSheetItem> SheetItems => _sheetItems ??= [
        new() { item = BagItemSlot(TBagItem.OldSword), desc = CmdStaryZardzewialyMiecz, width = 0, kind = TSheetKind.ikShown },
        new() { item = BagItemSlot(TBagItem.SmallShield), desc = CmdOkraglaMalaTarcza, width = 0, kind = TSheetKind.ikShown },
        new() { item = BagItemSlot(TBagItem.Heart), desc = CmdZakrwawioneSerce, width = 0, kind = TSheetKind.ikShown },
        new() { item = BagItemSlot(TBagItem.Diploma), desc = CmdDyplomSzkolyZycia, width = 0, kind = TSheetKind.ikShown },
        new() { item = BagItemSlot(TBagItem.Doughnut), desc = CmdSmacznyPolanyLikieremPaczek, width = 10, kind = TSheetKind.ikShown },
        new() { item = BagItemSlot(TBagItem.Cookie), desc = CmdSmaczneCiastko, width = 10, kind = TSheetKind.ikShown },
        new() { item = BagItemSlot(TBagItem.DryRation), desc = CmdTwardaSuchaRacje, width = 10, kind = TSheetKind.ikShown },
        new() { item = BagItemSlot(TBagItem.BreadRoll), desc = CmdOkraglaPachnacaBulka, width = 10, kind = TSheetKind.ikShown },
        new() { item = BagItemSlot(TBagItem.Bread), desc = CmdDuzyCieplyChleb, width = 10, kind = TSheetKind.ikShown },
        new() { item = BagItemSlot(TBagItem.Sausage), desc = CmdDlugaISmacznaWeke, width = 10, kind = TSheetKind.ikShown },
        new() { item = BagItemSlot(TBagItem.Bigos), desc = CmdPysznyBigosZKapustaZWroga, width = 10, kind = TSheetKind.ikShown },
        new() { item = BagItemSlot(TBagItem.Beer), desc = CmdOrzezwiajacePiwskoZKonkretnymKlimatem, width = 0, kind = TSheetKind.ikPlain },
        new() { item = BagItemSlot(TBagItem.Pipe), desc = CmdCiezkaFajkaOdDziadaMroza, width = 0, kind = TSheetKind.ikPlain },
        new() { item = BagItemSlot(TBagItem.Tracksuit), desc = TxtKompletUbranFirmySyf, width = 0, kind = TSheetKind.ikPlain },
        new() { item = BagItemSlot(TBagItem.SpikedSuit), desc = TxtGarniturZKolcami, width = 0, kind = TSheetKind.ikPlain },
        new() { item = BagItemSlot(TBagItem.ManaBottle), desc = CmdMalaButelkeMany, width = 0, kind = TSheetKind.ikPlain },
        new() { item = BagItemSlot(TBagItem.LuckyLeaf), desc = CmdDelikatnyChocSzorstkiListek, width = 0, kind = TSheetKind.ikPlain },
        new() { item = BagItemSlot(TBagItem.Cassette), desc = CmdKasetaLiroyaLJakLiroy, width = 0, kind = TSheetKind.ikPlain },
        new() { item = BagItemSlot(TBagItem.TeleportPill), desc = CmdPigulkaTransportujacaMG, width = 0, kind = TSheetKind.ikPlain },
        new() { item = new IntSlot(() => Bag.Permit, v => Bag.Permit = v), desc = CmdPrzepustkaWlasnoreczniePodpisanaPrzezQuestMastera, width = 0, kind = TSheetKind.ikPlain },
        new() { item = BagItemSlot(TBagItem.CompareScroll), desc = CmdScrollZCzaremPorownywanie, width = 0, kind = TSheetKind.ikPlain },
        new() { item = BagItemSlot(TBagItem.ReturnScroll), desc = CmdScrollZCzaremPowrot, width = 0, kind = TSheetKind.ikPlain },
    ];
    List<TSheetItem>? _sheetItems;

    public void ShowSheetItem(TSheetKind kind, int val, string desc, int width) {
        if (val > 0) {
            switch (kind) {
                case TSheetKind.ikShown:
                    Out.WriteLine(width == 0 ? desc : desc + val.ToString().PadLeft(width));
                    break;
                case TSheetKind.ikPlain:
                    Out.WriteLine(desc + val);
                    break;
            }
        }
    }


    public void ShowCharacter() {
        RecalcBag();
        Out.WriteLine($"{TxtShowCharacter_30}{Player.Name}");
        Out.WriteLine(TxtShowCharacter_01);
        for (int si = 0; si < SheetItems.Count; si++) {
            TSheetItem s = SheetItems[si];
            ShowSheetItem(s.kind, s.item.Get(), s.desc, s.width);
        }
        Out.WriteLine(TxtShowCharacter_02 + Bag.Count + TxtShowCharacter_03 + Bag.Limit + TxtShowCharacter_04);
        if (Gear.Values[TGearItem.WeaponKind] == 0) {
            Out.WriteLine(TxtShowCharacter_05);
        }

        if (Gear.Values[TGearItem.ShieldKind] == 0) {
            Out.WriteLine(TxtShowCharacter_06);
        }

        if (Gear.WornGarment == TxtEmpty) {
            Out.WriteLine(TxtShowCharacter_07);
        }

        if (Gear.WornGarment == TxtWornSyf) {
            Out.WriteLine(TxtWearingLabel + TxtKompletUbranFirmySyf);
        }

        if (Gear.WornGarment == TxtWornGarnitur) {
            Out.WriteLine(TxtWearingLabel + TxtGarniturZKolcami);
        }

        if (Gear.Values[TGearItem.ShieldKind] == 1) {
            Out.WriteLine(TxtShowCharacter_10);
        }

        if (Gear.Values[TGearItem.WeaponKind] == 1) {
            Out.WriteLine(TxtShowCharacter_11);
        }

        Out.WriteLine(TxtShowCharacter_12 + Player.Money + TxtShowCharacter_13 + Player.Practice + TxtShared_19);

        if (Player.Level > 0) {
            int gap = (LevelThreshold(Player.Level) - Player.Experience);
            Out.WriteLine(TxtShowCharacter_18 + Player.Level + TxtShowCharacter_19 + gap + TxtShared_22);
        }

        Out.WriteLine(TxtShowCharacter_21 + Player.Stats[TStat.Strength] + TxtShowCharacter_03 + Player.Stats[TStat.MaxStrength]
            + TxtShowCharacter_22 + Player.Stats[TStat.Dexterity] + TxtShowCharacter_03 + Player.Stats[TStat.MaxDexterity]
            + TxtShowCharacter_23 + Player.Stats[TStat.Wisdom] + TxtShowCharacter_03 + Player.Stats[TStat.MaxWisdom]);
        Gear.Values[TGearItem.ArmorRating] = (10 * Gear.Values[TGearItem.WornCount] + Gear.Values[TGearItem.Armor]);
        Out.WriteLine(TxtShowCharacter_24 + Gear.Values[TGearItem.ArmorRating]
            + TxtShowCharacter_25 + Gear.Values[TGearItem.LuckRoll]);
        Out.WriteLine(TxtShowCharacter_12 + Player.Mana + TxtShowCharacter_03 + Player.MaxMana + TxtShowCharacter_26);
        Out.WriteLine(TxtShowCharacter_12 + Player.Energy + TxtShowCharacter_03 + Player.MaxEnergy + TxtShared_20);
        if (World.Quest > 0) {
            bool needLiroy = (World.Quest > 1) && !World.LiroyKilled;
            Out.WriteLine(World.QuestKills > 0
                ? TxtShowCharacter_27 + World.QuestKills + (needLiroy ? TxtShowCharacter_31 : TxtShowCharacter_28)
                : needLiroy ? TxtShowCharacter_32 : TxtShowCharacter_33);
        }

        if (World.DuncanQuest != 0) {
            Out.WriteLine(TxtShowCharacter_29);
        }
    }

    public int FindRace(string name) {
        for (int i = 0; i < Races.Count; i++) {
            if (name == Races[i].name) {
                return i;
            }
        }

        return -1;
    }

    public bool IsNameChar(char c) =>
        (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == ' ';

    public void ChooseRace() {
        int chosen;
        ShowIntro();
        Out.WriteLine(TxtChooseRace_02);
        while (true) {
            Player.Name = Crt.ReadLn();
            if (Player.Name.Length == 0) {
                Out.WriteLine(TxtChooseRace_15);
            } else if (Player.Name.Any(c => !IsNameChar(c))) {
                Out.WriteLine(TxtChooseRace_13);
            } else if (Player.Name.Length > 63) {
                Out.WriteLine(TxtChooseRace_14);
            } else {
                break;
            }
        }
        if (File.Exists(SaveFileName())) {
            LoadGame();
            Session.SavedFlag = false;
            return;
        }
        do {
            Out.WriteLine(TxtChooseRace_01);
            Command = Crt.ReadLn();
            chosen = FindRace(Command);
            if (chosen >= 0) {
                foreach (TStat stat in Enum.GetValues<TStat>()) {
                    Player.Stats[stat] = Races[chosen].baseStats[stat];
                }

                Player.MaxMana += Races[chosen].MaxManaBonus;
                Player.Money += Races[chosen].MoneyBonus;
                Player.MaxEnergy += Races[chosen].MaxEnergyBonus;
            }
        } while (chosen < 0);
        Player.Energy = (50 + Races[chosen].EnergyBonus);
        Player.Practice = 10;
        SpawnAt(CageInits);
        Player.Level = 1;
        Player.MaxMana += 100;
        Player.Mana = Player.MaxMana;
        SpawnMonsters();
        Bag.AddGroundItem(TBagItem.OldSword, RoomStart);
        WriteLines([
            TxtChooseRace_03,
            TxtChooseRace_10,
            TxtChooseRace_11,
            TxtChooseRace_12,
        ]);
    }

    public void KillStreetFoe(string cmd, IntSlot target, bool extra, TStreetKind kind) {
        if ((Matches(cmd)) && (target.Get() == Session.Room)) {
            switch (kind) {
                case TStreetKind.skDog:
                    FightDog();
                    break;
                case TStreetKind.skFolk:
                    Fight(TFoe.VeryEasyEnemy);
                    break;
            }
            target.Set(0);
            if (extra) {
                switch (kind) {
                    case TStreetKind.skDog:
                        WriteLines([TxtKillDog_01, TxtKillDog_02, TxtKillDog_03]);
                        break;
                    case TStreetKind.skFolk:
                        SuitGain();
                        break;
                }
            }
        }
    }

    public void KillSweeper() {
        if ((Verb == CmdZabij && Rest == CmdZamiatacz) && (World.Monsters[TMonster.Sweeper] == Session.Room)) {
            Fight(TFoe.VeryEasyEnemy);
            World.Monsters[TMonster.Sweeper] = 0;
            Combat.Roll = Roll(100);
            if (Combat.Roll < 35) {
                Out.WriteLine(TxtKillSweeper_01);
                AcquireItem(TBagItem.Tracksuit);
            }
        }
    }

    public void KillSickMan() {
        if ((Verb == CmdZabij && Rest == CmdZebrak) && (World.Monsters[TMonster.SickMan] == Session.Room)) {
            Fight(TFoe.VeryEasyEnemy);
            if (Player.Energy > 0) {
                Combat.Roll = Roll(10);
                Out.WriteLine(TxtKillSickMan_01 + Combat.Roll + TxtShared_23);
                Player.Money += Combat.Roll;
                World.Monsters[TMonster.SickMan] = 0;
            }
        }
    }


    public List<TStreetKill> StreetKills => _streetKills ??= [
        new() { cmd = CmdZabij + CmdOwczarek, target = TMonster.Shepherd, extra = false, kind = TStreetKind.skDog },
        new() { cmd = CmdZabij + CmdPiesek, target = TMonster.Pooch, extra = false, kind = TStreetKind.skDog },
        new() { cmd = CmdZabij + CmdJamnik, target = TMonster.Dachshund, extra = false, kind = TStreetKind.skDog },
        new() { cmd = CmdZabij + CmdSpaniel, target = TMonster.Spaniel, extra = true, kind = TStreetKind.skDog },
        new() { cmd = CmdZabij + CmdPudel, target = TMonster.Poodle, extra = false, kind = TStreetKind.skDog },
        new() { cmd = CmdZabij + CmdTaksowkarz, target = TMonster.Cabman, extra = true, kind = TStreetKind.skFolk },
        new() { cmd = CmdZabij + CmdSprzedawca, target = TMonster.Vendor, extra = true, kind = TStreetKind.skFolk },
        new() { cmd = CmdZabij + CmdPijak, target = TMonster.Drunkard, extra = false, kind = TStreetKind.skFolk },
    ];
    List<TStreetKill>? _streetKills;

    public void StreetFights() {
        for (int ki = 0; ki < StreetKills.Count; ki++) {
            TStreetKill k = StreetKills[ki];
            KillStreetFoe(k.cmd, IntSlot.Into(World.Monsters, k.target), k.extra, k.kind);
        }
        KillSweeper();
        KillSickMan();
    }


    public List<TTrainingLine> TrainingLines => _trainingLines ??= [
        new() { wisdom = 10, strength = 11, dexterity = -1, text1 = TxtTraining_03 },
        new() { wisdom = 10, strength = -1, dexterity = 10, text1 = TxtTraining_04 },
        new() { wisdom = 15, strength = -1, dexterity = 11, text1 = TxtTraining_05 },
        new() { wisdom = 11, strength = -1, dexterity = -1, text1 = TxtTraining_06 },
        new() { wisdom = 18, strength = -1, dexterity = -1, text1 = TxtTraining_07 },
        new() { wisdom = 17, strength = -1, dexterity = -1, text1 = TxtTraining_08 },
        new() { wisdom = -1, strength = -1, dexterity = 20, text1 = TxtTraining_09 },
        new() { wisdom = -1, strength = 20, dexterity = 10, text1 = TxtTraining_10 },
        new() { wisdom = 19, strength = 10, dexterity = 10, text1 = TxtTraining_11 },
        new() { wisdom = 12, strength = 25, dexterity = -1, text1 = TxtTraining_12 },
        new() { wisdom = 14, strength = 29, dexterity = -1, text1 = TxtTraining_13 },
        new() { wisdom = -1, strength = -1, dexterity = 22, text1 = TxtTraining_14 },
        new() { wisdom = -1, strength = -1, dexterity = 28, text1 = TxtTraining_15 },
        new() { wisdom = 22, strength = -1, dexterity = -1, text1 = TxtTraining_16 },
        new() { wisdom = 23, strength = -1, dexterity = -1, text1 = TxtTraining_17 },
        new() { wisdom = 14, strength = 14, dexterity = 24, text1 = TxtTraining_18 },
        new() { wisdom = 24, strength = -1, dexterity = -1, text1 = TxtTraining_19 },
        new() { wisdom = 26, strength = -1, dexterity = -1, text1 = TxtTraining_20 },
        new() { wisdom = 19, strength = 12, dexterity = 12, text1 = TxtTraining_21 },
        new() { wisdom = 16, strength = -1, dexterity = 22, text1 = TxtTraining_22 },
        new() { wisdom = -1, strength = -1, dexterity = 30, text1 = TxtTraining_23 },
        new() { wisdom = 29, strength = -1, dexterity = -1, text1 = TxtTraining_24 },
        new() { wisdom = 19, strength = -1, dexterity = -1, text1 = TxtTraining_25 },
        new() { wisdom = 23, strength = -1, dexterity = 12, text1 = TxtTraining_26 },
    ];
    List<TTrainingLine>? _trainingLines;

    public bool GatesMet(int reqWisdom, int reqStrength, int reqDexterity) =>
        ((reqWisdom == -1) || (Player.Stats[TStat.Wisdom] > reqWisdom)) &&
        ((reqStrength == -1) || (Player.Stats[TStat.Strength] > reqStrength)) &&
        ((reqDexterity == -1) || (Player.Stats[TStat.Dexterity] > reqDexterity));

    public void WriteTrainingGain(string skillName, int value) =>
        Out.WriteLine(TxtTrainingVerb + skillName + TxtTrainingThreshold + value
            + TxtTraining_29 + Player.Practice + TxtShared_19);

    public void Training() {
        switch (Verb) {
            case CmdPatrz:
                if (Rest == CmdPlakat) {
                    Out.WriteLine(TxtShared_13);
                    Out.WriteLine(TxtTraining_01);
                    if (Player.Stats[TStat.Wisdom] < 11) {
                        Out.WriteLine(TxtTraining_02);
                    }

                    for (int i = 0; i < TrainingLines.Count; i++) {
                        if (GatesMet(TrainingLines[i].wisdom, TrainingLines[i].strength, TrainingLines[i].dexterity)) {
                            Out.WriteLine(TrainingLines[i].text1);
                        }
                    }

                    Out.WriteLine(TxtTraining_27);
                }
                break;
            case CmdCwicz:
                switch (Rest) {
                    case CmdKopac:
                        if ((Player.Stats[TStat.Wisdom] > 10) && (Player.Stats[TStat.Strength] > 11) && (Player.Practice > 0)) {
                            Skills.Values[TSkill.Kick] += Player.Stats[TStat.Wisdom];
                            Player.Practice -= 1;
                            WriteTrainingGain(TxtTrainingDigging, Skills.Values[TSkill.Kick]);
                        }
                        break;
                    case CmdUciekac:
                        if (Player.Stats[TStat.Wisdom] > 10) {
                            if (Player.Stats[TStat.Dexterity] > 10) {
                                if ((Player.Practice > 0) && (Skills.Values[TSkill.Fleeing] < 85)) {
                                    Skills.Values[TSkill.Fleeing] += Player.Stats[TStat.Wisdom] + Player.Stats[TStat.Dexterity] - 5;
                                    Player.Practice -= 1;
                                    WriteTrainingGain(TxtTrainingFleeing, Skills.Values[TSkill.Fleeing]);
                                } else {
                                    Out.WriteLine(TxtTraining_31);
                                }
                            }
                        }

                        break;
                    case CmdPowrot:
                        if ((Player.Stats[TStat.Wisdom] > 17) && (Skills.Values[TSkill.Returning] < 90) && (Player.Practice > 0)) {
                            Skills.Values[TSkill.Returning] += 2 * Player.Stats[TStat.Wisdom] - 3;
                            Player.Practice -= 1;
                            WriteTrainingGain(TxtTrainingReturning, Skills.Values[TSkill.Returning]);
                        }
                        break;
                    case TxtTrainingParrying:
                        if ((Player.Stats[TStat.Wisdom] > 15) && (Player.Stats[TStat.Dexterity] > 11) && (Skills.Values[TSkill.Parry] < 90) && (Player.Practice > 0)) {
                            Skills.Values[TSkill.Parry] += Player.Stats[TStat.Wisdom] + Player.Stats[TStat.Dexterity] - 14;
                            Player.Practice -= 1;
                            WriteTrainingGain(TxtTrainingParrying, Skills.Values[TSkill.Parry]);
                        }
                        break;
                    case CmdPorownanie:
                        if ((Player.Stats[TStat.Wisdom] > 11) && (Skills.Values[TSkill.Comparing] < 90) && (Player.Practice > 0)) {
                            Skills.Values[TSkill.Comparing] += 3 * Player.Stats[TStat.Wisdom] - 9;
                            Player.Practice -= 1;
                            WriteTrainingGain(TxtTrainingComparing, Skills.Values[TSkill.Comparing]);
                        }
                        break;
                    case TxtTrainingCooking:
                        if ((Player.Stats[TStat.Wisdom] > 18) && (Skills.Values[TSkill.Cooking] < 90) && (Player.Practice > 0)) {
                            Skills.Values[TSkill.Cooking] += Player.Stats[TStat.Wisdom] + 1;
                            Player.Practice -= 1;
                            WriteTrainingGain(TxtTrainingCooking, Skills.Values[TSkill.Cooking]);
                        }
                        break;
                }
                break;
        }
    }


    static readonly JsonSerializerOptions SaveFormat = new() {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        IncludeFields = true,
        Converters = { new TEnumListConverterFactory() },
    };

    public string SaveFileName() =>
        $"{Player.Name}.json";

    public void SaveCharacter() {
        Command = CmdJa;
        Session.SavedFlag = true;
        Out.WriteLine(TxtSaveCharacter_01);
        Crt.ClrScr();
        SaveData save = new(Player, Skills, Gear, Bag, World, Session);
        File.WriteAllText(SaveFileName(), JsonSerializer.Serialize(save, SaveFormat));
    }

    public void Death() {
        WriteLines([
            TxtDeath_01,
            TxtDeath_02,
            TxtDeath_03,
            TxtDeath_04,
            TxtDeath_05,
            TxtDeath_06,
            TxtDeath_07,
        ]);
        Player.Energy = Player.MaxEnergy;
        Player.Experience = (Player.Experience - (250 - Roll(50) + (5 * Player.Level))).Clamp(0, MaxInt);
        MoveTo(RoomCityCentre);
        SpawnMonsters();
        Command = CmdPamietaj;
        Crt.ReadLn();
        SaveCharacter();
    }




    public List<TShopOffer> ShopOffers => _shopOffers ??= [
        new() { item = TBagItem.OldSword, buyPrice = 70, buyName = TxtBuyOldSword_01,
            sellPrice = 30, sellLines = [TxtSellOldSword_01, TxtSellOldSword_02], },
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

    public TShopOffer? ShopOffer(TBagItem item) =>
        ShopOffers.FirstOrDefault(o => o.item == item);


    public bool Buy(TBagItem item) {
        TShopOffer? offer = ShopOffer(item);
        if ((offer is null)
            || (Player.Money < (offer.buyGate >= 0 ? offer.buyGate : offer.buyPrice))) {
            return false;
        }

        Out.WriteLine(offer.buyLine != TxtEmpty
            ? offer.buyLine
            : $"{TxtShopVerb}{offer.buyName}{TxtSpaceZa}{offer.buyPrice}{TxtSpaceMonet}");
        Player.Money -= offer.buyPrice;
        AcquireItem(item);
        return true;
    }

    public bool Sell(TBagItem item) {
        TShopOffer? offer = ShopOffer(item);
        if ((offer is null) || IsEquipped(item) || !LoseItem(item)) {
            return false;
        }

        offer.sellLines.ForEach(Out.WriteLine);
        Player.Money += offer.sellPrice;
        return true;
    }


    public void RunShopRules(List<TShopRule> rules) {
        TShopRule? rule = rules.FirstOrDefault(r => Matches(r.cmd));
        if (rule is null) {
            return;
        }

        if (rule.selling) {
            Sell(rule.item);
        } else {
            Buy(rule.item);
        }
    }

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

    public void BakeryShop() {
        if (Verb == CmdLista) {
            WriteLines([
                TxtShared_24,
                TxtBakeryShop_01,
                TxtBakeryShop_02,
                TxtBakeryShop_03,
                TxtBakeryShop_04,
                TxtBakeryShop_05,
                TxtBakeryShop_06,
            ]);
        }

        StreetFights();
        RunShopRules(BakeryRules);
    }


    public List<TShopStock> ArmouryStock => _armouryStock ??= [
        new() { lines = [TxtArmouryShop_01] },
        new() { lines = [TxtArmouryShop_02] },
    ];
    List<TShopStock>? _armouryStock;

    public List<TShopStock> GeneralStoreStock => _generalStoreStock ??= [
        new() { lines = [TxtGeneralStoreShop_02] },
        new() { lines = [TxtGeneralStoreShop_03, TxtGeneralStoreShop_04, TxtGeneralStoreShop_05] },
    ];
    List<TShopStock>? _generalStoreStock;

    public void ShowStock(List<string> lines) =>
        lines.ForEach(Out.WriteLine);

    public void ArmouryShop() {
        if (Verb == CmdLista) {
            Out.WriteLine(TxtShared_24);
            for (int si2 = 0; si2 < ArmouryStock.Count; si2++) {
                ShowStock(ArmouryStock[si2].lines);
            }

            Out.WriteLine(TxtArmouryShop_03);
        }
        StreetFights();
        RunShopRules(ArmouryRules);
    }

    public void GeneralStoreShop() {
        if (Verb == CmdLista) {
            Out.WriteLine(TxtGeneralStoreShop_01);
            for (int si2 = 0; si2 < GeneralStoreStock.Count; si2++) {
                ShowStock(GeneralStoreStock[si2].lines);
            }
        }
        RunShopRules(GeneralStoreRules);
    }

    public void MagicShop() {
        if (Verb == CmdLista) {
            WriteLines([
                TxtMagicShop_01,
                TxtMagicShop_02,
                TxtMagicShop_03,
                TxtMagicShop_04,
                TxtMagicShop_05,
                TxtMagicShop_06,
                TxtMagicShop_07,
                TxtMagicShop_08,
            ]);
        }

        RunShopRules(MagicShopRules);
    }



    public List<TConcertKill> ConcertKills => _concertKills ??= [
        new() { cmd = CmdZabij + CmdDziecko, monster = TMonster.Kid, foe = TFoe.WeakishEnemy, bonus = TConcertBonus.cbNone },
        new() { cmd = CmdZabij + CmdDziadek, monster = TMonster.OldMan, foe = TFoe.WeakishEnemy, bonus = TConcertBonus.cbPipe },
        new() { cmd = CmdZabij + CmdWariat, monster = TMonster.Lunatic, foe = TFoe.MediumEnemy, bonus = TConcertBonus.cbNone },
        new() { cmd = CmdZabij + CmdSluchacz, monster = TMonster.Listener, foe = TFoe.MediumEnemy, bonus = TConcertBonus.cbNone },
        new() { cmd = CmdZabij + CmdCzlowiek, monster = TMonster.Man, foe = TFoe.MediumEnemy, bonus = TConcertBonus.cbNone },
        new() { cmd = CmdZabij + CmdFan, monster = TMonster.Fan, foe = TFoe.MediumEnemy, bonus = TConcertBonus.cbNone },
        new() { cmd = CmdZabij + CmdReporter, monster = TMonster.Reporter, foe = TFoe.MediumEnemy, bonus = TConcertBonus.cbNone },
        new() { cmd = CmdZabij + CmdPolicjant, monster = TMonster.Policeman, foe = TFoe.HardEnemy, bonus = TConcertBonus.cbNone },
        new() { cmd = CmdZabij + CmdGoryl, monster = TMonster.Gorilla, foe = TFoe.HardEnemy, bonus = TConcertBonus.cbSuit },
        new() { cmd = CmdZabij + CmdOchroniarz, monster = TMonster.Bouncer, foe = TFoe.HardEnemy, bonus = TConcertBonus.cbSuit },
    ];
    List<TConcertKill>? _concertKills;

    public void ConcertKill(string cmd, TMonster monster, TFoe foe, TConcertBonus bonus) {
        if ((Matches(cmd)) && (Session.Room == World.Monsters[monster])) {
            Fight(foe);
            if (Combat.EnemyEnergy < 1) {
                World.Monsters[monster] = 0;
                if (bonus == TConcertBonus.cbPipe) {
                    Combat.Roll = Roll(100);
                    if (Combat.Roll < 5) {
                        Out.WriteLine(TxtShared_08 + TxtConcertFights_01);
                        Out.WriteLine(TxtConcertFights_02);
                        AcquireItem(TBagItem.Pipe);
                    }
                }
            }
            if (bonus == TConcertBonus.cbSuit) {
                SuitGain();
            }
        }
    }

    public void ConcertFights() {
        for (int ci = 0; ci < ConcertKills.Count; ci++) {
            TConcertKill c = ConcertKills[ci];
            ConcertKill(c.cmd, c.monster, c.foe, c.bonus);
        }
    }

    public List<TCheckpoint> CheckpointRules => _checkpointRules ??= [
        new(400, 0, 1, (-MaxInt - 1), 19),
        new(180, 0, 1, (-MaxInt - 1), 7),
        new(360, 1, 2, (-MaxInt - 1), 7),
        new(540, 2, 3, (-MaxInt - 1), 7),
        new(300, 0, 1, 8, 12),
        new(500, 1, 2, 8, 12),
    ];
    List<TCheckpoint>? _checkpointRules;

    public void ReachCheckpoint(int exp, int stage, int next, int loStr, int hiStr) {
        if ((Player.Experience >= exp) && (Session.CheckpointStage == stage)
            && (Player.Stats[TStat.Strength] >= loStr) && (Player.Stats[TStat.Strength] <= hiStr)) {
            Out.WriteLine(TxtCheckpoints_01);
            Session.CheckpointStage = next;
            SpawnMonsters();
        }
    }

    public void Checkpoints() {
        for (int ci = 0; ci < CheckpointRules.Count; ci++) {
            TCheckpoint c = CheckpointRules[ci];
            ReachCheckpoint(c.exp, c.stage, c.next, c.loStr, c.hiStr);
        }
    }


    public List<TBand> LevelCosts => _levelCosts ??= [
        new(1, 1, 725, 0),
        new(2, 2, 730, 0),
        new(3, 3, 735, 0),
        new(4, 8, Lvl4_Base, 1),
        new(9, MaxInt, Lvl9_Base, 2),
    ];
    List<TBand>? _levelCosts;

    public List<TBand> PracticeGains => _practiceGains ??= [
        new(-128, 10, 3, 0),
        new(11, 15, 4, 0),
        new(16, 22, 5, 0),
        new((-MaxInt - 1), MaxInt, 6, 0),
    ];
    List<TBand>? _practiceGains;

    public List<TEnergyBonus> MaxEnergyGains => _maxEnergyGains ??= [
        new() { dexLo = 12, strLo = (-MaxInt - 1), bonus = 1 },
        new() { dexLo = 16, strLo = (-MaxInt - 1), bonus = 1 },
        new() { dexLo = 21, strLo = (-MaxInt - 1), bonus = 1 },
        new() { dexLo = 26, strLo = 20, bonus = 1 },
    ];
    List<TEnergyBonus>? _maxEnergyGains;

    public void CheckLevelUp() {
        int threshold;
        if (Player.Level < 1) {
            return;
        }

        threshold = LevelThreshold(Player.Level);
        if (Player.Experience < threshold) {
            return;
        }

        WriteRainbow(TxtCheckLevelUp_01);
        Out.WriteLine();

        Player.Experience -= BandAmount(LevelCosts, Player.Level);
        Player.Practice += BandAmount(PracticeGains, Player.Stats[TStat.Wisdom]);
        Out.WriteLine(TxtCheckLevelUp_02 + Player.Practice + TxtCheckLevelUp_03);
        Player.MaxEnergy += 10;
        for (int ei = 0; ei < MaxEnergyGains.Count; ei++) {
            TEnergyBonus e = MaxEnergyGains[ei];
            if ((Player.Stats[TStat.Dexterity] > e.dexLo) && (Player.Stats[TStat.Strength] > e.strLo)) {
                Player.MaxEnergy += e.bonus;
            }
        }
        Out.WriteLine(TxtCheckLevelUp_02 + Player.MaxEnergy + TxtCheckLevelUp_04);
        Player.MaxMana += Player.Stats[TStat.Wisdom] + 2;
        Out.WriteLine(TxtCheckLevelUp_05 + (Player.Stats[TStat.Wisdom] + 2) + TxtCheckLevelUp_06);
        Player.Level += 1;
        Session.CheckpointStage = 0;
        if (Player.Level == 12) {
            Out.WriteLine(TxtCheckLevelUp_07);
            if (Player.Stats[TStat.Strength] >= Player.Stats[TStat.MaxStrength]) {
                Player.Stats[TStat.MaxStrength] += 3;
            }

            if (Player.Stats[TStat.Dexterity] >= Player.Stats[TStat.MaxDexterity]) {
                Player.Stats[TStat.MaxDexterity] += 3;
            }

            if (Player.Stats[TStat.Wisdom] >= Player.Stats[TStat.MaxWisdom]) {
                Player.Stats[TStat.MaxWisdom] += 3;
            }

            if (Player.Stats[TStat.Strength] < Player.Stats[TStat.MaxStrength]) {
                Player.Stats[TStat.MaxStrength] += 2;
            }

            if (Player.Stats[TStat.Dexterity] < Player.Stats[TStat.MaxDexterity]) {
                Player.Stats[TStat.MaxDexterity] += 2;
            }

            if (Player.Stats[TStat.Wisdom] < Player.Stats[TStat.MaxWisdom]) {
                Player.Stats[TStat.MaxWisdom] += 2;
            }

            if ((Player.Stats[TStat.MaxWisdom] > Player.Stats[TStat.MaxStrength]) && (Player.Stats[TStat.MaxWisdom] > Player.Stats[TStat.MaxDexterity])) {
                Player.Stats[TStat.MaxWisdom] += 3;
            }

            if ((Player.Stats[TStat.MaxStrength] > Player.Stats[TStat.MaxWisdom]) && (Player.Stats[TStat.MaxStrength] > Player.Stats[TStat.MaxDexterity])) {
                Player.Stats[TStat.MaxStrength] += 3;
            }

            if ((Player.Stats[TStat.MaxDexterity] > Player.Stats[TStat.MaxStrength]) && (Player.Stats[TStat.MaxDexterity] > Player.Stats[TStat.MaxWisdom])) {
                Player.Stats[TStat.MaxDexterity] += 3;
            }

            if ((Player.Stats[TStat.MaxDexterity] == Player.Stats[TStat.MaxStrength]) && (Player.Stats[TStat.MaxStrength] == Player.Stats[TStat.MaxWisdom])) {
                Player.Stats[TStat.MaxDexterity] += 1;
                Player.Stats[TStat.MaxWisdom] += 1;
                Player.Stats[TStat.MaxStrength] += 1;
            }
            if ((Player.Stats[TStat.MaxDexterity] > Player.Stats[TStat.MaxWisdom]) && (Player.Stats[TStat.MaxDexterity] == Player.Stats[TStat.MaxStrength])) {
                Player.Stats[TStat.MaxStrength] += 2;
                Player.Stats[TStat.MaxDexterity] += 1;
            }
            Out.WriteLine(TxtCheckLevelUp_08 + Player.Stats[TStat.MaxStrength]
                + TxtMaxymalnie + TxtCheckLevelUp_09);
            Out.WriteLine(TxtCheckLevelUp_08 + Player.Stats[TStat.MaxDexterity]
                + TxtMaxymalnie + TxtCheckLevelUp_10);
            Out.WriteLine(TxtCheckLevelUp_08 + Player.Stats[TStat.MaxWisdom]
                + TxtMaxymalnie + TxtCheckLevelUp_11);
        }
    }

    public void CmdScan() =>
        Out.WriteLine(TxtCmdScan_01);
    public void CmdClearScreen() =>
        Crt.ClrScr();

    public void CmdGiveEnergy() {
        Player.MaxEnergy += 200;
        Player.ChangeEnergy(200);
    }

    public void CmdGiveKunszt() =>
        Player.Experience += 500;
    public void CmdTakeMoney() =>
        Player.Money -= 100;

    public void CmdResetArmor() {
        Gear.Values[TGearItem.WornCount] = 0;
        Gear.Values[TGearItem.Armor] = 0;
        Gear.Values[TGearItem.LuckRoll] = 0;
    }

    public void CmdStrip() {
        Gear.Values[TGearItem.Wearing] = 0;
        Gear.Values[TGearItem.WeaponKind] = 0;
        Gear.Values[TGearItem.ShieldKind] = 0;
        Gear.WornGarment = TxtEmpty;
        Inventory.SetInventoryItemCount(TBagItem.OldSword, 1);
        Inventory.SetInventoryItemCount(TBagItem.SmallShield, 1);
        Player.Stats[TStat.Strength]--;
    }

    public void CmdJustSaved() {
        if (Session.SavedFlag) {
            Command = CmdZdolnosci;
            Session.SavedFlag = false;
        }
    }

    public void CmdDrinkBeer() {
        if (LoseItem(TBagItem.Beer)) {
            Out.WriteLine(TxtCmdDrinkBeer_01);
            Player.ChangeEnergy(10);
            Player.ChangeMana(10);
        }
    }

    public void CmdWieldOldSword() {
        if (Equip(TBagItem.OldSword)) {
            Out.WriteLine(TxtCmdWieldOldSword_01);
        }
    }

    public void CmdUnwieldOldSword() {
        if (Unequip(TBagItem.OldSword)) {
            Out.WriteLine(TxtCmdUnwieldOldSword_01);
        }
    }

    public void CmdWieldSmallShield() {
        if (Equip(TBagItem.SmallShield)) {
            Out.WriteLine(TxtCmdWieldSmallShield_01);
        }
    }

    public void CmdUnwieldSmallShield() {
        if (Unequip(TBagItem.SmallShield)) {
            Out.WriteLine(TxtCmdUnwieldSmallShield_01);
        }
    }

    public void CmdSetFleeEnergy() {
        if (Skills.Values[TSkill.Fleeing] > 0) {
            Out.WriteLine(TxtCmdSetFleeEnergy_01);
            Skills.Values[TSkill.FleeEnergy] = Crt.ReadInt();
        }
    }

    public void CmdSleep() {
        Session.SleepHours = 0;
        do {
            Session.SleepHours += 1;
            Crt.Delay(5000);
            Out.WriteLine(TxtCmdSleep_01 + Session.SleepHours + TxtCmdSleep_02);
            Player.Experience -= 20;
            Player.ChangeEnergy(10);
        } while (!Crt.KeyPressed());
        Out.WriteLine(TxtCmdSleep_03 + (Session.SleepHours * 10)
            + TxtCmdSleep_04 + ((20 * Session.SleepHours) - Roll(2 * Session.SleepHours)) + TxtShared_22);
        Player.Experience += Roll(2 * Session.SleepHours);
        if (Session.SleepHours > 4) {
            Out.WriteLine(TxtCmdSleep_05 + Player.Stats[TStat.Wisdom] + TxtCmdSleep_06);
            Player.ChangeEnergy(Player.Stats[TStat.Wisdom]);
        }
        if (Session.SleepHours > 8) {
            Out.WriteLine(TxtCmdSleep_07 + (Player.Stats[TStat.Wisdom] * 2) + TxtShared_19);
            Player.ChangeEnergy(Player.Stats[TStat.Wisdom] * 2);
        }
    }

    public void CmdCompare() {
        if (Skills.Values[TSkill.Comparing] > 0) {
            Combat.Roll = Roll(100);
            bool success = Skills.Values[TSkill.Comparing] > Combat.Roll;
            if (!Player.ChangeMana(success ? -10 : -5)) {
                Out.WriteLine(TxtNoMana);
                return;
            }
            if (success) {
                CompareFoe();
            } else {
                Out.WriteLine(TxtCmdCompare_01);
            }
        }
    }

    public void CmdGiveMoney() =>
        Player.Money += 5000;

    public List<string> RefusedFood => _refusedFood ??= [
        CmdOdrzuc + CmdPaczek, CmdOdrzuc + CmdCiastko,
        CmdOdrzuc + CmdSuchaRacja, CmdOdrzuc + CmdBulka,
        CmdOdrzuc + CmdChleb, CmdOdrzuc + CmdWeka,
        CmdOdrzuc + CmdSerce, CmdOdrzuc + CmdBigos, ];
    List<string>? _refusedFood;

    public void CmdRefuseFoodDrop() {
        for (int i = 0; i < RefusedFood.Count; i++) {
            if (Matches(RefusedFood[i])) {
                Out.WriteLine(TxtCmdRefuseFoodDrop_01);
                break;
            }
        }
    }

    public void CmdOpenPortal() {
        bool portal = false;
        switch (Verb) {
            case CmdPowrot:
                portal = Skills.Values[TSkill.Returning] > 0;
                break;
            case CmdUzyj:
                portal = Rest == CmdScrollPowrot && Bag.Items[TBagItem.ReturnScroll] > 0;
                break;
        }
        if (portal) {
            Combat.Roll = Roll(100);
            if (Rest == CmdScrollPowrot) {
                Combat.Roll = Roll(1);
            }
            bool success = Combat.Roll <= Skills.Values[TSkill.Returning];
            bool partial = !success && (Skills.Values[TSkill.Returning] + 30 > Combat.Roll);
            int cost = 10;
            if (success) {
                cost = 15;
            } else if (partial) {
                cost = 5;
            }
            if (!Player.ChangeMana(-cost)) {
                Out.WriteLine(TxtNoMana);
                return;
            }
            if (Rest == CmdScrollPowrot) {
                LoseItem(TBagItem.ReturnScroll);
            }
            if (success) {
                Out.WriteLine(TxtCmdOpenPortal_01);
                MoveTo(RoomCityCentre);
            } else {
                Out.WriteLine(partial ? TxtCmdOpenPortal_02 : TxtCmdOpenPortal_03);
            }
        }
    }

    public void CmdUseCompareScroll() {
        if (Bag.Items[TBagItem.CompareScroll] > 0) {
            if (!Player.ChangeMana(-10)) {
                Out.WriteLine(TxtNoMana);
                return;
            }
            CompareFoe();
            LoseItem(TBagItem.CompareScroll);
        }
    }

    public void CmdSetKickThresholds() {
        if (Skills.Values[TSkill.Kick] > 0) {
            Out.WriteLine(TxtCmdSetKickThresholds_01);
            Out.WriteLine(TxtCmdSetKickThresholds_02);
            Skills.Values[TSkill.KickMaxEnergy] = Crt.ReadInt();
            Out.WriteLine(TxtCmdSetKickThresholds_03);
            Skills.Values[TSkill.KickMinMana] = Crt.ReadInt();
        }
    }

    public void CmdSpeede() =>
        AcquirePermit();


    public List<TSkillLine> SkillLines => _skillLines ??= [
        new() { wisdom = 10, strength = 11, dexterity = -1, text1 = TxtCmdShowSkills_01, skill = TSkill.Kick, text2 = TxtShared_26 },
        new() { wisdom = 10, strength = -1, dexterity = 10, text1 = TxtCmdShowSkills_02, skill = TSkill.Fleeing, text2 = TxtShared_26 },
        new() { wisdom = 15, strength = -1, dexterity = 11, text1 = TxtCmdShowSkills_03, skill = TSkill.Parry, text2 = TxtShared_26 },
        new() { wisdom = 11, strength = -1, dexterity = -1, text1 = TxtCmdShowSkills_04, skill = TSkill.Comparing, text2 = TxtShared_26 },
        new() { wisdom = 18, strength = -1, dexterity = -1, text1 = TxtCmdShowSkills_05, skill = TSkill.Cooking, text2 = TxtShared_26 },
        new() { wisdom = 19, strength = -1, dexterity = -1, text1 = TxtCmdShowSkills_06, skill = TSkill.Returning, text2 = TxtShared_26 },
    ];
    List<TSkillLine>? _skillLines;

    public void CmdShowSkills() {
        for (int i = 0; i < SkillLines.Count; i++) {
            if (GatesMet(SkillLines[i].wisdom, SkillLines[i].strength, SkillLines[i].dexterity)) {
                TSkillLine s = SkillLines[i];
                Out.WriteLine(s.text1 + Skills.Values[s.skill] + s.text2);
            }
        }
    }

    public List<TCommandEntry> GroundCommands => _groundCommands ??= [
        new(CmdPamietaj, SaveCharacter),
        new(CmdScan2, CmdScan),
        new(CmdCzysc + CmdEkran, CmdClearScreen),
        new(CmdDawaj + CmdEn, CmdGiveEnergy),
        new(CmdDawaj + CmdKunszt, CmdGiveKunszt),
        new(CmdZabieraj + CmdKase, CmdTakeMoney),
        new(CmdUstaw + CmdSz, CmdResetArmor),
        new(CmdZdejmuj, CmdStrip),
        new(CmdJa, ShowCharacter),
        new(TxtEmpty, TakeDrop),
        new(TxtEmpty, CmdJustSaved),
        new(CmdUzyj + CmdPiwo, CmdDrinkBeer),
        new(CmdUzyj + CmdStary, CmdWieldOldSword),
        new(CmdOdloz + CmdStary, CmdUnwieldOldSword),
        new(CmdUzyj + CmdMala, CmdWieldSmallShield),
        new(CmdOdloz + CmdMala, CmdUnwieldSmallShield),
        new(TxtEmpty, UseItems),
        new(CmdZwiej, CmdSetFleeEnergy),
        new(CmdSpij, CmdSleep),
        new(CmdPorownaj, CmdCompare),
        new(CmdDawaj + CmdKase, CmdGiveMoney),
        new(TxtEmpty, CmdRefuseFoodDrop),
        new(TxtEmpty, CmdOpenPortal),
        new(CmdUzyj + CmdScrollPorownywanie, CmdUseCompareScroll),
        new(CmdKop, CmdSetKickThresholds),
        new(CmdJestem + CmdSpeede2, CmdSpeede),
        new(CmdZdolnosci, CmdShowSkills),
        new(TxtEmpty, DisplayPrefs),
        new(CmdWlacz + CmdPostac, LoadCharacter),
    ];
    List<TCommandEntry>? _groundCommands;

    public void Start() {
        Randomize();
        ChooseRace();
        do {
            Checkpoints();
            CheckLevelUp();

            RecalcBag();
            if (Bag.Count > Bag.Limit) {
                Out.WriteLine(TxtStart_01 + (Bag.Count - Bag.Limit) + TxtStart_02);
                Out.WriteLine(TxtStart_03 + ((Bag.Count - Bag.Limit) * 10) + TxtFightTailEnergii);
                if (!Player.ChangeEnergy(-((Bag.Count - Bag.Limit) * 10))) {
                    Session.Room = DeadRoom;
                }
            }

            if (Combat.PoisonRounds > 0) {
                Combat.Roll = Roll(5);
                Out.WriteLine(TxtStart_05 + Combat.Roll + TxtStart_06);
                Combat.PoisonRounds -= 1;
                if (!Player.ChangeEnergy(-Combat.Roll)) {
                    Session.Room = DeadRoom;
                }
            }

            Session.ExitRequested = false;
            Rooms.DispatchRoom();
            if (Session.ExitRequested) {
                return;
            }

            if (Session.Room == DeadRoom) {
                Death();
            }

            Combat.Fled = false;
        } while (true);
    }

    public void LoadCharacter() {
        Command = Crt.ReadLn();
        LoadGame();
    }

    public void LoadGame() {
        SaveData? save = JsonSerializer.Deserialize<SaveData>(File.ReadAllText(SaveFileName()), SaveFormat);
        if (save is null) {
            return;
        }
        Player = save.Player;
        Skills = save.Skills;
        Gear = save.Gear;
        Bag = save.Bag;
        World = save.World;
        Session = save.Session;
        RecalcBag();
        Out.WriteLine(TxtLoadCharacter_01 + Gear.WornGarment);
    }

}
