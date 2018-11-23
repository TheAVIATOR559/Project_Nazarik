using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    private enum Presets //TODO change these to the actual character classes once figured out
    {
        Tank,
        DPS,
        Healer,
        Rogue,
        Mage,
        Ranger,
    }

    //serialized fields for each stat
    [SerializeField] CharacterStat Strength = new CharacterStat(10);
    [SerializeField] CharacterStat Dexterity = new CharacterStat(10);
    [SerializeField] CharacterStat Intelligence = new CharacterStat(10);
    [SerializeField] CharacterStat Constitution = new CharacterStat(10);
    [SerializeField] CharacterStat NAME = new CharacterStat(10); //TODO rename this once you figure out what it should be

    //getters for each stat

    //setters for each stat


}
