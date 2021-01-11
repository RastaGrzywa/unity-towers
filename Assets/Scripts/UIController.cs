using UnityEngine;
using UnityEngine.UI;


public class UIController : MonoBehaviour
{
    public Text towerText;

    private int _towerAmount = 0;

    public void UpdateTowerText()
    {
        towerText.text = "Wieżyczki: " + _towerAmount;
    }

    public void AddTowers(int towers)
    {
        _towerAmount += towers;
        UpdateTowerText();
    }

    public void SubtractTowers(int towers)
    {
        _towerAmount -= towers;
        UpdateTowerText();
    }
}