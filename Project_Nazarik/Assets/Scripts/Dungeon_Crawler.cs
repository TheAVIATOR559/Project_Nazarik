using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon_Crawler : MonoBehaviour {

        [SerializeField] GameObject target;
        [SerializeField] float damping = 1;
        private Vector3 offset;

    //private void Start()
    //{
    //    offset = transform.position - target.transform.position;
    //}

    //private void OnDisable()
    //{
    //    Debug.Log("offset: " + offset);
    //    Debug.Log("position: " + transform.position);
    //    Debug.Log("rotation: " + transform.rotation);
    //}

    void OnEnable()
        {
        

        offset = transform.position - target.transform.position;

    }

    void LateUpdate()
        {
            Vector3 desiredPosition = target.transform.position + offset;
            Vector3 position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * damping);
            transform.position = position;

            transform.LookAt(target.transform.position);
        }

}
