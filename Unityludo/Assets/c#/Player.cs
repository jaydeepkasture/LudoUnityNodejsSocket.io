using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Image profilePicture;
    private string playerId;
    public PawnType playerPawn;

    private void Start()
    {
        profilePicture = GetComponent<Image>();   
    }
    public void SetProfile(string profilePic,string playerId)
    {
        print("setProfile");
        this.playerId = playerId;
        bool invalidPic = profilePic.Equals("noProfile") || profilePic.Equals(string.Empty);
        if (invalidPic) return;
        byte[] text = System.Convert.FromBase64String(profilePic);
        Texture2D tex = new Texture2D(2, 2);
        if (tex.LoadImage(text))
        {
            profilePicture.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), profilePicture.gameObject.transform.position); ;
        }
    }

    public void SetDefaultProfile()
    {
        profilePicture.sprite = UiManager.instance.defaultProfilePic;
    }


}
