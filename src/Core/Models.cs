using System.Text.Json.Serialization;

public enum TFoe {
    WeakEnemy, WeakishEnemy, MediumEnemy, HardEnemy, VeryEasyEnemy,
    EasyEnemy, NotEasyEnemy, VeryHardEnemy,
}

public enum TStat {
    Strength, Dexterity, Wisdom, MaxStrength, MaxDexterity, MaxWisdom,
}

public enum TSkill {
    Parry, Kick, KickMinMana, KickMaxEnergy, Fleeing, FleeEnergy,
    Comparing, Cooking, Returning,
}

public enum TGearItem {
    Armor, WornCount, ArmorRating, LuckRoll, Wearing,
    WeaponKind, ShieldKind,
}

public enum TBagItem {
    Backpack, OldSword, SmallShield, Heart, Diploma, Pipe,
    Doughnut, Cookie, DryRation, Bread, BreadRoll, Sausage, Bigos,
    ManaBottle, Tracksuit, SpikedSuit, Cassette, CompareScroll,
    TeleportPill, LuckyLeaf, ReturnScroll, Beer,
}

public enum TMonster {
    CageStrong, CageWeakling, CageAgile, CageTough, CageHuge,
    Woodworm, Fly, Bacterium, Snail, Beetle, Cockroach, Ant,
    Spider, Boar, Rat, Fox, Partridge, Hare, Mongrel, Eagle,
    Deer, Elephant, Lion, Giraffe, Camel, Ostrich, Boa, Wolf,
    Bison, Panther, Gladiator, Warrior, Tamer, Dachshund, Shepherd,
    Spaniel, Poodle, Pooch, Cabman, Vendor, Sweeper, Drunkard,
    SickMan, Kid, Lunatic, Listener, Fan, Man, Policeman, Bouncer,
    OldMan, Gorilla, Reporter, Guitarist, Drummer, Keyboardist,
    Liroy, Minibarman, Fatso, DJ, Pedal, Couple, Maciek, Sorrel,
    Daisy, Clover, SowThistle, Dandelion, Rose, Blackberry, Thistle,
    Gooseberry, Raspberry, Grass, Duncan,
}

public enum TSheetKind { ikShown, ikPlain }

public enum TMazeDir { mdPolodnie, mdPolnoc, mdWschod, mdZachod }

public enum TStreetKind { skDog, skFolk }

public class TPlayer {
    public TEnumList<TStat, int> Stats = new();
    public int Energy, MaxEnergy;
    public int Mana, MaxMana;
    public int Practice;
    public int Level;
    public int Experience;
    public int Money;
    public string Name = "";

    public bool ChangeEnergy(int delta) {
        Energy = (Energy + delta).Clamp(0, MaxEnergy);
        return Energy > 0;
    }

    public bool ChangeMana(int delta) {
        bool enough = (delta >= 0) || (Mana >= -delta);
        Mana = (Mana + delta).Clamp(0, MaxMana);
        return enough;
    }
}

public class TSkills {
    public TEnumList<TSkill, int> Values = new();
}

public class TGear {
    public TEnumList<TGearItem, int> Values = new();
    public string WornGarment = "";
}

public class TBag {
    public int Count;
    public int Limit;
    public int Permit;
    public TEnumList<TBagItem, int> Items = new();
    public TEnumList<TBagItem, List<int>> ItemRooms =
        new(Enum.GetValues<TBagItem>().Select(_ => new List<int>()));

    public void AddGroundItem(TBagItem item, int room) =>
        ItemRooms[item].Add(room);

    public bool TakeGroundItem(TBagItem item, int room) {
        int index = ItemRooms[item].IndexOf(room);
        if (index < 0) {
            return false;
        }

        ItemRooms[item].RemoveAt(index);
        return true;
    }

    public bool HasGroundItem(TBagItem item, int room) =>
        ItemRooms[item].Contains(room);

    public bool HasGroundItem(TBagItem item) =>
        ItemRooms[item].Any();
}

public class TCombat {
    public int EnemyEnergy, EnemyDexterity, EnemyStrength;
    public bool EnemyDodged, PlayerDodged;
    public int DodgeChance, Damage;
    public bool Fled;
    public int FightExperience;
    public int FireballCharges, PoisonCharges, PoisonRounds;
    public int Loot, Roll;
}

public class TWorld {
    public TEnumList<TMonster, int> Monsters = new();
    public bool Door, OldElf, LiroyKilled;
    public int Quest, QuestKills;
    public int DuncanQuest;
}

public class TSession {
    public int Room, OuterRoom;
    public int CheckpointStage;
    public bool SavedFlag;
    public int SleepHours, Overall;
    [JsonIgnore]
    public bool ExitRequested;
}

public record TQuestOffer(string cmd, string line, int quest, int kills);

public class TMovement {
    public string cmd = "";
    public int dest;
    public Func<bool>? predicate = null;
    public string? blocked = null;
}

public record TBand(int lo, int hi, int Base, int perUnit);

public class TEnumList<TKey, TValue> where TKey : struct, Enum {
    readonly List<TValue> values;

    public TEnumList() {
        values = [.. Enumerable.Repeat(default(TValue)!, Enum.GetValues<TKey>().Length)];
    }

    public TEnumList(IEnumerable<TValue> initial) {
        values = [.. initial];
    }

    public TValue this[TKey key] {
        get => values[Convert.ToInt32(key)];
        set => values[Convert.ToInt32(key)] = value;
    }
}

public record IntSlot(Func<int> Get, Action<int> Set) {
    public static IntSlot Into<TKey>(TEnumList<TKey, int> list, TKey key) where TKey : struct, Enum =>
        new(() => list[key], v => list[key] = v);
}

public record TGearEffect(TGearItem? Slot = null, string? Garment = null, int Strength = 0, int Luck = 0, int Armor = 0, int Worn = 0);

public record TGroundItem(TBagItem item, string line);

public record TSpawnedAt(TMonster monster, int room);

public record TCheckpoint(int exp, int stage, int next, int loStr, int hiStr);

public class TTrainable {
    public string cmd = "";
    public TStat stat, maxStat;
    public int minPractice, cost;
    public string cmdForm = "", nounForm = "";
}

public record TCommandEntry(string cmd, Action run);

public record TDodgeBound(int limit, int player, int enemy);

public record TSpot(TMonster where, string text);

public class TFoeStats {
    public int energyBase, energyRand, dexterityBase, dexterityRand;
    public int strengthBase, strengthRand, lootBase, lootRand, heartChance;
    public bool requireEnergy, antDoughnut;
}

public class TCageStats {
    public int energy, dexterity, strength, lootRand, lootThreshold;
}

public record TCompareFoeRule(IntSlot stat, int lo, int hi, string line);

public class TFightExpStat {
    public IntSlot playerStat = null!, enemyStat = null!;
    public Func<int, int> expFor = _ =>
        0;
}

public class TTakeDrop {
    public string cmd = "", verb = "", line = "";
    public TBagItem item;
}

public class TFoodItem {
    public string cmd = "", verb = "", line = "";
    public TBagItem item;
    public int gain;
    public IntSlot stat = null!, cap = null!;
}

public class TBeastKill {
    public string cmd = "";
    public TMonster beast;
    public TFoe foe;
    public bool unlessFled;
}

public class TArenaKill {
    public string cmd = "";
    public TMonster monster;
    public bool pills;
}

public class TMazeMove {
    public int room;
    public List<TMazeDir> exits = [];
}

public class TPlantKill {
    public string cmd = "";
    public TMonster plant;
}

public class TRace {
    public string name = "";
    public TEnumList<TStat, int> baseStats = new();
    public int MaxManaBonus, MoneyBonus, MaxEnergyBonus, EnergyBonus;
}

public class TDexterityLimit {
    public int gate, limit;
}

public class TSheetItem {
    public IntSlot item = null!;
    public string desc = "";
    public int width;
    public TSheetKind kind;
}

public class TStreetKill {
    public string cmd = "";
    public TMonster target;
    public bool extra;
    public TStreetKind kind;
}

public class TTrainingLine {
    public int wisdom, strength, dexterity;
    public string text1 = "";
}

public record SaveData(
    TPlayer Player,
    TSkills Skills,
    TGear Gear,
    TBag Bag,
    TWorld World,
    TSession Session);

public class TShopOffer {
    public int buyPrice, buyGate = -1, sellPrice;
    public string buyName = "", buyLine = "";
    public List<string> sellLines = [];
}

public class TItemCarryModifiers {
    public int armor, dexterity, maxEnergy, maxMana, wisdom;
}

public record TShopRule(TBagItem item, bool selling);

public enum TConcertBonus { cbNone, cbPipe, cbSuit }

public class TConcertKill {
    public string cmd = "";
    public TMonster monster;
    public TFoe foe;
    public TConcertBonus bonus;
}

public class TEnergyBonus {
    public int dexLo, strLo, bonus;
}

public class TSkillLine {
    public int wisdom, strength, dexterity;
    public string text1 = "";
    public TSkill skill;
    public string text2 = "";
}
