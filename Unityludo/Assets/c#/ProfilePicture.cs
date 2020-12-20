using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System;

public class ProfilePicture : MonoBehaviour
{
	[SerializeField] private Button profilePicBtn;
	[SerializeField] private TextMeshProUGUI profileName;

	//public Text text;
	bool isLoaded = false;
	Texture2D texture;
	string imagePath = "";
	string message = "yet to load";
	//public ImageSaver ImageSaver;

	private void Start()
	{
		bool invalidId = LocalPlayer.playerId == string.Empty || !LocalPlayer.playerId.Equals( "null");
		print("validity:" + invalidId);
		if (invalidId)
		{
			print(LocalPlayer.playerId);
			profileName.text = LocalPlayer.playerId;
		}
		loadImage();
		profilePicBtn.onClick.AddListener(() =>
		{
			PickImage();
		});
	}
	

	private void PickImage()
	{
		try
		{

			int maxSize = 1024;
			//message = "Pick image called";
			NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
			{
				Debug.Log("Image path: " + path);
			//message = "Image path: " + path;
				if (path != null)
				{
					texture = NativeGallery.LoadImageAtPath(path, maxSize);
					if (texture == null)
					{
					//	message = "Couldn't load texture from " + path;
					return;
					}
					profilePicBtn.image.sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), transform.position); ;
					File.Copy(path, imagePath, true);
					isLoaded = true;

				}
			}, "Select a PNG image", "image/png");
			message = "Permission result: " + permission;
			Debug.Log("Permission result: " + permission);
		}
		catch (Exception e)
		{
			print("error" + e);
		}
		loadImage();
	}

	private void loadImage()
	{

		message = "gudda";
		imagePath = Application.persistentDataPath + "/testTexture.png";
		Debug.Log(imagePath);
		if (File.Exists(imagePath))
		{
			message = "---1----";
			print("Loading from the device");
			byte[] byteArray = File.ReadAllBytes(imagePath);
			message = "---2----";
			texture = new Texture2D(8, 8);
			texture.LoadImage(byteArray);
			message = "---3----";
			isLoaded = true;
			profilePicBtn.image.sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), transform.position); ;

			if (texture.isReadable)
			{
				print("readable");
				byte[] tex = texture.EncodeToJPG();
				LocalPlayer.profilePic = System.Convert.ToBase64String(tex);
			}
			else
			{
				print("Notreadable");
			}
		}
		else
		{
			message = "no img found";
		}
	}
}
