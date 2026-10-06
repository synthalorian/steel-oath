using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;

namespace Modkit.Mutagen;

/// <summary>Condition helpers. Every condition defaults to "run on Subject", "== 1".</summary>
public static class Cond
{
    public static ConditionFloat Make(ConditionData data, float value = 1, CompareOperator op = CompareOperator.EqualTo, bool or = false)
        => new()
        {
            Data = data,
            CompareOperator = op,
            ComparisonValue = value,
            Flags = or ? Condition.Flag.OR : 0,
        };

    public static ConditionFloat Global(FormKey global, CompareOperator op, float value, bool or = false)
    {
        var d = new GetGlobalValueConditionData();
        d.Global.Link.SetTo(global);
        return Make(d, value, op, or);
    }

    public static ConditionFloat LocationHasKeyword(FormKey keyword, bool or = false)
    {
        var d = new LocationHasKeywordConditionData();
        d.Keyword.Link.SetTo(keyword);
        return Make(d, 1, CompareOperator.EqualTo, or);
    }

    public static ConditionFloat LocationHasRefType(FormKey refType)
    {
        var d = new LocationHasRefTypeConditionData();
        d.LocationReferenceType.Link.SetTo(refType);
        return Make(d);
    }

    public static ConditionFloat InCurrentLocFormList(FormKey formList)
    {
        var d = new GetInCurrentLocFormListConditionData();
        d.FormList.Link.SetTo(formList);
        return Make(d);
    }
}
