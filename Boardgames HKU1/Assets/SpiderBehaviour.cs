using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SpiderBehaviour : MonoBehaviour
{
    public NavMeshAgent myAgent;
    public Transform myHole;
    public GameObject targetAnt = null;
    public GameObject caughtAnt;
    [SerializeField] GameObject fakeAnt;
    [SerializeField] Transform fangPositionForAnt;
    GameObject currentFakeAnt = null;
    public enum SpiderStatesEnum
    {
        Roaming,
        Attacking,
        Dragging
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
            case SpiderStatesEnum.Dragging:
                DraggingBehaviour();
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

        if (!hasDestination && !hasTarget)
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
            targetLoc = targetAnt.transform.position;
        if (hasTarget && targetLoc != transform.position)// target is not where i am
        {
            if (Vector3.Distance(transform.position, targetLoc) > 0.5f)
            {
                myAgent.SetDestination(targetLoc);
            }

            if (Vector3.Distance(transform.position, targetLoc) < 1f)
            {
                Debug.Log("Ant is hella close to me imma eat");
                //Incapacitate ant
                if (targetAnt != null)
                {
                    //Re gets target position
                        //GetTarget(targetAnt); mehhhh

                    caughtAnt = targetAnt;
                    //Disables caughtAnt
                    caughtAnt.SetActive(false);
                    targetAnt = null;
                    caughtAnt = null;
                    //Make a fake ant in fangs
                    currentFakeAnt = Instantiate(fakeAnt, fangPositionForAnt.transform);
                    hasTarget = false;
                    //go dragg fakeant to hole
                    activestate = SpiderStatesEnum.Dragging;
                    return;
                }
            }
        }
        if (!hasTarget) activestate = SpiderStatesEnum.Roaming;
    }

    private void DraggingBehaviour()
    {
        if(Vector3.Distance(transform.position, myHole.position) > 0.5f)
        {
            if(myAgent.destination != myHole.position)
            {
                myAgent.SetDestination(myHole.position);
            }
        }
        else if (Vector3.Distance(transform.position, myHole.position) < 0.4f)
        {
            //Destroy fakeAnt
            Destroy(currentFakeAnt);
            currentFakeAnt = null;
            //Reset targets and all
            hasDestination = false;
            RemoveTarget();

            activestate = SpiderStatesEnum.Roaming;
        }
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
        targetAnt = target;
    }

    public void RemoveTarget()
    {
        hasTarget = false;
        targetLoc = transform.position;
        targetAnt = null;
    }
    #endregion
}
