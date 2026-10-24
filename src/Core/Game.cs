
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using static Texts;
using static World;
using static Rooms;

public class Game {

    public TPlayer Player = new();
    public TSkills Skills = new();
    public TGear Gear = new();
    public TBag Bag = new();
    public Combat Combat { get; }
    public World World { get; }
    public TSession Session = new();
    public Character Character { get; }
    public Inventory Inventory { get; }

    public Game() {
        Combat = new Combat(this);
        Character = new Character(this);
        Inventory = new Inventory(this);
        World = new World(this);
    }

    public void MoveTo(int room) =>
        World.MoveTo(room);

    public void MoveWhen(List<TMovement> moves) =>
        World.MoveWhen(moves);

    public bool MoveWhenOrQuit(List<TMovement> moves) =>
        World.MoveWhenOrQuit(moves);

    public void SpawnAt(List<TSpawnedAt> placements) =>
        World.SpawnAt(placements);

    public void SpawnMonsters() =>
        World.SpawnMonsters();

    public void DescribePlants() =>
        World.DescribePlants();

    public void DescribeStreet() =>
        World.DescribeStreet();

    public void DescribeStage() =>
        World.DescribeStage();

    public void DescribeCrowd() =>
        World.DescribeCrowd();

    public void DescribeBeasts() =>
        World.DescribeBeasts();

    public List<TMazeMove> MazeMoves =>
        World.MazeMoves;

    public string MazeDirectionCommand(TMazeDir dir) =>
        World.MazeDirectionCommand(dir);

    public void MazeMove(TMazeDir dir) =>
        World.MazeMove(dir);

    public void StartQuest(string cmd, string line, int quest, int kills) =>
        World.StartQuest(cmd, line, quest, kills);

    public void Checkpoints() =>
        World.Checkpoints();

    public bool Buy(TBagItem item) =>
        World.Buy(item);

    public bool Sell(TBagItem item) =>
        World.Sell(item);

    public void BakeryShop() =>
        World.BakeryShop();

    public void ArmouryShop() =>
        World.ArmouryShop();

    public void GeneralStoreShop() =>
        World.GeneralStoreShop();

    public void MagicShop() =>
        World.MagicShop();

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

    public static void Main() =>
        new Game().Start();

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
            World.DispatchRoom();
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
        World.Load(save.World);
        Session = save.Session;
        RecalcBag();
        Out.WriteLine(TxtLoadCharacter_01 + Gear.WornGarment);
    }

}
