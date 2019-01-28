using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySelectButton : MonoBehaviour {

    public GameObject enemyPrefab;

	public void SelectEnemy()
    {
        GameObject.Find("BattleController").GetComponent<BattleController>().EnemySelection(enemyPrefab);
        //Debug.Log(enemyPrefab.name);
    }

    public void OnMouseEnter()
    {
        enemyPrefab.transform.GetChild(0).gameObject.SetActive(true);
    }
    public void OnMouseExit()
    {
        enemyPrefab.transform.GetChild(0).gameObject.SetActive(false);
    }
}
