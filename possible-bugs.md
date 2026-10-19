# Possible bugs

Triaged audit of the pro version. Items completed or dismissed during
triage are not listed here. **pinned** = current tests assert the
behaviour, so a fix must re-pin them.

## Bugs

1. Portal failure does `MoveTo(Roll(100) + 1)`; rooms 19, 27-29, 58-59,
   73-74, 89-99 of 1..100 have no instance, so the game hangs
   (`Game.cs:3024`).
2. Room 104 (forest) is a destination (Road1 north) with no instance or
   texts - same hang.
3. Quest master west does `Session.Room = RoomRoad1`, bypassing
   `MoveTo`: `OuterRoom` stays 100, so dropped items are tagged room 100
   and ground items in 101 are not shown (`Rooms.cs:1246`).
4. `Bag.Limit` is 0 until the sheet is viewed (only `ShowCharacter`
   computes it) -> overweight drain from the start (`Game.cs:1950`).
5. Heart loot outer guard `Bag.Items[Heart] == 0` blocks drops while a
   heart lies anywhere on the ground; inner `!= -ItemUnit` is
   inconsistent (`Game.cs:758`).
6. Dropping/selling stacks: `DropItem` requires exactly `-ItemUnit`
   and overwrites the whole stack into one room; several use/sell
   paths also require exactly one unit while food uses `<= -ItemUnit`.
7. Portal/compare spend mana unchecked -> negative mana, no message
    (`Game.cs:2995-3028`, `CmdCompare`).
8. `Death()` sets quest 1 kills to 50 although it starts at 75 (death
    helps); the exp penalty can go negative (`Game.cs:2372`).
9. `TxtRoom0_02` and `TxtRoom3_04` name the destination "hala główna
    mud szkoły" but both moves go to the salon (room 1; the hall is
    room 4).
10. Room 61 (Crowd1) has a working east move to 64 but no WSCHÓD entry
    in its EXIT list (`Rooms.cs:714-719`).

## Text and data

11. MUD casing: "mud szkoły" (prose), "DYPLOM Mud szkoły" (ground
    item), "MUD SZKOŁY" (art), "Mud szkoła" (places).
12. Level-up banner prints 4x per level (`TxtCheckLevelUpLines1`
    repeats one const 4 times; 52 occurrences in the run).
13. Listed exits with no move: Road1 POŁUDNIE->Las; Road2/Road3
    PÓŁNOC/POŁUDNIE->Las (intentional barricade?).
14. RoomStart and RoomMeditation EXIT print one prose line instead of
    the standard exit list, though both have exits.
15. `TxtGeneralStoreShopLines1` is unused (and omits
    TxtGeneralStoreShop_02, which is used elsewhere).
16. Quest text "ZABIJ 50 potworów w tym LIROYA" vs 200 kill units
    (Liroy = 150; the sheet prints `QuestKills - 150`); magic 150 is
    duplicated in two places.
17. Sentinel styles mixed: line lists start with "", data tables with
    `null!`; consumption mixes Skip(1) and direct indexes
    (`TxtRoom1Lines1[1..3]`).

## Vestigial / dead code

18. `RoomAny` is used only as the teleport sentinel now, no longer for
    dispatch.
19. `Crt.Sound`/`Crt.NoSound` are empty unused stubs.
20. Flavor prose lives in lists with dummy first elements (e.g.
    TxtRoom5_01..03).

## Mechanical inconsistencies

21. Abbreviation expansion is room-dependent (on: corridor, ivy, cave,
    most concert rooms; off: start, salon, school, shops, streets, bar,
    cages 12-17, stages, roads, quest master).
22. Item quantity checks mix `== -ItemUnit`, `<= -ItemUnit`, `< 0`,
    `== 0` across use/drop/sell/buy/loot.
23. `ShowCharacter` has side effects: recomputes Bag.Limit and
    Gear.ArmorRating; ArmorRating is display-only (combat uses Armor).
24. Magic numbers duplicated: Liroy = 150 (StageFights + ShowCharacter),
    quest = 200, Staruch 4 weka/200 money/-2 count, cage stats, cave
    rocks 25.
25. `Bag.Count`/`Limit` have no sanity checks.
26. Quest prices duplicated as text (TxtRoom100_05..07) and data
    (QuestOffers) - drift risk.
27. Permit bookkeeping is inconsistent: the permit is a carried item
    (buying a forged one adds a bag slot, so quest 1's reward adding one
    is right), but quests 2/3 and the quest-master kill grant it without
    a slot, and ZNISZCZ PRZEPUSTKA adds a slot instead of freeing one
    (Game.cs:1598). Permits also stack with no use for more than one.
    The pipe's +1 wisdom is capped on pickup but its hand-in penalty is
    not, so a pipe taken at max wisdom loses a point. **pinned**

## Smells / architecture

28. `Game.cs` is a ~3.3k-line monolith (combat, economy, shops, saves,
    input, RNG); `Rooms.cs` ~1.36k; `Texts.cs` ~1k.
29. Verb/rest convention (trailing space in verb consts, first-space
    split) is undocumented and fails silently; SplitCache is a mutable
    static dictionary.
30. The `Command` setter throws on null (readers currently never return
    null).
31. `ExpandAbbrev` allocates two lists per command; `DebugEnabled` reads
    the environment on every command.
32. `TRoom.turn` is `Action<bool>` with undocumented "entering"
    semantics; some lambdas peek at `Session.Room`.
33. Data/graph divergence has no validation (e.g. items 10, 11, 14).
