using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Behavior : MonoBehaviour {

    [SerializeField] GameObject m_target;
    [SerializeField] GameObject roamingSphere;
    [SerializeField] float roamRange = 0;
    [SerializeField] float detectionAngle = 0;
    [SerializeField] float detectionDistance = 0;
    [SerializeField] float chaseSpeed = 0;

    private GameObject previousTarget;
    private Vector3 targetDir;
    private float angle = 0;
    private float range = 0;
    private Vector3 scale;

	// Use this for initialization
	void Start ()
    {
        roamingSphere.transform.position = transform.position;
        scale.x = roamRange;
        scale.z = roamRange;
        roamingSphere.transform.localScale += scale;
	}
	
	// Update is called once per frame
	void Update () {
        targetDir = m_target.transform.position - transform.position;
        angle = Vector3.Angle(targetDir, transform.forward);

        range = Vector3.Magnitude(targetDir);
        

        if(angle <= detectionAngle)
        {
            //Debug.Log("within angle");

            if(range <= detectionDistance)
            {
                //Debug.Log("within range");

                transform.LookAt(m_target.transform, transform.up);
                transform.Translate(transform.forward * chaseSpeed * Time.deltaTime);
            }
        }

        if(m_target.tag == "Roaming_Sphere")
        {
            transform.LookAt(m_target.transform, transform.up);
            //transform.Translate(transform.forward * chaseSpeed * Time.deltaTime);

            Vector3 position = Vector3.Lerp(transform.position, m_target.transform.position, Time.deltaTime);
            transform.position = position;

            if (1==3) //make this so that the enemy only needs to get close to center of roaming sphere
            {
                m_target = previousTarget;
            }
        }

        Debug.Log(m_target.name);
	}

    public void ChangeTarget(GameObject target)
    {
        previousTarget = m_target;
        m_target = target;
    }
}
