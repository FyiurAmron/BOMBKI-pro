
using static Game;
using static Game.TMonster;
using static Game.TFoe;
using static Game.TStreetKind;
using static Texts;

public static class Rooms {

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
    public const int RoomFutureRoad = 110;

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
        public string name = "";
        public List<string> intro = [];
        public List<TMovement> moves = [];
        public List<TRoomPoster> posters = [];
        public bool expand = false;
        public bool maze = false;
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
        Bag.Permit += 1;
        switch (quest) {
            case 1:
                Bag.Count += 1;
                break;
            case 2:
                Unequip(TBagItem.Diploma);
                Bag.Items[TBagItem.Diploma] -= 1;
                Bag.Count -= 1;
                Player.MaxEnergy -= 5;
                break;
            case 3:
                Unequip(TBagItem.Pipe);
                Bag.Count -= 1;
                Player.Stats[TStat.Wisdom]--;
                Bag.Items[TBagItem.Pipe] -= 1;
                break;
        }
    }

    static TRoom SideRoom(int number, int dest, string direction, string name) =>
        new() {
            number = number,
            name = name,
            intro = [TxtYouAreIn + TxtSimpleRoom_01],
            moves = [Go(direction, dest)],
        };

    static TRoom ArenaRoom(int number) =>
        new() {
            number = number,
            name = $"{TxtPlaceArena} {number - RoomArena33 + 1}",
            intro = [TxtArenaMaze_01],
            maze = true,
            turn = ArenaTurn,
            expand = true,
        };

    public static readonly List<TRoom> RoomDefs = [
        new() {
            number = RoomStart,
            name = TxtPlaceTamGdzieZaczynaszGre,
            intro = [TxtRoom0_01],
            moves = [Go(CmdPolnoc, RoomSalon)],
        },
        new() {
            number = RoomSalon,
            name = TxtPlaceSalon,
            intro = [TxtYouAreIn + TxtRoom1_01, TxtRoom1Lines1[1], TxtRoom1Lines1[2], TxtRoom1Lines1[3]],
            moves = [Go(CmdPolodnie, RoomStart),
                     Go(CmdWschod, RoomTraining),
                     Go(CmdDol, RoomMeditation),
                     Go(CmdZachod, RoomSchoolHall),
                     Go(CmdPolnoc, RoomCityCentre), ],
            posters = [new(CmdPatrz + CmdPlakat, [..TxtRoom1Lines2.Skip(1)])],
        },
        new() {
            number = RoomTraining,
            name = TxtPlacePokojTreningowy1,
            intro = [TxtYouAreIn + TxtRoom2_01, TxtRoom2_02, TxtShared_04],
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
            name = TxtPlacePoddziemnyPokojTreningowy2,
            intro = [..TxtRoom3Lines1.Skip(1)],
            moves = [Go(CmdGora, RoomSalon)],
            turn = entering => {
                if (!entering) {
                    Training();
                }
            },
        },
        new() {
            number = RoomSchoolHall,
            name = TxtPlaceSzkolaZycia,
            intro = [TxtYouAreIn + TxtRoom4_01, TxtRoom4_02],
            moves = [Go(CmdWschod, RoomSalon), Go(CmdZachod, RoomLost)],
            posters = [new(CmdPatrz + CmdAfisz, [..TxtRoom4Lines1.Skip(1)])],
        },
        new() {
            number = RoomLost,
            name = TxtPlacePokojOrientacyjny,
            intro = [..TxtRoom5Lines1.Skip(1)],
            moves = [Go(CmdWschod, RoomSchoolHall),
                     Go(CmdZachod, RoomSchoolSide1),
                     Go(CmdPolnoc, RoomSchoolSide2),
                     Go(CmdPolodnie, RoomSchoolSide3),
                     Go(CmdGora, RoomSchoolLanding),
                     Go(CmdDol, RoomSchoolSide4), ],
            posters = [new(CmdPatrz + CmdPlakat, [TxtRoom5_10, TxtRoom5_11])],
        },
        SideRoom(RoomSchoolSide1, RoomLost, CmdWschod, $"{TxtPlacePokoj} 1"),
        SideRoom(RoomSchoolSide3, RoomLost, CmdPolnoc, $"{TxtPlacePokoj} 3"),
        SideRoom(RoomSchoolSide2, RoomLost, CmdPolodnie, $"{TxtPlacePokoj} 2"),
        SideRoom(RoomSchoolSide4, RoomLost, CmdGora, $"{TxtPlacePokoj} 5"),
        new() {
            number = RoomSchoolLanding,
            name = $"{TxtPlacePokoj} 4",
            intro = [TxtYouAreIn + TxtRoom9_01, TxtRoom9_02],
            moves = [Go(CmdDol, RoomLost), Go(CmdGora, RoomCageCorridor)],
        },
        new() {
            number = RoomCageCorridor,
            name = TxtPlaceWejsciaDoKlatek,
            intro = [TxtYouAreIn + TxtRoom11_01, ..TxtRoom11Lines1.Skip(1)],
            moves = [Go(CmdWschod, RoomCageWeakling),
                     Go(CmdZachod, RoomCageStrong),
                     Go(CmdPolnoc, RoomCageAgile),
                     Go(CmdPolodnie, RoomCageTough),
                     Go(CmdGora, RoomCageHuge),
                     Go(CmdDol, RoomSchoolLanding), ],
            posters = [new(CmdPatrz + CmdPlakat, [..TxtRoom11Lines2.Skip(1)])],
            expand = true,
        },
        new() {
            number = RoomCageWeakling,
            name = TxtPlaceKlatkaCieniasa,
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
            name = TxtPlaceKlatkaSilnego,
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
            name = TxtPlaceKlatkaZrecznego,
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
            name = TxtPlaceKlatkaOdpornego,
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
            name = TxtPlaceKlatkaSilnegoOdpornegoIZrecznego,
            moves = [Go(CmdDol, RoomCageCorridor), Go(CmdGora, RoomCageReward)],
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
            name = TxtPlaceJedynaDrogaNieProwadzacaDoKlatek,
            intro = [TxtYouAreIn + TxtRoom17_01],
            moves = [Go(CmdDol, RoomCageHuge)],
            turn = entering => {
                if (entering) {
                    return;
                }
                if (Verb == CmdPatrz && Rest == CmdPlakat) {
                    WriteLines([.. TxtRoom17Lines1.Skip(1)]);
                    if ((World.Monsters[CageWeakling] == 0) && (World.Monsters[CageAgile] == 0) &&
                    (World.Monsters[CageStrong] == 0) && (World.Monsters[CageTough] == 0) &&
                    (World.Monsters[CageHuge] == 0) && (Bag.Items[TBagItem.Diploma] == 0) &&
                    !Bag.HasGroundItem(TBagItem.Diploma)) {
                        Bag.AddGroundItem(TBagItem.Diploma, RoomCageReward);
                    }
                }
            },
        },
        new() {
            number = RoomCityCentre,
            name = TxtPlaceCentrumMiasta,
            intro = [TxtYouAreIn + TxtRoom20_01],
            moves = [Go(CmdPolodnie, RoomSalon),
                     Go(CmdPolnoc, RoomShopStreet),
                     Go(CmdZachod, RoomLongStreet),
                     Go(CmdWschod, RoomDarkStreet), ],
            posters = [new(CmdPatrz + CmdPlakat, [TxtRoom20_02])],
            turn = StreetTurn,
        },
        new() {
            number = RoomShopStreet,
            name = TxtPlaceUlSklepikowa,
            intro = [TxtRoom21_01, TxtRoom21_02],
            moves = [Go(CmdPolodnie, RoomCityCentre),
                     Go(CmdPolnoc, RoomShopStreet2),
                     Go(CmdWschod, RoomBakery),
                     Go(CmdZachod, RoomArmoury), ],
            turn = StreetTurn,
        },
        new() {
            number = RoomShopStreet2,
            name = TxtPlaceUlSklepikowa2,
            intro = [TxtRoom22_01, TxtRoom22_02],
            moves = [Go(CmdPolodnie, RoomShopStreet),
                     Go(CmdWschod, RoomGeneralStore),
                     Go(CmdZachod, RoomMagicShop),
                     Go(CmdPolnoc, RoomQuestMaster), ],
            turn = StreetTurn,
        },
        new() {
            number = RoomBakery,
            name = TxtPlacePiekarnia,
            intro = [TxtYouAreIn + TxtRoom23_01, TxtRoom23_02, TxtRoom23_03],
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
            name = TxtPlaceZbrojownia,
            intro = [TxtYouAreIn + TxtRoom24_01, TxtRoom24_02, TxtRoom24_03],
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
            name = TxtPlaceSklepWielobranzowy,
            intro = [TxtRoom25_01, TxtRoom25_02],
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
            name = TxtPlaceSklepMagiczny,
            intro = [TxtRoom26_01],
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
            name = TxtPlaceUlDluga,
            intro = [TxtRoom30_01, TxtRoom30_02],
            moves = [Go(CmdWschod, RoomCityCentre),
                     Go(CmdZachod, RoomLongStreet2),
                     Go(CmdPolnoc, RoomArenaEntrance), ],
        },
        new() {
            number = RoomLongStreet2,
            name = TxtPlaceUlDluga2,
            intro = [TxtRoom31_01, TxtRoom31_02],
            moves = [Go(CmdWschod, RoomLongStreet), Go(CmdPolnoc, RoomValley)],
        },
        new() {
            number = RoomArenaEntrance,
            name = TxtPlaceWejscieNaArene,
            intro = [TxtRoom32_01, TxtRoom32_02],
            moves = [Go(CmdPolodnie, RoomLongStreet), Go(CmdPolnoc, RoomArena33)],
            posters = [new(CmdPatrz + CmdPlakat, [TxtRoom32_03, TxtRoom32_04])],
        },
        ArenaRoom(RoomArena33),
        ArenaRoom(RoomArena34),
        ArenaRoom(RoomArena35),
        ArenaRoom(RoomArena36),
        ArenaRoom(RoomArena37),
        ArenaRoom(RoomArena38),
        ArenaRoom(RoomArena39),
        ArenaRoom(RoomArena40),
        ArenaRoom(RoomArena41),
        ArenaRoom(RoomArena42),
        ArenaRoom(RoomArena43),
        ArenaRoom(RoomArena44),
        ArenaRoom(RoomArena45),
        ArenaRoom(RoomArena46),
        ArenaRoom(RoomArena47),
        ArenaRoom(RoomArena48),
        ArenaRoom(RoomArena49),
        ArenaRoom(RoomArena50),
        ArenaRoom(RoomArena51),
        ArenaRoom(RoomArena52),
        ArenaRoom(RoomArena53),
        ArenaRoom(RoomArena54),
        ArenaRoom(RoomArena55),
        ArenaRoom(RoomArena56),
        ArenaRoom(RoomArena57),
        new() {
            number = RoomValley,
            name = TxtPlaceDolinaRozrywek,
            intro = [TxtYouAreIn + TxtRoom60_01, TxtRoom60_02, TxtRoom60_03],
            moves = [Go(CmdPolodnie, RoomLongStreet2), Go(CmdWschod, RoomCrowd1)],
        },
        new() {
            number = RoomCrowd1,
            name = $"{TxtPlaceTlum} 1",
            intro = [..TxtConcertRoomsLines1.Skip(1)],
            moves = [Go(CmdZachod, RoomValley),
                     Go(CmdPolnoc, RoomCrowd3),
                     Go(CmdPolodnie, RoomCrowd2),
                     Go(CmdWschod, RoomCrowd4), ],
            posters = [new(CmdPatrz + CmdPlakat, [TxtConcertRooms_07, TxtConcertRooms_08])],
            turn = CrowdTurn,
        },
        new() {
            number = RoomCrowd2,
            name = $"{TxtPlaceTlum} 2",
            intro = [TxtConcertRooms_09],
            moves = [Go(CmdPolnoc, RoomCrowd1), Go(CmdWschod, RoomCrowd5)],
            turn = CrowdTurn,
            expand = true,
        },
        new() {
            number = RoomCrowd3,
            name = $"{TxtPlaceTlum} 3",
            intro = [TxtConcertRooms_11],
            moves = [Go(CmdPolodnie, RoomCrowd1), Go(CmdWschod, RoomCrowd6), Go(CmdPolnoc, RoomPub)],
            turn = CrowdTurn,
            expand = true,
        },
        new() {
            number = RoomCrowd4,
            name = $"{TxtPlaceTlum} 4",
            intro = [TxtYouAreIn + TxtConcertRooms_13, TxtConcertRooms_14],
            moves = [Go(CmdPolnoc, RoomCrowd6), Go(CmdZachod, RoomCrowd1), Go(CmdPolodnie, RoomCrowd5)],
            turn = CrowdTurn,
            expand = true,
        },
        new() {
            number = RoomCrowd5,
            name = $"{TxtPlaceTlum} 5",
            intro = [TxtConcertRooms_16, TxtConcertRooms_14],
            moves = [Go(CmdPolnoc, RoomCrowd4), Go(CmdZachod, RoomCrowd2)],
            turn = CrowdTurn,
            expand = true,
        },
        new() {
            number = RoomCrowd6,
            name = $"{TxtPlaceTlum} 6",
            intro = [TxtConcertRooms_17],
            moves = [Go(CmdPolodnie, RoomCrowd4), Go(CmdZachod, RoomCrowd3)],
            turn = CrowdTurn,
            expand = true,
        },
        new() {
            number = RoomPub,
            name = TxtPlacePiwiarnia,
            intro = [TxtYouAreIn + TxtConcertRooms_18, TxtConcertRooms_19],
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
                                            Bag.Items[TBagItem.Beer] += 1;
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
                                            Bag.Items[TBagItem.Beer] += 1;
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
            name = TxtPlaceWejscieNaScene,
            intro = [TxtConcertRooms_27, TxtConcertRooms_28],
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
            name = TxtPlaceZaKulisami,
            intro = [TxtConcertRooms_33, TxtConcertRooms_34],
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
                                Bag.Items[TBagItem.DryRation] += 1;
                                Bag.Count += 1;
                            }
                            if (Combat.Roll < 3) {
                                Out.WriteLine(TxtShared_08 + TxtConcertRooms_39);
                                Out.WriteLine(TxtBang + TxtUniqeItem + TxtConcertRooms_40);
                                Bag.Items[TBagItem.Cassette] += 1;
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
            name = $"{TxtPlaceScena} 1",
            intro = [..TxtConcertRoomsLines2.Skip(1)],
            moves = [Go(CmdPolnoc, RoomBackstage), Go(CmdPolodnie, RoomStage2)],
            turn = StageTurn,
        },
        new() {
            number = RoomStage2,
            name = TxtPlaceSrodekSceny,
            intro = [TxtConcertRooms_46, TxtConcertRooms_47],
            moves = [Go(CmdPolnoc, RoomStage1), Go(CmdPolodnie, RoomStage3)],
            turn = StageTurn,
        },
        new() {
            number = RoomStage3,
            name = $"{TxtPlaceScena} 3",
            intro = [TxtConcertRooms_48, TxtConcertRooms_49],
            moves = [Go(CmdPolnoc, RoomStage2)],
            turn = StageTurn,
        },
        new() {
            number = RoomDarkStreet,
            name = TxtPlaceUlCiemna,
            intro = [..TxtRoom75Lines1.Skip(1)],
            moves = [Go(CmdZachod, RoomCityCentre), Go(CmdPolnoc, RoomBar), Go(CmdPolodnie, RoomIvy1)],
        },
        new() {
            number = RoomBar,
            name = TxtPlaceBarPodDwomaPedalami,
            intro = [TxtYouAreIn + TxtRoom76_01, ..TxtRoom76Lines1.Skip(1)],
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
            name = $"{TxtPlaceBluszcz} 1",
            intro = [TxtRoom77_01, TxtRoom77_02],
            moves = [Go(CmdPolnoc, RoomDarkStreet),
                     Go(CmdWschod, RoomIvy2),
                     Go(CmdZachod, RoomIvy3),
                     Go(CmdPolodnie, RoomIvy4), ],
            turn = PlantTurn,
            expand = true,
        },
        new() {
            number = RoomIvy2,
            name = $"{TxtPlaceBluszcz} 2",
            intro = [TxtRoom78_01, TxtRoom78_02],
            moves = [Go(CmdZachod, RoomIvy1)],
            turn = PlantTurn,
            expand = true,
        },
        new() {
            number = RoomIvy3,
            name = $"{TxtPlaceBluszcz} 3",
            intro = [TxtRoom79_01, TxtRoom79_02],
            moves = [Go(CmdWschod, RoomIvy1)],
            turn = PlantTurn,
            expand = true,
        },
        new() {
            number = RoomIvy4,
            name = $"{TxtPlaceBluszcz} 4",
            intro = [..TxtRoom80Lines1.Skip(1)],
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
            name = $"{TxtPlaceBluszcz} 5",
            intro = [TxtRoom81_01, TxtRoom81_02],
            moves = [Go(CmdWschod, RoomIvy4)],
            turn = PlantTurn,
            expand = true,
        },
        new() {
            number = RoomIvy6,
            name = $"{TxtPlaceBluszcz} 6",
            intro = [TxtRoom82_01, TxtRoom82_02],
            moves = [Go(CmdZachod, RoomIvy4)],
            turn = PlantTurn,
            expand = true,
        },
        new() {
            number = RoomClearing,
            name = TxtPlacePolana,
            intro = [..TxtRoom83Lines1.Skip(1)],
            moves = [Go(CmdPolnoc, RoomIvy4), Go(CmdZachod, RoomCavePath)],
            turn = PlantTurn,
            expand = true,
        },
        new() {
            number = RoomCavePath,
            name = TxtPlacePodejrzaneKrzaki,
            intro = [..TxtRoom84Lines1.Skip(1)],
            moves = [Go(CmdWschod, RoomClearing), Go(CmdZachod, RoomLivingDoor), Go(CmdPolodnie, RoomCaveMouth)],
            turn = PlantTurn,
            expand = true,
        },
        new() {
            number = RoomLivingDoor,
            name = TxtPlaceBudynek,
            intro = [TxtRoom85_01, TxtRoom85_02],
            moves = [Go(CmdWschod, RoomCavePath),
                     Go(CmdZachod, RoomHut, () => !World.Door, TxtRoom85_07), ],
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

            },
            expand = true,
        },
        new() {
            number = RoomHut,
            name = TxtPlaceDomek,
            intro = [TxtRoom86_01],
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
                            if (Bag.Items[TBagItem.ReturnScroll] > 0) {
                                Out.WriteLine(TxtRoom86_13);
                                MoveTo(RoomCageHuge);
                            } else if ((Bag.Items[TBagItem.Sausage] < 3) && (Player.Money < StaruchPrice) && (Bag.Items[TBagItem.ReturnScroll] == 0)) {
                                WriteLines([.. TxtRoom86Lines1.Skip(1)]);
                            } else if ((Bag.Items[TBagItem.Sausage] > 3) && (Player.Money >= StaruchPrice)) {
                                WriteLines([.. TxtRoom86Lines2.Skip(1)]);
                                MoveTo(RoomCityCentre);
                                Bag.Items[TBagItem.Beer] += 1;
                                Bag.Items[TBagItem.Sausage] -= 4;
                                Player.Money -= StaruchPrice;
                                Bag.Items[TBagItem.ReturnScroll] += 1;
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
            name = TxtPlaceWejscieDoGroty,
            intro = [TxtRoom87_01, TxtRoom87_02],
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
            name = TxtPlaceGrota,
            intro = [..TxtRoom88Lines1.Skip(1)],
            moves = [Go(CmdPolnoc, RoomCaveMouth)],
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
            name = TxtPlaceRozjazd,
            intro = [TxtRoom100_01, TxtRoom100_02],
            moves = [Go(CmdPolodnie, RoomShopStreet2),
                     Go(CmdWschod, RoomFutureRoad),
                     Go(CmdZachod, RoomRoad1, () => Bag.Permit > 0, TxtRoom100_16), ],
            turn = entering => {
                if (entering) {
                    DescribeStreet();
                } else {
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
                                if ((World.QuestKills < 1) && World.LiroyKilled && (Bag.Items[TBagItem.Diploma] > 0)) {
                                    GrantQuestReward(2, 250);
                                } else {
                                    Out.WriteLine(TxtRoom100_19);
                                }
                                break;
                            case 3:
                                if ((World.QuestKills < 1) && World.LiroyKilled && (Bag.Items[TBagItem.Pipe] > 0)) {
                                    GrantQuestReward(3, 425);
                                } else {
                                    Out.WriteLine(TxtRoom100_19);
                                }
                                break;
                        }
                    }
                    switch (Verb) {
                        case CmdZabij:
                            if (Rest == CmdQuestMaster) {
                                Combat.PoisonCharges = 10;
                                Combat.FireballCharges = 20;
                                Combat.EnemyStrength = 10;
                                Combat.EnemyDexterity = 26;
                                Combat.EnemyEnergy = 188;
                                DoFight();
                                if (Combat.EnemyEnergy < 1) {
                                    Bag.Permit += 1;
                                }
                            }
                            break;
                    }
                }
            },
        },
        new() {
            number = RoomRoad1,
            name = $"{TxtPlaceDroga} 1",
            intro = [..TxtRoom101Lines1.Skip(1)],
            moves = [Go(CmdWschod, RoomQuestMaster), Go(CmdZachod, RoomRoad2),
                     Go(CmdPolnoc, RoomForest), Go(CmdPolodnie, RoomForest1), ],
            posters = [new(CmdPatrz + CmdDrogowskaz, [TxtRoom101_05])],
        },
        new() {
            number = RoomRoad2,
            name = $"{TxtPlaceDroga} 2",
            intro = [TxtRoom102_01, TxtRoom102_02],
            moves = [Go(CmdWschod, RoomRoad1), Go(CmdZachod, RoomRoad3),
                     Go(CmdPolnoc, RoomForest2), Go(CmdPolodnie, RoomForest3), ],
        },
        new() {
            number = RoomRoad3,
            name = $"{TxtPlaceDroga} 3",
            intro = [..TxtRoom103Lines1.Skip(1)],
            moves = [Go(CmdWschod, RoomRoad2),
                     Go(CmdPolnoc, RoomForest4), Go(CmdPolodnie, RoomForest5), ],
        },
        new() {
            number = RoomFutureRoad,
            name = TxtPlaceDrogaWProdukcji,
            intro = [TxtRoom110_01],
            moves = [Go(CmdZachod, RoomQuestMaster)],
        },
        new() {
            number = RoomForest,
            name = $"{TxtPlaceLas} 1",
            intro = [TxtForest_01],
            moves = [Go(CmdPolodnie, RoomRoad1)],
        },
        new() {
            number = RoomForest1,
            name = $"{TxtPlaceLas} 2",
            intro = [TxtForest_01],
            moves = [Go(CmdPolnoc, RoomRoad1)],
        },
        new() {
            number = RoomForest2,
            name = $"{TxtPlaceLas} 3",
            intro = [TxtForest_01],
            moves = [Go(CmdPolodnie, RoomRoad2)],
        },
        new() {
            number = RoomForest3,
            name = $"{TxtPlaceLas} 4",
            intro = [TxtForest_01],
            moves = [Go(CmdPolnoc, RoomRoad2)],
        },
        new() {
            number = RoomForest4,
            name = $"{TxtPlaceLas} 5",
            intro = [TxtForest_01],
            moves = [Go(CmdPolodnie, RoomRoad3)],
        },
        new() {
            number = RoomForest5,
            name = $"{TxtPlaceLas} 6",
            intro = [TxtForest_01],
            moves = [Go(CmdPolnoc, RoomRoad3)],
        },
    ];

    static readonly Dictionary<int, TRoom> RoomIndex = BuildRoomIndex();

    static readonly List<(TMazeDir dir, string label)> MazeExitLabels = [
        (TMazeDir.mdPolodnie, TxtExitPolodnie),
        (TMazeDir.mdPolnoc, TxtExitPolnoc),
        (TMazeDir.mdWschod, TxtExitWschod),
        (TMazeDir.mdZachod, TxtExitZachod),
    ];

    static Dictionary<int, TRoom> BuildRoomIndex() =>
        RoomDefs.ToDictionary(room => room.number);

    static string ExitLabel(string cmd) =>
        cmd switch {
            CmdPolnoc => TxtExitPolnoc,
            CmdPolodnie => TxtExitPolodnie,
            CmdWschod => TxtExitWschod,
            CmdZachod => TxtExitZachod,
            CmdGora => TxtExitGora,
            CmdDol => TxtExitDol,
            _ => TxtEmpty,
        };

    static string RoomName(int number) =>
        RoomIndex.TryGetValue(number, out TRoom? room) ? room.name : TxtEmpty;

    static void WriteMazeExits(int number) {
        TMazeMove? maze = MazeMoves.FirstOrDefault(move => move?.room == number);
        if (maze == null) {
            return;
        }
        Out.WriteLine(TxtShared_03);
        foreach ((TMazeDir dir, string label) in MazeExitLabels) {
            int dest = maze.dest[dir];
            if (dest != number) {
                Out.Write(label);
                Out.WriteLine(RoomName(dest));
            }
        }
    }

    static void WriteExits(TRoom room) {
        if (room.maze) {
            WriteMazeExits(room.number);
            return;
        }
        if (!room.moves.Any()) {
            return;
        }
        Out.WriteLine(TxtShared_03);
        foreach (TMovement move in room.moves) {
            Out.Write(ExitLabel(move.cmd));
            Out.WriteLine(RoomName(move.dest));
        }
    }

    public static void DispatchRoom() {
        if (RoomIndex.TryGetValue(Session.Room, out TRoom? room)) {
            RunRoom(room);
        }
    }

    static void RunRoom(TRoom room) {
        WriteLines(room.intro);
        room.turn?.Invoke(true);
        do {
            ReadCommand();
            if (room.expand) {
                ExpandAbbrev();
            }

            if (Verb == CmdExit) {
                WriteExits(room);
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
