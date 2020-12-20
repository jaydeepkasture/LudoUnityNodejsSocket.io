
using System.Collections.Generic;
using UnityEngine;

public abstract class OnlinePlayers : MonoBehaviour
{

    protected OnlinePlayersProfileManager onlinePlayersProfileManager;
    private void Start()
    {
        onlinePlayersProfileManager = GetComponent<OnlinePlayersProfileManager>();
    }
    public abstract void  PawnTypeAssignerToPlayerId(List<string> playersId, Dictionary<string, string> profiles);
}
