using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CropPlacer : MonoBehaviour {
    public static CropPlacer instance;

    [Header("Crop Prefab Settings")]
    private GameObject _cropPrefab;
    private GameObject _toPlant;

    [Header("Layer Settings")]
    public LayerMask cropPlotLayer;

    
    [Header("Raycast Settings")]
    public GameObject raycastOriginObject;
    private Ray _ray;
    private RaycastHit _hit;
    

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(this);
        }

        _cropPrefab = null;
    }

    private void Update() {
        if (_cropPrefab != null) {

            _ray = new Ray(raycastOriginObject.transform.position, Vector3.down);
            if (Physics.Raycast(_ray, out _hit, 1000f, cropPlotLayer, QueryTriggerInteraction.Collide)) {
                if (!_toPlant.activeSelf) {
                    _toPlant.SetActive(true);
                }
                Bounds bounds = _hit.collider.bounds;
                Vector3 hitpoint =bounds.center;
                hitpoint.y = 0;
                _toPlant.transform.position = hitpoint;

                // Left Click on Mouse place crop
                if (Input.GetMouseButtonDown(0)) {
                    CropManager c = _toPlant.GetComponent<CropManager>();
                    if (c.hasValidPlacement) {
                        c.SetCropPlacementMode(PlacementMode.Fixed);

                        //Exit building mode
                        _cropPrefab = null;
                        _toPlant = null;
                    }
                }
            } else if (_toPlant.activeSelf) {
                _toPlant.SetActive(false);
            }
        }
    }

    public void SetCropPrefab(GameObject cropPrefab) {
        _cropPrefab = cropPrefab;
        PrepareCrop();
        EventSystem.current.SetSelectedGameObject(null);
    }

    private void PrepareCrop() {
        if (_toPlant) {
            Destroy(_toPlant);
        }

        _toPlant = Instantiate(_cropPrefab);
        _toPlant.SetActive(false);

        CropManager c = _toPlant.GetComponent<CropManager>();

        c.isFixed = false;
        c.SetCropPlacementMode(PlacementMode.Valid);
    }


}
