# Enchantments Unbound

Enchantments Unbound is a BepInEx mod for SULFUR that removes the game's enchantment limits while preserving the native weapon upgrade flow.

## Features

- Allows repeated oils and scrolls on the same weapon.
- Allows multiple enchantments of the same type to stack.
- Allows weapons to keep ranking past level 5.
- Keeps enchantment slots tied to weapon rank, so additional slots still require upgrading the weapon.

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

Deploy to the configured Gale profile and copy a release DLL into `Thunderstore/BepInEx/plugins/EnchantmentsUnbound/`:

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

Release metadata lives in `Thunderstore/`.

The release DLL is copied to:

```text
Thunderstore/BepInEx/plugins/EnchantmentsUnbound/EnchantmentsUnbound.dll
```

The DLL itself is generated output and is not committed.
