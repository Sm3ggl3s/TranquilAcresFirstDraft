using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundManager : MonoBehaviour {

    [Header ("GameObject Settings")]
    public List<GameObject> GroundPrefabTypes;

    [Header ("Raycast Settings")]
    public GameObject playerLookIndicator;
    private RaycastHit _hit;
    public List<Vector3> raycastOffsets;

    [Header ("Layer Settings")]
    public LayerMask groundLayer;

    [Header ("Ground Tiles Settings")]
    private List<GameObject> hitPrefabs;
    private List<Vector3> prefabHitPositions;

    private void Start() {
        hitPrefabs = new List<GameObject>();
        prefabHitPositions = new List<Vector3>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Debug.Log("Space key was pressed!");
            Get4GroundPrefabs();
        }
    }

    private void Get4GroundPrefabs() {
        // Clear lists prior to repopulating them
        hitPrefabs.Clear();
        prefabHitPositions.Clear();

        if (playerLookIndicator == null) {
            Debug.LogWarning("playerLookIndicator is not instantiated.");
            return;
        }

        foreach (var offset in raycastOffsets) { 
            Vector3 raycastOrigin = playerLookIndicator.transform.position + offset;
            raycastOrigin.y = playerLookIndicator.transform.position.y + 0.1f; // Ensure the y-position is correct
            Vector3 raycastDirection = Vector3.down;

             // Log the raycast origin for debugging
            Debug.Log("Raycast origin: " + raycastOrigin);

            if (Physics.Raycast(raycastOrigin, raycastDirection, out _hit, 100f, groundLayer)) {
                hitPrefabs.Add(_hit.collider.gameObject);
                prefabHitPositions.Add(_hit.collider.gameObject.transform.position);
                Debug.Log("Hit prefab: " + _hit.collider.gameObject.name + " at position: " + _hit.collider.gameObject.transform.position);
            } else {
                Debug.Log("No hit at offset: " + offset);   
            }
        }
    }

    // private void GroundPrefabSwitch() {
    //     Debug.Log("Ground Prefab Switch!");
    //     if (hitPrefabs.Count == 4) {
    //         if (hitPrefabs[0] == hitPrefabs[1] && hitPrefabs[1] == hitPrefabs[2] && hitPrefabs[2] == hitPrefabs[3]) {
    //             Debug.Log("All 4 corners are the same ground prefab!");

    //             // Delete the old ground prefab
    //             foreach (GameObject hitPrefab in hitPrefabs) {
    //                 Destroy(hitPrefab);
    //             }

    //             // // Instantiate a new ground prefab
    //             foreach (Vector3 hitPosition in hitPositions) {
    //                 Instantiate(GroundPrefabTypes[0], hitPosition, Quaternion.identity);
    //             }
    //         } else {
    //             Debug.Log("All 4 corners are not the same ground prefab!");
    //         }
    //     }
    // }
    
}
