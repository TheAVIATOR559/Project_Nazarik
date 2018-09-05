using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WIP_Battle_mode : MonoBehaviour {

    [SerializeField] GameObject[] enemiesList;
    [SerializeField] GameObject player;
    [SerializeField] GameObject[] alliesList;
    [SerializeField] Camera mainCamera;
    [SerializeField] float characterOffset = 0;
    [SerializeField] float characterSpacing = 0;
    [SerializeField] float cameraBattleLineDistance = 0;
    [SerializeField] float cameraLineVerticalOffset = 0;
    [SerializeField] float backRowOffset = 0;
    [SerializeField] float cameraBattleTrapDistance = 0;
    [SerializeField] float cameraTrapVerticalOffset = 0;

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
    private Vector3 cameraTargetPosition;

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
        battleLayout = (BattleLayout)/*Random.Range(0, 2)*/1;///SINCE WHEN IS THE UPPER BOUND ON RANGE EXCLUSIVE? WHAT THE HELL, UNITY?!
        enemyCount = /*Random.Range(1, 4)*/ 4;
        player.GetComponent<Player_Movement>().enabled = false;
        player.GetComponent<Rigidbody>().freezeRotation = true;

        SpawnEnemies(enemyCount, battleLayout);
        SpawnAllies(3, battleLayout);

        battlePosition = (enemiesSpawned[0].transform.position - player.transform.position) / 2;
        battlePosition = player.transform.position + battlePosition;
        transform.position = battlePosition;
        transform.rotation = Quaternion.LookRotation(player.transform.forward);
        
        //moving camera to proper position then changing camera scripts
        mainCamera.GetComponent<Mouse_Aim>().enabled = false;
        switch (battleLayout)
        {
            case BattleLayout.Line:
                battleCameraPostion = transform.position;
                battleCameraPostion.y += cameraLineVerticalOffset;
                battleCameraPostion = battleCameraPostion + (transform.right * cameraBattleLineDistance);
                mainCamera.GetComponent<Dungeon_Crawler>().enabled = true;
                mainCamera.GetComponent<Dungeon_Crawler>().OnBattleStart(battleCameraPostion);
                break;

            case BattleLayout.Trapezoid:
                battleCameraPostion = player.transform.position;
                battleCameraPostion.y += cameraTrapVerticalOffset;
                battleCameraPostion += (player.transform.right * (0.5f * characterSpacing)) - (player.transform.forward * cameraBattleTrapDistance);
                //todo (0.5f * characterSpacing) isn't actually offsetting the camera half the character spacing

                cameraTargetPosition = enemiesSpawned[0].transform.position;
                cameraTargetPosition.y += cameraTrapVerticalOffset;
                //battleCameraPostion += (enemiesSpawned[0].transform.right * (0.5f * characterSpacing)) - (enemiesSpawned[0].transform.forward * cameraBattleTrapDistance);
                ////todo uncomment the above once rotation is implemented/fixed

                mainCamera.GetComponent<Trapezoid_camera>().enabled = true;
                mainCamera.GetComponent<Trapezoid_camera>().ChangeTarget(battleCameraPostion, cameraTargetPosition);
                break;
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

            mainCamera.GetComponent<Dungeon_Crawler>().enabled = false;
            mainCamera.GetComponent<Mouse_Aim>().enabled = true;

            //enable player movement
            player.GetComponent<Player_Movement>().enabled = true;
            player.GetComponent<Rigidbody>().freezeRotation = false;
            this.GetComponent<WIP_Battle_mode>().enabled = false;
        }

        //todo change this to be an event flag for the start of the player turn or the end of the enemy turn
        //todo modify this structure to work with 2 different battle layouts
        if (Input.GetKeyUp(KeyCode.J))
        {
            Debug.Log("player turn");
            mainCamera.GetComponent<Dungeon_Crawler>().ChangeTarget(player);
        }

        //todo change this to be an event flag for the start of the enemy turn or the end of the player turn
        //todo modify this structure to work with 2 different battle layouts
        if (Input.GetKeyUp(KeyCode.K))
        {
            Debug.Log("enemy turn");
            mainCamera.GetComponent<Dungeon_Crawler>().ChangeTarget(enemiesSpawned[0]);
        }
    }

    //todo Add ally rotation
    private void SpawnAllies(int allyNumber, BattleLayout layout)
    {
        for(int i = 1; i <= allyNumber; i++)
        {
            if(layout == BattleLayout.Line)
            {
                if(i <= 3)
                    {
                        allySpawnPosition = player.transform.position - (player.transform.forward * (characterSpacing * i));
                        alliesSpawned[i - 1] = (GameObject)Instantiate(alliesList[i - 1], allySpawnPosition, Quaternion.identity);
                        alliesSpawned[i - 1].transform.position = new Vector3(alliesSpawned[i - 1].transform.position.x, 0.5f * alliesSpawned[i - 1].GetComponent<Collider>().bounds.size.y, alliesSpawned[i - 1].transform.position.z);
                    }
            }
            else if(layout == BattleLayout.Trapezoid)
            {
                if(i == 1)
                {
                    allySpawnPosition = player.transform.position + (player.transform.right * characterSpacing);
                }
                if(i == 2)
                {
                    allySpawnPosition = player.transform.position + (player.transform.right * (characterSpacing * 1.5f)) - (player.transform.forward * backRowOffset);
                }
                if(i == 3)
                {
                    allySpawnPosition = player.transform.position - (player.transform.right * (characterSpacing * 0.5f)) - (player.transform.forward * backRowOffset);
                }

                if(i <= 3)
                {
                    alliesSpawned[i - 1] = (GameObject)Instantiate(alliesList[i - 1], allySpawnPosition, Quaternion.identity);
                    alliesSpawned[i - 1].transform.position = new Vector3(alliesSpawned[i - 1].transform.position.x, 0.5f * alliesSpawned[i - 1].GetComponent<Collider>().bounds.size.y, alliesSpawned[i - 1].transform.position.z);
                }
            }
        }
        
    }

    //todo add enemy rotation
    private void SpawnEnemies(int enemyCount, BattleLayout layout)
    {
        for(int i = 1; i <= enemyCount; i++)
        {
            enemyNumber = Random.Range(0, enemiesList.Length);

            if (layout == BattleLayout.Line)
            {
                if (i == 1)
                {
                    enemySpawnPosition = player.transform.position + (player.transform.forward * (characterOffset + characterSpacing));
                }
                else
                {
                    enemySpawnPosition = enemiesSpawned[i - 2].transform.position + (player.transform.forward * characterSpacing);
                }
                enemiesSpawned[i - 1] = (GameObject)Instantiate(enemiesList[enemyNumber], enemySpawnPosition, Quaternion.identity);
                enemiesSpawned[i - 1].transform.position = new Vector3(enemiesSpawned[i - 1].transform.position.x, 0.5f * enemiesSpawned[i - 1].GetComponent<Collider>().bounds.size.y, enemiesSpawned[i - 1].transform.position.z);
            }
            if(layout == BattleLayout.Trapezoid)
            {
                if (i == 1)
                {
                    enemySpawnPosition = player.transform.position + (player.transform.forward * characterOffset);
                }
                else if (i == 2)
                {
                    enemySpawnPosition = enemiesSpawned[i - 2].transform.position + (enemiesSpawned[i - 2].transform.right * characterSpacing);
                }
                else if (i == 3)
                {
                    enemySpawnPosition = enemiesSpawned[i - 2].transform.position - (enemiesSpawned[i - 2].transform.forward * backRowOffset * -1) + (enemiesSpawned[i - 3].transform.right);
                }
                else if (i == 4)
                {
                    enemySpawnPosition = enemiesSpawned[i - 4].transform.position - (enemiesSpawned[i - 4].transform.forward * backRowOffset * -1) - (enemiesSpawned[i - 4].transform.right);
                }
                enemiesSpawned[i - 1] = (GameObject)Instantiate(enemiesList[enemyNumber], enemySpawnPosition, Quaternion.identity);
                enemiesSpawned[i - 1].transform.position = new Vector3(enemiesSpawned[i - 1].transform.position.x, 0.5f * enemiesSpawned[i - 1].GetComponent<Collider>().bounds.size.y, enemiesSpawned[i - 1].transform.position.z);
            }
        }
        
    }
}
