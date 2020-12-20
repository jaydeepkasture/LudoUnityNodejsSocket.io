
using System;
using System.Collections.Generic;

public class TwoPlayers : OnlinePlayers
{
    public static TwoPlayers instance;

    private void Awake()
    {
        instance = this;
    }
    public override void PawnTypeAssignerToPlayerId(List<string> playersId, Dictionary<string, string> profiles)
    {
        foreach (var id in playersId)
        {
            if (id == LocalPlayer.playerId)
            {
                TempOnlinePlayersData.instance.AddPlayer(id, PlayerInfo.instance.selectedPawn);
                onlinePlayersProfileManager.SetPlayersProfile(PlayerInfo.instance.selectedPawn, profiles[id], id);
            }
            else
            {
                PawnType opponentPawnType = (PawnType)GetOpponentPawnColour();
                TempOnlinePlayersData.instance.AddPlayer(id, opponentPawnType);
                onlinePlayersProfileManager.SetPlayersProfile(opponentPawnType, profiles[id], id);

            }
        }
    }


    private int GetOpponentPawnColour()
    {
        int[] pawns = { 1, 2, 3, 4 };
        for (int i = 0; i < pawns.Length; i++)
        {
            bool isSameColour = pawns[i] == (int)PlayerInfo.instance.selectedPawn;
            if (isSameColour)
            {
                int opponentPawnColour = 2 + pawns[i] <= pawns.Length ? pawns[2 + i] : pawns[Math.Abs(pawns.Length - (2 + i))];
                return opponentPawnColour;
            }
        }
        return 0;
    }
}
