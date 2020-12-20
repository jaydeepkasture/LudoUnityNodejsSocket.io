using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    public List<PawnMovementController> pawnInstances=new List<PawnMovementController>();
    public GameObject[] safeSpots;
    public GameObject pawnPanel;
    public int players;
    public static PlayerInfo instance;
    public PawnType selectedPawn;


    private void Awake()
    {
        instance = this;
    }

    public void RemoveAllPawns()
    {
        pawnInstances.Clear();
        foreach (Transform pawn in pawnPanel.transform)
        {
            Destroy(pawn.gameObject);
        }
    }
    public void RemovePawn(PawnType removePawn)
    {
        //remove from list and game
        foreach (var pawn in pawnInstances)
        {
            if (pawn.pawnType == removePawn)
            {
                Destroy(pawn.gameObject);
            }
        }
        pawnInstances.RemoveAll(item => item == null);
        print("player left" + pawnInstances.Count);
    }
}
