---
name: ci
description: Pipelines and their results. Use when working with GitHub CI system.
---

# CI pipelines and runs

## The workflow

One workflow (`.github/workflows/bombki-tests.yml`) runs on pushes to
`main` and `dev`, on pull requests, and on demand. Four jobs:

- `style` - the style gate: `dotnet-format --verify-no-changes` over the
  solution, the Roslyn-based `tools/style-lint` over `src` and `tests`,
  and text hygiene (newline at EOF, no trailing whitespace).
- `csharp` - Linux build and tests: `dotnet build` then `dotnet test`
  over `BombkiPro.slnx`, then a self-contained single-file publish for
  `linux-x64`, zipped as `BOMBKI-pro-linux-x86_64.zip` and uploaded as
  the `bombki-linux-x64` artifact.
- `windows-build` - the same build and publish for `win-x64`, zipped as
  `BOMBKI-pro-win-x64.zip` and uploaded as `bombki-win-x64`.
- `latest` - a rolling release on every push to `main` (or manual
  dispatch): it downloads both artifacts, verifies and stages them,
  prunes stale assets, force-moves the `latest` tag to the new commit,
  and uploads with retries because clobbering races the asset deletion.
  A failed build publishes nothing and leaves the previous release
  untouched.

## Reading a run

- Pipeline breakage is usually pipeline-specific: a red run that covers none of
  the changed files says little about them. Verify the change locally before
  trusting a red run.
- Never wait on a pipeline unless it is what you changed or your task depends on
  it; waiting on an unrelated run just burns time. Report the pipeline result
  when it lands.
