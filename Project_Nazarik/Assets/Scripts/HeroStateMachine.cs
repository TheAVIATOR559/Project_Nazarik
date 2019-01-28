using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroStateMachine : MonoBehaviour {

    public ActionInfo action;

    private BattleController BC;
    private bool actionStarted = false;
    private Vector3 startingPos;
    private float animSpeed = 5;
    private Character thisCharacter;

    public enum State
    {
        Waiting,
        Acting,
        Dead
    }
    public State currentState;

	// Use this for initialization
	void Start () {
        BC = GameObject.Find("BattleController").GetComponent<BattleController>();
        currentState = State.Waiting;
        startingPos = transform.position;
        thisCharacter = GetComponent<Character>();
	}
	
	// Update is called once per frame
	void Update () {
        switch (currentState)
        {
            case State.Waiting:
                break;
            case State.Acting:
                switch (action.actionType)
                {
                    case ActionInfo.ActionType.Attack:
                        StartCoroutine(Attack());
                        break;
                    case ActionInfo.ActionType.Defend:
                        StartCoroutine(Defend());
                        break;
                    case ActionInfo.ActionType.Skill:
                        Debug.Log("SKILLS NOT IMPLEMENTED");
                        currentState = State.Waiting;
                        break;
                    case ActionInfo.ActionType.Item:
                        Debug.Log("ITEMS NOT IMPLEMENTED");
                        currentState = State.Waiting;
                        break;
                    default:
                        Debug.LogError("HeroStateMachine TakeAction() Error");
                        break;
                }
                break;
            case State.Dead:
                break;
            default:
                Debug.LogError("Hero State Machine Error");
                break;
        }
	}

    private IEnumerator Attack()
    {
        //do attack like things
        if (actionStarted)
        {
            yield break;
        }
        actionStarted = true;

        if (thisCharacter.isDead)
        {
            BC.activeHeroCount--;
            BC.characterList.Remove(this.gameObject);
            BC.deadCharacters.Add(this.gameObject);
            BC.turnCount--;

            Destroy(BC.initiativeBar.transform.GetChild(0).gameObject);

            actionStarted = false;
            currentState = State.Dead;
            yield break;
        }
        if (action.target.GetComponent<Character>().isDead)
        {
            currentState = State.Waiting;
            BC.turnCount--;
            Destroy(BC.initiativeBar.transform.GetChild(0).gameObject);
            BC.characterList.RemoveAt(0);
            BC.characterList.Add(this.gameObject);

            actionStarted = false;
            yield break;
        }

        Vector3 targetPos = new Vector3(action.target.transform.position.x, transform.position.y, action.target.transform.position.z);
        targetPos = targetPos + (1.5f * action.target.transform.forward);

        while (MoveToTarget(targetPos))
        {
            yield return null;
        }

        //arrived at target. now wait a bit
        yield return new WaitForSeconds(0.5f);

        //do damage
        action.target.GetComponent<Character>().TakeDamage(thisCharacter.currentJob.equippableWeaponType, thisCharacter.AttackValue);

        //return to startingPos
        while (MoveToTarget(startingPos))
        {
            yield return null;
        }

        BC.turnCount--;
        Destroy(BC.initiativeBar.transform.GetChild(0).gameObject);

        BC.characterList.RemoveAt(0);
        BC.characterList.Add(this.gameObject);

        actionStarted = false;
        currentState = State.Waiting;
    }

    private IEnumerator Defend()
    {
        if (actionStarted)
        {
            yield break;
        }
        actionStarted = true;

        if (thisCharacter.isDead)
        {
            BC.activeHeroCount--;
            BC.characterList.Remove(this.gameObject);
            BC.deadCharacters.Add(this.gameObject);
            BC.turnCount--;

            Destroy(BC.initiativeBar.transform.GetChild(0).gameObject);

            actionStarted = false;
            currentState = State.Dead;
            yield break;
        }

        transform.GetChild(1).gameObject.SetActive(true);
        thisCharacter.isDefending = true;

        yield return new WaitForSeconds(0.5f);

        BC.turnCount--;
        Destroy(BC.initiativeBar.transform.GetChild(0).gameObject);

        BC.characterList.RemoveAt(0);
        BC.characterList.Add(this.gameObject);

        actionStarted = false;
        currentState = State.Waiting;
    }

    private bool MoveToTarget(Vector3 target)
    {
        if (target == transform.position)
        {
            return false;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime);
            return true;
        }
    }
}
