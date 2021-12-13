using UnityEngine;

[CreateAssetMenu(fileName = "DrawCardsCommand", menuName = "CardCommand/DrawCardsCommand", order = 1)]
public class DrawCardsCommand : DeckRelatedCommand
{
    public override CardEffect Effect
    {
        get
        {
            return CardEffect.DRAW_CARD;
        }
    }
}
