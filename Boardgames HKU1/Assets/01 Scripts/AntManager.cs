using FischlWorks_FogWar;
using System.Collections;
using System.Collections.Generic;
using System.Linq;



//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class AntManager : MonoBehaviour
{
    //public GameObject currentAnt;

    //public List<GameObject> nearbyAntList = new List<GameObject>();

    public List<GameObject> lineList1 = new List<GameObject>();
    public List<GameObject> lineList2 = new List<GameObject>();
    public List<GameObject> lineList3 = new List<GameObject>();// = new List<GameObject>();
    public List<GameObject> lineList4 = new List<GameObject>();// = new List<GameObject>();
    public List<GameObject> lineList5 = new List<GameObject>();// = new List<GameObject>();

    public List<List<GameObject>> allLines = new List<List<GameObject>>();


    //This script will do the following things:
    //- check if an ant is nearby
    //- Add the ant to a list
    //- Send this manager to the ant
    //- make the ant look for more ants to add to the list
    // This all so that the ants can form a line by being nearby eachother.

    //This script still needs to be expanded so that there can be multiple ants in the area that will create multiple lines..
    //Now it will only add them to one list.

    private void Awake()
    {
        FillAntLinesList();// Fills the ants
    }
    private void FillAntLinesList()
    {
        allLines.Add(lineList1);
        allLines.Add(lineList2);
        allLines.Add(lineList3);
        allLines.Add(lineList4);
        allLines.Add(lineList5);
    }

    private void Start()
    {
        StartCoroutine(CheckSceneForAnts());
        InvokeRepeating("CheckAntsNearHole", 0, 0.2f);
        InvokeRepeating("GrabFoodSignal", 3, 1);
    }


    #region AreaCheckForAnts

    public float maxHoleDistance = 4;
    public void CheckAntsNearHole()// Every second, also starts the checks for all the ants in the line.
    {
        Debug.Log("CheckAntsNearHole every second.");
        foreach (GameObject ant in antsInScene)
        {

            float distance = Vector3.Distance(transform.position, ant.transform.position);
            ThisAntHandler thisAntHandler = ant.GetComponent<ThisAntHandler>();

            if (distance < maxHoleDistance && !thisAntHandler.inLine)       //SITUATION : A close ant is not in a line!
            {
                int index = CreateNewAntLine(thisAntHandler);
                thisAntHandler.ReceiveLineData(this, 0, this.gameObject);
                thisAntHandler.AddAntToList(ant, thisAntHandler.myLineIndex);

                LookingBehaviourFirstAnt(thisAntHandler.myLineIndex);
                //Starts to have the first Ant look around themselves for other ants.
            }
            else if (distance < maxHoleDistance && thisAntHandler.inLine)   // SITUATION: A nearby ant is in a line!
            {
                LookingBehaviourFirstAnt(thisAntHandler.myLineIndex);
                //Starts to have the first Ant look around themselves for other ants.
                AllAntsInLineLookARound(thisAntHandler.myLineIndex);
            }


            else if (distance >= maxHoleDistance && thisAntHandler != null) // SITUATION: Ant from my line TOO FAR AWAY!
            {
                List<GameObject> activeLine = new List<GameObject>();
                switch (thisAntHandler.myLineIndex)
                {
                    case 0: activeLine = lineList1; break;
                    case 1: activeLine = lineList2; break;
                    case 2: activeLine = lineList3; break;
                    case 3: activeLine = lineList4; break;
                    case 4: activeLine = lineList5; break;
                }
                if (activeLine.Any() && activeLine[0] == ant)// If the ant is on the first spot in the list but is too far away, remove it from the list.
                {
                    //activeLine[0] = null;
                    RemoveAntFromList(ant, thisAntHandler.myLineIndex);
                    EmptyAntLine(thisAntHandler.myLineIndex);
                    thisAntHandler.DestroyLineData();
                    thisAntHandler.SetInLine(false);
                }

            }

        }
    }


    public void AllAntsInLineLookARound(int index)
    {
        List<GameObject> activeLine = new List<GameObject>();// Active LineList
        switch (index)
        {
            case 0: activeLine = lineList1; break;
            case 1: activeLine = lineList2; break;
            case 2: activeLine = lineList3; break;
            case 3: activeLine = lineList4; break;
            case 4: activeLine = lineList5; break;
        }
        foreach (GameObject ant in activeLine)
        {
            ThisAntHandler tah = ant.GetComponent<ThisAntHandler>();
            if (tah != null && tah.myLineIndex != -1)
            {
                tah.CheckAntDistance(index);
            }
        }
    }

    #endregion


    #region FirstAntBehaviour

    public float maxAntDistance = 4;
    public GameObject[] antsInScene;


    public void LookingBehaviourFirstAnt(int index)
    {
        List<GameObject> activeLine = new List<GameObject>();// Active LineList
        switch (index)
        {
            case 0: activeLine = lineList1; break;
            case 1: activeLine = lineList2; break;
            case 2: activeLine = lineList3; break;
            case 3: activeLine = lineList4; break;
            case 4: activeLine = lineList5; break;
        }
        if (activeLine.Any())
        {
            ThisAntHandler myAntHandler = activeLine[0].GetComponent<ThisAntHandler>();
            myAntHandler.CheckAntDistance(index);
        }
    }

    #endregion


    private IEnumerator CheckSceneForAnts()
    {
        while (true)
        {
        antsInScene = GameObject.FindGameObjectsWithTag("Ant");
        Debug.Log("------ Checking scene for Ants -----");

        yield return new WaitForSeconds(1);
        }
    }



    private int CreateNewAntLine(ThisAntHandler thisAntHandler)
    {
        for (int i = 0; i < allLines.Count; i++) //Looks if lines exist, if not, creates one line.
        {
            List<GameObject> activeLine = new List<GameObject>();
            switch (i)
            {
                case 0: activeLine = lineList1; break;
                case 1: activeLine = lineList2; break;
                case 2: activeLine = lineList3; break;
                case 3: activeLine = lineList4; break;
                case 4: activeLine = lineList5; break;
            }
            if (!activeLine.Any())// || allLines[i] == null)
            {
                //Debug.Log("All lines with index " + i + "is empty");
                allLines[i] = new List<GameObject>();
                //List<GameObject> currentLine = allLines[i];
                thisAntHandler.ReceiveLineIndex(i);
                //Debug.Log("Created a new Line List");


                return i;
            }
            //else it does +1.
        }
        return -1;

    }





    public void MakeAllAntsForgetLists(int lineIndex)
    {
        List<GameObject> activeLine = new List<GameObject>();// Active LineList
        switch (lineIndex)
        {
            case 0: activeLine = lineList1; break;
            case 1: activeLine = lineList2; break;
            case 2: activeLine = lineList3; break;
            case 3: activeLine = lineList4; break;
            case 4: activeLine = lineList5; break;
        }

        foreach (GameObject thisAnt in activeLine)
        {
            ThisAntHandler thisAntHandler = thisAnt.GetComponent<ThisAntHandler>();
            thisAntHandler.DestroyLineData();
            thisAntHandler.SetInLine(false);

        }
    }


    public void EmptyAntLine(int index)
    {
        Debug.Log("Attempting to clear the array n " + index);
        MakeAllAntsForgetLists(index);
        //allLines[index] = null;
        switch (index)
        {
            case 0: lineList1.Clear(); break;
            case 1: lineList2.Clear(); break;
            case 2: lineList3.Clear(); break;
            case 3: lineList4.Clear(); break;
            case 4: lineList5.Clear(); break;
        }
    }

    #region CheckForSpecificAntsToBeDeleted

    public bool CheckIfAllAntsAreTooFarAway(int listIndex, GameObject ant)
    {
        List<GameObject> activeLine = new List<GameObject>();// Active LineList
        switch (listIndex)
        {
            case 0: activeLine = lineList1; break;
            case 1: activeLine = lineList2; break;
            case 2: activeLine = lineList3; break;
            case 3: activeLine = lineList4; break;
            case 4: activeLine = lineList5; break;
        }
        int countOfAntsCloseEnough = 0;

        foreach (GameObject thisAnt in activeLine)
        {
            bool closeEnough = CheckIfAntIsCloseEnough(thisAnt, ant);
            //if one returns close enough, return from function.
            if (!closeEnough) countOfAntsCloseEnough++;
            else if (closeEnough) return false;

        }
        if (countOfAntsCloseEnough == activeLine.Count) return true;
        return false;
    }


    private bool CheckIfAntIsCloseEnough(GameObject antInLine, GameObject ant)
    {
        float distance = Vector3.Distance(antInLine.transform.position, ant.transform.position);
        if (distance < maxAntDistance) return true;

        //if (distance >= maxAntDistance) 
        return false;
    }
    #endregion



    public void AddFirstAntToList(GameObject ant, int listIndex)    // Sets the first ant to the first slot in the list.
    {
        List<GameObject> theRightList;
        switch (listIndex)
        {
            case 0:
                theRightList = lineList1;
                break;
            case 1:
                theRightList = lineList2;
                break;
            case 2:
                theRightList = lineList3;
                break;
            case 3:
                theRightList = lineList4;
                break;
            case 4:
                theRightList = lineList5;
                break;
            default: theRightList = null; break;
        }
        if (!theRightList.Contains(ant) && !theRightList.Any())
        {
            theRightList[0] = ant;
        }

    }

    public void AddAntToList(GameObject ant, int listIndex)
    {
        List<GameObject> theRightList;
        switch (listIndex)
        {
            case 0:
                theRightList = lineList1;
                break;
            case 1:
                theRightList = lineList2;
                break;
            case 2:
                theRightList = lineList3;
                break;
            case 3:
                theRightList = lineList4;
                break;
            case 4:
                theRightList = lineList5;
                break;
            default: theRightList = null; break;
        }
        if (!theRightList.Contains(ant))
        {
            theRightList.Add(ant);
        }

    }



    #region signalling we can get food
    private void GrabFoodSignal()
    {
        List<GameObject> activeLine = new List<GameObject>();// Active LineList


        for (int i = 0; i < allLines.Count; i++)//Each Line
        {
            switch (i)
            {
                case 0: activeLine = lineList1; break;
                case 1: activeLine = lineList2; break;
                case 2: activeLine = lineList3; break;
                case 3: activeLine = lineList4; break;
                case 4: activeLine = lineList5; break;
            }
            foreach (GameObject ant in activeLine)//each ant in line
            {
                ThisAntHandler myHandler = ant.GetComponent<ThisAntHandler>();
                if (myHandler != null)
                {
                    if (myHandler.hasFood) myHandler.GiveFoodToPreviousInLine();
                    if (myHandler.hasFoodNearby) myHandler.GrabFood();
                }
            }
        }
    }



    #endregion

    #region Remove ants from list


    public void RemoveAntFromList(GameObject ant, int listIndex)
    {
        if (allLines[listIndex].Contains(ant))
        {
            lineList1.Remove(ant);
            ThisAntHandler thisAntHandler = ant.GetComponent<ThisAntHandler> ();
            thisAntHandler.DestroyLineData();
            thisAntHandler.SetInLine(false);
        }
    }


    public void RemoveAllTheUpcomingAntsFromList(GameObject ant, int index)
    {
        List<GameObject> activeLine = new List<GameObject>();
        switch (index)
        {
            case 0: activeLine = lineList1; break;
            case 1: activeLine = lineList2; break;
            case 2: activeLine = lineList3; break;
            case 3: activeLine = lineList4; break;
            case 4: activeLine = lineList5; break;
        }

        int indexOfThisAnt = activeLine.FindIndex(obj => obj == ant);
        if (indexOfThisAnt != -1)
        {
            for (int i = activeLine.Count - 1; i >= indexOfThisAnt; i--)
            {
                ThisAntHandler tah = activeLine[i].GetComponent<ThisAntHandler>();
                tah.DestroyLineData();
                tah.SetInLine(false);
                activeLine.RemoveAt(i);
            }
        }
    }

    #endregion
}
