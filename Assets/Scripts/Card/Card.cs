using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Card", order = 1)]
[System.Serializable]
public class Card : ScriptableObject
{
    [SerializeField] private string cardName;
    public string CardName
    {
        get { return cardName; }
    }

    [SerializeField] private CardCommand[] commands;
    public CardCommand[] Commands
    {
        get { return commands; }
    }
    [SerializeField] private bool oneTimeUse;
    public bool OneTimeUse
    {
        get { return oneTimeUse; }
    }

    [SerializeField] private int actionCost;
    public int ActionCost
    {
        get { return actionCost; }
    }

    [SerializeField] private Sprite background;
    public Sprite Background
    {
        get { return background; }
    }

    [SerializeField] private Sprite pictureOutline;

    public Sprite PictureOutline
    {
        get { return pictureOutline; }
    }

    [SerializeField] private Sprite pictureBackground;

    public Sprite PictureBackground
    {
        get { return pictureBackground; }
    }

    [SerializeField] private Sprite nameBackground;

    public Sprite NameBackground
    {
        get { return nameBackground; }
    }
}
