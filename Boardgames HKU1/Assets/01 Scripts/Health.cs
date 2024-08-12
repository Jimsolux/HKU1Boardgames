
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] public int health;
    [SerializeField] private int maxHealth;



    private void Awake()
    {
        health = maxHealth;
    }



    private void CheckHealth()
    {
        if (health <= 0)
        {
            gameObject.SetActive(false);
            StartCoroutine(DestroyAfterAwhile());
        }
    }

    public void GainHealth(int value) { health += value; CheckHealth(); }

    public void GainDamage(int value) { health -= value; CheckHealth(); Debug.Log("gained " + value + "Damage"); }

    private IEnumerator DestroyAfterAwhile()
    {
        while (true)
        {
            yield return new WaitForSeconds(5);
            Destroy(gameObject);
        }
    }
}
