using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using UnityEngine.EventSystems;

public class SlimeManager : MonoBehaviour
{
    public Sprite[] SpawnSlimeSprites;
    public Image NextSlimeImage;
    public Transform SlimeParent;
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
    public static Action OnBoosterEnd;
    public static Action OnIncreaseEnd;
    private int _nextSlimeIndex;
    private void Start() {
        _camera = Camera.main;
        CalculateNextSlime();
        
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
            if (IsPointerOverUIObject() == false) {
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
            _currentSlime = Instantiate(SlimePool[_nextSlimeIndex], GetClickedWorldPos(), Quaternion.identity, SlimeParent);
            CalculateNextSlime();
        }
        else if (BoosterManager.IncreaseSizeActive == true) {
            _currentSlime = Instantiate(IncreaseSizePrefab, GetClickedWorldPos(), Quaternion.identity);
        }
        _currentSlime.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        Line.SetActive(true);
    }

    private void CalculateNextSlime() {
        int randomNumber = Random.Range(0, 100);
        if (randomNumber >= 0 && randomNumber <= 40) {
            _nextSlimeIndex = 0;
        }
        else if (randomNumber >= 41 && randomNumber <= 60) {
            _nextSlimeIndex = 1;
        }
        else if (randomNumber >= 61 && randomNumber <= 75) {
            _nextSlimeIndex = 2;
        }
        else if (randomNumber >= 76 && randomNumber <= 90) {
            _nextSlimeIndex = 3;
        }
        else if (randomNumber >= 91) {
            _nextSlimeIndex = 4;
        }
        NextSlimeImage.sprite = SpawnSlimeSprites[_nextSlimeIndex];
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
        if (BoosterManager.IncreaseSizeActive == false && _currentSlime.GetComponent<Slime>() != null) {
            _currentSlime.GetComponent<Slime>().ActivateSlimeColliders();
        }
        else if (BoosterManager.IncreaseSizeActive == true) {
            _currentSlime.GetComponent<Collider2D>().enabled = true;
            BoosterManager.IncreaseSizeActive = false;
            OnIncreaseEnd?.Invoke();
            OnBoosterEnd?.Invoke();
        }
        _currentSlime = null;
    }

    private Vector2 GetClickedWorldPos() {
        Vector2 spawnPos = _camera.ScreenToWorldPoint(Input.mousePosition);
        spawnPos.y = SpawnYPos;
        spawnPos.x = Mathf.Clamp(spawnPos.x, -1.2f, 1.2f);
        return spawnPos;
    }

     private bool IsPointerOverUIObject() 
     {
        PointerEventData eventDataCurrentPosition = new(EventSystem.current)
        {
            position = new Vector2(Input.mousePosition.x, Input.mousePosition.y)
        };
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
     }
}
