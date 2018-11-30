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
    //TODO physical damage

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

    //TODO magic damage

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
        string coreStats = "Strength: " + Strength.Value + "\nConstitution: " + Constitution.Value + "\nDexterity: " + Dexterity.Value + 
            "\nIntelligence: " + Intelligence.Value + "\nKnowledge: " + Knowledge.Value + "\nLuck: " + Luck.Value;
        string derivedStats = "\nHealth: " + this.GetHealth() + "\nMana: " + this.GetMana() + "\nStatus Ailment Chance: " + this.GetStatusAilmentChance() + 
            "\nIniative: " + this.GetIniative() + "\nDodge Chance: " + this.GetDodgeChance() + "\nPhysical Damage Mitigation: " + this.GetPhysicalDamageMitigation() + 
            "\nMagic Damage Mitigation: " + this.GetMagicDamageMitigation() + "\nCrit Chance: " + this.GetCritChance();
        string stats = coreStats + derivedStats;

        return stats;
    }
}
