using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundManager : MonoBehaviour {

    [Header ("GameObject Settings")]
    public List<GameObject> GroundPrefabTypes;

    [Header ("Raycast Settings")]
    public GameObject playerLookIndicator;
    private Ray playerLookIndicatorRay;
    private RaycastHit _hit;
    public List<Vector3> raycastOffsets;

    [Header ("Layer Settings")]
    public LayerMask groundLayer;

    [Header ("Ground Tiles Settings")]
    private List<string> hitPrefabNames;

    private void Start() {
        hitPrefabNames = new List<string>();
    }

    private void Update() {
        hitPrefabNames.Clear();
        Grab4Corners();
    }

    private void Grab4Corners() {
        foreach (Vector3 offset in raycastOffsets) {
            Vector3 origin = playerLookIndicator.transform.position + offset;
            origin.y = 0.01f;
            Ray ray = new Ray(origin, Vector3.down);
            if (Physics.Raycast(ray, out _hit, 1000f, groundLayer)) {
                hitPrefabNames.Add(_hit.collider.gameObject.name);
                Debug.Log("Hit: " + _hit.collider.gameObject.name);
                Debug.Log("Hit Point: " + _hit.point);
            }
        }

        if (hitPrefabNames.Count == 4) {
            Debug.Log("All 4 corners hit the ground layer!");
        }
    }

    private void GroundPrefabSwitch() {
        if (hitPrefabNames.Count == 4) {
            if (hitPrefabNames[0] == hitPrefabNames[1] && hitPrefabNames[1] == hitPrefabNames[2] && hitPrefabNames[2] == hitPrefabNames[3]) {
                Debug.Log("All 4 corners are the same ground prefab!");
            } else {
                Debug.Log("All 4 corners are not the same ground prefab!");
            }
        }
    }
    
}
