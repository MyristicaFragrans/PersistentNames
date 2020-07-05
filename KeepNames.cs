using Steamworks;
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
	}
}