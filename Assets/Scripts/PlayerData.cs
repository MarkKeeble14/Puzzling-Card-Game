using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerData : MonoBehaviour
{
    /*
    [SerializeField] private int numCardsDrawnPerTurn;
    [SerializeField] private int maxNumCardsInHand;

    [SerializeField] private CardPile hand;
    [SerializeField] private CardPile discardPile;
    [SerializeField] private CardPile deck;

    private HandDisplay handDisplay;
    private TextMeshProUGUI deckDisplay;
    private TextMeshProUGUI discardPileDisplay;
    private TextMeshProUGUI actionsPerTurnDisplay;

    [SerializeField] private WorldGridCell activeCell;
    public WorldGridCell ActiveCell
    {
        get { return activeCell; }
        set { activeCell = value; }
    }


    [SerializeField] private CardDisplay activeCard;
    public CardDisplay ActiveCard
    {
        get { return activeCard; }
        set { activeCard = value; }
    }

    private int actionsPerTurn = 3;
    private int actionsThisTurn = 0;

    private void Start()
    {
        handDisplay = GameObject.Find("Hand Display").GetComponent<HandDisplay>();
        deckDisplay = GameObject.Find("Deck").GetComponent<TextMeshProUGUI>();
        discardPileDisplay = GameObject.Find("Discard Pile").GetComponent<TextMeshProUGUI>();
        actionsPerTurnDisplay = GameObject.Find("Actions Per Turn").GetComponent<TextMeshProUGUI>();

        hand.Init();
        discardPile.Init();
        deck.Init();
    }

    public void ResetActionsPerTurn()
    {
        actionsThisTurn = 0;
    }

    public void SetActionsPerTurnDisplay()
    {
        actionsPerTurnDisplay.text = actionsThisTurn + "/" + actionsPerTurn;
    }

    public bool TryPlayCard(Card c)
    {
        if (hand.NumCards <= 0)
        {
            return false;
        }

        if (actionsThisTurn + c.ActionCost > actionsPerTurn)
        {
            return false;
        }

        hand.Remove(c);

        if (!c.OneTimeUse)
        {
            discardPile.Add(c);
        }

        handDisplay.DestroyCardDisplay(c);

        c.Play();

        actionsThisTurn += c.ActionCost;

        SetDiscardPileDisplay();
        SetActionsPerTurnDisplay();

        return true;
    }

    public void DrawHand()
    {
        // Draw numCardsDrawnPerTurn number of Cards
        for (int i = 0; i < numCardsDrawnPerTurn; i++)
        {
            DrawCard();
        }
    }

    private void DrawCard()
    {
        // If there are no more cards in our deck, shuffle the discard pile back into the deck
        if (deck.NumCards <= 0)
        {
            deck.AddFromPile(discardPile);
            deck.Shuffle();
            SetDiscardPileDisplay();
        }

        // If we already have the max number of drawn cards, add the cards into our discard pile instead of our hand
        if (hand.NumCards >= maxNumCardsInHand)
        {
            discardPile.Add(deck.Draw());
            SetDiscardPileDisplay();
        }
        else
        {
            Card c = deck.Draw();
            hand.Add(c);
            handDisplay.AddCard(c);
        }

        SetOOCDeckDisplay();
        SetDiscardPileDisplay();
    }

    public void DiscardHand()
    {
        while (hand.NumCards > 0)
        {
            Card c = hand.GetAt(0);
            hand.Remove(c);
            discardPile.Add(c);
        }

        handDisplay.ResetDisplay();
        SetDiscardPileDisplay();
    }

    private void SetDiscardPileDisplay()
    {
        discardPileDisplay.text = "Discard Pile: " + discardPile.NumCards;
    }

    private void SetOOCDeckDisplay()
    {
        deckDisplay.text = "Deck: " + deck.NumCards;
    }
    */
}
