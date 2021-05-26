using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ToastMessenger
{
    private static AndroidJavaObject currentActivity;
    private static AndroidJavaClass UnityPlayer;
    private static AndroidJavaObject context;
    private static AndroidJavaObject toast;

    public static void Init()
    {
        UnityPlayer =
            new AndroidJavaClass("com.unity3d.player.UnityPlayer");

        currentActivity = UnityPlayer
            .GetStatic<AndroidJavaObject>("currentActivity");


        context = currentActivity
            .Call<AndroidJavaObject>("getApplicationContext");
    }

    public static void ShowToast(string message)
    {
        currentActivity.Call
        (
            "runOnUiThread",
            new AndroidJavaRunnable(() =>
            {
                AndroidJavaClass Toast
                = new AndroidJavaClass("android.widget.Toast");

                AndroidJavaObject javaString
                = new AndroidJavaObject("java.lang.String", message);

                toast = Toast.CallStatic<AndroidJavaObject>
                (
                    "makeText",
                    context,
                    javaString,
                    Toast.GetStatic<int>("LENGTH_SHORT")
                );

                toast.Call("show");
            })
         );
    }

    public static void CancelToast()
    {
        currentActivity.Call("runOnUiThread",
            new AndroidJavaRunnable(() =>
            {
                if (toast != null) toast.Call("cancel");
            }));
    }
}
