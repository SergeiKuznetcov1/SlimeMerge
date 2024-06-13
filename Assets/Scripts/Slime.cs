using System;
using System.Collections;
using System.Collections.Generic;
using SoftBody2D;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slime : MonoBehaviour
{
    public bool CanEndGame;
    public GameObject[] AllSlimesPrefabs;
    public SlimeType SlimeType;
    public GameObject[] SlimeColliders;
    public CircleCollider2D MainCollider;
    public static Action<Vector2, int> OnSlimeCollision;
    public static Action<GameObject> OnSlimeTap;
    public static Action<int> OnSlimeDestroy;
    public float IncreaseSizeSpeed;
    public void ActivateSlimeColliders() {
        foreach (GameObject slimeCollider in SlimeColliders) {
            slimeCollider.SetActive(true);
            MainCollider.enabled = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.layer == 6) {
            if ((int)SlimeType < 6) {
                Destroy(other.gameObject);
            }
            IncreaseSlimeLevel();
        }
        Slime otherSlime = other.gameObject.GetComponent<Slime>();
        if (otherSlime != null && otherSlime.SlimeType == SlimeType && (int)SlimeType < 6) {
            Vector2 spawnPos = (other.transform.position + transform.position) / 2;
            int coinsToAdd = 0;
            if ((int)SlimeType == 0) {
                coinsToAdd = 4;
            }
            else if ((int)SlimeType == 1) {
                coinsToAdd = 8;
            }
            else if ((int)SlimeType == 2) {
                coinsToAdd = 16;
            }
            else if ((int)SlimeType == 3) {
                coinsToAdd = 32;
            }
            else if ((int)SlimeType == 4) {
                coinsToAdd = 64;
            }
            else if ((int)SlimeType == 5) {
                coinsToAdd = 128;
            }
            OnSlimeDestroy?.Invoke(coinsToAdd);
            //OnSlimeDestroy?.Invoke((int)SlimeType + 1 + (int)other.gameObject.GetComponent<Slime>().SlimeType + 1);
            SlimeType += 1;
            OnSlimeCollision?.Invoke(spawnPos, (int)SlimeType);
            Destroy(gameObject);
            Destroy(other.gameObject);
        }
        CanEndGame = true;
    }

    private void OnMouseDown() {
        if (SlimeManager.BoosterActive == true) {
            OnSlimeTap?.Invoke(gameObject);
        }
    }
    private void IncreaseSlimeLevel() {
        if ((int)SlimeType < 6) {
            SlimeType += 1;
            Destroy(gameObject);
            GameObject slime = Instantiate(AllSlimesPrefabs[(int)SlimeType], transform.position, Quaternion.identity);
            slime.GetComponent<Slime>().ActivateSlimeColliders();
        }
    }
}

public enum SlimeType {
    One, Two, Three, Four, Five, Six, Seven
}
