using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour {

    [Header("GameObject Settings")]
    public GameObject playerLookIndicatorPrefab;
    private GameObject playerLookIndicator;

    [Header("Raycast Settings")]
    public GameObject raycastOriginObject;
    private Ray _ray;
    private RaycastHit _hit;

    [Header("Layer Settings")]
    public LayerMask groundLayer;

    private void Start() {
        if (playerLookIndicatorPrefab == null) {
            Debug.LogError("Player Look Indicator Prefab is not set!");
            return;
        } else {
            playerLookIndicator = Instantiate(playerLookIndicatorPrefab);
            playerLookIndicator.SetActive(false);
        }
    }

    private void Update() {
        if (playerLookIndicator == null) {
            Debug.LogError("Player Look Indicator is not instantiated!");
            return;
        }

        _ray = new Ray(raycastOriginObject.transform.position, Vector3.down);
        if (Physics.Raycast(_ray, out _hit, 1000f, groundLayer)) {            
            Debug.Log("Hit Floor: " + _hit.collider.gameObject.name);
            Debug.Log("Hit Point: " + _hit.point);
            
            if (!playerLookIndicator.activeSelf) {
                playerLookIndicator.SetActive(true);
            }

            playerLookIndicator.transform.position = _hit.point;
            Debug.Log("Player Look Indicator Position: " + playerLookIndicator.transform.position);
        } else {
            Debug.Log("Raycast did not hit the ground layer!");
        }
    }
}
