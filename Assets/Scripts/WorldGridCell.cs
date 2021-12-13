using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldGridCell : MonoBehaviour
{
    public RectTransform cursorSelectionTransform;

    public bool isHovered;
    public float cursorSelectionMinSize;
    public float cursorSelectionMaxSize;
    public float animateSpeed;

    private Game game;

    [SerializeField] private Canvas canvas;
    [SerializeField] private Image sprite;
    [SerializeField] private Color activeColor;
    [SerializeField] private Color inactiveColor;

    [SerializeField] private Occupant spawnOccupantOnCell;
    [SerializeField] private Occupant occupant;
    private OccupantController occupantController;

    public void SetOccupant(Occupant occupant)
    {
        this.occupant = occupant;
        occupantController = occupant.Controller;
    }

    private void Start()
    {
        game = GameObject.Find("Game").GetComponent<Game>();

        if (spawnOccupantOnCell != null)
        {
            SpawnOccupant(spawnOccupantOnCell);
        }
    }

    public void SpawnOccupant(Occupant occ)
    {
        Occupant clone = Instantiate(occ);
        GameObject spawned = Instantiate(clone.Prefab, transform.position + new Vector3(0, 3, 0), Quaternion.identity);
        clone.Controller = spawned.GetComponent<OccupantController>();

        clone.Controller.Occupant = clone;
        clone.Controller.TargetCell = this;

        occupant = clone;
        game.AddOccupant(occupant);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHover();

        if (isHovered && Input.GetMouseButtonDown(0))
        {
            // Toggle Behaviour
            if (game.SelectedCell == this)
                game.SelectedCell = null;
            else
                game.SelectedCell = this;
        }

        if (game.SelectedCell == this)
        {
            sprite.color = activeColor;
        } else
        {
            sprite.color = inactiveColor;
        }

        if (occupant != null)
        {
            
        }
    }

    void UpdateHover()
    {
        Vector2 targetScale;
        if (isHovered)
            targetScale = new Vector2(cursorSelectionMaxSize, cursorSelectionMaxSize);
        else
            targetScale = new Vector2(cursorSelectionMinSize, cursorSelectionMinSize);

        if (Mathf.Approximately(cursorSelectionTransform.localScale.x, targetScale.x) 
            && Mathf.Approximately(cursorSelectionTransform.localScale.y, targetScale.y))
            return;
        cursorSelectionTransform.localScale = Vector2.Lerp(cursorSelectionTransform.localScale, targetScale, animateSpeed * Time.deltaTime);
    }

    private void OnMouseExit()
    {
        isHovered = false; 
        canvas.sortingOrder = 0;
        if (game.ActiveCell == this)
        {
            game.ActiveCell = null;
        }
    }

    private void OnMouseOver()
    {
        if (game.ActiveCard == null)
        {
            isHovered = true;
            canvas.sortingOrder = 1;
            game.ActiveCell = this;
        } else
        {
            isHovered = false;
            canvas.sortingOrder = 0;
        }
    }
}
