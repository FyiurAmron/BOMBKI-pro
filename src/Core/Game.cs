
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
    public Combat Combat { get; }
    public TWorld World = new();
    public TSession Session = new();
    public Character Character { get; }
    public Inventory Inventory { get; }
    public Rooms Rooms { get; }

    public Game() {
        Combat = new Combat(this);
        Character = new Character(this);
        Inventory = new Inventory(this);
        Rooms = new Rooms(this);
    }

    public void TryLoot(TBagItem item, int threshold, string foundMsg) =>
        Combat.TryLoot(item, threshold, foundMsg);

    public void TryCageLoot(int threshold) =>
        Combat.TryCageLoot(threshold);

    public int FightExpForEnergy(int diff) =>
        Combat.FightExpForEnergy(diff);

    public int FightExpForStrength(int diff) =>
        Combat.FightExpForStrength(diff);

    public int FightExpForDexterity(int diff) =>
        Combat.FightExpForDexterity(diff);

    public TDodgeBound DodgeBoundRow(int chance) =>
        Combat.DodgeBoundRow(chance);

    public int DodgeBound(int chance) =>
        Combat.DodgeBound(chance);

    public int EnemyDodgeBound(int chance) =>
        Combat.EnemyDodgeBound(chance);

    public int DexterityBonus(int value) =>
        Combat.DexterityBonus(value);

    public int ParryBonus(int value) =>
        Combat.ParryBonus(value);

    public int KickBonus(int value) =>
        Combat.KickBonus(value);

    public void FightDog() =>
        Combat.FightDog();

    public void CompositeFight(int EnemyEnergyBase, int EnemyEnergyRand, int EnemyDexterityBase, int EnemyDexterityRand, int EnemyStrengthBase, int EnemyStrengthRand,
        int LootBase, int LootRand, int HeartChance, bool RequireEnergy, bool AntDoughnut) =>
        Combat.CompositeFight(EnemyEnergyBase, EnemyEnergyRand, EnemyDexterityBase, EnemyDexterityRand, EnemyStrengthBase, EnemyStrengthRand,
            LootBase, LootRand, HeartChance, RequireEnergy, AntDoughnut);

    public void Fight(TFoe foe) =>
        Combat.Fight(foe);

    public void KillBeast(string cmd, IntSlot beast, TFoe foe) =>
        Combat.KillBeast(cmd, beast, foe);

    public void KillBeastUnlessFled(string cmd, IntSlot beast, TFoe foe) =>
        Combat.KillBeastUnlessFled(cmd, beast, foe);

    public void CompareFoeRules(List<TCompareFoeRule> rules) =>
        Combat.CompareFoeRules(rules);

    public void CompareFoe() =>
        Combat.CompareFoe();

    public void ShieldBlock() =>
        Combat.ShieldBlock();

    public void DoFight() =>
        Combat.DoFight();

    public void ArenaKill(string cmd, TMonster monster, bool pills) =>
        Combat.ArenaKill(cmd, monster, pills);

    public void StageFights() =>
        Combat.StageFights();

    public void KillPlant(string cmd, IntSlot Plant) =>
        Combat.KillPlant(cmd, Plant);

    public void KillPlantIfAlive(string cmd, IntSlot Plant) =>
        Combat.KillPlantIfAlive(cmd, Plant);

    public void KillPlantEasyIfAlive(string cmd, IntSlot Plant) =>
        Combat.KillPlantEasyIfAlive(cmd, Plant);

    public void PlantFights() =>
        Combat.PlantFights();

    public void KillStreetFoe(string cmd, IntSlot target, bool extra, TStreetKind kind) =>
        Combat.KillStreetFoe(cmd, target, extra, kind);

    public void KillSweeper() =>
        Combat.KillSweeper();

    public void KillSickMan() =>
        Combat.KillSickMan();

    public void StreetFights() =>
        Combat.StreetFights();

    public void ConcertKill(string cmd, TMonster monster, TFoe foe, TConcertBonus bonus) =>
        Combat.ConcertKill(cmd, monster, foe, bonus);

    public void ConcertFights() =>
        Combat.ConcertFights();

    public void TrainAttribute(IntSlot Attribute, IntSlot MaxAttribute, int MinPractice, int PracticeCost, string cmdForm, string nounForm) =>
        Character.TrainAttribute(Attribute, MaxAttribute, MinPractice, PracticeCost, cmdForm, nounForm);

    public void HandleTrain() =>
        Character.HandleTrain();

    public int LevelThreshold(int lvl) =>
        Character.LevelThreshold(lvl);

    public void ShowSheetItem(TSheetKind kind, int val, string desc, int width) =>
        Character.ShowSheetItem(kind, val, desc, width);

    public void ShowCharacter() =>
        Character.ShowCharacter();

    public int FindRace(string name) =>
        Character.FindRace(name);

    public bool IsNameChar(char c) =>
        Character.IsNameChar(c);

    public void ChooseRace() =>
        Character.ChooseRace();

    public bool GatesMet(int reqWisdom, int reqStrength, int reqDexterity) =>
        Character.GatesMet(reqWisdom, reqStrength, reqDexterity);

    public void WriteTrainingGain(string skillName, int value) =>
        Character.WriteTrainingGain(skillName, value);

    public void Training() =>
        Character.Training();

    public void CmdSetKickThresholds() =>
        Character.CmdSetKickThresholds();

    public void CmdShowSkills() =>
        Character.CmdShowSkills();

    public void CheckLevelUp() =>
        Character.CheckLevelUp();

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






















    internal IntSlot PlayerEnergy() =>
        new(() => Player.Energy, v => Player.Energy = v);
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




    public List<TDexterityLimit> DexterityItemLimits => _dexterityItemLimits ??= [
        new() { gate = 0, limit = 10 }, new() { gate = 11, limit = 11 },
        new() { gate = 13, limit = 12 }, new() { gate = 16, limit = 13 },
        new() { gate = 19, limit = 14 }, new() { gate = 21, limit = 15 },
        new() { gate = 25, limit = 16 },
    ];
    List<TDexterityLimit>? _dexterityItemLimits;
























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


    public void CmdSpeede() =>
        AcquirePermit();




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
