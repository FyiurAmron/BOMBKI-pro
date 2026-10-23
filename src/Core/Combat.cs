using static Texts;

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
                        game.AcquireItem(TBagItem.Doughnut);
                    }
                }
            } else if (HeartChance > 0) {
                Roll = game.Roll(20);
                if (Roll < HeartChance) {
                    Out.WriteLine(TxtShared_08 + TxtCompositeFight_03);
                    game.AcquireItem(TBagItem.Heart);
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

    public void KillBeast(string cmd, IntSlot beast, TFoe foe) {
        if ((game.Matches(cmd)) && (game.Session.Room == beast.Get())) {
            Fight(foe);
            beast.Set(0);
        }
    }

    public void KillBeastUnlessFled(string cmd, IntSlot beast, TFoe foe) {
        if ((game.Matches(cmd)) && (game.Session.Room == beast.Get())) {
            Fight(foe);
            if (!Fled) {
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

    public void CompareFoe() {
        Roll = game.Roll(100);
        Out.WriteLine(TxtCompareFoe_01);
        game.Command = Crt.ReadLn();
        game.Session.Overall = (game.Player.Level + game.Player.Stats[TStat.Strength] + DexterityBonus(game.Player.Stats[TStat.Dexterity]) + ParryBonus(game.Skills.Values[TSkill.Parry]) + KickBonus(game.Skills.Values[TSkill.Kick]));
        switch (game.Verb) {
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
                game.Player.ChangeMana(5);
                break;
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
            if (game.Player.MaxEnergy > 75) {
                FightExperience -= 2;
            }

            if (game.Player.MaxEnergy > 115) {
                FightExperience -= 3;
            }

            if (game.Skills.Values[TSkill.Parry] > 50) {
                FightExperience -= 2;
            }

            if (game.Skills.Values[TSkill.Parry] > 75) {
                FightExperience -= 2;
            }

            if (game.Skills.Values[TSkill.Parry] > 95) {
                FightExperience -= 1;
            }

            if (game.Skills.Values[TSkill.Kick] > 50) {
                FightExperience -= 5;
            }

            if (game.Skills.Values[TSkill.Kick] > 95) {
                FightExperience -= 2;
            }

            if (FightExperience < 0) {
                FightExperience = 0;
            }

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
                    game.AcquireItem(TBagItem.Bigos);
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
            game.Session.Room = DeadRoom;
        }
        FightExperience = 0;
    }

    public void ArenaKill(string cmd, TMonster monster, bool pills) {
        if ((game.Matches(cmd)) && (game.Session.Room == game.World.Monsters[monster])) {
            Fight(TFoe.HardEnemy);
            game.World.Monsters[monster] = 0;
            if (pills) {
                game.PillGain();
            } else {
                game.SuitGain();
            }
        }
    }

    public void StageFights() {
        switch (game.Verb) {
            case CmdZabij:
                switch (game.Rest) {
                    case CmdPerkusista:
                        if (game.World.Monsters[TMonster.Drummer] == game.Session.Room) {
                            Fight(TFoe.VeryEasyEnemy);
                            game.World.Monsters[TMonster.Drummer] = 0;
                            game.CassetteGain();
                        }
                        break;
                    case CmdGitarzysta:
                        if (game.World.Monsters[TMonster.Guitarist] == game.Session.Room) {
                            Fight(TFoe.VeryEasyEnemy);
                            game.World.Monsters[TMonster.Guitarist] = 0;
                            game.CassetteGain();
                        }
                        break;
                    case CmdLiroy:
                        if (game.World.Monsters[TMonster.Liroy] == game.Session.Room) {
                            Fight(TFoe.NotEasyEnemy);
                            if ((game.Session.Room != DeadRoom) && (game.Player.Energy > 0) && !Fled) {
                                Out.WriteLine(TxtStageFights_01);
                                game.Player.Money += 30;
                                game.CassetteGain();
                                game.World.LiroyKilled = true;
                                game.World.Monsters[TMonster.Liroy] = 0;
                            }
                        }
                        break;
                    case CmdOrganista:
                        if (game.World.Monsters[TMonster.Keyboardist] == game.Session.Room) {
                            Fight(TFoe.VeryEasyEnemy);
                            game.World.Monsters[TMonster.Keyboardist] = 0;
                            game.CassetteGain();
                        }
                        break;
                }
                break;
        }
    }

    public void KillPlant(string cmd, IntSlot Plant) {
        if ((game.Matches(cmd)) && (Plant.Get() == game.Session.Room)) {
            Fight(TFoe.VeryEasyEnemy);
            Plant.Set(0);
        }
    }

    public void KillPlantIfAlive(string cmd, IntSlot Plant) {
        if ((game.Matches(cmd)) && (Plant.Get() == game.Session.Room)) {
            Fight(TFoe.VeryEasyEnemy);
            if (game.Player.Energy > 0) {
                Plant.Set(0);
            }
        }
    }

    public void KillPlantEasyIfAlive(string cmd, IntSlot Plant) {
        if ((game.Matches(cmd)) && (Plant.Get() == game.Session.Room)) {
            Fight(TFoe.EasyEnemy);
            if (game.Player.Energy > 0) {
                Plant.Set(0);
            }
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
                    game.WriteLines([
                        TxtPlantFights_01,
                        TxtPlantFights_02,
                        TxtPlantFights_03,
                        TxtPlantFights_04,
                    ]);
                }

                break;
            case CmdKup:
                switch (game.Rest) {
                    case CmdDokument:
                        if (game.Player.Money > 399) {
                            Out.WriteLine(TxtPlantFights_05);
                            game.AcquirePermit();
                            game.Player.Money -= 400;
                        }
                        break;
                    case CmdPlecak:
                        if ((game.World.Monsters[TMonster.Duncan] == game.Session.Room) && (game.Player.Money > 4799)) {
                            Out.WriteLine(TxtPlantFights_06);
                            game.Player.Money -= 4800;
                            game.AcquireItem(TBagItem.Backpack);
                        }
                        break;
                }
                break;
            case CmdRozmawiaj:
                if ((game.Rest == CmdDuncan) && (game.World.Monsters[TMonster.Duncan] == game.Session.Room)) {
                    if (game.World.DuncanQuest == 0) {
                        game.WriteLines([
                            TxtPlantFights_07,
                            TxtPlantFights_08,
                            TxtPlantFights_09,
                            TxtPlantFights_10,
                            TxtPlantFights_11,
                            TxtPlantFights_12,
                        ]);
                        game.World.DuncanQuest = 75;
                    }
                    if (game.World.DuncanQuest != 0) {
                        Out.WriteLine(TxtPlantFights_13);
                    }

                    if (game.World.DuncanQuest == -125) {
                        Out.WriteLine(TxtPlantFights_14);
                        Out.WriteLine(TxtPlantFights_15);
                        game.World.DuncanQuest = 0;
                        game.Player.Experience += 125;
                        Out.WriteLine(TxtPlantFights_16);
                    }
                }
                break;
        }

        for (int i = 0; i < PlantKills.Count; i++) {
            KillPlant(PlantKills[i].cmd, IntSlot.Into(game.World.Monsters, PlantKills[i].plant));
        }

        KillPlantIfAlive(CmdZabij + CmdStokrotka, IntSlot.Into(game.World.Monsters, TMonster.Daisy));

        for (int i = 0; i < EasyPlantKills.Count; i++) {
            KillPlantEasyIfAlive(EasyPlantKills[i].cmd, IntSlot.Into(game.World.Monsters, EasyPlantKills[i].plant));
        }
    }

    public void KillStreetFoe(string cmd, IntSlot target, bool extra, TStreetKind kind) {
        if ((game.Matches(cmd)) && (target.Get() == game.Session.Room)) {
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
                        game.WriteLines([TxtKillDog_01, TxtKillDog_02, TxtKillDog_03]);
                        break;
                    case TStreetKind.skFolk:
                        game.SuitGain();
                        break;
                }
            }
        }
    }

    public void KillSweeper() {
        if ((game.Verb == CmdZabij && game.Rest == CmdZamiatacz) && (game.World.Monsters[TMonster.Sweeper] == game.Session.Room)) {
            Fight(TFoe.VeryEasyEnemy);
            game.World.Monsters[TMonster.Sweeper] = 0;
            Roll = game.Roll(100);
            if (Roll < 35) {
                Out.WriteLine(TxtKillSweeper_01);
                game.AcquireItem(TBagItem.Tracksuit);
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
        for (int ki = 0; ki < StreetKills.Count; ki++) {
            TStreetKill k = StreetKills[ki];
            KillStreetFoe(k.cmd, IntSlot.Into(game.World.Monsters, k.target), k.extra, k.kind);
        }
        KillSweeper();
        KillSickMan();
    }

    public void ConcertKill(string cmd, TMonster monster, TFoe foe, TConcertBonus bonus) {
        if ((game.Matches(cmd)) && (game.Session.Room == game.World.Monsters[monster])) {
            Fight(foe);
            if (EnemyEnergy < 1) {
                game.World.Monsters[monster] = 0;
                if (bonus == TConcertBonus.cbPipe) {
                    Roll = game.Roll(100);
                    if (Roll < 5) {
                        Out.WriteLine(TxtShared_08 + TxtConcertFights_01);
                        Out.WriteLine(TxtConcertFights_02);
                        game.AcquireItem(TBagItem.Pipe);
                    }
                }
            }
            if (bonus == TConcertBonus.cbSuit) {
                game.SuitGain();
            }
        }
    }

    public void ConcertFights() {
        for (int ci = 0; ci < ConcertKills.Count; ci++) {
            TConcertKill c = ConcertKills[ci];
            ConcertKill(c.cmd, c.monster, c.foe, c.bonus);
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
}
