using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LineController : MonoBehaviour
{
    private LineRenderer lr;
    [SerializeField] private LineRenderer lr2;
    [SerializeField] private LineRenderer lr3;
    [SerializeField] private LineRenderer lr4;
    [SerializeField] private LineRenderer lr5;


    private Transform[] points;
    AntManager antManager;

    private List<Transform> antLocations1 = new List<Transform>();
    private List<Transform> antLocations2 = new List<Transform>();
    private List<Transform> antLocations3 = new List<Transform>();
    private List<Transform> antLocations4 = new List<Transform>();
    private List<Transform> antLocations5 = new List<Transform>();


    //I need the ants individual ThisAntManager.RelevantPositionIndex and then connect the lines depending on their ?? maybe not important.

    //List - multiple lines.


    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        antManager = GameObject.Find("NewHole").GetComponent<AntManager>();
        InvokeRepeating("SetUpLineRenderer", 2f, 0.1f);


    }

    private void SetUpLineRenderer()
    {
        for (int i = 0; i < 4; i++)
        {

            Debug.Log("Setting up lineRenderer");
            ClearList(i);//Empties my list.
            GetDataFromManager(i);//gets the list data from the manager to draw the lines with.
            DrawLineOnAllAntsOfLine(i);//Updates the relevant linepositions.
        }
    }

    public void ClearList(int index)
    {
        List<Transform> activeList = new List<Transform>();
        switch (index)
        {
            case 0:
                activeList = antLocations1;
                break;
            case 1:
                activeList = antLocations2;
                break;
            case 2:
                activeList = antLocations3;
                break;
            case 3:
                activeList = antLocations4;
                break;
            case 4:
                activeList = antLocations5;
                break;
        }

        activeList.Clear();
    }

    private void GetDataFromManager(int index)
    {
        List<GameObject> objectList = new List<GameObject>();
        //Actions:
        //gets the right objectlist from the manager.
        //sets the linerenderer positioncount the same as the objectlist count.
        //if the right list does not contain the ant transform, it adds it to the list.
        switch (index)
        {
            case 0:
                objectList = antManager.lineList1;
                lr.positionCount = objectList.Count;
                foreach (GameObject obj in objectList)
                {
                    if (!antLocations1.Contains(obj.transform))
                    {
                        antLocations1.Add(obj.transform);
                    }
                }
                break;
            case 1:
                objectList = antManager.lineList2;
                lr2.positionCount = objectList.Count;
                foreach (GameObject obj in objectList)
                {
                    if (!antLocations2.Contains(obj.transform))
                    {
                        antLocations2.Add(obj.transform);
                    }
                }
                break;
            case 2:
                objectList = antManager.lineList3;
                lr3.positionCount = objectList.Count;
                foreach (GameObject obj in objectList)
                {
                    if (!antLocations3.Contains(obj.transform))
                    {
                        antLocations3.Add(obj.transform);
                    }
                }
                break;
            case 3:
                objectList = antManager.lineList4;
                lr4.positionCount = objectList.Count;
                foreach (GameObject obj in objectList)
                {
                    if (!antLocations4.Contains(obj.transform))
                    {
                        antLocations4.Add(obj.transform);
                    }
                }
                break;
            case 4:
                objectList = antManager.lineList5;
                lr5.positionCount = objectList.Count;
                foreach (GameObject obj in objectList)
                {
                    if (!antLocations5.Contains(obj.transform))
                    {
                        antLocations5.Add(obj.transform);
                    }
                }
                break;
        }

    }

    public void SetUpLine(List<Transform> transforms, int lineIndex)
    {
        List<Transform> activeList = new List<Transform>();
        switch (lineIndex)
        {
            case 0:
                activeList = antLocations1;
                break;
            case 1:
                activeList = antLocations2;
                break;
            case 2:
                activeList = antLocations3;
                break;
            case 3:
                activeList = antLocations4;
                break;
            case 4:
                activeList = antLocations5;
                break;
        }

        activeList = transforms;

    }


    private void DrawLineOnAllAntsOfLine(int lineIndex)
    {
        List<Transform> activeList = new List<Transform>();
        switch (lineIndex)
        {
            case 0:
                    activeList = antLocations1;
                if (activeList.Any())
                {
                    lr.positionCount = activeList.Count;
                    for (int i = 0; i < activeList.Count; i++)
                    {
                        lr.SetPosition(i, activeList[i].position);
                        Debug.Log("Set up position for index " + i);
                    }
                }
                break;
            case 1:
                    activeList = antLocations2;
                if (activeList.Any())
                {
                    lr2.positionCount = activeList.Count;
                    for (int i = 0; i < activeList.Count; i++)
                    {
                        lr2.SetPosition(i, activeList[i].position);
                        Debug.Log("Set up position for index " + i);
                    }
                }
                break;
            case 2:
                activeList = antLocations3;
                if (activeList.Any())
                {
                    lr3.positionCount = activeList.Count;
                    for (int i = 0; i < activeList.Count; i++)
                    {
                        lr3.SetPosition(i, activeList[i].position);
                        Debug.Log("Set up position for index " + i);
                    }
                }
                break;
            case 3:
                activeList = antLocations4;
                if (activeList.Any())
                {
                    lr4.positionCount = activeList.Count;
                    for (int i = 0; i < activeList.Count; i++)
                    {
                        lr4.SetPosition(i, activeList[i].position);
                        Debug.Log("Set up position for index " + i);
                    }
                }
                break;
            case 4:
                activeList = antLocations5;
                if (activeList.Any())
                {
                    lr5.positionCount = activeList.Count;
                    for (int i = 0; i < activeList.Count; i++)
                    {
                        lr5.SetPosition(i, activeList[i].position);
                        Debug.Log("Set up position for index " + i);
                    }
                }
                break;
        }


        //Debug.Log("Drew lines for " + activeList.Count + " Ants.");

    }
}
