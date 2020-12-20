using System;
using UnityEngine;
class SpotsManager : MonoBehaviour
{
    public GameObject[] spots;

    public static SpotsManager instance;

    private void Awake()
    {
        instance = this;
        int spotNumber = 1;
        foreach (var spot in spots)
        {
            spot.AddComponent<Spot>();
            spot.GetComponent<Spot>().spotNo = spotNumber;
            spotNumber++;
        }
    }
}

