using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Look_At : MonoBehaviour {

    [SerializeField] GameObject target;

    // Use this for initialization
    void Start()
    {
        //move camera to battle mode position
    }

    // Update is called once per frame
    void LateUpdate () {
        transform.LookAt(target.transform);
	}
}
