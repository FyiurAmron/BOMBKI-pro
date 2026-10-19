# Possible bugs

Triaged audit of the pro version. Items completed or dismissed during
triage are not listed here. **pinned** = current tests assert the
behaviour, so a fix must re-pin them.

## Bugs

1. Heart loot outer guard `Bag.Items[Heart] == 0` blocks drops while a
   heart lies anywhere on the ground; inner `!= -ItemUnit` is
   inconsistent (`Game.cs:758`).
2. Dropping/selling stacks: `DropItem` requires exactly `-ItemUnit`
   and overwrites the whole stack into one room; several use/sell
   paths also require exactly one unit while food uses `<= -ItemUnit`.
3. Portal/compare spend mana unchecked -> negative mana, no message
    (`Game.cs:2995-3028`, `CmdCompare`).
4. `TxtRoom0_02` and `TxtRoom3_04` name the destination "hala główna
    Szkoły Życia" but both moves go to the salon (room 1; the hall is
    room 4).

## Text and data

5. `TxtGeneralStoreShopLines1` is unused (and omits
    TxtGeneralStoreShop_02, which is used elsewhere).
6. Quest text "ZABIJ 50 potworów w tym LIROYA" vs 200 kill units
    (Liroy = 150; the sheet prints `QuestKills - 150`); magic 150 is
    duplicated in two places.
7. Sentinel styles mixed: line lists start with "", data tables with
    `null!`; consumption mixes Skip(1) and direct indexes
    (`TxtRoom1Lines1[1..3]`).

## Vestigial / dead code

8. `RoomAny` is used only as the teleport sentinel now, no longer for
    dispatch.
9. `Crt.Sound`/`Crt.NoSound` are empty unused stubs.
10. Flavor prose lives in lists with dummy first elements (e.g.
    TxtRoom5_01..03).

## Mechanical inconsistencies

11. Abbreviation expansion is room-dependent (on: corridor, ivy, cave,
    most concert rooms; off: start, salon, school, shops, streets, bar,
    cages 12-17, stages, roads, quest master).
12. Item quantity checks mix `== -ItemUnit`, `<= -ItemUnit`, `< 0`,
    `== 0` across use/drop/sell/buy/loot.
13. `ShowCharacter` has side effects: recomputes Bag.Limit and
    Gear.ArmorRating; ArmorRating is display-only (combat uses Armor).
14. Magic numbers duplicated: Liroy = 150 (StageFights + ShowCharacter),
    quest = 200, Staruch 4 weka/200 money/-2 count, cage stats, cave
    rocks 25.
15. `Bag.Count`/`Limit` have no sanity checks.
16. Quest prices duplicated as text (TxtRoom100_05..07) and data
    (QuestOffers) - drift risk.
17. Permit bookkeeping is inconsistent: the permit is a carried item
    (buying a forged one adds a bag slot, so quest 1's reward adding one
    is right), but quests 2/3 and the quest-master kill grant it without
    a slot, and ZNISZCZ PRZEPUSTKA adds a slot instead of freeing one
    (Game.cs:1598). Permits also stack with no use for more than one.
    The pipe's +1 wisdom is capped on pickup but its hand-in penalty is
    not, so a pipe taken at max wisdom loses a point. **pinned**

## Smells / architecture

18. `Game.cs` is a ~3.3k-line monolith (combat, economy, shops, saves,
    input, RNG); `Rooms.cs` ~1.36k; `Texts.cs` ~1k.
19. Verb/rest convention (trailing space in verb consts, first-space
    split) is undocumented and fails silently; SplitCache is a mutable
    static dictionary.
20. The `Command` setter throws on null (readers currently never return
    null).
21. `ExpandAbbrev` allocates two lists per command; `DebugEnabled` reads
    the environment on every command.
22. `TRoom.turn` is `Action<bool>` with undocumented "entering"
    semantics; some lambdas peek at `Session.Room`.
23. Data/graph divergence has no validation (e.g. item 4).
