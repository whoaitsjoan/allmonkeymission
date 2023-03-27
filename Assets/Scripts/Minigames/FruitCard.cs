using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class FruitCard : ScriptableObject
{
    public string fruitType;
    public string fruitSprite;
    public bool isFlipped;

    // Get Functions
    public string GetFruitType(string fruit)
    {
        return this.fruitType;
    }

    public bool GetIsFlipped()
    {
        return this.isFlipped;
    }

    public FruitCard GetFruitCard()
    {
        return this;
    }

    // Set Functions
    public void SetFruitType(string fruit)
    {
        this.fruitType = fruit;
    }

    public void SetFruitSprite(string fruit)
    {
        this.fruitSprite = fruit;
    }

    public void SetIsFlipped(bool flippedStatus)
    {
        this.isFlipped = flippedStatus;
    }

    public string FruitCardToString()
    {
        return "Fruit Type: " + this.fruitType + " Is Flipped?: " + this.isFlipped;
    }
}