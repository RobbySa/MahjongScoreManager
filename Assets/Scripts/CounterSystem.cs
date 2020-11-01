using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CounterSystem : MonoBehaviour
{
    [SerializeField] PlayersManager playersManager;
    [SerializeField] GameObject pointsHolder;
    [SerializeField] Text pointText;

    Player dealer;
    GameState gameState;
    Wind currentWind;

    int dealerNumber;
    int currentPoints;

    int? selectedWinner;
    int? selectedLoser;

    public void Awake()
    {
        playersManager.Init();
        pointsHolder.SetActive(false);
        pointText.text = "0";

        dealer = playersManager.PlayerList[0];
        gameState = GameState.Waiting;
        currentWind = Wind.East;

        dealerNumber = 0;
        currentPoints = 0;
    }

    public void Update()
    {
        // waiting for input
        if (gameState == GameState.Waiting)
        {
            pointsHolder.SetActive(false);

            foreach (var player in playersManager.PlayerList)
            {
                player.IsHighlighted(false);

                if (player == dealer)
                    player.DealerMark.color = new Color(1f, 0f, 0f, 1f);
                else
                    player.DealerMark.color = new Color(1f, 0f, 0f, 0f);
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                selectedWinner = 0;
                selectedLoser = 0;
                currentPoints = 0;
                pointText.text = currentPoints.ToString();
                gameState = GameState.SelectingWinner;
            }
        }
        // select who won
        else if (gameState == GameState.SelectingWinner)
            HandleSelectingWinner();
        else if (gameState == GameState.SelectingLooser)
            HandleSelectingLoser();
        else if (gameState == GameState.PointSelection)
            HandlePointSelection();
    }

    void HandleSelectingWinner()
    {
        // go left and right in the selected
        if (Input.GetKeyDown(KeyCode.RightArrow) && selectedWinner < 3)
            selectedWinner++;
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && selectedWinner > 0)
            selectedWinner--;

        // there exist a winner
        if (Input.GetKeyDown(KeyCode.W))
        {
            gameState = GameState.SelectingLooser;

            foreach (var player in playersManager.PlayerList)
                player.IsHighlighted(false);
        }
        // it's a draw
        else if (Input.GetKeyDown(KeyCode.N))
        {
            gameState = GameState.Waiting;
            playersManager.HandCompleted(null, null, 0, currentWind);

            dealerNumber++;
            nextJhong();
        }
        // go back
        else if (Input.GetKeyDown(KeyCode.Backspace))
            gameState = GameState.Waiting;

        // change who is highlighted
        foreach (var player in playersManager.PlayerList)
        {
            if (playersManager.PlayerList[selectedWinner.Value] == player)
                player.IsHighlighted(true);
            else
                player.IsHighlighted(false);
        }
    }
    void HandleSelectingLoser()
    {
        // go left and right in the selected
        if (Input.GetKeyDown(KeyCode.RightArrow) && selectedLoser < 3)
            selectedLoser++;
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && selectedLoser > 0)
            selectedLoser--;

        // there exist a loser
        if (Input.GetKeyDown(KeyCode.L))
        {
            gameState = GameState.PointSelection;

            foreach (var player in playersManager.PlayerList)
                player.IsHighlighted(false);
        }
        // it's a draw
        else if (Input.GetKeyDown(KeyCode.N))
        {
            gameState = GameState.PointSelection;
            selectedLoser = null;
        }
        // go back
        else if (Input.GetKeyDown(KeyCode.Backspace))
            gameState = GameState.SelectingWinner;

        // change who is highlighted
        foreach (var player in playersManager.PlayerList)
        {
            if (playersManager.PlayerList[selectedLoser.Value] == player)
                player.IsHighlighted(true);
            else
                player.IsHighlighted(false);
        }
    }
    void HandlePointSelection()
    {
        pointsHolder.SetActive(true);

        // go up and down in points
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentPoints++;
            pointText.text = currentPoints.ToString();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && currentPoints > 0)
        {
            currentPoints--;
            pointText.text = currentPoints.ToString();
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            pointsHolder.SetActive(false);
            selectedLoser = 0;

            gameState = GameState.SelectingLooser;
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            gameState = GameState.Waiting;

            var loser = (selectedLoser == null) ? null : playersManager.PlayerList[selectedLoser.Value];
            playersManager.HandCompleted(playersManager.PlayerList[selectedWinner.Value], loser, currentPoints, currentWind);

            if (playersManager.PlayerList[selectedWinner.Value] != dealer)
                dealerNumber++;

            nextJhong();
        }
    }

    private void nextJhong()
    {
        if (dealerNumber > 3)
        {
            dealerNumber = 0;

            playersManager.WindCompleted(currentWind);

            if (currentWind == Wind.East)
            {
                currentWind = Wind.South;
            }
            else if (currentWind == Wind.South)
                currentWind = Wind.West;
            else if (currentWind == Wind.West)
                currentWind = Wind.North;
            else
                currentWind = Wind.End;
        }

        dealer = playersManager.PlayerList[dealerNumber];
    }
}

public enum GameState
{
    Waiting,
    SelectingWinner,
    SelectingLooser,
    PointSelection
}
