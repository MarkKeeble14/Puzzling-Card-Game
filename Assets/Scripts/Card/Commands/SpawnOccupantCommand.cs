using UnityEngine;

[CreateAssetMenu(fileName = "SpawnOccupantCommand", menuName = "CardCommand/SpawnOccupantCommand", order = 1)]
public class SpawnOccupantCommand : CardCommand
{
    [SerializeField] private Occupant occupant;
    public Occupant Occupant
    {
        get { return occupant; }
    }

    public override CardEffect Effect
    {
        get
        {
            return CardEffect.SPAWN_OCCUPANT;
        }
    }
}
