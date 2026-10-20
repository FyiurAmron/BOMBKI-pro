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
3. `TxtRoom0_02` and `TxtRoom3_04` name the destination "hala główna
    Szkoły Życia" but both moves go to the salon (room 1; the hall is
    room 4).

## Text and data

4. `TxtGeneralStoreShopLines1` is unused (and omits
    TxtGeneralStoreShop_02, which is used elsewhere).
5. Sentinel styles mixed: line lists start with "", data tables with
    `null!`; consumption mixes Skip(1) and direct indexes
    (`TxtRoom1Lines1[1..3]`).

## Vestigial / dead code

6. `RoomAny` is used only as the teleport sentinel now, no longer for
    dispatch.
7. `Crt.Sound`/`Crt.NoSound` are empty unused stubs.
8. Flavor prose lives in lists with dummy first elements (e.g.
    TxtRoom5_01..03).

## Mechanical inconsistencies

9. Abbreviation expansion is room-dependent (on: corridor, ivy, cave,
    most concert rooms; off: start, salon, school, shops, streets, bar,
    cages 12-17, stages, roads, quest master).
10. Item quantity checks mix `== -ItemUnit`, `<= -ItemUnit`, `< 0`,
    `== 0` across use/drop/sell/buy/loot.
11. `ShowCharacter` has side effects: recomputes Bag.Limit and
    Gear.ArmorRating; ArmorRating is display-only (combat uses Armor).
12. `Bag.Count`/`Limit` have no sanity checks.
13. Permit bookkeeping is inconsistent: the permit is a carried item
    (buying a forged one adds a bag slot, so quest 1's reward adding one
    is right), but quests 2/3 and the quest-master kill grant it without
    a slot, and ZNISZCZ PRZEPUSTKA adds a slot instead of freeing one
    (Game.cs:1598). Permits also stack with no use for more than one.
    The pipe's +1 wisdom is capped on pickup but its hand-in penalty is
    not, so a pipe taken at max wisdom loses a point. **pinned**

## Smells / architecture

14. `Game.cs` is a ~3.3k-line monolith (combat, economy, shops, saves,
    input, RNG); `Rooms.cs` ~1.36k; `Texts.cs` ~1k.
15. Verb/rest convention (trailing space in verb consts, first-space
    split) is undocumented and fails silently; SplitCache is a mutable
    static dictionary.
16. The `Command` setter throws on null (readers currently never return
    null).
17. `ExpandAbbrev` allocates two lists per command; `DebugEnabled` reads
    the environment on every command.
18. `TRoom.turn` is `Action<bool>` with undocumented "entering"
    semantics; some lambdas peek at `Session.Room`.
19. Data/graph divergence has no validation (e.g. item 3).
