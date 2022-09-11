using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartScripts : MonoBehaviour
{
    [HideInInspector] public int row;
    [HideInInspector] public int column;
    public string partName = "Empty";
    public ENTITY_TYPE partType;
    [HideInInspector] public GameObject part;
    [HideInInspector] public GUIStyle style;
}