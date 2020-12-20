using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine.SocialPlatforms;

class ServerRequest : MonoBehaviour
{
    public static ServerRequest instance;
    public bool serverConnection = false;

    private void Awake()
    {
        instance = this;
    }

    public void OnGameStarted()
    {
        if (serverConnection)
            return;

        object playerId = new { LocalPlayer.playerId };
        ServerResponse.socket.Emit(ServerRequestApi.ON_GAME_STARTED.ToString(),
            new JSONObject(JsonConvert.SerializeObject(playerId)));

    }

    public void RollDice(int diceValue, PawnType pawn)
    {
        if (serverConnection)
            return;

        object dice = new { diceValue, pawn };


        if (diceValue == 6)
        {
            ServerResponse.socket.Emit(ServerRequestApi.ROLL_DICE.ToString(), new JSONObject(JsonConvert.SerializeObject(dice)));
        }
        else if (AvoidSwitchingPlayers(diceValue, pawn))
        {
            dice = new { diceValue, pawn, PlayerInfo.instance.players };
            ServerResponse.socket.Emit(ServerRequestApi.ROLL_DICE.ToString(),
                new JSONObject(JsonConvert.SerializeObject(dice)));
        }
        else
        {
            SwitchPlayers(diceValue);
        }
    }

    private void SwitchPlayers(int diceValue)
    {
        if (serverConnection)
            return;
        object dice = new { diceValue, LocalPlayer.playerId, PlayerInfo.instance.players };
        ServerResponse.socket.Emit(ServerRequestApi.SWITCH_PLAYER.ToString(),
            new JSONObject(JsonConvert.SerializeObject(dice)));
    }
    private bool AvoidSwitchingPlayers(int diceValue,PawnType currentPawn)
    {
        bool switchPawns = false;

        foreach (var Player in PlayerInfo.instance.pawnInstances)
        {
            
            if (Player.pawnType != currentPawn)
            {
                continue;
            }

            if (Player.isLeftTheHouse)
            {
                bool canMoveAhead = Player.spotIndexOnLudoBoard + diceValue <= Player.Lastspot;
                if (canMoveAhead)
                {
                    switchPawns = false;
                    break;
                }
            }
            else
            {
                switchPawns = true;
            }
        }
        return !switchPawns;
    }
    public void PlayerFinishedMoving(bool richedTheDestination)
    {
        if (serverConnection)
            return;
        int diceValue = DiceController.instance.currentDiceValue;
        object pawndetails = new {  diceValue ,LocalPlayer.playerId , richedTheDestination };
        ServerResponse.socket.Emit(ServerRequestApi.PLAYER_FINISHED_MOVING.ToString(),
            new JSONObject(JsonConvert.SerializeObject(pawndetails)));
    }

    public void MatchMaking(int players, PawnType pawnType)
    {
        if (serverConnection)
            return;
        object generalInfo = new { players, LocalPlayer.playerId,LocalPlayer.profilePic };
        ServerResponse.socket.Emit(ServerRequestApi.MATCH_MAKING.ToString(),
            new JSONObject(JsonConvert.SerializeObject(generalInfo)), (data) => { Debug.Log(data); });
    }

    public void ExitRoom()
    {
        if (serverConnection)
            return;
        object generalInfo = new { LocalPlayer.playerId};
        ServerResponse.socket.Emit(ServerRequestApi.EXIT_ROOM.ToString(),
            new JSONObject(JsonConvert.SerializeObject(generalInfo)), (data) => { Debug.Log(data); });
    }
    
    public void QuitGame()
    {
        if (serverConnection)
            return;
        var quit = new { quit = true, PlayerInfo.instance.players, LocalPlayer.playerId };
        ServerResponse.socket.Emit(ServerRequestApi.ON_QUIT.ToString(), new JSONObject(JsonConvert.SerializeObject(quit)));

    }
    public void ThisPlayerWins()
    {
        if (serverConnection)
            return;
        var quit = new {LocalPlayer.playerId };
        ServerResponse.socket.Emit(ServerRequestApi.ON_PLAYER_WIN.ToString(), new JSONObject(JsonConvert.SerializeObject(quit)));
    }

    public void MovePlayer(int diceValue , int pawnNo ,int pawnType)
    {
        if (serverConnection)
            return;
        object generalInfo = new { diceValue , pawnNo ,LocalPlayer.playerId};
        ServerResponse.socket.Emit(ServerRequestApi.MOVE_PLAYER.ToString(),
            new JSONObject(JsonConvert.SerializeObject(generalInfo)), (data) => { Debug.Log(data); });
    }
    
    public void OnlinePlayers()
    {
        if (serverConnection)
            return;
        ServerResponse.socket.Emit(ServerReponseApi.ONLINE_PLAYERS.ToString());
    }

   

    public void OnPlayerWins(PawnType winnerPawn)
    {
        if (serverConnection)
            return;
        object winner = new { winnerPawn };

        ServerResponse.socket.Emit(ServerReponseApi.ON_PLAYER_WIN.ToString(),
            new JSONObject(JsonConvert.SerializeObject(winner)));

    }

}

