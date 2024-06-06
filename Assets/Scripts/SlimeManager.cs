using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.EventSystems;

public class SlimeManager : MonoBehaviour
{
    public GameObject IncreaseSizePrefab;
    public GameObject[] SlimePool;
    public GameObject SlimePrefab;
    public GameObject Line;
    public float SpawnDelay;
    private float _currentSpawnDelay;
    private bool _startDelayCountdown = false;
    public float SpawnYPos;
    private Camera _camera;
    private GameObject _currentSlime;
    public static bool BoosterActive;
    private void Start() {
        _camera = Camera.main;
    }

    private void Update() {
        ManagePlayerInput();
    }

    private void ManagePlayerInput()
    {
        if (_startDelayCountdown == true) {
            _currentSpawnDelay -= Time.deltaTime;
        }
        if (Input.GetMouseButtonDown(0)) {
            if (EventSystem.current.IsPointerOverGameObject() == false) {
                MouseDownCallback();
            }
        }
        else if (Input.GetMouseButton(0)) {
            MouseButtonCallback();
        }
        else if (Input.GetMouseButtonUp(0)) {
            MouseUpCallback();
        }
    }
    
    private void MouseDownCallback()
    {
        if (_currentSpawnDelay > 0) return;
        if (BoosterActive == true) return;
        _currentSpawnDelay = SpawnDelay;
        _startDelayCountdown = false;
        if (BoosterManager.IncreaseSizeActive == false) {
            _currentSlime = Instantiate(SlimePool[Random.Range(0, SlimePool.Length)], GetClickedWorldPos(), Quaternion.identity);
        }
        else if (BoosterManager.IncreaseSizeActive == true) {
            _currentSlime = Instantiate(IncreaseSizePrefab, GetClickedWorldPos(), Quaternion.identity);
        }
        _currentSlime.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        Line.SetActive(true);
    }

    private void MouseButtonCallback()
    {
        if (_currentSlime == null) return;
        if (BoosterActive == true) return;
        _currentSlime.transform.position = GetClickedWorldPos();
        Line.transform.position = GetClickedWorldPos();
    }

    private void MouseUpCallback()
    {
        if (_currentSlime == null) return;
        if (BoosterActive == true) return;
        _startDelayCountdown = true;
        _currentSlime.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        Line.SetActive(false);
        if (BoosterManager.IncreaseSizeActive == false) {
            _currentSlime.GetComponent<Slime>().ActivateSlimeColliders();
        }
        else if (BoosterManager.IncreaseSizeActive == true) {
            _currentSlime.GetComponent<Collider2D>().enabled = true;
            BoosterManager.IncreaseSizeActive = false;
        }
        _currentSlime = null;
    }

    private Vector2 GetClickedWorldPos() {
        Vector2 spawnPos = _camera.ScreenToWorldPoint(Input.mousePosition);
        spawnPos.y = SpawnYPos;
        return spawnPos;
    }

    public static void InstantiateSlimeAfterCollision(Vector2 vector, GameObject type) {

    }
}
