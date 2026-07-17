using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;

namespace EnchantmentsUnbound
{
    [BepInPlugin(ModInfo.Id, ModInfo.Name, ModInfo.Version)]
    public sealed class Plugin : BaseUnityPlugin
    {
        private const float MinExtendedRankXpGrowth = 1f;
        private const float MaxExtendedRankXpGrowth = 2.5f;
        private const float DefaultExtendedRankXpGrowth = 1.5f;

        private static ConfigEntry<float> extendedRankXpGrowth;

        private Harmony harmony;

        private void Awake()
        {
            extendedRankXpGrowth = Config.Bind(
                "Progression",
                "ExtendedRankXpGrowth",
                DefaultExtendedRankXpGrowth,
                new ConfigDescription(
                    "How much more XP each rank past 5 costs compared to the previous rank. " +
                    "Ranks 1-5 always use the vanilla curve. " +
                    "1.0 = every rank past 5 costs the same as rank 5, " +
                    "2.5 = keep compounding like the vanilla curve (XP requirements explode quickly).",
                    new AcceptableValueRange<float>(MinExtendedRankXpGrowth, MaxExtendedRankXpGrowth)
                )
            );

            harmony = new Harmony(ModInfo.Id);
            harmony.PatchAll();
            Logger.LogInfo(ModInfo.Name + " loaded.");
        }

        private void OnDestroy()
        {
            if (harmony != null)
            {
                harmony.UnpatchSelf();
            }
        }

        internal static float GetExtendedRankXpGrowth()
        {
            if (extendedRankXpGrowth == null)
            {
                return DefaultExtendedRankXpGrowth;
            }

            float value = extendedRankXpGrowth.Value;
            if (float.IsNaN(value) || float.IsInfinity(value))
            {
                return DefaultExtendedRankXpGrowth;
            }

            if (value < MinExtendedRankXpGrowth)
            {
                return MinExtendedRankXpGrowth;
            }

            if (value > MaxExtendedRankXpGrowth)
            {
                return MaxExtendedRankXpGrowth;
            }

            return value;
        }
    }
}
