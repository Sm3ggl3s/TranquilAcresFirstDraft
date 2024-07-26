using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundManager : MonoBehaviour {

    // Reference to PlayerLook Script
    public PlayerLook playerLookScript;

    [Header ("GameObject Settings")]
    public List<GameObject> GroundPrefabTypes;

    [Header ("Raycast Settings")]
    private GameObject playerLookIndicator;
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

        if (playerLookScript != null) {
            playerLookIndicator = playerLookScript.GetPlayerLookIndicator();
            if (playerLookIndicator == null) {
                Debug.LogWarning("playerLookIndicator is null after calling GetPlayerLookIndicator.");
            } else {
                Debug.Log("PlayerLookIndicator: " + playerLookIndicator);
            }
        } else {
            Debug.LogWarning("PlayerLook script is not assigned.");
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Debug.Log("Space key was pressed!");
            Get4GroundPrefabs();
            removePrefabs();
            switchGroundPrefabs();
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

    private void removePrefabs() {
        foreach (var prefab in hitPrefabs) {
            Destroy(prefab);
        }
    } 

    private void switchGroundPrefabs() {
        Debug.Log("Switching ground prefabs...");
        // Validation checks
        // if (hitPrefabs.Count == 0) {
        //     Debug.Log("No prefabs to switch.");
        //     return;
        // }
        
        // if (GroundPrefabTypes.Count == 0) {
        //     Debug.Log("No ground prefabs to switch to.");
        //     return;
        // }

        // if (hitPrefabs.Count != prefabHitPositions.Count) {
        //     Debug.LogWarning("hitPrefabs and prefabHitPositions lists are not the same size.");
        //     return;
        // }
        foreach (var prefab in hitPrefabs) {
            Debug.Log(prefab.name + " at position: " + hitPrefabs.IndexOf(prefab));
        }
        if (hitPrefabs[0].name == hitPrefabs[1].name && hitPrefabs[1].name == hitPrefabs[2].name && hitPrefabs[2].name == hitPrefabs[3].name) {
            Debug.Log("All prefabs are the same.");
            foreach (var position in prefabHitPositions) {
                Instantiate(GroundPrefabTypes[0], position, Quaternion.identity);
            }
        }


    }
    
}
