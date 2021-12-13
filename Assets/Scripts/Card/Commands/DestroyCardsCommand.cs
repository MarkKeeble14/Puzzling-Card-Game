using UnityEngine;

[CreateAssetMenu(fileName = "DestroyCardsCommand", menuName = "CardCommand/DestroyCardsCommand", order = 1)]
public class DestroyCardsCommand : DeckRelatedCommand
{
    public override CardEffect Effect
    {
        get
        {
            return CardEffect.DESTROY_CARD;
        }
    }
}
