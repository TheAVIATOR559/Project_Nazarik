public class StatModifier {

    public readonly float Value;
    public readonly StatModType Type;
    public readonly int Order;
    public readonly object Source;

    public enum StatModType
    {
        Flat = 100,
        PercentAdd = 200,
        PercentMult = 300,
    }

    //main constructor
    public StatModifier(float value, StatModType type, int order, object source)
    {
        Value = value;
        Type = type;
        Order = order;
        Source = source;
    }

    //requires value and type
    public StatModifier(float value, StatModType type) : this(value, type, (int)type, null)
    {
        //doesnt need anything in here
    }

    //requires value, type and order
    public StatModifier(float value, StatModType type, int order) : this(value, type, order, null)
    {
        //doesnt need anything in here
    }

    //requires value, type and source
    public StatModifier(float value, StatModType type, object source) : this(value, type, (int)type, source)
    {
        //doesnt need anything in here
    }
}
