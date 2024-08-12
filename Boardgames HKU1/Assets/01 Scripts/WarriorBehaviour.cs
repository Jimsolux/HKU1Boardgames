using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WarriorBehaviour : MonoBehaviour
{
    //References
    [SerializeField] AntManager myManager;

    public NavMeshAgent antAgent;

    //ifSelected
    public bool isSelected;

    public List<GameObject> defendingLine = null;


    public enum WarriorStatesEnum
    {
        Idle,
        Attacking
    }

    public WarriorStatesEnum activestate;

    private void Awake()
    {
        antAgent = GetComponent<NavMeshAgent>();
        activestate = WarriorStatesEnum.Idle;//start idle
        myManager = GameObject.Find("NewHole").GetComponent<AntManager>();
    }


    private void FixedUpdate()
    {
        ActBehaviour();
    }

    private void ActBehaviour()
    {
        switch (activestate)
        {
            case WarriorStatesEnum.Idle: IdleBehaviour();
                break;
            case WarriorStatesEnum.Attacking: AttackingBehaviour();
                break;
        }
    }

    private void IdleBehaviour()
    {
        //If spawned, do nothing.
        if (hasTarget)// Switch to attacking
        {
            activestate = WarriorStatesEnum.Attacking;
        }
    }

    #region toomuchwork..
    //public bool hasDefendingLine = false;
    //private void PlayerDirectedBehaviour()
    //{
    //    //Only walk towards where placed, turn off other behaviour. Look for an activeLine and set it active, then patrol.
    //    if (antAgent.isStopped)
    //    {
    //        if(hasDefendingLine && defendingLine != null)
    //        {
    //            activestate = WarriorStatesEnum.Patrolling; // Switch to patrolling.

    //        }
    //    }
    //}

    //public void GetAntLine(int lineIndex)
    //{
    //    List<GameObject> activeLine = new List<GameObject>();
    //    switch (lineIndex)
    //    {
    //        case 0: activeLine = myManager.lineList1; break;
    //        case 1: activeLine = myManager.lineList2; break;
    //        case 2: activeLine = myManager.lineList3; break;
    //        case 3: activeLine = myManager.lineList4; break;
    //        case 4: activeLine = myManager.lineList5; break;
    //    }
    //    defendingLine = activeLine;
    //    hasDefendingLine = true;

    //}

    //public void RemoveAntLine()
    //{
    //    defendingLine = null;
    //    hasDefendingLine = false;
    //}
    #endregion
    #region patrolling
    //way too much work oops...
    ////patrolvars


    //private void PatrollingBehaviour()
    //{
    //    //Walk next to my DefendingLine and check for enemies, if enemy, attack.
    //    if (hasDefendingLine && defendingLine != null)
    //    {
    //        Vector3 firstAnt = new Vector3();
    //        Vector3 lastAnt = new Vector3();

    //        firstAnt = defendingLine[0].transform.position;
    //        lastAnt = defendingLine[defendingLine.Count].transform.position;

    //        //Destination - last ant + OFFSET OTHERWISE HE BUMPS INTO EM

    //        //if distance small enough, stop

    //        //turn around, Destination - first ant. Loop.


    //    }


    //    if (hasTarget)// Switch to attacking
    //    {
    //        activestate = WarriorStatesEnum.Attacking;
    //    }
    //}
    #endregion

    #region attacking
    //TargetInfo
    Vector3 targetLoc;
    private bool hasTarget = false;

    private void AttackingBehaviour()
    {
        //Set destination to enemy as long as they are alive.
        if(hasTarget&& targetLoc != transform.position)// target is not where i am
        {
            antAgent.SetDestination(targetLoc);
        }
        if (!hasTarget) activestate = WarriorStatesEnum.Idle; // Back to patrolling.
    }

    #endregion

    #region target
    public void GetTarget(GameObject target)
    {
        hasTarget = true;
        targetLoc =  target.transform.position;
    }

    public void RemoveTarget()
    {
        hasTarget = false;
        targetLoc = transform.position;
    }
    #endregion
}
