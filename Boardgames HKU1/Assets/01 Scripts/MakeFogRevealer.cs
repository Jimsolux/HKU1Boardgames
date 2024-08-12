using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using FischlWorks_FogWar;

public class MakeFogRevealer : MonoBehaviour
{
    private void Awake()
    {
        csFogWar fogwarReference = GameObject.Find("FogWar").GetComponent<csFogWar>();
        fogwarReference.AddFogRevealer(new csFogWar.FogRevealer(gameObject.transform, 3, false));
    }
}

