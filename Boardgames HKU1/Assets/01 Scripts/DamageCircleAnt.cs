using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCircleAnt : MonoBehaviour
{ 
    bool isDealingDamage = false;
    Health enemyHealth = null;


    private void OnTriggerStay(Collider collision)
    {
        Debug.Log(collision.tag + " and  " + this.transform.parent.name);

        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("I SEE MY ENEMY ");
            enemyHealth = collision.gameObject.GetComponent<Health>();
            if (!isDealingDamage)
            {
                StartCoroutine(SendDamage(enemyHealth));
            }
            Debug.Log("Started Coroutine senddamage");
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            enemyHealth = null;
            //Debug.Log("I Dont see food no more...!!");
        }
    }


    private IEnumerator SendDamage(Health health)
    {
        isDealingDamage = true;
        yield return new WaitForSeconds(0.5f);
        if (health != null && health.health > 0)
        {
            health.GainDamage(1);
            Debug.Log("One damage dealt to a ");
        }
        isDealingDamage = false;
    }






}
