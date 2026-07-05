# Enchantments Unbound

Enchantments Unbound is a local mod project for SULFUR.

## Project Rules

- Target framework: `net472`
- Default game managed DLL folder: `D:\SteamLibrary\steamapps\common\SULFUR\Sulfur_Data\Managed`
- Decompiled game source belongs under `GameSource/`
- `GameSource/` contents are local reference material and must not be committed

If SULFUR is installed somewhere else, build with:

```powershell
dotnet build -p:SulfurGameDir="D:\Path\To\SULFUR"
```

or override the managed folder directly:

```powershell
dotnet build -p:SulfurManagedDir="D:\Path\To\SULFUR\Sulfur_Data\Managed"
```
