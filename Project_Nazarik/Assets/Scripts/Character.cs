using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {


    private enum PresetClasses //change these to the actual character classes once figured out
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

    [SerializeField] PresetClasses characterClass = PresetClasses.MinStats;

    ///CORE STATS
    //TODO better way to declare these???
    //Lower bound is 0 upper bound is 20
    [SerializeField] CharacterStat Strength = new CharacterStat(0); //physical damage
    [SerializeField] CharacterStat Dexterity = new CharacterStat(0); //turn order, dodge chance
    [SerializeField] CharacterStat Constitution = new CharacterStat(0); //health, physical damage mitigation
    [SerializeField] CharacterStat Intelligence = new CharacterStat(0); //mana
    [SerializeField] CharacterStat Knowledge = new CharacterStat(0); //magic damage, magic damage mitigation
    [SerializeField] CharacterStat Luck = new CharacterStat(0); //crit chance, status ailment chance

    //upper bound is 50
    [SerializeField] int characterLevel = 0; //level of character
    [SerializeField] float defendingBoost = 0; //how much the character's defense is boosted by when defending

    ///DERIVED STATS
    #region Physical Damage
    [SerializeField] float basePhysicalDamage = 0;
    private float[] physicalDamageBalance = new float[2]; //0 for DEX, 1 for STR

    private enum PhysicalWeaponType
    {
        meleeLight,
        meleeMedium,
        meleeHeavy,
        rangedLight,
        rangedHeavy

    }
    [SerializeField] PhysicalWeaponType PWT = PhysicalWeaponType.meleeLight;
    protected CharacterStat PhysicalDamage = new CharacterStat();

    public float GetPhysicalDamage()
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
                PhysicalDamage.BaseValue = temp;

                return PhysicalDamage.Value;

            case PhysicalWeaponType.meleeMedium:
                physicalDamageBalance[0] = 1.0f; //DEX
                physicalDamageBalance[1] = 0.65f; //STR

                temp = basePhysicalDamage * (Strength.Value + characterLevel);
                temp = temp / characterLevel;
                //add flat mods here
                //add percent mods here
                temp = temp * ((Dexterity.Value * physicalDamageBalance[0]) + (Strength.Value * physicalDamageBalance[1]));
                temp = Mathf.Round(temp);
                PhysicalDamage.BaseValue = temp;

                return PhysicalDamage.Value;

            case PhysicalWeaponType.meleeHeavy:
                physicalDamageBalance[0] = 1.0f; //DEX
                physicalDamageBalance[1] = 1.50f; //STR

                temp = basePhysicalDamage * (Strength.Value + characterLevel);
                temp = temp / characterLevel;
                //add flat mods here
                //add percent mods here
                temp = temp * ((Dexterity.Value * physicalDamageBalance[0]) + (Strength.Value * physicalDamageBalance[1]));
                temp = Mathf.Round(temp);
                PhysicalDamage.BaseValue = temp;

                return PhysicalDamage.Value;

            case PhysicalWeaponType.rangedLight:
                physicalDamageBalance[0] = 1.2f; //DEX

                temp = basePhysicalDamage * (Dexterity.Value + characterLevel);
                temp = temp / characterLevel;
                //add flat mods here
                //add percent mods here
                temp = temp * ((Dexterity.Value * physicalDamageBalance[0]));
                temp = Mathf.Round(temp);
                PhysicalDamage.BaseValue = temp;

                return PhysicalDamage.Value;

            case PhysicalWeaponType.rangedHeavy:
                physicalDamageBalance[0] = 1.35f; //DEX
                physicalDamageBalance[1] = 1.50f; //STR

                temp = basePhysicalDamage * (Dexterity.Value + characterLevel);
                temp = temp / characterLevel;
                //add flat mods here
                //add percent mods here
                temp = temp * ((Dexterity.Value * physicalDamageBalance[0]) + (Strength.Value * physicalDamageBalance[1]));
                temp = Mathf.Round(temp);
                PhysicalDamage.BaseValue = temp;

                return PhysicalDamage.Value;

            default:
                return 0;
        }
    }
    #endregion

    #region Iniative
    [SerializeField] float[] iniativeBalance = new float[2] { 0.8f, 0.2f }; //0 for dex, 1 for luck
    protected CharacterStat Iniative = new CharacterStat();

    public float GetIniative()
    {
        Iniative.BaseValue = (Dexterity.Value * iniativeBalance[0]) + (Luck.Value * iniativeBalance[1]);
        //add flat mods here

        return Iniative.Value;
    }
    #endregion

    #region Dodge Chance
    protected CharacterStat DodgeChance = new CharacterStat();
    [SerializeField] float[] dodgeBalance = new float[2] { 0.65f, 0.35f }; //0 for dex, 1 for luck

    public float GetDodgeChance()
    {
        DodgeChance.BaseValue = Dexterity.Value + (Dexterity.Value * dodgeBalance[0]) + (Luck.Value * dodgeBalance[1]);
        //add percent mods here

        return DodgeChance.Value;
    }
    #endregion

    #region Mana
    [SerializeField] float baseMana = 100;
    protected CharacterStat Mana = new CharacterStat();
    public float GetMana()
    {
        Mana.BaseValue = ((baseMana + characterLevel) * Intelligence.Value) / 10;
        //add flat mods and percent mods here

        return Mana.Value;
    }
    #endregion

    #region Health 
    //TODO need to mess with this to boost the number higher
    [SerializeField] float baseHealth = 100;
    protected CharacterStat Health = new CharacterStat();

    public float GetHealth()
    {
        Health.BaseValue = ((baseHealth + Constitution.Value) * characterLevel) * 2.5f;
        //add flat mods and percent mods here

        return Health.Value;
    }
    #endregion

    #region Physicial Defense
    //this is really only useful to the character's survival when they are defending
    protected CharacterStat physicalDefense = new CharacterStat();
    [SerializeField] float[] physicalDefenseBalance = new float[2] { 0.8f, 0.2f };

    public float GetPhysicalDefense()
    {
        physicalDefense.BaseValue = ((Constitution.Value + characterLevel) / 2 + defendingBoost) * ((Constitution.Value * physicalDefenseBalance[0]) + (Strength.Value * physicalDefenseBalance[1]));
        //add percent mods here

        return physicalDefense.Value;
    }
    #endregion

    #region Magic Damage
    [SerializeField] float baseMagicDamage = 0;
    [SerializeField] float knowBalance = 0;

    //TODO replace this with the actual magic weapon type number once items are implemented
    private enum MagicWeaponType
    {
        Light = 200,
        Heavy = 125
    }
    [SerializeField] MagicWeaponType MWT = MagicWeaponType.Light;

    protected CharacterStat magicDamage = new CharacterStat();
    public float GetMagicDamage()
    {
        float temp = baseMagicDamage * ((Knowledge.Value + Intelligence.Value) / ((float)MWT / 100) + characterLevel);
        temp = temp / characterLevel;
        //add flat mods here
        //add precent mods here

        //weapon class damage modifier
        temp = temp * ((Knowledge.Value * knowBalance) + Intelligence.Value);
        temp = Mathf.Round(temp);
        magicDamage.BaseValue = temp;

        return magicDamage.Value;
    }
    #endregion

    #region Magic Defense
    //this is really only useful to the character's survival when they are defending
    protected CharacterStat magicDefense = new CharacterStat();
    [SerializeField] float[] magicDefenseBalance = new float[2] { 0.8f, 0.2f };

    public float GetMagicDefense()
    {
        magicDefense.BaseValue = ((Knowledge.Value + characterLevel) / 2 + defendingBoost) * ((Knowledge.Value * magicDefenseBalance[0]) + (Intelligence.Value * magicDefenseBalance[1]));
        //add percent mods here

        return magicDefense.Value;
    }
    #endregion

    #region Crit Chance
    protected CharacterStat critChance = new CharacterStat();
    [SerializeField] float critBalance = 0; //percent chance to crit per point in luck

    public float GetCritChance()
    {
        critChance.BaseValue = Luck.Value * critBalance;
        //add flat mods here

        return critChance.Value;
    }
    #endregion

    #region Status Ailment Chance
    [SerializeField] float ailmentBalance = 0; //percent chance to apply ailment per point in luck
    protected CharacterStat AilmentChance = new CharacterStat();

    public float GetStatusAilmentChance()
    {
        AilmentChance.BaseValue = (Luck.Value * ailmentBalance);
        //add percent mods here

        return AilmentChance.Value;
    }
    #endregion

    //TODO some way to select a preset and set the stats to the corresponding amounts


    //TESTING STUFF
    public string GetStats()
    {
        string stats = "Physical Damage: " + this.GetPhysicalDamage() + "\nMagic Damage: " + this.GetMagicDamage() + "\nPhysical Defense: " + this.GetPhysicalDefense() +
             "\nMagic Defense: " + this.GetMagicDefense() + "\nHealth: " + this.GetHealth() + "\nMana: " + this.GetMana() + "\nIniative: " + this.GetIniative() +
             "\nCrit Chance: " + this.GetCritChance() + "\nStatus Ailment Chance: " + this.GetStatusAilmentChance() + "\nDodge Chance: " + this.GetDodgeChance();
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
