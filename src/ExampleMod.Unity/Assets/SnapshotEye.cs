
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapshotEye : MonoBehaviour
{
    public int size = 256;
    public LayerMask layer;
    public bool alpha = true;
    public float outline = 1;
    public Color color;

#if UNITY_EDITOR

    [ContextMenu("Take")]
    public void Take()
    {
        //StartCoroutine(SnapshotUtility.DoSnapshot(size, size, GetComponent<Camera>(), layer, alpha));
    }

#endif


}
