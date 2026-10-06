# modkit: shared Skyrim SE build tooling

The build tooling for the Blackshield mod series, written to be reused by every mod in it. Everything runs natively on Linux. No Creation Kit, Wine or Windows needed.

| Path | What it is |
|---|---|
| `Modkit.Mutagen/` | C# class library on top of [Mutagen](https://github.com/Mutagen-Modding/Mutagen) 0.54 (.NET 9). |
| `Modkit.Mutagen/FormIdRegistry.cs` | Stable FormID allocation from a committed `formids.json`, starting at 0x800 so the plugin stays ESL-safe. |
| `Modkit.Mutagen/Vmad.cs` | Script attachment helpers: object, alias, array, int, float and bool properties. |
| `Modkit.Mutagen/Cond.cs` | Condition helpers (globals, location keyword/ref type, location form lists). |
| `Modkit.Mutagen/PapyrusIndex.cs` + `PluginValidator.cs` | Re-reads a written plugin and checks the ESL range, masters, dangling links, quest alias references, `<Alias=…>`/`<Global=…>` text tags against the quest's aliases and Text Display Globals, message box button counts, and every script property against the `.psc` sources. Properties left unfilled on purpose can be passed as `expectedUnfilled` so real gaps still stand out as warnings. Ends with a byte-exact round trip. Writes a JSON report. |
| `papyrus/build-papyrus.sh` | Compiles `.psc` to `.pex` with the open-source [russo-2025/papyrus-compiler](https://github.com/russo-2025/papyrus-compiler), pinned and checksum-verified. `-H <dir>` type-checks against the game's real sources. `--official <CK dir>` uses Bethesda's PapyrusCompiler.exe through Wine (untested). |
| `papyrus/pex_normalize.py` | Strips the user name, machine name and timestamp from `.pex` headers for reproducible builds. |
| `papyrus-stubs/` | Clean-room declarations of the vanilla Papyrus API subset used so far. These are signatures only, written from the public API docs, not Bethesda's sources. **Add new functions here as later mods need them**, matching the vanilla signature exactly, including default arguments, because the compiler bakes defaults into the call site. |
| `fomod/pack.py` | Validates `ModuleConfig.xml` against `fomod/schema/ModuleConfig.xsd` (GandaG, MIT, one typo fixed) and checks that every source path exists. Writes a deterministic zip. |

## Starting the next mod

1. Copy `tools/modkit/` into the new repo as-is (or extract it into its own repo and add it as a git submodule once a second mod uses it).
2. Copy the shape of `src/plugin/BlackshieldCompany.Generator` (`Program.cs` stays almost identical; records go in a `Builder`), `build.sh`, `fomod/`, and `CHANGELOG.md` (the build reads the version from the first `## [x.y.z]` heading).
3. Use a new EditorID prefix per mod (`BSC_`, `BOO_`, …) and start with an empty `formids.json`.
4. Keep the copies of `tools/modkit/` in sync between repos when you change it.

## Changelog

- **Bounties of Oblivion (mod 2):** validator checks quest text tags, message button counts and an expected-unfilled list. Stubs gained `Form.RegisterForSleep`/`UnregisterForSleep`, `OnSleepStart`/`OnSleepStop`, `Quest.UpdateCurrentInstanceGlobal` and `Potion`. Removed `Form.GetName`, which is an SKSE addition and must not be used by mods that promise "no SKSE". Generators set `QuestFormVersion = 65` (what the SE Creation Kit writes; Mutagen's default is 255).
- **Blackshield Company (mod 1):** initial version.

## Notes

- Mutagen writes the plugin with `WithNoLoadOrder()`. Only reference masters you list in the validator's allowed set (Skyrim.esm here).
- Vanilla FormIDs were looked up with the `Mutagen.Bethesda.FormKeys.SkyrimSE` NuGet package. Mesh paths and record shapes were checked against public Spriggit exports of vanilla records. No Skyrim.esm was needed on the build machine.
- To check the stubs against real headers on a machine with the game installed: unpack `Data/Scripts.zip` and run `./build.sh -H "<Skyrim>/Data/Source/Scripts"`.
