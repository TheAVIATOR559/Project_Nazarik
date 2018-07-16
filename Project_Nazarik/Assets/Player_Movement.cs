using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour {

    [SerializeField] float movementSpeed = 0;
    private float horizontal;
    private float vertical;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        horizontal = Input.GetAxis("Horizontal") * Time.deltaTime * movementSpeed;
        vertical = Input.GetAxis("Vertical") * Time.deltaTime * movementSpeed;

        transform.Translate(horizontal, 0, vertical);

	}
}
