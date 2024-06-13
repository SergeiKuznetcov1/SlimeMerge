using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLineActivator : MonoBehaviour
{
    public GameObject EndLine;
    public GameObject SlimesParent;
    public float TimeToShowLine;
    private float _curTimeToShowLine;
    private void Update() {
        float curHighestPosition = 0;
        foreach (Transform slime in SlimesParent.transform) {
            if (slime.position.y >= curHighestPosition && slime.GetComponent<Slime>().CanEndGame == true) {
                curHighestPosition = slime.position.y;
            }
        }
        if (curHighestPosition >= 0.1f) {
            _curTimeToShowLine -= Time.deltaTime;
            if (_curTimeToShowLine < 0) {
                EndLine.SetActive(true);
            }
        }
        else {
            _curTimeToShowLine = TimeToShowLine;
            EndLine.SetActive(false);
        }
    }
}
