using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public GameObject SettingBGBlocker;
    public Animator SettingsAnimator;
    public void ShowSettings() {
        SettingBGBlocker.SetActive(true);
        SettingsAnimator.SetTrigger("ShowSettings");
    }

    public void HideSettings() {
        SettingBGBlocker.SetActive(false);
        SettingsAnimator.SetTrigger("HideSettings");
    }
}
