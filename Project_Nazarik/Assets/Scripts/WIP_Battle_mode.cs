using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WIP_Battle_mode : MonoBehaviour {

    [SerializeField] GameObject[] enemiesList;
    [SerializeField] GameObject player;
    [SerializeField] GameObject[] alliesList;
    [SerializeField] Camera mainCamera;
    [SerializeField] float enemyOffset = 0;
    [SerializeField] float cameraBCDistance = 0;
    [SerializeField] float cameraVerticalOffset = 0;

    private Vector3 enemySpawnPosition;
    private bool battlemode = false;
    private GameObject enemySpawned;
    private bool battlemodeStart = false;
    private Vector3 battlePosition;
    private int buffer = 10;
    private Vector3 enemyDirection;
    private Vector3 battleCameraPostion;
    private GameObject enemyToSpawn;
    private int enemyCount;

	// Use this for initialization
	void Start () {
		
	}

    private void OnEnable()
    {
        enemyCount = Random.Range(1, 4);
        Debug.Log(enemyCount);
    }

    // Update is called once per frame
    void Update () {

        //change this to an event flag of start of battle
        if (Input.GetKeyUp(KeyCode.B))
        {
            if(battlemode)
            {
                battlemode = false;
                Destroy(enemySpawned);

                mainCamera.GetComponent<Dungeon_Crawler>().enabled = false;
                mainCamera.GetComponent<Mouse_Aim>().enabled = true;

                //enable player movement
                player.GetComponent<Player_Movement>().enabled = true;

                buffer = 10;
            }
            if(!battlemode && buffer <= 0)
            {
                battlemode = true;
                battlemodeStart = true;
            }
        }

        if (battlemodeStart)
        {
            enemySpawnPosition = player.transform.position + (player.transform.forward * enemyOffset);

            player.GetComponent<Player_Movement>().enabled = false;

            enemySpawned = (GameObject) Instantiate(enemyToSpawn, enemySpawnPosition, Quaternion.identity);

            enemyDirection = player.transform.position - enemySpawned.transform.position;
            enemySpawned.transform.rotation = Quaternion.LookRotation(enemyDirection);

            battlePosition = (enemySpawnPosition - player.transform.position) / 2;
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
            

            battlemodeStart = false;
            buffer = 10;
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
            mainCamera.GetComponent<Dungeon_Crawler>().ChangeTarget(enemySpawned);
        }

        buffer--;
    }
}
