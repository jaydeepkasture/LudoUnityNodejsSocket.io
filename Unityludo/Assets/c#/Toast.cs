using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toast : MonoBehaviour
{
#if UNITY_ANDROID
    //AndroidJavaObject currentActivity;
    public static Toast instance;

    private void Awake()
    {
        instance = this;
    }
    //public void Start()
    //{
    //    //currentActivity androidjavaobject must be assigned for toasts to access currentactivity;
    //    AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    //    currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    //}
    //public void Massage(string message, ToastMsg type)
    //{
    //    AndroidJavaObject context = currentActivity.Call<AndroidJavaObject>("getApplicationContext");
    //    AndroidJavaClass Toast = new AndroidJavaClass("android.widget.Toast");
    //    AndroidJavaObject javaString = new AndroidJavaObject("java.lang.String", message);
    //    switch (type)
    //    {

    //        case ToastMsg.SHORT:
    //            AndroidJavaObject _toast = Toast.CallStatic<AndroidJavaObject>("makeText", context, javaString, Toast.GetStatic<int>("LENGTH_SHORT"));
    //            _toast.Call("show");
    //            break;

    //        case ToastMsg.LONG:
    //            AndroidJavaObject toast = Toast.CallStatic<AndroidJavaObject>("makeText", context, javaString, Toast.GetStatic<int>("LENGTH_LONG"));
    //            toast.Call("show");
    //            break;
    //    }
    //}
    public void Massage(string message)
    {
        //AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        //AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        //if (unityActivity != null)
        //{
        //    AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
        //    unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
        //    {
        //        AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity, message, 0);
        //        toastObject.Call("show");
        //    }));
        //}
    }


#endif
}

