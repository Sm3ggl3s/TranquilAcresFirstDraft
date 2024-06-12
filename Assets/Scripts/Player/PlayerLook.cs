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
    public List<Vector3> raycastOffsets;

    [Header("Layer Settings")]
    public LayerMask groundLayer;

    [Header("Grid Settings")]
    public float cellSize;
    public Vector2 gridOffset;

    private List<string> hitPrefabNames;

    private void Start() {
        if (playerLookIndicatorPrefab == null) {
            Debug.LogError("Player Look Indicator Prefab is not set!");
            return;
        } else {
            playerLookIndicator = Instantiate(playerLookIndicatorPrefab);
            playerLookIndicator.SetActive(false);
        }

        hitPrefabNames = new List<string>();
    }

    private void Update() {
        hitPrefabNames.Clear();

        if (playerLookIndicator == null) {
            Debug.LogError("Player Look Indicator is not instantiated!");
            return;
        }

        playerLookIndicatorRay = new Ray(raycastOriginObject.transform.position, Vector3.down);
        if (Physics.Raycast(playerLookIndicatorRay, out _hit, 1000f, groundLayer)) {            
            Debug.Log("Hit Floor: " + _hit.collider.gameObject.name);
            Debug.Log("Hit Point: " + _hit.point);
            
            if (!playerLookIndicator.activeSelf) {
                playerLookIndicator.SetActive(true);
            }

            Vector3 hitPoint = _hit.point;
            playerLookIndicator.transform.position = ClampToNearest(hitPoint, cellSize);
            Debug.Log("Player Look Indicator Position: " + playerLookIndicator.transform.position);
        } else {
            Debug.Log("Raycast did not hit the ground layer!");
        }

        Grab4Corners();
    }


    private Vector3 ClampToNearest(Vector3 position, float threshold) {
        float t = 1f / threshold;
        Vector3 v = ((Vector3)Vector3Int.FloorToInt(position * t)) / t;

        // Offset to center of cell
        float s = threshold / 2.0f;
        v.x += s + gridOffset.x;
        v.z += s + gridOffset.y;

        return v;
    }

    private void Grab4Corners() {
        foreach (Vector3 offset in raycastOffsets) {
            Vector3 origin = playerLookIndicator.transform.position + offset;
            origin.y = 0.01f;
            Ray ray = new Ray(origin, Vector3.down);
            if (Physics.Raycast(ray, out _hit, 1000f, groundLayer)) {
                hitPrefabNames.Add(_hit.collider.gameObject.name);
                Debug.Log("Hit: " + _hit.collider.gameObject.name);
                if (Input.GetKeyDown(KeyCode.Space)) {
                    Destroy(_hit.collider.gameObject);
                }
            }
        }

        if (hitPrefabNames.Count == 4) {
            Debug.Log("All 4 corners hit the ground layer!");
        }
    }
}
