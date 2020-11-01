using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersManager : MonoBehaviour
{
    [SerializeField] Player playerOne;
    [SerializeField] Player playerTwo;
    [SerializeField] Player playerThree;
    [SerializeField] Player playerFour;

    [SerializeField] int winMultiplier;
    [SerializeField] int selfDrawMultiplier;

    public List<Player> PlayerList { get; private set; }


    public void Init()
    {
        PlayerList = new List<Player>();
        PlayerList.Add(playerOne);
        PlayerList.Add(playerTwo);
        PlayerList.Add(playerThree);
        PlayerList.Add(playerFour);
    }

    public void HandCompleted(Player winner, Player loser, int valueOfHand, Wind currentWind)
    {
        // standard win
        if (loser != null)
        {
            foreach (var player in PlayerList)
            {
                if (player == winner)
                    player.AddPointsToWind(valueOfHand * winMultiplier, currentWind);
                else if (player == loser)
                    player.AddPointsToWind(-valueOfHand * winMultiplier, currentWind);
                else
                    player.AddPointsToWind(0, currentWind);
            }
        }
        // draw
        else if (winner == null)
            foreach (var player in PlayerList)
                player.AddPointsToWind(0, currentWind);
        // self draw
        else
        {
            foreach (var player in PlayerList)
            {
                if (player == winner)
                    player.AddPointsToWind(valueOfHand * selfDrawMultiplier, currentWind);
                else
                    player.AddPointsToWind(-valueOfHand, currentWind);

                player.ShowTotalForWind(currentWind);
            }
        }
    }

    public void WindCompleted(Wind wind)
    {
        foreach (var player in PlayerList)
            player.ShowTotalForWind(wind);
    }
}
