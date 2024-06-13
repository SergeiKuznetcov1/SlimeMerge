using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static void SetBoosterShowenInfo(int boosterIndex) {
        PlayerPrefs.SetInt($"Booster{boosterIndex}InfoShowen", 1);
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Backspace)) {
            PlayerPrefs.DeleteAll();
        }
    }

    public static int GetBestScore() {
        return PlayerPrefs.GetInt("BestScore", 0);
    }

    public static void SetBestScore(int score) {
        PlayerPrefs.SetInt("BestScore", score);
    }

    public static int GetMissleAmount() {
        return PlayerPrefs.GetInt("MissleAmount", 1);
    }

    public static void SetMissleAmount(int MissleAmount) {
        PlayerPrefs.SetInt("MissleAmount", GetMissleAmount() + MissleAmount);
    }

    public static int GetMagicStickAmount() {
        return PlayerPrefs.GetInt("MagicStickAmount", 1);
    }

    public static void SetMagicStickAmount(int MagicStickAmount) {
        PlayerPrefs.SetInt("MagicStickAmount", GetMagicStickAmount() + MagicStickAmount);
    }

    public static int GetIncreaseSizeAmount() {
        return PlayerPrefs.GetInt("IncreaseSizeAmount", 1);
    }

    public static void SetIncreaseSizeAmount(int IncreaseSizeAmount) {
        PlayerPrefs.SetInt("IncreaseSizeAmount", GetIncreaseSizeAmount() + IncreaseSizeAmount);
    }
}
