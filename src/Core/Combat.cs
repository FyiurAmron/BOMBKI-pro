using static Texts;
using static Rooms;

public class Combat : TCombat {
    readonly Game game;

    public Combat(Game game) {
        this.game = game;
    }

    public void TryLoot(TBagItem item, int threshold, string foundMsg) {
        Roll = game.Roll(20);
        if (Roll < threshold) {
            game.Bag.AddGroundItem(item, game.Session.Room);
            Out.WriteLine(TxtShared_08 + foundMsg);
        }
    }

    public void TryCageLoot(int threshold) {
        TryLoot(TBagItem.OldSword, threshold, TxtShared_10);
        TryLoot(TBagItem.SmallShield, threshold, TxtShared_11);
        TryLoot(TBagItem.Heart, 6, TxtShared_12);
    }

    public int FightExpForEnergy(int diff) =>
        diff > 0
            ? game.BandAmount(game.PosExpBands, diff)
            : diff < 0 ? game.BandAmount(game.NegExpBands, (-diff)) : 11;

    static int ExpForStep(int diff, int step) =>
        diff == 0 ? 11 : diff > 0 ? Math.Max(1, 11 - step) : Math.Min(21, 11 + step);

    public int FightExpForStrength(int diff) =>
        ExpForStep(diff, Math.Abs(diff));

    public int FightExpForDexterity(int diff) =>
        ExpForStep(diff, (Math.Abs(diff) + 1) / 2);

    static int Bonus(int value, params List<(int over, int amount)> steps) =>
        steps.Where(step => value > step.over).Sum(step => step.amount);

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

    public int ParryBonus(int value) =>
        Bonus(value, (50, 1), (75, 1));

    public int KickBonus(int value) =>
        Bonus(value, (10, 2), (70, 2));

    public void FightDog() {
        EnemyEnergy = 10;
        EnemyDexterity = 10;
        EnemyStrength = 5;
        DoFight();
        Loot = game.Roll(15);
        Out.WriteLine(TxtShared_08 + Loot + TxtFightDog_01);
        game.Player.Money += Loot;
        TryLoot(TBagItem.Heart, 5, TxtFightDog_02);
    }

    public void CompositeFight(int EnemyEnergyBase, int EnemyEnergyRand, int EnemyDexterityBase, int EnemyDexterityRand, int EnemyStrengthBase, int EnemyStrengthRand,
        int LootBase, int LootRand, int HeartChance, bool RequireEnergy, bool AntDoughnut) {
        EnemyEnergy = EnemyEnergyRand > 0 ? game.Roll(EnemyEnergyRand) + EnemyEnergyBase : EnemyEnergyBase;

        EnemyDexterity = EnemyDexterityRand > 0 ? game.Roll(EnemyDexterityRand) + EnemyDexterityBase : EnemyDexterityBase;

        EnemyStrength = EnemyStrengthRand > 0 ? game.Roll(EnemyStrengthRand) + EnemyStrengthBase : EnemyStrengthBase;

        DoFight();
        if (!Fled && (!RequireEnergy || (game.Player.Energy > 0))) {
            Loot = (game.Roll(LootRand) + LootBase);
            Out.WriteLine(TxtShared_08 + Loot + TxtCompositeFight_01);
            game.Player.Money += Loot;
            if (AntDoughnut) {
                if (game.Verb == CmdZabij && game.Rest == CmdMrowka) {
                    Roll = game.Roll(10);
                    if (Roll < 7) {
                        Out.WriteLine(TxtShared_08 + TxtCompositeFight_02);
                        game.Inventory.AcquireItem(TBagItem.Doughnut);
                    }
                }
            } else if (HeartChance > 0) {
                Roll = game.Roll(20);
                if (Roll < HeartChance) {
                    Out.WriteLine(TxtShared_08 + TxtCompositeFight_03);
                    game.Inventory.AcquireItem(TBagItem.Heart);
                }
            }
        }
    }

    public void Fight(TFoe foe) {
        TFoeStats f = Foes[foe];
        CompositeFight(f.energyBase, f.energyRand, f.dexterityBase, f.dexterityRand,
            f.strengthBase, f.strengthRand, f.lootBase, f.lootRand, f.heartChance,
            f.requireEnergy, f.antDoughnut);
    }

    public void KillBeasts() {
        if (!BeastKills.TryGetValue(game.Command, out TBeastKill? kill) || (game.Session.Room != game.World.Monsters[kill.beast])) {
            return;
        }

        Fight(kill.foe);
        if (!(kill.unlessFled && Fled)) {
            game.World.Monsters[kill.beast] = 0;
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

    static readonly List<string> DogCompareCommands = [CmdSpaniel, CmdOwczarek, CmdPiesek, CmdJamnik, CmdPudel];

    Dictionary<string, List<TCompareFoeRule>> CompareFoeTable => _compareFoeTable ??= new List<(List<TCompareFoeRule> rules, List<string> cmds)> {
        (BeastCompareRules, [CmdDzik, CmdSzczur, CmdLis, CmdKuropatwa, CmdZajac, CmdWilczur, CmdOrzel, CmdSarna, CmdDziecko, CmdDziadek]),
        (BigBeastCompareRules, [CmdSlon, CmdLew, CmdZyrafa, CmdWielblad, CmdStrus, CmdBoa, CmdWilk, CmdBizon, CmdPantera, CmdWariat, CmdSluchacz, CmdFan, CmdCzlowiek, CmdReporter]),
        (FighterCompareRules, [CmdGladiator, CmdWojownik, CmdTrener, CmdPolicjant, CmdGoryl, CmdOchroniarz]),
        (DrunkCompareRules, [CmdMiniBarman, CmdGrubas, CmdDJ]),
        (MusicianCompareRules, [CmdTaksowkarz, CmdSprzedawca, CmdGitarzysta, CmdPerkusista, CmdOrganista, CmdZebrak, CmdPijak, CmdZamiatacz, CmdSzczaw, CmdStokrotka, CmdKoniczynka, CmdMlecz, CmdDmuchawiec]),
        (IvyCompareRules, [CmdPedal, CmdPara, CmdMaciek, CmdRoza, CmdMalina, CmdTrawa, CmdAgrest, CmdJezyna]),
        (LiroyCompareRules, [CmdLiroy]),
        (PokrzywaCompareRules, [CmdPokrzywa]),
    }.SelectMany(group => group.cmds.Select(cmd => (cmd, group.rules))).ToDictionary(pair => pair.cmd, pair => pair.rules);
    Dictionary<string, List<TCompareFoeRule>>? _compareFoeTable;

    public void CompareFoe() {
        Roll = game.Roll(100);
        Out.WriteLine(TxtCompareFoe_01);
        game.Command = Crt.ReadLn();
        game.Session.Overall = (game.Player.Level + game.Player.Stats[TStat.Strength] + DexterityBonus(game.Player.Stats[TStat.Dexterity]) + ParryBonus(game.Skills.Values[TSkill.Parry]) + KickBonus(game.Skills.Values[TSkill.Kick]));
        if (CompareFoeTable.TryGetValue(game.Verb, out var rules)) {
            CompareFoeRules(rules);
        } else if (DogCompareCommands.Contains(game.Verb)) {
            Out.WriteLine(TxtCompareFoe_34);
        } else if (game.Verb == CmdBakteria) {
            Out.WriteLine(TxtCompareFoe_36);
            game.Player.ChangeMana(5);
        }
        if ((Roll < 3) && (game.Skills.Values[TSkill.Comparing] < 100)) {
            Out.WriteLine(TxtFightStars17 + TxtLearnVerb + TxtCompareFoe_35);
            game.Skills.Values[TSkill.Comparing]++;
            game.Player.Experience += 5;
        }
    }

    public void ShieldBlock() {
        Roll = game.Roll(100);
        if (Roll <= game.Gear.Values[TGearItem.Armor]) {
            Damage -= game.Gear.Values[TGearItem.WornCount];
            if (Damage < 0) {
                Damage = 0;
            }

            Out.WriteLine(TxtShieldBlock_01 + Damage + TxtShared_20);
        }
    }

    public void DoFight() {
        for (int i = 0; i < FightExpStats.Count; i++) {
            TFightExpStat s = FightExpStats[i];
            FightExperience += s.expFor((s.playerStat.Get() - s.enemyStat.Get()));
        }

        do {
            EnemyDodged = false;
            PlayerDodged = false;
            Out.WriteLine(TxtFightEngagedEnemy + EnemyEnergy + TxtFightEngagedYou + game.Player.Energy + TxtFightEngagedEnd);
            if (game.Player.Stats[TStat.Dexterity] > EnemyDexterity) {
                DodgeChance = (game.Player.Stats[TStat.Dexterity] - EnemyDexterity);
                Roll = game.Roll(DodgeBound(DodgeChance));
                if (Roll < 10) {
                    Out.WriteLine(TxtFightYouDodgeMocking);
                    PlayerDodged = true;
                }
                Roll = game.Roll(EnemyDodgeBound(DodgeChance));
                if (Roll < 10) {
                    Out.WriteLine(TxtEnemy + TxtFightEnemyDodgesLamersAttack);
                    EnemyDodged = true;
                }
            }
            if (EnemyDexterity > game.Player.Stats[TStat.Dexterity]) {
                DodgeChance = (EnemyDexterity - game.Player.Stats[TStat.Dexterity]);
                Roll = game.Roll(DodgeBound(DodgeChance));
                if (Roll < 10) {
                    Out.WriteLine(TxtEnemy + TxtFightEnemyDodgesWithGrace);
                    EnemyDodged = true;
                }
                Roll = game.Roll(EnemyDodgeBound(DodgeChance));
                if (Roll < 10) {
                    Out.WriteLine(TxtFightYouDodgeWithoutPardon);
                    PlayerDodged = true;
                }
            }
            if (!PlayerDodged) {
                Damage = game.Roll(EnemyStrength);
                switch (Damage) {
                    case >= 0 and <= 5:
                        Out.WriteLine(TxtEnemy + TxtFightScratchedLabel + Damage + TxtFightTailEnergii);
                        break;
                    case >= 6 and <= 20:
                        Out.WriteLine(TxtFightTakenLabel + Damage + TxtFightTailEnergii);
                        break;
                    case >= 21 and <= 50:
                        Out.WriteLine(TxtEnemy + TxtFightMassacredYouLabel + Damage + TxtFightTailEnergii);
                        break;
                    default:
                        Out.WriteLine(TxtEnemy + TxtFightGougedYouLabel + Damage + TxtFightTailEnergii);
                        break;
                }
                FightExperience += 1;
                ShieldBlock();
                if ((game.Skills.Values[TSkill.Parry] > 0) && (Damage > 0)) {
                    Roll = game.Roll(140);
                    if (Roll <= game.Skills.Values[TSkill.Parry]) {
                        switch (Damage) {
                            case >= 0 and <= 2:
                                Out.WriteLine(TxtFightParryLabel + TxtFightParryZeroTail);
                                Damage = 0;
                                break;
                            case >= 3 and <= 9:
                                Damage -= 2;
                                Out.WriteLine(TxtFightParryLabel + Damage + TxtFightTailEnergii);
                                break;
                            default:
                                Damage -= 3;
                                Out.WriteLine(TxtFightParryLabel + Damage + TxtFightTailEnergii);
                                break;
                        }
                        if ((Roll < 1) && (game.Skills.Values[TSkill.Parry] < 100)) {
                            Out.WriteLine(TxtFightStars15 + TxtLearnVerb + TxtFightParryLearn);
                            game.Skills.Values[TSkill.Parry]++;
                            FightExperience += 5;
                        }
                    }
                }
                if (Damage > 0) {
                    game.Player.ChangeEnergy(-Damage);
                }
                if (FireballCharges > 0) {
                    Roll = game.Roll(100);
                    if (Roll < 10) {
                        Crt.TextColor(14);
                        Roll = game.Roll(20);
                        Out.WriteLine(TxtEnemy + TxtFightFireballLabel + Roll + TxtFightTailEnergii);
                        game.Player.ChangeEnergy(-Roll);
                        FireballCharges -= 1;
                        Crt.TextColor(15);
                    }
                }
                if (PoisonCharges > 0) {
                    Roll = game.Roll(100);
                    if (Roll < 10) {
                        Crt.TextColor(4);
                        Out.WriteLine(TxtEnemy + TxtFightCastsPoison);
                        PoisonCharges -= 1;
                        Crt.TextColor(15);
                        PoisonRounds += 10;
                    }
                }
                if (PoisonRounds > 0) {
                    Roll = game.Roll(5);
                    Out.WriteLine(TxtFightPoisonedLabel + Roll + TxtFightTailEnergii);
                    game.Player.ChangeEnergy(-Roll);
                    PoisonRounds -= 1;
                }
            }
            if (!EnemyDodged) {
                Damage = game.Roll(game.Player.Stats[TStat.Strength]);
                if (Damage * 10 < game.Gear.Values[TGearItem.LuckRoll]) {
                    do {
                        Damage = game.Roll(game.Player.Stats[TStat.Strength]);
                        Out.WriteLine(TxtFightLuckRoll);
                    } while (!((Damage * 10) > game.Gear.Values[TGearItem.LuckRoll]));
                }
                EnemyEnergy -= Damage;
                switch (Damage) {
                    case >= 0 and <= 5:
                        Out.WriteLine(TxtFightPushedLabel + Damage + TxtFightTailEnergii);
                        break;
                    case >= 6 and <= 20:
                        Out.WriteLine(TxtFightHitLabel + Damage + TxtFightTailEnergii);
                        break;
                    case >= 21 and <= 50:
                        Out.WriteLine(TxtFightMassacredHimLabel + Damage + TxtFightTailEnergii);
                        break;
                    default:
                        Out.WriteLine(TxtFightGougedHimLabel + Damage + TxtFightTailEnergii);
                        break;
                }
            }
            Crt.Delay(2000);
            if ((game.Skills.Values[TSkill.Kick] > 0) && (game.Player.Mana > game.Skills.Values[TSkill.KickMinMana]) && (game.Player.Energy < game.Skills.Values[TSkill.KickMaxEnergy])) {
                Roll = game.Roll(100);
                if (Roll <= game.Skills.Values[TSkill.Kick] - 10) {
                    Roll = (game.Roll(10) + game.Roll(game.Player.Level));
                    Crt.TextColor(14);
                    Out.WriteLine(TxtFightKickVerb + TxtFightSuperKickLabel + Roll + TxtFightTailEnergii);
                    Crt.TextColor(15);
                    game.Player.ChangeMana(-(game.Roll(3) + 3));
                    EnemyEnergy -= Roll;
                } else {
                    Out.WriteLine(TxtFightKickVerb + TxtFightSuperKickMiss);
                }

                game.Player.ChangeMana(-(game.Roll(2) + 2));
            }
            if ((game.Skills.Values[TSkill.Fleeing] > 0) && (game.Player.Energy < game.Skills.Values[TSkill.FleeEnergy]) && (game.Player.Mana > 14)) {
                game.Player.ChangeMana(-15);
                Roll = game.Roll(100);
                if (Roll <= game.Skills.Values[TSkill.Fleeing]) {
                    Fled = true;
                    Out.WriteLine(TxtFightFled);
                    game.Player.Experience -= 20;
                } else {
                    Out.WriteLine(TxtFightFleeFailed);
                }
            }
        } while (!((game.Player.Energy < 1) || (EnemyEnergy < 1) || Fled));
        if ((EnemyEnergy < 1) && (game.Player.Energy > 0)) {
            FightExperience = Math.Max(0, FightExperience
                - Bonus(game.Player.MaxEnergy, (75, 2), (115, 3))
                - Bonus(game.Skills.Values[TSkill.Parry], (50, 2), (75, 2), (95, 1))
                - Bonus(game.Skills.Values[TSkill.Kick], (50, 5), (95, 2)));

            Out.WriteLine(TxtFightVictoryLabel + FightExperience + TxtShared_22);
            game.Player.Experience = (FightExperience + game.Player.Experience);
            FireballCharges = 0;
            PoisonCharges = 0;

            if (game.World.Quest > 0) {
                game.World.QuestKills -= 1;
            }

            if (game.Skills.Values[TSkill.Cooking] > 0) {
                Roll = game.Roll(100);
                if (game.Skills.Values[TSkill.Cooking] > Roll) {
                    Out.WriteLine(TxtFightMadeBigos);
                    game.Inventory.AcquireItem(TBagItem.Bigos);
                    if ((Roll < 1) && (game.Skills.Values[TSkill.Cooking] < 100)) {
                        Out.WriteLine(TxtFightCookingLearn);
                        game.Skills.Values[TSkill.Cooking]++;
                        game.Player.Experience += 10;
                    }
                }
            }
        }
        if (game.Player.Energy < 1) {
            Out.WriteLine(TxtFightKilled);
            Crt.Delay(3000);
            game.Session.Room = RoomDead;
        }
        FightExperience = 0;
    }

    public void KillArenaFoes() {
        if (!ArenaKills.TryGetValue(game.Command, out TArenaKill? kill) || (game.Session.Room != game.World.Monsters[kill.monster])) {
            return;
        }

        Fight(TFoe.HardEnemy);
        game.World.Monsters[kill.monster] = 0;
        if (kill.pills) {
            game.Inventory.PillGain();
        } else {
            game.Inventory.SuitGain();
        }
    }

    public void StageFights() {
        if (game.Verb == CmdZabij && BandMembers.TryGetValue(game.Rest, out TMonster member) && (game.World.Monsters[member] == game.Session.Room)) {
            Fight(TFoe.VeryEasyEnemy);
            game.World.Monsters[member] = 0;
            game.Inventory.CassetteGain();
        }
        if (game.Matches($"{CmdZabij}{CmdLiroy}") && (game.World.Monsters[TMonster.Liroy] == game.Session.Room)) {
            Fight(TFoe.NotEasyEnemy);
            if ((game.Session.Room != RoomDead) && (game.Player.Energy > 0) && !Fled) {
                Out.WriteLine(TxtStageFights_01);
                game.Player.Money += 30;
                game.Inventory.CassetteGain();
                game.World.LiroyKilled = true;
                game.World.Monsters[TMonster.Liroy] = 0;
            }
        }
    }

    static readonly Dictionary<string, TMonster> BandMembers = new() {
        [CmdPerkusista] = TMonster.Drummer,
        [CmdGitarzysta] = TMonster.Guitarist,
        [CmdOrganista] = TMonster.Keyboardist,
    };

    void KillPlants() {
        if (!PlantKills.TryGetValue(game.Command, out TPlantKill? kill) || (game.World.Monsters[kill.plant] != game.Session.Room)) {
            return;
        }

        Fight(kill.foe);
        if (!kill.ifAlive || (game.Player.Energy > 0)) {
            game.World.Monsters[kill.plant] = 0;
        }
    }

    public void PlantFights() {
        switch (game.Verb) {
            case CmdZabij:
                switch (game.Rest) {
                    case CmdDuncan:
                        if (game.World.Monsters[TMonster.Duncan] == game.Session.Room) {
                            Fight(TFoe.EasyEnemy);
                            if (EnemyEnergy < 1) {
                                game.World.Monsters[TMonster.Duncan] = 0;
                            }
                        }
                        break;
                    case CmdTrawa:
                        if (game.World.Monsters[TMonster.Grass] == game.Session.Room) {
                            Out.WriteLine(TxtPlantFights_17);
                            Fight(TFoe.HardEnemy);
                            for (int i = 2; i <= 4; i++) {
                                if (game.Player.Energy > 0) {
                                    Fight(TFoe.HardEnemy);
                                }
                            }

                            if ((game.Player.Energy > 0) && !Fled) {
                                Out.WriteLine(TxtPlantFights_18);
                            }
                        }
                        break;
                }
                break;
            case CmdSecret:
                if ((game.Rest == CmdLista) && (game.World.Monsters[TMonster.Duncan] == game.Session.Room)) {
                    Out.WriteLine(TxtPlantFights_01);
                }

                break;
            case CmdKup:
                switch (game.Rest) {
                    case CmdDokument:
                        if (game.Player.Money > 399) {
                            Out.WriteLine(TxtPlantFights_05);
                            game.Inventory.AcquirePermit();
                            game.Player.Money -= 400;
                        }
                        break;
                    case CmdPlecak:
                        if ((game.World.Monsters[TMonster.Duncan] == game.Session.Room) && (game.Player.Money > 4799)) {
                            Out.WriteLine(TxtPlantFights_06);
                            game.Player.Money -= 4800;
                            game.Inventory.AcquireItem(TBagItem.Backpack);
                        }
                        break;
                }
                break;
            case CmdRozmawiaj:
                if ((game.Rest == CmdDuncan) && (game.World.Monsters[TMonster.Duncan] == game.Session.Room)) {
                    if (game.World.DuncanQuest == 0) {
                        Out.WriteLine(TxtPlantFights_07);
                        game.World.DuncanQuest = 75;
                    }
                    if (game.World.DuncanQuest != 0) {
                        Out.WriteLine(TxtPlantFights_13);
                    }

                    if (game.World.DuncanQuest == -125) {
                        Out.WriteLine(TxtPlantFights_14);
                        game.World.DuncanQuest = 0;
                        game.Player.Experience += 125;
                        Out.WriteLine(TxtPlantFights_16);
                    }
                }
                break;
        }

        KillPlants();
    }

    void KillStreetFoe() {
        if (!StreetKills.TryGetValue(game.Command, out TStreetKill? kill) || (game.World.Monsters[kill.target] != game.Session.Room)) {
            return;
        }

        if (kill.kind == TStreetKind.skDog) {
            FightDog();
        } else {
            Fight(TFoe.VeryEasyEnemy);
        }

        game.World.Monsters[kill.target] = 0;
        if (!kill.extra) {
            return;
        }
        if (kill.kind == TStreetKind.skDog) {
            Out.WriteLine(TxtKillDog_01);
        } else {
            game.Inventory.SuitGain();
        }
    }

    public void KillSweeper() {
        if ((game.Verb == CmdZabij && game.Rest == CmdZamiatacz) && (game.World.Monsters[TMonster.Sweeper] == game.Session.Room)) {
            Fight(TFoe.VeryEasyEnemy);
            game.World.Monsters[TMonster.Sweeper] = 0;
            Roll = game.Roll(100);
            if (Roll < 35) {
                Out.WriteLine(TxtKillSweeper_01);
                game.Inventory.AcquireItem(TBagItem.Tracksuit);
            }
        }
    }

    public void KillSickMan() {
        if ((game.Verb == CmdZabij && game.Rest == CmdZebrak) && (game.World.Monsters[TMonster.SickMan] == game.Session.Room)) {
            Fight(TFoe.VeryEasyEnemy);
            if (game.Player.Energy > 0) {
                Roll = game.Roll(10);
                Out.WriteLine(TxtKillSickMan_01 + Roll + TxtShared_23);
                game.Player.Money += Roll;
                game.World.Monsters[TMonster.SickMan] = 0;
            }
        }
    }

    public void StreetFights() {
        KillStreetFoe();
        KillSweeper();
        KillSickMan();
    }

    public void ConcertFights() {
        if (!ConcertKills.TryGetValue(game.Command, out TConcertKill? kill) || (game.Session.Room != game.World.Monsters[kill.monster])) {
            return;
        }

        Fight(kill.foe);
        if (EnemyEnergy < 1) {
            game.World.Monsters[kill.monster] = 0;
            if (kill.bonus == TConcertBonus.cbPipe) {
                Roll = game.Roll(100);
                if (Roll < 5) {
                    Out.WriteLine(TxtShared_08 + TxtConcertFights_01);
                    Out.WriteLine(TxtConcertFights_02);
                    game.Inventory.AcquireItem(TBagItem.Pipe);
                }
            }
        }
        if (kill.bonus == TConcertBonus.cbSuit) {
            game.Inventory.SuitGain();
        }
    }

    IntSlot SessionOverall() =>
        new(() =>
            game.Session.Overall, v => game.Session.Overall = v);

    IntSlot PlayerStrength() =>
        IntSlot.Into(game.Player.Stats, TStat.Strength);

    IntSlot PlayerStat(TStat stat) =>
        new(() => game.Player.Stats[stat], v => game.Player.Stats[stat] = v);

    public List<TDodgeBound> DodgeBounds => _dodgeBounds ??= [
        new(30, 12, 60), new(27, 13, 58), new(24, 14, 56),
        new(20, 15, 54), new(18, 16, 52), new(16, 17, 51),
        new(12, 18, 48), new(9, 19, 46), new(6, 20, 42),
        new(4, 21, 40), new(2, 22, 38), new(0, 23, 35),
    ];
    List<TDodgeBound>? _dodgeBounds;

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

    public List<TCompareFoeRule> BeastCompareRules => _beastCompareRules ??= [
        new(SessionOverall(), -Game.MaxInt - 1, 15, TxtCompareFoe_02),
        new(PlayerStrength(), 16, 18, TxtEnemy + TxtCompareFoe_03),
        new(PlayerStrength(), 19, Game.MaxInt, TxtCompareFoe_04),
    ];
    List<TCompareFoeRule>? _beastCompareRules;

    public List<TCompareFoeRule> BigBeastCompareRules => _bigBeastCompareRules ??= [
        new(SessionOverall(), -Game.MaxInt - 1, 15, TxtCompareFoe_05),
        new(PlayerStrength(), 16, 20, TxtCompareFoe_06),
        new(PlayerStrength(), 21, 22, TxtCompareFoe_07),
        new(PlayerStrength(), 23, Game.MaxInt, TxtCompareFoe_08),
    ];
    List<TCompareFoeRule>? _bigBeastCompareRules;

    public List<TCompareFoeRule> FighterCompareRules => _fighterCompareRules ??= [
        new(SessionOverall(), -Game.MaxInt - 1, 14, TxtCompareFoe_09),
        new(SessionOverall(), 15, 17, TxtCompareFoe_10),
        new(SessionOverall(), 18, 22, TxtCompareFoe_11),
        new(SessionOverall(), 23, 25, TxtCompareFoe_12),
        new(SessionOverall(), 26, Game.MaxInt, TxtCompareFoe_13),
    ];
    List<TCompareFoeRule>? _fighterCompareRules;

    public List<TCompareFoeRule> DrunkCompareRules => _drunkCompareRules ??= [
        new(SessionOverall(), -Game.MaxInt - 1, 23, TxtCompareFoe_14),
        new(SessionOverall(), 24, 25, TxtCompareFoe_15),
        new(SessionOverall(), 26, 28, TxtCompareFoe_16),
        new(SessionOverall(), 29, Game.MaxInt, TxtCompareFoe_17),
        new(SessionOverall(), 35, Game.MaxInt, TxtCompareFoe_18),
    ];
    List<TCompareFoeRule>? _drunkCompareRules;

    public List<TCompareFoeRule> MusicianCompareRules => _musicianCompareRules ??= [
        new(SessionOverall(), -Game.MaxInt - 1, 20, TxtCompareFoe_19),
        new(SessionOverall(), 23, 27, TxtCompareFoe_20),
        new(SessionOverall(), 28, Game.MaxInt, TxtCompareFoe_21),
    ];
    List<TCompareFoeRule>? _musicianCompareRules;

    public List<TCompareFoeRule> IvyCompareRules => _ivyCompareRules ??= [
        new(SessionOverall(), -Game.MaxInt - 1, 19, TxtCompareFoe_22),
        new(SessionOverall(), 20, 25, TxtCompareFoe_23),
        new(SessionOverall(), 26, 29, TxtCompareFoe_24),
        new(SessionOverall(), 30, Game.MaxInt, TxtCompareFoe_25),
        new(SessionOverall(), 36, Game.MaxInt, TxtCompareFoe_26),
    ];
    List<TCompareFoeRule>? _ivyCompareRules;

    public List<TCompareFoeRule> LiroyCompareRules => _liroyCompareRules ??= [
        new(SessionOverall(), -Game.MaxInt - 1, 27, TxtCompareFoe_27),
        new(SessionOverall(), 28, 32, TxtCompareFoe_28),
        new(SessionOverall(), 33, 43, TxtCompareFoe_29),
        new(SessionOverall(), 44, 47, TxtCompareFoe_30),
        new(SessionOverall(), 48, Game.MaxInt, TxtCompareFoe_31),
    ];
    List<TCompareFoeRule>? _liroyCompareRules;

    public List<TCompareFoeRule> PokrzywaCompareRules => _pokrzywaCompareRules ??= [
        new(SessionOverall(), -Game.MaxInt - 1, 39, TxtCompareFoe_32),
        new(SessionOverall(), 40, Game.MaxInt, TxtCompareFoe_33),
    ];
    List<TCompareFoeRule>? _pokrzywaCompareRules;

    public List<TFightExpStat> FightExpStats => _fightExpStats ??= [
        new() { playerStat = new IntSlot(() => game.Player.MaxEnergy, v => game.Player.MaxEnergy = v), enemyStat = new IntSlot(() => EnemyEnergy, v => EnemyEnergy = v), expFor = FightExpForEnergy },
        new() { playerStat = PlayerStat(TStat.Strength), enemyStat = new IntSlot(() => EnemyStrength, v => EnemyStrength = v), expFor = FightExpForStrength },
        new() { playerStat = PlayerStat(TStat.Dexterity), enemyStat = new IntSlot(() => EnemyDexterity, v => EnemyDexterity = v), expFor = FightExpForDexterity },
    ];
    List<TFightExpStat>? _fightExpStats;

    public Dictionary<string, TBeastKill> BeastKills => _beastKills ??= new() {
        [$"{CmdZabij}{CmdSlimak}"] = new() { beast = TMonster.Snail, foe = TFoe.WeakEnemy, unlessFled = true },
        [$"{CmdZabij}{CmdKornik}"] = new() { beast = TMonster.Woodworm, foe = TFoe.WeakEnemy, unlessFled = true },
        [$"{CmdZabij}{CmdMucha}"] = new() { beast = TMonster.Fly, foe = TFoe.WeakEnemy, unlessFled = true },
        [$"{CmdZabij}{CmdZuk}"] = new() { beast = TMonster.Beetle, foe = TFoe.WeakEnemy, unlessFled = true },
        [$"{CmdZabij}{CmdKaraluch}"] = new() { beast = TMonster.Cockroach, foe = TFoe.WeakEnemy, unlessFled = false },
        [$"{CmdZabij}{CmdMrowka}"] = new() { beast = TMonster.Ant, foe = TFoe.WeakEnemy, unlessFled = false },
        [$"{CmdZabij}{CmdPajak}"] = new() { beast = TMonster.Spider, foe = TFoe.WeakEnemy, unlessFled = false },
        [$"{CmdZabij}{CmdDzik}"] = new() { beast = TMonster.Boar, foe = TFoe.WeakishEnemy, unlessFled = false },
        [$"{CmdZabij}{CmdSzczur}"] = new() { beast = TMonster.Rat, foe = TFoe.WeakishEnemy, unlessFled = false },
        [$"{CmdZabij}{CmdLis}"] = new() { beast = TMonster.Fox, foe = TFoe.WeakishEnemy, unlessFled = false },
        [$"{CmdZabij}{CmdKuropatwa}"] = new() { beast = TMonster.Partridge, foe = TFoe.WeakishEnemy, unlessFled = false },
        [$"{CmdZabij}{CmdZajac}"] = new() { beast = TMonster.Hare, foe = TFoe.WeakishEnemy, unlessFled = false },
        [$"{CmdZabij}{CmdOrzel}"] = new() { beast = TMonster.Eagle, foe = TFoe.WeakishEnemy, unlessFled = false },
        [$"{CmdZabij}{CmdSarna}"] = new() { beast = TMonster.Deer, foe = TFoe.WeakishEnemy, unlessFled = false },
        [$"{CmdZabij}{CmdSlon}"] = new() { beast = TMonster.Elephant, foe = TFoe.MediumEnemy, unlessFled = false },
        [$"{CmdZabij}{CmdLew}"] = new() { beast = TMonster.Lion, foe = TFoe.MediumEnemy, unlessFled = false },
        [$"{CmdZabij}{CmdZyrafa}"] = new() { beast = TMonster.Giraffe, foe = TFoe.MediumEnemy, unlessFled = false },
        [$"{CmdZabij}{CmdWilk}"] = new() { beast = TMonster.Wolf, foe = TFoe.MediumEnemy, unlessFled = false },
        [$"{CmdZabij}{CmdWielblad}"] = new() { beast = TMonster.Camel, foe = TFoe.MediumEnemy, unlessFled = false },
        [$"{CmdZabij}{CmdStrus}"] = new() { beast = TMonster.Ostrich, foe = TFoe.MediumEnemy, unlessFled = false },
        [$"{CmdZabij}{CmdBoa}"] = new() { beast = TMonster.Boa, foe = TFoe.MediumEnemy, unlessFled = false },
        [$"{CmdZabij}{CmdBizon}"] = new() { beast = TMonster.Bison, foe = TFoe.MediumEnemy, unlessFled = false },
        [$"{CmdZabij}{CmdPantera}"] = new() { beast = TMonster.Panther, foe = TFoe.MediumEnemy, unlessFled = false },
    };
    Dictionary<string, TBeastKill>? _beastKills;

    public Dictionary<string, TArenaKill> ArenaKills => _arenaKills ??= new() {
        [$"{CmdZabij}{CmdGladiator}"] = new() { monster = TMonster.Gladiator, pills = true },
        [$"{CmdZabij}{CmdWojownik}"] = new() { monster = TMonster.Warrior, pills = true },
        [$"{CmdZabij}{CmdTrener}"] = new() { monster = TMonster.Tamer, pills = false },
    };
    Dictionary<string, TArenaKill>? _arenaKills;

    public Dictionary<string, TPlantKill> PlantKills => _plantKills ??= new() {
        [$"{CmdZabij}{CmdSzczaw}"] = new() { plant = TMonster.Sorrel, foe = TFoe.VeryEasyEnemy, ifAlive = false },
        [$"{CmdZabij}{CmdKoniczynka}"] = new() { plant = TMonster.Clover, foe = TFoe.VeryEasyEnemy, ifAlive = false },
        [$"{CmdZabij}{CmdMlecz}"] = new() { plant = TMonster.SowThistle, foe = TFoe.VeryEasyEnemy, ifAlive = false },
        [$"{CmdZabij}{CmdDmuchawiec}"] = new() { plant = TMonster.Dandelion, foe = TFoe.VeryEasyEnemy, ifAlive = false },
        [$"{CmdZabij}{CmdStokrotka}"] = new() { plant = TMonster.Daisy, foe = TFoe.VeryEasyEnemy, ifAlive = true },
        [$"{CmdZabij}{CmdRoza}"] = new() { plant = TMonster.Rose, foe = TFoe.EasyEnemy, ifAlive = true },
        [$"{CmdZabij}{CmdOset}"] = new() { plant = TMonster.Thistle, foe = TFoe.EasyEnemy, ifAlive = true },
        [$"{CmdZabij}{CmdMalina}"] = new() { plant = TMonster.Raspberry, foe = TFoe.EasyEnemy, ifAlive = true },
        [$"{CmdZabij}{CmdAgrest}"] = new() { plant = TMonster.Gooseberry, foe = TFoe.EasyEnemy, ifAlive = true },
        [$"{CmdZabij}{CmdJezyna}"] = new() { plant = TMonster.Blackberry, foe = TFoe.EasyEnemy, ifAlive = true },
    };
    Dictionary<string, TPlantKill>? _plantKills;

    public Dictionary<string, TConcertKill> ConcertKills => _concertKills ??= new() {
        [$"{CmdZabij}{CmdDziecko}"] = new() { monster = TMonster.Kid, foe = TFoe.WeakishEnemy, bonus = TConcertBonus.cbNone },
        [$"{CmdZabij}{CmdDziadek}"] = new() { monster = TMonster.OldMan, foe = TFoe.WeakishEnemy, bonus = TConcertBonus.cbPipe },
        [$"{CmdZabij}{CmdWariat}"] = new() { monster = TMonster.Lunatic, foe = TFoe.MediumEnemy, bonus = TConcertBonus.cbNone },
        [$"{CmdZabij}{CmdSluchacz}"] = new() { monster = TMonster.Listener, foe = TFoe.MediumEnemy, bonus = TConcertBonus.cbNone },
        [$"{CmdZabij}{CmdCzlowiek}"] = new() { monster = TMonster.Man, foe = TFoe.MediumEnemy, bonus = TConcertBonus.cbNone },
        [$"{CmdZabij}{CmdFan}"] = new() { monster = TMonster.Fan, foe = TFoe.MediumEnemy, bonus = TConcertBonus.cbNone },
        [$"{CmdZabij}{CmdReporter}"] = new() { monster = TMonster.Reporter, foe = TFoe.MediumEnemy, bonus = TConcertBonus.cbNone },
        [$"{CmdZabij}{CmdPolicjant}"] = new() { monster = TMonster.Policeman, foe = TFoe.HardEnemy, bonus = TConcertBonus.cbNone },
        [$"{CmdZabij}{CmdGoryl}"] = new() { monster = TMonster.Gorilla, foe = TFoe.HardEnemy, bonus = TConcertBonus.cbSuit },
        [$"{CmdZabij}{CmdOchroniarz}"] = new() { monster = TMonster.Bouncer, foe = TFoe.HardEnemy, bonus = TConcertBonus.cbSuit },
    };
    Dictionary<string, TConcertKill>? _concertKills;

    public Dictionary<string, TStreetKill> StreetKills => _streetKills ??= new() {
        [$"{CmdZabij}{CmdOwczarek}"] = new() { target = TMonster.Shepherd, extra = false, kind = TStreetKind.skDog },
        [$"{CmdZabij}{CmdPiesek}"] = new() { target = TMonster.Pooch, extra = false, kind = TStreetKind.skDog },
        [$"{CmdZabij}{CmdJamnik}"] = new() { target = TMonster.Dachshund, extra = false, kind = TStreetKind.skDog },
        [$"{CmdZabij}{CmdSpaniel}"] = new() { target = TMonster.Spaniel, extra = true, kind = TStreetKind.skDog },
        [$"{CmdZabij}{CmdPudel}"] = new() { target = TMonster.Poodle, extra = false, kind = TStreetKind.skDog },
        [$"{CmdZabij}{CmdTaksowkarz}"] = new() { target = TMonster.Cabman, extra = true, kind = TStreetKind.skFolk },
        [$"{CmdZabij}{CmdSprzedawca}"] = new() { target = TMonster.Vendor, extra = true, kind = TStreetKind.skFolk },
        [$"{CmdZabij}{CmdPijak}"] = new() { target = TMonster.Drunkard, extra = false, kind = TStreetKind.skFolk },
    };
    Dictionary<string, TStreetKill>? _streetKills;
}
