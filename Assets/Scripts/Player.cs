using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] Text playerName;
    [SerializeField] Text dealerMark;
    [SerializeField] Text eastPointsTotal;
    [SerializeField] Text southPointsTotal;
    [SerializeField] Text westPointsTotal;
    [SerializeField] Text northPointsTotal;

    [SerializeField] GameObject eastPointsObject;
    [SerializeField] GameObject southPointsObject;
    [SerializeField] GameObject westPointsObject;
    [SerializeField] GameObject northPointsObject;

    public Text DealerMark { get { return dealerMark; } }

    public List<int> EastWindPoints { get; private set; }
    public List<int> SouthWindPoints { get; private set; }
    public List<int> WestWindPoints { get; private set; }
    public List<int> NorthWindPoints { get; private set; }

    public string PlayerName { get { return playerName.text; } }

    public void Awake()
    {
        EastWindPoints = new List<int>();
        SouthWindPoints = new List<int>();
        WestWindPoints = new List<int>();
        NorthWindPoints = new List<int>();

        eastPointsTotal.text = "";
        southPointsTotal.text = "";
        westPointsTotal.text = "";
        northPointsTotal.text = "";
    }

    public void UpdateWindPoints(GameObject points, int value, int currentRoundCounts)
    {
        GameObject child = new GameObject(currentRoundCounts.ToString(), typeof(RectTransform));
        child.transform.SetParent(points.transform);

        float newX = 0;
        float newY = -30;
        if (currentRoundCounts <= 4)
        {
            newX -= 50;
            newY -= (20 * (currentRoundCounts - 1));
        }
        else if (currentRoundCounts <= 8)
            newY -= (20 * (currentRoundCounts - 5));
        else
        {
            newX += 50;
            newY -= (20 * (currentRoundCounts - 9));
        }

        child.transform.localPosition = new Vector3(newX, newY, 0f);

        var newText = child.AddComponent<Text>();
        newText.text = value.ToString();

        Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        newText.font = ArialFont;
        newText.fontSize = 40;
        newText.material = ArialFont.material;
        newText.color = Color.black;
        newText.alignment = TextAnchor.UpperCenter;
    }

    // Every Game adds some points, if not involved add 0
    public void AddPointsToWind(int points, Wind wind)
    {
        if (wind == Wind.East)
        {
            EastWindPoints.Add(points);
            UpdateWindPoints(eastPointsObject, points, EastWindPoints.Count);
        }
        else if (wind == Wind.South)
        {
            SouthWindPoints.Add(points);
            UpdateWindPoints(southPointsObject, points, SouthWindPoints.Count);
        }
        else if (wind == Wind.West)
        {
            WestWindPoints.Add(points);
            UpdateWindPoints(westPointsObject, points, WestWindPoints.Count);
        }
        else
        {
            NorthWindPoints.Add(points);
            UpdateWindPoints(northPointsObject, points, NorthWindPoints.Count);
        }
    }

    // Highlighted or not
    public void IsHighlighted(bool highlighted)
    {
        if (highlighted)
            playerName.color = Color.blue;
        else
            playerName.color = Color.black;
    }

    // Total prints out the total for a wind
    public void ShowTotalForWind(Wind wind)
    {
        if (wind == Wind.East)
            eastPointsTotal.text = "(" + Sum(EastWindPoints).ToString() + ")";
        else if (wind == Wind.South)
            southPointsTotal.text = "(" + (Sum(EastWindPoints) + Sum(SouthWindPoints)).ToString() + ")";
        else if (wind == Wind.West)
            westPointsTotal.text = "(" + (Sum(EastWindPoints) + Sum(SouthWindPoints) + Sum(WestWindPoints)).ToString() + ")";
        else
            northPointsTotal.text = (Sum(EastWindPoints) + Sum(SouthWindPoints) + Sum(WestWindPoints) + Sum(NorthWindPoints)).ToString();
    }
    public void ResetWind(Wind wind)
    {
        if (wind == Wind.East)
            eastPointsTotal.text = "";
        else if (wind == Wind.South)
            southPointsTotal.text = "";
        else if (wind == Wind.West)
            westPointsTotal.text = "";
        else
            northPointsTotal.text = "";
    }
    private int Sum(List<int> list)
    {
        int result = 0;

        foreach (int number in list)
            result += number;

        return result;
    }
}

public enum Wind
{
    East,
    South,
    West,
    North,
    End
}
