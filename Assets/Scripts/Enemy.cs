using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Occupants/Enemy", order = 1)]
public class Enemy : Occupant, IOccupantHasAttack, IOccupantHasHP
{
    public int Atk { get => OAtk; }
    public int HP { get => OHP; }
}