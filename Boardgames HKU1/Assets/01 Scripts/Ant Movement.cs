using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AntMovement : MonoBehaviour
{
    public GameObject selectedAnt;
    //public List<GameObject> selectedAnts;

    public NavMeshAgent antAgent;
    public List<NavMeshAgent> antAgents;
    [SerializeField] private LayerMask creatureMask;

    public Camera mainCam;

    //Colors
    ChangeSelectColour changeSelectColor = null;
    //Reference for colorfeedback
    ThisAntHandler myHandler;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ControlAnts();

        }
    }


    #region control the ants and shit
    private void ControlAnts()
    {
        if (selectedAnt == null || selectedAnt)// if no selected ant
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);//raycast 
            if (Physics.Raycast(ray, out RaycastHit hit, 100, creatureMask))
            {
                if (hit.collider.gameObject.tag == "Ant" || hit.collider.gameObject.tag == "OtherAnt")   //if ant, get the ant
                {
                    if (selectedAnt)//if i already have an ant, deselect it.
                    {
                        DeselectAnts();
                    }
                    GetAnt(hit.collider.gameObject);
                    //Set ant to selected
                    changeSelectColor.ChangeToSelectedColor();
                    //selectedAnt = hit.transform.gameObject; // Outdated, now in GetAnt function.
                    //Debug.Log("Ant Selected.");
                    return;
                }
                if (hit.collider.gameObject.tag == "Map")
                {
                    DeselectAnts(); 
                    //Debug.Log("Deselected Ant because ray saw " + hit.collider.gameObject.name);
                }
                if (hit.collider.gameObject.tag == "AreaChecker")
                {
                    if (selectedAnt)//if i already have an ant, deselect it.
                    {
                        DeselectAnts();
                    }
                    GetAnt(hit.transform.parent.gameObject);
                }
            }
        }

        if (selectedAnt != null)    // There is ant selected
        {
            MoveAnts();
        }
    }

    private void DeselectAnts()
    {
        //Change to unselected colour.
        changeSelectColor.ChangeToDefaultColor();

        selectedAnt = null;
        changeSelectColor = null;

        ClearAntAgent();
    }

    private void GetAnt(GameObject hit)
    {
        selectedAnt = hit;
        GetAntAgent();
        //Colors
        changeSelectColor = GetColorsOfAnt(hit.gameObject); //Ant color taken.
    }

    private void GetAntAgent()
    {
        if (selectedAnt != null)
        {
            antAgent = selectedAnt.GetComponent<NavMeshAgent>();
        }
    }

    private void ClearAntAgent()
    {
        if (antAgent != null) antAgent = null;
    }


    private void MoveAnts()
    {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100))
        {
            //antAgent.ResetPath();
            antAgent.destination = hit.point;
            //DeselectAnts();
        }
    }
    #endregion

    #region colorFeedback

    private ChangeSelectColour GetColorsOfAnt(GameObject ant)
    {
        ChangeSelectColour changeSelectColour = ant.GetComponentInChildren<ChangeSelectColour>();
        if (changeSelectColour != null)
        {
            return changeSelectColour;
        }
        else return null;
    }

    #endregion





}
