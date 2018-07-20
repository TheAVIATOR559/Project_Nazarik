﻿using System.Collections;
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
    private Vector3 battlePosition;
    private int buffer = 10;

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
                //Debug.Log("removing enemy...");
                Destroy(enemySpawned);
                //Debug.Log("enemy removed");
                
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
            enemySpawnPosition = player.transform.position;

            //enemySpawnPosition.x = enemySpawnPosition.x + enemyOffset;
            enemySpawnPosition.z = enemySpawnPosition.z + enemyOffset;

            player.GetComponent<Player_Movement>().enabled = false;

            enemySpawned = (GameObject) Instantiate(enemyToSpawn, enemySpawnPosition, Quaternion.identity);

            battlePosition = (enemySpawnPosition - player.transform.position) / 2;
            battlePosition = player.transform.position + battlePosition;

            //Debug.Log(battlePosition);

            transform.position = battlePosition;

            mainCamera.GetComponent<Mouse_Aim>().enabled = false;
            mainCamera.GetComponent<Dungeon_Crawler>().enabled = true;

            battlemodeStart = false;
            buffer = 10;
        }

        buffer--;
        //Debug.Log(buffer);
    }
}
