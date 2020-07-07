using System.Collections.Generic;
using System;
using System.IO;
using Terraria;
using Terraria.IO;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Terraria.ModLoader.Config;

namespace KeepNames {
   // [BackgroundColor(144, 252, 249)]
   // [Label("Override Persistant Names Functionality")]
    class nameConfigServer : ModConfig {
        public override ConfigScope Mode => ConfigScope.ServerSide;
        [Label("Blacklist NPCs")]
        [Tooltip("If PersistantNames is interfering with the functionality of other mods, you can prevent Persistant Names from making changes per-NPC")]
        public List<NPCDefinition> manualBlackList { get; set; } = new List<NPCDefinition>();
        public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref string message) {
            return true;
        }
    }
}
