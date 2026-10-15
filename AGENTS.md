# AGENTS.md — AI working notes

ALWAYS ASK IF UNSURE!

`CONTRIBUTING.md` is the project rules — read it in full before starting and
comply. This file is the operational layer for non-human contributors: the
concrete checks and procedures, not a restatement of the rules.

## Project shape

The project implements the game in `src/` (C#) as the `pro` version:
bugfixes, UX improvements, and extensions on top of the reconstructed
1999 BOMBKI text RPG. There is no frozen Pascal reference in this repo;
`src/` is the single source of truth.

## Check tiers

The `dotnet` toolchain is the daily driver: `dotnet build`, `dotnet test`
(the xUnit behavioral suite over the built stub), and `tools/gate.sh`
(style plus tests) run locally, and their results are enough to keep
working.

## Language

Always use English for communication. Tolerate input in other languages, but
respond and contribute entirely in English.

In documentation and user-facing text or output, prefer plain ASCII whenever
possible. Never use an em dash; write a regular ASCII hyphen-minus (`-`)
instead, except when reproducing a direct quotation verbatim.

## Task-start sequence

1. Read `README.md` for the project overview and documented build/test commands.
2. Read `CONTRIBUTING.md` in full to learn the project conventions and scope.
3. Establish the working baseline on the `dev` branch (branch and worktree
   status), then identify the files and behavior relevant to the task.
4. Refresh your view of any in-scope document changed during the task before
   relying on its updated contents.

## End-of-task verification

- Run the applicable checks (`dotnet build`, `dotnet test`, or the full
  `tools/gate.sh`) and `git diff --check`;
  report only checks that actually ran.
- On Linux, prefer running the required compiler and checks directly. Do not
  use Docker or Podman when the needed functionality is available locally.
- For actual behavior tests, prefer a host-native executable when available.
  Drive it through an interactive PTY with expect-style synchronization: wait
  for expected output before sending each response. For example, wait for
  startup `TICK` before sending the race, then wait for the player-name prompt
  before sending the name. This complete sequence is illustrative only, not a
  required scenario; tailor the steps and output markers to the behavior under
  test. Synchronize on expected output rather than using fixed sleeps, blind
  input batches, or repeated polling.
- Verify the worktree, then commit on `dev` and push it when the current
  task is complete; `main` moves per Branch workflow only.

## Running tests

- Test runners must explicitly emit the names of the
  failed tests (not only a failure count), so a single
  run shows exactly which tests broke and why.
- Always redirect a test run's full output to a file
  under `build/tmp/` and inspect that file (read or
  grep it) to see the results and failures. Never
  rerun a test suite merely to view its output; rerun
  only to reproduce a failure after changing code.
- **Never run more than one scenario at a time.** One
  scenario per invocation, serially, never a loop and
  never in parallel. Scenarios are interactive PTY
  sessions against a real game process; running several
  at once starves the machine (each one is
  latency-sensitive, so timing-sensitive steps fail
  spuriously) and makes CPU and wall-clock cost
  unpredictable. If several scenarios need checking,
  run them one after another, one command at a time.

## Scenarios

`tests/scenarios/*.json` drives the game through a PTY: each
step waits for an `expect` regex and then sends a command.
Naming carries no meaning beyond that, so keep it plain and
consistent:

- `<area>-tour.json` - a room-graph walk covering that area.
- `<what-it-covers>.json` - a targeted scenario, hand-written for the
  behavior it pins.

Do not add a `-smoke` suffix: it used to decorate a handful of
early scenarios, it does not distinguish anything, and it was
dropped from all of them.

## Test execution modes

One way to execute the tests: `dotnet test` runs every scenario in
`tests/scenarios/` against the built stub through the same PTY
harness (serially, never in parallel). Report which runner a result
came from.

The suite pins its randomness with the `BOMBKI_SEED` environment
variable (decimal or `0x` hex) and compresses timing with
`BOMBKI_DELAY_MULT=0`, so a scenario whose outcome depends on a roll
or on timing stays reproducible instead of passing on one run and
timing out on the next. When a scenario is flaky for reasons that are
not a genuine bug in the game, reach for the `tools/find-stable-seed`
helper to search for a seed where it passes deterministically - one
scenario at a time, as above.

## Autonomy

Do not stop after completing an intermediate task. Continue autonomously with
the next useful step. Only stop when the entire requested objective is
complete or you genuinely require information that cannot be obtained
independently.

Temporary build and analysis files belong under the project-local
`build/tmp/` directory. Do not use a shared/global `/tmp` location.

## Skills

Subject knowledge lives in the globally installed skills; load the one
matching the task. Inventory: `ci` (pipelines), `cs` (C# style),
`docs` (docs and skills), `godot` (Godot toolchain, future use),
`repo` (git habits), `yaml` (data-file format).

## Installing software

- On Windows, prefer portable / temp-only solutions (download-and-extract into
  a temp dir, then run from there) over system installs whenever possible.
- On Windows, always ask the human for approval before installing software on
  the machine (e.g. winget/choco installs).

## Branch workflow

- `dev` is the working branch: every commit lands on `dev`; never commit
  directly to `main`.
- Commit per functional change/subject, not per prompt: several commits for
  one complex prompt are expected; one commit bundling unrelated changes
  just because they arrived together is not acceptable.
- `main` advances only by fast-forward from `dev`, and only after explicit
  confirmation from the maintainer. No direct pushes to `main`, no merge
  commits, no squash-merges.
- `dev` history is rewritten freely: amends, squashes, fixups, and rebases
  are all pre-approved on `dev`, pushed or not; force-push with
  `--force-with-lease`. `main` history is rewritten only on the
  maintainer's explicit demand.
- Reviews happen on `dev`: push it and report. The maintainer reviews
  in-session or on GitHub and confirms each fast-forward to `main`.

## Committing

- Subject: one line, one idea, concise, ≤ 60 chars,
  prefix from the CONTRIBUTING list, no body.
- Stage explicit paths only (`git add <files>`), never `git add -A`.
- Pre-commit checks: file is LF-only (no CR bytes), no trailing whitespace,
  `git diff --check` clean.
- Hook: enable `.githooks/pre-commit` with `git config core.hooksPath .githooks`
  - enforces newline at EOF, LF-only, and no trailing whitespace.
- A feature is finished only when the user explicitly confirms it.
  Never assume completion: the TODO.md to new-features.md move and
  the final commit happen solely on the user's confirmation.

## History rewrites

1. `dev`: amend, squash, fixup, and rebase freely; no per-change approval
   needed. `main`: rewritten only on the maintainer's explicit demand.
2. Reword only the intended commits (interactive rebase) or amend the tip.
3. Verify a message-only rewrite: `git diff <old-tip> <new-tip>` is empty.
4. Push rewritten branches with `--force-with-lease`.
