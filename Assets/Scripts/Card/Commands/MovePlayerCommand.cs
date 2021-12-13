using UnityEngine;

[CreateAssetMenu(fileName = "MovePlayerCommand", menuName = "CardCommand/MovePlayerCommand", order = 1)]
public class MovePlayerCommand : OccupantRelatedCommand
{
    public override CardEffect Effect
    {
        get
        {
            return CardEffect.MOVE;
        }
    }

    [SerializeField] private int cellsTraversed;
    public int CellsTraversed
    {
        get { return cellsTraversed; }
    }
}
