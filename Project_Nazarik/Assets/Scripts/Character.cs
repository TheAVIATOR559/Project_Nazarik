using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {


    private enum PresetClasses 
    {
        Tank, //STR: 20 | DEX: 08 | CON: 20 | INT: 01 | KNOW: 01 | LUCK: 03 | LVL: 50
        Fighter, //STR: 20 | DEX: 15 | CON: 15 | INT: 01 | KNOW: 01 | LUCK: 01 | LVL: 50
        Healer, //STR: 01 | DEX: 01 | CON: 05 | INT: 20 | KNOW: 20 | LUCK: 06 | LVL: 50
        Rogue, //STR: 15 | DEX: 20 | CON: 05 | INT: 01 | KNOW: 01 | LUCK: 10 | LVL: 50
        Mage, //STR: 01 | DEX: 01 | CON: 08 | INT: 20 | KNOW: 20 | LUCK: 03 | LVL: 50
        Ranger, //STR: 15 | DEX: 20 | CON: 10 | INT: 01 | KNOW: 01 | LUCK: 05 | LVL: 50
        MinStats, //STR: 01 | DEX: 01 | CON: 01 | INT: 01 | KNOW: 01 | LUCK: 01 | LVL: 01
        MaxStats, //STR: 20 | DEX: 20 | CON: 20 | INT: 20 | KNOW: 20 | LUCK: 20 | LVL: 50
    }
    private enum PhysicalWeaponType
    {
        meleeLight,
        meleeMedium,
        meleeHeavy,
        rangedLight,
        rangedHeavy

    }
    private enum MagicWeaponType
    {
        Light = 200,
        Heavy = 125
    }

    [SerializeField] PresetClasses characterClass = PresetClasses.MinStats;
    [SerializeField] PhysicalWeaponType PWT = PhysicalWeaponType.meleeLight;
    [SerializeField] MagicWeaponType MWT = MagicWeaponType.Light;

    ///CORE STATS
    //TODO better way to declare these???
    //Lower bound is 1 upper bound is 20
    [SerializeField] CharacterStat Strength = new CharacterStat(1); //physical damage
    [SerializeField] CharacterStat Dexterity = new CharacterStat(1); //turn order, dodge chance
    [SerializeField] CharacterStat Constitution = new CharacterStat(1); //health, physical damage mitigation
    [SerializeField] CharacterStat Intelligence = new CharacterStat(1); //mana
    [SerializeField] CharacterStat Knowledge = new CharacterStat(1); //magic damage, magic damage mitigation
    [SerializeField] CharacterStat Luck = new CharacterStat(1); //crit chance, status ailment chance

    [SerializeField] int characterLevel = 1; //level of character. upper bound is 50
    [SerializeField] float defendingBoost = 50; //how much the character's defense is boosted by when defending

    /// DERIVED STATS
    public readonly CharacterStat Initiative = new CharacterStat();
    [SerializeField] float[] initiativeBalance = new float[2] { 0.8f, 0.2f }; //0 for dex, 1 for luck
    public readonly CharacterStat DodgeChance = new CharacterStat();
    [SerializeField] float[] dodgeBalance = new float[2] { 0.65f, 0.35f }; //0 for dex, 1 for luck
    public readonly CharacterStat Mana = new CharacterStat();
    [SerializeField] float baseMana = 100;
    public readonly CharacterStat Health = new CharacterStat();
    [SerializeField] float baseHealth = 100;
    public readonly CharacterStat PhysicalDefense = new CharacterStat();
    [SerializeField] float[] physicalDefenseBalance = new float[2] { 0.8f, 0.2f };
    public readonly CharacterStat MagicDefense = new CharacterStat();
    [SerializeField] float[] magicDefenseBalance = new float[2] { 0.8f, 0.2f };
    public readonly CharacterStat CritChance = new CharacterStat();
    [SerializeField] float critBalance = 2;
    public readonly CharacterStat AilmentChance = new CharacterStat();
    [SerializeField] float ailmentBalance = 2;
    public readonly CharacterStat PhysicalDamage = new CharacterStat();
    [SerializeField] float basePhysicalDamage = 100;
    private float[] physicalDamageBalance = new float[2];
    public readonly CharacterStat MagicDamage = new CharacterStat();
    [SerializeField] float baseMagicDamage = 100;
    [SerializeField] float knowBalance = 0.8f;

    void Awake()
    {
        switch (characterClass)
        {
            case PresetClasses.Tank: //STR: 20 | DEX: 08 | CON: 20 | INT: 01 | KNOW: 01 | LUCK: 03 | LVL: 50
                this.Strength.BaseValue = 20;
                this.Dexterity.BaseValue = 08;
                this.Constitution.BaseValue = 20;
                this.Intelligence.BaseValue = 01;
                this.Knowledge.BaseValue = 01;
                this.Luck.BaseValue = 03;
                this.characterLevel = 50;
                break;
            case PresetClasses.Fighter: //STR: 20 | DEX: 15 | CON: 15 | INT: 01 | KNOW: 01 | LUCK: 01 | LVL: 50
                this.Strength.BaseValue = 20;
                this.Dexterity.BaseValue = 15;
                this.Constitution.BaseValue = 15;
                this.Intelligence.BaseValue = 01;
                this.Knowledge.BaseValue = 01;
                this.Luck.BaseValue = 01;
                this.characterLevel = 50;
                break;
            case PresetClasses.Healer: //STR: 01 | DEX: 01 | CON: 05 | INT: 20 | KNOW: 20 | LUCK: 06 | LVL: 50
                this.Strength.BaseValue = 01;
                this.Dexterity.BaseValue = 01;
                this.Constitution.BaseValue = 05;
                this.Intelligence.BaseValue = 20;
                this.Knowledge.BaseValue = 20;
                this.Luck.BaseValue = 06;
                this.characterLevel = 50;
                break;
            case PresetClasses.Mage: //STR: 01 | DEX: 01 | CON: 08 | INT: 20 | KNOW: 20 | LUCK: 03 | LVL: 50
                this.Strength.BaseValue = 01;
                this.Dexterity.BaseValue = 01;
                this.Constitution.BaseValue = 08;
                this.Intelligence.BaseValue = 20;
                this.Knowledge.BaseValue = 20;
                this.Luck.BaseValue = 03;
                this.characterLevel = 50;
                break;
            case PresetClasses.Ranger: //STR: 15 | DEX: 20 | CON: 10 | INT: 01 | KNOW: 01 | LUCK: 05 | LVL: 50
                this.Strength.BaseValue = 15;
                this.Dexterity.BaseValue = 20;
                this.Constitution.BaseValue = 10;
                this.Intelligence.BaseValue = 01;
                this.Knowledge.BaseValue = 01;
                this.Luck.BaseValue = 05;
                this.characterLevel = 50;
                break;
            case PresetClasses.Rogue: //STR: 15 | DEX: 20 | CON: 05 | INT: 01 | KNOW: 01 | LUCK: 10 | LVL: 50
                this.Strength.BaseValue = 15;
                this.Dexterity.BaseValue = 20;
                this.Constitution.BaseValue = 05;
                this.Intelligence.BaseValue = 01;
                this.Knowledge.BaseValue = 01;
                this.Luck.BaseValue = 10;
                this.characterLevel = 50;
                break;
            case PresetClasses.MinStats:
                this.Strength.BaseValue = 01;
                this.Dexterity.BaseValue = 01;
                this.Constitution.BaseValue = 01;
                this.Intelligence.BaseValue = 01;
                this.Knowledge.BaseValue = 01;
                this.Luck.BaseValue = 01;
                this.characterLevel = 01;
                break;
            case PresetClasses.MaxStats:
                this.Strength.BaseValue = 20;
                this.Dexterity.BaseValue = 20;
                this.Constitution.BaseValue = 20;
                this.Intelligence.BaseValue = 20;
                this.Knowledge.BaseValue = 20;
                this.Luck.BaseValue = 20;
                this.characterLevel = 50;
                break;
            default:
                Debug.LogError("Weird Character Class Selection Error Encountered.");
                break;
        }
    }

    void OnEnable()
    {
        //calc stats here
        Initiative.BaseValue = (Dexterity.Value * initiativeBalance[0]) + (Luck.Value * initiativeBalance[1]);
        DodgeChance.BaseValue = Dexterity.Value + (Dexterity.Value * dodgeBalance[0]) + (Luck.Value * dodgeBalance[1]);
        Mana.BaseValue = ((baseMana + characterLevel) * Intelligence.Value) / 10;
        Health.BaseValue = ((baseHealth + Constitution.Value) * characterLevel) * 2.5f;
        PhysicalDefense.BaseValue = ((Constitution.Value + characterLevel) / 2 + defendingBoost) * ((Constitution.Value * physicalDefenseBalance[0]) + (Strength.Value * physicalDefenseBalance[1]));
        MagicDefense.BaseValue = ((Knowledge.Value + characterLevel) / 2 + defendingBoost) * ((Knowledge.Value * magicDefenseBalance[0]) + (Intelligence.Value * magicDefenseBalance[1]));
        CritChance.BaseValue = Luck.Value * critBalance;
        AilmentChance.BaseValue = (Luck.Value * ailmentBalance);
        PhysicalDamage.BaseValue = GetPhysicalDamage();
        MagicDamage.BaseValue = GetMagicDamage();
    }

    ///DERIVED STATS

    private float GetPhysicalDamage()
    {
        float temp;
        switch (PWT)
        {
            case PhysicalWeaponType.meleeLight:
                physicalDamageBalance[0] = 1.0f; //DEX
                physicalDamageBalance[1] = 0.2f; //STR

                temp = basePhysicalDamage * (Dexterity.Value + characterLevel);
                temp = temp / characterLevel;
                //add flat mods here
                //add percent mods here
                temp = temp * ((Dexterity.Value* physicalDamageBalance[0]) + (Strength.Value * physicalDamageBalance[1]));
                temp = Mathf.Round(temp);

                return temp;

            case PhysicalWeaponType.meleeMedium:
                physicalDamageBalance[0] = 1.0f; //DEX
                physicalDamageBalance[1] = 0.65f; //STR

                temp = basePhysicalDamage * (Strength.Value + characterLevel);
                temp = temp / characterLevel;
                //add flat mods here
                //add percent mods here
                temp = temp * ((Dexterity.Value * physicalDamageBalance[0]) + (Strength.Value * physicalDamageBalance[1]));
                temp = Mathf.Round(temp);

                return temp;

            case PhysicalWeaponType.meleeHeavy:
                physicalDamageBalance[0] = 1.0f; //DEX
                physicalDamageBalance[1] = 1.50f; //STR

                temp = basePhysicalDamage * (Strength.Value + characterLevel);
                temp = temp / characterLevel;
                //add flat mods here
                //add percent mods here
                temp = temp * ((Dexterity.Value * physicalDamageBalance[0]) + (Strength.Value * physicalDamageBalance[1]));
                temp = Mathf.Round(temp);

                return temp;

            case PhysicalWeaponType.rangedLight:
                physicalDamageBalance[0] = 1.2f; //DEX

                temp = basePhysicalDamage * (Dexterity.Value + characterLevel);
                temp = temp / characterLevel;
                //add flat mods here
                //add percent mods here
                temp = temp * ((Dexterity.Value * physicalDamageBalance[0]));
                temp = Mathf.Round(temp);

                return temp;

            case PhysicalWeaponType.rangedHeavy:
                physicalDamageBalance[0] = 1.35f; //DEX
                physicalDamageBalance[1] = 1.50f; //STR

                temp = basePhysicalDamage * (Dexterity.Value + characterLevel);
                temp = temp / characterLevel;
                //add flat mods here
                //add percent mods here
                temp = temp * ((Dexterity.Value * physicalDamageBalance[0]) + (Strength.Value * physicalDamageBalance[1]));
                temp = Mathf.Round(temp);

                return temp;

            default:
                return 0;
        }
    }    

    private float GetMagicDamage()
    {
        float temp = baseMagicDamage * ((Knowledge.Value + Intelligence.Value) / ((float)MWT / 100) + characterLevel);
        temp = temp / characterLevel;
        //add flat mods here
        //add precent mods here

        //weapon class damage modifier
        temp = temp * ((Knowledge.Value * knowBalance) + Intelligence.Value);
        temp = Mathf.Round(temp);

        return temp;
    }

    //TESTING STUFF
    public string GetStats()
    {
        string stats = "Class: " + characterClass.ToString() + "\nPhysical Damage: " + PhysicalDamage.Value + "\nMagic Damage: " + MagicDamage.Value + 
            "\nPhysical Defense: " + PhysicalDefense.Value + "\nMagic Defense: " + MagicDefense.Value + "\nHealth: " + Health.Value + "\nMana: " + Mana.Value + 
            "\nIniative: " + Initiative.Value + "\nCrit Chance: " + CritChance.Value + "\nStatus Ailment Chance: " + AilmentChance.Value + "\nDodge Chance: " + DodgeChance.Value;
        return stats;
    }

    //TODO FIND A BETTER WAY TO DO THIS
    private PresetClasses previousClass = PresetClasses.MinStats;
    private void Update()
    {
        if(characterClass != previousClass)
        {
            previousClass = characterClass;

            switch (characterClass)
            {
                case PresetClasses.Tank: //STR: 20 | DEX: 08 | CON: 20 | INT: 01 | KNOW: 01 | LUCK: 03 | LVL: 50
                    this.Strength.BaseValue = 20;
                    this.Dexterity.BaseValue = 08;
                    this.Constitution.BaseValue = 20;
                    this.Intelligence.BaseValue = 01;
                    this.Knowledge.BaseValue = 01;
                    this.Luck.BaseValue = 03;
                    this.characterLevel = 50;
                    break;
                case PresetClasses.Fighter: //STR: 20 | DEX: 15 | CON: 15 | INT: 01 | KNOW: 01 | LUCK: 01 | LVL: 50
                    this.Strength.BaseValue = 20;
                    this.Dexterity.BaseValue = 15;
                    this.Constitution.BaseValue = 15;
                    this.Intelligence.BaseValue = 01;
                    this.Knowledge.BaseValue = 01;
                    this.Luck.BaseValue = 01;
                    this.characterLevel = 50;
                    break;
                case PresetClasses.Healer: //STR: 01 | DEX: 01 | CON: 05 | INT: 20 | KNOW: 20 | LUCK: 06 | LVL: 50
                    this.Strength.BaseValue = 01;
                    this.Dexterity.BaseValue = 01;
                    this.Constitution.BaseValue = 05;
                    this.Intelligence.BaseValue = 20;
                    this.Knowledge.BaseValue = 20;
                    this.Luck.BaseValue = 06;
                    this.characterLevel = 50;
                    break;
                case PresetClasses.Mage: //STR: 01 | DEX: 01 | CON: 08 | INT: 20 | KNOW: 20 | LUCK: 03 | LVL: 50
                    this.Strength.BaseValue = 01;
                    this.Dexterity.BaseValue = 01;
                    this.Constitution.BaseValue = 08;
                    this.Intelligence.BaseValue = 20;
                    this.Knowledge.BaseValue = 20;
                    this.Luck.BaseValue = 03;
                    this.characterLevel = 50;
                    break;
                case PresetClasses.Ranger: //STR: 15 | DEX: 20 | CON: 10 | INT: 01 | KNOW: 01 | LUCK: 05 | LVL: 50
                    this.Strength.BaseValue = 15;
                    this.Dexterity.BaseValue = 20;
                    this.Constitution.BaseValue = 10;
                    this.Intelligence.BaseValue = 01;
                    this.Knowledge.BaseValue = 01;
                    this.Luck.BaseValue = 05;
                    this.characterLevel = 50;
                    break;
                case PresetClasses.Rogue: //STR: 15 | DEX: 20 | CON: 05 | INT: 01 | KNOW: 01 | LUCK: 10 | LVL: 50
                    this.Strength.BaseValue = 15;
                    this.Dexterity.BaseValue = 20;
                    this.Constitution.BaseValue = 05;
                    this.Intelligence.BaseValue = 01;
                    this.Knowledge.BaseValue = 01;
                    this.Luck.BaseValue = 10;
                    this.characterLevel = 50;
                    break;
                case PresetClasses.MinStats:
                    this.Strength.BaseValue = 01;
                    this.Dexterity.BaseValue = 01;
                    this.Constitution.BaseValue = 01;
                    this.Intelligence.BaseValue = 01;
                    this.Knowledge.BaseValue = 01;
                    this.Luck.BaseValue = 01;
                    this.characterLevel = 01;
                    break;
                case PresetClasses.MaxStats:
                    this.Strength.BaseValue = 20;
                    this.Dexterity.BaseValue = 20;
                    this.Constitution.BaseValue = 20;
                    this.Intelligence.BaseValue = 20;
                    this.Knowledge.BaseValue = 20;
                    this.Luck.BaseValue = 20;
                    this.characterLevel = 50;
                    break;
                default:
                    Debug.LogError("Weird Character Class Selection Error Encountered.");
                    break;
            }
        }
    }
}
