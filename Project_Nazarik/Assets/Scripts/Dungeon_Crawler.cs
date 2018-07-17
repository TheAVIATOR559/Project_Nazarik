using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon_Crawler : MonoBehaviour {

        [SerializeField] GameObject target;
        [SerializeField] float damping = 1;
        private Vector3 offset;

        void Start()
        {
            offset = transform.position - target.transform.position;
            //move to correct position
        }

        void LateUpdate()
        {
            Vector3 desiredPosition = target.transform.position + offset;
            Vector3 position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * damping);
            transform.position = position;

            transform.LookAt(target.transform.position);
        }

}
