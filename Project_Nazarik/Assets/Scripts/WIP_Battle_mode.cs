using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WIP_Battle_mode : MonoBehaviour {

    [SerializeField] GameObject[] enemiesList;
    [SerializeField] GameObject player;
    [SerializeField] GameObject[] alliesList;
    [SerializeField] Camera mainCamera;
    [SerializeField] float enemyOffset = 0;
    [SerializeField] float enemySpacing = 0;
    [SerializeField] float cameraBCDistance = 0;
    [SerializeField] float cameraVerticalOffset = 0;
    [SerializeField] float backRowOffset = 0;

    private Vector3 enemySpawnPosition;
    private GameObject[] enemiesSpawned = new GameObject[4];
    private Vector3 battlePosition;
    private Vector3 enemyDirection;
    private Vector3 battleCameraPostion;
    private int enemyCount;
    private BattleLayout battleLayout;
    private Vector3 allySpawnPosition;
    private GameObject[] alliesSpawned = new GameObject[3];
    private Vector3 allyDirection;
    private int enemyNumber;

    enum BattleLayout
    {
        Line = 0,
        Trapezoid = 1
    }

	// Use this for initialization
	void Start () {
		
	}

    private void OnEnable()
    {
        battleLayout = (BattleLayout)Random.Range(0, 2);///SINCE WHEN IS THE UPPER BOUND ON RANGE EXCLUSIVE? WHAT THE HELL, UNITY?!
        enemyCount = /*Random.Range(1, 4)*/ 4;
        player.GetComponent<Player_Movement>().enabled = false;
        player.GetComponent<Rigidbody>().freezeRotation = true;
        for(int i = 1; i <= enemyCount; i++)
        {
            SpawnEnemies(i, battleLayout);
            SpawnAllies(i, battleLayout);
        }

        battlePosition = (enemiesSpawned[0].transform.position - player.transform.position) / 2;
        battlePosition = player.transform.position + battlePosition;
        transform.position = battlePosition;
        
        //moving camera to proper position then changing camera scripts
        transform.rotation = Quaternion.LookRotation(player.transform.forward);
        battleCameraPostion = transform.position;
        battleCameraPostion.y += cameraVerticalOffset;
        battleCameraPostion = battleCameraPostion + (transform.right * cameraBCDistance);

        mainCamera.GetComponent<Mouse_Aim>().enabled = false;
        mainCamera.GetComponent<Dungeon_Crawler>().enabled = true;
        mainCamera.GetComponent<Dungeon_Crawler>().OnBattleStart(battleCameraPostion);
    }

    // Update is called once per frame
    void Update () {

        //change this to an event flag of start of battle
        if (Input.GetKeyUp(KeyCode.B))
        {
            foreach(GameObject obj in enemiesSpawned)
            {
                Destroy(obj);
            }
            foreach(GameObject obj in alliesSpawned)
            {
                Destroy(obj);
            }

            mainCamera.GetComponent<Dungeon_Crawler>().enabled = false;
            mainCamera.GetComponent<Mouse_Aim>().enabled = true;

            //enable player movement
            player.GetComponent<Player_Movement>().enabled = true;
            player.GetComponent<Rigidbody>().freezeRotation = false;
            this.GetComponent<WIP_Battle_mode>().enabled = false;
        }

        //change this to be an event flag for the start of the player turn or the end of the enemy turn
        if(Input.GetKeyUp(KeyCode.J))
        {
            Debug.Log("player turn");
            mainCamera.GetComponent<Dungeon_Crawler>().ChangeTarget(player);
        }

        //change this to be an event flag for the start of the enemy turn or the end of the player turn
        if (Input.GetKeyUp(KeyCode.K))
        {
            Debug.Log("enemy turn");
            mainCamera.GetComponent<Dungeon_Crawler>().ChangeTarget(enemiesSpawned[0]);
        }
    }

    //todo Add ally rotation
    private void SpawnAllies(int loopCount, BattleLayout layout)
    {
        if(layout == BattleLayout.Line)
        {
            if(loopCount <= 3)
                {
                    allySpawnPosition = player.transform.position - (player.transform.forward * (enemySpacing * loopCount));
                    alliesSpawned[loopCount - 1] = (GameObject)Instantiate(alliesList[loopCount - 1], allySpawnPosition, Quaternion.identity);
                    alliesSpawned[loopCount - 1].transform.position = new Vector3(alliesSpawned[loopCount - 1].transform.position.x, 0.5f * alliesSpawned[loopCount - 1].GetComponent<Collider>().bounds.size.y, alliesSpawned[loopCount - 1].transform.position.z);
                }
        }
        else if(layout == BattleLayout.Trapezoid)
        {
            if(loopCount == 1)
            {
                allySpawnPosition = player.transform.position + (player.transform.right * enemySpacing);
            }
            if(loopCount == 2)
            {
                allySpawnPosition = player.transform.position + (player.transform.right * (enemySpacing * 1.5f)) - (player.transform.forward * backRowOffset);
            }
            if(loopCount == 3)
            {
                allySpawnPosition = player.transform.position - (player.transform.right * (enemySpacing * 0.5f)) - (player.transform.forward * backRowOffset);
            }

            if (loopCount <= 3)
            {
                alliesSpawned[loopCount - 1] = (GameObject)Instantiate(alliesList[loopCount - 1], allySpawnPosition, Quaternion.identity);
                alliesSpawned[loopCount - 1].transform.position = new Vector3(alliesSpawned[loopCount - 1].transform.position.x, 0.5f * alliesSpawned[loopCount - 1].GetComponent<Collider>().bounds.size.y, alliesSpawned[loopCount - 1].transform.position.z);
            }
        }
    }

    //todo add enemy rotation
    private void SpawnEnemies(int loopCount, BattleLayout layout)
    {
        enemyNumber = Random.Range(0, enemiesList.Length);

        if (layout == BattleLayout.Line)
        {
            if (loopCount == 1)
            {
                enemySpawnPosition = player.transform.position + (player.transform.forward * (enemyOffset + enemySpacing));
            }
            else
            {
                enemySpawnPosition = enemiesSpawned[loopCount - 2].transform.position + (player.transform.forward * enemySpacing);
            }
            enemiesSpawned[loopCount - 1] = (GameObject)Instantiate(enemiesList[enemyNumber], enemySpawnPosition, Quaternion.identity);
            enemiesSpawned[loopCount - 1].transform.position = new Vector3(enemiesSpawned[loopCount - 1].transform.position.x, 0.5f * enemiesSpawned[loopCount - 1].GetComponent<Collider>().bounds.size.y, enemiesSpawned[loopCount - 1].transform.position.z);
        }
        if(layout == BattleLayout.Trapezoid)
        {
            if (loopCount == 1)
            {
                enemySpawnPosition = player.transform.position + (player.transform.forward * enemyOffset);
            }
            else if (loopCount == 2)
            {
                enemySpawnPosition = enemiesSpawned[loopCount - 2].transform.position + (enemiesSpawned[loopCount - 2].transform.right * enemySpacing);
            }
            else if (loopCount == 3)
            {
                enemySpawnPosition = enemiesSpawned[loopCount - 2].transform.position - (enemiesSpawned[loopCount - 2].transform.forward * backRowOffset * -1) + (enemiesSpawned[loopCount - 3].transform.right);
            }
            else if (loopCount == 4)
            {
                enemySpawnPosition = enemiesSpawned[loopCount - 4].transform.position - (enemiesSpawned[loopCount - 4].transform.forward * backRowOffset * -1) - (enemiesSpawned[loopCount - 4].transform.right);
            }
            enemiesSpawned[loopCount - 1] = (GameObject)Instantiate(enemiesList[enemyNumber], enemySpawnPosition, Quaternion.identity);
            enemiesSpawned[loopCount - 1].transform.position = new Vector3(enemiesSpawned[loopCount - 1].transform.position.x, 0.5f * enemiesSpawned[loopCount - 1].GetComponent<Collider>().bounds.size.y, enemiesSpawned[loopCount - 1].transform.position.z);
        }
    }
}
