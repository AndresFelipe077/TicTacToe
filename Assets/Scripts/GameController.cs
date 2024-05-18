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

    void SetGameControllerReferenceOnButtons()
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<GridSpace>().SetGameControllerReference(this);
        }
    }

    public void SetStartingSide(string startingSide)
    {
        playerSide = startingSide;
        if (playerSide == "X")
        {
            computerSide = "O";
            //SetPlayerColors(playerX, playerO);
        }
        else
        {
            computerSide = "X";
            //SetPlayerColors(playerO, playerX);
        }

        StartGame();

    }

    void StartGame()
    {
        SetBoardInteratable(true);
        SetPlayerButtons(false);
        startInfo.SetActive(false);
    }

    public string GetPlayerSide()
    {
        return playerSide;
    }

    public string GetComputerSide()
    {
        return computerSide;
    }

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

    private bool CheckForWinner(int a, int b, int c)
    {
        // Convertir los valores de texto en los botones a mayúsculas y comparar
        return buttonList[a].text == playerSide &&
                buttonList[b].text == playerSide &&
                buttonList[c].text == playerSide;
    }

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
    /// Establece todos los botones en la lista de botones como no interactivos, indicando que el juego ha terminado.
    /// </summary>
    void GameOver(string winningPlayer)
    {

        Debug.Log(winningPlayer);

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

    void ChangeSides()
    {
        playerMove = (playerMove == true) ? false : true;

        //if (playerMove)
        //{
        //SetPlayerColors(playerX, playerO);
        //}
        //else
        //{
        //SetPlayerColors(playerO, playerX);
        //}
    }

    void SetGameOverText(string value)
    {
        gameOverPanel.SetActive(true);
        gameOverText.text = value;
    }

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

    void SetBoardInteratable(bool toggle)
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<Button>().interactable = toggle;
        }
    }

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
