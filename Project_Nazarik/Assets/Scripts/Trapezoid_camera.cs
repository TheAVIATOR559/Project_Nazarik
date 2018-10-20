using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trapezoid_camera : MonoBehaviour {

    [SerializeField] float damping = 1;

    private Vector3 m_desiredPosition;
    private Quaternion m_targetRotation;
    private Vector3 m_targetPosition;
    private Vector3 m_finalPosition;
    private Vector3 m_midPoint;
	
	// Update is called once per frame
	void LateUpdate () {
        Vector3 position = Vector3.Lerp(transform.position, m_desiredPosition, Time.deltaTime * damping);
        transform.position = position;

        m_targetRotation = Quaternion.LookRotation(m_targetPosition - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, m_targetRotation, Time.deltaTime * damping);
	}

    public void ChangeTarget(Vector3 desiredPosition, Vector3 targetPosition)
    {
        m_desiredPosition = desiredPosition;
        m_targetPosition = targetPosition;
    }

    public void ChangeBattlePosition(Vector3 midPoint, Vector3 finalPosition)
    {
        //somehow moves that camera to a midpoint roughly 2 above the battleController and then move it to the final position
        //I have no idea how to do this
        m_midPoint = midPoint;
        m_finalPosition = finalPosition;
    }
}
