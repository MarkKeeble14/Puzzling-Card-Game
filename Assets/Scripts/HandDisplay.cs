using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HandDisplay : MonoBehaviour
{
    public GameObject cardPrefab;

    // Tracking
    private Dictionary<Card, GameObject> displayedCards = new Dictionary<Card, GameObject>();
    private Dictionary<GameObject, CardDisplay> gameObjectCardDictionary = new Dictionary<GameObject, CardDisplay>();
    private Dictionary<CardDisplay, int> displayIndexDictionary = new Dictionary<CardDisplay, int>();

    // Showing/Aligning the Displays
    [SerializeField]
    private List<CardDisplay> displays = new List<CardDisplay>();

    public float midPoint;
    public float displayWidth;
    public float overlapWidth;
    public float defaultYPos;
    public float handBendAngle;
    public float handBendYReduction;

    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;
    [SerializeField] Canvas canvas;
    [SerializeField] private Game game;

    private void Start()
    {
        //Fetch the Raycaster from the GameObject (the Canvas)
        m_Raycaster = canvas.GetComponent<GraphicRaycaster>();
        //Fetch the Event System from the Scene
        m_EventSystem = GetComponent<EventSystem>();
        // Fetch the Game Script
        game = GameObject.Find("Game").GetComponent<Game>();
    }

    private void Update()
    {
        RaycastForHoveredCard();
    }

    private void RaycastForHoveredCard()
    {
        //Set up the new Pointer Event
        m_PointerEventData = new PointerEventData(m_EventSystem);
        //Set the Pointer Event Position to that of the mouse position
        m_PointerEventData.position = Input.mousePosition;

        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
        m_Raycaster.Raycast(m_PointerEventData, results);

        CardDisplay currentHover = null;

        //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag("CardDisplay"))
            {
                CardDisplay hovered = null;
                if (gameObjectCardDictionary.ContainsKey(result.gameObject))
                {
                    hovered = gameObjectCardDictionary[result.gameObject];
                }
                else
                {
                    hovered = result.gameObject.GetComponent<CardDisplay>();
                    gameObjectCardDictionary.Add(result.gameObject, hovered);
                }
                hovered.isHovered = true;
                currentHover = hovered;
                break;
            }
        }

        foreach (KeyValuePair<GameObject, CardDisplay> entry in gameObjectCardDictionary)
        {
            if (entry.Value != currentHover)
            {
                entry.Value.isHovered = false;
                entry.Value.rTransform.SetSiblingIndex(displayIndexDictionary[entry.Value]);
            }
        }

        if (currentHover)
        {
            game.ActiveCard = currentHover;
            currentHover.rTransform.SetAsLastSibling();
        } else
        {
            game.ActiveCard = null;
        }
    }

    public void AddCard(Card c)
    {
        GameObject i = Instantiate(cardPrefab);
        CardDisplay cd = i.GetComponent<CardDisplay>();
        cd.Set(c);
        cd.SetHandDisplay(this);
        i.transform.SetParent(transform);

        displayedCards.Add(c, i);

        displays.Add(cd);
        UpdateHandDisplay();

        displayIndexDictionary.Add(cd, displayIndexDictionary.Count);
    }

    public bool PlayCard(CardDisplay cd)
    {
        return game.TryPlayCard(cd.Card);
    }

    public void ResetDisplay()
    {
        while (displayedCards.Count > 0)
        {
            DestroyCardDisplay(displayedCards.Keys.ToList()[0]);
        }
    }

    public void DestroyCardDisplay(Card c)
    {
        Destroy(displayedCards[c]);
        gameObjectCardDictionary.Remove(displayedCards[c]);

        CardDisplay cd = displayedCards[c].GetComponent<CardDisplay>();
        displays.Remove(cd);
        displayIndexDictionary.Remove(cd);
        displayedCards.Remove(c);

        UpdateHandDisplay();
    }

    private void UpdateHandDisplay()
    {
        for (int i = 0; i < displays.Count; i++)
        {
            CardDisplay cd = displays[i];
            int indicesFromMiddle = 0;

            if (displays.Count % 2 == 0) // Even number of cards in hand
            {
                int middleIndex = (int)Mathf.Floor(displays.Count / 2);
                bool leftMiddle = false;
                if (i < middleIndex)
                {
                    middleIndex--;
                    leftMiddle = true;
                }
                indicesFromMiddle = i - middleIndex;

                Vector2 newPos = new Vector2((i * 0.5f * (displayWidth - overlapWidth)), 
                    defaultYPos - (handBendYReduction * Mathf.Abs(indicesFromMiddle)));
                cd.SetTargetPos(newPos);
                if (indicesFromMiddle == 0)
                {
                    if (leftMiddle)
                        cd.SetTargetRot(handBendAngle * 0.5f);
                    else
                        cd.SetTargetRot(handBendAngle * -0.5f);
                } else
                {
                    cd.SetTargetRot(handBendAngle * -indicesFromMiddle);
                }
            } else // Odd number of cards in hand
            {
                int middleIndex = (int)Mathf.Floor(displays.Count / 2);
                indicesFromMiddle = i - middleIndex;

                Vector2 newPos = new Vector2(((-0.5f * (displayWidth - overlapWidth)) + i * 0.5f * (displayWidth - overlapWidth)),
                    defaultYPos - (handBendYReduction * Mathf.Abs(indicesFromMiddle)));
                cd.SetTargetPos(newPos);
                cd.SetTargetRot(handBendAngle * -indicesFromMiddle);
            }

        }
    }
}
