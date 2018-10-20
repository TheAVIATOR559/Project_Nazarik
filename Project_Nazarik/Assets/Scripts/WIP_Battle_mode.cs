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
    [SerializeField] float lineCameraDistance = 0;
    [SerializeField] float lineCameraVerticalOffset = 0;
    [SerializeField] float backRowOffset = 0;
    [SerializeField] float trapezoidCameraDistance = 0;
    [SerializeField] float trapezoidCameraVerticalOffset = 0;

    private Vector3 enemySpawnPosition;
    private GameObject[] enemiesSpawned = new GameObject[4];
    private Vector3 battlePosition;
    private Vector3 enemyDirection;
    private Vector3 battleCameraPosition;
    private int enemyCount;
    private BattleLayout battleLayout;
    private Vector3 allySpawnPosition;
    private GameObject[] alliesSpawned = new GameObject[3];
    private Vector3 allyDirection;
    private int enemyNumber;
    private Vector3 enemyCameraPosition;
    private Vector3 trapezoidCameraTransitionPoint;

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
        battleLayout = (BattleLayout)/*Random.Range(0, 2)*/ 1;///SINCE WHEN IS THE UPPER BOUND ON RANGE EXCLUSIVE? WHAT THE HELL, UNITY?!
        enemyCount = /*Random.Range(1, 4)*/ 4;
        player.GetComponent<Player_Movement>().enabled = false;
        player.GetComponent<Rigidbody>().freezeRotation = true;
        for(int i = 1; i <= enemyCount; i++)
        {
            SpawnEnemies(i, battleLayout);
            SpawnAllies(i, battleLayout);
        }

        battlePosition = ((enemiesSpawned[0].transform.position - player.transform.position) / 2) + (player.transform.right * (0.5f * enemySpacing));
        battlePosition = player.transform.position + battlePosition;
        transform.position = battlePosition;
        transform.rotation = Quaternion.LookRotation(player.transform.forward);
        mainCamera.GetComponent<Mouse_Aim>().enabled = false;

        if (battleLayout == BattleLayout.Line)
        {
            //moving camera to proper position then changing camera scripts
            battleCameraPosition = transform.position;
            battleCameraPosition.y += lineCameraVerticalOffset;
            battleCameraPosition = battleCameraPosition + (transform.right * lineCameraDistance);

            mainCamera.GetComponent<Dungeon_Crawler>().enabled = true;
            mainCamera.GetComponent<Dungeon_Crawler>().OnBattleStart(battleCameraPosition);
        }
        if(battleLayout == BattleLayout.Trapezoid)
        {
            battleCameraPosition = transform.position - (transform.forward * trapezoidCameraDistance);
            battleCameraPosition.y += trapezoidCameraVerticalOffset;

            enemyCameraPosition = transform.position + (transform.forward * trapezoidCameraDistance);
            enemyCameraPosition.y += trapezoidCameraVerticalOffset;

            trapezoidCameraTransitionPoint = transform.position;
            trapezoidCameraTransitionPoint.y += 4;

            mainCamera.GetComponent<Trapezoid_camera>().enabled = true;
            mainCamera.GetComponent<Trapezoid_camera>().ChangeTarget(battleCameraPosition, transform.position);
        }
        
    }

    // Update is called once per frame
    void Update () {

        //todo change this to an event flag of start of battle
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

            if(battleLayout == BattleLayout.Line)
            {
                mainCamera.GetComponent<Dungeon_Crawler>().enabled = false;
            }
            if(battleLayout == BattleLayout.Trapezoid)
            {
                mainCamera.GetComponent<Trapezoid_camera>().enabled = false;
            }
            mainCamera.GetComponent<Mouse_Aim>().enabled = true;

            //enable player movement
            player.GetComponent<Player_Movement>().enabled = true;
            player.GetComponent<Rigidbody>().freezeRotation = false;
            this.GetComponent<WIP_Battle_mode>().enabled = false;
        }

        //todo change this to be an event flag for the start of the player turn or the end of the enemy turn
        if(battleLayout == BattleLayout.Line)
        {
            if (Input.GetKeyUp(KeyCode.J))
            {
                Debug.Log("player turn");
                mainCamera.GetComponent<Dungeon_Crawler>().ChangeTarget(player);
            }

            if (Input.GetKeyUp(KeyCode.K))
            {
                Debug.Log("enemy turn");
                mainCamera.GetComponent<Dungeon_Crawler>().ChangeTarget(enemiesSpawned[0]);
            }
        }

        //todo change this to be an event flag for the start of the enemy turn or the end of the player turn
        if (battleLayout == BattleLayout.Trapezoid)
        {
            if (Input.GetKeyUp(KeyCode.J))
            {
                Debug.Log("player turn");
                mainCamera.GetComponent<Trapezoid_camera>().ChangeTarget(battleCameraPosition, transform.position);
            }

            if (Input.GetKeyUp(KeyCode.K))
            {
                Debug.Log("enemy turn");
                mainCamera.GetComponent<Trapezoid_camera>().ChangeTarget(enemyCameraPosition, transform.position);
            }
        }
    }

    private void SpawnAllies(int loopCount, BattleLayout layout)
    {
        if(layout == BattleLayout.Line)
        {
            if(loopCount <= 3)
                {
                    allySpawnPosition = player.transform.position - (player.transform.forward * (enemySpacing * loopCount));
                    alliesSpawned[loopCount - 1] = (GameObject)Instantiate(alliesList[loopCount - 1], allySpawnPosition, Quaternion.identity);
                    alliesSpawned[loopCount - 1].transform.position = new Vector3(alliesSpawned[loopCount - 1].transform.position.x, 0.5f * alliesSpawned[loopCount - 1].GetComponent<Collider>().bounds.size.y, alliesSpawned[loopCount - 1].transform.position.z);
                    alliesSpawned[loopCount - 1].transform.rotation = Quaternion.LookRotation(player.transform.forward, player.transform.up);
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
                //todo this might be offsetting this ally by too much. Not sure need to test
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
                alliesSpawned[loopCount - 1].transform.rotation = Quaternion.LookRotation(player.transform.forward, player.transform.up);
            }
        }
    }


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
            enemiesSpawned[loopCount - 1].transform.rotation = Quaternion.LookRotation(-1 * player.transform.forward, player.transform.up);
        }
        if(layout == BattleLayout.Trapezoid)
        {
            if (loopCount == 1)
            {
                enemySpawnPosition = player.transform.position + (player.transform.forward * enemyOffset);
            }
            else if (loopCount == 2)
            {
                enemySpawnPosition = enemiesSpawned[0].transform.position - (enemiesSpawned[0].transform.right * enemySpacing);
            }
            else if (loopCount == 3)
            {
                //todo this might be offsetting this enemy by too much. Not sure need to test
                enemySpawnPosition = enemiesSpawned[0].transform.position - (enemiesSpawned[0].transform.right * (1.5f * enemySpacing)) - (enemiesSpawned[0].transform.forward * backRowOffset);
            }
            else if (loopCount == 4)
            {
                enemySpawnPosition = enemiesSpawned[0].transform.position + (enemiesSpawned[0].transform.right * (0.5f * enemySpacing)) - (enemiesSpawned[0].transform.forward * backRowOffset);
            }
            enemiesSpawned[loopCount - 1] = (GameObject)Instantiate(enemiesList[enemyNumber], enemySpawnPosition, Quaternion.identity);
            enemiesSpawned[loopCount - 1].transform.position = new Vector3(enemiesSpawned[loopCount - 1].transform.position.x, 0.5f * enemiesSpawned[loopCount - 1].GetComponent<Collider>().bounds.size.y, enemiesSpawned[loopCount - 1].transform.position.z);
            enemiesSpawned[loopCount - 1].transform.rotation = Quaternion.LookRotation(-1 * player.transform.forward, player.transform.up);
        }
    }
}
