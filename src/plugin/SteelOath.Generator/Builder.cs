using Modkit.Mutagen;
using Mutagen.Bethesda;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Records;
using Mutagen.Bethesda.Skyrim;
using Noggog;
using static SteelOath.Generator.Vanilla;

namespace SteelOath.Generator;

internal sealed class Builder
{
    private readonly SkyrimMod _mod;
    private readonly FormIdRegistry _ids;
    private static readonly SkyrimRelease R = SkyrimRelease.SkyrimSE;

    // Quest DNAM form version written by the SE Creation Kit (Mutagen defaults to 255).
    private const byte VanillaQuestFormVersion = 65;
    private const int AliasPlayer = 0;

    /// <summary>Script properties deliberately left unfilled ("EditorID:Script.Property"), for the validator.</summary>
    public HashSet<string> ExpectedUnfilled { get; } = new();

    public Builder(SkyrimMod mod, FormIdRegistry ids)
    {
        _mod = mod;
        _ids = ids;
    }

    private FormKey Id(string editorId) => _ids.Get(editorId);
    private static FormLink<T> L<T>(FormKey k) where T : class, IMajorRecordGetter => new(k);

    // ----------------------------------------------------------------------------------------
    public void Build()
    {
        var questOath = Id("SOT_OathQuest");
        var oaths = Texts.Oaths;

        // --- Globals ---------------------------------------------------------------------------
        var gGranted = Global("SOT_PowerGranted", 0);
        var gActive = oaths.Select(o => Global($"SOT_Active{o.Key}", 0)).ToArray();
        var gAvailable = oaths.Select(o => Global($"SOT_Available{o.Key}", 1)).ToArray();
        var gActiveCount = Global("SOT_ActiveCount", 0);
        var gCanSwear = Global("SOT_CanSwear", 1);
        var gMax = Global("SOT_MaxOaths", 3);
        var gBroken = Global("SOT_OathsBroken", 0);
        var gLongest = Global("SOT_LongestOathDays", 0);
        var gTier2 = Global("SOT_Tier2Days", 3);
        var gTier3 = Global("SOT_Tier3Days", 7);
        var gCooldown = Global("SOT_CooldownHours", 72);
        var gPenalty = Global("SOT_PenaltyHours", 24);

        // --- Magic effects -----------------------------------------------------------------------
        var mgefPower = Add(new MagicEffect(Id("SOT_SwearEffect"), R)
        {
            EditorID = "SOT_SwearEffect",
            Name = "Swear an Oath",
            Flags = MagicEffect.Flag.NoDuration | MagicEffect.Flag.NoMagnitude | MagicEffect.Flag.NoArea
                    | MagicEffect.Flag.Painless | MagicEffect.Flag.NoHitEvent | MagicEffect.Flag.HideInUI,
            MagicSkill = ActorValue.None, ResistValue = ActorValue.None, SecondActorValue = ActorValue.None,
            Archetype = new MagicEffectArchetype { Type = MagicEffectArchetype.TypeEnum.Script, ActorValue = ActorValue.None },
            CastType = CastType.FireAndForget,
            TargetType = TargetType.Self,
            CastingSoundLevel = SoundLevel.Normal,
            Description = "Swear, review or renounce a warrior's oath.",
            VirtualMachineAdapter = Vmad.Adapter(Vmad.Script("SOT_PowerEffectScript", Vmad.Obj("Oaths", questOath))),
        });

        var statMgefs = new Dictionary<string, FormKey>
        {
            ["SOT_FortHealth"] = ValueEffect("SOT_FortHealth", "Oath: Health", ActorValue.Health, false, "Health is increased by <mag> points."),
            ["SOT_FortBlock"] = ValueEffect("SOT_FortBlock", "Oath: Block", ActorValue.Block, false, "Blocking is <mag>% more effective."),
            ["SOT_FortArmor"] = ValueEffect("SOT_FortArmor", "Oath: Armor", ActorValue.DamageResist, false, "Armor rating is increased by <mag>."),
            ["SOT_FortStamina"] = ValueEffect("SOT_FortStamina", "Oath: Stamina", ActorValue.Stamina, false, "Stamina is increased by <mag> points."),
            ["SOT_FortOneHanded"] = ValueEffect("SOT_FortOneHanded", "Oath: One-Handed", ActorValue.OneHanded, false, "One-handed weapons do <mag>% more damage."),
            ["SOT_FortTwoHanded"] = ValueEffect("SOT_FortTwoHanded", "Oath: Two-Handed", ActorValue.TwoHanded, false, "Two-handed weapons do <mag>% more damage."),
        };
        var dmgHealth = ValueEffect("SOT_OathbreakerHealth", "Oathbreaker", ActorValue.Health, true, "Health is reduced by <mag> points.");
        var dmgStamina = ValueEffect("SOT_OathbreakerStamina", "Oathbreaker", ActorValue.Stamina, true, "Stamina is reduced by <mag> points.");

        // --- Spells ------------------------------------------------------------------------------
        var power = Add(new Spell(Id("SOT_PowerOath"), R)
        {
            EditorID = "SOT_PowerOath",
            Name = "Swear an Oath",
            Description = "Swear a warrior's oath, review the oaths you hold, or renounce one.",
            Type = SpellType.LesserPower,
            CastType = CastType.FireAndForget,
            TargetType = TargetType.Self,
            Flags = SpellDataFlag.ManualCostCalc | SpellDataFlag.NoAbsorbOrReflect,
            EquipmentType = L<IEquipTypeGetter>(EquipVoice).AsNullable(),
            Effects = { Effect(mgefPower.FormKey, 0, 0) },
        });

        // Tier abilities: [tier][oath]
        var abilities = new FormKey[3][];
        for (int t = 0; t < 3; t++)
        {
            abilities[t] = new FormKey[oaths.Length];
            for (int o = 0; o < oaths.Length; o++)
            {
                var oath = oaths[o];
                var edid = $"SOT_Ab{oath.Key}{t + 1}";
                var sp = Ability(edid, $"{oath.Name} ({Texts.TierNames[t]})", $"{oath.Vow}");
                foreach (var s in oath.Stats)
                    sp.Effects.Add(Effect(statMgefs[s.Mgef], s.PerTier[t], 0));
                abilities[t][o] = Add(sp).FormKey;
            }
        }
        var abBreaker = Ability("SOT_AbOathbreaker", "Oathbreaker", "You broke a sworn oath.");
        abBreaker.Effects.Add(Effect(dmgHealth, 30, 0));
        abBreaker.Effects.Add(Effect(dmgStamina, 30, 0));
        Add(abBreaker);

        // --- Messages ----------------------------------------------------------------------------
        var msgIntro = Msg("SOT_MsgIntro", Texts.Intro);
        var msgMain = Msg("SOT_MsgMainMenu", Texts.MainMenu,
            Btn("Swear an oath", Cond.Global(gCanSwear, CompareOperator.EqualTo, 1)),
            Btn("Renounce an oath", Cond.Global(gActiveCount, CompareOperator.GreaterThan, 0)),
            Btn("My oaths"),
            Btn("Close"));
        var swearButtons = oaths.Select((o, i) => Btn(o.Name, Cond.Global(gAvailable[i], CompareOperator.EqualTo, 1))).ToList();
        swearButtons.Add(Btn("Back"));
        var msgSwear = Msg("SOT_MsgSwearMenu", Texts.SwearMenu, swearButtons.ToArray());
        var renounceButtons = oaths.Select((o, i) => Btn(o.Name, Cond.Global(gActive[i], CompareOperator.EqualTo, 1))).ToList();
        renounceButtons.Add(Btn("Back"));
        var msgRenounce = Msg("SOT_MsgRenounceMenu", Texts.RenounceMenu, renounceButtons.ToArray());
        var msgConfirm = oaths.Select(o => Msg($"SOT_MsgConfirm{o.Key}", o.Confirm, Btn("Swear it"), Btn("Back"))).ToArray();

        // --- Controller quest --------------------------------------------------------------------
        var quest = Add(new Quest(questOath, R)
        {
            EditorID = "SOT_OathQuest",
            Name = "Steel Oath",
            Flags = Quest.Flag.StartGameEnabled,
            Priority = 50,
            Type = Quest.TypeEnum.None,
            QuestFormVersion = VanillaQuestFormVersion,
        });
        quest.Aliases.Add(new QuestAlias
        {
            ID = AliasPlayer, Type = QuestAlias.TypeEnum.Reference, Name = "Player",
            ForcedReference = new FormLinkNullable<IPlacedGetter>(PlayerRef),
        });
        quest.NextAliasID = (uint)quest.Aliases.Count;
        quest.VirtualMachineAdapter = Vmad.QuestAdapter(Vmad.Script("SOT_OathScript",
                Vmad.Obj("SOT_PowerGranted", gGranted),
                Vmad.ObjList("ActiveFlags", gActive), Vmad.ObjList("AvailableFlags", gAvailable),
                Vmad.Obj("SOT_ActiveCount", gActiveCount), Vmad.Obj("SOT_CanSwear", gCanSwear), Vmad.Obj("SOT_MaxOaths", gMax),
                Vmad.Obj("SOT_OathsBroken", gBroken), Vmad.Obj("SOT_LongestOathDays", gLongest),
                Vmad.Obj("SOT_Tier2Days", gTier2), Vmad.Obj("SOT_Tier3Days", gTier3),
                Vmad.Obj("SOT_CooldownHours", gCooldown), Vmad.Obj("SOT_PenaltyHours", gPenalty),
                Vmad.ObjList("Tier1", abilities[0]), Vmad.ObjList("Tier2", abilities[1]), Vmad.ObjList("Tier3", abilities[2]),
                Vmad.Obj("SOT_AbOathbreaker", abBreaker.FormKey), Vmad.Obj("SOT_PowerOath", power.FormKey),
                Vmad.Obj("MQ101", MQ101), Vmad.Obj("WerewolfBeastRace", WerewolfBeastRace),
                Vmad.Obj("ArmorLight", ArmorLight), Vmad.Obj("ArmorShield", ArmorShield), Vmad.Obj("WeapTypeBow", WeapTypeBow),
                Vmad.Obj("SOT_MsgIntro", msgIntro), Vmad.Obj("SOT_MsgMainMenu", msgMain),
                Vmad.Obj("SOT_MsgSwearMenu", msgSwear), Vmad.Obj("SOT_MsgRenounceMenu", msgRenounce),
                Vmad.ObjList("ConfirmMessages", msgConfirm)))
            .WithAliasScripts(questOath, AliasPlayer, Vmad.Script("SOT_PlayerAliasScript", Vmad.Obj("Oaths", questOath)));
    }

    // ----------------------------------------------------------------------------------------
    // Record helpers
    // ----------------------------------------------------------------------------------------

    private T Add<T>(T record) where T : SkyrimMajorRecord
    {
        _mod.GetTopLevelGroup<T>().Add(record);
        return record;
    }

    private FormKey Global(string edid, float value)
    {
        var g = new GlobalFloat(Id(edid), R) { EditorID = edid, Data = value };
        _mod.Globals.Add(g);
        return g.FormKey;
    }

    private FormKey ValueEffect(string edid, string name, ActorValue av, bool detrimental, string description)
    {
        var flags = MagicEffect.Flag.Recover | MagicEffect.Flag.NoArea | MagicEffect.Flag.NoDuration | MagicEffect.Flag.NoHitEvent;
        if (detrimental) flags |= MagicEffect.Flag.Detrimental;
        var m = new MagicEffect(Id(edid), R)
        {
            EditorID = edid,
            Name = name,
            Flags = flags,
            MagicSkill = ActorValue.None, ResistValue = ActorValue.None, SecondActorValue = ActorValue.None,
            Archetype = new MagicEffectArchetype { Type = MagicEffectArchetype.TypeEnum.ValueModifier, ActorValue = av },
            CastType = CastType.ConstantEffect,
            TargetType = TargetType.Self,
            CastingSoundLevel = SoundLevel.Normal,
            Description = description,
        };
        _mod.MagicEffects.Add(m);
        return m.FormKey;
    }

    private Spell Ability(string edid, string name, string description) => new(Id(edid), R)
    {
        EditorID = edid,
        Name = name,
        Description = description,
        Type = SpellType.Ability,
        CastType = CastType.ConstantEffect,
        TargetType = TargetType.Self,
        Flags = SpellDataFlag.ManualCostCalc | SpellDataFlag.NoAbsorbOrReflect,
        EquipmentType = L<IEquipTypeGetter>(EquipEitherHand).AsNullable(),
    };

    private static Effect Effect(FormKey mgef, float magnitude, int duration) => new()
    {
        BaseEffect = new FormLinkNullable<IMagicEffectGetter>(mgef),
        Data = new EffectData { Magnitude = magnitude, Area = 0, Duration = duration },
    };

    private static MessageButton Btn(string text, params Condition[] conditions)
    {
        var b = new MessageButton { Text = text };
        b.Conditions.AddRange(conditions);
        return b;
    }

    private FormKey Msg(string edid, string text, params MessageButton[] buttons)
    {
        if (buttons.Length > 10) throw new InvalidOperationException($"{edid}: a message box holds at most 10 buttons");
        var m = new Message(Id(edid), R)
        {
            EditorID = edid,
            Description = text,
            Flags = Message.Flag.MessageBox,
            INAM = new byte[4],
        };
        m.MenuButtons.AddRange(buttons);
        _mod.Messages.Add(m);
        return m.FormKey;
    }
}
