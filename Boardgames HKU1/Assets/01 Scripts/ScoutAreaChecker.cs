using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScoutAreaChecker : MonoBehaviour
{
    [SerializeField] GameObject ExclamationMark;
    private void Awake()
    {
        ExclamationMark.SetActive(false);
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Food")
        {
            Debug.Log("I SEE FOOD!!");
            ExclamationMark.SetActive(true);

        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Food")
        {
            Debug.Log("I Dont see food no more...!!");
            ExclamationMark.SetActive(false);
        }
    }
}
