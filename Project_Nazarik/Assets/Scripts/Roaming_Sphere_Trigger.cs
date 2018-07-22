using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roaming_Sphere_Trigger : MonoBehaviour {

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Enemy(WORLD)")
        {
            other.GetComponent<Enemy_Behavior>().ChangeTarget(this.gameObject);
        }
    }
}
