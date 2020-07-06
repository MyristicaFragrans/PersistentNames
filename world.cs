using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace KeepNames {
    class world : ModWorld {
		public override void Initialize() {
		}
		public override TagCompound Save() {
			List<int> ids = new List<int>();
			List<string> names = new List<string>();
			for(int i = 0; i < KeepNames.names.Count; i++) {
				ids.Add(KeepNames.names[i].id);
				names.Add(KeepNames.names[i].givenName);
            }
			if (ids.Count != 0)
				return new TagCompound {
					["ids"] = ids,
					["names"] = names
				};
			else return new TagCompound { };
		}
		public override void Load(TagCompound tag) {
			List<int> ids = tag.Get<List<int>>("ids");
			List<string> names = tag.Get<List<string>>("names");
            if (ids.Count != names.Count) throw new System.Exception("Persistant Names: Mismatch between ids and names in NBT data");
			KeepNames.names = new List<name> { };
			for(var i = 0; i < ids.Count; i++) {
				if(names[i]!="")
					KeepNames.names.Add(new name(ids[i], names[i]));
            }
		}
	}
}
