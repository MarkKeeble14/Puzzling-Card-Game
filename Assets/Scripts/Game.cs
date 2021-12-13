using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public enum Turn
{
    PLAYER,
    ENEMY
}

public enum Direction
{
    UNSPECIFIED,
    RIGHT,
    LEFT,
    UP,
    DOWN
}

public class Game : MonoBehaviour
{
    private KeyCode endTurnHotKey = KeyCode.KeypadEnter;

    [Header("Mechanics")]
    [SerializeField] private Turn currentTurn = Turn.ENEMY;
    [SerializeField] private bool turnEnded;

    [SerializeField] private CardPile hand;
    [SerializeField] private CardPile discardPile;
    [SerializeField] private CardPile deck;

    [SerializeField] private int numCardsDrawnPerTurn;
    [SerializeField] private int maxNumCardsInHand;

    [SerializeField] private CardDisplay activeCard;
    public CardDisplay ActiveCard
    {
        get { return activeCard; }
        set { activeCard = value; }
    }

    [SerializeField] private WorldGridCell selectedCell;
    public WorldGridCell SelectedCell
    {
        get { return selectedCell; }
        set { selectedCell = value; }
    }

    
    [SerializeField] private WorldGridCell activeCell;
    public WorldGridCell ActiveCell
    {
        get { return activeCell; }
        set { activeCell = value; }
    }
    
    [SerializeField] private int actionsPerTurn = 3;
    private int actionsThisTurn = 0;
    private int turnNumber = 0;

    private Queue<CardCommand> commandQueue = new Queue<CardCommand>();

    [SerializeField] private int numToDiscard = 0;
    [SerializeField] private int numToDestroy = 0;

    private Player player;
    private List<Enemy> enemies = new List<Enemy>();
    private List<Obstacle> obstacles = new List<Obstacle>();

    [SerializeField] private float cellSize;
    [SerializeField] private LayerMask gridCell;
    [SerializeField] private Direction chosenDirection = Direction.UNSPECIFIED;

    [Header("UI")]
    [SerializeField] private DirectionChooser directionChooser;
    private TextMeshProUGUI turnDisplay;
    private TextMeshProUGUI turnNumberDisplay;
    private HandDisplay handDisplay;
    private TextMeshProUGUI deckDisplay;
    private TextMeshProUGUI discardPileDisplay;
    private TextMeshProUGUI actionsPerTurnDisplay;

    public Direction ChosenDirection
    {
        get { return chosenDirection; }
        set { chosenDirection = value; }
    }

    private void Start()
    {
        // Set Everything
        turnDisplay = GameObject.Find("Whos Turn").GetComponent<TextMeshProUGUI>();
        turnNumberDisplay = GameObject.Find("Turn Number").GetComponent<TextMeshProUGUI>();
        handDisplay = GameObject.Find("Hand Display").GetComponent<HandDisplay>();
        deckDisplay = GameObject.Find("Deck").GetComponent<TextMeshProUGUI>();
        discardPileDisplay = GameObject.Find("Discard Pile").GetComponent<TextMeshProUGUI>();
        actionsPerTurnDisplay = GameObject.Find("Actions Per Turn").GetComponent<TextMeshProUGUI>();

        // Clone all Scriptable Objects
        hand.Init();
        discardPile.Init();
        deck.Init();

        // Set any UI
        SetActionsPerTurnDisplay();
        SetDeckDisplay();
        SetDiscardPileDisplay();

        // Start the Game
        AdvanceTurn();
    }

    private void Update()
    {
        if (Input.GetKeyDown(endTurnHotKey))
        {
            EndTurn();
        }
    }

    public void AddOccupant(Occupant occ)
    {
        if (occ is Player)
        {
            player = (Player)occ;
        } else if (occ is Enemy)
        {
            enemies.Add((Enemy)occ);
        } else if (occ is Obstacle)
        {
            obstacles.Add((Obstacle)occ);
        }
    }

    public void RemoveOccupant(Occupant occ)
    {
        if (occ is Player)
        {
            player = null;
        }
        else if (occ is Enemy)
        {
            enemies.Remove((Enemy)occ);
        }
        else if (occ is Obstacle)
        {
            obstacles.Remove((Obstacle)occ);
        }
    }

    public void AdvanceTurn()
    {
        turnEnded = false;
        switch (currentTurn)
        {
            case Turn.PLAYER:
                currentTurn = Turn.ENEMY;
                DiscardHand();
                break;
            case Turn.ENEMY:
                currentTurn = Turn.PLAYER;
                turnNumber++;
                DrawHand();
                break;
        }

        turnDisplay.text = currentTurn + " Turn";
        turnNumberDisplay.text = "Turn #: " + turnNumber;

        ResetTurnNumbers();

        // Start the Turn
        StartCoroutine(PlayTurn());
    }

    private IEnumerator PlayTurn()
    {
        // This Runs while the turn is still going on

        while (!turnEnded)
        {
            yield return null;
        }

        Debug.Log(currentTurn + " Ended");

        AdvanceTurn();
    }

    public void EndTurn()
    {
        turnEnded = true;
    }

    public void ResetActionsPerTurn()
    {
        actionsThisTurn = 0;
    }

    private void ResetTurnNumbers()
    {
        numToDiscard = 0;
        numToDestroy = 0;
        ResetActionsPerTurn();
        SetActionsPerTurnDisplay();
    }

    public void SetActionsPerTurnDisplay()
    {
        actionsPerTurnDisplay.text = actionsThisTurn + "/" + actionsPerTurn;
    }

    // Returns true if the Card was played, false if it was not
    public bool TryPlayCard(Card c)
    {
        // Playing the Card
        if (numToDiscard > 0)
        {
            numToDiscard--;
            if (!c.OneTimeUse)
                discardPile.Add(c);
        } else if (numToDestroy > 0)
        {
            numToDestroy--;
        } else
        {
            // Checking to make sure that the player has the actions to play the card
            if (actionsThisTurn + c.ActionCost > actionsPerTurn)
                return false;
            PlayCard(c);
            if (!c.OneTimeUse)
                discardPile.Add(c);
        }

        // Removing/Cleaning up the card
        hand.Remove(c);
        handDisplay.DestroyCardDisplay(c);

        // Updating UI
        SetDiscardPileDisplay();

        return true;
    }

    private void PlayCard(Card c)
    {
        actionsThisTurn += c.ActionCost;

        foreach (CardCommand cc in c.Commands)
            commandQueue.Enqueue(cc);

        SetActionsPerTurnDisplay();

        NextCommandQueue();
    }

    private void NextCommandQueue()
    {
        if (commandQueue.Count > 0)
        {
            ExecuteCommand(commandQueue.Dequeue());
        }
    }

    private void ExecuteCommand(CardCommand cc)
    {
        if (cc is DiscardCardsCommand)
        {
            StartCoroutine(DiscardCards((DiscardCardsCommand)cc));
        } else if (cc is DrawCardsCommand)
        {
            StartCoroutine(DrawCards((DrawCardsCommand)cc));
        } else if (cc is DestroyCardsCommand)
        {
            StartCoroutine(DestroyCards((DestroyCardsCommand)cc));
        } else if (cc is SpawnOccupantCommand)
        {
            StartCoroutine(SpawnOccupant((SpawnOccupantCommand)cc));
        } else if (cc is MovePlayerCommand)
        {
            StartCoroutine(MovePlayer((MovePlayerCommand)cc));
        } else if (cc is PushOccupantCommand)
        {
            StartCoroutine(PushOccupant((PullOccupantCommand)cc));
        } else if (cc is PullOccupantCommand)
        {
            StartCoroutine(PullOccupant((PullOccupantCommand)cc));
        }
    }

    private IEnumerator PullOccupant(PullOccupantCommand cc)
    {
        yield return null;
    }

    private IEnumerator PushOccupant(PullOccupantCommand cc)
    {
        yield return null;
    }

    private IEnumerator MovePlayer(MovePlayerCommand cc)
    {
        // Determine which way the player wants to move
        // Enable Direction Chooser
        directionChooser.gameObject.SetActive(true);

        yield return new WaitUntil(() => chosenDirection != Direction.UNSPECIFIED);

        directionChooser.gameObject.SetActive(false);

        Vector3 rayPos = Vector3.zero;
        switch (chosenDirection)
        {
            case (Direction.UP):
                rayPos = new Vector3(0, 10, cellSize * cc.CellsTraversed - cellSize / 2);
                break;
            case (Direction.DOWN):
                rayPos = new Vector3(0, 10, -cellSize * cc.CellsTraversed - cellSize / 2);
                break;
            case (Direction.RIGHT):
                rayPos = new Vector3(cellSize * cc.CellsTraversed - cellSize / 2, 10, 0);
                break;
            case (Direction.LEFT):
                rayPos = new Vector3(-cellSize * cc.CellsTraversed - cellSize / 2, 10, 0);
                break;
            default:
                rayPos = Vector3.zero;
                break;
        }

        RaycastHit hit;
        if (Physics.Raycast(player.Controller.transform.position + rayPos, 
            Vector3.down, out hit, Mathf.Infinity, gridCell))
        {
            WorldGridCell cellHit = hit.collider.gameObject.GetComponent<WorldGridCell>();
            player.Controller.TargetCell = cellHit;
        }

        chosenDirection = Direction.UNSPECIFIED;
    }

    private IEnumerator DrawCards(DrawCardsCommand cc)
    {
        for (int i = 0; i < cc.numAffected; i++)
        {
            DrawCard();
        }

        NextCommandQueue();

        yield return null;
    }

    private IEnumerator DiscardCards(DiscardCardsCommand cc)
    {
        numToDiscard = cc.numAffected;

        yield return new WaitUntil(() => numToDiscard <= 0);

        NextCommandQueue();
    }

    private IEnumerator DestroyCards(DestroyCardsCommand cc)
    {
        numToDestroy = cc.numAffected;

        yield return new WaitUntil(() => numToDestroy <= 0);

        NextCommandQueue();
    }

    private IEnumerator SpawnOccupant(SpawnOccupantCommand cc)
    {
        yield return new WaitUntil(() => selectedCell != null);

        selectedCell.SpawnOccupant(cc.Occupant);

        selectedCell = null;

        NextCommandQueue();
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
            if (c != null)
            {
                hand.Add(c);
                handDisplay.AddCard(c);
            }
        }

        SetDeckDisplay();
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

    private void SetDeckDisplay()
    {
        deckDisplay.text = "Deck: " + deck.NumCards;
    }
}
