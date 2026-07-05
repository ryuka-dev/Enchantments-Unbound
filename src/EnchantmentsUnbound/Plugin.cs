using BepInEx;
using HarmonyLib;

namespace EnchantmentsUnbound
{
    [BepInPlugin(ModInfo.Id, ModInfo.Name, ModInfo.Version)]
    public sealed class Plugin : BaseUnityPlugin
    {
        private Harmony harmony;

        private void Awake()
        {
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
    }
}