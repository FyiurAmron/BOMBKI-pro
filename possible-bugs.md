# Possible bugs

Triaged audit of the pro version. Items completed or dismissed during
triage are not listed here. **pinned** = current tests assert the
behaviour, so a fix must re-pin them.

## Bugs

1. Portal failure does `MoveTo(Roll(100) + 1)`; rooms 19, 27-29, 58-59,
   73-74, 89-99 of 1..100 have no instance, so the game hangs
   (`Game.cs:3090`).
2. Room 104 (forest) is a destination (Road1 north) with no instance or
   texts - same hang.
3. Quest master west does `Session.Room = RoomRoad1`, bypassing
   `MoveTo`: `OuterRoom` stays 100, dropped items belong to 100,
   save/load returns to 100 (`Rooms.cs:1253`).
4. Startup load never calls `SpawnMonsters` -> no arena/street/concert/
   plant monsters (and `Door = 0`) after load.
5. `Bag.Permit` is not saved -> the west road is blocked again after
   load.
6. `World.Door` is not saved -> the living door is already destroyed
   after load.
7. `Bag.Limit` is 0 until the sheet is viewed (only `ShowCharacter`
   computes it) -> overweight drain from the start (`Game.cs:1953-1957`).
8. Death only via combat (`DeadRoom`); overload, poison, cave rocks and
   the well can take Energy to <= 0 with no death.
9. Cave-mouth rocks subtract `25 - Dex`: high Dex heals, Energy can
   exceed MaxEnergy, no clamp/death (`Rooms.cs:1180`).
10. Well `-40` Energy can go below 0 with no clamp/death
    (`Rooms.cs:1035`).
11. Heart loot outer guard `Bag.Items[Heart] == 0` blocks drops while a
    heart lies anywhere on the ground; inner `!= -ItemUnit` is
    inconsistent (`Game.cs:762`).
12. Dropping/selling stacks: `DropItem` requires exactly `-ItemUnit`
    and overwrites the whole stack into one room; several use/sell
    paths also require exactly one unit while food uses `<= -ItemUnit`.
13. Portal/compare spend mana unchecked -> negative mana, no message
    (`Game.cs:3070-3095`, `CmdCompare`).
14. `Death()` sets quest 1 kills to 50 although it starts at 75 (death
    helps); the exp penalty can go negative (`Game.cs:2438`).
15. `TxtRoom0_02` and `TxtRoom3_04` name the destination "hala główna
    mud szkoły" but both moves go to the salon (room 1; the hall is
    room 4).
16. In-game `WLACZ POSTAC` load keeps the current room; startup load
    restores `OuterRoom`.
17. Room 61 (Crowd1) has a working east move to 64 but no WSCHÓD entry
    in its EXIT list (`Rooms.cs:722-729`).

## Text and data

18. MUD casing: "mud szkoły" (prose), "DYPLOM Mud szkoły" (ground
    item), "MUD SZKOŁY" (art), "Mud szkoła" (places).
19. Level-up banner prints 4x per level (`TxtCheckLevelUpLines1`
    repeats one const 4 times; 52 occurrences in the run).
20. Duplicate constants: TxtPlacePokojCentralny/TxtPlacePok224JCentralny;
    TxtFightTailEnergii/TxtFightTailEnergi/TxtStart_04;
    TxtShared_22/TxtFightVictoryTail; CmdChleb/TxtItemChleb;
    CmdCiastko/TxtItemCiastko; CmdPaczek/TxtItemPaczek;
    CmdMudSzkola2/TxtPlaceMudSzkola2;
    CmdGarniturZKolcami/TxtShowCharacter_09;
    CmdKompletUbranFirmySyf/TxtShowCharacter_08;
    TxtTakeDrop_05/TxtBuyPipe_01; TxtTakeDrop_09/CmdZakrwawioneSerce.
21. Odd names: TxtPlacePok224JCentralny (224 artifact),
    TxtFightTailEnergi (truncated), Cmd* constants that are display
    texts (CmdGarniturZKolcami, CmdKompletUbranFirmySyf).
22. Listed exits with no move: Road1 POŁUDNIE->Las; Road2/Road3
    PÓŁNOC/POŁUDNIE->Las (intentional barricade?).
23. RoomStart and RoomMeditation EXIT print one prose line instead of
    the standard exit list, though both have exits.
24. `TxtGeneralStoreShopLines1` is unused (and omits
    TxtGeneralStoreShop_02, which is used elsewhere).
25. Quest text "ZABIJ 50 potworów w tym LIROYA" vs 200 kill units
    (Liroy = 150; the sheet prints `QuestKills - 150`); magic 150 is
    duplicated in two places.
26. Sentinel styles mixed: line lists start with "", data tables with
    `null!`; consumption mixes Skip(1) and direct indexes
    (`TxtRoom1Lines1[1..3]`).

## Vestigial / dead code

27. `RoomAny` is used only as the teleport sentinel now, no longer for
    dispatch.
28. `Session.MoveGuard` is write-only (arena set/reset, never read,
    never saved).
29. `Session.InArena` is only touched by save/load after the arena
    refactor; no gameplay reads.
30. `TSkill.CookingDisplayBug` is a save-slot-only skill (the name
    documents an old display bug).
31. Save placeholders: three `SaveSlot(0)` slots and Bread/BreadRoll
    saved/loaded twice (likely original-format compatibility - needs a
    comment).
32. `Crt.Sound`/`Crt.NoSound` are empty unused stubs.
33. `CmdJustSaved` prints the room number + " mięcho!" ("20 mięcho!") -
    a debug-looking easter egg.
34. Flavor prose lives in lists with dummy first elements (e.g.
    TxtRoom5_01..03).

## Mechanical inconsistencies

35. Abbreviation expansion is room-dependent (on: corridor, ivy, cave,
    most concert rooms; off: start, salon, school, shops, streets, bar,
    cages 12-17, stages, roads, quest master).
36. Item quantity checks mix `== -ItemUnit`, `<= -ItemUnit`, `< 0`,
    `== 0` across use/drop/sell/buy/loot.
37. `ShowCharacter` has side effects: recomputes Bag.Limit and
    Gear.ArmorRating; ArmorRating is display-only (combat uses Armor).
38. Magic numbers duplicated: Liroy = 150 (StageFights + ShowCharacter),
    quest = 200, Staruch 4 weka/200 money/-2 count, cage stats, cave
    rocks 25.
39. `Bag.Count`/`Limit` have no sanity checks.
40. Quest prices duplicated as text (TxtRoom100_05..07) and data
    (QuestOffers) - drift risk.
41. Permit bookkeeping is inconsistent: the permit is a carried item
    (buying a forged one adds a bag slot, so quest 1's reward adding one
    is right), but quests 2/3 and the quest-master kill grant it without
    a slot, and ZNISZCZ PRZEPUSTKA adds a slot instead of freeing one
    (Game.cs:1598). Permits also stack with no use for more than one.
    The pipe's +1 wisdom is capped on pickup but its hand-in penalty is
    not, so a pipe taken at max wisdom loses a point. **pinned**

## Smells / architecture

42. `Game.cs` is a ~3.3k-line monolith (combat, economy, shops, saves,
    input, RNG); `Rooms.cs` ~1.36k; `Texts.cs` ~1k.
43. Save layout is positional with bias constants and duplicated edit
    points (`SaveCharacter`/`LoadGame`) - drift-prone (see items 5/6/33).
44. Verb/rest convention (trailing space in verb consts, first-space
    split) is undocumented and fails silently; SplitCache is a mutable
    static dictionary.
45. The `Command` setter throws on null (readers currently never return
    null).
46. `ExpandAbbrev` allocates two lists per command; `DebugEnabled` reads
    the environment on every command.
47. `TRoom.turn` is `Action<bool>` with undocumented "entering"
    semantics; some lambdas peek at `Session.Room`.
48. Data/graph divergence has no validation (e.g. items 16, 18, 23).
