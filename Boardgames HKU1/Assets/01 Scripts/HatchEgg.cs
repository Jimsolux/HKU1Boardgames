using System.Collections;
using UnityEngine;

public class HatchEgg : MonoBehaviour
{
    private float hatchTime;


    public void StartHatch(float time, GameObject antObj)
    {
        hatchTime = time;
        StartCoroutine(HatchTimer(antObj));
    }

    private IEnumerator HatchTimer(GameObject antObj)
    {
        yield return new WaitForSeconds(hatchTime);
        //Do the hatching
        GameObject newAnt = Instantiate(antObj, transform.position, Quaternion.identity);
        Destroy(this.gameObject);

        //yield return new WaitForSeconds(hatchTime);

    }

    //GameObject newAnt = Instantiate(collectorAnt, loc, Quaternion.identity);

}

