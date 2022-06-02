using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class StyleManager : MonoBehaviour
{
    public ButtonStyle[] buttonStyles;
}

[System.Serializable]
public struct ButtonStyle
{
    public Texture2D icon;
    public string buttonTex;
    public GameObject prefab;

    [HideInInspector]
    public GUIStyle nodeStyle;
}