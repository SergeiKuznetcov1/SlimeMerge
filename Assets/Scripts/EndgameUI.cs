using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndgameUI : MonoBehaviour
{
    public Canvas Canvas;
    public CoinManager CoinManager;
    public Animator EndAnimator;
    public TMP_Text CurrentScore;
    public TMP_Text BestScore;
    public GameObject BackgroundImage;
    private void OnEnable() {
        EndGame.OnGameEnd += SetGameEnd;
    }

    private void OnDisable() {
        EndGame.OnGameEnd -= SetGameEnd;
    }

    private void SetGameEnd()
    {
        Canvas.sortingOrder = 10;
        BackgroundImage.SetActive(true);
        EndAnimator.SetTrigger("ShowEndgame");
        CurrentScore.text = CoinManager.CurrentAmount.ToString();
        if (CoinManager.CurrentAmount > SaveSystem.GetBestScore()) {
            SaveSystem.SetBestScore(CoinManager.CurrentAmount);
            BestScore.text = CoinManager.CurrentAmount.ToString();
        }
        else {
            BestScore.text = SaveSystem.GetBestScore().ToString();
        }
    }

    public void ReloadGame() {
        SceneManager.LoadScene(0);
    }
}
