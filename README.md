*Dedicated to my children, today's and future; let The Sun always shine in your eyes.*

# BOMBKI-pro

This is a test-backed rewrite of the reconstructed original 1999' BOMBKI text
RPG game. It used behavior-perfect Pascal refactoring of the original code
to create a complete minimal C# implementation - feature-identical
(including bugs), which is now a basis for a "pro" version - with bugfixes,
UX improvements, and different extensions and reimaginings.

It is based on extensive work done as part of collaboration on
https://github.com/rzuf79/bombki , with an explicit approval from the game's
original author, Mateusz Pawluczuk. The test setup is based on extensive work
done in https://github.com/FyiurAmron/BOMBKI-tests (historic reference only now)
and the refactorings are based on extensive work done in
https://github.com/FyiurAmron/BOMBKI-refactor (also a historic reference),
which in turn fuelled https://github.com/FyiurAmron/BOMBKI-sharp - direct parent
of this repo.

tl;dr question:
>(...) uwaga, jest prosba o blogoslawienstwo projektu rekonstrukcyjnego!(...)

tl;dr answer:
>(...) Blogoslawienstwo (...) dane! (...) Pozdrawiam, Mateusz

## Setup

- .NET 11 SDK for building and testing the C# port (`dotnet build`,
  `dotnet test`).
- `dotnet-format` for the C# formatting gate
  (`dotnet tool install -g dotnet-format`, checked by `tools/gate.sh`).
