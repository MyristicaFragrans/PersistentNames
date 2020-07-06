using System;
using Terraria;
using Terraria.ModLoader;

namespace KeepNames {
    class interceptNames : GlobalNPC {
        public override void SetDefaults(NPC npc) {
            if (!KeepNames.blacklist.Contains(npc.type) && npc.townNPC) {
                int i = KeepNames.names.FindIndex(obj => obj.id == npc.type);
                if (i != -1) {
                    npc.GivenName = KeepNames.names[i].givenName;
                }
            }
            base.SetDefaults(npc);
        }
        public override bool PreAI(NPC npc) {
            if (!KeepNames.blacklist.Contains(npc.type) && npc.townNPC) {
                int i = KeepNames.names.FindIndex(obj => obj.id == npc.type);
                if (i != -1) {
                    npc.GivenName = KeepNames.names[i].givenName;
                }
                else if (npc.townNPC && !KeepNames.blacklist.Contains(npc.type) && npc.GivenName!="") {
                    KeepNames.setName(npc.type, npc.GivenName);
                }
            }
            return base.PreAI(npc);
        }
    }
}
