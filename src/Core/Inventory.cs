using static Texts;
using static Rooms;

public class Inventory {
    readonly Game game;

    public Inventory(Game game) {
        this.game = game;
    }

    public void GainLoot1000(int threshold, string line1, string line2, TBagItem item) {
        game.Combat.Roll = game.Roll(1000);
        if (game.Combat.Roll <= threshold) {
            Out.WriteLine(line1);
            if (line2 != "") {
                Out.WriteLine(TxtUniqeItem + line2);
            }

            AcquireItem(item);
        }
    }

    public void GainLoot100(int threshold, string line1, string line2, TBagItem item) {
        game.Combat.Roll = game.Roll(100);
        if ((game.Combat.Roll < threshold) && (game.Session.Room != RoomDead)) {
            Out.WriteLine(TxtShared_08 + line1);
            if (line2 != "") {
                Out.WriteLine(TxtBang + TxtUniqeItem + line2);
            }

            AcquireItem(item);
        }
    }

    public void SuitGain() =>
        GainLoot1000(25, TxtSuitGain_01, TxtSuitGain_02, TBagItem.SpikedSuit);

    public void PillGain() =>
        GainLoot1000(42, TxtPillGain_01, "", TBagItem.TeleportPill);

    public void CassetteGain() =>
        GainLoot100(2, TxtCassetteGain_01, TxtCassetteGain_02, TBagItem.Cassette);

    public void LeafGain() =>
        GainLoot100(6, TxtLeafGain_01, TxtLeafGain_02, TBagItem.LuckyLeaf);

    public void ScrollGain() =>
        GainLoot100(10, TxtScrollGain_01, "", TBagItem.CompareScroll);

    public void TakeItem(TBagItem item, string verb, string line) {
        if (game.Bag.TakeGroundItem(item, game.Session.OuterRoom)) {
            Out.WriteLine(verb + line);
            AcquireItem(item);
        }
    }

    public bool IsEquipped(TBagItem item) =>
        GearEffects.TryGetValue(item, out TGearEffect? effect) && (effect.Slot is TGearItem slot
            ? game.Gear.Values[slot] == 1
            : game.Gear.WornGarment == effect.Garment);

    public void AcquireItem(TBagItem item, int quantity = 1) {
        if (quantity < 1) {
            throw new ArgumentOutOfRangeException(nameof(quantity));
        }

        bool newlyCarried = game.Bag.Items[item] == 0;
        AddInventoryItem(item, quantity);
        if (newlyCarried) {
            ApplyItemCarryModifiers(item, 1);
        }
    }

    public bool LoseItem(TBagItem item, int quantity = 1) {
        if ((quantity < 1) || (game.Bag.Items[item] < quantity)) {
            return false;
        }

        RemoveInventoryItem(item, quantity);
        if (game.Bag.Items[item] == 0) {
            ApplyItemCarryModifiers(item, -1);
        }
        return true;
    }

    public void AcquirePermit() {
        game.Bag.Permit++;
        game.Bag.Count++;
    }

    public bool LosePermit() {
        if (game.Bag.Permit < 1) {
            return false;
        }

        game.Bag.Permit--;
        game.Bag.Count--;
        return true;
    }

    public void DropItem(TBagItem item, string verb, string line) {
        if ((game.Bag.Items[item] > 0) && !IsEquipped(item)) {
            Out.WriteLine(verb + line);
            LoseItem(item);
            game.Bag.AddGroundItem(item, game.Session.OuterRoom);
        }
    }

    public bool Equip(TBagItem item) {
        if ((game.Bag.Items[item] == 0) || IsEquipped(item) || !GearEffects.TryGetValue(item, out TGearEffect? effect)) {
            return false;
        }
        if (effect.Garment != null && game.Gear.WornGarment != TxtEmpty) {
            return false;
        }

        LoseItem(item);
        ApplyGear(effect, 1);
        return true;
    }

    public bool Unequip(TBagItem item) {
        if (!IsEquipped(item)) {
            return false;
        }

        AcquireItem(item);
        ApplyGear(GearEffects[item], -1);
        return true;
    }

    void ApplyGear(TGearEffect effect, int sign) {
        if (effect.Slot is TGearItem slot) {
            game.Gear.Values[slot] = (sign + 1) / 2;
        }
        if (effect.Garment != null) {
            game.Gear.Values[TGearItem.Wearing] = (sign + 1) / 2;
            game.Gear.WornGarment = sign > 0 ? effect.Garment : TxtEmpty;
        }

        game.Player.Stats[TStat.Strength] += sign * effect.Strength;
        game.Gear.Values[TGearItem.LuckRoll] += sign * effect.Luck;
        game.Gear.Values[TGearItem.Armor] += sign * effect.Armor;
        game.Gear.Values[TGearItem.WornCount] += sign * effect.Worn;
    }

    static readonly Dictionary<TBagItem, TGearEffect> GearEffects = new() {
        [TBagItem.OldSword] = new(Slot: TGearItem.WeaponKind, Strength: 1, Luck: 3),
        [TBagItem.SmallShield] = new(Slot: TGearItem.ShieldKind, Armor: 15, Worn: 1),
        [TBagItem.Tracksuit] = new(Garment: TxtWornSyf, Armor: 7),
        [TBagItem.SpikedSuit] = new(Garment: TxtWornGarnitur, Armor: 10, Luck: 15, Worn: 1),
    };

    public void TakeDrop() {
        if (TakeItems.TryGetValue(game.Command, out TTakeDrop? take)) {
            TakeItem(take.item, take.verb, take.line);
        }
        if (DropItems.TryGetValue(game.Command, out TTakeDrop? drop)) {
            DropItem(drop.item, drop.verb, drop.line);
        }
    }

    public void ClampStat(ref int stat, int cap) {
        if (stat > cap) {
            stat = cap;
        }
    }

    public void EatFood(TBagItem item, string verb, string line, int gain, IntSlot stat, IntSlot cap) {
        if (LoseItem(item)) {
            Out.WriteLine(verb + line);
            stat.Set((stat.Get() + gain));
            int tmp = stat.Get();
            ClampStat(ref tmp, cap.Get());
            stat.Set(tmp);
        }
    }

    public void UseItems() {
        if (FoodItems.TryGetValue(game.Command, out TFoodItem? food)) {
            EatFood(food.item, food.verb, food.line, food.gain, food.stat, food.cap);
        }
        if (UseCommands.TryGetValue(game.Command, out Action? use)) {
            use();
        }
    }

    void UseTeleportPill() {
        if (game.Bag.Items[TBagItem.TeleportPill] == 0) {
            return;
        }

        Out.WriteLine(TxtUseItems_32);
        LoseItem(TBagItem.TeleportPill);
        game.World.SpawnMonsters();
        if (game.Player.Stats[TStat.Wisdom] < 10) {
            Out.WriteLine(TxtUseItems_33);
            game.Player.MaxEnergy -= 1;
            game.Player.Energy = 1;
            game.Player.Experience -= 50;
        } else if (game.Player.Stats[TStat.Wisdom] < 16) {
            Out.WriteLine(TxtUseItems_35);
            if (!game.Player.ChangeEnergy(-40)) {
                game.Player.Energy = 1;
            }

            game.Player.Experience -= 30;
        } else {
            Out.WriteLine(TxtUseItems_37);
        }
    }

    void SayIfHeld(TBagItem item, string line) {
        if (game.Bag.Items[item] > 0) {
            Out.WriteLine(line);
        }
    }

    void SayIf(bool changed, string line) {
        if (changed) {
            Out.WriteLine(line);
        }
    }

    Dictionary<string, Action> UseCommands => _useCommands ??= new() {
        [$"{CmdUzyj}{CmdFajka}"] = () => SayIfHeld(TBagItem.Pipe, TxtUseItems_01),
        [$"{CmdUzyj}{CmdSerce}"] = () => {
            if (LoseItem(TBagItem.Heart)) {
                Out.WriteLine($"{TxtEatVerb}{TxtUseItems_02}");
                game.Player.ChangeEnergy(5);
            }
        },
        [$"{CmdUzyj}{CmdDyplom}"] = () => {
            if (game.Bag.Items[TBagItem.Diploma] > 0) {
                game.WriteLines([
                    TxtUseItems_03,
                    TxtUseItems_06,
                    TxtUseItems_06,
                    TxtUseItems_06,
                    TxtUseItems_07,
                ]);
            }
        },
        [$"{CmdUzyj}{CmdKaseta}"] = () => Out.WriteLine(TxtUseItems_24),
        [$"{CmdUzyj}{CmdKomplet}"] = () => SayIf(Equip(TBagItem.Tracksuit), TxtUseItems_28),
        [$"{CmdUzyj}{CmdListek}"] = () => SayIfHeld(TBagItem.LuckyLeaf, TxtUseItems_30),
        [$"{CmdUzyj}{TxtWornGarnitur}"] = () => SayIf(Equip(TBagItem.SpikedSuit), TxtUseItems_31),
        [$"{CmdUzyj}{CmdPigulka}"] = UseTeleportPill,
        [$"{CmdZniszcz}{CmdPrzepustka}"] = () => SayIf(LosePermit(), TxtUseItems_16),
        [$"{CmdPatrz}{CmdPrzepustka}"] = () => {
            if (game.Bag.Permit > 0) {
                game.WriteLines([
                    TxtUseItems_17,
                    TxtUseItems_18,
                    TxtUseItems_17,
                ]);
            }
        },
        [$"{CmdOdloz}{CmdKomplet}"] = () => SayIf(Unequip(TBagItem.Tracksuit), TxtUseItems_29),
        [$"{CmdOdloz}{TxtWornGarnitur}"] = () => SayIf(Unequip(TBagItem.SpikedSuit), TxtUseItems_38),
    };
    Dictionary<string, Action>? _useCommands;

    public void RecalcBag() {
        game.Bag.Count = Enum.GetValues<TBagItem>().Sum(item => game.Bag.Items[item]) + game.Bag.Permit;
        game.Bag.Limit = game.DexterityItemLimits[0].limit;
        for (int i = game.DexterityItemLimits.Count - 1; i >= 0; i--) {
            if (game.Player.Stats[TStat.Dexterity] >= game.DexterityItemLimits[i].gate) {
                game.Bag.Limit = game.DexterityItemLimits[i].limit;
                break;
            }
        }

        if (game.Bag.Items[TBagItem.Backpack] > 0) {
            game.Bag.Limit += 4;
        }
    }

    void AddInventoryItem(TBagItem item, int quantity) {
        game.Bag.Items[item] += quantity;
        game.Bag.Count += quantity;
    }

    void RemoveInventoryItem(TBagItem item, int quantity) {
        game.Bag.Items[item] -= quantity;
        game.Bag.Count -= quantity;
    }

    internal void SetInventoryItemCount(TBagItem item, int count) {
        if (count > game.Bag.Items[item]) {
            AcquireItem(item, count - game.Bag.Items[item]);
        } else if (count < game.Bag.Items[item]) {
            LoseItem(item, game.Bag.Items[item] - count);
        }
    }

    void ApplyItemCarryModifiers(TBagItem item, int sign) {
        if (!ItemCarryModifiers.TryGetValue(item, out TItemCarryModifiers? modifiers)
            || modifiers is null) {
            return;
        }

        game.Gear.Values[TGearItem.Armor] += sign * modifiers.armor;
        game.Player.MaxEnergy += sign * modifiers.maxEnergy;
        game.Player.Stats[TStat.Dexterity] += sign * modifiers.dexterity;
        game.Player.Stats[TStat.Wisdom] += sign * modifiers.wisdom;
        game.Player.MaxMana += sign * modifiers.maxMana;
    }

    public Dictionary<string, TTakeDrop> TakeItems => _takeItems ??= new() {
        [$"{CmdBierz}{CmdStary}"] = new() { item = TBagItem.OldSword, verb = TxtPickUpVerb, line = TxtTakeDrop_01 },
        [$"{CmdBierz}{CmdMala}"] = new() { item = TBagItem.SmallShield, verb = TxtPickUpVerb, line = TxtTakeDrop_02 },
        [$"{CmdBierz}{CmdSerce}"] = new() { item = TBagItem.Heart, verb = TxtPickUpVerb, line = TxtTakeDrop_03 },
        [$"{CmdBierz}{CmdDyplom}"] = new() { item = TBagItem.Diploma, verb = TxtPickUpVerb, line = TxtTakeDrop_04 },
        [$"{CmdBierz}{CmdFajka}"] = new() { item = TBagItem.Pipe, verb = TxtPickUpVerb, line = TxtTakeDrop_05 },
    };
    Dictionary<string, TTakeDrop>? _takeItems;

    public Dictionary<string, TTakeDrop> DropItems => _dropItems ??= new() {
        [$"{CmdOdrzuc}{CmdStary}"] = new() { item = TBagItem.OldSword, verb = TxtDropVerb, line = TxtTakeDrop_06 },
        [$"{CmdOdrzuc}{CmdFajka}"] = new() { item = TBagItem.Pipe, verb = "", line = TxtTakeDrop_07 },
        [$"{CmdOdrzuc}{CmdMala}"] = new() { item = TBagItem.SmallShield, verb = TxtDropVerb, line = TxtTakeDrop_08 },
        [$"{CmdOdrzuc}{CmdSerce}"] = new() { item = TBagItem.Heart, verb = TxtDropVerb, line = CmdZakrwawioneSerce },
        [$"{CmdOdrzuc}{CmdDyplom}"] = new() { item = TBagItem.Diploma, verb = TxtDropVerb, line = TxtTakeDrop_10 },
    };
    Dictionary<string, TTakeDrop>? _dropItems;

    public Dictionary<string, TFoodItem> FoodItems => _foodItems ??= new() {
        [$"{CmdUzyj}{CmdPaczek}"] = new() { item = TBagItem.Doughnut, verb = TxtEatVerb, line = TxtUseItems_12, gain = 8, stat = game.PlayerEnergy(), cap = game.PlayerMaxEnergy() },
        [$"{CmdUzyj}{CmdCiastko}"] = new() { item = TBagItem.Cookie, verb = TxtEatVerb, line = TxtUseItems_13, gain = 12, stat = game.PlayerEnergy(), cap = game.PlayerMaxEnergy() },
        [$"{CmdUzyj}{CmdSuchaRacja}"] = new() { item = TBagItem.DryRation, verb = TxtEatVerb, line = TxtUseItems_14, gain = 16, stat = game.PlayerEnergy(), cap = game.PlayerMaxEnergy() },
        [$"{CmdUzyj}{CmdBulka}"] = new() { item = TBagItem.BreadRoll, verb = TxtEatVerb, line = TxtUseItems_15, gain = 20, stat = game.PlayerEnergy(), cap = game.PlayerMaxEnergy() },
        [$"{CmdUzyj}{CmdChleb}"] = new() { item = TBagItem.Bread, verb = TxtEatVerb, line = TxtUseItems_23, gain = 26, stat = game.PlayerEnergy(), cap = game.PlayerMaxEnergy() },
        [$"{CmdUzyj}{CmdWeka}"] = new() { item = TBagItem.Sausage, verb = TxtEatVerb, line = TxtUseItems_25, gain = 34, stat = game.PlayerEnergy(), cap = game.PlayerMaxEnergy() },
        [$"{CmdUzyj}{CmdMalaButelkaMany}"] = new() { item = TBagItem.ManaBottle, verb = "", line = TxtUseItems_26, gain = 30, stat = game.PlayerMana(), cap = game.PlayerMaxMana() },
        [$"{CmdUzyj}{CmdBigos}"] = new() { item = TBagItem.Bigos, verb = TxtEatVerb, line = TxtUseItems_27, gain = 20, stat = game.PlayerEnergy(), cap = game.PlayerMaxEnergy() },
    };
    Dictionary<string, TFoodItem>? _foodItems;

    public Dictionary<TBagItem, TItemCarryModifiers> ItemCarryModifiers => _itemCarryModifiers ??= new() {
        [TBagItem.Diploma] = new() { maxEnergy = 5 },
        [TBagItem.Pipe] = new() { wisdom = 1 },
        [TBagItem.Cassette] = new() { armor = -8, dexterity = 1, maxEnergy = 5 },
        [TBagItem.LuckyLeaf] = new() { maxMana = 40 },
    };
    Dictionary<TBagItem, TItemCarryModifiers>? _itemCarryModifiers;
}
