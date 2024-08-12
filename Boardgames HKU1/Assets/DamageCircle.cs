using System.Collections;
using UnityEngine;

public class DamageCircle : MonoBehaviour
{

    bool isDealingDamage = false;
    AntHealth enemyHealth = null;


    private void OnTriggerStay(Collider collision)
    {
        //Debug.Log(collision.tag + " and  " + this.transform.parent.name);

        if (collision.gameObject.tag == "Ant" || collision.gameObject.tag == "OtherAnt")
        {
            Debug.Log("I SEE MY ENEMY ");
            enemyHealth = collision.gameObject.GetComponent<AntHealth>();
            if (!isDealingDamage)
            {
                StartCoroutine(SendDamage(enemyHealth));
                Debug.Log("Started Coroutine senddamage");
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Ant" || collision.gameObject.tag == "OtherAnt")
        {
            enemyHealth = null;
            //Debug.Log("I Dont see food no more...!!");
        }
    }


    private IEnumerator SendDamage(AntHealth health)
    {
        isDealingDamage = true;
        yield return new WaitForSeconds(0.5f);

        Debug.Log(enemyHealth.health);
        if (enemyHealth != null && enemyHealth.health > 0)
        {
            enemyHealth.GainDamage(1);
            Debug.Log("One damage dealt to a ");
        }
        isDealingDamage = false;
    }

}
