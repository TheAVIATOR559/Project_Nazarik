using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleController : MonoBehaviour {

    public enum BattleState
    {
        CollectEnemyActions,
        Wait,
        ExecuteActions,
        AssessBattleCondition,
        EndOfBattle
    }
    public BattleState battleState;

    public enum HeroInput
    {
        Active,
        Wait,
        Done
    }
    public HeroInput heroInput;

    [SerializeField] GameObject[] enemiesList;
    private List<GameObject> alliesList = new List<GameObject>();
    [SerializeField] GameObject player;
    [SerializeField] Camera mainCamera;

    public List<GameObject> characterList = new List<GameObject>();

    private float enemyOffset = 10;
    private float characterSpacing = 2;
    private float cameraHorizontalDistance = 8;
    private float cameraVerticalDistance = 2;

    private Vector3 enemySpawnPosition;
    private Vector3 enemyDirection;
    private Vector3 allySpawnPosition;
    private Vector3 allyDirection;
    private Vector3 cameraTargetPosition;
    private Vector3 battleCameraPosition;
    private int enemyCount;

    public Canvas battleCanvas;
    private GameObject actionPanel;
    private GameObject enemySelectPanel;
    private GameObject enemyPanel;
    private GameObject heroPanel;
    public GameObject initiativeBar;
    private GameObject victoryPanel;
    private GameObject defeatPanel;

    public GameObject expBar;
    public GameObject itemBar;
    public GameObject noItemsBar;
    public GameObject enemyButton;
    public GameObject enemyBar;
    public GameObject heroBar;
    public GameObject attackBar;
    public GameObject defendBar;

    public int turnCount;
    public int activeHeroCount;
    public int activeEnemyCount;
    public ActionInfo heroAction;
    public List<GameObject> heroesToManage = new List<GameObject>();
    public List<GameObject> deadCharacters = new List<GameObject>();

    private void Awake()
    {
        heroPanel = battleCanvas.transform.GetChild(0).gameObject;
        enemyPanel = battleCanvas.transform.GetChild(1).gameObject;
        actionPanel = battleCanvas.transform.GetChild(2).gameObject;
        enemySelectPanel = battleCanvas.transform.GetChild(3).gameObject;
        initiativeBar = battleCanvas.transform.GetChild(4).gameObject;
        victoryPanel = battleCanvas.transform.GetChild(5).gameObject;
        defeatPanel = battleCanvas.transform.GetChild(6).gameObject;
    }

    private void OnEnable()
    {
        //enemyCount = UnityEngine.Random.Range(1, 5); //I kind of want to tweak this so 4 happens more often or at least it 'feels' random
        enemyCount = 1;
        player.GetComponent<Player_Movement>().enabled = false;
        player.GetComponent<Rigidbody>().freezeRotation = true;

        alliesList = player.GetComponent<ListofAllies>().allies;

        foreach(GameObject obj in alliesList)
        {
            obj.GetComponent<Character>().CalculateStats();
        }

        characterList.Add(player);
        SpawnEnemies(enemyCount);
        SpawnAllies(alliesList.Count);
        player.GetComponent<HeroStateMachine>().enabled = true;
        activeHeroCount++;

        foreach(GameObject obj in characterList)
        {
            obj.GetComponent<Character>().enabled = true;
        }
        turnCount = characterList.Count;      

        //disable camera mouseAim script
        mainCamera.GetComponent<Mouse_Aim>().enabled = false;

        //calculate camera position for battle
        battleCameraPosition = player.transform.position + (0.5f * (player.transform.forward * enemyOffset)) - (player.transform.right * cameraHorizontalDistance);
        battleCameraPosition.y += cameraVerticalDistance;

        cameraTargetPosition = player.transform.position + (player.transform.forward * (0.5f * enemyOffset)) + (player.transform.right * (1.5f * characterSpacing));
        cameraTargetPosition.y = 0;

        //enable corresponding camera script and move camera
        mainCamera.GetComponent<Camera_Battle>().enabled = true;
        mainCamera.GetComponent<Camera_Battle>().SetTargetPosition(cameraTargetPosition);
        mainCamera.GetComponent<Camera_Battle>().SetStandbyPosition(battleCameraPosition);

        battleCanvas.gameObject.SetActive(true);
        actionPanel.SetActive(false);
        enemySelectPanel.SetActive(false);
        victoryPanel.SetActive(false);
        defeatPanel.SetActive(false);
        initiativeBar.SetActive(true);
        heroPanel.SetActive(true);
        enemyPanel.SetActive(true);

        CharacterBars();
        EnemyButtons();

        ///FAIR WARNING I HAVE NO IDEA HOW THIS WORKS
        ///I STOLE IT FROM THE SECOND ANSWER ON THIS STACKEXCHANGE POST: https://stackoverflow.com/questions/3309188/how-to-sort-a-listt-by-a-property-in-the-object
        characterList.Sort((x, y) => y.GetComponent<Character>().Initiative.Value.CompareTo(x.GetComponent<Character>().Initiative.Value));

        battleState = BattleState.CollectEnemyActions;
        heroInput = HeroInput.Wait;
    }
	
	// Update is called once per frame
	void Update ()
    {
        switch (battleState)
        {
            case BattleState.CollectEnemyActions:
                foreach(GameObject obj in characterList)
                {
                    if(obj.GetComponent<EnemyStateMachine>() != null && !obj.GetComponent<Character>().isDead)
                    {
                        obj.GetComponent<EnemyStateMachine>().currentState = EnemyStateMachine.State.Thinking;
                    }
                    if(obj.GetComponent<HeroStateMachine>() != null && !obj.GetComponent<Character>().isDead)
                    {
                        heroesToManage.Add(obj);
                    }
                }
                battleState = BattleState.Wait;
                heroInput = HeroInput.Active;
                break;
            case BattleState.Wait:
                if(heroesToManage.Count == 0)
                {
                    battleState = BattleState.ExecuteActions;
                }
                break;
            case BattleState.ExecuteActions:
                if (turnCount == 0)
                {
                    battleState = BattleState.AssessBattleCondition;
                    break;
                }

                if (characterList[0].GetComponent<EnemyStateMachine>() != null)
                {
                    characterList[0].GetComponent<EnemyStateMachine>().currentState = EnemyStateMachine.State.Acting;
                }
                if (characterList[0].GetComponent<HeroStateMachine>() != null)
                {
                    characterList[0].GetComponent<HeroStateMachine>().currentState = HeroStateMachine.State.Acting;
                }
                break;
            case BattleState.AssessBattleCondition:
                if(activeEnemyCount <= 0)
                {

                    heroPanel.SetActive(false);
                    enemyPanel.SetActive(false);
                    initiativeBar.SetActive(false);
                    actionPanel.SetActive(false);
                    enemySelectPanel.SetActive(false);

                    int totalExp = 0;
                    foreach(GameObject obj in deadCharacters)
                    {
                        if(obj.GetComponent<EnemyStateMachine>() != null)
                        {
                            totalExp += obj.GetComponent<Character>().expIfDefeated;
                            totalExp += UnityEngine.Random.Range(5 * obj.GetComponent<Character>().Level, (100 * obj.GetComponent<Character>().Level) + 1);
                        }
                    }
                    battleState = BattleState.EndOfBattle;

                    DisplayExperience(totalExp);
                    DisplayItems();

                    victoryPanel.SetActive(true);

                    break;
                }
                else if(activeHeroCount <= 0)
                {
                    heroPanel.SetActive(false);
                    enemyPanel.SetActive(false);
                    initiativeBar.SetActive(false);
                    actionPanel.SetActive(false);
                    enemySelectPanel.SetActive(false);

                    defeatPanel.SetActive(true);

                    battleState = BattleState.EndOfBattle;
                    break;
                }
                else
                {
                    foreach(GameObject obj in characterList)
                    {
                        obj.GetComponent<Character>().isDefending = false;
                        obj.transform.GetChild(1).gameObject.SetActive(false);
                    }

                    turnCount = characterList.Count;
                    battleState = BattleState.CollectEnemyActions;
                    break;
                }

            case BattleState.EndOfBattle:
                break;
            default:
                Debug.LogError("Battle State Error");
                break;
        }

        switch (heroInput)
        {
            case HeroInput.Active:
                if(heroesToManage.Count > 0)
                {
                    //heroAction = new ActionInfo();
                    actionPanel.SetActive(true);
                    CheckEnemyButtons();
                    heroesToManage[0].transform.GetChild(0).gameObject.SetActive(true);
                    heroInput = HeroInput.Wait;
                }
                break;
            case HeroInput.Wait:
                break;
            case HeroInput.Done:
                HeroInputDone();
                break;
            default:
                Debug.LogError("Hero Input State Error");
                break;
        }

    }

    private void SpawnAllies(int allyCount)
    {
        int m_allyCount = allyCount - 1;

        allyDirection = player.transform.forward;
        for(int i = 0; i <= m_allyCount; i++)
        {

            //set spawn position
            allySpawnPosition = player.transform.position + (player.transform.right * (characterSpacing * (i + 1)));
            allySpawnPosition.y = 0.5f * alliesList[i].GetComponent<Renderer>().bounds.size.y;

            //alliesSpawned[i] = Instantiate(alliesList[i], allySpawnPosition, Quaternion.LookRotation(allyDirection));
            characterList.Add(Instantiate(alliesList[i], allySpawnPosition, Quaternion.LookRotation(allyDirection)));
            activeHeroCount++;
        }

    }

    private void SpawnEnemies(int enemyCount)
    {
        int m_enemyCount = enemyCount - 1;
        int enemyNumber = 0;

        enemyDirection = player.transform.forward * -1;
        for(int i = 0; i <= m_enemyCount; i++)
        {
            enemyNumber = UnityEngine.Random.Range(0, enemiesList.Length);

            enemySpawnPosition = player.transform.position + (player.transform.forward * enemyOffset);
            enemySpawnPosition = enemySpawnPosition + (player.transform.right * (characterSpacing * i));
            enemySpawnPosition.y = 0.5f * enemiesList[enemyNumber].GetComponent<Renderer>().bounds.size.y;

            //enemiesSpawned[i] = Instantiate(enemiesList[enemyNumber], enemySpawnPosition, Quaternion.LookRotation(enemyDirection));
            characterList.Add(Instantiate(enemiesList[enemyNumber], enemySpawnPosition, Quaternion.LookRotation(enemyDirection)));
            activeEnemyCount++;
        }

    }

    private void EnemyButtons()
    {
        foreach(GameObject obj in characterList)
        {
            if(obj.tag == "Enemy(BATTLE)")
            {
                GameObject newButton = Instantiate(enemyButton) as GameObject;
                EnemySelectButton button = newButton.GetComponent<EnemySelectButton>();

                Character currEnemy = obj.GetComponent<Character>();
                Text buttonText = newButton.transform.GetChild(0).gameObject.GetComponent<Text>();

                buttonText.text = currEnemy.name;
                button.enemyPrefab = obj;
                newButton.transform.SetParent(enemySelectPanel.transform.GetChild(0).transform);
            }
        }
    }

    private void CharacterBars()
    {
        foreach(GameObject obj in characterList)
        {
            if(obj.tag == "Enemy(BATTLE)")
            {
                GameObject newBar = Instantiate(enemyBar) as GameObject;

                newBar.transform.GetChild(0).GetComponent<Text>().text = obj.GetComponent<Character>().name;
                newBar.transform.GetChild(1).GetComponent<Text>().text = "HP: " + obj.GetComponent<Character>().currentHealth;
                obj.GetComponent<Character>().characterBar = newBar;

                newBar.transform.SetParent(enemyPanel.transform.GetChild(0).transform);
            }
            if(obj.tag == "Ally(BATTLE)" || obj.tag == "Player")
            {
                GameObject newBar = Instantiate(heroBar) as GameObject;

                newBar.transform.GetChild(0).GetComponent<Text>().text = obj.GetComponent<Character>().name;
                newBar.transform.GetChild(1).GetComponent<Text>().text = "HP: " + obj.GetComponent<Character>().currentHealth + "/" + obj.GetComponent<Character>().totalHealth;
                newBar.transform.GetChild(2).GetComponent<Text>().text = "MP: " + obj.GetComponent<Character>().currentMana + "/" + obj.GetComponent<Character>().totalMana;
                obj.GetComponent<Character>().characterBar = newBar;

                newBar.transform.SetParent(heroPanel.transform.GetChild(0).transform);
            }
        }
    }

    public void AttackAction()
    {
        heroAction.actionType = ActionInfo.ActionType.Attack;
        enemySelectPanel.SetActive(true);
    }

    public void DefendAction()
    {
        heroAction.actionType = ActionInfo.ActionType.Defend;
        heroInput = HeroInput.Done;
    }

    public void EnemySelection(GameObject target)
    {
        heroAction.target = target;
        target.transform.GetChild(0).gameObject.SetActive(false);
        enemySelectPanel.SetActive(false);
        heroInput = HeroInput.Done;
    }

    public void HeroInputDone()
    {
        heroAction.actor = heroesToManage[0];
        heroesToManage[0].GetComponent<HeroStateMachine>().action = new ActionInfo(heroAction);
        heroesToManage[0].transform.GetChild(0).gameObject.SetActive(false);
        heroesToManage.RemoveAt(0);
        AddAction(heroAction);
        actionPanel.SetActive(false);
        heroInput = HeroInput.Active;
    }

    public void AddAction(ActionInfo action)
    {
        switch (action.actionType)
        {
            case ActionInfo.ActionType.Attack:
                GameObject newAttackBar = Instantiate(attackBar) as GameObject;

                newAttackBar.transform.GetChild(0).GetComponent<Text>().text = action.actor.GetComponent<Character>().name;
                newAttackBar.transform.GetChild(1).GetComponent<Text>().text = action.target.GetComponent<Character>().name;
                newAttackBar.transform.SetParent(initiativeBar.transform);

                newAttackBar.transform.SetSiblingIndex(characterList.FindIndex(a => a.gameObject == action.actor));
                
                break;
            case ActionInfo.ActionType.Defend:
                GameObject newDefendBar = Instantiate(defendBar) as GameObject;
                newDefendBar.transform.GetChild(0).GetComponent<Text>().text = action.actor.GetComponent<Character>().name;
                newDefendBar.transform.SetParent(initiativeBar.transform);

                newDefendBar.transform.SetSiblingIndex(characterList.FindIndex(a => a.gameObject == action.actor));

                break;
            case ActionInfo.ActionType.Skill:
                Debug.Log("SKILLS NOT IMPLEMENTED");
                break;
            case ActionInfo.ActionType.Item:
                Debug.Log("ITEMS NOT IMPLEMENTED");
                break;
            default:
                Debug.LogError("AddAction() actionType Error");
                break;
        }
    }

    public void CheckEnemyButtons()
    {
        for(int i = 0; i < enemySelectPanel.transform.GetChild(0).childCount; i++)
        {
            if (enemySelectPanel.transform.GetChild(0).transform.GetChild(i).GetComponent<EnemySelectButton>().enemyPrefab.GetComponent<Character>().isDead)
            {
                enemySelectPanel.transform.GetChild(0).transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    public void CombatCleanup()
    {
        characterList.Remove(player);
        player.GetComponent<Character>().enabled = false;
        player.GetComponent<HeroStateMachine>().enabled = false;
        foreach (GameObject obj in characterList)
        {
            Destroy(obj);
        }
        characterList.Clear();
        foreach(GameObject obj in deadCharacters)
        {
            Destroy(obj);
        }
        deadCharacters.Clear();

        for(int i = 0; i < enemySelectPanel.transform.GetChild(0).childCount; i++)
        {
            Destroy(enemySelectPanel.transform.GetChild(0).GetChild(i).gameObject);
        }
        for(int i = 0; i < heroPanel.transform.GetChild(0).childCount; i++)
        {
            Destroy(heroPanel.transform.GetChild(0).GetChild(i).gameObject);
        }
        for(int i = 0; i < enemyPanel.transform.GetChild(0).childCount; i++)
        {
            Destroy(enemyPanel.transform.GetChild(0).GetChild(i).gameObject);
        }
        for(int i = 0; i < victoryPanel.transform.GetChild(1).childCount; i++)
        {
            Destroy(victoryPanel.transform.GetChild(1).GetChild(i).gameObject);
        }
        for(int i = 0; i < victoryPanel.transform.GetChild(2).GetChild(1).childCount; i++)
        {
            Destroy(victoryPanel.transform.GetChild(2).transform.GetChild(1).GetChild(i).gameObject);
        }


        //disable battle camera script
        mainCamera.GetComponent<Camera_Battle>().enabled = false;
        mainCamera.GetComponent<Mouse_Aim>().enabled = true;

        //enable player movement
        player.GetComponent<Player_Movement>().enabled = true;
        player.GetComponent<Rigidbody>().freezeRotation = false;
        battleCanvas.gameObject.SetActive(false);
        this.GetComponent<BattleController>().enabled = false;
    }

    private void DisplayExperience(int totalExperience)
    {
        //divide by alliesList.Length
        int individualExp = totalExperience / (alliesList.Count + 1);

        GameObject newExpBar = Instantiate(expBar) as GameObject;

        newExpBar.transform.GetChild(0).GetComponent<Text>().text = player.GetComponent<Character>().name;
        newExpBar.transform.GetChild(1).GetComponent<Text>().text = individualExp.ToString();

        player.GetComponent<Character>().currentExperience += individualExp;
        if (player.GetComponent<Character>().LevelUp())
        {
            newExpBar.transform.GetChild(2).gameObject.SetActive(true);
        }
        newExpBar.transform.SetParent(victoryPanel.transform.GetChild(1));

        //create ExpBars for player and allies and add exp to each character
        foreach (GameObject obj in alliesList)
        {
            newExpBar = Instantiate(expBar) as GameObject;

            newExpBar.transform.GetChild(0).GetComponent<Text>().text = obj.GetComponent<Character>().name;
            newExpBar.transform.GetChild(1).GetComponent<Text>().text = individualExp.ToString();

            obj.GetComponent<Character>().currentExperience += individualExp;
            if (obj.GetComponent<Character>().LevelUp())
            {
                newExpBar.transform.GetChild(2).gameObject.SetActive(true);
            }
            newExpBar.transform.SetParent(victoryPanel.transform.GetChild(1)); 
        }

    }

    private void DisplayItems()
    {
        Transform itemSpacer = victoryPanel.transform.GetChild(2).transform.GetChild(1);

        //TODO some way of 'randomly' determining what items the player can get
        int numberOfItemsToAdd = 0;

        string[] itemNames = { "Health Potion", "Sword of Destiny", "Mana Potion", "Iron Helmet", "Rock" };
        int[] itemAmount = { 2, 1, 5, 1, 9};

        if(numberOfItemsToAdd > 0)
        {
            for (int i = 1; i <= numberOfItemsToAdd; i++)
            {
                GameObject newItemBar = Instantiate(itemBar) as GameObject;

                newItemBar.transform.GetChild(0).GetComponent<Text>().text = itemAmount[i - 1] + "x";
                newItemBar.transform.GetChild(1).GetComponent<Text>().text = itemNames[i - 1];
                newItemBar.transform.SetParent(itemSpacer);
            }
        }
        else
        {
            GameObject newNoItemsBar = Instantiate(noItemsBar) as GameObject;
            newNoItemsBar.transform.SetParent(itemSpacer);
        }

        //TODO add items to player inventory
    }
}