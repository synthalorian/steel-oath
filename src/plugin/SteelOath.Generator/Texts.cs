namespace SteelOath.Generator;

/// <summary>All player-facing text, kept in one place.</summary>
internal static class Texts
{
    public sealed record Stat(string Mgef, string Label, float[] PerTier);

    public sealed record Oath(string Key, string Name, string Vow, string Confirm, Stat[] Stats);

    public static readonly string[] TierNames = { "Sworn", "Steadfast", "Unbroken" };

    // Order matters: it is the oath index used by the scripts (0..4).
    public static readonly Oath[] Oaths =
    {
        new("Steel", "Oath of Steel", "Cast no spell from your hands and never ready a scroll.",
            "OATH OF STEEL\n\nTrust the blade, not the art. Cast no spell from your hands, and never ready a scroll.\n\nPowers, shouts, staves, potions and enchanted gear are allowed.\n\nBlessing: Health +20, rising to +40 after %.0f days and +60 after %.0f days.",
            new[] { new Stat("SOT_FortHealth", "Health", new[] { 20f, 40f, 60f }) }),
        new("Shield", "Oath of the Shield", "Keep a shield on your arm.",
            "OATH OF THE SHIELD\n\nNever set your shield aside. If it leaves your arm and isn't back within a few heartbeats, the oath is broken. Swapping one shield for another is fine.\n\nYou must be carrying a shield to swear it.\n\nBlessing: Block +10, rising to +20 after %.0f days and +30 after %.0f days.",
            new[] { new Stat("SOT_FortBlock", "Block", new[] { 10f, 20f, 30f }) }),
        new("Iron", "Oath of Iron", "Wear no light armor, light shields included.",
            "OATH OF IRON\n\nBear the weight. Never put on a piece of light armor; light shields count too. Heavy armor, heavy shields and clothing are fine.\n\nYou must take off any light armor before you swear it.\n\nBlessing: Armor +30, rising to +60 after %.0f days and +100 after %.0f days.",
            new[] { new Stat("SOT_FortArmor", "Armor", new[] { 30f, 60f, 100f }) }),
        new("Endurance", "Oath of Endurance", "Take no potion, food or poison while in combat.",
            "OATH OF ENDURANCE\n\nFight on what you brought into the fight. While in combat, drink no potion, eat no food, and apply no poison. Outside combat, anything goes.\n\nBlessing: Stamina +20, rising to +40 after %.0f days and +60 after %.0f days.",
            new[] { new Stat("SOT_FortStamina", "Stamina", new[] { 20f, 40f, 60f }) }),
        new("Close", "Oath of Close Quarters", "Never equip a bow.",
            "OATH OF CLOSE QUARTERS\n\nFace your enemies at arm's length. Never equip a bow.\n\nYou must put away your bow before you swear it.\n\nBlessing: One-Handed and Two-Handed +10, rising to +20 after %.0f days and +30 after %.0f days.",
            new[] { new Stat("SOT_FortOneHanded", "One-Handed", new[] { 10f, 20f, 30f }), new Stat("SOT_FortTwoHanded", "Two-Handed", new[] { 10f, 20f, 30f }) }),
    };

    public const string Intro =
        "You feel the weight of old vows settle on your shoulders. The warriors of the old songs swore oaths, and kept them.\n\n(New lesser power: Swear an Oath.)";

    public const string MainMenu =
        "OATHS\n\nActive oaths: %.0f / %.0f\nOaths broken: %.0f\nLongest oath kept: %.0f days";

    public const string SwearMenu =
        "Which oath will you swear? Oaths you already hold, or broke or renounced recently, are not offered.\n\nSTEEL: no spells from your hands, no scrolls.\nSHIELD: keep a shield on your arm.\nIRON: no light armor or light shields.\nENDURANCE: no potions, food or poisons in combat.\nCLOSE QUARTERS: no bows.";

    public const string RenounceMenu =
        "Renounce an oath? You lose its blessing, but you are not cursed as an oathbreaker. You can't swear it again for %.0f hours.";
}
