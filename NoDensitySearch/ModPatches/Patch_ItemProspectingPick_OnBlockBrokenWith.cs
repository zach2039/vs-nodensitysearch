using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;
using Vintagestory.API.Client;
using Vintagestory.API.Util;
using System.Reflection;

namespace NoDensitySearch.ModPatches
{
	[HarmonyPatchCategory("NoDensitySearch_ItemProspectingPick")]
	[HarmonyPatch(typeof(ItemProspectingPick), "OnBlockBrokenWith")]
	class Patch_ItemProspectingPick_OnBlockBrokenWith
	{
		static bool Prefix(ItemProspectingPick __instance, ref bool __result, ref ICoreAPI ___api, ref SkillItem[] ___toolModes, IWorldAccessor world, Entity byEntity, ItemSlot itemslot, BlockSelection blockSel, float dropQuantityMultiplier = 1f)
		{
			// Since vanilla uses indexes instead of mode names for tool modes, we will need to change how modes are selected on the propick
			int selectedIdx = __instance.GetToolMode(itemslot, (byEntity as EntityPlayer).Player, blockSel);
			
			SkillItem toolMode = ___toolModes[selectedIdx];

			int radius = ___api.World.Config.GetString("propickNodeSearchRadius", null).ToInt(0);
			int damage = 1;

			if (toolMode.Code.BeginsWith("game", "node") && radius > 0)
			{
				// Call ProbeBlockNodeMode via reflection, and hope performance hit is negligable 
				MethodInfo methodProbeBlockNodeMode = typeof(ItemProspectingPick).GetMethod("ProbeBlockNodeMode", BindingFlags.NonPublic | BindingFlags.Instance);
				methodProbeBlockNodeMode.Invoke(__instance, new object[] { world, byEntity, itemslot, blockSel, radius });
				damage = 2;
			}
			else if (toolMode.Code.BeginsWith("game", "density"))
			{
				// Call ProbeBlockDensityMode via reflection, and hope performance hit is negligable 
				MethodInfo methodProbeBlockDensityMode = typeof(ItemProspectingPick).GetMethod("ProbeBlockDensityMode", BindingFlags.NonPublic | BindingFlags.Instance);
				methodProbeBlockDensityMode.Invoke(__instance, new object[] { world, byEntity, itemslot, blockSel });
			}
			if (__instance.DamagedBy != null && __instance.DamagedBy.Contains(EnumItemDamageSource.BlockBreaking))
			{
				__instance.DamageItem(world, byEntity, itemslot, damage);
			}

			__result = true;

			return false; // skip original method
		}
	}
}