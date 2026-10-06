# Steel Oath

A Skyrim Special Edition mod about warrior's vows. Swear an oath through a lesser power, and keep it: the longer it holds, the stronger its blessing grows. Break it and you're marked as an oathbreaker for a day, and the oath won't take you back for three.

**Status: v0.1.0, early beta.** It passes every automated check in the build (Papyrus compile, plugin validation, byte-exact round trip, FOMOD schema) but has not been playtested in-game yet. Expect rough edges and please report what you find. See [docs/TESTING.md](docs/TESTING.md).

- Game: Skyrim Special Edition / Anniversary Edition 1.6.x (also fine on 1.5.97). Works on Windows and on Linux through Proton.
- Requirements: none beyond the base game. No SKSE, no SkyUI, no DLC, no Creation Club content.
- Plugin: `SteelOath.esp`, ESL-flagged, Skyrim.esm is the only master. No vanilla records edited.
- Compatible with [Blackshield Company](https://github.com/synthalorian/blackshield-company), [Daedric Ledger: Bounties of Oblivion](https://github.com/synthalorian/bounties-of-oblivion) and [Contract Hunter](https://github.com/synthalorian/contract-hunter): separate prefix (`SOT_`), records and globals.

## The oaths

Every vow is something the game reports to scripts without SKSE, so the mod can tell for certain when you break it.

| Oath | Vow | Blessing (Sworn / Steadfast / Unbroken) |
|---|---|---|
| Oath of Steel | Cast no spell from your hands, never ready a scroll. Powers, shouts, staves, potions and enchanted gear are fine. | Health +20 / +40 / +60 |
| Oath of the Shield | Keep a shield on your arm. A 3-second grace lets you swap shields. You need a shield to swear it. | Block +10 / +20 / +30 |
| Oath of Iron | Wear no light armor, light shields included. Heavy armor and clothing are fine. | Armor +30 / +60 / +100 |
| Oath of Endurance | No potions, food or poisons while in combat. | Stamina +20 / +40 / +60 |
| Oath of Close Quarters | Never equip a bow. | One-Handed and Two-Handed +10 / +20 / +30 |

- **Up to three oaths at once.**
- **Blessings grow**: Sworn when you swear, Steadfast after 3 in-game days, Unbroken after 7.
- **Breaking an oath** ends it and its blessing, makes you an **Oathbreaker** (Health −30, Stamina −30) for 24 in-game hours, and puts that oath on a 72-hour cooldown.
- **Renouncing** an oath from the menu ends it without the penalty, but the cooldown still applies.
- The menu's *My oaths* shows how long each oath has held, when the next blessing comes, cooldowns and any time left as an oathbreaker.

## Install

Grab `SteelOath-<version>.zip` from [Releases](https://github.com/synthalorian/steel-oath/releases) (FOMOD installer).

- **Mod Organizer 2**: *Install a new mod from an archive*, pick the zip, enable `SteelOath.esp`.
- **Vortex**: drag the zip into the Mods page, install and enable it.
- **Manual (Linux/Proton)**: copy everything inside `00 Core/` into `.../steamapps/common/Skyrim Special Edition/Data/`, then add `*SteelOath.esp` to `.../steamapps/compatdata/489830/pfx/drive_c/users/steamuser/AppData/Local/Skyrim Special Edition/Plugins.txt`.

Safe to add to an existing save: the power arrives within a minute of loading (after Helgen on a new game). Renounce your oaths before uninstalling.

## Console tunables

```
set SOT_MaxOaths to 3          (oaths held at once)
set SOT_Tier2Days to 3         (days to Steadfast)
set SOT_Tier3Days to 7         (days to Unbroken)
set SOT_PenaltyHours to 24     (oathbreaker duration)
set SOT_CooldownHours to 72    (before a broken or renounced oath can be sworn again)
```

Changes to the day and hour values apply to oaths sworn or broken after the change, and to the next scheduled check.

## Build from source

`./build.sh` on Linux builds the release zip from this repo: Papyrus is compiled with the open-source [papyrus-compiler](https://github.com/russo-2025/papyrus-compiler), the plugin is generated from C# with [Mutagen](https://github.com/Mutagen-Modding/Mutagen) and validated (ESL range, masters, links, script properties, script base types, alias scripts, byte-exact round trip), and the FOMOD is schema-checked and zipped reproducibly. Needs .NET SDK 9+, python3, curl and optionally xmllint. See [tools/modkit/README.md](tools/modkit/README.md).

`src/plugin/formids.json` pins every record's FormID. Never edit or reuse entries.

## Layout

```
src/papyrus/      Papyrus sources (.psc)
src/plugin/       Mutagen plugin generator + pinned FormIDs
fomod/            FOMOD installer config
docs/             Testing checklist, design notes, Nexus page and upload fields
tools/modkit/     Shared tooling for the mod series
build.sh          One-command build
```

## Credits and licenses

- Code in this repo: MIT (see `LICENSE`).
- [Mutagen](https://github.com/Mutagen-Modding/Mutagen) by Noggog and contributors (GPL-3.0), used as a build tool, not redistributed.
- [papyrus-compiler](https://github.com/russo-2025/papyrus-compiler) by russo-2025 (MIT), downloaded at build time.
- FOMOD schema from [GandaG/fomod-schema](https://github.com/GandaG/fomod-schema) (MIT).
- Skyrim and its assets belong to Bethesda Softworks. This repo contains no Bethesda assets or script sources; the plugin references vanilla records by FormID.
