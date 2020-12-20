using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlinePlayersProfileManager : MonoBehaviour
{
    [SerializeField] private Player[] players;

    public void SetPlayersProfile(PawnType pawn,string profilePic,string playerId)
    {
        foreach (var player in players)
        {
            if (player.playerPawn == pawn)
            {
                player.SetProfile(profilePic, playerId);
            }
        }
    }
}
