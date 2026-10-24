using static Texts;
using static World;
using static Rooms;

public class Character {
    readonly Game game;

    public Character(Game game) {
        this.game = game;
    }

    public const int Lvl1_Threshold = 700;
    public const int Lvl2_Threshold = 725;
    public const int Lvl3_Threshold = 730;
    public const int Lvl4_Base = 735;
    public const int Lvl9_Base = 735;

    public void TrainAttribute(IntSlot Attribute, IntSlot MaxAttribute, int MinPractice, int PracticeCost, string cmdForm, string nounForm) {
        if ((game.Player.Practice > MinPractice) && (Attribute.Get() < MaxAttribute.Get())) {
            Attribute.Set((Attribute.Get() + 1));
            game.Player.Practice -= PracticeCost;
            Out.WriteLine(TxtTrainAttribute_01 + cmdForm + TxtTrainAttribute_02 + Attribute.Get() + TxtTrainAttribute_03 + nounForm + TxtTrainAttribute_04 + game.Player.Practice + TxtShared_19);
        }
    }

    public void HandleTrain() {
        if (game.Verb == CmdTrenuj) {
            Out.WriteLine(TxtHandleTrain_01);
        }

        for (int ti = 0; ti < Trainables.Count; ti++) {
            TTrainable t = Trainables[ti];
            if (game.Matches(t.cmd)) {
                TrainAttribute(IntSlot.Into(game.Player.Stats, t.stat), IntSlot.Into(game.Player.Stats, t.maxStat), t.minPractice, t.cost, t.cmdForm, t.nounForm);
            }
        }
    }

    public int LevelThreshold(int lvl) =>
        game.BandAmount(LvlThresholds, lvl);

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
        game.RecalcBag();
        Out.WriteLine($"{TxtShowCharacter_30}{game.Player.Name}");
        Out.WriteLine(TxtShowCharacter_01);
        for (int si = 0; si < SheetItems.Count; si++) {
            TSheetItem s = SheetItems[si];
            ShowSheetItem(s.kind, s.item.Get(), s.desc, s.width);
        }
        Out.WriteLine(TxtShowCharacter_02 + game.Bag.Count + TxtShowCharacter_03 + game.Bag.Limit + TxtShowCharacter_04);
        if (game.Gear.Values[TGearItem.WeaponKind] == 0) {
            Out.WriteLine(TxtShowCharacter_05);
        }

        if (game.Gear.Values[TGearItem.ShieldKind] == 0) {
            Out.WriteLine(TxtShowCharacter_06);
        }

        if (game.Gear.WornGarment == TxtEmpty) {
            Out.WriteLine(TxtShowCharacter_07);
        }

        if (game.Gear.WornGarment == TxtWornSyf) {
            Out.WriteLine(TxtWearingLabel + TxtKompletUbranFirmySyf);
        }

        if (game.Gear.WornGarment == TxtWornGarnitur) {
            Out.WriteLine(TxtWearingLabel + TxtGarniturZKolcami);
        }

        if (game.Gear.Values[TGearItem.ShieldKind] == 1) {
            Out.WriteLine(TxtShowCharacter_10);
        }

        if (game.Gear.Values[TGearItem.WeaponKind] == 1) {
            Out.WriteLine(TxtShowCharacter_11);
        }

        Out.WriteLine(TxtShowCharacter_12 + game.Player.Money + TxtShowCharacter_13 + game.Player.Practice + TxtShared_19);

        if (game.Player.Level > 0) {
            int gap = (LevelThreshold(game.Player.Level) - game.Player.Experience);
            Out.WriteLine(TxtShowCharacter_18 + game.Player.Level + TxtShowCharacter_19 + gap + TxtShared_22);
        }

        Out.WriteLine(TxtShowCharacter_21 + game.Player.Stats[TStat.Strength] + TxtShowCharacter_03 + game.Player.Stats[TStat.MaxStrength]
            + TxtShowCharacter_22 + game.Player.Stats[TStat.Dexterity] + TxtShowCharacter_03 + game.Player.Stats[TStat.MaxDexterity]
            + TxtShowCharacter_23 + game.Player.Stats[TStat.Wisdom] + TxtShowCharacter_03 + game.Player.Stats[TStat.MaxWisdom]);
        game.Gear.Values[TGearItem.ArmorRating] = (10 * game.Gear.Values[TGearItem.WornCount] + game.Gear.Values[TGearItem.Armor]);
        Out.WriteLine(TxtShowCharacter_24 + game.Gear.Values[TGearItem.ArmorRating]
            + TxtShowCharacter_25 + game.Gear.Values[TGearItem.LuckRoll]);
        Out.WriteLine(TxtShowCharacter_12 + game.Player.Mana + TxtShowCharacter_03 + game.Player.MaxMana + TxtShowCharacter_26);
        Out.WriteLine(TxtShowCharacter_12 + game.Player.Energy + TxtShowCharacter_03 + game.Player.MaxEnergy + TxtShared_20);
        if (game.World.Quest > 0) {
            bool needLiroy = (game.World.Quest > 1) && !game.World.LiroyKilled;
            Out.WriteLine(game.World.QuestKills > 0
                ? TxtShowCharacter_27 + game.World.QuestKills + (needLiroy ? TxtShowCharacter_31 : TxtShowCharacter_28)
                : needLiroy ? TxtShowCharacter_32 : TxtShowCharacter_33);
        }

        if (game.World.DuncanQuest != 0) {
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
        game.ShowIntro();
        Out.WriteLine(TxtChooseRace_02);
        while (true) {
            game.Player.Name = Crt.ReadLn();
            if (game.Player.Name.Length == 0) {
                Out.WriteLine(TxtChooseRace_15);
            } else if (game.Player.Name.Any(c => !IsNameChar(c))) {
                Out.WriteLine(TxtChooseRace_13);
            } else if (game.Player.Name.Length > 63) {
                Out.WriteLine(TxtChooseRace_14);
            } else {
                break;
            }
        }
        if (File.Exists(game.SaveFileName())) {
            game.LoadGame();
            game.Session.SavedFlag = false;
            return;
        }
        do {
            Out.WriteLine(TxtChooseRace_01);
            game.Command = Crt.ReadLn();
            chosen = FindRace(game.Command);
            if (chosen >= 0) {
                foreach (TStat stat in Enum.GetValues<TStat>()) {
                    game.Player.Stats[stat] = Races[chosen].baseStats[stat];
                }

                game.Player.MaxMana += Races[chosen].MaxManaBonus;
                game.Player.Money += Races[chosen].MoneyBonus;
                game.Player.MaxEnergy += Races[chosen].MaxEnergyBonus;
            }
        } while (chosen < 0);
        game.Player.Energy = (50 + Races[chosen].EnergyBonus);
        game.Player.Practice = 10;
        game.SpawnAt(game.World.CageInits);
        game.Player.Level = 1;
        game.Player.MaxMana += 100;
        game.Player.Mana = game.Player.MaxMana;
        game.SpawnMonsters();
        game.Bag.AddGroundItem(TBagItem.OldSword, RoomStart);
        Out.WriteLine(TxtChooseRace_03);
    }

    public bool GatesMet(int reqWisdom, int reqStrength, int reqDexterity) =>
        ((reqWisdom == -1) || (game.Player.Stats[TStat.Wisdom] > reqWisdom)) &&
        ((reqStrength == -1) || (game.Player.Stats[TStat.Strength] > reqStrength)) &&
        ((reqDexterity == -1) || (game.Player.Stats[TStat.Dexterity] > reqDexterity));

    public void WriteTrainingGain(string skillName, int value) =>
        Out.WriteLine(TxtTrainingVerb + skillName + TxtTrainingThreshold + value
            + TxtTraining_29 + game.Player.Practice + TxtShared_19);

    public void Training() {
        switch (game.Verb) {
            case CmdPatrz:
                if (game.Rest == CmdPlakat) {
                    Out.WriteLine(TxtShared_13);
                    Out.WriteLine(TxtTraining_01);
                    if (game.Player.Stats[TStat.Wisdom] < 11) {
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
                switch (game.Rest) {
                    case CmdKopac:
                        if ((game.Player.Stats[TStat.Wisdom] > 10) && (game.Player.Stats[TStat.Strength] > 11) && (game.Player.Practice > 0)) {
                            game.Skills.Values[TSkill.Kick] += game.Player.Stats[TStat.Wisdom];
                            game.Player.Practice -= 1;
                            WriteTrainingGain(TxtTrainingDigging, game.Skills.Values[TSkill.Kick]);
                        }
                        break;
                    case CmdUciekac:
                        if (game.Player.Stats[TStat.Wisdom] > 10) {
                            if (game.Player.Stats[TStat.Dexterity] > 10) {
                                if ((game.Player.Practice > 0) && (game.Skills.Values[TSkill.Fleeing] < 85)) {
                                    game.Skills.Values[TSkill.Fleeing] += game.Player.Stats[TStat.Wisdom] + game.Player.Stats[TStat.Dexterity] - 5;
                                    game.Player.Practice -= 1;
                                    WriteTrainingGain(TxtTrainingFleeing, game.Skills.Values[TSkill.Fleeing]);
                                } else {
                                    Out.WriteLine(TxtTraining_31);
                                }
                            }
                        }

                        break;
                    case CmdPowrot:
                        if ((game.Player.Stats[TStat.Wisdom] > 17) && (game.Skills.Values[TSkill.Returning] < 90) && (game.Player.Practice > 0)) {
                            game.Skills.Values[TSkill.Returning] += 2 * game.Player.Stats[TStat.Wisdom] - 3;
                            game.Player.Practice -= 1;
                            WriteTrainingGain(TxtTrainingReturning, game.Skills.Values[TSkill.Returning]);
                        }
                        break;
                    case TxtTrainingParrying:
                        if ((game.Player.Stats[TStat.Wisdom] > 15) && (game.Player.Stats[TStat.Dexterity] > 11) && (game.Skills.Values[TSkill.Parry] < 90) && (game.Player.Practice > 0)) {
                            game.Skills.Values[TSkill.Parry] += game.Player.Stats[TStat.Wisdom] + game.Player.Stats[TStat.Dexterity] - 14;
                            game.Player.Practice -= 1;
                            WriteTrainingGain(TxtTrainingParrying, game.Skills.Values[TSkill.Parry]);
                        }
                        break;
                    case CmdPorownanie:
                        if ((game.Player.Stats[TStat.Wisdom] > 11) && (game.Skills.Values[TSkill.Comparing] < 90) && (game.Player.Practice > 0)) {
                            game.Skills.Values[TSkill.Comparing] += 3 * game.Player.Stats[TStat.Wisdom] - 9;
                            game.Player.Practice -= 1;
                            WriteTrainingGain(TxtTrainingComparing, game.Skills.Values[TSkill.Comparing]);
                        }
                        break;
                    case TxtTrainingCooking:
                        if ((game.Player.Stats[TStat.Wisdom] > 18) && (game.Skills.Values[TSkill.Cooking] < 90) && (game.Player.Practice > 0)) {
                            game.Skills.Values[TSkill.Cooking] += game.Player.Stats[TStat.Wisdom] + 1;
                            game.Player.Practice -= 1;
                            WriteTrainingGain(TxtTrainingCooking, game.Skills.Values[TSkill.Cooking]);
                        }
                        break;
                }
                break;
        }
    }

    public void CmdSetKickThresholds() {
        if (game.Skills.Values[TSkill.Kick] > 0) {
            Out.WriteLine(TxtCmdSetKickThresholds_01);
            game.Skills.Values[TSkill.KickMaxEnergy] = Crt.ReadInt();
            Out.WriteLine(TxtCmdSetKickThresholds_03);
            game.Skills.Values[TSkill.KickMinMana] = Crt.ReadInt();
        }
    }

    public void CmdShowSkills() {
        for (int i = 0; i < SkillLines.Count; i++) {
            if (GatesMet(SkillLines[i].wisdom, SkillLines[i].strength, SkillLines[i].dexterity)) {
                TSkillLine s = SkillLines[i];
                Out.WriteLine(s.text1 + game.Skills.Values[s.skill] + s.text2);
            }
        }
    }

    public void CheckLevelUp() {
        int threshold;
        if (game.Player.Level < 1) {
            return;
        }

        threshold = LevelThreshold(game.Player.Level);
        if (game.Player.Experience < threshold) {
            return;
        }

        game.WriteRainbow(TxtCheckLevelUp_01);
        Out.WriteLine();

        game.Player.Experience -= game.BandAmount(LevelCosts, game.Player.Level);
        game.Player.Practice += game.BandAmount(PracticeGains, game.Player.Stats[TStat.Wisdom]);
        Out.WriteLine(TxtCheckLevelUp_02 + game.Player.Practice + TxtCheckLevelUp_03);
        game.Player.MaxEnergy += 10;
        for (int ei = 0; ei < MaxEnergyGains.Count; ei++) {
            TEnergyBonus e = MaxEnergyGains[ei];
            if ((game.Player.Stats[TStat.Dexterity] > e.dexLo) && (game.Player.Stats[TStat.Strength] > e.strLo)) {
                game.Player.MaxEnergy += e.bonus;
            }
        }
        Out.WriteLine(TxtCheckLevelUp_02 + game.Player.MaxEnergy + TxtCheckLevelUp_04);
        game.Player.MaxMana += game.Player.Stats[TStat.Wisdom] + 2;
        Out.WriteLine(TxtCheckLevelUp_05 + (game.Player.Stats[TStat.Wisdom] + 2) + TxtCheckLevelUp_06);
        game.Player.Level += 1;
        game.Session.CheckpointStage = 0;
        if (game.Player.Level == 12) {
            Out.WriteLine(TxtCheckLevelUp_07);
            if (game.Player.Stats[TStat.Strength] >= game.Player.Stats[TStat.MaxStrength]) {
                game.Player.Stats[TStat.MaxStrength] += 3;
            }

            if (game.Player.Stats[TStat.Dexterity] >= game.Player.Stats[TStat.MaxDexterity]) {
                game.Player.Stats[TStat.MaxDexterity] += 3;
            }

            if (game.Player.Stats[TStat.Wisdom] >= game.Player.Stats[TStat.MaxWisdom]) {
                game.Player.Stats[TStat.MaxWisdom] += 3;
            }

            if (game.Player.Stats[TStat.Strength] < game.Player.Stats[TStat.MaxStrength]) {
                game.Player.Stats[TStat.MaxStrength] += 2;
            }

            if (game.Player.Stats[TStat.Dexterity] < game.Player.Stats[TStat.MaxDexterity]) {
                game.Player.Stats[TStat.MaxDexterity] += 2;
            }

            if (game.Player.Stats[TStat.Wisdom] < game.Player.Stats[TStat.MaxWisdom]) {
                game.Player.Stats[TStat.MaxWisdom] += 2;
            }

            if ((game.Player.Stats[TStat.MaxWisdom] > game.Player.Stats[TStat.MaxStrength]) && (game.Player.Stats[TStat.MaxWisdom] > game.Player.Stats[TStat.MaxDexterity])) {
                game.Player.Stats[TStat.MaxWisdom] += 3;
            }

            if ((game.Player.Stats[TStat.MaxStrength] > game.Player.Stats[TStat.MaxWisdom]) && (game.Player.Stats[TStat.MaxStrength] > game.Player.Stats[TStat.MaxDexterity])) {
                game.Player.Stats[TStat.MaxStrength] += 3;
            }

            if ((game.Player.Stats[TStat.MaxDexterity] > game.Player.Stats[TStat.MaxStrength]) && (game.Player.Stats[TStat.MaxDexterity] > game.Player.Stats[TStat.MaxWisdom])) {
                game.Player.Stats[TStat.MaxDexterity] += 3;
            }

            if ((game.Player.Stats[TStat.MaxDexterity] == game.Player.Stats[TStat.MaxStrength]) && (game.Player.Stats[TStat.MaxStrength] == game.Player.Stats[TStat.MaxWisdom])) {
                game.Player.Stats[TStat.MaxDexterity] += 1;
                game.Player.Stats[TStat.MaxWisdom] += 1;
                game.Player.Stats[TStat.MaxStrength] += 1;
            }
            if ((game.Player.Stats[TStat.MaxDexterity] > game.Player.Stats[TStat.MaxWisdom]) && (game.Player.Stats[TStat.MaxDexterity] == game.Player.Stats[TStat.MaxStrength])) {
                game.Player.Stats[TStat.MaxStrength] += 2;
                game.Player.Stats[TStat.MaxDexterity] += 1;
            }
            Out.WriteLine(TxtCheckLevelUp_08 + game.Player.Stats[TStat.MaxStrength]
                + TxtMaxymalnie + TxtCheckLevelUp_09);
            Out.WriteLine(TxtCheckLevelUp_08 + game.Player.Stats[TStat.MaxDexterity]
                + TxtMaxymalnie + TxtCheckLevelUp_10);
            Out.WriteLine(TxtCheckLevelUp_08 + game.Player.Stats[TStat.MaxWisdom]
                + TxtMaxymalnie + TxtCheckLevelUp_11);
        }
    }

    IntSlot BagItemSlot(TBagItem item) =>
        new(() => game.Bag.Items[item], v => game.Bag.Items[item] = v);

    public List<TTrainable> Trainables => _trainables ??= [
        new() { cmd = CmdTrenuj2 + CmdSila, stat = TStat.Strength, maxStat = TStat.MaxStrength, minPractice = 2, cost = 3, cmdForm = CmdSile, nounForm = CmdSily },
        new() { cmd = CmdTrenuj2 + CmdZrecznosc, stat = TStat.Dexterity, maxStat = TStat.MaxDexterity, minPractice = 1, cost = 2, cmdForm = CmdZrecznosc, nounForm = CmdZrecznosci },
        new() { cmd = CmdTrenuj2 + CmdMadrosc, stat = TStat.Wisdom, maxStat = TStat.MaxWisdom, minPractice = 2, cost = 3, cmdForm = CmdMadrosc, nounForm = CmdMadrosci },
    ];
    List<TTrainable>? _trainables;

    public List<TBand> LvlThresholds => _lvlThresholds ??= [
        new(1, 1, Lvl1_Threshold, 0),
        new(2, 2, Lvl2_Threshold, 0),
        new(3, 3, Lvl3_Threshold, 0),
        new(4, 8, Lvl4_Base, 1),
        new(9, Game.MaxInt, Lvl9_Base, 2),
    ];
    List<TBand>? _lvlThresholds;

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
        new() { item = new IntSlot(() => game.Bag.Permit, v => game.Bag.Permit = v), desc = CmdPrzepustkaWlasnoreczniePodpisanaPrzezQuestMastera, width = 0, kind = TSheetKind.ikPlain },
        new() { item = BagItemSlot(TBagItem.CompareScroll), desc = CmdScrollZCzaremPorownywanie, width = 0, kind = TSheetKind.ikPlain },
        new() { item = BagItemSlot(TBagItem.ReturnScroll), desc = CmdScrollZCzaremPowrot, width = 0, kind = TSheetKind.ikPlain },
    ];
    List<TSheetItem>? _sheetItems;

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

    public List<TBand> LevelCosts => _levelCosts ??= [
        new(1, 1, 725, 0),
        new(2, 2, 730, 0),
        new(3, 3, 735, 0),
        new(4, 8, Lvl4_Base, 1),
        new(9, Game.MaxInt, Lvl9_Base, 2),
    ];
    List<TBand>? _levelCosts;

    public List<TBand> PracticeGains => _practiceGains ??= [
        new(-128, 10, 3, 0),
        new(11, 15, 4, 0),
        new(16, 22, 5, 0),
        new((-Game.MaxInt - 1), Game.MaxInt, 6, 0),
    ];
    List<TBand>? _practiceGains;

    public List<TEnergyBonus> MaxEnergyGains => _maxEnergyGains ??= [
        new() { dexLo = 12, strLo = (-Game.MaxInt - 1), bonus = 1 },
        new() { dexLo = 16, strLo = (-Game.MaxInt - 1), bonus = 1 },
        new() { dexLo = 21, strLo = (-Game.MaxInt - 1), bonus = 1 },
        new() { dexLo = 26, strLo = 20, bonus = 1 },
    ];
    List<TEnergyBonus>? _maxEnergyGains;

    public List<TSkillLine> SkillLines => _skillLines ??= [
        new() { wisdom = 10, strength = 11, dexterity = -1, text1 = TxtCmdShowSkills_01, skill = TSkill.Kick, text2 = TxtShared_26 },
        new() { wisdom = 10, strength = -1, dexterity = 10, text1 = TxtCmdShowSkills_02, skill = TSkill.Fleeing, text2 = TxtShared_26 },
        new() { wisdom = 15, strength = -1, dexterity = 11, text1 = TxtCmdShowSkills_03, skill = TSkill.Parry, text2 = TxtShared_26 },
        new() { wisdom = 11, strength = -1, dexterity = -1, text1 = TxtCmdShowSkills_04, skill = TSkill.Comparing, text2 = TxtShared_26 },
        new() { wisdom = 18, strength = -1, dexterity = -1, text1 = TxtCmdShowSkills_05, skill = TSkill.Cooking, text2 = TxtShared_26 },
        new() { wisdom = 19, strength = -1, dexterity = -1, text1 = TxtCmdShowSkills_06, skill = TSkill.Returning, text2 = TxtShared_26 },
    ];
    List<TSkillLine>? _skillLines;
}
