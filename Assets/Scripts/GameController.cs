using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Player
{
    public Image panel;
    public TextMeshProUGUI text;
    public Button button;
}

[System.Serializable]
public class PlayerColor
{
    public Color panelColor;
    public Color textColor;
}

public class GameController : MonoBehaviour
{
    public Text[] buttonList;
    public string playerSide;

    public GameObject gameOverPanel;
    [SerializeField] public TextMeshProUGUI gameOverText;

    private int moveCount;

    public GameObject restartButton;

    public Player playerX;
    public Player playerO;
    public PlayerColor activePlayerColor;
    public PlayerColor inactivePlayerColor;

    public GameObject startInfo;

    private string computerSide;
    public bool playerMove;
    public float delay;
    private int value;

    void Awake()
    {
        gameOverPanel.SetActive(false);
        SetGameControllerReferenceOnButtons();
        moveCount = 0;
        restartButton.SetActive(false);
        playerMove = true;
    }

    private void Update()
    {
        if (playerMove == false)
        {
            delay += delay * Time.deltaTime;
            if (delay >= 100)
            {
                value = UnityEngine.Random.Range(0, 8);
                if (buttonList[value].GetComponentInParent<Button>().interactable == true)
                {
                    buttonList[value].text = GetComputerSide();
                    buttonList[value].GetComponentInParent<Button>().interactable = false;
                    EndTurn();
                }
            }
        }
    }

    /// <summary>
    /// Set reference of buttons
    /// </summary>
    void SetGameControllerReferenceOnButtons()
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<GridSpace>().SetGameControllerReference(this);
        }
    }

    /// <summary>
    /// Set player to start game
    /// </summary>
    /// <param name="startingSide"></param>
    public void SetStartingSide(string startingSide)
    {
        playerSide = startingSide;
        if (playerSide == "X")
        {
            computerSide = "O";
        }
        else
        {
            computerSide = "X";
        }

        StartGame();

    }

    /// <summary>
    /// Start the game
    /// </summary>
    void StartGame()
    {
        SetBoardInteratable(true);
        SetPlayerButtons(false);
        startInfo.SetActive(false);
    }

    /// <summary>
    /// Get my player
    /// </summary>
    /// <returns></returns>
    public string GetPlayerSide()
    {
        return playerSide;
    }

    /// <summary>
    /// Get player machine
    /// </summary>
    /// <returns></returns>
    public string GetComputerSide()
    {
        return computerSide;
    }

    /// <summary>
    /// Turn of players
    /// </summary>
    public void EndTurn()
    {
        moveCount++;

        // Comprueba si hay un ganador en las filas, columnas y diagonales
        if (CheckForWinner(0, 1, 2) || CheckForWinner(3, 4, 5) || CheckForWinner(6, 7, 8) ||
            CheckForWinner(0, 3, 6) || CheckForWinner(1, 4, 7) || CheckForWinner(2, 5, 8) ||
            CheckForWinner(0, 4, 8) || CheckForWinner(2, 4, 6))
        {
            GameOver(playerSide);
        }
        else if (CheckForWinnerMachine(0, 1, 2) || CheckForWinnerMachine(3, 4, 5) || CheckForWinnerMachine(6, 7, 8) ||
                 CheckForWinnerMachine(0, 3, 6) || CheckForWinnerMachine(1, 4, 7) || CheckForWinnerMachine(2, 5, 8) ||
                 CheckForWinnerMachine(0, 4, 8) || CheckForWinnerMachine(2, 4, 6))
        {
            GameOver(computerSide);
        }
        else if (moveCount >= 9)
        {
            GameOver("empate");
        }
        else
        {
            ChangeSides();
        }
    }

    /// <summary>
    /// Check if I win
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="c"></param>
    /// <returns></returns>
    private bool CheckForWinner(int a, int b, int c)
    {
        // Convertir los valores de texto en los botones a mayúsculas y comparar
        return buttonList[a].text == playerSide &&
               buttonList[b].text == playerSide &&
               buttonList[c].text == playerSide;
    }

    /// <summary>
    /// Check if the machine won
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="c"></param>
    /// <returns></returns>
    private bool CheckForWinnerMachine(int a, int b, int c)
    {
        return buttonList[a].text == GetComputerSide() &&
               buttonList[b].text == GetComputerSide() &&
               buttonList[c].text == GetComputerSide();
    }

    void SetPlayerColors(Player newPlayer, Player oldPlayer)
    {
        newPlayer.panel.color = activePlayerColor.panelColor;
        newPlayer.text.color = activePlayerColor.textColor;
        oldPlayer.panel.color = inactivePlayerColor.panelColor;
        oldPlayer.text.color = inactivePlayerColor.textColor;
    }

    /// <summary>
    /// Sets all buttons in the button list to non-interactive, indicating that the game is over.
    /// </summary>
    void GameOver(string winningPlayer)
    {

        SetBoardInteratable(false);

        if (winningPlayer == "empate")
        {
            SetGameOverText("¡Empate!");
        }
        else if (winningPlayer == GetComputerSide())
        {
            SetGameOverText("¡" + GetComputerSide() + " ganó!");
        }
        else if (winningPlayer == playerSide)
        {
            SetGameOverText("¡" + playerSide + " ganó!");
        }

        restartButton.SetActive(true);
    }

    /// <summary>
    /// Change of players
    /// </summary>
    void ChangeSides()
    {
        playerMove = (playerMove == true) ? false : true;
    }

    /// <summary>
    /// Set of param value to game over
    /// </summary>
    /// <param name="value"></param>
    void SetGameOverText(string value)
    {
        gameOverPanel.SetActive(true);
        gameOverText.text = value;
    }

    /// <summary>
    /// Reset game
    /// </summary>
    public void RestartGame()
    {

        moveCount = 0;
        gameOverPanel.SetActive(false);
        restartButton.SetActive(false);
        SetPlayerButtons(true);
        // SetPlayerColorsInactive();
        startInfo.SetActive(true);
        playerMove = true;
        delay = 10;

        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].text = "";
        }

    }

    /// <summary>
    /// inactivate buttons when they have already been pressed or the game has not started
    /// </summary>
    /// <param name="toggle"></param>
    void SetBoardInteratable(bool toggle)
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<Button>().interactable = toggle;
        }
    }

    /// <summary>
    /// Set the buttons with the interactivity of each player
    /// </summary>
    /// <param name="toggle"></param>
    void SetPlayerButtons(bool toggle)
    {
        playerX.button.interactable = toggle;
        playerO.button.interactable = toggle;
    }

    void SetPlayerColorsInactive()
    {
        playerX.panel.color = inactivePlayerColor.panelColor;
        playerX.text.color = inactivePlayerColor.textColor;
        playerO.panel.color = inactivePlayerColor.panelColor;
        playerO.text.color = inactivePlayerColor.textColor;
    }

}
