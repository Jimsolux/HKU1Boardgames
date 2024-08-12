using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class SpiderBehaviour : MonoBehaviour
{
    public NavMeshAgent myAgent;

    public enum SpiderStatesEnum
    {
        Roaming,
        Attacking
    }

    public SpiderStatesEnum activestate;

    private void Awake()
    {
        myAgent = GetComponent<NavMeshAgent>();
        activestate = SpiderStatesEnum.Roaming;
    }

    private void FixedUpdate()
    {
        ActBehaviour();
    }

    private void ActBehaviour()
    {
        switch (activestate)
        {
            case SpiderStatesEnum.Roaming:
                RoamingBehaviour();
                break;
            case SpiderStatesEnum.Attacking:
                AttackingBehaviour();
                break;
        }
    }

    #region behaviour

    bool hasDestination = false;

    private void RoamingBehaviour()
    {
        //Roam, pick random points in a range of distances, randomly around me.
        float maxRange = 10;
        float minRange = 1;

        if(!hasDestination && !hasTarget)
        {
            StartCoroutine(GoToRoamSpot(minRange, maxRange));
        }

        if (hasTarget && targetLoc != transform.position)//Target?
        {
            activestate = SpiderStatesEnum.Attacking;
        }
    }

    private void AttackingBehaviour()
    {
        if (hasTarget && targetLoc != transform.position)// target is not where i am
        {
            if (Vector3.Distance(transform.position, targetLoc) > 0.5f)
            {
                myAgent.SetDestination(targetLoc);
            }
        }
        if (!hasTarget) activestate = SpiderStatesEnum.Roaming;
    }

    #endregion

    private Vector3 PickRandomDirection(float min, float max)
    {
        // Generate a random point inside a unit sphere
        Vector3 randomDirection = Random.insideUnitSphere;

        // Flatten the direction to only consider the XZ plane (2D movement)
        randomDirection.y = 0;

        // Scale the direction to the desired move distance

        Vector3 targetdir = randomDirection.normalized * Random.Range(min, max);
        return targetdir;
    }

    private IEnumerator GoToRoamSpot(float min, float max)
    {
        while (true)
        {

            Vector3 pos = PickRandomDirection(min, max);

            if (Vector3.Distance(transform.position, transform.position + pos) > 0.5f)
            {
                myAgent.SetDestination(transform.position + pos);
                hasDestination = true;

            }

            if (Vector3.Distance(transform.position, transform.position + pos) <= 0.5f)
            {
                hasDestination = false;
            }

            if (hasTarget)
            {
                yield break;
            }

            yield return new WaitForSeconds(2);
        }
    }
    #region target
    //vars
    private bool hasTarget = false;
    Vector3 targetLoc;
    public void GetTarget(GameObject target)
    {
        hasTarget = true;
        targetLoc = target.transform.position;
    }

    public void RemoveTarget()
    {
        hasTarget = false;
        targetLoc = transform.position;
    }
    #endregion
}
