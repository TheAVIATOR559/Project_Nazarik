using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon_Crawler : MonoBehaviour {

    [SerializeField] GameObject m_target;
    [SerializeField] float damping = 1;

    private Vector3 m_desiredPosition;
    private Quaternion m_targetRotation;

    void LateUpdate()
    {
        //moves the camera all slow and cinematic like
        Vector3 position = Vector3.Lerp(transform.position, m_desiredPosition, Time.deltaTime * damping);
        transform.position = position;
            
        //rotates the camera to look at the target all slow and cinematic like
        m_targetRotation = Quaternion.LookRotation(m_target.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, m_targetRotation, Time.deltaTime * damping);
    }

    public void OnBattleStart(Vector3 desiredPostion)
    {
        m_desiredPosition = desiredPostion;
    }

    public void ChangeTarget(GameObject target)
    {
        m_target = target;
    }

}
