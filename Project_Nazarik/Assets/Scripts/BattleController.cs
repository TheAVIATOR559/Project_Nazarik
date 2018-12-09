using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour {

    [SerializeField] GameObject[] enemiesList;
    [SerializeField] GameObject player; //this does not work if it set to the player prefab
    [SerializeField] GameObject[] alliesList;
    [SerializeField] Camera mainCamera;
    [SerializeField] float enemyOffset = 5;
    [SerializeField] float characterSpacing = 2;
    [SerializeField] float cameraHorizontalDistance = 1;
    [SerializeField] float cameraVerticalDistance = 1;

    private Vector3 enemySpawnPosition;
    private Vector3 enemyDirection;
    private Vector3 battleCameraPosition;
    private int enemyCount;
    private Vector3 allySpawnPosition;
    private Vector3 allyDirection;
    private Vector3[] cameraTargetPosition = new Vector3[3]; //0 for left, 1 for center, 2 for right
    private List<GameObject> characterList = new List<GameObject>();
    

    private void OnEnable()
    {
        enemyCount = UnityEngine.Random.Range(1, 5); //I kind of want to tweak this so 4 happens more often or at least it 'feels'random
        player.GetComponent<Player_Movement>().enabled = false;
        player.GetComponent<Rigidbody>().freezeRotation = true;

        SpawnEnemies(enemyCount);
        SpawnAllies(alliesList.Length);

        characterList.Add(player);
        foreach(GameObject obj in characterList)
        {
            obj.GetComponent<Character>().enabled = true;
        }

        ///FAIR WARNING I HAVE NO IDEA HOW THIS WORKS
        ///I STOLE IT FROM THE SECOND ANSWER ON THIS STACKEXCHANGE POST: https://stackoverflow.com/questions/3309188/how-to-sort-a-listt-by-a-property-in-the-object
        characterList.Sort((x, y) => y.GetComponent<Character>().Initiative.Value.CompareTo(x.GetComponent<Character>().Initiative.Value));        

        //move battleController into position
        //is there even a need to move the battleController?

        //disable camera mouseAim script
        mainCamera.GetComponent<Mouse_Aim>().enabled = false;

        //calculate camera position for battle
        battleCameraPosition = player.transform.position + (0.5f * (player.transform.forward * enemyOffset)) - (player.transform.right * cameraHorizontalDistance);
        battleCameraPosition.y += cameraVerticalDistance;

        cameraTargetPosition[0] = player.transform.position + (player.transform.forward * enemyOffset) + (player.transform.right * (1.5f * characterSpacing));
        cameraTargetPosition[1] = player.transform.position + (player.transform.forward * (0.5f * enemyOffset)) + (player.transform.right * (1.5f * characterSpacing));
        cameraTargetPosition[2] = player.transform.position + (player.transform.right * (1.5f * characterSpacing));

        //enable corresponding camera script and move camera
        mainCamera.GetComponent<Camera_Battle>().enabled = true;
        mainCamera.GetComponent<Camera_Battle>().SetTargetPosition(cameraTargetPosition[1]);
        mainCamera.GetComponent<Camera_Battle>().SetStandbyPosition(battleCameraPosition);
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyUp(KeyCode.B))
        {
            characterList.Remove(player);
            player.GetComponent<Character>().enabled = false;
            foreach(GameObject obj in characterList)
            {
                Destroy(obj);
            }

            //disable battle camera script
            //TODO should be a more efficient way to do this
            mainCamera.GetComponent<Camera_Battle>().enabled = false;
            mainCamera.GetComponent<Mouse_Aim>().enabled = true;

            //enable player movement
            //TODO should be a more efficient way to do this
            player.GetComponent<Player_Movement>().enabled = true;
            player.GetComponent<Rigidbody>().freezeRotation = false;
            this.GetComponent<BattleController>().enabled = false;
        }

        if (Input.GetKeyUp(KeyCode.J))
        {
            Debug.Log("Player Turn");
            //TODO should be a more efficient way to do this
            mainCamera.GetComponent<Camera_Battle>().SetTargetPosition(cameraTargetPosition[2]);
        }
        if (Input.GetKeyUp(KeyCode.K))
        {
            Debug.Log("Enemy Turn");
            //TODO should be a more efficient way to do this
            mainCamera.GetComponent<Camera_Battle>().SetTargetPosition(cameraTargetPosition[0]);
        }

        
        if (Input.GetKeyUp(KeyCode.T)) //TODO remove me once done testing
        {
            foreach(GameObject obj in characterList)
            {
                Debug.Log(obj.name + "\n" + obj.GetComponent<Character>().GetStats());
            }
        }
        
        //TODO handle changing turns and battle flow here


    }

    private void SpawnAllies(int allyCount)
    {
        int m_allyCount = allyCount - 1;

        allyDirection = player.transform.forward;
        for(int i = 0; i <= m_allyCount; i++)
        {

            //spawn position and instantiate
            allySpawnPosition = player.transform.position + (player.transform.right * (characterSpacing * (i + 1)));

            //TODO should be a more efficient way to get this info
            allySpawnPosition.y = 0.5f * alliesList[i].GetComponent<Renderer>().bounds.size.y;

            //alliesSpawned[i] = Instantiate(alliesList[i], allySpawnPosition, Quaternion.LookRotation(allyDirection));
            characterList.Add(Instantiate(alliesList[i], allySpawnPosition, Quaternion.LookRotation(allyDirection)));
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

            //TODO should be more efficient way to get this
            enemySpawnPosition.y = 0.5f * enemiesList[enemyNumber].GetComponent<Renderer>().bounds.size.y;

            //enemiesSpawned[i] = Instantiate(enemiesList[enemyNumber], enemySpawnPosition, Quaternion.LookRotation(enemyDirection));
            characterList.Add(Instantiate(enemiesList[enemyNumber], enemySpawnPosition, Quaternion.LookRotation(enemyDirection)));
        }

    }
}
