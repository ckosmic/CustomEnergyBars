using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace CustomEnergyBar.API
{
	public class CEBAPI
	{
		internal static EnergyBar overrideBar { get; private set; }

		/// <summary>
		/// Loads and returns an energy bar that can be used by various API functions.
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static EnergyBar LoadEnergyBar(byte[] data) {
			EnergyBar energyBar = new EnergyBar(data);
			energyBar.loadedFrom = Assembly.GetCallingAssembly().GetName().Name;
			EnergyLoader.AddAPIEnergyBar(energyBar);
			return energyBar;
		}

		/// <summary>
		/// Loads and returns an energy bar that can be used by various API functions.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static EnergyBar LoadEnergyBar(string path) {
			EnergyBar energyBar = new EnergyBar(path);
			energyBar.loadedFrom = Assembly.GetCallingAssembly().GetName().Name;
			EnergyLoader.AddAPIEnergyBar(energyBar);
			return energyBar;
		}

		/// <summary>
		/// Unloads a mod-loaded energy bar.
		/// </summary>
		/// <param name="energyBar"></param>
		public static void UnloadEnergyBar(EnergyBar energyBar) {
			if(!IsMyEnergyBar(energyBar))
				Plugin.Log.Warn("Unloading an energy bar loaded by another mod...  Quite rude tbh.");
			EnergyLoader.APIEnergyBars.Remove(energyBar);
		}

		/// <summary>
		/// Gets a mod-loaded energy bar by its bundle identifier.
		/// </summary>
		/// <param name="bundleId"></param>
		/// <returns></returns>
		public static EnergyBar GetEnergyBarByBundleId(string bundleId) {
			return EnergyLoader.GetAPIEnergyBarByBundleId(bundleId);
		}

		/// <summary>
		/// Flags CustomEnergyBars to override the player's custom energy bar with one that you've loaded from your assembly.
		/// </summary>
		/// <param name="energyBar"></param>
		/// <param name="forceOverride"></param>
		public static void SetOverrideEnergyBar(EnergyBar energyBar, bool forceOverride) {
			if (overrideBar != null && !IsMyEnergyBar(energyBar)) {
				if (forceOverride) {
					Plugin.Log.Warn("The override bar is currently occupied by " + energyBar.descriptor.name + " from " + energyBar.loadedFrom + ".  Overriding anyway.");
					Plugin.Log.Info("Custom Energy Bar is now being overridden by " + Assembly.GetCallingAssembly().GetName().Name);
					overrideBar = energyBar;
				} else {
					Plugin.Log.Error("The override bar is currently occupied by " + energyBar.descriptor.name + " from " + energyBar.loadedFrom + ".");
					return;
				}
			}
			Plugin.Log.Info("Custom Energy Bar is now being overridden by " + Assembly.GetCallingAssembly().GetName().Name);
			overrideBar = energyBar;
		}

		/// <summary>
		/// Flags CustomEnergyBars to override the player's custom energy bar with one that you've loaded from your assembly.
		/// </summary>
		/// <param name="energyBar"></param>
		public static void SetOverrideEnergyBar(EnergyBar energyBar) {
			SetOverrideEnergyBar(energyBar, false);
		}

		/// <summary>
		/// Gets the current override energy bar.
		/// </summary>
		/// <returns></returns>
		public static EnergyBar GetOverrideEnergyBar() {
			return overrideBar;
		}

		/// <summary>
		/// Returns true if the energy bar has been loaded from the calling assembly (your mod).  False if not.
		/// </summary>
		/// <param name="energyBar"></param>
		/// <returns></returns>
		public static bool IsMyEnergyBar(EnergyBar energyBar) {
			return energyBar.loadedFrom == Assembly.GetCallingAssembly().GetName().Name;
		}
	}
}
