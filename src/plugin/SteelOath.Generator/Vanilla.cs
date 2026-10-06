using Mutagen.Bethesda.Plugins;

namespace SteelOath.Generator;

/// <summary>
/// Vanilla Skyrim.esm records this mod links to. Only FormIDs are referenced;
/// no Bethesda data or assets are copied into the plugin or the repo.
/// FormIDs were looked up with the Mutagen.Bethesda.FormKeys.SkyrimSE package.
/// </summary>
internal static class Vanilla
{
    public static readonly ModKey Skyrim = ModKey.FromFileName("Skyrim.esm");
    private static FormKey K(uint id) => new(Skyrim, id);

    public static readonly FormKey PlayerRef = K(0x000014);
    public static readonly FormKey MQ101 = K(0x03372B);
    public static readonly FormKey WerewolfBeastRace = K(0x0CDD84);

    // Keywords used to recognise what the player equips
    public static readonly FormKey ArmorLight = K(0x06BBD3);
    public static readonly FormKey ArmorShield = K(0x0965B2);
    public static readonly FormKey WeapTypeBow = K(0x01E715);

    // Equip slots
    public static readonly FormKey EquipVoice = K(0x025BEE);
    public static readonly FormKey EquipEitherHand = K(0x013F44);
}
