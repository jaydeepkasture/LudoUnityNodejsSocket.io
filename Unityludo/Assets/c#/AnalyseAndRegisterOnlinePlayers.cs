
using System;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

class AnalyseAndRegisterOnlinePlayers : MonoBehaviour
{

    public static AnalyseAndRegisterOnlinePlayers instance;
    private void Awake()
    {
        instance = this;
    }

    public void AnaliysisAndRegistration(List<string> playersId, Dictionary<string, string> profiles)
    {

        foreach (var item in playersId)
        {
            print(item);
        }
        int thisPlayerIdInTheListIndex = 0;
        if (playersId.Contains(LocalPlayer.playerId))
        {
            thisPlayerIdInTheListIndex = playersId.FindIndex(x => x == LocalPlayer.playerId);
        }
        else
        {
            print("player id not in the list");
            print("something went wrong try again");
            UiManager.instance.ResetLevel();
            return;
        }

        Registertion(thisPlayerIdInTheListIndex, playersId,profiles);
    }

   
    private void Registertion(int thisPlayerListIndex,List<string> playersId, Dictionary<string, string> profiles)
    {
        switch (PlayerInfo.instance.players)
        {
            case 2:
                TwoPlayers.instance.PawnTypeAssignerToPlayerId(playersId,profiles);
                break;

            case 3:
                break;
            case 4:
                FourPlayers.instance.PawnTypeAssignerToPlayerId(playersId,profiles);
                break;
            default:
                break;
        }

        StartGame(playersId);

    }
  
    private void StartGame(List<string> playersId)
    {
        UiManager.instance.uiPanel.SetActive(false);
        PawnType currentPawn = TempOnlinePlayersData.instance.GetPlayerPawnType(playersId[0]);
        DiceController.instance.currentPawn = currentPawn;
        StartCoroutine(PawnTimer.instance.Timer(currentPawn));
        DiceController.instance.UpdateValue();
        UiManager.instance.uiPanel.SetActive(false);
    }
}

