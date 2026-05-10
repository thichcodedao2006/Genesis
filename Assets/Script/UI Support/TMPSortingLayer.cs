using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TMPSortingLayer : MonoBehaviour
{
    [SerializeField] private int sortingOrder = 10;
    [SerializeField] private string sortingLayerName = "Default";

    void Awake()
    {
        var mr = GetComponent<MeshRenderer>();
        mr.sortingLayerName = sortingLayerName;
        mr.sortingOrder = sortingOrder;
    }
}
