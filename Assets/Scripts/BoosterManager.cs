using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
// using YG;

public class BoosterManager : MonoBehaviour
{
    public Animator InfoAnimator;
    public TMP_Text InfoText;
    public static bool MissleFired;
    public Transform MissleSpawnPos;
    public GameObject MisslePrefab;
    public static bool MissleActive = false;
    public static int MissleAmount = 1;
    public GameObject MissleAmountInfo;
    public TMP_Text MissleAmountText;
    public GameObject MissleVideoIcon;
    public static bool MagicStickActive = false;
    public static int MagicStickAmount = 1;
    public GameObject MagicStickAmountInfo;
    public TMP_Text MagicStickAmountText;
    public GameObject MagicStickVideoIcon;
    public static bool IncreaseSizeActive = false;
    public static int IncreaseSizeAmount = 1;
    public GameObject IncreaseSizeAmountInfo;
    public TMP_Text IncreaseSizeText;
    public GameObject IncreaseSizeVideoIcon;
    public GameObject CancelBoosterBtn;
    private GameObject[] _deleteSlimes = {null, null};
    private int _deleteSlimesInt;
    public static bool RewardSuccess;
    private int _rewardIndex;

    private void OnEnable() {
        // YandexGame.CloseVideoEvent += Rewarded;
        Missle.OnReduceMissleAmount += HandleReduceMissle;
        Slime.OnSlimeTap += ManageBoosters;
        SlimeManager.OnBoosterEnd += CancelActiveBooster;
        SlimeManager.OnIncreaseEnd += HandleIncreaseEnd;
    }

    private void OnDisable() {
        // YandexGame.CloseVideoEvent -= Rewarded;
        Missle.OnReduceMissleAmount -= HandleReduceMissle;
        Slime.OnSlimeTap -= ManageBoosters;
        SlimeManager.OnBoosterEnd -= CancelActiveBooster;
        SlimeManager.OnIncreaseEnd -= HandleIncreaseEnd;
    }

    private void Start() {
        SetupBoosters();
    }
    
    private void SetupBoosters() {
        int missle = SaveSystem.GetMissleAmount();
        int stick = SaveSystem.GetMagicStickAmount();
        int increase = SaveSystem.GetIncreaseSizeAmount();
        if (missle > 0) {
            MissleAmountInfo.SetActive(true);
            MissleVideoIcon.SetActive(false);
        }
        else {
            MissleAmountInfo.SetActive(false);
            MissleVideoIcon.SetActive(true);
        }
        if (stick > 0) {
            MagicStickAmountInfo.SetActive(true);
            MagicStickVideoIcon.SetActive(false);
        }
        else {
            MagicStickAmountInfo.SetActive(false);
            MagicStickVideoIcon.SetActive(true);
        }
        if (increase > 0) {
            IncreaseSizeAmountInfo.SetActive(true);
            IncreaseSizeVideoIcon.SetActive(false);
        }
        else {
            IncreaseSizeAmountInfo.SetActive(false);
            IncreaseSizeVideoIcon.SetActive(true);
        }
    }

    private void HandleIncreaseEnd()
    {
        SaveSystem.SetIncreaseSizeAmount(-1);
        IncreaseSizeAmountInfo.SetActive(false);
        IncreaseSizeVideoIcon.SetActive(true);
    }

    private void HandleReduceMissle()
    {
        SaveSystem.SetMissleAmount(-1);
        MissleAmountInfo.SetActive(false);
        MissleVideoIcon.SetActive(true);
    }

    private void ManageBoosters(GameObject tappedSlime) {
        if (MissleActive == true) {
            LaunchMissle(tappedSlime);
        }
        else if (MagicStickActive == true) {
            if (_deleteSlimesInt == 0) {
                _deleteSlimes[0] = tappedSlime;
                _deleteSlimesInt += 1;
            }
            else if (_deleteSlimes[0] != tappedSlime) {
                _deleteSlimes[1] = tappedSlime;
                _deleteSlimesInt += 1;
            }

            if (_deleteSlimesInt == 2) {
                int coinsToAdd = 0;
                if ((int)_deleteSlimes[0].GetComponent<Slime>().SlimeType == 0) {
                    coinsToAdd = 2;
                }
                else if ((int)_deleteSlimes[0].GetComponent<Slime>().SlimeType == 1) {
                    coinsToAdd = 4;
                }
                else if ((int)_deleteSlimes[0].GetComponent<Slime>().SlimeType == 2) {
                    coinsToAdd = 8;
                }
                else if ((int)_deleteSlimes[0].GetComponent<Slime>().SlimeType == 3) {
                    coinsToAdd = 16;
                }
                else if ((int)_deleteSlimes[0].GetComponent<Slime>().SlimeType == 4) {
                    coinsToAdd = 32;
                }
                else if ((int)_deleteSlimes[0].GetComponent<Slime>().SlimeType == 5) {
                    coinsToAdd = 64;
                }
                else if ((int)_deleteSlimes[0].GetComponent<Slime>().SlimeType == 6) {
                    coinsToAdd = 128;
                }
                Slime.OnSlimeDestroy(coinsToAdd);
                if ((int)_deleteSlimes[1].GetComponent<Slime>().SlimeType == 0) {
                    coinsToAdd = 2;
                }
                else if ((int)_deleteSlimes[1].GetComponent<Slime>().SlimeType == 1) {
                    coinsToAdd = 4;
                }
                else if ((int)_deleteSlimes[1].GetComponent<Slime>().SlimeType == 2) {
                    coinsToAdd = 8;
                }
                else if ((int)_deleteSlimes[1].GetComponent<Slime>().SlimeType == 3) {
                    coinsToAdd = 16;
                }
                else if ((int)_deleteSlimes[1].GetComponent<Slime>().SlimeType == 4) {
                    coinsToAdd = 32;
                }
                else if ((int)_deleteSlimes[1].GetComponent<Slime>().SlimeType == 5) {
                    coinsToAdd = 64;
                }
                else if ((int)_deleteSlimes[1].GetComponent<Slime>().SlimeType == 6) {
                    coinsToAdd = 128;
                }
                Slime.OnSlimeDestroy(coinsToAdd);
                Destroy(_deleteSlimes[0]);
                Destroy(_deleteSlimes[1]);
                _deleteSlimesInt = 0;
                StartCoroutine(nameof(DeactivateMagicStick));
                return;
            }
        }
    }

    private IEnumerator DeactivateMagicStick() {
        yield return null;
        MagicStickActive = false;
        SlimeManager.BoosterActive = false;
        SaveSystem.SetMagicStickAmount(-1);
        MagicStickAmountInfo.SetActive(false);
        MagicStickVideoIcon.SetActive(true);
        CancelActiveBooster();
    }

    public void LaunchMissle(GameObject target)
    {
        if (MissleFired == false) {
            GameObject missle = Instantiate(MisslePrefab, MissleSpawnPos.position, Quaternion.identity);
            missle.GetComponent<Missle>().Target = target;
            MissleFired = true;
        }
    }

    public void StartBoosterClickedSequence(int boosterIndex) {
        MissleActive = false;
        MagicStickActive = false;
        _deleteSlimes[0] = null;
        _deleteSlimesInt = 0;
        IncreaseSizeActive = false;
        SlimeManager.BoosterActive = false;
        if (boosterIndex == 0) {
            if (SaveSystem.GetMissleAmount() >= 1) {
                MissleActive = true;
                SlimeManager.BoosterActive = true;
                CancelBoosterBtn.SetActive(true);
                if (PlayerPrefs.GetInt("Booster0InfoShowen") == 0) {
                    SaveSystem.SetBoosterShowenInfo(0);
                    InfoAnimator.SetTrigger("ShowInfo");
                    // if (YandexGame.EnvironmentData.language == "ru") {
                    //     InfoText.text = "Выбери слайм и запусти ракету!";
                    // }
                    // else {
                    //     InfoText.text = "Pick slime and launch rocket!";
                    // }
                    InfoText.text = "Выбери слайм и запусти ракету!";
                }
            }
            else {
                _rewardIndex = 0;
                // YandexGame.RewVideoShow(0);
            }
        }
        if (boosterIndex == 1) {
            if (SaveSystem.GetMagicStickAmount() >= 1) {
                MagicStickActive = true;
                SlimeManager.BoosterActive = true;
                CancelBoosterBtn.SetActive(true);
                if (PlayerPrefs.GetInt("Booster1InfoShowen") == 0) {
                    SaveSystem.SetBoosterShowenInfo(1);
                    InfoAnimator.SetTrigger("ShowInfo");
                    // if (YandexGame.EnvironmentData.language == "ru") {
                    //     InfoText.text = "Выбери и удали два разных слайма!";
                    // }
                    // else {
                    //     InfoText.text = "Pick and delete two different slimes!";
                    // }
                    InfoText.text = "Выбери и удали два разных слайма!";
                }
            }
            else {
                _rewardIndex = 1;
                // YandexGame.RewVideoShow(0);
            }
        }
        if (boosterIndex == 2) {
            if (SaveSystem.GetIncreaseSizeAmount() >= 1) {
                IncreaseSizeActive = true;
                CancelBoosterBtn.SetActive(true);
                if (PlayerPrefs.GetInt("Booster2InfoShowen") == 0) {
                    SaveSystem.SetBoosterShowenInfo(2);
                    InfoAnimator.SetTrigger("ShowInfo");
                    // if (YandexGame.EnvironmentData.language == "ru") {
                    //     InfoText.text = "Сбрось шар улучшающий слайм!";
                    // }
                    // else {
                    //     InfoText.text = "Drop ball improving slime!";
                    // }
                    InfoText.text = "Сбрось шар улучшающий слайм!";
                }
            }
            else {
                _rewardIndex = 2;
                // YandexGame.RewVideoShow(0);
            }
        }
    }

    public void CancelActiveBooster() {
        MissleActive = false;
        MagicStickActive = false;
        _deleteSlimes[0] = null;
        _deleteSlimesInt = 0;
        IncreaseSizeActive = false;
        SlimeManager.BoosterActive = false;
        CancelBoosterBtn.SetActive(false);
    }

    private void Rewarded()
    {
        if (RewardSuccess) {
            if (_rewardIndex == 0) {
                SaveSystem.SetMissleAmount(1);
                MissleAmountInfo.SetActive(true);
                MissleVideoIcon.SetActive(false);
            }
            if (_rewardIndex == 1) {
                SaveSystem.SetMagicStickAmount(1);
                MagicStickAmountInfo.SetActive(true);
                MagicStickVideoIcon.SetActive(false);
            }
            if (_rewardIndex == 2) {
                SaveSystem.SetIncreaseSizeAmount(1);
                IncreaseSizeAmountInfo.SetActive(true);
                IncreaseSizeVideoIcon.SetActive(false);
            }
        }
        RewardSuccess = false;
    }
}
