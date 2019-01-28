using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ActionInfo {

    public enum ActionType
    {
        Attack,
        Defend,
        Skill,
        Item
    }

    public GameObject actor;
    public GameObject target;
    public ActionType actionType;

    public ActionInfo(ActionInfo action)
    {
        actor = action.actor;
        target = action.target;
        actionType = action.actionType;
    }

    public ActionInfo()
    {

    }

}
