using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WIP_Battle_mode : MonoBehaviour {

    [SerializeField] GameObject enemyToSpawn;
    [SerializeField] GameObject player;
    [SerializeField] Camera mainCamera;
    [SerializeField] float enemyOffset = 0;

    private Vector3 enemySpawnPosition;
    private bool battlemode = false;
    private GameObject enemySpawned;
    private bool battlemodeStart = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.B)) //initalizes battle mode for testing
        {
            if(battlemode)
            {
                battlemode = false;
                //remove enemy
                //move camera to non-battle position
                //enable player movement
            }
            if(!battlemode)
            {
                battlemode = true;
                battlemodeStart = true;
            }
        }

        if (battlemodeStart)
        {
            enemySpawnPosition = player.transform.position;

            enemySpawnPosition.x = enemySpawnPosition.x + enemyOffset;
            enemySpawnPosition.z = enemySpawnPosition.z + enemyOffset;

            player.GetComponent<Player_Movement>().enabled = false;
            enemySpawned = Instantiate(enemyToSpawn, enemySpawnPosition, Quaternion.identity);
            //have camera move to correct view
        }
    }
}
