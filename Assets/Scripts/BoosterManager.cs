using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterManager : MonoBehaviour
{
    public Transform MissleSpawnPos;
    public GameObject MisslePrefab;
    public static bool MissleActive = false;
    public static bool IncreaseSizeActive = false;
    public static bool MagicStickActive = false;
    private GameObject[] _deleteSlimes = {null, null};
    private int _deleteSlimesInt;

    private void OnEnable() {
        Slime.OnSlimeTap += ManageBoosters;
    }

    private void OnDisable() {
        Slime.OnSlimeTap -= ManageBoosters;
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            MissleActive = true;
            SlimeManager.BoosterActive = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            IncreaseSizeActive = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            MagicStickActive = true;
            SlimeManager.BoosterActive = true;
        }
    }

    private void ManageBoosters(GameObject tappedSlime) {
        if (MissleActive == true) {
            LaunchMissle(tappedSlime);
        }
        else if (MagicStickActive == true) {
            if (_deleteSlimesInt == 0) {
                _deleteSlimes[0] = tappedSlime;
                Debug.Log(_deleteSlimes[0] + "0");
                _deleteSlimesInt += 1;
            }
            else if (_deleteSlimes[0] != tappedSlime) {
                _deleteSlimes[1] = tappedSlime;
                Debug.Log(_deleteSlimes[1] + "1");
                _deleteSlimesInt += 1;
            }

            if (_deleteSlimesInt == 2) {
                Destroy(_deleteSlimes[0]);
                Destroy(_deleteSlimes[1]);
                _deleteSlimesInt = 0;
                // MagicStickActive = false;
                // SlimeManager.BoosterActive = false;
                StartCoroutine(nameof(DeactivateMagicStick));
                return;
            }
        }
    }
    private IEnumerator DeactivateMagicStick() {
        yield return null;
        MagicStickActive = false;
        SlimeManager.BoosterActive = false;
    }
    public void LaunchMissle(GameObject target)
    {
        GameObject missle = Instantiate(MisslePrefab, MissleSpawnPos.position, Quaternion.identity);
        missle.GetComponent<Missle>().Target = target;
        // SlimeManager.BoosterActive = false;
        // MissleActive = false;
    }
}
