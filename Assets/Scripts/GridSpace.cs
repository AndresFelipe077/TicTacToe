using UnityEngine;
using UnityEngine.UI;

public class GridSpace : MonoBehaviour
{
    public Button button;
    public Text buttonText;

    private GameController gameController;

    /// <summary>
    /// Set the game controller reference
    /// </summary>
    /// <param name="controller"></param>
    public void SetGameControllerReference(GameController controller)
    {
        gameController = controller;
    }

    /// <summary>
    /// Sending movements and interactive buttons.
    /// </summary>
    public void SetSpace()
    {
        if (gameController.playerMove == true)
        {
            buttonText.text = gameController.GetPlayerSide();
            button.interactable = false;
            gameController.EndTurn();
        }

    }

}
