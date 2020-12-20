using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft;
using Newtonsoft.Json;
using System;

public class ServerResponse : MonoBehaviour
{
    public static SocketIOComponent socket;

    public GameObject socketIo;
    [SerializeField]private YouWinPanel _youWinPanel;

    void Awake()
    {
        socket = socketIo.GetComponent<SocketIOComponent>();
        socket.On("open", OnConnected);
        socket.On("disconnected", OnDisconnected);
        socket.On("test", Test);

        socket.On(ServerReponseApi.START_GAME.ToString(), StartGame);
        socket.On(ServerReponseApi.ROLL_DICE.ToString(), RollDice);
        socket.On(ServerReponseApi.ON_PLAYER_WIN.ToString(), OnPlayerWin);
        socket.On(ServerReponseApi.YOU_WIN.ToString(), YouWin);
        socket.On(ServerReponseApi.PLAYER_REGISTRATION.ToString(), RegisterPlayerId);
        socket.On(ServerReponseApi.SWITCH_PLAYER.ToString(), SwitchPlayers);
        socket.On(ServerReponseApi.PLAYER_FINISHED_MOVING.ToString(),PlayerFinishedMoving);
        socket.On(ServerReponseApi.AVOID_SWITCH_PLAYER.ToString(), AvoidSwitchingPlayer);
        socket.On(ServerReponseApi.PLAYER_MOVED.ToString(), PlayerMoved);
        socket.On(ServerReponseApi.ONLINE_PLAYERS.ToString(), OnlinePlayers);
        socket.On(ServerReponseApi.EXIT_ROOM.ToString(), ExitRoom);
        socket.On(ServerReponseApi.GET_PLAYERS_INFO.ToString(), GetPlayerInfo);
    }

    private void GetPlayerInfo(SocketIOEvent obj)
    {
        try
        {

        List<string> videogames = JsonConvert.DeserializeObject<List<string>>(obj.data["array"].ToString());

        print(obj.data["array"].ToString());
        }
        catch(Exception e)
        {
            print(e);
        }
    }

    void OnConnected(SocketIOEvent e)
    {
        print("connected");
#if UNITY_ANDROID
        Toast.instance.Massage("started");

#endif
    }
    private void StartGame(SocketIOEvent e)
    {
        print(LocalPlayer.playerId);
        try
        {
            List<string> allPlayersId = JsonConvert.DeserializeObject<List<string>>(e.data["allPlayersId"].ToString());
            Dictionary<string, string> profiles = JsonConvert.DeserializeObject<Dictionary<string,string>>(e.data["allProfiles"].ToString());
            AnalyseAndRegisterOnlinePlayers.instance.AnaliysisAndRegistration(allPlayersId,profiles);
            ServerRequest.instance.OnGameStarted();
            print("game started");
        }
        catch (Exception error)
        {
            print("exception " + error);
        }
    }

    void SwitchPlayers(SocketIOEvent e)
    {
        //PawnType currentPawn = (PawnType)int.Parse(e.data["nextPlayerId"].ToString());
        PawnType currentPawn = TempOnlinePlayersData.instance.GetPlayerPawnType(JsonConvert.DeserializeObject<string>(e.data["nextPlayerId"].ToString()));
        print("#switchpawns "+currentPawn);
        int diceValue = int.Parse(e.data["diceValue"].ToString());

        if (PlayerInfo.instance.selectedPawn == currentPawn)
            DiceController.instance.playerMovementIsFinished = true;

        StartCoroutine(DiceController.instance.RollDice(diceValue));
        DiceController.instance.currentPawn = currentPawn;
        DiceController.instance.UpdateValue();
        StartCoroutine(PawnTimer.instance.Timer(currentPawn));
    }
    void RollDice(SocketIOEvent e)
    {
        int diceValue = JsonConvert.DeserializeObject<int>(e.data["diceValue"].ToString());
        print("#RollDice " + diceValue);
        StartCoroutine(DiceController.instance.RollDice(diceValue));
    }
    void OnPlayerWin(SocketIOEvent e)
    {
        string  winnerPawn = (JsonConvert.DeserializeObject<string>(e.data["PlayerId"].ToString()));
        PawnType winnerPawnType = TempOnlinePlayersData.instance.GetPlayerPawnType(winnerPawn);
        SetWinner.instance.OnPlayerWin(winnerPawnType);
        TempOnlinePlayersData.instance.RemovePlayer(winnerPawn);

    }


    void Test(SocketIOEvent e)
    {
        print("test from server ");
    }
    void AvoidSwitchingPlayer(SocketIOEvent e)
    {
        int diceValue = int.Parse(e.data["diceValue"].ToString());
        StartCoroutine(DiceController.instance.RollDice(diceValue));
    }

    void PlayerFinishedMoving(SocketIOEvent e)
    {
        PawnType currentPawn = TempOnlinePlayersData.instance.
            GetPlayerPawnType(JsonConvert.DeserializeObject<string>(e.data["nextPlayerId"].ToString()));
        print("#PawnFinishedMoving " + currentPawn);
        int diceValue = int.Parse(e.data["diceValue"].ToString());

        if (PlayerInfo.instance.selectedPawn == currentPawn)
            DiceController.instance.playerMovementIsFinished = true;

        DiceController.instance.currentPawn = currentPawn;
        DiceController.instance.UpdateValue();
        StartCoroutine(PawnTimer.instance.Timer(currentPawn));
    }

 

    private void RegisterPlayerId(SocketIOEvent e)
    {
        LocalPlayer.playerId = JsonConvert.DeserializeObject < string> ( e.data["id"].ToString());
        UiManager.instance.UpdateUi();
        LocalPlayer.SaveGame();
        print("playerId"+ LocalPlayer.playerId);
    }



    private void ExitRoom(SocketIOEvent e)
    {
        try
        {

            print("exit room triger");
            string playerId = JsonConvert.DeserializeObject<string>(e.data["playerId"].ToString());
            PawnType exitPawn = TempOnlinePlayersData.instance.GetPlayerPawnType(playerId);
            print("exit room triger by :"+playerId+"and pawntype:"+exitPawn);
            PlayerInfo.instance.RemovePawn(exitPawn);
            UiManager.instance.ExitPanel(exitPawn);
            print($"remove player {playerId} of pawntype {exitPawn}");
            TempOnlinePlayersData.instance.RemovePlayer(playerId);

        }
        catch (Exception error)
        {
            print(error.Message);
            throw;
        }   
    }

    private void YouWin(SocketIOEvent e)
    {
        try
        {

            string playerId = JsonConvert.DeserializeObject<string>(e.data["playerId"].ToString());
            PawnType exitPawn = TempOnlinePlayersData.instance.GetPlayerPawnType(playerId);
            _youWinPanel.OnPlayerWin(exitPawn);
            PlayerInfo.instance.RemovePawn(exitPawn);
            UiManager.instance.ExitPanel(exitPawn);
            print($"remove player {playerId} of pawntype {exitPawn}");
            TempOnlinePlayersData.instance.RemovePlayer(playerId);
        }
        catch (Exception error)
        {

            print(error.Message);
        }
    }
    private void OnDisconnected(SocketIOEvent e)
    {
        Debug.Log("Client disconnected: " + e.data);
        
    }

    private void PlayerMoved(SocketIOEvent e)
    {
        int diceValue = int.Parse(e.data["diceValue"].ToString());
        int pawnNo = int.Parse(e.data["pawnNo"].ToString());
        PawnType pawnType = TempOnlinePlayersData.instance.GetPlayerPawnType(JsonConvert.DeserializeObject<string>(e.data["playerId"].ToString()));
        print($"pawm movement dicevalue:{diceValue}  pawnNo:{pawnNo}");
        FindAndMoveThePawn(diceValue, pawnNo, pawnType);
    }


    private void FindAndMoveThePawn(int diceValue, int pawnNo, PawnType pawnType)
    {
        foreach (var pawn in PlayerInfo.instance.pawnInstances)
        {
            bool notSamePawn = pawn.pawnType != pawnType;
            if (notSamePawn)
                continue;
            if (pawn.pawnNumber == pawnNo)
            {
                StartCoroutine(pawn.MoveTo(diceValue,true));
            }
        }
    }

    private void OnlinePlayers(SocketIOEvent e)
    {
        int players = int.Parse(e.data["onlinePlayers"].ToString());
        UiManager.instance.onlinePlayers.text = players.ToString();

    }
}

