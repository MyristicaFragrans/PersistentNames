using Steamworks;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace KeepNames {
	public class KeepNames : Mod {
		internal static List<int> blacklist = new List<int> { };
		internal static List<name> names = new List<name> { };
		/// <summary>
		/// Prevents an NPC from being given a persistant name
		/// </summary>
		/// <param name="id">The NPCID of the NPC to blacklist</param>
		public static void blacklistNPC(int id) {
			if (!blacklist.Contains(id)) blacklist.Add(id);
			int i = names.FindIndex(obj => obj.id == id);
			if(i!=-1) { names.RemoveAt(i); }

        }
		/// <summary>
		/// Manually set an entities name for later use.
		/// Does not update current NPCs
		/// </summary>
		/// <param name="id">The NPCID of the NPC</param>
		/// <param name="newName">The new name to set</param>
		public static void setName(int id, string newName) {
			if (blacklist.Contains(id)) return;
			int i = names.FindIndex(obj => obj.id == id);
			if(i==-1) {
				names.Add(new name(id, newName));
            } else {
				names[i].givenName = newName;
            }
        }
		/// <summary>
		/// Get a name that has been saved,
		/// Returns the saved name or <c>null</c> otherwise
		/// </summary>
		/// <param name="id">The NPCID of the NPC</param>
		public static string getSavedName(int id) {
			if (blacklist.Contains(id)) return null;
			int i = names.FindIndex(obj => obj.id == id);
			if (i == -1) return null;
			else {
				return names[i].givenName;
            }
		}

		/// <summary>
		/// Dynamically change listings and blacklists in KeepNames
		/// Returns true on success, false on fail
		/// Prints to log errors
		/// </summary>
		/// <example>Call( "blacklistNPC", NPCType<MyNPC>() );</example>
		/// <example>Call( "setName", NPCType<MyNPC>(), "MyNewName" );</example>
		/// <example>Call( "getSavedName", NPCType<MyNPC>() );</example>
		/// <param name="args">An object array of {"Method Name", [args in order]}</param>
		public override object Call(params object[] args) {
			if (args.Length < 2) {
				Logger.Error("Mod.Call() expects at least 2 arguments.");
				return false;
			}
			try {
				string functionName = args[0] as string;
				switch (functionName) {
					case "blacklistNPC": {
							int? id = args[1] as int?;
							if (id == null) { Logger.Error("Second argument of blacklistNPC must be an int."); return false; }
							blacklistNPC((int)id);
							return true;
						}
					case "setName": {
							if (args.Length < 3) { Logger.Error("Mod.Call() expects at least 3 arguments."); return false; }
							int? id = args[1] as int?;
							if (id == null) { Logger.Error("Second argument of setName must be an int."); return false; }
							string name = args[2] as string;
							if (name == null) { Logger.Error("Third argument of setName must be a string."); return false; }
							setName((int)id, name);
							return true;
						}
					case "getSavedName": {
							int? id = args[1] as int?;
							if (id == null) { Logger.Error("Second argument of getSavedName must be an int."); return false; }
							getSavedName((int)id);
							return true;
						}
					default:
						Logger.Warn($"Call Warning: Unknown method name '{functionName}'");
						return false;
				}
			}
			catch (Exception e) {
				Logger.Error($"Call Error: {e.StackTrace} {e.Message}");
				return false;
			}
		}
	}
}