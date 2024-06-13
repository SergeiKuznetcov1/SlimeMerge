using UnityEngine;
using System;

public class EndGame : MonoBehaviour
{
    public GameObject BoosterBGImage;
    public GameObject BoosterHolder;
    public GameObject SlimesParent;
    public float TimeToEndGame;
    private float _curTimeToEndGame;
    private bool _gameEnded;
    public static Action OnGameEnd;
    private void Update() {
        if (_gameEnded == true) return;
        float curHighestPosition = 0;
        foreach (Transform slime in SlimesParent.transform) {
            if (slime.position.y >= curHighestPosition && slime.GetComponent<Slime>().CanEndGame == true) {
                curHighestPosition = slime.position.y;
            }
        }
        if (curHighestPosition >= 2.6f) {
            _curTimeToEndGame -= Time.deltaTime;
            if (_curTimeToEndGame < 0) {
                Debug.Log("game Ended");
                _gameEnded = true;
                OnGameEnd?.Invoke();
                BoosterBGImage.SetActive(false);
                BoosterHolder.SetActive(false);
            }
        }
        else {
            _curTimeToEndGame = TimeToEndGame;
        }
    }
}
