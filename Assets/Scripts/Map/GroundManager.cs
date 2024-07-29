using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
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

        if (hitPrefabs.Count != 4) {
            Debug.Log("Expected 4 ground prefabs, but got: " + hitPrefabs.Count);
            foreach (var prefab in hitPrefabs) {
                Debug.Log("Prefab: " + prefab.name);
            }
            return;
        }

        // Storing Names and Rotations of the Prefabs for easy comparison and reinstantiation
        string[] prefabNames = hitPrefabs.Select(prefab => prefab.name).ToArray();
        Quaternion[] prefabRotations = hitPrefabs.Select(prefab => prefab.transform.rotation).ToArray();

        foreach (var name in prefabNames) {
            Debug.Log("Prefab Name: " + name);
        }

        var prefabGroups = prefabNames.GroupBy(name => name).Select(group => new { Name = group.Key, Count = group.Count() })
        .OrderByDescending(group => group.Count).ToList();

        Debug.Log("Prefab Groups: "+ prefabGroups.Count);

        if (prefabGroups.Count == 1) {
            string uniquePrefabName = prefabGroups[0].Name;
            Debug.Log("Prefab Group Name: " + uniquePrefabName);

            if (uniquePrefabName != "Grass(Clone)") {
                Debug.Log("Unique prefab is not Grass(Clone). No adjustments will be made.");

                // Reinstantiate original prefabs with the same orientation
                ReinstantiateOriginalPrefabs();

                return;
            }
            float rotationAngle = 90.0f;

            foreach (var position in prefabHitPositions) {
                Quaternion rotation = Quaternion.Euler(0.0f, rotationAngle, 0.0f);
                Instantiate(GroundPrefabTypes[1], position, rotation);
                rotationAngle += 90.0f;
            }
        }

        if (prefabGroups.Count == 1 && hitPrefabs[0].name != "Grass(Clone)") {
            return;
        }

        if (prefabGroups.Count == 2) {

            #region TopAdjustment
            if (prefabNames[0] == "Grass(Clone)" && prefabNames[1] == "Grass(Clone)") {
                float rotationAngle = 90.0f;

                for (int i = 0; i < prefabHitPositions.Count - 2; i++) {
                    var position = prefabHitPositions[i];
                    Quaternion rotation = Quaternion.Euler(0.0f, rotationAngle, 0.0f);
                    Instantiate(GroundPrefabTypes[1], position, rotation);
                    rotationAngle += 90.0f;
                }
            }
            if (prefabNames[2] == "Grass-Dirt-1(Clone)" && prefabNames[3] == "Grass-Dirt-1(Clone)") {
                
                float rotationAngle = 0.0f;

                for (int i = 2; i < prefabHitPositions.Count; i++) {
                    var position = prefabHitPositions[i];
                    Quaternion rotation = Quaternion.Euler(0.0f, rotationAngle, 0.0f);
                    Instantiate(GroundPrefabTypes[2], position, rotation);
                    rotationAngle += 180.0f;
                }
            }
            #endregion
            
            #region LeftAdjustment
            if (prefabNames[0] == "Grass(Clone)" && prefabNames[3] == "Grass(Clone)") {
                Debug.Log("Grass and Grass-Dirt-1 prefab group detected.");
                float rotationAngle = 90.0f;

                for (int i = 0; i < prefabHitPositions.Count; i++) {
                    if (i == 0 || i == 3) {
                        var position = prefabHitPositions[i];
                        Quaternion rotation = Quaternion.Euler(0.0f, rotationAngle, 0.0f);
                        Instantiate(GroundPrefabTypes[1], position, rotation);
                        rotationAngle += 270.0f;
                    }
                }
            }

            if (prefabNames[1] == "Grass-Dirt-1(Clone)" && prefabNames[2] == "Grass-Dirt-1(Clone)") {
                
                float rotationAngle = -90.0f;

                for (int i = 1; i < prefabHitPositions.Count-1; i++) {
                    var position = prefabHitPositions[i];
                    Quaternion rotation = Quaternion.Euler(0.0f, rotationAngle, 0.0f);
                    Instantiate(GroundPrefabTypes[2], position, rotation);
                    rotationAngle += 180.0f;
                }
            }
            #endregion

            #region RightAdjustment

            if (prefabNames[1] == "Grass(Clone)" && prefabNames[2] == "Grass(Clone)") {                
                float rotationAngle = 180.0f;
                
                for (int i = 1; i < prefabHitPositions.Count-1; i++) {
                    var position = prefabHitPositions[i];
                    Quaternion rotation = Quaternion.Euler(0.0f, rotationAngle, 0.0f);
                    Instantiate(GroundPrefabTypes[1], position, rotation);
                    rotationAngle += 90.0f;
                }
            }

            if (prefabNames[0] == "Grass-Dirt-1(Clone)" && prefabNames[3] == "Grass-Dirt-1(Clone)") {
                float rotationAngle = -90.0f;
                for (int i = 0; i < prefabHitPositions.Count; i++) {
                    if (i == 0 || i == 3) {
                        var position = prefabHitPositions[i];
                        Quaternion rotation = Quaternion.Euler(0.0f, rotationAngle, 0.0f);
                        Instantiate(GroundPrefabTypes[2], position, rotation);
                        rotationAngle += 180.0f;
                    }
                }
            }
            #endregion

            #region BottomAdjustment
            
            if (prefabNames[2] == "Grass(Clone)" && prefabNames[3] == "Grass(Clone)") {
                float rotationAngle = -90.0f;

                for (int i = 2; i < prefabHitPositions.Count; i++) {
                    var position = prefabHitPositions[i];
                    Quaternion rotation = Quaternion.Euler(0.0f, rotationAngle, 0.0f);
                    Instantiate(GroundPrefabTypes[1], position, rotation);
                    rotationAngle += 90.0f;
                }
            }
            if (prefabNames[0] == "Grass-Dirt-1(Clone)" && prefabNames[1] == "Grass-Dirt-1(Clone)") {
                
                float rotationAngle = 180.0f;

                for (int i = 0; i < prefabHitPositions.Count - 2; i++) {
                    var position = prefabHitPositions[i];
                    Quaternion rotation = Quaternion.Euler(0.0f, rotationAngle, 0.0f);
                    Instantiate(GroundPrefabTypes[2], position, rotation);
                    rotationAngle -= 180.0f;
                }
            }

            #endregion
        }
    }
    
    private void ReinstantiateOriginalPrefabs() {
    for (int i = 0; i < prefabHitPositions.Count; i++) {
        var position = prefabHitPositions[i];
        var rotation = hitPrefabs[i].transform.rotation;
        Instantiate(hitPrefabs[i], position, rotation);
    }
} 
    
}
