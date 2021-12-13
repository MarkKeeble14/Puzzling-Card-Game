using UnityEngine;

[CreateAssetMenu(fileName = "PullOccupantCommand", menuName = "CardCommand/PullOccupantCommand", order = 1)]
public class PullOccupantCommand : OccupantRelatedCommand
{
    public override CardEffect Effect
    {
        get
        {
            return CardEffect.PULL_OCCUPANT;
        }
    }
}
