using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roll
{
    public enum TypeOfDie
    {
        D4,
        D6,
        D8,
        D10,
        D12,
        D20,
        D100
    }

    public enum GrowthCurve
    {
        Slow,
        Medium,
        Fast
    }

    public static int RollTheDie(GrowthCurve growth)
    {
        switch (growth)
        {
            case GrowthCurve.Slow:
                return Die(TypeOfDie.D10);
            case GrowthCurve.Medium:
                return Die(TypeOfDie.D20);
            case GrowthCurve.Fast:
                return Die(TypeOfDie.D100);
            default:
                return 0;
        }
    }

    public static int RollTheDie(int nmbrOfRolls, TypeOfDie dieType)
    {
        int results = 0;
        for(int i = nmbrOfRolls; i >= 0; i--)
        {
            results = results + Die(dieType);
        }
        return results;
    }

    public static int RollTheDie(int nmbrOfRolls, TypeOfDie dieType, int modifier)
    {
        int results = 0;
        for (int i = nmbrOfRolls; i >= 0; i--)
        {
            results = results + (Die(dieType) + modifier);
        }
        return results;
    }

    private static int Die(TypeOfDie die)
    {
        switch (die)
        {
            case TypeOfDie.D4:
                return Random.Range(1, 5);
            case TypeOfDie.D6:
                return Random.Range(1, 7);
            case TypeOfDie.D8:
                return Random.Range(1, 9);
            case TypeOfDie.D10:
                return Random.Range(1, 11);
            case TypeOfDie.D12:
                return Random.Range(1, 13);
            case TypeOfDie.D20:
                return Random.Range(1, 21);
            case TypeOfDie.D100:
                return Random.Range(1, 101);
            default:
                return 0;
        }
    }
}
