using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WarriorAreaChecker : MonoBehaviour
{
    WarriorBehaviour myBehaviour;

    private void Awake()
    {
        myBehaviour = GetComponentInParent<WarriorBehaviour>();
    }


    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            //Debug.Log("ENEMY SPOTTED!!");
            myBehaviour.GetTarget(collision.gameObject);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            //Debug.Log("ENEMY Vanished!!");
            myBehaviour.RemoveTarget();
        }
    }
}
