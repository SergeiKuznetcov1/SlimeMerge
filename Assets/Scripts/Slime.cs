using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slime : MonoBehaviour
{
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
            // StopCoroutine(nameof(IncreaseSlimeSize));
            // StartCoroutine(nameof(IncreaseSlimeSize));
            IncreaseSlimeSize2();
            Destroy(other.gameObject);
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

    private IEnumerator IncreaseSlimeSize() {
        Vector3 initSize = transform.localScale;
        Vector3 targetSize = initSize * 2;
        while (transform.localScale.x < targetSize.x) {
            transform.localScale = Vector3.Lerp(transform.localScale, targetSize, Time.deltaTime * IncreaseSizeSpeed);
            yield return null;
        }
    }
    private void IncreaseSlimeSize2() {
        Vector3 initSize = transform.localScale;
        transform.localScale = initSize * 2;
    }
}

public enum SlimeType {
    One, Two, Three, Four
}
