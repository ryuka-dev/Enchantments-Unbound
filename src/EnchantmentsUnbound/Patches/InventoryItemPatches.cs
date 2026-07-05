using System.Collections.Generic;
using HarmonyLib;
using PerfectRandom.Sulfur.Core;
using PerfectRandom.Sulfur.Core.CharacterStats;
using PerfectRandom.Sulfur.Core.Items;
using PerfectRandom.Sulfur.Core.Stats;
using PerfectRandom.Sulfur.Core.UI;
using PerfectRandom.Sulfur.Core.UI.Inventory;
using UnityEngine;

namespace EnchantmentsUnbound.Patches
{
    [HarmonyPatch(typeof(InventoryItem))]
    internal static class InventoryItemPatches
    {
        [HarmonyPatch(nameof(InventoryItem.IsEnchantmentCompatible), typeof(EnchantmentDefinition))]
        [HarmonyPrefix]
        private static bool IsEnchantmentCompatiblePrefix(InventoryItem __instance, EnchantmentDefinition enchantment, ref bool __result)
        {
            __result = enchantment != null && __instance.HasFreeEnchantmentSlot;
            return false;
        }

        [HarmonyPatch(nameof(InventoryItem.GetRankLevelProgress))]
        [HarmonyPrefix]
        private static bool GetRankLevelProgressPrefix(InventoryItem __instance, ref float __result)
        {
            __result = CalculateUnlimitedRankProgress(__instance.GetExperience());
            return false;
        }

        [HarmonyPatch(nameof(InventoryItem.AddEnchantment))]
        [HarmonyPrefix]
        private static bool AddEnchantmentPrefix(InventoryItem __instance, ItemDefinition enchantmentItem, bool announce)
        {
            if (enchantmentItem == null || !enchantmentItem.appliesEnchantment.IsValid)
            {
                Debug.LogError("Trying to add invalid enchantment item.");
                return false;
            }

            EnchantmentDefinition enchantment = enchantmentItem.appliesEnchantment.GetAsset();
            if (enchantment == null)
            {
                Debug.LogError("Trying to add null enchantment.");
                return false;
            }

            enchantment.RegisterAppliedBy(enchantmentItem);
            __instance.enchantments.Add(enchantment);
            StaticInstance<AchievementManager>.Instance.EnchantmentAppliedToItem(enchantment, __instance.itemDefinition, __instance.enchantments.Count);

            if (HasRandomOilAttribute(enchantment))
            {
                for (int i = 0; i < enchantment.modifiersApplied.Count; i++)
                {
                    AddRandomOilAttributes(__instance, enchantment.modifiersApplied[i].attribute, enchantment);
                }
            }
            else
            {
                AddEnchantmentModifiers(__instance, enchantment, enchantment.id.AsGlobalId());
            }

            if (announce)
            {
                StaticInstance<SoundBankUI>.Instance.PlayClip(UISounds.UI_Enchant);
            }
            __instance.SyncWithInstancedVersion();
            return false;
        }

        private static float CalculateUnlimitedRankProgress(float experience)
        {
            if (experience <= 0f)
            {
                return 0f;
            }

            const float firstRankExperience = 50f;
            const float rankExperienceMultiplier = 2.5f;
            float currentThreshold = 0f;
            float nextThreshold = firstRankExperience;
            int rank = 0;

            while (experience >= nextThreshold && rank < 200)
            {
                rank++;
                currentThreshold = nextThreshold;
                nextThreshold *= rankExperienceMultiplier;
            }

            if (nextThreshold <= currentThreshold)
            {
                return rank;
            }

            return rank + (experience - currentThreshold) / (nextThreshold - currentThreshold);
        }

        private static bool HasRandomOilAttribute(EnchantmentDefinition enchantment)
        {
            if (enchantment.modifiersApplied == null)
            {
                return false;
            }

            for (int i = 0; i < enchantment.modifiersApplied.Count; i++)
            {
                if (enchantment.modifiersApplied[i].attribute == ItemAttributes.EnchantmentAddRandomOilAttributes)
                {
                    return true;
                }
            }
            return false;
        }

        private static void AddRandomOilAttributes(InventoryItem inventoryItem, ItemAttributes attribute, EnchantmentDefinition randomOilEnchantment)
        {
            if (attribute != ItemAttributes.EnchantmentAddRandomOilAttributes)
            {
                return;
            }

            inventoryItem.enchantments.Remove(randomOilEnchantment);
            ItemDefinition selectedItem = null;
            EnchantmentDefinition selectedEnchantment = null;
            LootTable oilLootTable = StaticInstance<GameManager>.Instance.Settings.LootSettings.enchantmentOilLootTable;
            if (oilLootTable == null || oilLootTable.entries == null)
            {
                return;
            }

            bool found = false;
            int attempts = oilLootTable.entries.Count;

            for (int i = 0; i < attempts; i++)
            {
                selectedItem = oilLootTable.SelectOneItem();
                if (selectedItem == null || !selectedItem.appliesEnchantment.IsValid)
                {
                    continue;
                }

                selectedEnchantment = selectedItem.appliesEnchantment.GetAsset();
                if (selectedEnchantment != null &&
                    selectedEnchantment.modifiersApplied != null &&
                    randomOilEnchantment.id != selectedItem.appliesEnchantment &&
                    selectedEnchantment.modifiersApplied.Count > 0)
                {
                    found = true;
                    break;
                }
            }

            if (!found || selectedItem == null || selectedEnchantment == null)
            {
                return;
            }

            selectedEnchantment.RegisterAppliedBy(selectedItem);
            inventoryItem.enchantments.Add(selectedEnchantment);
            AddEnchantmentModifiers(inventoryItem, selectedEnchantment, selectedItem.appliesEnchantment.AsGlobalId());
        }

        private static void AddEnchantmentModifiers(InventoryItem inventoryItem, EnchantmentDefinition enchantment, uint sourceId)
        {
            if (enchantment.modifiersApplied == null)
            {
                return;
            }

            for (int i = 0; i < enchantment.modifiersApplied.Count; i++)
            {
                ItemModifierContainer modifier = enchantment.modifiersApplied[i];
                inventoryItem.stats.AddModifier(modifier.attribute, new StatModifier(modifier.value, modifier.modType, sourceId));
            }
        }
    }

    [HarmonyPatch(typeof(Paperdoll), nameof(Paperdoll.GetItemsCompatibleWithEnchantment))]
    internal static class PaperdollEnchantmentHighlightPatch
    {
        private static bool Prefix(Paperdoll __instance, EnchantmentDefinition enchantment, ref List<InventoryItem> __result)
        {
            List<InventoryItem> items = new List<InventoryItem>();
            List<PaperdollSlot> slots = __instance.GetSlots();
            for (int i = 0; i < slots.Count; i++)
            {
                InventoryItem itemInSlot = slots[i].itemInSlot;
                if (itemInSlot != null && itemInSlot.IsEnchantmentCompatible(enchantment))
                {
                    items.Add(itemInSlot);
                }
            }
            __result = items;
            return false;
        }
    }

    [HarmonyPatch(typeof(ItemGrid), nameof(ItemGrid.GetItemsCompatibleWithEnchantment))]
    internal static class ItemGridEnchantmentHighlightPatch
    {
        private static bool Prefix(ItemGrid __instance, EnchantmentDefinition enchantment, ref List<InventoryItem> __result)
        {
            List<InventoryItem> items = new List<InventoryItem>();
            GridSlot[,] slots = __instance.Slots;
            int upperBoundX = slots.GetUpperBound(0);
            int upperBoundY = slots.GetUpperBound(1);

            for (int x = slots.GetLowerBound(0); x <= upperBoundX; x++)
            {
                for (int y = slots.GetLowerBound(1); y <= upperBoundY; y++)
                {
                    InventoryItem itemInSlot = slots[x, y].ItemInSlot;
                    if (itemInSlot != null && itemInSlot.IsEnchantmentCompatible(enchantment) && !items.Contains(itemInSlot))
                    {
                        items.Add(itemInSlot);
                    }
                }
            }

            if (StaticInstance<UIManager>.Instance != null && StaticInstance<UIManager>.Instance.Paperdoll != null)
            {
                items.AddRange(StaticInstance<UIManager>.Instance.Paperdoll.GetItemsCompatibleWithEnchantment(enchantment));
            }

            __result = items;
            return false;
        }
    }
}