using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "PopupWindow", menuName = "ScriptableObjects/PopupWindow", order = 2)]

public class PopupWindow : ScriptableObject
{
    public float screenTime;
    public string windowImage;

    // Get Functions
    public float GetScreenTime(float screentime)
    {
        return this.screenTime;
    }

    public string GetWindowImage(string image)
    {
        return this.windowImage;
    }

    public PopupWindow GetPopupWindow()
    {
        return this;
    }

    // Set Functions
    public void SetScreenTime(float screentime)
    {
        this.screenTime = screentime;
    }

    public void SetWindowImage(string image)
    {
        this.windowImage = image;
    }


}
