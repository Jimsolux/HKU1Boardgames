using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntHealth : MonoBehaviour
{
    [SerializeField] public int health;
    [SerializeField] private int maxHealth;

    //Reference ant manager
    AntManager antManager = null;
    ThisAntHandler myHandler;

    private void Awake()
    {
        health = maxHealth;

        myHandler = gameObject.GetComponent<ThisAntHandler>();
    }

    private void GetManager()
    {
        if (antManager == null && myHandler.activeAntManager != null )
        {
            antManager = myHandler.activeAntManager;
        }
    }

    private void CheckHealth()
    {
        if (health <= 0)
        {
            GetManager();
            //Delete the list I am in, then delete me.
            if( antManager != null )
            {
                antManager.EmptyAntLine(myHandler.myLineIndex);
            }
            Destroy(gameObject);
        }
    }

    public void GainHealth(int value) { health += value; CheckHealth(); }

    public void GainDamage(int value) { health -= value; CheckHealth(); }

}
