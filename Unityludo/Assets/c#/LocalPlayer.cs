using UnityEngine;

public static class LocalPlayer 
{
    private static string _playerId;
    private static string _balance;
    private static string _profilePic;

    public static string playerId { get { return _playerId; } set { _playerId = value; } }
    public static string balance { get { return _balance; } set { _balance = value; } }
    public static string profilePic { get { return _profilePic; } set { _profilePic = value; } }
    
    public static void LoadGame()
    {
        _playerId = PlayerPrefs.GetString("_playerId", "null");
        _balance = PlayerPrefs.GetString("_balance","100000");
        _profilePic = PlayerPrefs.GetString("_profilePic", "noProfile");
    }
    public static void SaveGame()
    {
        PlayerPrefs.SetString("_playerId", _playerId);
        PlayerPrefs.SetString("_balance", _balance);
        PlayerPrefs.SetString("_profilePic", _profilePic);
        PlayerPrefs.Save();
    }

}
