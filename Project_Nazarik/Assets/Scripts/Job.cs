using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Job
{
    public enum JobClass
    {
        Fighter,
        Knight,
        Berserker,
        WeaponMaster,
        Mage,
        Cleric,
        Evoker,
        SpellMaster,
        Rogue,
        Bandit,
        Ranger,
        Antiquarian
    }

    public enum WeaponType
    {
        Physical,
        Magical
    }

    public enum WeaponClass
    {
        meleeLight,
        meleeMedium,
        meleeHeavy,
        rangedLight,
        rangedHeavy,
        magicLight,
        magicHeavy
    }

    public enum ArmorClass
    {
        Light,
        Medium,
        Heavy
    }

    public JobClass jobClass;
    public WeaponType equippableWeaponType;
    public WeaponClass equippableWeaponClass;
    public ArmorClass equippableArmorClass;

    public string className;

    public float healthModifier;
    public float manaModifier;
    public float initiativeModifier;
    public float dodgeChanceModifier;
    public float critChanceModifier;
    public float critDamageModifier;
    public float ailmentMissModifier;
    public float physDamageModifier;
    public float magDamageModifier;
    public float physDefenseModifier;
    public float magDefenseModifier;

    //usable skills

    public void SetJobClass(JobClass jobClass)
    {
        switch (jobClass)
        {
            case JobClass.Fighter:
                equippableWeaponType = WeaponType.Physical;
                equippableWeaponClass = WeaponClass.meleeMedium;
                equippableArmorClass = ArmorClass.Medium;

                className = "Fighter";

                //usable skills

                //stat modifiers
                healthModifier = 1.4f;
                manaModifier = 0;
                physDamageModifier = 1.4f;
                magDamageModifier = 0;
                physDefenseModifier = 1.2f;
                magDefenseModifier = 0;
                initiativeModifier = 0;
                dodgeChanceModifier = 0;
                ailmentMissModifier = 0;
                critChanceModifier = 0;
                critDamageModifier = 0;

                break;
            case JobClass.Knight:
                equippableWeaponType = WeaponType.Physical;
                equippableWeaponClass = WeaponClass.meleeHeavy;
                equippableArmorClass = ArmorClass.Heavy;

                className = "Knight";

                //usable skills

                //stat modifiers
                healthModifier = 1.4f;
                manaModifier = 1.1f;
                physDamageModifier = 1.4f;
                magDamageModifier = 1.1f;
                physDefenseModifier = 1.4f;
                magDefenseModifier = 1.2f;
                initiativeModifier = 0;
                dodgeChanceModifier = 0;
                ailmentMissModifier = 5;
                critChanceModifier = 0;
                critDamageModifier = 0;
                break;
            case JobClass.Berserker:
                equippableWeaponType = WeaponType.Physical;
                equippableWeaponClass = WeaponClass.meleeHeavy;
                equippableArmorClass = ArmorClass.Medium;

                className = "Berserker";

                //usable skills

                //stat modifiers
                healthModifier = 1.2f;
                manaModifier = 1.1f;
                physDamageModifier = 1.4f;
                magDamageModifier = 0;
                physDefenseModifier = 1.2f;
                magDefenseModifier = 0;
                initiativeModifier = 1.35f;
                dodgeChanceModifier = 10;
                ailmentMissModifier = 5;
                critChanceModifier = 10;
                critDamageModifier = 7.5f;
                break;
            case JobClass.WeaponMaster:
                equippableWeaponType = WeaponType.Physical;
                equippableWeaponClass = WeaponClass.meleeHeavy;
                equippableArmorClass = ArmorClass.Heavy;

                className = "Weapon Master";

                //usable skills

                //stat modifiers
                healthModifier = 1.2f;
                manaModifier = 1.2f;
                physDamageModifier = 1.4f;
                magDamageModifier = 1.1f;
                physDefenseModifier = 1.2f;
                magDefenseModifier = 0;
                initiativeModifier = 0;
                dodgeChanceModifier = 5;
                ailmentMissModifier = 0;
                critChanceModifier = 5;
                critDamageModifier = 5;
                break;
            case JobClass.Mage:
                equippableWeaponType = WeaponType.Magical;
                equippableWeaponClass = WeaponClass.magicLight;
                equippableArmorClass = ArmorClass.Light;

                className = "Mage";

                //usable skills

                //stat modifiers
                healthModifier = 0;
                manaModifier = 1.2f;
                physDamageModifier = 0;
                magDamageModifier = 1.4f;
                physDefenseModifier = 0;
                magDefenseModifier = 1.4f;
                initiativeModifier = 0;
                dodgeChanceModifier = 0;
                ailmentMissModifier = 5;
                critChanceModifier = 0;
                critDamageModifier = 0;
                break;
            case JobClass.Cleric:
                equippableWeaponType = WeaponType.Magical;
                equippableWeaponClass = WeaponClass.magicLight;
                equippableArmorClass = ArmorClass.Light;

                className = "Cleric";

                //usable skills

                //stat modifiers
                healthModifier = 1.1f;
                manaModifier = 1.4f;
                physDamageModifier = 0;
                magDamageModifier = 1.2f;
                physDefenseModifier = 0;
                magDefenseModifier = 1.4f;
                initiativeModifier = 1.15f;
                dodgeChanceModifier = 0;
                ailmentMissModifier = 10;
                critChanceModifier = 0;
                critDamageModifier = 0;
                break;
            case JobClass.Evoker:
                equippableWeaponType = WeaponType.Magical;
                equippableWeaponClass = WeaponClass.magicHeavy;
                equippableArmorClass = ArmorClass.Light;

                className = "Evoker";

                //usable skills

                //stat modifiers
                healthModifier = 0;
                manaModifier = 1.4f;
                physDamageModifier = 0;
                magDamageModifier = 1.2f;
                physDefenseModifier = 0;
                magDefenseModifier = 1.2f;
                initiativeModifier = 1.15f;
                dodgeChanceModifier = 0;
                ailmentMissModifier = 5;
                critChanceModifier = 0;
                critDamageModifier = 0;
                break;
            case JobClass.SpellMaster:
                equippableWeaponType = WeaponType.Magical;
                equippableWeaponClass = WeaponClass.magicHeavy;
                equippableArmorClass = ArmorClass.Light;

                className = "Evoker";

                //usable skills

                //stat modifiers
                healthModifier = 0;
                manaModifier = 1.4f;
                physDamageModifier = 0;
                magDamageModifier = 1.4f;
                physDefenseModifier = 0;
                magDefenseModifier = 1.4f;
                initiativeModifier = 1.15f;
                dodgeChanceModifier = 0;
                ailmentMissModifier = 10;
                critChanceModifier = 0;
                critDamageModifier = 0;

                break;
            case JobClass.Rogue:
                equippableWeaponType = WeaponType.Physical;
                equippableWeaponClass = WeaponClass.meleeLight;
                equippableArmorClass = ArmorClass.Light;

                className = "Rogue";

                //usable skills

                //stat modifiers
                healthModifier = 0;
                manaModifier = 1.2f;
                physDamageModifier = 0;
                magDamageModifier = 1.4f;
                physDefenseModifier = 0;
                magDefenseModifier = 1.4f;
                initiativeModifier = 0;
                dodgeChanceModifier = 0;
                ailmentMissModifier = 5;
                critChanceModifier = 0;
                critDamageModifier = 0;
                break;
            default:
                break;
        }
    }
}
