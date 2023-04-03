using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu (fileName = "PopupWindow", menuName = "ScriptableObjects/PopupWindow", order = 2)]

public class PopupWindow : ScriptableObject
{
    public Image windowImage;
    public float orderOnScreen;

    // Get Functions

    public Image GetWindowImage(Image img)
    {
        return this.windowImage;
    }

    public float GetOrderOnScreen (float order)
    {
        return this.orderOnScreen;
    }

    public PopupWindow GetPopupWindow()
    {
        return this;
    }

    // Set Functions

    public void SetWindowImage(Image img)
    {
        this.windowImage = img;
    }
    public void SetOrderOnScreen (float order)
    {
        this.orderOnScreen = order;
    }

}
