using UnityEngine;

public class DirectionChooser : MonoBehaviour
{
    [SerializeField] private Game game;

    public void ChoseUp()
    {
        game.ChosenDirection = Direction.UP;
    }

    public void ChoseDown()
    {
        game.ChosenDirection = Direction.DOWN;
    }

    public void ChoseRight()
    {
        game.ChosenDirection = Direction.RIGHT;
    }

    public void ChoseLeft()
    {
        game.ChosenDirection = Direction.LEFT;
    }
}