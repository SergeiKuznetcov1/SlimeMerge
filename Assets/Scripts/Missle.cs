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
                Destroy(col.gameObject);
            }
        }
        Instantiate(ExplosionFX, transform.position, Quaternion.identity);
        SlimeManager.BoosterActive = false;
        BoosterManager.MissleActive = false;
        Destroy(gameObject);
    }
}
