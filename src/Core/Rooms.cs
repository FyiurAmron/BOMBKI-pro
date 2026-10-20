
using static Game;
using static Game.TMonster;
using static Game.TFoe;
using static Game.TStreetKind;
using static Texts;

public static class Rooms {

    public const int RoomAny = -1;
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
    public const int RoomTeleport = 18;
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
    public const int RoomValley = 60;
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

    public const int StaruchPrice = 200;

    public class TRoomPoster {
        public string cmd = "";
        public List<string> lines = [];
        public TRoomPoster(string cmd, List<string> lines) {
            this.cmd = cmd;
            this.lines = lines;
        }
    }

    public class TRoom {
        public int number;
        public List<string> intro = [];
        public List<string> exits = [];
        public List<TMovement> moves = [];
        public List<TRoomPoster> posters = [];
        public bool expand = false;
        public int teleport = RoomAny;
        public Action<bool>? turn = null;
    }

    static void StreetTurn(bool entering) {
        if (entering) {
            DescribeStreet();
        } else {
            StreetFights();
        }
    }

    static void PlantTurn(bool entering) {
        if (entering) {
            DescribePlants();
        } else {
            PlantFights();
        }
    }

    static void CrowdTurn(bool entering) {
        if (entering) {
            DescribeCrowd();
        } else {
            ConcertFights();
        }
    }

    static void StageTurn(bool entering) {
        if (entering) {
            DescribeStage();
        } else {
            StageFights();
        }
    }

    static void ArenaTurn(bool entering) {
        if (entering) {
            DescribeBeasts();
            return;
        }
        switch (Verb) {
            case CmdZabij:
                switch (Rest) {
                    case CmdBakteria:
                        if (Session.Room == World.Monsters[TMonster.Bacterium]) {
                            Fight(TFoe.WeakEnemy);
                            World.Monsters[TMonster.Bacterium] = 0;
                            Out.WriteLine(TxtArenaMaze_02);
                        }
                        break;
                    case CmdWilczur:
                        if (Session.Room == World.Monsters[TMonster.Mongrel]) {
                            Out.WriteLine(TxtArenaMaze_03);
                            Fight(TFoe.WeakishEnemy);
                            World.Monsters[TMonster.Mongrel] = 0;
                        }
                        break;
                }
                break;
        }
        for (int bi = 1; bi < BeastKills.Count; bi++) {
            TBeastKill b = BeastKills[bi];
            if (b.unlessFled) {
                KillBeastUnlessFled(b.cmd, IntSlot.Into(World.Monsters, b.beast), b.foe);
            } else {
                KillBeast(b.cmd, IntSlot.Into(World.Monsters, b.beast), b.foe);
            }
        }
        for (int ai = 1; ai < ArenaKills.Count; ai++) {
            TArenaKill a = ArenaKills[ai];
            ArenaKill(a.cmd, a.monster, a.pills);
        }
        switch (Verb) {
            case CmdPolodnie:
                MazeMove(TMazeDir.mdPolodnie);
                break;
            case CmdPolnoc:
                MazeMove(TMazeDir.mdPolnoc);
                break;
            case CmdWschod:
                MazeMove(TMazeDir.mdWschod);
                break;
            case CmdZachod:
                MazeMove(TMazeDir.mdZachod);
                break;
        }
    }

    public static void CageMonsterFight(IntSlot monster, int room, TCageStats stats) {
        if ((Verb == CmdZabij && Rest == CmdPotwor) && (monster.Get() == room)) {
            Combat.EnemyEnergy = stats.energy;
            Combat.EnemyDexterity = stats.dexterity;
            Combat.EnemyStrength = stats.strength;
            DoFight();
            if (!Combat.Fled) {
                monster.Set(0);
                Combat.Loot = Roll(stats.lootRand);
                Out.WriteLine(TxtShared_08 + Combat.Loot + TxtShared_09);
                Player.Money += Combat.Loot;
                TryCageLoot(stats.lootThreshold);
            } else {
                Combat.Fled = false;
            }
        }
    }

    public static void DescribeCage(TMonster monster, int room, string aliveLine, string deadLine) {
        Out.WriteLine(TxtShared_06);
        if (World.Monsters[monster] == room) {
            Out.WriteLine(aliveLine);
        } else if (World.Monsters[monster] == 0) {
            Out.WriteLine(deadLine);
        }
    }

    public static void GrantQuestReward(int quest, int exp) {
        Out.WriteLine(TxtRoom100_12);
        Out.WriteLine(TxtQuestReward + exp + TxtSpaceKunsztDashes);
        Player.Experience += exp;
        World.Quest = 0;
        World.QuestKills = 0;
        Bag.Permit -= ItemUnit;
        switch (quest) {
            case 1:
                Bag.Count += 1;
                break;
            case 2:
                Bag.Items[TBagItem.Diploma] = 0;
                Bag.Count -= 1;
                Player.MaxEnergy -= 5;
                break;
            case 3:
                Bag.Count -= 1;
                Player.Stats[TStat.Wisdom]--;
                Bag.Items[TBagItem.Pipe] += ItemUnit;
                break;
        }
    }

    static TRoom SideRoom(int number, int dest, string direction, string exitDesc) =>
        new() {
            number = number,
            intro = [TxtYouAreIn + TxtSimpleRoom_01],
            exits = [TxtShared_03, direction + TxtSimpleRoom_02, exitDesc],
            moves = [Go(direction, dest)],
        };

    static TRoom ArenaRoom(int number, List<string> exits) =>
        new() {
            number = number,
            intro = [TxtArenaMaze_01],
            exits = exits,
            turn = ArenaTurn,
            expand = true,
        };

    public static readonly List<TRoom> RoomDefs = [
        new() {
            number = RoomStart,
            intro = [TxtRoom0_01],
            exits = [TxtShared_03,
                     TxtExitPolnoc, TxtPlacePokojCentralny, ],
            moves = [Go(CmdPolnoc, RoomSalon)],
        },
        new() {
            number = RoomSalon,
            intro = [TxtYouAreIn + TxtRoom1_01, TxtRoom1Lines1[1], TxtRoom1Lines1[2], TxtRoom1Lines1[3]],
            exits = [TxtShared_03,
                     TxtExitPolodnie, TxtPlaceTamGdzieZaczynaszGre,
                     TxtExitPolnoc, TxtPlaceMiasto,
                     TxtExitWschod, TxtPlacePokojTreningowy1,
                     TxtExitZachod, TxtPlaceSzkolaZycia,
                     TxtExitDol, TxtPlacePoddziemnyPokojTreningowy2, ],
            moves = [Go(CmdPolodnie, RoomStart),
                     Go(CmdWschod, RoomTraining),
                     Go(CmdDol, RoomMeditation),
                     Go(CmdZachod, RoomSchoolHall),
                     Go(CmdPolnoc, RoomCityCentre), ],
            posters = [new(CmdPatrz + CmdPlakat, [..TxtRoom1Lines2.Skip(1)])],
        },
        new() {
            number = RoomTraining,
            intro = [TxtYouAreIn + TxtRoom2_01, TxtRoom2_02, TxtShared_04],
            exits = [TxtShared_03,
                     TxtExitZachod, TxtPlacePokojCentralny, ],
            moves = [Go(CmdZachod, RoomSalon)],
            posters = [new(CmdPatrz + CmdPlakat, [..TxtRoom2Lines1.Skip(1)])],
            turn = entering => {
                if (!entering) {
                    HandleTrain();
                }
            },
        },
        new() {
            number = RoomMeditation,
            intro = [..TxtRoom3Lines1.Skip(1)],
            exits = [TxtShared_03,
                     TxtExitGora, TxtPlacePokojCentralny, ],
            moves = [Go(CmdGora, RoomSalon)],
            turn = entering => {
                if (!entering) {
                    Training();
                }
            },
        },
        new() {
            number = RoomSchoolHall,
            intro = [TxtYouAreIn + TxtRoom4_01, TxtRoom4_02],
            exits = [TxtShared_03,
                     TxtExitWschod, TxtPlacePokojCentralny,
                     TxtExitZachod, CmdSzkolaZycia2, ],
            moves = [Go(CmdWschod, RoomSalon), Go(CmdZachod, RoomLost)],
            posters = [new(CmdPatrz + CmdAfisz, [..TxtRoom4Lines1.Skip(1)])],
        },
        new() {
            number = RoomLost,
            intro = [..TxtRoom5Lines1.Skip(1)],
            exits = [TxtShared_03,
                     TxtExitWschod, TxtPlaceSzkolaZycia,
                     TxtExitZachod, TxtPlacePokoj,
                     TxtExitPolnoc, TxtPlacePokoj,
                     TxtExitPolodnie, TxtPlacePokoj,
                     TxtExitGora, TxtPlacePokoj,
                     TxtExitDol, TxtPlacePokoj, ],
            moves = [Go(CmdWschod, RoomSchoolHall),
                     Go(CmdZachod, RoomSchoolSide1),
                     Go(CmdPolnoc, RoomSchoolSide2),
                     Go(CmdPolodnie, RoomSchoolSide3),
                     Go(CmdGora, RoomSchoolLanding),
                     Go(CmdDol, RoomSchoolSide4), ],
            posters = [new(CmdPatrz + CmdPlakat, [TxtRoom5_10, TxtRoom5_11])],
        },
        SideRoom(RoomSchoolSide1, RoomLost, CmdWschod, CmdSzkolaZycia2),
        SideRoom(RoomSchoolSide3, RoomLost, CmdPolnoc, CmdSzkolaZycia2),
        SideRoom(RoomSchoolSide2, RoomLost, CmdPolodnie, CmdSzkolaZycia2),
        SideRoom(RoomSchoolSide4, RoomLost, CmdGora, CmdSzkolaZycia2),
        new() {
            number = RoomSchoolLanding,
            intro = [TxtYouAreIn + TxtRoom9_01, TxtRoom9_02],
            exits = [TxtShared_03,
                     TxtExitDol, CmdSzkolaZycia2,
                     TxtExitGora, TxtPlaceKlatkiPelneGajdy, ],
            moves = [Go(CmdDol, RoomLost), Go(CmdGora, RoomCageCorridor)],
        },
        new() {
            number = RoomCageCorridor,
            intro = [TxtYouAreIn + TxtRoom11_01, ..TxtRoom11Lines1.Skip(1)],
            exits = [TxtShared_03,
                     TxtExitWschod, TxtPlaceKlatkaCieniasa,
                     TxtExitZachod, TxtPlaceKlatkaSilnego,
                     TxtExitPolnoc, TxtPlaceKlatkaZrecznego,
                     TxtExitPolodnie, TxtPlaceKlatkaOdpornego,
                     TxtExitGora, TxtPlaceKlatkaSilnegoOdpornegoIZrecznego,
                     TxtExitDol, TxtPlaceJedynaDrogaNieProwadzacaDoKlatek, ],
            moves = [Go(CmdWschod, RoomCageWeakling),
                     Go(CmdZachod, RoomCageStrong),
                     Go(CmdPolnoc, RoomCageAgile),
                     Go(CmdPolodnie, RoomCageTough),
                     Go(CmdGora, RoomCageHuge),
                     Go(CmdDol, RoomCageReward), ],
            posters = [new(CmdPatrz + CmdPlakat, [..TxtRoom11Lines2.Skip(1)])],
            expand = true,
        },
        new() {
            number = RoomCageWeakling,
            exits = [TxtShared_03,
                     TxtExitZachod, TxtPlaceKlatkiPelneGajdy, ],
            moves = [Go(CmdZachod, RoomCageCorridor)],
            turn = entering => {
                if (entering) {
                    DescribeCage(CageWeakling, RoomCageWeakling, TxtRoom12_01, TxtShared_07);
                } else {
                    CageMonsterFight(IntSlot.Into(World.Monsters, CageWeakling), RoomCageWeakling, CageWeaklingStats);
                }
            },
        },
        new() {
            number = RoomCageStrong,
            exits = [TxtShared_03,
                     TxtExitWschod, TxtPlaceKlatkiPelneGajdy, ],
            moves = [Go(CmdWschod, RoomCageCorridor)],
            turn = entering => {
                if (entering) {
                    DescribeCage(CageStrong, RoomCageStrong, TxtRoom13_01, TxtShared_07);
                } else {
                    CageMonsterFight(IntSlot.Into(World.Monsters, CageStrong), RoomCageStrong, CageStrongStats);
                }
            },
        },
        new() {
            number = RoomCageAgile,
            exits = [TxtShared_03,
                     TxtExitPolodnie, TxtPlaceKlatkiPelneGajdy, ],
            moves = [Go(CmdPolodnie, RoomCageCorridor)],
            turn = entering => {
                if (entering) {
                    DescribeCage(CageAgile, RoomCageAgile, TxtRoom14_01, TxtRoom14_02);
                } else {
                    CageMonsterFight(IntSlot.Into(World.Monsters, CageAgile), RoomCageAgile, CageAgileStats);
                }
            },
        },
        new() {
            number = RoomCageTough,
            exits = [TxtShared_03,
                     TxtExitPolnoc, TxtPlaceKlatkiPelneGajdy, ],
            moves = [Go(CmdPolnoc, RoomCageCorridor)],
            turn = entering => {
                if (entering) {
                    DescribeCage(CageTough, RoomCageTough, TxtRoom15_01, TxtShared_07);
                } else {
                    CageMonsterFight(IntSlot.Into(World.Monsters, CageTough), RoomCageTough, CageToughStats);
                }
            },
        },
        new() {
            number = RoomCageHuge,
            exits = [TxtShared_03,
                     TxtExitDol, TxtPlaceKlatkiPelneGajdy, ],
            moves = [Go(CmdDol, RoomCageCorridor)],
            turn = entering => {
                if (entering) {
                    DescribeCage(CageHuge, RoomCageHuge, TxtRoom16_01, TxtShared_07);
                }
                if (World.Monsters[CageHuge] == RoomCageHuge) {
                    Combat.EnemyEnergy = CageHugeStats.energy;
                    Combat.EnemyDexterity = CageHugeStats.dexterity;
                    Combat.EnemyStrength = CageHugeStats.strength;
                    DoFight();
                    if (Combat.Fled) {
                        Combat.Fled = false;
                        MoveTo(RoomCageCorridor);
                    } else {
                        World.Monsters[CageHuge] = 0;
                        Combat.Loot = Roll(CageHugeStats.lootRand);
                        Out.WriteLine(TxtShared_08 + Combat.Loot + TxtShared_09);
                        Player.Money += Combat.Loot;
                        TryCageLoot(CageHugeStats.lootThreshold);
                    }
                }
            },
        },
        new() {
            number = RoomCageReward,
            intro = [TxtYouAreIn + TxtRoom17_01],
            exits = [TxtShared_03,
                     TxtExitGora, TxtPlaceKlatkiPelneGajdy,
                     TxtExitDol, TxtPlaceTeleport, ],
            moves = [Go(CmdGora, RoomCageCorridor), Go(CmdDol, RoomTeleport)],
            turn = entering => {
                if (entering) {
                    return;
                }
                if (Verb == CmdPatrz && Rest == CmdPlakat) {
                    WriteLines([.. TxtRoom17Lines1.Skip(1)]);
                    if ((World.Monsters[CageWeakling] == 0) && (World.Monsters[CageAgile] == 0) &&
                    (World.Monsters[CageStrong] == 0) && (World.Monsters[CageTough] == 0) &&
                    (World.Monsters[CageHuge] == 0) && (Bag.Items[TBagItem.Diploma] == 0)) {
                        Bag.Items[TBagItem.Diploma] = RoomCageReward;
                    }
                }
            },
        },
        new() {
            number = RoomTeleport,
            intro = [..TxtRoom18Lines1.Skip(1)],
            teleport = RoomSalon,
        },
        new() {
            number = RoomCityCentre,
            intro = [TxtYouAreIn + TxtRoom20_01],
            exits = [TxtShared_03,
                     TxtExitPolodnie, TxtPlacePokojCentralny,
                     TxtExitPolnoc, TxtPlaceUlSklepikowa,
                     TxtExitWschod, TxtPlaceUlCiemna,
                     TxtExitZachod, TxtPlaceUlDluga, ],
            moves = [Go(CmdPolodnie, RoomSalon),
                     Go(CmdPolnoc, RoomShopStreet),
                     Go(CmdZachod, RoomLongStreet),
                     Go(CmdWschod, RoomDarkStreet), ],
            posters = [new(CmdPatrz + CmdPlakat, [TxtRoom20_02])],
            turn = StreetTurn,
        },
        new() {
            number = RoomShopStreet,
            intro = [TxtRoom21_01, TxtRoom21_02],
            exits = [TxtShared_03,
                     TxtExitPolodnie, TxtPlaceCentrumMiasta,
                     TxtExitPolnoc, TxtPlaceUlSklepikowa,
                     TxtExitWschod, TxtPlacePiekarnia,
                     TxtExitZachod, TxtPlaceZbrojownia, ],
            moves = [Go(CmdPolodnie, RoomCityCentre),
                     Go(CmdPolnoc, RoomShopStreet2),
                     Go(CmdWschod, RoomBakery),
                     Go(CmdZachod, RoomArmoury), ],
            turn = StreetTurn,
        },
        new() {
            number = RoomShopStreet2,
            intro = [TxtRoom22_01, TxtRoom22_02],
            exits = [TxtShared_03,
                     TxtExitPolodnie, TxtPlaceUlSklepikowa,
                     TxtExitPolnoc, TxtPlaceRozjazd,
                     TxtExitWschod, TxtPlaceSklepWielobranzowy,
                     TxtExitZachod, TxtPlaceSklepMagiczny, ],
            moves = [Go(CmdPolodnie, RoomShopStreet),
                     Go(CmdWschod, RoomGeneralStore),
                     Go(CmdZachod, RoomMagicShop),
                     Go(CmdPolnoc, RoomQuestMaster), ],
            turn = StreetTurn,
        },
        new() {
            number = RoomBakery,
            intro = [TxtYouAreIn + TxtRoom23_01, TxtRoom23_02, TxtRoom23_03],
            exits = [TxtShared_03,
                     TxtExitZachod, TxtPlaceUlicaSklepikowa, ],
            moves = [Go(CmdZachod, RoomShopStreet)],
            posters = [new(CmdPatrz + CmdPlakat, [TxtRoom23_04, TxtRoom23_05])],
            turn = entering => {
                if (entering) {
                    DescribeStreet();
                } else {
                    BakeryShop();
                }
            },
        },
        new() {
            number = RoomArmoury,
            intro = [TxtYouAreIn + TxtRoom24_01, TxtRoom24_02, TxtRoom24_03],
            exits = [TxtShared_03,
                     TxtExitWschod, TxtPlaceUlicaSklepikowa, ],
            moves = [Go(CmdWschod, RoomShopStreet)],
            posters = [new(CmdPatrz + CmdPlakat, [TxtRoom24_04])],
            turn = entering => {
                if (entering) {
                    DescribeStreet();
                } else {
                    ArmouryShop();
                    StreetFights();
                }
            },
        },
        new() {
            number = RoomGeneralStore,
            intro = [TxtRoom25_01, TxtRoom25_02],
            exits = [TxtShared_03,
                     TxtExitZachod, TxtPlaceUlicaSklepikowa, ],
            moves = [Go(CmdZachod, RoomShopStreet2)],
            posters = [new(CmdPatrz + CmdWywieszka, [TxtRoom25_03]),
                       new(CmdPatrz + CmdPoster, [TxtGeneralStoreShop_06]), ],
            turn = entering => {
                if (entering) {
                    DescribeStreet();
                } else {
                    StreetFights();
                    GeneralStoreShop();
                }
            },
        },
        new() {
            number = RoomMagicShop,
            intro = [TxtRoom26_01],
            exits = [TxtShared_03,
                     TxtExitWschod, TxtPlaceUlicaSklepikowa, ],
            moves = [Go(CmdWschod, RoomShopStreet2)],
            posters = [new(CmdPatrz + CmdPlakat, [TxtRoom26_02])],
            turn = entering => {
                if (entering) {
                    DescribeStreet();
                } else {
                    MagicShop();
                    StreetFights();
                }
            },
        },
        new() {
            number = RoomLongStreet,
            intro = [TxtRoom30_01, TxtRoom30_02],
            exits = [TxtShared_03,
                     TxtExitWschod, TxtPlaceCentrumMiasta,
                     TxtExitZachod, TxtPlaceUlicaDluga,
                     TxtExitPolnoc, TxtPlaceWejscieNaArene, ],
            moves = [Go(CmdWschod, RoomCityCentre),
                     Go(CmdZachod, RoomLongStreet2),
                     Go(CmdPolnoc, RoomArenaEntrance), ],
        },
        new() {
            number = RoomLongStreet2,
            intro = [TxtRoom31_01, TxtRoom31_02],
            exits = [TxtShared_03,
                     TxtExitWschod, TxtPlaceUlicaDluga,
                     TxtExitPolnoc, TxtPlaceDolinaRozrywek, ],
            moves = [Go(CmdWschod, RoomLongStreet), Go(CmdPolnoc, RoomValley)],
        },
        new() {
            number = RoomArenaEntrance,
            intro = [TxtRoom32_01, TxtRoom32_02],
            exits = [TxtShared_03,
                     TxtExitPolodnie, TxtPlaceUlicaDluga,
                     TxtExitPolnoc, TxtPlaceArena, ],
            moves = [Go(CmdPolodnie, RoomLongStreet), Go(CmdPolnoc, RoomArena33)],
            posters = [new(CmdPatrz + CmdPlakat, [TxtRoom32_03, TxtRoom32_04])],
        },
        ArenaRoom(RoomArena33, [TxtShared_03,
                                TxtExitPolodnie, TxtPlaceWejscieNaArene,
                                TxtExitPolnoc, TxtPlaceArena,
                                TxtExitWschod, TxtPlaceArena,
                                TxtExitZachod, TxtPlaceArena, ]),
        ArenaRoom(RoomArena34, [TxtShared_03,
                                TxtExitPolodnie, TxtPlaceArena,
                                TxtExitPolnoc, TxtPlaceArena,
                                TxtExitWschod, TxtPlaceArena,
                                TxtExitZachod, TxtPlaceArena, ]),
        ArenaRoom(RoomArena35, [TxtShared_03,
                                TxtExitPolnoc, TxtPlaceArena,
                                TxtExitWschod, TxtPlaceArena,
                                TxtExitZachod, TxtPlaceArena, ]),
        ArenaRoom(RoomArena36, [TxtShared_03,
                                TxtExitPolnoc, TxtPlaceArena,
                                TxtExitWschod, TxtPlaceArena,
                                TxtExitZachod, TxtPlaceArena, ]),
        ArenaRoom(RoomArena37, [TxtShared_03,
                                TxtExitPolodnie, TxtPlaceArena,
                                TxtExitPolnoc, TxtPlaceArena,
                                TxtExitWschod, TxtPlaceArena,
                                TxtExitZachod, TxtPlaceArena, ]),
        ArenaRoom(RoomArena38, [TxtShared_03,
                                TxtExitPolodnie, TxtPlaceArena,
                                TxtExitPolnoc, TxtPlaceArena,
                                TxtExitWschod, TxtPlaceArena,
                                TxtExitZachod, TxtPlaceArena, ]),
        ArenaRoom(RoomArena39, [TxtShared_03,
                                TxtExitPolodnie, TxtPlaceArena,
                                TxtExitPolnoc, TxtPlaceArena,
                                TxtExitWschod, TxtPlaceArena,
                                TxtExitZachod, TxtPlaceArena, ]),
        ArenaRoom(RoomArena40, [TxtShared_03,
                                TxtExitPolnoc, TxtPlaceArena,
                                TxtExitZachod, TxtPlaceArena, ]),
        ArenaRoom(RoomArena41, [TxtShared_03,
                                TxtExitPolnoc, TxtPlaceArena,
                                TxtExitWschod, TxtPlaceArena, ]),
        ArenaRoom(RoomArena42, [TxtShared_03,
                                TxtExitPolodnie, TxtPlaceArena,
                                TxtExitPolnoc, TxtPlaceArena,
                                TxtExitWschod, TxtPlaceArena,
                                TxtExitZachod, TxtPlaceArena, ]),
        ArenaRoom(RoomArena43, [TxtShared_03,
                                TxtExitPolodnie, TxtPlaceArena,
                                TxtExitPolnoc, TxtPlaceArena,
                                TxtExitWschod, TxtPlaceArena,
                                TxtExitZachod, TxtPlaceArena, ]),
        ArenaRoom(RoomArena44, [TxtShared_03,
                                TxtExitPolodnie, TxtPlaceArena,
                                TxtExitPolnoc, TxtPlaceArena,
                                TxtExitWschod, TxtPlaceArena,
                                TxtExitZachod, TxtPlaceArena, ]),
        ArenaRoom(RoomArena45, [TxtArenaMaze_08,
                                TxtExitPolnoc, TxtPlaceArena,
                                TxtExitPolodnie, TxtPlaceArena,
                                TxtExitZachod, TxtPlaceArena, ]),
        ArenaRoom(RoomArena46, [TxtArenaMaze_08,
                                TxtExitPolnoc, TxtPlaceArena,
                                TxtExitPolodnie, TxtPlaceArena,
                                TxtExitWschod, TxtPlaceArena, ]),
        ArenaRoom(RoomArena47, [TxtArenaMaze_09,
                                TxtExitPolodnie, TxtPlaceArena,
                                TxtExitWschod, TxtPlaceArena,
                                TxtExitZachod, TxtPlaceArena, ]),
        ArenaRoom(RoomArena48, [TxtShared_03,
                                TxtExitPolodnie, TxtPlaceArena,
                                TxtExitPolnoc, TxtPlaceArena,
                                TxtExitWschod, TxtPlaceArena,
                                TxtExitZachod, TxtPlaceArena, ]),
        ArenaRoom(RoomArena49, [TxtShared_03,
                                TxtExitPolodnie, TxtPlaceArena,
                                TxtExitPolnoc, TxtPlaceArena,
                                TxtExitWschod, TxtPlaceArena,
                                TxtExitZachod, TxtPlaceArena, ]),
        ArenaRoom(RoomArena50, [TxtArenaMaze_08,
                                TxtExitPolnoc, TxtPlaceArena,
                                TxtExitPolodnie, TxtPlaceArena,
                                TxtExitZachod, TxtPlaceArena, ]),
        ArenaRoom(RoomArena51, [TxtArenaMaze_08,
                                TxtExitPolnoc, TxtPlaceArena,
                                TxtExitPolodnie, TxtPlaceArena,
                                TxtExitWschod, TxtPlaceArena, ]),
        ArenaRoom(RoomArena52, [TxtArenaMaze_09,
                                TxtExitPolodnie, TxtPlaceArena,
                                TxtExitWschod, TxtPlaceArena,
                                TxtExitZachod, TxtPlaceArena, ]),
        ArenaRoom(RoomArena53, [TxtArenaMaze_09,
                                TxtExitPolodnie, TxtPlaceArena,
                                TxtExitWschod, TxtPlaceArena,
                                TxtExitZachod, TxtPlaceArena, ]),
        ArenaRoom(RoomArena54, [TxtArenaMaze_08,
                                TxtExitPolnoc, TxtPlaceArena,
                                TxtExitPolodnie, TxtPlaceArena,
                                TxtExitZachod, TxtPlaceArena, ]),
        ArenaRoom(RoomArena55, [TxtArenaMaze_08,
                                TxtExitPolnoc, TxtPlaceArena,
                                TxtExitPolodnie, TxtPlaceArena,
                                TxtExitWschod, TxtPlaceArena, ]),
        ArenaRoom(RoomArena56, [TxtArenaMaze_10,
                                TxtExitPolodnie, TxtPlaceArena,
                                TxtExitZachod, TxtPlaceArena, ]),
        ArenaRoom(RoomArena57, [TxtArenaMaze_10,
                                TxtExitPolodnie, TxtPlaceArena,
                                TxtExitWschod, TxtPlaceArena, ]),
        new() {
            number = RoomValley,
            intro = [TxtYouAreIn + TxtRoom60_01, TxtRoom60_02, TxtRoom60_03],
            exits = [TxtShared_03,
                     TxtExitPolodnie, TxtPlaceUlicaDluga,
                     TxtExitWschod, TxtPlaceBudowlaPelnaHalasu, ],
            moves = [Go(CmdPolodnie, RoomLongStreet2), Go(CmdWschod, RoomCrowd1)],
        },
        new() {
            number = RoomCrowd1,
            intro = [..TxtConcertRoomsLines1.Skip(1)],
            exits = [TxtShared_03,
                     TxtExitZachod, TxtPlaceDolinaRozrywek,
                     TxtExitPolnoc, TxtPlaceTlum,
                     TxtExitPolodnie, TxtPlaceTlum,
                     TxtExitWschod, TxtPlaceTlum, ],
            moves = [Go(CmdZachod, RoomValley),
                     Go(CmdPolnoc, RoomCrowd3),
                     Go(CmdPolodnie, RoomCrowd2),
                     Go(CmdWschod, RoomCrowd4), ],
            posters = [new(CmdPatrz + CmdPlakat, [TxtConcertRooms_07, TxtConcertRooms_08])],
            turn = CrowdTurn,
        },
        new() {
            number = RoomCrowd2,
            intro = [TxtConcertRooms_09],
            exits = [TxtShared_03,
                     TxtExitPolnoc, TxtPlaceTlum,
                     TxtExitWschod, TxtPlaceTlum, ],
            moves = [Go(CmdPolnoc, RoomCrowd1), Go(CmdWschod, RoomCrowd5)],
            turn = CrowdTurn,
            expand = true,
        },
        new() {
            number = RoomCrowd3,
            intro = [TxtConcertRooms_11],
            exits = [TxtShared_03,
                     TxtExitPolodnie, TxtPlaceTlum,
                     TxtExitWschod, TxtPlaceTlum,
                     TxtExitPolnoc, TxtPlacePiwiarnia, ],
            moves = [Go(CmdPolodnie, RoomCrowd1), Go(CmdWschod, RoomCrowd6), Go(CmdPolnoc, RoomPub)],
            turn = CrowdTurn,
            expand = true,
        },
        new() {
            number = RoomCrowd4,
            intro = [TxtYouAreIn + TxtConcertRooms_13, TxtConcertRooms_14],
            exits = [TxtShared_03,
                     TxtExitZachod, TxtPlaceTlum,
                     TxtExitPolnoc, TxtPlaceTlum,
                     TxtExitPolodnie, TxtPlaceTlum, ],
            moves = [Go(CmdPolnoc, RoomCrowd6), Go(CmdZachod, RoomCrowd1), Go(CmdPolodnie, RoomCrowd5)],
            turn = CrowdTurn,
            expand = true,
        },
        new() {
            number = RoomCrowd5,
            intro = [TxtConcertRooms_16, TxtConcertRooms_14],
            exits = [TxtShared_03,
                     TxtExitZachod, TxtPlaceTlum,
                     TxtExitPolnoc, TxtPlaceTlum, ],
            moves = [Go(CmdPolnoc, RoomCrowd4), Go(CmdZachod, RoomCrowd2)],
            turn = CrowdTurn,
            expand = true,
        },
        new() {
            number = RoomCrowd6,
            intro = [TxtConcertRooms_17],
            exits = [TxtShared_03,
                     TxtExitZachod, TxtPlaceTlum,
                     TxtExitPolodnie, TxtPlaceTlum, ],
            moves = [Go(CmdPolodnie, RoomCrowd4), Go(CmdZachod, RoomCrowd3)],
            turn = CrowdTurn,
            expand = true,
        },
        new() {
            number = RoomPub,
            intro = [TxtYouAreIn + TxtConcertRooms_18, TxtConcertRooms_19],
            exits = [TxtShared_03,
                     TxtExitWschod, TxtPlaceWejscieNaEstrade,
                     TxtExitPolodnie, TxtPlaceTlum, ],
            moves = [Go(CmdPolodnie, RoomCrowd3), Go(CmdWschod, RoomStageEntrance)],
            turn = entering => {
                if (entering) {
                    if (World.Monsters[Minibarman] == 67) {
                        Out.WriteLine(TxtConcertRooms_20);
                    }
                    if (World.Monsters[Fatso] == 67) {
                        Out.WriteLine(TxtConcertRooms_21);
                    }
                    DescribeCrowd();
                    return;
                }
                ConcertFights();
                switch (Verb) {
                    case CmdZabij:
                        switch (Rest) {
                            case CmdMiniBarman:
                                if (World.Monsters[Minibarman] == 67) {
                                    Out.WriteLine(TxtConcertRooms_23);
                                    Out.WriteLine(TxtConcertRooms_24);
                                    Fight(VeryHardEnemy);
                                    if (Player.Energy > 0) {
                                        World.Monsters[Minibarman] = 0;
                                        Combat.Roll = Roll(100);
                                        if (Combat.Roll < 15) {
                                            Out.WriteLine(TxtConcertRooms_25 + TxtShared_08 + TxtPiwoTail);
                                            Bag.Items[TBagItem.Beer] -= ItemUnit;
                                            Bag.Count += 1;
                                        }
                                    }
                                }
                                break;
                            case CmdGrubas:
                                if (World.Monsters[Fatso] == 67) {
                                    Fight(VeryHardEnemy);
                                    if (Player.Energy > 0) {
                                        World.Monsters[Fatso] = 0;
                                        if (Combat.Roll < 15) {
                                            Out.WriteLine(TxtConcertRooms_26 + TxtShared_08 + TxtPiwoTail);
                                            Bag.Items[TBagItem.Beer] -= ItemUnit;
                                            Bag.Count += 1;
                                        }
                                    }
                                }
                                break;
                        }
                        break;
                }
            },
            expand = true,
        },
        new() {
            number = RoomStageEntrance,
            intro = [TxtConcertRooms_27, TxtConcertRooms_28],
            exits = [TxtShared_03,
                     TxtExitZachod, TxtPlacePiwiarnia,
                     TxtExitWschod, TxtPlaceScena, ],
            moves = [Go(CmdZachod, RoomPub), Go(CmdWschod, RoomBackstage)],
            posters = [new(CmdPatrz + CmdTabliczka, [TxtConcertRooms_32])],
            turn = entering => {
                if (entering) {
                    DescribeCrowd();
                    return;
                }
                if (Verb == CmdExit) {
                    Out.WriteLine(TxtConcertRooms_31);
                }
                ConcertFights();
            },
            expand = true,
        },
        new() {
            number = RoomBackstage,
            intro = [TxtConcertRooms_33, TxtConcertRooms_34],
            exits = [TxtShared_03,
                     TxtExitZachod, TxtPlaceWejscieNaScene,
                     TxtExitPolodnie, TxtPlaceScena, ],
            moves = [Go(CmdZachod, RoomStageEntrance), Go(CmdPolodnie, RoomStage1)],
            turn = entering => {
                if (entering) {
                    if (World.Monsters[DJ] == 69) {
                        Out.WriteLine(TxtConcertRooms_35);
                    }
                    return;
                }
                if (Verb == CmdZabij && Rest == CmdDJ) {
                    if (World.Monsters[DJ] == 69) {
                        Fight(VeryHardEnemy);
                        if (Combat.EnemyEnergy < 0) {
                            World.Monsters[DJ] = 0;
                            Combat.Roll = Roll(100);
                            if (Combat.Roll < 25) {
                                Out.WriteLine(TxtShared_08 + TxtConcertRooms_38);
                                Bag.Items[TBagItem.DryRation] -= ItemUnit;
                                Bag.Count += 1;
                            }
                            if (Combat.Roll < 3) {
                                Out.WriteLine(TxtShared_08 + TxtConcertRooms_39);
                                Out.WriteLine(TxtBang + TxtUniqeItem + TxtConcertRooms_40);
                                Bag.Items[TBagItem.Cassette] -= ItemUnit;
                                Player.MaxEnergy += 5;
                                Gear.Values[TGearItem.Armor] -= 8;
                                Bag.Count += 1;
                                Player.Stats[TStat.Dexterity]++;
                            }
                        }
                    }
                }
            },
        },
        new() {
            number = RoomStage1,
            intro = [..TxtConcertRoomsLines2.Skip(1)],
            exits = [TxtShared_03,
                     TxtExitPolnoc, TxtPlaceScena,
                     TxtExitPolodnie, TxtPlaceSrodekSceny, ],
            moves = [Go(CmdPolnoc, RoomBackstage), Go(CmdPolodnie, RoomStage2)],
            turn = StageTurn,
        },
        new() {
            number = RoomStage2,
            intro = [TxtConcertRooms_46, TxtConcertRooms_47],
            exits = [TxtShared_03,
                     TxtExitPolnoc, TxtPlaceScena,
                     TxtExitPolodnie, TxtPlaceScena, ],
            moves = [Go(CmdPolnoc, RoomStage1), Go(CmdPolodnie, RoomStage3)],
            turn = StageTurn,
        },
        new() {
            number = RoomStage3,
            intro = [TxtConcertRooms_48, TxtConcertRooms_49],
            exits = [TxtShared_03,
                     TxtExitPolnoc, TxtPlaceSrodekSceny, ],
            moves = [Go(CmdPolnoc, RoomStage2)],
            turn = StageTurn,
        },
        new() {
            number = RoomDarkStreet,
            intro = [..TxtRoom75Lines1.Skip(1)],
            exits = [TxtShared_03,
                     TxtExitZachod, TxtPlaceCentrumMiasta,
                     TxtExitPolnoc, TxtPlaceBarPodDwomaPedalami,
                     TxtExitPolodnie, TxtPlaceBluszcz, ],
            moves = [Go(CmdZachod, RoomCityCentre), Go(CmdPolnoc, RoomBar), Go(CmdPolodnie, RoomIvy1)],
        },
        new() {
            number = RoomBar,
            intro = [TxtYouAreIn + TxtRoom76_01, ..TxtRoom76Lines1.Skip(1)],
            exits = [TxtShared_03,
                     TxtExitPolodnie, TxtPlaceUlicaCiemna, ],
            moves = [Go(CmdPolodnie, RoomDarkStreet)],
            turn = entering => {
                if (entering) {
                    if (World.Monsters[Maciek] == 76) {
                        Out.WriteLine(TxtRoom76_05);
                    }
                    if (World.Monsters[Pedal] == 76) {
                        Out.WriteLine(TxtRoom76_06);
                    }
                    if (World.Monsters[Couple] == 76) {
                        Out.WriteLine(TxtRoom76_07);
                    }
                    return;
                }
                switch (Verb) {
                    case CmdLista:
                        WriteLines([.. TxtRoom76Lines2.Skip(1)]);
                        break;
                    case CmdZabij:
                        switch (Rest) {
                            case CmdPedal:
                                Fight(EasyEnemy);
                                World.Monsters[Pedal] = 0;
                                PillGain();
                                break;
                            case CmdPara:
                                Fight(VeryHardEnemy);
                                World.Monsters[Couple] = 0;
                                Fight(VeryHardEnemy);
                                break;
                            case CmdMaciek:
                                Fight(EasyEnemy);
                                World.Monsters[Maciek] = 0;
                                break;
                        }
                        break;
                    case CmdMam:
                        if (Rest == Cmd18Lat) {
                            Out.WriteLine(TxtRoom76_13);
                        }
                        break;
                }
            },
        },
        new() {
            number = RoomIvy1,
            intro = [TxtRoom77_01, TxtRoom77_02],
            exits = [TxtShared_03,
                     TxtExitPolnoc, TxtPlaceUlCiemna,
                     TxtExitWschod, TxtPlaceBluszcz,
                     TxtExitZachod, TxtPlaceBluszcz,
                     TxtExitPolodnie, TxtPlaceBluszcz, ],
            moves = [Go(CmdPolnoc, RoomDarkStreet),
                     Go(CmdWschod, RoomIvy2),
                     Go(CmdZachod, RoomIvy3),
                     Go(CmdPolodnie, RoomIvy4), ],
            turn = PlantTurn,
            expand = true,
        },
        new() {
            number = RoomIvy2,
            intro = [TxtRoom78_01, TxtRoom78_02],
            exits = [TxtShared_03,
                     TxtExitZachod, TxtPlaceBluszcz, ],
            moves = [Go(CmdZachod, RoomIvy1)],
            turn = PlantTurn,
            expand = true,
        },
        new() {
            number = RoomIvy3,
            intro = [TxtRoom79_01, TxtRoom79_02],
            exits = [TxtShared_03,
                     TxtExitWschod, TxtPlaceBluszcz, ],
            moves = [Go(CmdWschod, RoomIvy1)],
            turn = PlantTurn,
            expand = true,
        },
        new() {
            number = RoomIvy4,
            intro = [..TxtRoom80Lines1.Skip(1)],
            exits = [TxtShared_03,
                     TxtExitPolnoc, TxtPlaceBluszcz,
                     TxtExitWschod, TxtPlaceBluszcz,
                     TxtExitZachod, TxtPlaceBluszcz,
                     TxtExitPolodnie, TxtPlaceBluszcz, ],
            moves = [Go(CmdPolnoc, RoomIvy1),
                     Go(CmdZachod, RoomIvy5),
                     Go(CmdWschod, RoomIvy6),
                     Go(CmdPolodnie, RoomClearing), ],
            posters = [new(CmdPatrz + CmdNapis, [TxtRoom80_04])],
            turn = entering => {
                if (entering) {
                    DescribePlants();
                    return;
                }
                PlantFights();
                if (Verb == CmdPatrz && Rest == CmdStudnia) {
                    Combat.Roll = (Roll(3) + 1);
                    Out.WriteLine(TxtRoom80_05 + Combat.Roll + TxtShared_23);
                    Player.Money += Combat.Roll;
                    Combat.Roll = Roll(100);
                    if (Combat.Roll < 10) {
                        Out.WriteLine(TxtRoom80_06);
                        if (!Player.ChangeEnergy(-40)) {
                            Session.Room = DeadRoom;
                            return;
                        }
                        Out.WriteLine(TxtRoom80_07);
                        Player.Money -= 100;
                        if (Player.Money < 0) {
                            Player.Money = 0;
                        }
                    }
                }
            },
            expand = true,
        },
        new() {
            number = RoomIvy5,
            intro = [TxtRoom81_01, TxtRoom81_02],
            exits = [TxtShared_03,
                     TxtExitWschod, TxtPlaceBluszcz, ],
            moves = [Go(CmdWschod, RoomIvy4)],
            turn = PlantTurn,
            expand = true,
        },
        new() {
            number = RoomIvy6,
            intro = [TxtRoom82_01, TxtRoom82_02],
            exits = [TxtShared_03,
                     TxtExitZachod, TxtPlaceBluszcz, ],
            moves = [Go(CmdZachod, RoomIvy4)],
            turn = PlantTurn,
            expand = true,
        },
        new() {
            number = RoomClearing,
            intro = [..TxtRoom83Lines1.Skip(1)],
            exits = [TxtShared_03,
                     TxtExitPolnoc, TxtPlaceBluszcz,
                     TxtExitZachod, TxtPlacePodejrzaneKrzaki, ],
            moves = [Go(CmdPolnoc, RoomIvy4), Go(CmdZachod, RoomCavePath)],
            turn = PlantTurn,
            expand = true,
        },
        new() {
            number = RoomCavePath,
            intro = [..TxtRoom84Lines1.Skip(1)],
            exits = [TxtShared_03,
                     TxtExitWschod, TxtPlaceBluszcz,
                     TxtExitZachod, TxtPlaceBudynek,
                     TxtExitPolodnie, TxtPlaceWejscieDoGroty, ],
            moves = [Go(CmdWschod, RoomClearing), Go(CmdZachod, RoomLivingDoor), Go(CmdPolodnie, RoomCaveMouth)],
            turn = PlantTurn,
            expand = true,
        },
        new() {
            number = RoomLivingDoor,
            intro = [TxtRoom85_01, TxtRoom85_02],
            moves = [Go(CmdWschod, RoomCavePath)],
            turn = entering => {
                if (entering) {
                    if (World.Door) {
                        Out.WriteLine(TxtRoom85_03);
                    }
                    return;
                }
                if (Verb == CmdZabij && Rest == CmdDrzwi) {
                    if (World.Door) {
                        Fight(NotEasyEnemy);
                        if (Player.Energy > 0) {
                            World.Door = false;
                        }
                    }
                }
                if (Verb == CmdExit) {
                    WriteExits([TxtShared_03,
                                TxtExitWschod, TxtPlacePodejrzaneKrzaki, ]);
                    Out.WriteLine(World.Door
                        ? TxtExitZachod + TxtPlaceGdybyNieTeCholerneDrzwi
                        : TxtExitZachod + TxtPlaceWejscieDoDomku);
                }
                if (Verb == CmdZachod) {
                    if (!World.Door) {
                        MoveTo(RoomHut);
                    } else {
                        Out.WriteLine(TxtRoom85_07);
                    }
                }
            },
            expand = true,
        },
        new() {
            number = RoomHut,
            intro = [TxtRoom86_01],
            exits = [TxtShared_03,
                     TxtExitWschod, TxtPlaceBudynek, ],
            moves = [Go(CmdWschod, RoomLivingDoor)],
            turn = entering => {
                if (entering) {
                    if (World.OldElf) {
                        Out.WriteLine(TxtRoom86_02);
                    }
                    if (!World.OldElf) {
                        Out.WriteLine(TxtRoom86_03);
                    }
                    return;
                }
                switch (Verb) {
                    case CmdPatrz:
                        if (Rest == CmdGarnki) {
                            Out.WriteLine(TxtRoom86_04);
                        }
                        break;
                    case CmdRozmawiaj:
                        if (Rest == CmdStaruch) {
                            if (Bag.Items[TBagItem.ReturnScroll] < 0) {
                                Out.WriteLine(TxtRoom86_13);
                                MoveTo(RoomCageHuge);
                            } else if ((Bag.Items[TBagItem.Sausage] > -30) && (Player.Money < StaruchPrice) && (Bag.Items[TBagItem.ReturnScroll] == 0)) {
                                WriteLines([.. TxtRoom86Lines1.Skip(1)]);
                            } else if ((Bag.Items[TBagItem.Sausage] < -30) && (Player.Money >= StaruchPrice)) {
                                WriteLines([.. TxtRoom86Lines2.Skip(1)]);
                                MoveTo(RoomCityCentre);
                                Bag.Items[TBagItem.Beer] -= ItemUnit;
                                Bag.Items[TBagItem.Sausage] += 40;
                                Player.Money -= StaruchPrice;
                                Bag.Items[TBagItem.ReturnScroll] -= ItemUnit;
                                Player.Experience += 50;
                                Bag.Count -= 2;
                            }
                        }
                        break;
                    case CmdZabij:
                        if (Rest == CmdStaruch) {
                            if (World.OldElf) {
                                Fight(WeakEnemy);
                                World.OldElf = false;
                                Out.WriteLine(TxtRoom86_14);
                                Out.WriteLine(TxtRoom86_15);
                                Player.Level -= 1;
                            }
                        }
                        break;
                }
            },
            expand = true,
        },
        new() {
            number = RoomCaveMouth,
            intro = [TxtRoom87_01, TxtRoom87_02],
            exits = [TxtShared_03,
                     TxtExitPolodnie, TxtPlaceGrota,
                     TxtExitPolnoc, TxtPlacePodejrzaneKrzaki, ],
            moves = [Go(CmdPolnoc, RoomCavePath), Go(CmdPolodnie, RoomNettleThrone)],
            turn = entering => {
                if (!entering) {
                    return;
                }
                Combat.Roll = Roll(100);
                if (Combat.Roll < 40) {
                    int damage = Math.Max(0, 25 - Player.Stats[TStat.Dexterity]);
                    Out.WriteLine(TxtRoom87_03 + damage + TxtShared_26);
                    if (!Player.ChangeEnergy(-damage)) {
                        Session.Room = DeadRoom;
                    }
                }
            },
            expand = true,
        },
        new() {
            number = RoomNettleThrone,
            intro = [..TxtRoom88Lines1.Skip(1)],
            exits = [TxtShared_03,
                     TxtExitPolodnie, TxtPlaceWejscieDoGroty2, ],
            moves = [Go(CmdPolodnie, RoomCaveMouth)],
            turn = entering => {
                if (!entering) {
                    return;
                }
                Out.Write(TxtRoom88_05);
                Command = Crt.ReadLn();
                Out.WriteLine(TxtRoom88_06);
                Combat.EnemyEnergy = 200;
                Combat.EnemyDexterity = 15;
                Combat.EnemyStrength = 18;
                Combat.FireballCharges = 5;
                Combat.PoisonCharges = 1;
                DoFight();
                if (Combat.EnemyEnergy < 0) {
                    LeafGain();
                    PillGain();
                    ScrollGain();
                    if (World.DuncanQuest != 0) {
                        World.DuncanQuest -= 50;
                    }
                }
                if (Player.Energy > 0) {
                    Out.WriteLine(TxtRoom88_07);
                    MoveTo(RoomCaveMouth);
                }
            },
            expand = true,
        },
        new() {
            number = RoomQuestMaster,
            intro = [TxtRoom100_01, TxtRoom100_02],
            moves = [Go(CmdPolodnie, RoomShopStreet2)],
            turn = entering => {
                if (entering) {
                    DescribeStreet();
                } else {
                    if (Verb == CmdExit) {
                        WriteExits([TxtShared_03,
                                    TxtExitPolodnie, TxtPlaceUlSklepikowa,
                                    TxtExitWschod, TxtPlaceDrogaOKtorejMozeszPomarzycBoJestWProdukcji, ]);
                        if (Bag.Permit == 0) {
                            Out.WriteLine(TxtExitZachod + TxtPlaceDrogaZablokowanaPrzezQuestMastera);
                        }
                        if (Bag.Permit < 0) {
                            Out.WriteLine(TxtExitZachod + TxtPlaceDroga);
                        }
                    }
                    if (Verb == CmdLista) {
                        WriteLines([.. TxtRoom100Lines1.Skip(1)]);
                    }
                    for (int i = 1; i < QuestOffers.Count; i++) {
                        TQuestOffer q = QuestOffers[i];
                        StartQuest(q.cmd, q.line, q.quest, q.kills);
                    }
                    if (Verb == CmdZakoncz && Rest == CmdQuest) {
                        if (World.Quest == 0) {
                            Out.WriteLine(TxtRoom100_18);
                        }

                        switch (World.Quest) {
                            case 1:
                                if (World.QuestKills < 1) {
                                    GrantQuestReward(1, 100);
                                } else {
                                    Out.WriteLine(TxtRoom100_19);
                                }
                                break;
                            case 2:
                                if ((World.QuestKills < 1) && World.LiroyKilled && (Bag.Items[TBagItem.Diploma] <= -ItemUnit)) {
                                    GrantQuestReward(2, 250);
                                } else {
                                    Out.WriteLine(TxtRoom100_19);
                                }
                                break;
                            case 3:
                                if ((World.QuestKills < 1) && World.LiroyKilled && (Bag.Items[TBagItem.Pipe] <= -ItemUnit)) {
                                    GrantQuestReward(3, 425);
                                } else {
                                    Out.WriteLine(TxtRoom100_19);
                                }
                                break;
                        }
                    }
                    switch (Verb) {
                        case CmdZachod:
                            if (Bag.Permit <= -ItemUnit) {
                                MoveTo(RoomRoad1);
                            }
                            if (Bag.Permit == 0) {
                                Out.WriteLine(TxtRoom100_16);
                            }
                            break;
                        case CmdZabij:
                            if (Rest == CmdQuestMaster) {
                                Combat.PoisonCharges = 10;
                                Combat.FireballCharges = 20;
                                Combat.EnemyStrength = 10;
                                Combat.EnemyDexterity = 26;
                                Combat.EnemyEnergy = 188;
                                DoFight();
                                if (Combat.EnemyEnergy < 1) {
                                    Bag.Permit -= ItemUnit;
                                }
                            }
                            break;
                    }
                }
            },
        },
        new() {
            number = RoomRoad1,
            intro = [..TxtRoom101Lines1.Skip(1)],
            exits = [TxtShared_03,
                     TxtExitWschod, TxtPlaceRozjazd,
                     TxtExitZachod, TxtPlaceDroga,
                     TxtExitPolnoc, TxtPlaceLas,
                     TxtExitPolodnie, TxtPlaceLas, ],
            moves = [Go(CmdWschod, RoomQuestMaster), Go(CmdZachod, RoomRoad2),
                     Go(CmdPolnoc, RoomForest), Go(CmdPolodnie, RoomForest1), ],
            posters = [new(CmdPatrz + CmdDrogowskaz, [TxtRoom101_05])],
        },
        new() {
            number = RoomRoad2,
            intro = [TxtRoom102_01, TxtRoom102_02],
            exits = [TxtShared_03,
                     TxtExitWschod, TxtPlaceDroga,
                     TxtExitZachod, TxtPlaceDroga,
                     TxtExitPolnoc, TxtPlaceLas,
                     TxtExitPolodnie, TxtPlaceLas, ],
            moves = [Go(CmdWschod, RoomRoad1), Go(CmdZachod, RoomRoad3),
                     Go(CmdPolnoc, RoomForest2), Go(CmdPolodnie, RoomForest3), ],
        },
        new() {
            number = RoomRoad3,
            intro = [..TxtRoom103Lines1.Skip(1)],
            exits = [TxtShared_03,
                     TxtExitWschod, TxtPlaceDroga,
                     TxtExitPolnoc, TxtPlaceLas,
                     TxtExitPolodnie, TxtPlaceLas, ],
            moves = [Go(CmdWschod, RoomRoad2),
                     Go(CmdPolnoc, RoomForest4), Go(CmdPolodnie, RoomForest5), ],
        },
        new() {
            number = RoomForest,
            intro = [TxtForest_01],
            exits = [TxtShared_03,
                     TxtExitPolodnie, TxtPlaceDroga, ],
            moves = [Go(CmdPolodnie, RoomRoad1)],
        },
        new() {
            number = RoomForest1,
            intro = [TxtForest_01],
            exits = [TxtShared_03,
                     TxtExitPolnoc, TxtPlaceDroga, ],
            moves = [Go(CmdPolnoc, RoomRoad1)],
        },
        new() {
            number = RoomForest2,
            intro = [TxtForest_01],
            exits = [TxtShared_03,
                     TxtExitPolodnie, TxtPlaceDroga, ],
            moves = [Go(CmdPolodnie, RoomRoad2)],
        },
        new() {
            number = RoomForest3,
            intro = [TxtForest_01],
            exits = [TxtShared_03,
                     TxtExitPolnoc, TxtPlaceDroga, ],
            moves = [Go(CmdPolnoc, RoomRoad2)],
        },
        new() {
            number = RoomForest4,
            intro = [TxtForest_01],
            exits = [TxtShared_03,
                     TxtExitPolodnie, TxtPlaceDroga, ],
            moves = [Go(CmdPolodnie, RoomRoad3)],
        },
        new() {
            number = RoomForest5,
            intro = [TxtForest_01],
            exits = [TxtShared_03,
                     TxtExitPolnoc, TxtPlaceDroga, ],
            moves = [Go(CmdPolnoc, RoomRoad3)],
        },
    ];

    static readonly Dictionary<int, TRoom> RoomIndex = BuildRoomIndex();

    static Dictionary<int, TRoom> BuildRoomIndex() {
        Dictionary<int, TRoom> index = [];
        for (int i = 0; i < RoomDefs.Count; i++) {
            index.Add(RoomDefs[i].number, RoomDefs[i]);
        }

        return index;
    }

    public static void DispatchRoom() {
        if (RoomIndex.TryGetValue(Session.Room, out TRoom? room)) {
            RunRoom(room);
        }
    }

    static void RunRoom(TRoom room) {
        WriteLines(room.intro);
        room.turn?.Invoke(true);
        if (room.teleport != RoomAny) {
            MoveTo(room.teleport);
            return;
        }
        do {
            ReadCommand();
            if (room.expand) {
                ExpandAbbrev();
            }

            if (room.exits.Any() && (Verb == CmdExit)) {
                WriteExits(room.exits);
            }

            if (Session.Room == room.number) {
                room.turn?.Invoke(false);
            }

            if (MoveWhenOrQuit(room.moves)) {
                return;
            }

            for (int pi = 0; pi < room.posters.Count; pi++) {
                if (Matches(room.posters[pi].cmd)) {
                    WriteLines(room.posters[pi].lines);
                }
            }
        } while (Session.Room == room.number);
    }
}
