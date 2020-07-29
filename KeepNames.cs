using Steamworks;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using IL.Terraria;
using MonoMod.Cil;
using static Mono.Cecil.Cil.OpCodes;
using Mono.Cecil.Cil;
using Terraria.ID;

namespace KeepNames {
	public class KeepNames : Mod {
		internal static List<int> blacklist = new List<int> { };
		internal static List<int> considerAsTownNPCs = new List<int> { NPCID.SkeletonMerchant };
		internal static List<name> names = new List<name> { };
		internal static bool _patchedGame;
		public static bool patchedGame { get { return _patchedGame; } }

        public override void Load() {
            NPC.getNewNPCName += NPC_getNewNPCName;
        }

        private void NPC_getNewNPCName(ILContext il) {
			ILCursor c = new ILCursor(il);
			
			try {
				System.Reflection.MethodInfo method = typeof(KeepNames).GetMethod(nameof(getSavedName));

				ILLabel label = il.DefineLabel();
				c.Emit(Ldarg_0);
				c.Emit(OpCodes.Call, method);
				c.Emit(Stloc_0);
				c.Emit(Ldloc_0);
				c.Emit(Brfalse_S, label);
				c.Emit(Ldloc_0);
				c.Emit(Ret);

				c.MarkLabel(label);
				_patchedGame = true;
			} catch(System.Exception e) {
				Logger.Error($"{e.Message} - {e.StackTrace}");
				_patchedGame = false;
            }
		}

        /// <summary>
        /// Prevents an NPC from being given a persistant name
        /// </summary>
        /// <param name="id">The NPCID of the NPC to blacklist</param>
        public static void blacklistNPC(int id) {
			//exit if we are not either the host or in singleplayer
			if (Terraria.Main.netMode == Terraria.ID.NetmodeID.MultiplayerClient || Terraria.Main.dedServ) return;
			if (!blacklist.Contains(id)) blacklist.Add(id);
			int i = names.FindIndex(obj => obj.id == id);
			if(i!=-1) { names.RemoveAt(i); }
			
		}
		/// <summary>
		/// Persistent Names selects NPCs that have <c>bool townNPC = true</c>. This allows you to override this functionality for a specific NPC. NPCs on the blacklist still won't be considered.
		/// </summary>
		/// <param name="id">The NPCID of the NPC to use</param>
		public static void useNPC(int id) {
			//exit if we are not either the host or in singleplayer
			if (Terraria.Main.netMode == Terraria.ID.NetmodeID.MultiplayerClient || Terraria.Main.dedServ) return;
			if (!considerAsTownNPCs.Contains(id)) considerAsTownNPCs.Add(id);

		}
		/// <summary>
		/// Manually set an entities name for later use.
		/// Does not update current NPCs
		/// </summary>
		/// <param name="id">The NPCID of the NPC</param>
		/// <param name="newName">The new name to set</param>
		public static void setName(int id, string newName) {
			//exit if we are not either the host or in singleplayer
			if (Terraria.Main.netMode == Terraria.ID.NetmodeID.MultiplayerClient || Terraria.Main.dedServ) return;
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
			//exit if we are not either the host or in singleplayer
			if (Terraria.Main.netMode == Terraria.ID.NetmodeID.MultiplayerClient || Terraria.Main.dedServ) return null;
			if (blacklist.Contains(id) || GetInstance<nameConfigServer>().manualBlackList.FindIndex(b => b.Type == id) != -1) return null;
			int i = names.FindIndex(obj => obj.id == id);
			if (i == -1 || names[i].givenName == "") return null;
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
                            _ = KeepNames.getSavedName((int)id);
							return true;
						}
					case "useNPC": {
							int? id = args[1] as int?;
							if (id == null) { Logger.Error("Second argument of useNPC must be an int."); return false; }
							useNPC((int)id);
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