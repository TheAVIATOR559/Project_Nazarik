using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour {

    public ActionInfo action;

    private BattleController BC;
    private float animSpeed = 5;
    private bool actionStarted = false;
    private Vector3 startingPos;
    private Character thisCharacter;

    public enum State
    {
        Waiting,
        Thinking,
        Acting,
        Dead
    }
    public State currentState;

    // Use this for initialization
    void Start()
    {
        BC = GameObject.Find("BattleController").GetComponent<BattleController>();
        action = new ActionInfo();
        currentState = State.Waiting;
        startingPos = transform.position;
        thisCharacter = GetComponent<Character>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.Waiting:
                break;
            case State.Thinking:
                DecideAction();
                BC.AddAction(action);

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
                        //do skill like things
                        Debug.Log("SKILLS NOT IMPLEMENTED YET");

                        currentState = State.Waiting;
                        break;
                    case ActionInfo.ActionType.Item:
                        //do item like things
                        Debug.Log("ITEMS NOT IMPLEMENTED YET");

                        currentState = State.Waiting;
                        break;
                    default:
                        Debug.LogError("EnemyStateMachine TakeAction() Error");
                        break;
                }

                break;
            case State.Dead:
                break;
            default:
                Debug.LogError("Enemy State Machine Error");
                break;
        }
    }

    private void DecideAction()
    {
        int rand = Random.Range(0, /*4*/2); //TODO uncomment once skills and items are implemented

        if (rand == 0)
        {
            //attacking
            action.actor = this.gameObject;
            action.actionType = ActionInfo.ActionType.Attack;

            int targetNumber;
            while (true)
            {
                targetNumber = Random.Range(0, BC.characterList.Count);
                if (BC.characterList[targetNumber].tag == "Ally(BATTLE)" || BC.characterList[targetNumber].tag == "Player")
                {
                    action.target = BC.characterList[targetNumber];
                    break;
                }
            }
        }
        if (rand == 1)
        {
            //defending
            action.actor = this.gameObject;
            action.actionType = ActionInfo.ActionType.Defend;
        }
        if (rand == 2)
        {
            //skilling
            Debug.Log("SKILLS NOT IMPLEMENTED");
        }
        if(rand == 3)
        {
            //iteming
            Debug.Log("ITEMS NOT IMPLEMENTED");
        }

        currentState = State.Waiting;
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
            BC.activeEnemyCount--;
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
            BC.activeEnemyCount--;
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
        if(target == transform.position)
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
