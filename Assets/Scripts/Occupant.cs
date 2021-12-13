using UnityEngine;

public abstract class Occupant : ScriptableObject
{
    [SerializeField] private GameObject prefab;
    public GameObject Prefab
    {
        get { return prefab; }
    }

    [SerializeField] private int hp;
    protected int OHP
    {
        get { return hp; }
    }

    [SerializeField] private int atk;
    protected int OAtk
    {
        get { return atk; }
    }

    private OccupantController controller;
    public OccupantController Controller
    {
        get { return controller; }
        set { controller = value; }
    }
}
