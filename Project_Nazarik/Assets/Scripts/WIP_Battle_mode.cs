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
    private bool battlemode = false;
    private GameObject[] enemiesSpawned = new GameObject[4];
    private bool battlemodeStart = false;
    private Vector3 battlePosition;
    //private int buffer = 10;
    private Vector3 enemyDirection;
    private Vector3 battleCameraPostion;
    //private GameObject[] enemiesToSpawn;
    private int enemyCount;
    private int battleLayout;//0 for line | 1 for shape

	// Use this for initialization
	void Start () {
		
	}

    private void OnEnable()
    {
        battleLayout = /*Random.Range(0, 1)*/ 1;
        enemyCount = /*Random.Range(1, 4)*/ 4;
        Debug.Log("enemyCount: " + enemyCount);
        player.GetComponent<Player_Movement>().enabled = false;
        for(int i = 1; i <= enemyCount; i++)
        {
            //Debug.Log(i);
            int enemyNumber = Random.Range(0, enemiesList.Length);
            if(battleLayout == 0)//line layout
            {
                //enemyOffset = enemyOffset * i;

                if(i == 1)
                {
                    enemySpawnPosition = player.transform.position + (player.transform.forward * (enemyOffset + enemySpacing));
                    //Debug.Log("enemy spawn position math:" + player.transform.position + " + (" + player.transform.forward + " * " + enemyOffset + " + " + enemySpacing + ")");
                }
                else
                {
                    //Debug.Log(i - 1);
                    enemySpawnPosition = enemiesSpawned[i - 2].transform.position + (player.transform.forward * enemySpacing);
                    //Debug.Log("enemy spawn position math:" + enemiesSpawned[i - 2].transform.position + " + (" + player.transform.forward + " * " + " + " + enemySpacing + ")");
                }

                
                //Debug.Log("enemySpawnPosition variable:" + enemySpawnPosition);
                enemiesSpawned[i - 1] = (GameObject)Instantiate(enemiesList[enemyNumber], enemySpawnPosition, Quaternion.identity);
                //Debug.Log(enemiesSpawned[i -1].name + ": " + enemiesSpawned[i - 1].transform.position);
                enemyDirection = player.transform.position - enemiesSpawned[i - 1].transform.position;
                enemiesSpawned[i - 1].transform.rotation = Quaternion.LookRotation(enemyDirection);
            }
            else if(battleLayout == 1)//shape layout
            {
                if(i == 1)
                {
                    enemySpawnPosition = player.transform.position + (player.transform.forward * enemyOffset);
                }
                else if(i == 2)
                {
                    enemySpawnPosition = enemiesSpawned[i - 2].transform.position + (enemiesSpawned[i - 2].transform.right * enemySpacing);
                }
                else if(i == 3)
                {
                    enemySpawnPosition = enemiesSpawned[i - 3].transform.position + (enemiesSpawned[i - 3].transform.right * (enemySpacing * 2)) + (enemiesSpawned[i - 3].transform.forward * backRowOffset);//pull this in toward the center a bit
                }
                else if(i == 4)
                {
                    enemySpawnPosition = enemiesSpawned[i - 4].transform.position - (enemiesSpawned[i - 4].transform.right * enemySpacing) + (enemiesSpawned[i - 4].transform.forward * backRowOffset);//pull this in toward the center a bit
                }

                enemiesSpawned[i - 1] = (GameObject)Instantiate(enemiesList[enemyNumber], enemySpawnPosition, Quaternion.identity);
                Debug.Log(enemiesSpawned[i - 1].name + ": " + enemiesSpawned[i - 1].transform.position);
                //enemyDirection = player.transform.position - enemiesSpawned[i - 1].transform.position;
                //enemiesSpawned[i - 1].transform.rotation = Quaternion.LookRotation(enemyDirection);

            }
            
        }
        


        //enemySpawnPosition = player.transform.position + (player.transform.forward * enemyOffset);
        //enemySpawned = (GameObject)Instantiate(enemyToSpawn, enemySpawnPosition, Quaternion.identity);

        //enemyDirection = player.transform.position - enemySpawned.transform.position;
        //enemySpawned.transform.rotation = Quaternion.LookRotation(enemyDirection);

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

            mainCamera.GetComponent<Dungeon_Crawler>().enabled = false;
            mainCamera.GetComponent<Mouse_Aim>().enabled = true;

            //enable player movement
            player.GetComponent<Player_Movement>().enabled = true;
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
            //mainCamera.GetComponent<Dungeon_Crawler>().ChangeTarget(enemySpawned);
        }

        //buffer--;
    }
}
