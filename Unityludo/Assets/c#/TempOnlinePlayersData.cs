using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempOnlinePlayersData : MonoBehaviour
{
    private Dictionary<string, PawnType> _players = new Dictionary<string, PawnType>();
    public static TempOnlinePlayersData instance;
    private void Awake()
    {
        instance=this;
    }
    public void AddPlayer(string playerId, PawnType pawnType)=>_players.Add(playerId, pawnType);
    public void RemovePlayer(string playerId) 
    {
        _players.Remove(playerId);
    }
    public void RemoveAllPlayers() => _players.Clear();
    public PawnType GetPlayerPawnType(string playerId) => _players[playerId];

}
