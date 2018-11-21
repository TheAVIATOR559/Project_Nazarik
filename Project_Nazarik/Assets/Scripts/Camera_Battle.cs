using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Battle : MonoBehaviour {

    private Vector3 m_targetPosition;
    [SerializeField] float damping = 1;
    private Vector3 m_standbyPosition;
    

	// Use this for initialization
	void OnEnable () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
        Vector3 position = Vector3.Lerp(transform.position, m_standbyPosition, Time.deltaTime * damping);
        transform.position = position;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(m_targetPosition - transform.position), Time.deltaTime * damping);
    }

    public Vector3 GetTargetPosition()
    {
        return m_targetPosition;
    }

    public void SetTargetPosition(Vector3 target)
    {
        m_targetPosition = target;
    }

    public Vector3 GetStandbyPosition()
    {
        return m_standbyPosition;
    }

    public void SetStandbyPosition(Vector3 newPosition)
    {
        m_standbyPosition = newPosition;
    }
}
