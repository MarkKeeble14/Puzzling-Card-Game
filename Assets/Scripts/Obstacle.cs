using UnityEngine;

[CreateAssetMenu(fileName = "Obstacle", menuName = "Occupants/Obstacle", order = 1)]
public class Obstacle : Occupant, IOccupantHasHP
{
    public int HP { get => OHP; }
}
