using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixingCamera : MonoBehaviour
{
    private void Awake()
    {
        SetupCamera();
    }

    private void SetupCamera()
    {
        float targetWidthAspect = 16.0f;

        float targetHeightAspect = 9.0f;

        Camera mainCamera = Camera.main;

        mainCamera.aspect = targetWidthAspect / targetHeightAspect;

        float widthRatio = (float)Screen.width / targetWidthAspect;
        float heightRatio = (float)Screen.height / targetHeightAspect;

        float heightadd = ((widthRatio / (heightRatio / 100)) - 100) / 200;
        float widthtadd = ((heightRatio / (widthRatio / 100)) - 100) / 200;

        if (heightRatio > widthRatio)
            widthtadd = 0.0f;
        else
            heightadd = 0.0f;


        mainCamera.rect = new Rect(
            mainCamera.rect.x + Math.Abs(widthtadd),
            mainCamera.rect.y + Math.Abs(heightadd),
            mainCamera.rect.width + (widthtadd * 2),
            mainCamera.rect.height + (heightadd * 2));
    }
}

//https://m.blog.naver.com/PostView.nhn?blogId=fnzlz&logNo=221069086916&proxyReferer=https%3A%2F%2Fwww.google.com%2F