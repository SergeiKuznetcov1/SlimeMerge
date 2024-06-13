using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MergeManager : MonoBehaviour
{
    public Transform SlimeParent;
    public GameObject[] AllSlimes;
    private void OnEnable() {
        Slime.OnSlimeCollision += ManageCollision;
    }

    private void OnDisable() {
        Slime.OnSlimeCollision -= ManageCollision;
    }

    private void ManageCollision(Vector2 vector, int arg2)
    {
        if (arg2 < AllSlimes.Length) {
            GameObject slime = Instantiate(AllSlimes[arg2], vector, Quaternion.identity, SlimeParent);
            slime.GetComponent<Slime>().ActivateSlimeColliders();
        }
        
    }
}
