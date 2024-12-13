using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Resources : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI foodAmountBox;
    [SerializeField] private AudioSource gainFoodAudio;

    public int food;
    private void Awake()
    {
        UpdateFoodUI();
    }
    private void UpdateFoodUI()
    {
        foodAmountBox.text = food.ToString();
    }

    public void GainFood(int amount)
    {
        gainFoodAudio.Play();
        food += amount;
        UpdateFoodUI();
    }

    public void SpendFood(int amount)
    {
        food -= amount;
        UpdateFoodUI();
    }
}
