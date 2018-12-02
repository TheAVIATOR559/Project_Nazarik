using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {


    private enum Presets //change these to the actual character classes once figured out
    {
        Tank,
        DPS,
        Healer,
        Rogue,
        Mage,
        Ranger,
    }

    ///CORE STATS
    //TODO better way to declare these???
    //TODO need an upper bound for non-augmented(raw) stat. 10? 20? larger?
    [SerializeField] CharacterStat Strength = new CharacterStat(0); //physical damage
    [SerializeField] CharacterStat Dexterity = new CharacterStat(0); //turn order, dodge chance
    [SerializeField] CharacterStat Intelligence = new CharacterStat(0); //mana
    [SerializeField] CharacterStat Constitution = new CharacterStat(0); //health, physical damage mitigation
    [SerializeField] CharacterStat Knowledge = new CharacterStat(0); //magic damage, magic damage mitigation
    [SerializeField] CharacterStat Luck = new CharacterStat(0); //crit chance, status ailment chance
    [SerializeField] int characterLevel = 0; //level of character

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
                physicalDamageBalance[0] = 0.8f; //DEX
                physicalDamageBalance[1] = 0.2f; //STR

                temp = basePhysicalDamage * (Dexterity.Value + characterLevel);
                //add flat mods here
                //add percent mods here
                temp = temp + (Dexterity.Value * physicalDamageBalance[0]) + (Strength.Value * physicalDamageBalance[1]);
                PhysicalDamage.BaseValue = temp;

                return PhysicalDamage.Value;

            case PhysicalWeaponType.meleeMedium:
                physicalDamageBalance[0] = 0.35f; //DEX
                physicalDamageBalance[1] = 0.65f; //STR

                temp = basePhysicalDamage * (Strength.Value + characterLevel);
                //add flat mods here
                //add percent mods here
                temp = temp + (Dexterity.Value * physicalDamageBalance[0]) + (Strength.Value * physicalDamageBalance[1]);
                PhysicalDamage.BaseValue = temp;

                return PhysicalDamage.Value;

            case PhysicalWeaponType.meleeHeavy:
                physicalDamageBalance[0] = 0.2f; //DEX
                physicalDamageBalance[1] = 0.8f; //STR

                temp = basePhysicalDamage * (Strength.Value + characterLevel);
                //add flat mods here
                //add percent mods here
                temp = temp + (Dexterity.Value * physicalDamageBalance[0]) + (Strength.Value * physicalDamageBalance[1]);
                PhysicalDamage.BaseValue = temp;

                return PhysicalDamage.Value;

            case PhysicalWeaponType.rangedLight:
                physicalDamageBalance[0] = 0.8f; //DEX

                temp = basePhysicalDamage * (Dexterity.Value + characterLevel);
                //add flat mods here
                //add percent mods here
                temp = temp + (Dexterity.Value * physicalDamageBalance[0]);
                PhysicalDamage.BaseValue = temp;

                return PhysicalDamage.Value;

            case PhysicalWeaponType.rangedHeavy:
                physicalDamageBalance[0] = 0.2f; //DEX
                physicalDamageBalance[1] = 0.8f; //STR

                temp = basePhysicalDamage * (Dexterity.Value + characterLevel);
                //add flat mods here
                //add percent mods here
                temp = temp + (Dexterity.Value * physicalDamageBalance[0]) + (Strength.Value * physicalDamageBalance[1]);
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
        Mana.BaseValue = (baseMana + characterLevel) * Intelligence.Value;
        //add flat mods and percent mods here

        return Mana.Value;
    }
    #endregion

    #region Health 
    [SerializeField] float baseHealth = 100;
    protected CharacterStat Health = new CharacterStat();

    public float GetHealth()
    {
        Health.BaseValue = (baseHealth + Constitution.Value) * characterLevel;
        //add flat mods and percent mods here

        return Health.Value;
    }
    #endregion

    #region Physicial Damage Mitigation
    protected CharacterStat physDamMit = new CharacterStat();
    [SerializeField] float[] PDMBalance = new float[2] { 0.8f, 0.2f };

    public float GetPhysicalDamageMitigation()
    {
        physDamMit.BaseValue = Constitution.Value + (Constitution.Value * PDMBalance[0]) + (Strength.Value * PDMBalance[1]);
        //add percent mods here

        return physDamMit.Value;
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
        //add flat mods here
        //add precent mods here

        //weapon class damage modifier
        temp = temp + (Knowledge.Value * knowBalance) + Intelligence.Value;
        magicDamage.BaseValue = temp;

        return magicDamage.Value;
    }
    #endregion

    #region Magic Damage Mitigation
    protected CharacterStat MDM = new CharacterStat();
    [SerializeField] float[] MDMBalance = new float[2] { 0.8f, 0.2f };

    public float GetMagicDamageMitigation()
    {
        MDM.BaseValue = Knowledge.Value + (Knowledge.Value * MDMBalance[0]) + (Intelligence.Value * MDMBalance[1]);
        //add percent mods here

        return MDM.Value;
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
        string stats = "Physical Damage: " + this.GetPhysicalDamage();

        return stats;
    }
}
