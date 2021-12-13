using UnityEngine;

[CreateAssetMenu(fileName = "PushOccupantCommand", menuName = "CardCommand/PushOccupantCommand", order = 1)]
public class PushOccupantCommand : OccupantRelatedCommand
{
    public override CardEffect Effect
    {
        get
        {
            return CardEffect.PUSH_OCCUPANT;
        }
    }
}
