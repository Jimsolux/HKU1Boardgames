using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThisAntHandler : MonoBehaviour
{
    //References
    Resources resources;
    [SerializeField] GameObject GameManagerObject;

    //LineData
    //public List<GameObject> myLine = new List<GameObject>();
    public bool inLine;
    public int myLineIndex = -1;
    public AntManager activeAntManager;
    private float maxDistance = 1;

    //Connection to hub
    public int relevantPositionIndex = 0; //This is the relative position to the connection of the line. Not the actual index.
    public GameObject antThatPutMeInLine = null;

    //Reference to Colourfeedback
    ChangeSelectColour colorthing;

    private void Start()
    {
        GameManagerObject = GameObject.Find("GameManagerObject");
        resources = GameManagerObject.GetComponent<Resources>();
        DisplayFoodIcon();
    }

    #region receiveDataFromManager
    public void ReceiveLineData(AntManager manager, int relevantIndex, GameObject parentAnt)  // For this ant, the active hole = the received manager.
    {
        activeAntManager = manager;
        inLine = true;
        maxDistance = activeAntManager.maxAntDistance;
        relevantPositionIndex = relevantIndex;
        antThatPutMeInLine = parentAnt;
        Debug.Log("NEW LINEDATA RECEIVED");
        colorthing = GetComponentInChildren<ChangeSelectColour>();
        colorthing.ChangeInLineColor(true);
    }
    public void ReceiveLineIndex(int index)
    {
        myLineIndex = index;
    }

    public void DestroyLineData()
    {
        activeAntManager = null;
        //myLine = null;
        inLine = false;
        myLineIndex = -1;
        relevantPositionIndex = -1;
        antThatPutMeInLine = null;
        Debug.Log("OLD LINEDATA REMOVED");
        colorthing = GetComponentInChildren<ChangeSelectColour>();
        colorthing.ChangeInLineColor(false);
    }

    public void SetInLine(bool value)
    {
        inLine = value;
    }
    #endregion

    #region Add and remove this ant from line
    public void AddAntToList(GameObject ant, int lineIndex)
    {
        if (activeAntManager != null)
        {
            activeAntManager.AddAntToList(ant.gameObject, lineIndex);
        }
    }
    public void RemoveAntFromList(GameObject ant)
    {
        if (activeAntManager != null)
        {
            activeAntManager.RemoveAntFromList(ant.gameObject, myLineIndex);
        }
    }
    #endregion

    #region Distance Checking
    public void CheckAntDistance(int listIndex)
    {
        //Debug.Log(this.gameObject.name + " is searching for other ants!");
        //When Triggered, should:
        //Check what my own line is.
        //For each ant in the scene, check your distance from them.
        //If an ant is closerthan the maxDistance, Add them to the list, give them your data, and


        if (activeAntManager != null)
        {
            List<GameObject> activeLine = new List<GameObject>();// Active LineList
            switch (listIndex)
            {
                case 0: activeLine = activeAntManager.lineList1; break;
                case 1: activeLine = activeAntManager.lineList2; break;
                case 2: activeLine = activeAntManager.lineList3; break;
                case 3: activeLine = activeAntManager.lineList4; break;
                case 4: activeLine = activeAntManager.lineList5; break;
            }

            foreach (GameObject ant in activeAntManager.antsInScene)
            {
                if (ant != this.gameObject)
                {
                    float distance = Vector3.Distance(transform.position, ant.transform.position);

                    if (distance < maxDistance && !activeLine.Contains(ant))    // Other ant not yet in list
                    {
                        //Debug.Log("Ant recognized as being close enough to add to line!");
                        ThisAntHandler otherAntHandler = ant.GetComponent<ThisAntHandler>();
                        otherAntHandler.AddAntToList(ant, listIndex);
                        otherAntHandler.ReceiveLineData(activeAntManager, relevantPositionIndex + 1, this.gameObject);
                        otherAntHandler.ReceiveLineIndex(listIndex);
                        //Debug.Log("Found Distance is " + distance);
                    }

                    else if (distance >= maxDistance && activeLine.Contains(ant))// Ant in list too far away
                    {
                        //Check if that ant is next index in my line, and then delete it if its too far.

                        ThisAntHandler otherAntHandler = ant.GetComponent<ThisAntHandler>();
                        if (otherAntHandler.relevantPositionIndex == relevantPositionIndex + 1)// if next in line...
                        {
                            if (otherAntHandler.antThatPutMeInLine == this.gameObject)
                            {
                                activeAntManager.RemoveAllTheUpcomingAntsFromList(ant, listIndex);
                                Debug.Log("The Ant After me is too far from me! Deleted!");
                            }
                        }
                        //check if all the ants are too far away.
                        if (activeAntManager.CheckIfAllAntsAreTooFarAway(myLineIndex, ant))
                        {
                            otherAntHandler.DestroyLineData();
                            //remove all ants from list with this index or higher.
                            activeAntManager.RemoveAllTheUpcomingAntsFromList(ant, listIndex);
                            //activeAntManager.EmptyAntLine(listIndex);
                            //Debug.Log("AN ANT IS TOO FAR FROM ALL ANTSS ");
                        }
                    }
                }
            }
        }
    }

    #endregion



    #region foodstuff
    //Food vars
    public bool hasFoodNearby = false;
    public bool hasFood;
    //Food references
    public FoodContainer activeContainer = null;
    [SerializeField] private GameObject foodIcon;
    public void GrabFood()
    {
        Debug.Log("I am tasked to grab food.");
        if (hasFoodNearby && activeContainer != null)   
        {
            Debug.Log("I have a foodcontainer nearby.");

            if (activeContainer.foodAmount >= 1)
            {
                activeContainer.foodAmount--;
                hasFood = true;
                DisplayFoodIcon();
                Debug.Log("taken food.");

            }
            else if (activeContainer.foodAmount <= 0)   // if food runs out, remove food objec.
            {
                activeContainer.gameObject.SetActive(false);
            }
            
        }


    }

    public void GetFoodFromOtherAnt(bool value)
    {
        hasFood = value;
        DisplayFoodIcon();
    }

    public void GiveFoodToPreviousInLine()
    {
        if(hasFood && antThatPutMeInLine != null)
        {
            if(antThatPutMeInLine.tag == "Hole")
            {
                Resources resources = antThatPutMeInLine.GetComponent<Resources>();
                resources.GainFood(1);
                hasFood = false;
                DisplayFoodIcon(); 
                return;
            }
            //Give the food to the previous Ant in line.
            ThisAntHandler otherAntHandler = antThatPutMeInLine.GetComponent<ThisAntHandler>();
            otherAntHandler.GetFoodFromOtherAnt(true);
            otherAntHandler.DisplayFoodIcon();
            //Reset my current food
            hasFood = false;
            DisplayFoodIcon();
        }
    }

    public void FoodTakenAway()
    {
        hasFood = false;
    }


    private void DisplayFoodIcon()
    {
        if (hasFood) foodIcon.SetActive(true);

        else if (!hasFood) foodIcon.SetActive(false);
    }
    #endregion

}

