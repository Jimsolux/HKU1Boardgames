using UnityEngine;

public class AntAreaChecker : MonoBehaviour
{

    private ThisAntHandler myHandler;

    private void Awake()
    {
        myHandler = GetComponentInParent<ThisAntHandler>();//Gets its own handler from the parent.
    }


    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Food")
        {
            //Debug.Log("I SEE FOOD!!");
            myHandler.hasFoodNearby = true;
            myHandler.activeContainer = collision.gameObject.GetComponent<FoodContainer>();
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Food")
        {
            Debug.Log("I Dont see food no more...!!");

            myHandler.hasFoodNearby = false;
        }
    }


}
