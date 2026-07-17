# Changelog

## 0.2.0

- Updated for SULFUR v0.18.x (verified against v0.18.4). The enchant sound now uses the game's new audio path; the old build played a wrong sound on 0.18.x.
- Reworked the XP curve past rank 5. The old curve kept multiplying each rank's XP requirement by 2.5x, so ranks beyond ~8 needed hundreds of thousands of XP and progress effectively stalled (also reported when combined with XP multiplier mods). Ranks 1-5 still match vanilla exactly.
- New config `Progression.ExtendedRankXpGrowth` (default `1.5`): how much more XP each rank past 5 costs compared to the previous rank. Set `2.5` to restore the old runaway curve, `1.0` for a flat cost per rank.
- Existing weapons are safe: the change only reinterprets stored XP, and the gentler curve can only raise a weapon's rank, never lower it.

## 0.1.0

- Initial release.
- Removed duplicate oil and scroll restrictions.
- Removed the weapon rank 5 cap for enchantment slot progression.
