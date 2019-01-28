using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour {

    new public string name;

    public Image characterImage;
    public GameObject characterBar;
    public Roll.GrowthCurve[] growthCurves = new Roll.GrowthCurve[4];
    [SerializeField] Job.JobClass jobClass;
    public Job currentJob;

    public int Level = 1;
    public int currentExperience = 0;
    public int expToNextLevel;
    public int expIfDefeated;

    private int baseExp = 100;
    private float exponent = 1.25f;

    public int totalMana = 100;
    public int currentMana = 100;

    public int totalHealth = 100;
    public int currentHealth = 100;

    private bool m_Dead = false;
    public bool isDead
    {
        get
        {
            return m_Dead;
        }
    }

    private bool m_isDefending = false;
    public bool isDefending
    {
        get
        {
            return m_isDefending;
        }
        set
        {
            m_isDefending = value;
        }
    }

    public int AttackValue
    {
        get
        {
            if(currentJob.equippableWeaponType == Job.WeaponType.Physical)
            {
                return (int)PhysicalDamage.Value;
            }
            if(currentJob.equippableWeaponType == Job.WeaponType.Magical)
            {
                return (int)MagicDamage.Value;
            }
            else
            {
                return 0;
            }
        }
    }

    public CharacterStat Initiative = new CharacterStat(5);
    public CharacterStat DodgeChance = new CharacterStat(20);
    public CharacterStat PhysicalDefense = new CharacterStat(100);
    public CharacterStat MagicDefense = new CharacterStat(100);
    public CharacterStat CritChance = new CharacterStat(5);
    public CharacterStat CritDamage = new CharacterStat();
    [SerializeField] float critDamBalance = 2;
    public CharacterStat AilmentMissChance = new CharacterStat(20);
    public CharacterStat PhysicalDamage = new CharacterStat();
    [SerializeField] float basePhysicalDamage = 100;
    public CharacterStat MagicDamage = new CharacterStat();
    [SerializeField] float baseMagicDamage = 100;


    private void Awake()
    {
        CalculateStats();
        currentJob.SetJobClass(jobClass);
    }

    void OnEnable()
    {
        CalculateStats();
    }

    public void CalculateStats()
    {
        expIfDefeated = Mathf.RoundToInt(Mathf.RoundToInt(Mathf.Pow(100, 1.15f)) * (Level / 4f));
        expToNextLevel = Mathf.RoundToInt((baseExp * Mathf.Pow(Level, exponent)));
        if(currentJob.equippableWeaponType == Job.WeaponType.Physical)
        {
            CritDamage.BaseValue = PhysicalDamage.Value * critDamBalance;
        }
        else if(currentJob.equippableWeaponType == Job.WeaponType.Magical)
        {
            CritDamage.BaseValue = MagicDamage.Value * critDamBalance;
        }
        CalculateDamage();
    }

    private void CalculateDamage()
    {
        switch (currentJob.equippableWeaponClass)
        {
            case Job.WeaponClass.meleeLight:
                PhysicalDamage.BaseValue = basePhysicalDamage;
                MagicDamage.BaseValue = baseMagicDamage;
                break;

            case Job.WeaponClass.meleeMedium:
                PhysicalDamage.BaseValue = basePhysicalDamage;
                MagicDamage.BaseValue = baseMagicDamage;
                break;

            case Job.WeaponClass.meleeHeavy:
                PhysicalDamage.BaseValue = basePhysicalDamage;
                MagicDamage.BaseValue = baseMagicDamage;
                break;

            case Job.WeaponClass.rangedLight:
                PhysicalDamage.BaseValue = basePhysicalDamage;
                MagicDamage.BaseValue = baseMagicDamage;
                break;

            case Job.WeaponClass.rangedHeavy:
                PhysicalDamage.BaseValue = basePhysicalDamage;
                MagicDamage.BaseValue = baseMagicDamage;
                break;

            case Job.WeaponClass.magicLight:
                MagicDamage.BaseValue = baseMagicDamage;
                PhysicalDamage.BaseValue = basePhysicalDamage;
                break;

            case Job.WeaponClass.magicHeavy:
                MagicDamage.BaseValue = baseMagicDamage;
                PhysicalDamage.BaseValue = basePhysicalDamage;
                break;

            default:
                MagicDamage.BaseValue = baseMagicDamage;
                PhysicalDamage.BaseValue = basePhysicalDamage;
                break;
        }
    }

    public void TakeDamage(Job.WeaponType weaponType, int damage)
    {
        if (isDefending)
        {
            int newDamage;
            if(weaponType == Job.WeaponType.Physical)
            {
                newDamage = damage - (int)PhysicalDefense.Value;

                if (newDamage <= 0)
                {
                    newDamage = 0;
                }

                if (currentHealth - newDamage <= 0)
                {
                    currentHealth = 0;
                    m_Dead = true;
                    GetComponent<Renderer>().material.color = new Color(0.37f, 0.37f, 0.37f, 1f);
                }
                else
                {
                    currentHealth = currentHealth - newDamage;
                }
            }
            if(weaponType == Job.WeaponType.Magical)
            {
                newDamage = damage - (int)MagicDefense.Value;

                if(newDamage <= 0)
                {
                    newDamage = 0;
                }

                if (currentHealth - newDamage <= 0)
                {
                    currentHealth = 0;
                    m_Dead = true;
                    gameObject.GetComponent<Renderer>().material.color = new Color(0.37f, 0.37f, 0.37f, 1f);
                }
                else
                {
                    currentHealth = currentHealth - newDamage;
                }
            }
        }
        else
        {
            if (weaponType == Job.WeaponType.Physical)
            {
                if(currentHealth - damage <= 0)
                {
                    currentHealth = 0;
                    m_Dead = true;
                    gameObject.GetComponent<Renderer>().material.color = new Color(0.37f, 0.37f, 0.37f, 1f);
                }
                else
                {
                    currentHealth = currentHealth - damage;
                }
            }
            if (weaponType == Job.WeaponType.Magical)
            {
                if (currentHealth - damage <= 0)
                {
                    currentHealth = 0;
                    m_Dead = true;
                    gameObject.GetComponent<Renderer>().material.color = new Color(0.37f, 0.37f, 0.37f, 1f);
                }
                else
                {
                    currentHealth = currentHealth - damage;
                }
            }
        }
        
        if(tag == "Enemy(BATTLE)")
        {
            characterBar.transform.GetChild(1).GetComponent<Text>().text = "HP: " + currentHealth;
        }
        else if(tag == "Ally(BATTLE)" || tag == "Player")
        {
            characterBar.transform.GetChild(1).GetComponent<Text>().text = "HP: " + currentHealth + "/" + totalHealth;
        }
    }

    public void Defend()
    {
        m_isDefending = true;
    }    

    public bool LevelUp()
    {
        if(currentExperience >= expToNextLevel)
        {
            Level++;
            //increase HP/MP/PhysAttack/MagAttack/PhysDef/MagDef
            totalHealth += Roll.RollTheDie(1, Roll.TypeOfDie.D100);
            totalMana += Roll.RollTheDie(1, Roll.TypeOfDie.D20);

            PhysicalDamage.BaseValue += Roll.RollTheDie(growthCurves[0]);
            MagicDamage.BaseValue += Roll.RollTheDie(growthCurves[1]);

            PhysicalDefense.BaseValue += Roll.RollTheDie(growthCurves[2]);
            MagicDefense.BaseValue += Roll.RollTheDie(growthCurves[3]);

            expIfDefeated = Mathf.RoundToInt(Mathf.RoundToInt(Mathf.Pow(100, 1.15f)) * (Level / 4f));
            expToNextLevel = Mathf.RoundToInt((baseExp * Mathf.Pow(Level, exponent)));

            return true;
        }
        else
        {
            return false;
        }
    }

    //TESTING STUFF
    public void DEBUG_GetStats()
    {
        string stats = name + " :: Job " + currentJob.className + " :: Level " + Level + " :: Physical Damage " + PhysicalDamage.Value + " :: Magic Damage " + MagicDamage.Value +
            " :: Physical Defense " + PhysicalDefense.Value + " :: Magic Defense " + MagicDefense.Value + " :: Current Health " + currentHealth +  " :: Total Health " + totalHealth + 
            " :: Current Mana " + currentMana + " :: Total Mana " + totalMana + " :: Iniative " + Initiative.Value + " :: Crit Chance " + CritChance.Value + " :: Status Ailment Chance " + 
            AilmentMissChance.Value + " :: Dodge Chance " + DodgeChance.Value;
        Debug.Log(stats);
    }

}
