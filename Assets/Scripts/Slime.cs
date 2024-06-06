using System;
using System.Collections;
using System.Collections.Generic;
using SoftBody2D;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slime : MonoBehaviour
{
    public GameObject[] AllSlimesPrefabs;
    public SlimeType SlimeType;
    public GameObject[] SlimeColliders;
    public static Action<Vector2, int> OnSlimeCollision;
    public static Action<GameObject> OnSlimeTap;
    public float IncreaseSizeSpeed;
    public void ActivateSlimeColliders() {
        foreach (GameObject slimeCollider in SlimeColliders) {
            slimeCollider.SetActive(true);
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
        if (otherSlime != null && otherSlime.SlimeType == SlimeType) {
            Vector2 spawnPos = (other.transform.position + transform.position) / 2;
            SlimeType += 1;
            OnSlimeCollision?.Invoke(spawnPos, (int)SlimeType);
            Destroy(gameObject);
            Destroy(other.gameObject);
        }
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
