# Enchantments Unbound

Enchantments Unbound is a BepInEx mod for SULFUR that removes the game's enchantment limits while preserving the native weapon upgrade flow.

## Features

- Allows repeated oils and scrolls on the same weapon.
- Allows multiple enchantments of the same type to stack.
- Allows weapons to keep ranking past level 5.
- Keeps enchantment slots tied to weapon rank, so additional slots still require upgrading the weapon.
- Configurable XP cost growth for ranks past 5 (`Progression.ExtendedRankXpGrowth`, default `1.5`). Ranks 1-5 always use the vanilla curve; `2.5` restores the vanilla-style compounding where high ranks need astronomical XP.

## Compatibility

- Targets SULFUR v0.18.x (verified against the v0.18.4 assemblies). v0.18.0 removed `UISounds.UI_Enchant`, so builds older than 0.2.0 play a wrong enchant sound on v0.18.x.
- Compatible with Weapon XP multiplier mods: they scale the XP a weapon receives, this mod only decides how much XP each rank requires.

## Links

- GitHub: https://github.com/ryuka-dev/Enchantments-Unbound

## Project Rules

- Target framework: `net472`
- Default game managed DLL folder: `D:\SteamLibrary\steamapps\common\SULFUR\Sulfur_Data\Managed`
- Gale development profile paths are configured in `Directory.Build.props`
- Decompiled game source belongs under `GameSource/`
- `GameSource/` contents are local reference material and must not be committed
- Build DLLs, release zips, PSD source files, and decompiled game source are ignored by Git

## Build

Build the mod:

```powershell
dotnet build
```

Deploy to the configured Gale profile and copy a release DLL into `Thunderstore/`:

```powershell
dotnet build -p:DeployToSulfurProfile=true
```

If SULFUR is installed somewhere else, override the game path:

```powershell
dotnet build -p:SulfurGameDir="D:\Path\To\SULFUR"
```

or override the managed folder directly:

```powershell
dotnet build -p:SulfurManagedDir="D:\Path\To\SULFUR\Sulfur_Data\Managed"
```

## Thunderstore

Release metadata lives in `Thunderstore/`. The folder is flat: its content (minus `nexusmods.txt` and generated archives) is exactly the release zip.

The release DLL is copied to:

```text
Thunderstore/EnchantmentsUnbound.dll
```

`Thunderstore/nexusmods.txt` is the generated NexusMods BBCode description (README + CHANGELOG); it is committed but never packaged.

The DLL itself is generated output and is not committed.
