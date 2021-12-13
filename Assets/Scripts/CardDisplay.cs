using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    [SerializeField] private Card card;
    public Card Card
    {
        get { return card; }
    }
    [SerializeField] private Image background;
    [SerializeField] private Image pictureOutline;
    [SerializeField] private Image pictureBackground;
    [SerializeField] private Image nameBackground;

    [SerializeField] private TextMeshProUGUI cardName;
    [SerializeField] private TextMeshProUGUI cost;

    public bool isHovered;
    public RectTransform rTransform;
    public float minSizeX;
    public float maxSizeX;
    public float minSizeY;
    public float maxSizeY;
    public float moveScaleSpeed;
    public float movePosSpeed;
    public float moveRotSpeed;

    public float hoveredYAdd = 10f;

    private Vector3 targetLocalPos;
    private Vector3 storedTargetLocalPos;

    private bool isHeld;
    private int playCardDistanceThreshold = 50;

    private bool hasBeenPlayed;
    private Vector3 storedPos;
    [SerializeField] private float storedRot;

    private bool played;
    private HandDisplay hd;

    public void SetTargetPos(Vector3 t)
    {
        targetLocalPos = t;
        storedTargetLocalPos = t;
    }

    public void SetTargetRot(float rot)
    {
        storedRot = rot;
    }

    public void SetHandDisplay(HandDisplay hd)
    {
        this.hd = hd;
    }

    public void Set(Card c)
    {
        card = c;

        background.sprite = card.Background;
        pictureOutline.sprite = card.PictureOutline;
        pictureBackground.sprite = card.PictureBackground;
        nameBackground.sprite = card.NameBackground;

        cardName.text = card.CardName;
        cost.text = card.ActionCost.ToString();
    }

    private void Update()
    {
        // Move Card
        if (isHovered)
        {
            // Moving Card Upwards on Hover
            targetLocalPos = storedTargetLocalPos + new Vector3(0, hoveredYAdd, 0);

            // Clicking essentially selects the card
            if (Input.GetMouseButton(0))
            {
                isHeld = true;
                if (storedPos == Vector3.zero)
                {
                    storedPos = rTransform.position;
                }
            } else
            {
                if (isHeld == true && Vector3.Distance(rTransform.position, storedPos) > playCardDistanceThreshold)
                {
                    played = hd.PlayCard(this);
                }

                isHeld = false;
            }
        } else
        {
            targetLocalPos = storedTargetLocalPos;
            isHeld = false;
        }

        if (isHeld)
        {
            rTransform.position = Input.mousePosition;

            if (rTransform.localEulerAngles.z != 0)
            {
                rTransform.localEulerAngles = new Vector3(0, 0, Mathf.LerpAngle(rTransform.localEulerAngles.z, 0, moveRotSpeed * Time.deltaTime));
            }
        } else
        {
            if (!played)
            {
                if (rTransform.localPosition != targetLocalPos && !hasBeenPlayed)
                {
                    rTransform.localPosition = Vector2.Lerp(rTransform.localPosition, targetLocalPos, movePosSpeed * Time.deltaTime);
                }

                if (rTransform.localEulerAngles.z != storedRot)
                {
                    rTransform.localEulerAngles = new Vector3(0, 0, Mathf.LerpAngle(rTransform.localEulerAngles.z, storedRot, moveRotSpeed * Time.deltaTime));
                }
            }
        }
        UpdateOnHover();
    }

    private void UpdateOnHover()
    {
        Vector2 targetScale;
        if (isHovered)
            targetScale = new Vector2(maxSizeX, maxSizeY);
        else
            targetScale = new Vector2(minSizeX, minSizeY);

        if (Mathf.Approximately(rTransform.localScale.x, targetScale.x)
            && Mathf.Approximately(rTransform.localScale.y, targetScale.y))
            return;
        rTransform.localScale = Vector2.Lerp(rTransform.localScale, targetScale, moveScaleSpeed * Time.deltaTime);
    }
}