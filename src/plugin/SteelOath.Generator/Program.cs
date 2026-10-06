using Modkit.Mutagen;
using Mutagen.Bethesda;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
using Noggog;
using static SteelOath.Generator.Vanilla;

namespace SteelOath.Generator;

/// <summary>
/// Generates SteelOath.esp (ESL-flagged) from code.
/// Usage: generator --out &lt;dir&gt; --ids &lt;formids.json&gt; --papyrus &lt;src/papyrus&gt; [--report &lt;file&gt;]
/// </summary>
internal static class Program
{
    public const string PluginName = "SteelOath.esp";
    private static readonly SkyrimRelease Release = SkyrimRelease.SkyrimSE;

    private static int Main(string[] args)
    {
        string Arg(string name, string fallback)
        {
            var i = Array.IndexOf(args, name);
            return i >= 0 && i + 1 < args.Length ? args[i + 1] : fallback;
        }
        var outDir = Arg("--out", "build/plugin");
        var idsPath = Arg("--ids", "src/plugin/formids.json");
        var papyrusDir = Arg("--papyrus", "src/papyrus");
        var reportPath = Arg("--report", Path.Combine(outDir, "plugin-report.json"));
        Directory.CreateDirectory(outDir);

        var modKey = ModKey.FromFileName(PluginName);
        var ids = new FormIdRegistry(idsPath, modKey);
        var mod = new SkyrimMod(modKey, Release);
        var builder = new Builder(mod, ids);
        builder.Build();

        mod.ModHeader.Flags |= SkyrimModHeader.HeaderFlag.Small; // ESL-flagged ESP
        // Header 1.70 (not 1.71): FormIDs start at 0x800, so this loads on every SE/AE
        // runtime, including 1.5.97 and 1.6.640, without Backported Extended ESL Support.
        mod.ModHeader.Stats.Version = 1.7f;
        mod.ModHeader.Author = "synthalorian";
        mod.ModHeader.Description = "Steel Oath: warrior oaths sworn through a lesser power, with growing blessings and oathbreaker penalties.";

        var outPath = Path.Combine(outDir, PluginName);
        mod.BeginWrite
            .ToPath(outPath)
            .WithNoLoadOrder()
            .Write();
        ids.Save();
        foreach (var r in ids.Retired) Console.WriteLine($"[generate] note: retired EditorID kept reserved: {r}");
        Console.WriteLine($"[generate] wrote {outPath}");

        var errors = PluginValidator.Validate(outPath, new PapyrusIndex(papyrusDir), new HashSet<ModKey> { Skyrim }, reportPath, builder.ExpectedUnfilled);
        return errors == 0 ? 0 : 1;
    }
}
