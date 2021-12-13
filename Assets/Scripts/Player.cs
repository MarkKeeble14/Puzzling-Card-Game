using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "Occupants/Player", order = 1)]
public class Player : Occupant, IOccupantHasAttack, IOccupantHasHP
{
    public int Atk { get => OAtk; }
    public int HP { get => OHP; }
}
