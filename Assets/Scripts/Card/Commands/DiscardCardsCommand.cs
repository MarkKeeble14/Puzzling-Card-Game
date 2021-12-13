using UnityEngine;

[CreateAssetMenu(fileName = "DiscardCardsCommand", menuName = "CardCommand/DiscardCardsCommand", order = 1)]
public class DiscardCardsCommand : DeckRelatedCommand
{
    public override CardEffect Effect
    {
        get
        {
            return CardEffect.DISCARD_CARD;
        }
    }
}
