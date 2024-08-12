using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Resources : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI foodAmountBox;

    public int food;

    private void UpdateFoodUI()
    {
        foodAmountBox.text = food.ToString();
    }

    public void GainFood(int amount)
    {
        food += amount;
        UpdateFoodUI();
    }

    public void SpendFood(int amount)
    {
        food -= amount;
        UpdateFoodUI();
    }
}
