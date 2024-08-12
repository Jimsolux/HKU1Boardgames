using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAnts : MonoBehaviour
{

    //Vars
    private int spawnedAnts = 0;

    //Reference to resources
    private Resources resources;

    private void Awake()
    {
        resources = GetComponent<Resources>();
    }

    private void Update()
    {
        PlayerInput();
    }

    private void PlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))//Collector
        {
            CreateNewEgg(antTypeEnum.collector);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))//Warrior
        {
            CreateNewEgg(antTypeEnum.warrior);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))//Flying
        {
            CreateNewEgg(antTypeEnum.scout);
        }
    }


    #region create new ants

    [SerializeField] private GameObject antEgg;
    //Ants
    public enum antTypeEnum { collector, warrior, scout }

    [SerializeField] private GameObject collectorAnt;
    [SerializeField] private GameObject warriorAnt;
    [SerializeField] private GameObject scoutAnt;
    //Costs
    [SerializeField] private int collectorAntCost;
    [SerializeField] private int warriorAntCost;
    [SerializeField] private int scoutAntCost;

    //Timers
    [SerializeField] private float hatchTime = 10;

    private void CreateNewEgg(antTypeEnum typeAnt)
    {
        int cost = 0;
        GameObject theAnt = null;
        switch (typeAnt)
        {
            case antTypeEnum.collector:
                theAnt = collectorAnt;
                //Debug.Log("TheAnt has been set.");
                cost = 3;
                break;
            case antTypeEnum.warrior:
                theAnt = warriorAnt;
                //Debug.Log("TheAnt has been set.");
                cost = 5;
                break;
            case antTypeEnum.scout:
                theAnt = scoutAnt;
                //Debug.Log("TheAnt has been set.");
                cost = 5;
                break;
        }
        //Debug.Log("My type is " + typeAnt.ToString() + " and my cost is " + cost);
        if (resources.food >= cost && theAnt != null)
        {
            StartHatchEgg(theAnt);
            resources.SpendFood(cost);

        }
        else
        {
            Debug.Log("Not spawned egg because my cost is " + cost + " , The food is " + resources.food + " , and theAnt = " + theAnt.name) ;
        }
    }

    private void StartHatchEgg(GameObject ant)
    {
        //location
        Vector3 randomOffSet = GetRandomOffset(3);

        GameObject nextEgg = Instantiate(antEgg, transform.position + randomOffSet, Quaternion.identity);
        Debug.Log("Spawned Egg");
        HatchEgg eggScript = nextEgg.GetComponent<HatchEgg>();
        eggScript.StartHatch(hatchTime, ant);
        spawnedAnts++;

    }

    Vector3 GetRandomOffset(float range)
    {
        float offsetX = Random.Range(-range, range);
        float offsetY = .2f;
        float offsetZ = Random.Range(-range, range);

        return new Vector3(offsetX, offsetY, offsetZ);
    }

    #endregion
}
