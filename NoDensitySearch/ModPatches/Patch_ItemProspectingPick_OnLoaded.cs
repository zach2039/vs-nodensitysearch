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

namespace NoDensitySearch.ModPatches
{
	[HarmonyPatchCategory("NoDensitySearch_ItemProspectingPick")]
	[HarmonyPatch(typeof(ItemProspectingPick), "OnLoaded")]
	class Patch_ItemProspectingPick_OnLoaded
	{
		static void Postfix(ItemAxe __instance, ICoreAPI api, ref SkillItem[] ___toolModes)
		{
            // Remove density search item from toolMode, if present and found
			int densityIdx = -1;
			for (int i = 0; i < ___toolModes.Length; i++)
			{
				if (___toolModes[i].Name == "density")
				{
					densityIdx = i;
					break;
				}
			}

			if (densityIdx != -1)
			{
				___toolModes.RemoveEntry(densityIdx);
			}
		}
	}
}