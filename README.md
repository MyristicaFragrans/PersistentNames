# Persistant Names
Persistant Names will make it so your NPCs will always keep the same names- even if they die and respawn!

## Your Town will remain!
By the end of a normal game, your town has been completely replaced many times. Instead, you can have your NPCs just respawn with the same names.

## Mod Integration
All town NPCs from other mods work by default

## Known Issues
* `(Name) Has Arrived!` Will show an incorrect name
* Some mods may not work out of box
* Once a world is loaded with Persistant Names, it cannot be used without Persistant Names.
# Modding API
Persistant Names aims to work with other mods!
## Using Mod.Call
This is probably the easiest. Just make sure ```Mod KeepNames = ModLoader.GetMod("KeepNames");``` is somewhere in your context.
### Blacklist an NPC
You can prevent Persistant Names from automatically assigning your name to all new NPCs. This does not work dynamically and should be set at load.
```cs
if (KeepNames != null) {
  KeepNames.Call("blacklistNPC", ModContent.NPCType<MyModdedNPC>());
}
```
### Manually Assign a Name
Manually set the NPC's name, before they even spawn. This does not update existing NPCs.
```cs
if (KeepNames != null) {
  KeepNames.Call("setName", ModContent.NPCType<MyModdedNPC>(), "MyNewName");
}
```
### Get an NPC's Name
Get a name that has been saved, Returns the saved name or `null` otherwise
```cs
if (KeepNames != null) {
  KeepNames.Call("getSavedName", ModContent.NPCType<MyModdedNPC>());
}
```
---
## Using Weak References:
You can use weak references relatively easily.  
Just make sure to put `weakReferences = KeepNames@0.1` in your build.txt
### Blacklist an NPC
You can prevent Persistant Names from automatically assigning your name to all new NPCs. This does not work dynamically and should be set at load.
```cs
if (ModLoader.GetMod("KeepNames") != null) {
  KeepNames.KeepNames.blacklistNPC(ModContent.NPCType<MyModdedNPC>());
}
```
### Manually Assign a Name
Manually set the NPC's name, before they even spawn. This does not update existing NPCs.
```cs
if (ModLoader.GetMod("KeepNames") != null) {
  KeepNames.KeepNames.setName(ModContent.NPCType<MyModdedNPC>(), "MyNewName");
}
```
### Get an NPC's name
Get a name that has been saved, Returns the saved name or `null` otherwise
```cs
if (ModLoader.GetMod("KeepNames") != null) {
  KeepNames.KeepNames.getSavedName(ModContent.NPCType<MyModdedNPC>());
}
```