using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "New Card", menuName = "Card Game/Card")]

public class Card : ScriptableObject
{
    public string cardName;
    public int cardNumber;
    public CardSuit suit;
    public int currencyValue;
    public Sprite artAsset;
}
