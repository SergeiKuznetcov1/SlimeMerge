using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missle : MonoBehaviour
{
    public float ExplosionRadius;
    private Collider2D[] _collider2Ds;
    public GameObject Target;
    public float MissleSpeed;
    public GameObject ExplosionFX;
    public static Action OnReduceMissleAmount;
    private void Update() {
        if (Target != null) {
            transform.position = Vector2.MoveTowards(transform.position, Target.transform.position, Time.deltaTime * MissleSpeed);
            transform.up = Target.transform.position - transform.position;
        }
        if (Target == null) {
            Explode();
        }
        else if (Mathf.Approximately(Vector2.Distance(transform.position, Target.transform.position), 0)) {
            Explode();
        }
    }

    private void Explode()
    {
        _collider2Ds = Physics2D.OverlapCircleAll(transform.position, ExplosionRadius);
        foreach (Collider2D col in _collider2Ds) {
            if (col.GetComponent<Slime>() != null) {
                int coinsToAdd = 0;
                if ((int)col.GetComponent<Slime>().SlimeType == 0) {
                    coinsToAdd = 2;
                }
                else if ((int)col.GetComponent<Slime>().SlimeType == 1) {
                    coinsToAdd = 4;
                }
                else if ((int)col.GetComponent<Slime>().SlimeType == 2) {
                    coinsToAdd = 8;
                }
                else if ((int)col.GetComponent<Slime>().SlimeType == 3) {
                    coinsToAdd = 16;
                }
                else if ((int)col.GetComponent<Slime>().SlimeType == 4) {
                    coinsToAdd = 32;
                }
                else if ((int)col.GetComponent<Slime>().SlimeType == 5) {
                    coinsToAdd = 64;
                }
                else if ((int)col.GetComponent<Slime>().SlimeType == 6) {
                    coinsToAdd = 128;
                }
                Slime.OnSlimeDestroy(coinsToAdd);
                // Slime.OnSlimeDestroy((int)col.GetComponent<Slime>().SlimeType + 1);
                Destroy(col.gameObject);
            }
        }
        Instantiate(ExplosionFX, transform.position, Quaternion.identity);
        OnReduceMissleAmount?.Invoke();
        SlimeManager.BoosterActive = false;
        BoosterManager.MissleActive = false;
        SlimeManager.OnBoosterEnd?.Invoke();
        BoosterManager.MissleFired = false;
        Destroy(gameObject);
    }
}
