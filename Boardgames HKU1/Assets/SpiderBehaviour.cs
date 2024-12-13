using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.PlayerSettings;

public class SpiderBehaviour : MonoBehaviour
{
    LineController lineController;
    public NavMeshAgent myAgent;
    public Transform myHole;
    public GameObject targetAnt = null;
    public GameObject caughtAnt;
    [SerializeField] GameObject fakeAnt;
    [SerializeField] Transform fangPositionForAnt;
    GameObject currentFakeAnt = null;
    AudioSource biteAudio;
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
        AddObjectLocationsToPatrolSpots();
        lineController = GameObject.Find("LineDrawer").GetComponent<LineController>();
        biteAudio = GetComponent<AudioSource>();
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
    int currentPatrolIndex = 0;

    private void RoamingBehaviour()
    {
        if (IHavePatrolSpots()) //PatrolBehaviour
        {
            //Move to the first patrol spot. When arrived, move to the second patrol spot etc.
            Vector3 targetPos = patrolSpots[currentPatrolIndex];
            //Move to the tagetPos.
            myAgent.SetDestination(targetPos);
            
            if (Vector3.Distance(transform.position, targetPos) < 1 )
            {
                currentPatrolIndex++;
            }
            if(currentPatrolIndex > patrolSpots.Count -1) currentPatrolIndex = 0;//if max, reset


        }
        else    //Roaming Behaviour
        {
            //Roam, pick random points in a range of distances, randomly around me.
            float maxRange = 10;
            float minRange = 1;

            if (!hasDestination && !hasTarget && !roaming)
            {
                StartCoroutine(GoToRoamSpot(minRange, maxRange));
            }
        }


        if (hasTarget && targetLoc != transform.position)//Target?
        {
            activestate = SpiderStatesEnum.Attacking;
        }
    }


    #region patrolling
    [SerializeField] List<Vector3> patrolSpots = new List<Vector3>();
    [SerializeField] List<GameObject> patrolObjects = new List<GameObject>();


    private bool IHavePatrolSpots()
    {
        if (patrolSpots.Any())
        {
            return true;
        }
        else { return false; }
    }

    private void AddObjectLocationsToPatrolSpots()
    {
        foreach (GameObject obj in patrolObjects) {
            patrolSpots.Add(obj.GetComponent<Transform>().position);
        }
    }

    #endregion
    private void AttackingBehaviour()
    {
            targetLoc = targetAnt.transform.position;
        if (hasTarget && targetLoc != transform.position)// target is not where i am
        {
            if (Vector3.Distance(transform.position, targetLoc) > 0.5f)
            {
                myAgent.SetDestination(targetLoc);
            }

            if (Vector3.Distance(transform.position, targetLoc) < 1.3f)
            {
                Debug.Log("Ant is hella close to me imma eat");
                //Incapacitate ant
                if (targetAnt != null)
                {
                    biteAudio.Play();
                    caughtAnt = targetAnt;
                    //Disables caughtAnt
                    //Removes the line that the ant was in.
                    ThisAntHandler caughtAntHandler = caughtAnt.GetComponent<ThisAntHandler>();
                    if (caughtAntHandler.myLineIndex != -1)
                    {
                        caughtAntHandler.activeAntManager.EmptyAntLine(caughtAntHandler.myLineIndex);//Empty the current Ant line the ant is in.
                        lineController.ClearList(caughtAntHandler.myLineIndex);//Clear the Line controllers list of this ant.
                    }
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
        if(Vector3.Distance(transform.position, myHole.position) > 1f)
        {
            if(myAgent.destination != myHole.position)
            {
                myAgent.SetDestination(myHole.position);
            }
        }
        else if (Vector3.Distance(transform.position, myHole.position) < 1f)
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

    bool roaming;
    private IEnumerator GoToRoamSpot(float min, float max)
    {
        while (true)
        {
            roaming = true;
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
                roaming = false;
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
        if (!hasTarget)
        {
            hasTarget = true;
            targetLoc = target.transform.position;
            targetAnt = target;
        }
    }

    public void RemoveTarget()
    {
        hasTarget = false;
        targetLoc = transform.position;
        targetAnt = null;
    }
    #endregion
}
