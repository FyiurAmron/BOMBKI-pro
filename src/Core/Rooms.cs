using static TMonster;
using static TFoe;
using static TStreetKind;
using static Texts;

public class Rooms {
    readonly Game game;

    public Rooms(Game game) {
        this.game = game;
    }

    public const int RoomStart = 0;
    public const int RoomSalon = 1;
    public const int RoomTraining = 2;
    public const int RoomMeditation = 3;
    public const int RoomSchoolHall = 4;
    public const int RoomLost = 5;
    public const int RoomSchoolSide1 = 6;
    public const int RoomSchoolSide2 = 7;
    public const int RoomSchoolSide3 = 8;
    public const int RoomSchoolLanding = 9;
    public const int RoomSchoolSide4 = 10;
    public const int RoomCageCorridor = 11;
    public const int RoomCageWeakling = 12;
    public const int RoomCageStrong = 13;
    public const int RoomCageAgile = 14;
    public const int RoomCageTough = 15;
    public const int RoomCageHuge = 16;
    public const int RoomCageReward = 17;
    public const int RoomSchoolGate = 19;
    public const int RoomCityCentre = 20;
    public const int RoomShopStreet = 21;
    public const int RoomShopStreet2 = 22;
    public const int RoomBakery = 23;
    public const int RoomArmoury = 24;
    public const int RoomGeneralStore = 25;
    public const int RoomMagicShop = 26;
    public const int RoomLongStreet = 30;
    public const int RoomLongStreet2 = 31;
    public const int RoomArenaEntrance = 32;
    public const int RoomArena33 = 33;
    public const int RoomArena34 = 34;
    public const int RoomArena35 = 35;
    public const int RoomArena36 = 36;
    public const int RoomArena37 = 37;
    public const int RoomArena38 = 38;
    public const int RoomArena39 = 39;
    public const int RoomArena40 = 40;
    public const int RoomArena41 = 41;
    public const int RoomArena42 = 42;
    public const int RoomArena43 = 43;
    public const int RoomArena44 = 44;
    public const int RoomArena45 = 45;
    public const int RoomArena46 = 46;
    public const int RoomArena47 = 47;
    public const int RoomArena48 = 48;
    public const int RoomArena49 = 49;
    public const int RoomArena50 = 50;
    public const int RoomArena51 = 51;
    public const int RoomArena52 = 52;
    public const int RoomArena53 = 53;
    public const int RoomArena54 = 54;
    public const int RoomArena55 = 55;
    public const int RoomArena56 = 56;
    public const int RoomArena57 = 57;
    public const int RoomArenaGate = 58;
    public const int RoomEntertainmentCave = 60;
    public const int RoomCrowd1 = 61;
    public const int RoomCrowd2 = 62;
    public const int RoomCrowd3 = 63;
    public const int RoomCrowd4 = 64;
    public const int RoomCrowd5 = 65;
    public const int RoomCrowd6 = 66;
    public const int RoomPub = 67;
    public const int RoomStageEntrance = 68;
    public const int RoomBackstage = 69;
    public const int RoomStage1 = 70;
    public const int RoomStage2 = 71;
    public const int RoomStage3 = 72;
    public const int RoomConcertGate = 73;
    public const int RoomDarkStreet = 75;
    public const int RoomBar = 76;
    public const int RoomIvy1 = 77;
    public const int RoomIvy2 = 78;
    public const int RoomIvy3 = 79;
    public const int RoomIvy4 = 80;
    public const int RoomIvy5 = 81;
    public const int RoomIvy6 = 82;
    public const int RoomClearing = 83;
    public const int RoomCavePath = 84;
    public const int RoomLivingDoor = 85;
    public const int RoomHut = 86;
    public const int RoomCaveMouth = 87;
    public const int RoomNettleThrone = 88;
    public const int RoomIvyGate = 89;
    public const int RoomRoadsGate = 99;
    public const int RoomQuestMaster = 100;
    public const int RoomRoad1 = 101;
    public const int RoomRoad2 = 102;
    public const int RoomRoad3 = 103;
    public const int RoomForest = 104;
    public const int RoomForest1 = 105;
    public const int RoomForest2 = 106;
    public const int RoomForest3 = 107;
    public const int RoomForest4 = 108;
    public const int RoomForest5 = 109;
    public const int RoomFutureRoad = 110;
    public const int RoomDead = 10000;

    public const int StaruchPrice = 200;

    public class TRoomPoster {
        public string cmd = "";
        public List<string> lines = [];
        public TRoomPoster(string cmd, List<string> lines) {
            this.cmd = cmd;
            this.lines = lines;
        }
    }

    public readonly record struct TCoordinate(int X, int Y, int Z);

    public class TRoomExit {
        public string cmd = "";
        public bool enabled = true;
        public Predicate<bool>? predicate;
        public Func<bool>? state;
        public string? blocked;
        public bool IsOpen() =>
            predicate?.Invoke(state?.Invoke() ?? enabled) ?? enabled;
    }

    public enum TRoomPhase {
        Enter,
        Command,
    }

    public class TRoom {
        public int number;
        public TCoordinate position;
        public string name = "";
        public List<string> intro = [];
        public List<TRoomExit> exits = [];
        public List<TMovement> moves = [];
        public List<TRoomPoster> posters = [];
        public bool maze = false;
        public Action<TRoomPhase>? turn = null;
    }

    void StreetTurn(TRoomPhase phase) {
        if (phase == TRoomPhase.Enter) {
            game.DescribeStreet();
        } else {
            game.StreetFights();
        }
    }

    void PlantTurn(TRoomPhase phase) {
        if (phase == TRoomPhase.Enter) {
            game.DescribePlants();
        } else {
            game.PlantFights();
        }
    }

    void CrowdTurn(TRoomPhase phase) {
        if (phase == TRoomPhase.Enter) {
            game.DescribeCrowd();
        } else {
            game.ConcertFights();
        }
    }

    void StageTurn(TRoomPhase phase) {
        if (phase == TRoomPhase.Enter) {
            game.DescribeStage();
        } else {
            game.StageFights();
        }
    }

    void ArenaTurn(int room, TRoomPhase phase) {
        if (phase == TRoomPhase.Enter) {
            game.DescribeBeasts();
            return;
        }
        switch (game.Verb) {
            case CmdZabij:
                switch (game.Rest) {
                    case CmdBakteria:
                        if (room == game.World.Monsters[TMonster.Bacterium]) {
                            game.Fight(TFoe.WeakEnemy);
                            game.World.Monsters[TMonster.Bacterium] = 0;
                            Out.WriteLine(TxtArenaMaze_02);
                        }
                        break;
                    case CmdWilczur:
                        if (room == game.World.Monsters[TMonster.Mongrel]) {
                            Out.WriteLine(TxtArenaMaze_03);
                            game.Fight(TFoe.WeakishEnemy);
                            game.World.Monsters[TMonster.Mongrel] = 0;
                        }
                        break;
                }
                break;
        }
        for (int bi = 0; bi < game.Combat.BeastKills.Count; bi++) {
            TBeastKill b = game.Combat.BeastKills[bi];
            if (b.unlessFled) {
                game.KillBeastUnlessFled(b.cmd, IntSlot.Into(game.World.Monsters, b.beast), b.foe);
            } else {
                game.KillBeast(b.cmd, IntSlot.Into(game.World.Monsters, b.beast), b.foe);
            }
        }
        for (int ai = 0; ai < game.Combat.ArenaKills.Count; ai++) {
            TArenaKill a = game.Combat.ArenaKills[ai];
            game.ArenaKill(a.cmd, a.monster, a.pills);
        }
        switch (game.Verb) {
            case CmdPolodnie:
                game.MazeMove(TMazeDir.mdPolodnie);
                break;
            case CmdPolnoc:
                game.MazeMove(TMazeDir.mdPolnoc);
                break;
            case CmdWschod:
                game.MazeMove(TMazeDir.mdWschod);
                break;
            case CmdZachod:
                game.MazeMove(TMazeDir.mdZachod);
                break;
        }
    }

    public void CageMonsterFight(IntSlot monster, int room, TCageStats stats) {
        if ((game.Verb == CmdZabij && game.Rest == CmdPotwor) && (monster.Get() == room)) {
            game.Combat.EnemyEnergy = stats.energy;
            game.Combat.EnemyDexterity = stats.dexterity;
            game.Combat.EnemyStrength = stats.strength;
            game.DoFight();
            if (!game.Combat.Fled) {
                monster.Set(0);
                game.Combat.Loot = game.Roll(stats.lootRand);
                Out.WriteLine(TxtShared_08 + game.Combat.Loot + TxtShared_09);
                game.Player.Money += game.Combat.Loot;
                game.TryCageLoot(stats.lootThreshold);
            } else {
                game.Combat.Fled = false;
            }
        }
    }

    public void DescribeCage(TMonster monster, int room, string aliveLine, string deadLine) {
        Out.WriteLine(TxtShared_06);
        if (game.World.Monsters[monster] == room) {
            Out.WriteLine(aliveLine);
        } else if (game.World.Monsters[monster] == 0) {
            Out.WriteLine(deadLine);
        }
    }

    TRoom SideRoom(int number, TCoordinate position, string direction, string name) =>
        new() {
            number = number,
            position = position,
            name = name,
            intro = [TxtYouAreIn + TxtSimpleRoom_01],
            exits = [Exit(direction, true)],
        };

    TRoom ArenaRoom(int number, TCoordinate position) =>
        new() {
            number = number,
            position = position,
            name = $"{TxtPlaceArena} {number - RoomArena33 + 1}",
            intro = [TxtArenaMaze_01],
            maze = true,
            turn = phase => ArenaTurn(number, phase),
        };

    public List<TRoom> RoomDefs => _roomDefs ??= [
        new() {
            number = RoomStart,
            position = new(0, -2, 0),
            name = TxtPlaceTamGdzieZaczynaszGre,
            intro = [TxtRoom0_01],
            exits = [Exit(CmdPolnoc, true)],
        },
        new() {
            number = RoomSalon,
            position = new(0, -1, 0),
            name = TxtPlaceSalon,
            intro = [TxtYouAreIn + TxtRoom1_01, TxtRoom1_02, TxtShared_04, TxtRoom1_03],
            exits = [Exit(CmdPolodnie, true),
                     Exit(CmdWschod, true),
                     Exit(CmdDol, true),
                     Exit(CmdZachod, true),
                     Exit(CmdPolnoc, true), ],
            posters = [new(CmdPatrz + CmdPlakat, [TxtShared_05, TxtRoom1_09])],
        },
        new() {
            number = RoomTraining,
            position = new(1, -1, 0),
            name = TxtPlacePokojTreningowy1,
            intro = [TxtYouAreIn + TxtRoom2_01, TxtRoom2_02, TxtShared_04],
            exits = [Exit(CmdZachod, true)],
            posters = [new(CmdPatrz + CmdPlakat, [TxtShared_05, TxtRoom2_04])],
            turn = phase => {
                if (phase == TRoomPhase.Command) {
                    game.HandleTrain();
                }
            },
        },
        new() {
            number = RoomMeditation,
            position = new(0, -1, -1),
            name = TxtPlacePoddziemnyPokojTreningowy2,
            intro = [TxtRoom3_01],
            exits = [Exit(CmdGora, true)],
            turn = phase => {
                if (phase == TRoomPhase.Command) {
                    game.Training();
                }
            },
        },
        new() {
            number = RoomSchoolHall,
            position = new(-2, -1, 0),
            name = TxtPlaceSzkolaZycia,
            intro = [TxtYouAreIn + TxtRoom4_01, TxtRoom4_02],
            exits = [Exit(CmdWschod, true),
                     Exit(CmdZachod, true), ],
            posters = [new(CmdPatrz + CmdAfisz, [TxtRoom4_05])],
        },
        new() {
            number = RoomLost,
            position = new(-3, -1, 0),
            name = TxtPlacePokojOrientacyjny,
            intro = [TxtRoom5_01],
            exits = [Exit(CmdWschod, true),
                     Exit(CmdZachod, true),
                     Exit(CmdPolnoc, true),
                     Exit(CmdPolodnie, true),
                     Exit(CmdGora, true),
                     Exit(CmdDol, true), ],
            posters = [new(CmdPatrz + CmdPlakat, [TxtRoom5_10])],
        },
        SideRoom(RoomSchoolSide1, new(-4, -1, 0), CmdWschod, $"{TxtPlacePokoj} 1"),
        SideRoom(RoomSchoolSide3, new(-3, -2, 0), CmdPolnoc, $"{TxtPlacePokoj} 3"),
        SideRoom(RoomSchoolSide2, new(-3, 0, 0), CmdPolodnie, $"{TxtPlacePokoj} 2"),
        SideRoom(RoomSchoolSide4, new(-3, -1, -1), CmdGora, $"{TxtPlacePokoj} 5"),
        new() {
            number = RoomSchoolLanding,
            position = new(-3, -1, 1),
            name = $"{TxtPlacePokoj} 4",
            intro = [TxtYouAreIn + TxtRoom9_01, TxtRoom9_02],
            exits = [Exit(CmdDol, true),
                     Exit(CmdGora, true),
                     Exit(CmdWschod, true), ],
        },
        new() {
            number = RoomCageCorridor,
            position = new(-3, -1, 2),
            name = TxtPlaceWejsciaDoKlatek,
            intro = [TxtYouAreIn + TxtRoom11_01, TxtRoom11_03, TxtShared_04],
            exits = [Exit(CmdWschod, true),
                     Exit(CmdZachod, true),
                     Exit(CmdPolnoc, true),
                     Exit(CmdPolodnie, true),
                     Exit(CmdGora, true),
                     Exit(CmdDol, true), ],
            posters = [new(CmdPatrz + CmdPlakat, [TxtShared_13, TxtRoom11_10])],
        },
        new() {
            number = RoomCageWeakling,
            position = new(-2, -1, 2),
            name = TxtPlaceKlatkaCieniasa,
            exits = [Exit(CmdZachod, true)],
            turn = phase => {
                if (phase == TRoomPhase.Enter) {
                    DescribeCage(CageWeakling, RoomCageWeakling, TxtRoom12_01, TxtShared_07);
                } else {
                    CageMonsterFight(IntSlot.Into(game.World.Monsters, CageWeakling), RoomCageWeakling, game.Combat.CageWeaklingStats);
                }
            },
        },
        new() {
            number = RoomCageStrong,
            position = new(-4, -1, 2),
            name = TxtPlaceKlatkaSilnego,
            exits = [Exit(CmdWschod, true)],
            turn = phase => {
                if (phase == TRoomPhase.Enter) {
                    DescribeCage(CageStrong, RoomCageStrong, TxtRoom13_01, TxtShared_07);
                } else {
                    CageMonsterFight(IntSlot.Into(game.World.Monsters, CageStrong), RoomCageStrong, game.Combat.CageStrongStats);
                }
            },
        },
        new() {
            number = RoomCageAgile,
            position = new(-3, 0, 2),
            name = TxtPlaceKlatkaZrecznego,
            exits = [Exit(CmdPolodnie, true)],
            turn = phase => {
                if (phase == TRoomPhase.Enter) {
                    DescribeCage(CageAgile, RoomCageAgile, TxtRoom14_01, TxtRoom14_02);
                } else {
                    CageMonsterFight(IntSlot.Into(game.World.Monsters, CageAgile), RoomCageAgile, game.Combat.CageAgileStats);
                }
            },
        },
        new() {
            number = RoomCageTough,
            position = new(-3, -2, 2),
            name = TxtPlaceKlatkaOdpornego,
            exits = [Exit(CmdPolnoc, true)],
            turn = phase => {
                if (phase == TRoomPhase.Enter) {
                    DescribeCage(CageTough, RoomCageTough, TxtRoom15_01, TxtShared_07);
                } else {
                    CageMonsterFight(IntSlot.Into(game.World.Monsters, CageTough), RoomCageTough, game.Combat.CageToughStats);
                }
            },
        },
        new() {
            number = RoomCageHuge,
            position = new(-3, -1, 3),
            name = TxtPlaceKlatkaSilnegoOdpornegoIZrecznego,
            exits = [Exit(CmdDol, true), ],
            turn = phase => {
                if (phase == TRoomPhase.Enter) {
                    DescribeCage(CageHuge, RoomCageHuge, TxtRoom16_01, TxtShared_07);
                }
                if (game.World.Monsters[CageHuge] == RoomCageHuge) {
                    game.Combat.EnemyEnergy = game.Combat.CageHugeStats.energy;
                    game.Combat.EnemyDexterity = game.Combat.CageHugeStats.dexterity;
                    game.Combat.EnemyStrength = game.Combat.CageHugeStats.strength;
                    game.DoFight();
                    if (game.Combat.Fled) {
                        game.Combat.Fled = false;
                        game.MoveTo(RoomCageCorridor);
                    } else {
                        game.World.Monsters[CageHuge] = 0;
                        game.Combat.Loot = game.Roll(game.Combat.CageHugeStats.lootRand);
                        Out.WriteLine(TxtShared_08 + game.Combat.Loot + TxtShared_09);
                        game.Player.Money += game.Combat.Loot;
                        game.TryCageLoot(game.Combat.CageHugeStats.lootThreshold);
                    }
                }
            },
        },
        new() {
            number = RoomCageReward,
            position = new(-2, -1, 1),
            name = TxtPlaceJedynaDrogaNieProwadzacaDoKlatek,
            intro = [TxtYouAreIn + TxtRoom17_01],
            exits = [Exit(CmdZachod, true)],
            turn = phase => {
                if (phase == TRoomPhase.Enter) {
                    return;
                }
                if (game.Verb == CmdPatrz && game.Rest == CmdPlakat) {
                    game.WriteLines([TxtShared_13, TxtRoom17_03]);
                    if ((game.World.Monsters[CageWeakling] == 0) && (game.World.Monsters[CageAgile] == 0) &&
                    (game.World.Monsters[CageStrong] == 0) && (game.World.Monsters[CageTough] == 0) &&
                    (game.World.Monsters[CageHuge] == 0) && (game.Bag.Items[TBagItem.Diploma] == 0) &&
                    !game.Bag.HasGroundItem(TBagItem.Diploma)) {
                        game.Bag.AddGroundItem(TBagItem.Diploma, RoomCageReward);
                    }
                }
            },
        },
        new() {
            number = RoomSchoolGate,
            position = new(-1, -1, 0),
            name = TxtPlacePrzejscieDoSzkoly,
            intro = [TxtRoom19_01],
            exits = [Exit(CmdWschod, true),
                     Exit(CmdZachod, true), ],
        },
        new() {
            number = RoomCityCentre,
            position = new(0, 0, 0),
            name = TxtPlaceCentrumMiasta,
            intro = [TxtYouAreIn + TxtRoom20_01],
            exits = [Exit(CmdPolodnie, true),
                     Exit(CmdPolnoc, true),
                     Exit(CmdZachod, true),
                     Exit(CmdWschod, true), ],
            posters = [new(CmdPatrz + CmdPlakat, [TxtRoom20_02])],
            turn = StreetTurn,
        },
        new() {
            number = RoomShopStreet,
            position = new(0, 1, 0),
            name = TxtPlaceUlSklepikowa,
            intro = [TxtRoom21_01],
            exits = [Exit(CmdPolodnie, true),
                     Exit(CmdPolnoc, true),
                     Exit(CmdWschod, true),
                     Exit(CmdZachod, true), ],
            turn = StreetTurn,
        },
        new() {
            number = RoomShopStreet2,
            position = new(0, 2, 0),
            name = TxtPlaceUlSklepikowa2,
            intro = [TxtRoom22_01],
            exits = [Exit(CmdPolodnie, true),
                     Exit(CmdWschod, true),
                     Exit(CmdZachod, true),
                     Exit(CmdPolnoc, true), ],
            turn = StreetTurn,
        },
        new() {
            number = RoomBakery,
            position = new(1, 1, 0),
            name = TxtPlacePiekarnia,
            intro = [TxtYouAreIn + TxtRoom23_01, TxtRoom23_02],
            exits = [Exit(CmdZachod, true)],
            posters = [new(CmdPatrz + CmdPlakat, [TxtRoom23_04])],
            turn = phase => {
                if (phase == TRoomPhase.Enter) {
                    game.DescribeStreet();
                } else {
                    game.BakeryShop();
                }
            },
        },
        new() {
            number = RoomArmoury,
            position = new(-1, 1, 0),
            name = TxtPlaceZbrojownia,
            intro = [TxtYouAreIn + TxtRoom24_01, TxtRoom24_02],
            exits = [Exit(CmdWschod, true)],
            posters = [new(CmdPatrz + CmdPlakat, [TxtRoom24_04])],
            turn = phase => {
                if (phase == TRoomPhase.Enter) {
                    game.DescribeStreet();
                } else {
                    game.ArmouryShop();
                    game.StreetFights();
                }
            },
        },
        new() {
            number = RoomGeneralStore,
            position = new(1, 2, 0),
            name = TxtPlaceSklepWielobranzowy,
            intro = [TxtRoom25_01],
            exits = [Exit(CmdZachod, true)],
            posters = [new(CmdPatrz + CmdWywieszka, [TxtRoom25_03]),
                       new(CmdPatrz + CmdPoster, [TxtGeneralStoreShop_06]), ],
            turn = phase => {
                if (phase == TRoomPhase.Enter) {
                    game.DescribeStreet();
                } else {
                    game.StreetFights();
                    game.GeneralStoreShop();
                }
            },
        },
        new() {
            number = RoomMagicShop,
            position = new(-1, 2, 0),
            name = TxtPlaceSklepMagiczny,
            intro = [TxtRoom26_01],
            exits = [Exit(CmdWschod, true)],
            posters = [new(CmdPatrz + CmdPlakat, [TxtRoom26_02])],
            turn = phase => {
                if (phase == TRoomPhase.Enter) {
                    game.DescribeStreet();
                } else {
                    game.MagicShop();
                    game.StreetFights();
                }
            },
        },
        new() {
            number = RoomLongStreet,
            position = new(-1, 0, 0),
            name = TxtPlaceUlDluga,
            intro = [TxtRoom30_01],
            exits = [Exit(CmdWschod, true),
                     Exit(CmdZachod, true),
                     Exit(CmdGora, true), ],
        },
        new() {
            number = RoomLongStreet2,
            position = new(-2, 0, 0),
            name = TxtPlaceUlDluga2,
            intro = [TxtRoom31_01],
            exits = [Exit(CmdWschod, true),
                     Exit(CmdPolnoc, true), ],
        },
        new() {
            number = RoomArenaEntrance,
            position = new(-1, 1, 1),
            name = TxtPlaceWejscieNaArene,
            intro = [TxtRoom32_01],
            exits = [Exit(CmdPolodnie, true),
                     Exit(CmdPolnoc, true), ],
            posters = [new(CmdPatrz + CmdPlakat, [TxtRoom32_03])],
        },
        ArenaRoom(RoomArena33, new(-1, 2, 1)),
        ArenaRoom(RoomArena34, new(-1, 3, 1)),
        ArenaRoom(RoomArena35, new(0, 2, 1)),
        ArenaRoom(RoomArena36, new(-2, 2, 1)),
        ArenaRoom(RoomArena37, new(-1, 4, 1)),
        ArenaRoom(RoomArena38, new(0, 3, 1)),
        ArenaRoom(RoomArena39, new(-2, 3, 1)),
        ArenaRoom(RoomArena40, new(1, 2, 1)),
        ArenaRoom(RoomArena41, new(-3, 2, 1)),
        ArenaRoom(RoomArena42, new(-1, 5, 1)),
        ArenaRoom(RoomArena43, new(0, 4, 1)),
        ArenaRoom(RoomArena44, new(-2, 4, 1)),
        ArenaRoom(RoomArena45, new(1, 3, 1)),
        ArenaRoom(RoomArena46, new(-3, 3, 1)),
        ArenaRoom(RoomArena47, new(-1, 6, 1)),
        ArenaRoom(RoomArena48, new(0, 5, 1)),
        ArenaRoom(RoomArena49, new(-2, 5, 1)),
        ArenaRoom(RoomArena50, new(1, 4, 1)),
        ArenaRoom(RoomArena51, new(-3, 4, 1)),
        ArenaRoom(RoomArena52, new(0, 6, 1)),
        ArenaRoom(RoomArena53, new(-2, 6, 1)),
        ArenaRoom(RoomArena54, new(1, 5, 1)),
        ArenaRoom(RoomArena55, new(-3, 5, 1)),
        ArenaRoom(RoomArena56, new(1, 6, 1)),
        ArenaRoom(RoomArena57, new(-3, 6, 1)),
        new() {
            number = RoomArenaGate,
            position = new(-1, 0, 1),
            name = TxtPlaceDrogaNaArene,
            intro = [TxtRoom58_01],
            exits = [Exit(CmdDol, true),
                     Exit(CmdPolnoc, true), ],
        },
        new() {
            number = RoomEntertainmentCave,
            position = new(-2, 1, -1),
            name = TxtPlaceJaskiniaRozrywek,
            intro = [TxtYouAreIn + TxtRoom60_01, TxtRoom60_02],
            exits = [Exit(CmdGora, true),
                     Exit(CmdWschod, true), ],
        },
        new() {
            number = RoomCrowd1,
            position = new(-1, 1, -1),
            name = $"{TxtPlaceTlum} 1",
            intro = [TxtConcertRooms_01],
            exits = [Exit(CmdZachod, true),
                     Exit(CmdPolnoc, true),
                     Exit(CmdPolodnie, true),
                     Exit(CmdWschod, true), ],
            posters = [new(CmdPatrz + CmdPlakat, [TxtConcertRooms_07])],
            turn = CrowdTurn,
        },
        new() {
            number = RoomCrowd2,
            position = new(-1, 0, -1),
            name = $"{TxtPlaceTlum} 2",
            intro = [TxtConcertRooms_09],
            exits = [Exit(CmdPolnoc, true),
                     Exit(CmdWschod, true), ],
            turn = CrowdTurn,
        },
        new() {
            number = RoomCrowd3,
            position = new(-1, 2, -1),
            name = $"{TxtPlaceTlum} 3",
            intro = [TxtConcertRooms_11],
            exits = [Exit(CmdPolodnie, true),
                     Exit(CmdWschod, true),
                     Exit(CmdPolnoc, true), ],
            turn = CrowdTurn,
        },
        new() {
            number = RoomCrowd4,
            position = new(0, 1, -1),
            name = $"{TxtPlaceTlum} 4",
            intro = [TxtYouAreIn + TxtConcertRooms_13, TxtConcertRooms_14],
            exits = [Exit(CmdPolnoc, true),
                     Exit(CmdZachod, true),
                     Exit(CmdPolodnie, true), ],
            turn = CrowdTurn,
        },
        new() {
            number = RoomCrowd5,
            position = new(0, 0, -1),
            name = $"{TxtPlaceTlum} 5",
            intro = [TxtConcertRooms_16, TxtConcertRooms_14],
            exits = [Exit(CmdPolnoc, true),
                     Exit(CmdZachod, true), ],
            turn = CrowdTurn,
        },
        new() {
            number = RoomCrowd6,
            position = new(0, 2, -1),
            name = $"{TxtPlaceTlum} 6",
            intro = [TxtConcertRooms_17],
            exits = [Exit(CmdPolodnie, true),
                     Exit(CmdZachod, true), ],
            turn = CrowdTurn,
        },
        new() {
            number = RoomPub,
            position = new(-1, 3, -1),
            name = TxtPlacePiwiarnia,
            intro = [TxtYouAreIn + TxtConcertRooms_18, TxtConcertRooms_19],
            exits = [Exit(CmdPolodnie, true),
                     Exit(CmdWschod, true), ],
            turn = phase => {
                if (phase == TRoomPhase.Enter) {
                    if (game.World.Monsters[Minibarman] == 67) {
                        Out.WriteLine(TxtConcertRooms_20);
                    }
                    if (game.World.Monsters[Fatso] == 67) {
                        Out.WriteLine(TxtConcertRooms_21);
                    }
                    game.DescribeCrowd();
                    return;
                }
                game.ConcertFights();
                switch (game.Verb) {
                    case CmdZabij:
                        switch (game.Rest) {
                            case CmdMiniBarman:
                                if (game.World.Monsters[Minibarman] == 67) {
                                    Out.WriteLine(TxtConcertRooms_23);
                                    game.Fight(VeryHardEnemy);
                                    if (game.Player.Energy > 0) {
                                        game.World.Monsters[Minibarman] = 0;
                                        game.Combat.Roll = game.Roll(100);
                                        if (game.Combat.Roll < 15) {
                                            Out.WriteLine(TxtConcertRooms_25 + TxtShared_08 + TxtPiwoTail);
                                            game.AcquireItem(TBagItem.Beer);
                                        }
                                    }
                                }
                                break;
                            case CmdGrubas:
                                if (game.World.Monsters[Fatso] == 67) {
                                    game.Fight(VeryHardEnemy);
                                    if (game.Player.Energy > 0) {
                                        game.World.Monsters[Fatso] = 0;
                                        if (game.Combat.Roll < 15) {
                                            Out.WriteLine(TxtConcertRooms_26 + TxtShared_08 + TxtPiwoTail);
                                            game.AcquireItem(TBagItem.Beer);
                                        }
                                    }
                                }
                                break;
                        }
                        break;
                }
            },
        },
        new() {
            number = RoomStageEntrance,
            position = new(0, 3, -1),
            name = TxtPlaceWejscieNaScene,
            intro = [TxtConcertRooms_27],
            exits = [Exit(CmdZachod, true),
                     Exit(CmdWschod, true), ],
            posters = [new(CmdPatrz + CmdTabliczka, [TxtConcertRooms_32])],
            turn = phase => {
                if (phase == TRoomPhase.Enter) {
                    game.DescribeCrowd();
                    return;
                }
                if (game.Verb == CmdExit) {
                    Out.WriteLine(TxtConcertRooms_31);
                }
                game.ConcertFights();
            },
        },
        new() {
            number = RoomBackstage,
            position = new(1, 3, -1),
            name = TxtPlaceZaKulisami,
            intro = [TxtConcertRooms_33],
            exits = [Exit(CmdZachod, true),
                     Exit(CmdPolodnie, true), ],
            turn = phase => {
                if (phase == TRoomPhase.Enter) {
                    if (game.World.Monsters[DJ] == 69) {
                        Out.WriteLine(TxtConcertRooms_35);
                    }
                    return;
                }
                if (game.Verb == CmdZabij && game.Rest == CmdDJ) {
                    if (game.World.Monsters[DJ] == 69) {
                        game.Fight(VeryHardEnemy);
                        if (game.Combat.EnemyEnergy < 0) {
                            game.World.Monsters[DJ] = 0;
                            game.Combat.Roll = game.Roll(100);
                            if (game.Combat.Roll < 25) {
                                Out.WriteLine(TxtShared_08 + TxtConcertRooms_38);
                                game.AcquireItem(TBagItem.DryRation);
                            }
                            if (game.Combat.Roll < 3) {
                                Out.WriteLine(TxtShared_08 + TxtConcertRooms_39);
                                Out.WriteLine(TxtBang + TxtUniqeItem + TxtConcertRooms_40);
                                game.AcquireItem(TBagItem.Cassette);
                            }
                        }
                    }
                }
            },
        },
        new() {
            number = RoomStage1,
            position = new(1, 2, -1),
            name = $"{TxtPlaceScena} 1",
            intro = [TxtConcertRooms_41],
            exits = [Exit(CmdPolnoc, true),
                     Exit(CmdPolodnie, true), ],
            turn = StageTurn,
        },
        new() {
            number = RoomStage2,
            position = new(1, 1, -1),
            name = TxtPlaceSrodekSceny,
            intro = [TxtConcertRooms_46],
            exits = [Exit(CmdPolnoc, true),
                     Exit(CmdPolodnie, true), ],
            turn = StageTurn,
        },
        new() {
            number = RoomStage3,
            position = new(1, 0, -1),
            name = $"{TxtPlaceScena} 3",
            intro = [TxtConcertRooms_48],
            exits = [Exit(CmdPolnoc, true)],
            turn = StageTurn,
        },
        new() {
            number = RoomConcertGate,
            position = new(-2, 1, 0),
            name = TxtPlaceDrogaDoJaskini,
            intro = [TxtRoom73_01],
            exits = [Exit(CmdPolodnie, true),
                     Exit(CmdDol, true), ],
        },
        new() {
            number = RoomDarkStreet,
            position = new(1, 0, 1),
            name = TxtPlaceUlCiemna,
            intro = [TxtRoom75_01],
            exits = [Exit(CmdDol, true),
                     Exit(CmdPolnoc, true),
                     Exit(CmdPolodnie, true), ],
        },
        new() {
            number = RoomBar,
            position = new(1, 1, 1),
            name = TxtPlaceBarPodDwomaPedalami,
            intro = [TxtYouAreIn + TxtRoom76_01, TxtRoom76_02],
            exits = [Exit(CmdPolodnie, true)],
            turn = phase => {
                if (phase == TRoomPhase.Enter) {
                    if (game.World.Monsters[Maciek] == 76) {
                        Out.WriteLine(TxtRoom76_05);
                    }
                    if (game.World.Monsters[Pedal] == 76) {
                        Out.WriteLine(TxtRoom76_06);
                    }
                    if (game.World.Monsters[Couple] == 76) {
                        Out.WriteLine(TxtRoom76_07);
                    }
                    return;
                }
                switch (game.Verb) {
                    case CmdLista:
                        Out.WriteLine(TxtRoom76_09);
                        break;
                    case CmdZabij:
                        switch (game.Rest) {
                            case CmdPedal:
                                game.Fight(EasyEnemy);
                                game.World.Monsters[Pedal] = 0;
                                game.PillGain();
                                break;
                            case CmdPara:
                                game.Fight(VeryHardEnemy);
                                game.World.Monsters[Couple] = 0;
                                game.Fight(VeryHardEnemy);
                                break;
                            case CmdMaciek:
                                game.Fight(EasyEnemy);
                                game.World.Monsters[Maciek] = 0;
                                break;
                        }
                        break;
                    case CmdMam:
                        if (game.Rest == Cmd18Lat) {
                            Out.WriteLine(TxtRoom76_13);
                        }
                        break;
                }
            },
        },
        new() {
            number = RoomIvy1,
            position = new(1, -1, 1),
            name = $"{TxtPlaceBluszcz} 1",
            intro = [TxtRoom77_01],
            exits = [Exit(CmdPolnoc, true),
                     Exit(CmdWschod, true),
                     Exit(CmdZachod, true),
                     Exit(CmdPolodnie, true), ],
            turn = PlantTurn,
        },
        new() {
            number = RoomIvy2,
            position = new(2, -1, 1),
            name = $"{TxtPlaceBluszcz} 2",
            intro = [TxtRoom78_01],
            exits = [Exit(CmdZachod, true)],
            turn = PlantTurn,
        },
        new() {
            number = RoomIvy3,
            position = new(0, -1, 1),
            name = $"{TxtPlaceBluszcz} 3",
            intro = [TxtRoom79_01],
            exits = [Exit(CmdWschod, true)],
            turn = PlantTurn,
        },
        new() {
            number = RoomIvy4,
            position = new(1, -2, 1),
            name = $"{TxtPlaceBluszcz} 4",
            intro = [TxtRoom80_01],
            exits = [Exit(CmdPolnoc, true),
                     Exit(CmdZachod, true),
                     Exit(CmdWschod, true),
                     Exit(CmdPolodnie, true), ],
            posters = [new(CmdPatrz + CmdNapis, [TxtRoom80_04])],
            turn = phase => {
                if (phase == TRoomPhase.Enter) {
                    game.DescribePlants();
                    return;
                }
                game.PlantFights();
                if (game.Verb == CmdPatrz && game.Rest == CmdStudnia) {
                    game.Combat.Roll = (game.Roll(3) + 1);
                    Out.WriteLine(TxtRoom80_05 + game.Combat.Roll + TxtShared_23);
                    game.Player.Money += game.Combat.Roll;
                    game.Combat.Roll = game.Roll(100);
                    if (game.Combat.Roll < 10) {
                        Out.WriteLine(TxtRoom80_06);
                        if (!game.Player.ChangeEnergy(-40)) {
                            game.Session.Room = RoomDead;
                            return;
                        }
                        Out.WriteLine(TxtRoom80_07);
                        game.Player.Money -= 100;
                        if (game.Player.Money < 0) {
                            game.Player.Money = 0;
                        }
                    }
                }
            },
        },
        new() {
            number = RoomIvy5,
            position = new(0, -2, 1),
            name = $"{TxtPlaceBluszcz} 5",
            intro = [TxtRoom81_01],
            exits = [Exit(CmdWschod, true)],
            turn = PlantTurn,
        },
        new() {
            number = RoomIvy6,
            position = new(2, -2, 1),
            name = $"{TxtPlaceBluszcz} 6",
            intro = [TxtRoom82_01],
            exits = [Exit(CmdZachod, true)],
            turn = PlantTurn,
        },
        new() {
            number = RoomClearing,
            position = new(1, -3, 1),
            name = TxtPlacePolana,
            intro = [TxtRoom83_01],
            exits = [Exit(CmdPolnoc, true),
                     Exit(CmdZachod, true), ],
            turn = PlantTurn,
        },
        new() {
            number = RoomCavePath,
            position = new(0, -3, 1),
            name = TxtPlacePodejrzaneKrzaki,
            intro = [TxtRoom84_01],
            exits = [Exit(CmdWschod, true),
                     Exit(CmdZachod, true),
                     Exit(CmdPolodnie, true), ],
            turn = PlantTurn,
        },
        new() {
            number = RoomLivingDoor,
            position = new(-1, -3, 1),
            name = TxtPlaceBudynek,
            intro = [TxtRoom85_01],
            exits = [Exit(CmdWschod, true),
                      Exit(CmdZachod, static doorClosed => !doorClosed, () => game.World.Door, TxtRoom85_07), ],
            turn = phase => {
                if (phase == TRoomPhase.Enter) {
                    if (game.World.Door) {
                        Out.WriteLine(TxtRoom85_03);
                    }
                    return;
                }
                if (game.Verb == CmdZabij && game.Rest == CmdDrzwi) {
                    if (game.World.Door) {
                        game.Fight(NotEasyEnemy);
                        if (game.Player.Energy > 0) {
                            game.World.Door = false;
                        }
                    }
                }

            },
        },
        new() {
            number = RoomHut,
            position = new(-2, -3, 1),
            name = TxtPlaceDomek,
            intro = [TxtRoom86_01],
            exits = [Exit(CmdWschod, true)],
            turn = phase => {
                if (phase == TRoomPhase.Enter) {
                    if (game.World.OldElf) {
                        Out.WriteLine(TxtRoom86_02);
                    }
                    if (!game.World.OldElf) {
                        Out.WriteLine(TxtRoom86_03);
                    }
                    return;
                }
                switch (game.Verb) {
                    case CmdPatrz:
                        if (game.Rest == CmdGarnki) {
                            Out.WriteLine(TxtRoom86_04);
                        }
                        break;
                    case CmdRozmawiaj:
                        if (game.Rest == CmdStaruch) {
                            if (game.Bag.Items[TBagItem.ReturnScroll] > 0) {
                                Out.WriteLine(TxtRoom86_13);
                                game.MoveTo(RoomCageHuge);
                            } else if ((game.Bag.Items[TBagItem.Sausage] < 3) && (game.Player.Money < StaruchPrice) && (game.Bag.Items[TBagItem.ReturnScroll] == 0)) {
                                Out.WriteLine(TxtRoom86_05);
                            } else if ((game.Bag.Items[TBagItem.Sausage] > 3) && (game.Player.Money >= StaruchPrice)) {
                                Out.WriteLine(TxtRoom86_08);
                                game.MoveTo(RoomCityCentre);
                                game.AcquireItem(TBagItem.Beer);
                                game.LoseItem(TBagItem.Sausage, 4);
                                game.Player.Money -= StaruchPrice;
                                game.AcquireItem(TBagItem.ReturnScroll);
                                game.Player.Experience += 50;
                            }
                        }
                        break;
                    case CmdZabij:
                        if (game.Rest == CmdStaruch) {
                            if (game.World.OldElf) {
                                game.Fight(WeakEnemy);
                                game.World.OldElf = false;
                                Out.WriteLine(TxtRoom86_14);
                                game.Player.Level -= 1;
                            }
                        }
                        break;
                }
            },
        },
        new() {
            number = RoomCaveMouth,
            position = new(0, -4, 1),
            name = TxtPlaceWejscieDoGroty,
            intro = [TxtRoom87_01],
            exits = [Exit(CmdPolnoc, true),
                     Exit(CmdPolodnie, true), ],
            turn = phase => {
                if (phase == TRoomPhase.Command) {
                    return;
                }
                game.Combat.Roll = game.Roll(100);
                if (game.Combat.Roll < 40) {
                    int damage = Math.Max(0, 25 - game.Player.Stats[TStat.Dexterity]);
                    Out.WriteLine(TxtRoom87_03 + damage + TxtShared_26);
                    if (!game.Player.ChangeEnergy(-damage)) {
                        game.Session.Room = RoomDead;
                    }
                }
            },
        },
        new() {
            number = RoomNettleThrone,
            position = new(0, -5, 1),
            name = TxtPlaceGrota,
            intro = [TxtRoom88_01],
            exits = [Exit(CmdPolnoc, true)],
            turn = phase => {
                if (phase == TRoomPhase.Command) {
                    return;
                }
                Out.Write(TxtRoom88_05);
                game.Command = Crt.ReadLn();
                Out.WriteLine(TxtRoom88_06);
                game.Combat.EnemyEnergy = 200;
                game.Combat.EnemyDexterity = 15;
                game.Combat.EnemyStrength = 18;
                game.Combat.FireballCharges = 5;
                game.Combat.PoisonCharges = 1;
                game.DoFight();
                if (game.Combat.EnemyEnergy < 0) {
                    game.LeafGain();
                    game.PillGain();
                    game.ScrollGain();
                    if (game.World.DuncanQuest != 0) {
                        game.World.DuncanQuest -= 50;
                    }
                }
                if (game.Player.Energy > 0) {
                    Out.WriteLine(TxtRoom88_07);
                    game.MoveTo(RoomCaveMouth);
                }
            },
        },
        new() {
            number = RoomIvyGate,
            position = new(1, 0, 0),
            name = TxtPlacePrzejscieNaCiemna,
            intro = [TxtRoom89_01],
            exits = [Exit(CmdZachod, true),
                     Exit(CmdGora, true), ],
        },
        new() {
            number = RoomRoadsGate,
            position = new(0, 3, 0),
            name = TxtPlaceBramaNaTrakt,
            intro = [TxtRoom99_01],
            exits = [Exit(CmdPolodnie, true),
                     Exit(CmdPolnoc, true), ],
        },
        new() {
            number = RoomQuestMaster,
            position = new(0, 4, 0),
            name = TxtPlaceRozjazd,
            intro = [TxtRoom100_01],
            exits = [Exit(CmdPolodnie, true),
                     Exit(CmdWschod, true),
                     Exit(CmdZachod, static allowed => allowed, () => game.Bag.Permit > 0, TxtRoom100_16), ],
            turn = phase => {
                if (phase == TRoomPhase.Enter) {
                    game.DescribeStreet();
                } else {
                    if (game.Verb == CmdLista) {
                        Out.WriteLine(TxtRoom100_05);
                    }
                    for (int i = 0; i < game.World.QuestOffers.Count; i++) {
                        TQuestOffer q = game.World.QuestOffers[i];
                        game.StartQuest(q.cmd, q.line, q.quest, q.kills);
                    }
                    if (game.Verb == CmdZakoncz && game.Rest == CmdQuest) {
                        if (game.World.Quest == 0) {
                            Out.WriteLine(TxtRoom100_18);
                        }

                        switch (game.World.Quest) {
                            case 1:
                                if (game.World.QuestKills < 1) {
                                    game.World.GrantQuestReward(1, 100);
                                } else {
                                    Out.WriteLine(TxtRoom100_19);
                                }
                                break;
                            case 2:
                                if ((game.World.QuestKills < 1) && game.World.LiroyKilled && (game.Bag.Items[TBagItem.Diploma] > 0)) {
                                    game.World.GrantQuestReward(2, 250);
                                } else {
                                    Out.WriteLine(TxtRoom100_19);
                                }
                                break;
                            case 3:
                                if ((game.World.QuestKills < 1) && game.World.LiroyKilled && (game.Bag.Items[TBagItem.Pipe] > 0)) {
                                    game.World.GrantQuestReward(3, 425);
                                } else {
                                    Out.WriteLine(TxtRoom100_19);
                                }
                                break;
                        }
                    }
                    switch (game.Verb) {
                        case CmdZabij:
                            if (game.Rest == CmdQuestMaster) {
                                game.Combat.PoisonCharges = 10;
                                game.Combat.FireballCharges = 20;
                                game.Combat.EnemyStrength = 10;
                                game.Combat.EnemyDexterity = 26;
                                game.Combat.EnemyEnergy = 188;
                                game.DoFight();
                                if (game.Combat.EnemyEnergy < 1) {
                                    game.AcquirePermit();
                                }
                            }
                            break;
                    }
                }
            },
        },
        new() {
            number = RoomRoad1,
            position = new(-1, 4, 0),
            name = $"{TxtPlaceDroga} 1",
            intro = [TxtRoom101_01],
            exits = [Exit(CmdWschod, true),
                     Exit(CmdZachod, true),
                     Exit(CmdPolnoc, true),
                     Exit(CmdPolodnie, true), ],
            posters = [new(CmdPatrz + CmdDrogowskaz, [TxtRoom101_05])],
        },
        new() {
            number = RoomRoad2,
            position = new(-2, 4, 0),
            name = $"{TxtPlaceDroga} 2",
            intro = [TxtRoom102_01],
            exits = [Exit(CmdWschod, true),
                     Exit(CmdZachod, true),
                     Exit(CmdPolnoc, true),
                     Exit(CmdPolodnie, true), ],
        },
        new() {
            number = RoomRoad3,
            position = new(-3, 4, 0),
            name = $"{TxtPlaceDroga} 3",
            intro = [TxtRoom103_01],
            exits = [Exit(CmdWschod, true),
                     Exit(CmdPolnoc, true),
                     Exit(CmdPolodnie, true), ],
        },
        new() {
            number = RoomFutureRoad,
            position = new(1, 4, 0),
            name = TxtPlaceDrogaWProdukcji,
            intro = [TxtRoom110_01],
            exits = [Exit(CmdZachod, true)],
        },
        new() {
            number = RoomForest,
            position = new(-1, 5, 0),
            name = $"{TxtPlaceLas} 1",
            intro = [TxtForest_01],
            exits = [Exit(CmdPolodnie, true)],
        },
        new() {
            number = RoomForest1,
            position = new(-1, 3, 0),
            name = $"{TxtPlaceLas} 2",
            intro = [TxtForest_01],
            exits = [Exit(CmdPolnoc, true)],
        },
        new() {
            number = RoomForest2,
            position = new(-2, 5, 0),
            name = $"{TxtPlaceLas} 3",
            intro = [TxtForest_01],
            exits = [Exit(CmdPolodnie, true)],
        },
        new() {
            number = RoomForest3,
            position = new(-2, 3, 0),
            name = $"{TxtPlaceLas} 4",
            intro = [TxtForest_01],
            exits = [Exit(CmdPolnoc, true)],
        },
        new() {
            number = RoomForest4,
            position = new(-3, 5, 0),
            name = $"{TxtPlaceLas} 5",
            intro = [TxtForest_01],
            exits = [Exit(CmdPolodnie, true)],
        },
        new() {
            number = RoomForest5,
            position = new(-3, 3, 0),
            name = $"{TxtPlaceLas} 6",
            intro = [TxtForest_01],
            exits = [Exit(CmdPolnoc, true)],
        },
    ];
    List<TRoom>? _roomDefs;

    public Dictionary<int, TRoom> RoomIndex => _roomIndex ??= BuildRoomIndex();
    Dictionary<int, TRoom>? _roomIndex;

    Dictionary<TCoordinate, TRoom> RoomGrid => _roomGrid ??= BuildRoomGrid();
    Dictionary<TCoordinate, TRoom>? _roomGrid;

    Dictionary<int, TRoom> BuildRoomIndex() =>
        RoomDefs.ToDictionary(room => room.number);

    Dictionary<TCoordinate, TRoom> BuildRoomGrid() =>
        RoomDefs.ToDictionary(room => room.position);

    TCoordinate DirectionDelta(string cmd) =>
        cmd switch {
            CmdPolnoc => new(0, 1, 0),
            CmdPolodnie => new(0, -1, 0),
            CmdWschod => new(1, 0, 0),
            CmdZachod => new(-1, 0, 0),
            CmdGora => new(0, 0, 1),
            CmdDol => new(0, 0, -1),
            _ => throw new ArgumentOutOfRangeException(nameof(cmd)),
        };

    TCoordinate Neighbor(TCoordinate position, string cmd) {
        TCoordinate delta = DirectionDelta(cmd);
        return new(position.X + delta.X, position.Y + delta.Y, position.Z + delta.Z);
    }

    public int? RoomInDirection(int number, string cmd) =>
        RoomGrid.TryGetValue(Neighbor(RoomIndex[number].position, cmd), out TRoom? adjacent)
            ? adjacent.number
            : null;

    public TMovement ResolveMove(TRoom room, TRoomExit exit) {
        int destination = RoomInDirection(room.number, exit.cmd)
            ?? throw new InvalidOperationException($"room {room.number} exits {exit.cmd} into an empty coordinate");
        return new() {
            cmd = exit.cmd,
            dest = destination,
            predicate = exit.IsOpen,
            blocked = exit.blocked,
        };
    }

    TRoomExit Exit(string cmd, bool enabled = true, string? blocked = null) =>
        new() {
            cmd = cmd,
            enabled = enabled,
            blocked = blocked,
        };

    TRoomExit Exit(string cmd, Predicate<bool> predicate, Func<bool> state, string? blocked = null) =>
        new() {
            cmd = cmd,
            predicate = predicate,
            state = state,
            blocked = blocked,
        };
}
