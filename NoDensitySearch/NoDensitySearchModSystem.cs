using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;
using Vintagestory.GameContent;

using NoDensitySearch.ModPatches;

namespace NoDensitySearch
{
	public class NoDensitySearchModSystem : ModSystem
	{
		public Harmony harmony;

		public override void Start(ICoreAPI api)
		{
			if (!Harmony.HasAnyPatches(Mod.Info.ModID)) {
				harmony = new Harmony(Mod.Info.ModID);

				var original0 = typeof(ItemProspectingPick).GetMethod("OnLoaded", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
				var postfix0 = typeof(Patch_ItemProspectingPick_OnLoaded).GetMethod("Postfix", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
				
				harmony.Patch(original0, null, new HarmonyMethod(postfix0));			

				api.Logger.Notification("Applied patch to VintageStory's ItemProspectingPick.OnLoaded from No Density Search!");	

				var original1 = typeof(ItemProspectingPick).GetMethod("OnBlockBrokenWith", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
				var prefix1 = typeof(Patch_ItemProspectingPick_OnBlockBrokenWith).GetMethod("Prefix", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
				
				harmony.Patch(original1, new HarmonyMethod(prefix1), null);

				api.Logger.Notification("Applied patch to VintageStory's ItemProspectingPick.OnBlockBrokenWith from No Density Search!");	
			}

			base.Start(api);

			api.Logger.Notification("Loaded No Density Search!");
		}
	}
}
