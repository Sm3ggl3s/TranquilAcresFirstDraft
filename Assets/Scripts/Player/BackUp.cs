using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackUp : MonoBehaviour {

    public GameObject playerLookIndicator;

    [Header("Raycast Settings")]
    public GameObject raycastOriginObject;
    private Ray _ray;
    private RaycastHit _hit;

    [Header("Layer Settings")]
    public LayerMask groundLayer;

    // InventoryManager inv;

    // private void Start() {
        // inv = InventoryManager.instance;
    // }

    private void Update() {
        _ray = new Ray(raycastOriginObject.transform.position, Vector3.down);
        if (Physics.Raycast(_ray, out _hit, 1000f, groundLayer)) {            
            Debug.Log("Hit Floor: " + _hit.collider.gameObject.name);
            Debug.Log("Hit Point: " + _hit.point);
            Debug.Log("Player Look Indicator: " + playerLookIndicator.activeSelf);
            playerLookIndicator.transform.position = _hit.point;
        }

        // if (Input.GetKeyDown(KeyCode.E)) {
        //     Debug.Log("E Key Pressed");
        //     _ray = new Ray(raycastOriginObject.transform.position, Vector3.down);
        //     if (Physics.Raycast(_ray, out _hit, 1000f, cropLayer, QueryTriggerInteraction.Collide)) {
        //         Bounds bounds = _hit.collider.bounds;
        //         Vector3 hitpoint =bounds.center;
        //         hitpoint.y = 0;

        //         Debug.Log("Hit Crop: " + _hit.collider.gameObject.name);
        //         CropManager c = _hit.collider.gameObject.GetComponent<CropManager>();
        //         if (c.isHarvestable) {
        //             c.HarvestCrop();
        //         }
        //     }
        // }

        // if (Input.GetKeyDown(KeyCode.Q)) {
        //     Debug.Log("Q Key Pressed");
        //     _ray = new Ray(raycastOriginObject.transform.position, Vector3.down);
        //     if (Physics.Raycast(_ray, out _hit, 1000f, cropLayer, QueryTriggerInteraction.Collide)) {
        //         Bounds bounds = _hit.collider.bounds;
        //         Vector3 hitpoint =bounds.center;
        //         hitpoint.y = 0;

        //         Debug.Log("Hit Crop: " + _hit.collider.gameObject.name);
        //         CropManager c = _hit.collider.gameObject.GetComponent<CropManager>();
        //         Debug.Log("Inventory: " + inv);
        //         if (c != null && c.cropData != null && inv != null) {
        //             inv.AddCrop(c.cropData, c.cropData.cropQuantityToAdd);
        //             c.DestroyHarvestPrefabInstance();
        //             Destroy(_hit.collider.gameObject);
        //         }
        //     }
        // }
    }
}
