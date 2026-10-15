# Contribution rules

Test-backed rewrite of the reconstructed original 1999' BOMBKI text
RPG game in C# - `pro` version, with bugfixes and new features!

## Ground truth and work areas

* `src/` - C# implementation sources; all work happens here.

## Functional dir layout
* `tests/` - all things related to running tests
* `tests/scenarios` - PTY test scenarios (also driven from xUnit)
* `tests/Bombki.Tests` - xUnit behavioral suite over the built stub
* `tools/` - helpers, utils and toolings
* `build/` - ephemeral build directory
* `build/tmp` - project-scoped temp directory
* `docker/` - reserved for containerized runs (currently empty)

## Commits

One line, conventional prefix (`docs: feat: fix: refactor: ci:` etc.),
no bodies. Concise and short messages.
Work lands on `dev`, one commit per functional change/subject. `main` is
fast-forwarded from `dev` only after maintainer confirmation; `main`
history is rewritten only on explicit demand.

## Features

Work is feature-based: one feature, one final commit.
Planned features are defined beforehand in `TODO.md`; as soon as
a feature is completed, its entry is removed from `TODO.md` and
added to `new-features.md`.

## Text

LF, UTF-8, Polish diacritics preserved.
Files always end with a newline; enforced locally by `.githooks/pre-commit`
(`git config core.hooksPath .githooks`).
