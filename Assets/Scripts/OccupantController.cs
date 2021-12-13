using UnityEngine;

public class OccupantController : MonoBehaviour
{
    [SerializeField] private MoveBehaviour moveBehaviour;
    [SerializeField] private float moveSpeed;

    private WorldGridCell targetCell;

    public WorldGridCell TargetCell
    {
        get { return targetCell; }
        set {
            targetCell = value;
            targetCell.SetOccupant(representedOccupant);
        }
    }

    private Occupant representedOccupant;
    public Occupant Occupant
    {
        get { return representedOccupant; }
        set { representedOccupant = value; }
    }

    private Vector3 occupantOffset = new Vector3(0, 3, 0);

    private void Update()
    {
        Vector3 targetPos = targetCell.transform.position + occupantOffset;
        if (transform.position != targetPos)
        {
            switch (moveBehaviour)
            {
                case (MoveBehaviour.LERP):
                    transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);
                    break;
                case (MoveBehaviour.MOVE_TOWARDS):
                    transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
                    break;
            }
        }
    }
}
