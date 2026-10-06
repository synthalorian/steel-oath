# Nexus Mods page: Steel Oath

Early beta: v0.1.0 passed every automated build check but hasn't been playtested in-game. The page says so up front. Exact upload-form values are in [NEXUS_FIELDS.md](NEXUS_FIELDS.md).

## Page settings

- **Game:** Skyrim Special Edition
- **Mod name:** Steel Oath
- **Version:** 0.1.0 (early beta)
- **Category:** Gameplay
- **Suggested tags:** Gameplay, Lore-Friendly, Combat, Roleplay, Powers, ESL, No SKSE, Scripted
- **Requirements:** none.
- **Permissions (suggested):** source on GitHub under MIT, so allow modification and conversions with credit. Don't allow re-uploads of the archive itself.
- **File to upload:** `SteelOath-0.1.0.zip` (FOMOD), main file, named "Steel Oath".

## Description (paste into the BBCode editor)

```bbcode
[center][size=6][b]STEEL OATH[/b][/size]
[size=3][i]Swear it. Keep it. The longer it holds, the stronger you become.[/i][/size]

[size=3][color=#e0a030][b]EARLY BETA (v0.1.0)[/b]: this version passes every automated build check but hasn't been playtested in-game yet. Please report bugs in the Bugs tab or on GitHub, ideally with your Papyrus log.[/color][/size][/center]

[line]

[size=4][b]Summary[/b][/size]
After Helgen you gain a lesser power, [b]Swear an Oath[/b]. Pick a warrior's vow and keep it: the blessing starts small and grows the longer the oath holds. Break it and you lose the blessing, carry the mark of an [b]Oathbreaker[/b] for a day, and can't swear that oath again for three. Every vow is something the game itself reports to scripts, so the mod always knows when you've broken one. No SKSE needed.

[size=4][b]The oaths[/b][/size]
[list]
[*][b]Oath of Steel[/b]: cast no spell from your hands and never ready a scroll. Powers, shouts, staves and potions are fine. [i]Health +20 / +40 / +60[/i]
[*][b]Oath of the Shield[/b]: keep a shield on your arm (a few seconds' grace to swap shields). [i]Block +10 / +20 / +30[/i]
[*][b]Oath of Iron[/b]: wear no light armor, light shields included. [i]Armor +30 / +60 / +100[/i]
[*][b]Oath of Endurance[/b]: no potions, food or poisons while in combat. [i]Stamina +20 / +40 / +60[/i]
[*][b]Oath of Close Quarters[/b]: never equip a bow. [i]One-Handed and Two-Handed +10 / +20 / +30[/i]
[/list]

[size=4][b]How it works[/b][/size]
[list]
[*][b]Up to three oaths[/b] at once.
[*][b]Blessings grow[/b]: Sworn when you take the oath, Steadfast after 3 in-game days, Unbroken after 7.
[*][b]Breaking an oath[/b]: blessing gone, [b]Oathbreaker[/b] (Health and Stamina −30) for 24 hours, and a 72-hour wait before you can swear that oath again.
[*][b]Renouncing[/b] an oath from the menu costs only the wait, not the penalty.
[*][b]My oaths[/b] shows how long each oath has held, when the next blessing comes, and any cooldowns.
[*]Event-driven scripts with no polling loops: the mod only wakes up when you cast, equip or unequip something, or when a blessing or penalty is due.
[/list]

[size=4][b]Requirements[/b][/size]
[list]
[*]Skyrim Special Edition or Anniversary Edition (1.5.97 or 1.6.x). Works on Steam under Proton on Linux.
[*]Nothing else. No SKSE, no SkyUI, no DLC, no Creation Club content.
[/list]

[size=4][b]Installation[/b][/size]
Install with [b]Mod Organizer 2[/b] or [b]Vortex[/b] (FOMOD installer) and enable [b]SteelOath.esp[/b]. It's ESL-flagged, so it doesn't take a full load-order slot. Safe to add mid-playthrough; the power arrives within a minute (after Helgen on a new game, alternate-start friendly).

[size=4][b]Compatibility[/b][/size]
[list]
[*]No edits to vanilla records, cells or navmesh. Everything is new records, including the magic effects. Skyrim.esm is the only master.
[*]Fully compatible with [b]Blackshield Company[/b], [b]Daedric Ledger: Bounties of Oblivion[/b] and [b]Contract Hunter[/b] (same author).
[*]Works alongside perk overhauls: the blessings are plain skill and attribute bonuses.
[/list]

[size=4][b]Uninstalling[/b][/size]
As with any scripted mod, don't remove it mid-playthrough. If you must, renounce your oaths first and wait out any Oathbreaker mark.

[size=4][b]Known issues / early beta limits[/b][/size]
[list]
[*]Not playtested in-game yet. Found something? Please post it in the Bugs tab.
[*]Only vows the game can detect are offered. Mercy, honesty or "never flee" can't be checked from vanilla scripts, so they're left out on purpose.
[*]The Oath of Steel counts every hand-cast spell. Telling spell schools apart would need SKSE.
[*]Werewolf form doesn't break the Oath of the Shield. Re-equip your shield after changing back.
[*]Settings are console globals for now: [code]set SOT_MaxOaths to 3 / set SOT_Tier2Days to 3 / set SOT_Tier3Days to 7 / set SOT_PenaltyHours to 24 / set SOT_CooldownHours to 72[/code]
[/list]

[size=4][b]Roadmap[/b][/size]
More oaths where the game allows detection, an oath shrine as an alternative to the power, and an MCM.

[size=4][b]Source[/b][/size]
Open source (MIT): [url=https://github.com/synthalorian/steel-oath]github.com/synthalorian/steel-oath[/url]

[size=4][b]Credits[/b][/size]
[list]
[*][url=https://github.com/Mutagen-Modding/Mutagen]Mutagen[/url] by Noggog and contributors, which generates the plugin from code.
[*][url=https://github.com/russo-2025/papyrus-compiler]papyrus-compiler[/url] by russo-2025, the open-source Papyrus compiler.
[*][url=https://github.com/GandaG/fomod-schema]FOMOD schema[/url] by GandaG.
[*]Bethesda Game Studios for Skyrim.
[/list]
```

## Screenshots to capture in-game

Take these at 16:9, with the HUD hidden for scenery shots (`tm` in the console toggles menus).

1. **Hero shot.** A heavily armored warrior with sword and shield at a shrine or on a mountain path. This goes on the mod tile.
2. **The power.** Swear an Oath in Magic > Powers.
3. **Main menu.** The OATHS box.
4. **Swear list.** The five oaths.
5. **Confirmation.** The Oath of the Shield or Oath of Iron confirmation text.
6. **Active effects.** A blessing at Unbroken in Active Effects.
7. **My oaths.** Status with several oaths and a next-blessing timer.
8. **Breaking.** The "Oath broken" notification mid-fight.
9. **Oathbreaker.** The debuff in Active Effects.
