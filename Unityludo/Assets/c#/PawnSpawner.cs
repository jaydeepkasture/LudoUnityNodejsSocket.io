using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PawnSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _parentPanel;
    public static PawnSpawner instance;

    public GameObject spotPanel { get { return _parentPanel; } }
    private void Awake()
    {
        instance = this;
    }
    
    [Serializable]
    public struct Spawnhelper
    {
        public GameObject pawnPrefab;
        public GameObject[] home;
        public PawnType color;
    }

    public Spawnhelper[] spawnHelper;

    public void ArrangePawns(PawnType pawnType)
    {
        int selectedPawnIndex = (int)pawnType;
        switch (PlayerInfo.instance.players)
        {
            case 2:
                int opponetPawnColor = GetOpponentPawnColour();
                foreach (var pawn in spawnHelper)
                {
                    if (pawn.color == (PawnType)opponetPawnColor)
                    {
                        SpawnPawns(pawn.home, pawn.pawnPrefab, pawn.color);
                        
                    }

                    if (pawn.color == PlayerInfo.instance.selectedPawn)
                    {
                        SpawnPawns(pawn.home, pawn.pawnPrefab, pawn.color);
                       
                    }
                }
                break;

            case 3:
                foreach (var item in spawnHelper)
                {
                    int skipPawn = selectedPawnIndex != 1 ? selectedPawnIndex - 1 : 4;
                    if (skipPawn == (int)item.color)
                        continue;
                    SpawnPawns(item.home, item.pawnPrefab, item.color);
                }
                break;

            case 4:
                foreach (var item in spawnHelper)
                {
                    SpawnPawns(item.home, item.pawnPrefab, item.color);
                }
                break;

            default:
                break;
        }
        RearrangePawns.instance.Rearrange();

    }

    private int GetOpponentPawnColour()
    {
        int[] pawns = { 1, 2, 3, 4 };
        for (int i = 0; i < pawns.Length; i++)
        {
            bool isSameColour = pawns[i] == (int)PlayerInfo.instance.selectedPawn;
            if (isSameColour)
            {
                int opponentPawnColour = 2 + pawns[i] <= pawns.Length ? pawns[2 +i] : pawns[Math.Abs(pawns.Length - (2 + i))];
                return opponentPawnColour;
            }
        }
        return 0;
    }

    private void SpawnPawns(GameObject[] homeSpots,GameObject playerPrefab,PawnType pawnType)
    {

        int index = 0;
        foreach (var home in homeSpots)
        {
            index++;
            var player = Instantiate(playerPrefab);
            player.GetComponent<PawnMovementController>().home = home;
            player.GetComponent<PawnMovementController>().pawnNumber = index;
            if (PlayerInfo.instance.selectedPawn != pawnType)
            {
                player.GetComponent<Button>().enabled = false;
            }
            player.SetActive(true);
            PlayerInfo.instance.pawnInstances.Add(player.GetComponent<PawnMovementController>());
            player.transform.position = home.transform.position;
            player.transform.localScale = new Vector3(1f, 1f,1f);
            player.transform.SetParent( _parentPanel.transform);
        }
    }
}
