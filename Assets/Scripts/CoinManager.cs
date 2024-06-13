using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
public class CoinManager : MonoBehaviour
{
    public int CurrentAmount;
    public TMP_Text CoinText;
    public TMP_Text LevelText;
    private void OnEnable() {
        Slime.OnSlimeDestroy += IncreaseAmount;
    }

    private void OnDisable() {
        Slime.OnSlimeDestroy -= IncreaseAmount;
    }

    private void IncreaseAmount(int amount)
    {
        CurrentAmount += amount;
        CoinText.text = CurrentAmount.ToString();
        LevelText.text = (CurrentAmount / 100 + 1).ToString();
    }
}
