using UnityEngine;

public class DangerBlock : MonoBehaviour
{
    [SerializeField] LineController lineController;
    [SerializeField] AudioSource audioSource;

    private void Awake()
    {
        lineController = GameObject.Find("LineDrawer").GetComponent<LineController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ant" || other.gameObject.tag == "OtherAnt")
        {
            Debug.Log("Ant in collision!");
            GameObject caughtAnt = other.gameObject;
            ThisAntHandler caughtAntHandler = caughtAnt.GetComponent<ThisAntHandler>();
            if (caughtAntHandler.myLineIndex != -1)
            {
                caughtAntHandler.activeAntManager.EmptyAntLine(caughtAntHandler.myLineIndex);//Empty the current Ant line the ant is in.
                lineController.ClearList(caughtAntHandler.myLineIndex);//Clear the Line controllers list of this ant.

            }
            other.gameObject.SetActive(false);
            if (audioSource != null)
            {
                //play sfx
                audioSource.Play();
            }

        }

    }
}
