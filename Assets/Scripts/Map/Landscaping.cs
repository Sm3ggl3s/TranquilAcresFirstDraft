using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Landscaping : MonoBehaviour
{
    [Header("Palette Settings")]
    public List<GameObject> GroundPrefabs;

    public void FixOrientation(GameObject obj)
    {
        Quaternion currentRotation = obj.transform.rotation;
        obj.transform.rotation = new Quaternion(0, currentRotation.y + 90, 0, 0);
    }
}
