using UnityEngine;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

class GameManager : MonoBehaviour
{
    private void Awake()
    {
        LocalPlayer.LoadGame();
        CheckInternet();

    }

    private void CheckInternet()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            print("internet connection not available");
            ServerRequest.instance.serverConnection = false;
        }
        else
        {
            ServerRequest.instance.serverConnection = true;

        }
    }

    private void Start()
    {

        UiManager.instance.UpdateUi();

        if (LocalPlayer.playerId == "null")
        {
            var id = new { LocalPlayer.playerId };
            ServerResponse.socket.Emit(ServerRequestApi.PLAYER_REGISTRATION.ToString(), new JSONObject(JsonConvert.SerializeObject(id)));
        }
        else
        {
            var id = new { LocalPlayer.playerId };
            ServerResponse.socket.Emit(ServerRequestApi.PLAYER_REGISTRATION.ToString(), new JSONObject(JsonConvert.SerializeObject(id)));
        }
        LocalPlayer.LoadGame();

    }

    private void OnApplicationQuit()
    {
        LocalPlayer.SaveGame();
        ServerRequest.instance.QuitGame();
    }
}
