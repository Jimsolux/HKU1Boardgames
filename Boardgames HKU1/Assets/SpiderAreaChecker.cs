using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpiderAreaChecker : MonoBehaviour
{
    SpiderBehaviour myBehaviour;

    private void Awake()
    {
        myBehaviour = GetComponentInParent<SpiderBehaviour>();
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Ant" || collision.gameObject.tag == "OtherAnt")
        {
            //Debug.Log("juice ant spotted!!");
            myBehaviour.GetTarget(collision.gameObject);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "OtherAnt")
        {
            //Debug.Log("ENEMY Vanished!!");
            myBehaviour.RemoveTarget();
        }
    }
}
