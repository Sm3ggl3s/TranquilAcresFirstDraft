using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour {

    [Header("GameObject Settings")]
    public GameObject playerLookIndicatorPrefab;
    private GameObject playerLookIndicator;

    [Header("Raycast Settings")]
    public GameObject raycastOriginObject;
    private Ray playerLookIndicatorRay;
    private RaycastHit _hit;

    [Header("Layer Settings")]
    public LayerMask groundLayer;

    [Header("Grid Settings")]
    public float cellSize;
    public Vector2 gridOffset;

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
        Vector3 origin = raycastOriginObject.transform.position;
        origin.y = 0.01f;
        playerLookIndicatorRay = new Ray(origin, Vector3.down);
        if (Physics.Raycast(playerLookIndicatorRay, out _hit, 1000f, groundLayer)) {            
            
            if (!playerLookIndicator.activeSelf) {
                playerLookIndicator.SetActive(true);
            }
            Vector3 hitPoint = _hit.point;
            playerLookIndicator.transform.position = ClampToNearest(hitPoint, cellSize);
        } else {
            Debug.Log("Raycast did not hit the ground layer!");
        }
    }

    private Vector3 ClampToNearest(Vector3 position, float threshold) {
        float t = 1f / threshold;
        Vector3 v = ((Vector3)Vector3Int.FloorToInt(position * t)) / t;

        // Offset to center of cell
        float s = threshold / 2.0f;
        v.x += s + gridOffset.x;
        v.z += s + gridOffset.y;
        v.y = -0.5f;

        return v;
    }
}
