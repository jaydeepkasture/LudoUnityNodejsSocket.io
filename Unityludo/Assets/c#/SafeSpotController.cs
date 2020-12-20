using System;
using UnityEngine;


class SafeSpotController : MonoBehaviour
{
    [HideInInspector] public GameObject[] safeSpot;
    [HideInInspector] public GameObject currentlyActiveSafeSpotGrid;
    public static SafeSpotController instance;

    private void Start()
    {
        instance = this;
    }


}

