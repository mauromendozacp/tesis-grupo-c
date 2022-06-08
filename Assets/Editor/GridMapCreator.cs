using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GridMapCreator : EditorWindow
{
    private Vector2 offset;
    private Vector2 drag;
    private List<List<Node>> nodes;
    private List<List<PartScripts>> parts;
    private GUIStyle empty;
    private Vector2 nodePos;
    private StyleManager styleManager;
    private bool isErasing;
    private Rect menuBar;
    private GUIStyle currentStyle;
    private GameObject map;

    [MenuItem("Window/Grid Map Creator")]
    private static void OpenWindow()
    {
        GridMapCreator window = GetWindow<GridMapCreator>();
        window.titleContent = new GUIContent("Grid Map Creator");
    }

    private void OnEnable()
    {
        SetUpStyles();
        SetUpNodesAndParts();
        SetUpMap();
    }

    private void SetUpMap()
    {
        try
        {
            map = GameObject.FindGameObjectWithTag("Map");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        if (map != null)
        {
            RestoreTheMap(map);
        }
        else
        {
            map = new GameObject("Map")
            {
                tag = "Map"
            };
        }
    }

    private void RestoreTheMap(GameObject map)
    {
        if (map.transform.childCount > 0)
        {
            for (int i = 0; i < map.transform.childCount; i++)
            {
                int ii = map.transform.GetChild(i).GetComponent<PartScripts>().row;
                int jj = map.transform.GetChild(i).GetComponent<PartScripts>().column;
                GUIStyle style = map.transform.GetChild(i).GetComponent<PartScripts>().style;
                nodes[ii][jj].SetStyle(style);
                parts[ii][jj] = map.transform.GetChild(i).GetComponent<PartScripts>();
                parts[ii][jj].part = map.transform.GetChild(i).gameObject;
                parts[ii][jj].name = map.transform.GetChild(i).name;
                parts[ii][jj].row = ii;
                parts[ii][jj].column = jj;
            }
        }
    }

    private void SetUpStyles()
    {
        try
        {
            styleManager = FindObjectOfType<StyleManager>();
            for (int i = 0; i < styleManager.buttonStyles.Length; i++)
            {
                styleManager.buttonStyles[i].nodeStyle = new GUIStyle();
                styleManager.buttonStyles[i].nodeStyle.normal.background = styleManager.buttonStyles[i].icon;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        empty = styleManager.buttonStyles[0].nodeStyle;
        currentStyle = styleManager.buttonStyles[1].nodeStyle;
    }

    private void SetUpNodesAndParts()
    {
        nodes = new List<List<Node>>();
        parts = new List<List<PartScripts>>();
        for (int i = 0; i < 20; i++)
        {
            nodes.Add(new List<Node>());
            parts.Add(new List<PartScripts>());
            for (int j = 0; j < 10; j++)
            {
                nodePos.Set(i * 30, j * 30);
                nodes[i].Add(new Node(nodePos, 30, 30, empty));
                parts[i].Add(null);
            }
        }
    }

    private void OnGUI()
    {
        DrawGrid();
        DrawNodes();
        DrawMenuBar();
        ProcessNodes(Event.current);
        ProcessGrid(Event.current);

        if (GUI.changed)
        {
            Repaint();
        }
    }

    private void DrawMenuBar()
    {
        menuBar = new Rect(0, 0, position.width, 20);
        GUILayout.BeginArea(menuBar, EditorStyles.toolbar);
        GUILayout.BeginHorizontal();

        for (int i = 0; i < styleManager.buttonStyles.Length; i++)
        {
            if (GUILayout.Toggle(currentStyle == styleManager.buttonStyles[i].nodeStyle, new GUIContent(styleManager.buttonStyles[i].buttonTex), EditorStyles.toolbarButton, GUILayout.Width(80)))
            {
                currentStyle = styleManager.buttonStyles[i].nodeStyle;
            }
        }

        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    private void ProcessNodes(Event e)
    {
        int row = (int)(e.mousePosition.x - offset.x) / 30;
        int col = (int)(e.mousePosition.y - offset.y) / 30;

        if (e.mousePosition.x - offset.x < 0 || e.mousePosition.x - offset.x > 600 || e.mousePosition.y - offset.y < 0 || e.mousePosition.y - offset.y > 300)
        { }
        else
        {
            if (e.type == EventType.MouseDown)
            {
                if (nodes[row][col].style.normal.background.name == "Empty")
                {
                    isErasing = false;
                }
                else
                {
                    isErasing = true;
                }

                PaintNodes(row, col);
            }

            if (e.type == EventType.MouseDrag)
            {
                PaintNodes(row, col);
                e.Use();
            }
        }
    }

    private void PaintNodes(int row, int col)
    {
        if (isErasing)
        {
            if (parts[row][col] != null)
            {
                nodes[row][col].SetStyle(empty);
                DestroyImmediate(parts[row][col].gameObject);
                GUI.changed = true;
            }
            parts[row][col] = null;
        }
        else
        {
            if (parts[row][col] == null)
            {
                nodes[row][col].SetStyle(currentStyle);
                GameObject go = Instantiate(Resources.Load("MapParts/" + currentStyle.normal.background.name)) as GameObject;
                go.name = currentStyle.normal.background.name;
                go.transform.position = new Vector3(col, 0, row) + Vector3.forward + Vector3.right;
                go.transform.parent = map.transform;

                parts[row][col] = go.GetComponent<PartScripts>();
                parts[row][col].part = go;
                parts[row][col].name = go.name;
                parts[row][col].row = row;
                parts[row][col].column = col;
                parts[row][col].style = currentStyle;

                GUI.changed = true;
            }
        }
    }

    private void DrawNodes()
    {
        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                nodes[i][j].Draw();
            }
        }
    }

    private void ProcessGrid(Event e)
    {
        drag = Vector2.zero;
        switch (e.type)
        {
            case EventType.MouseDrag:
                if (e.button == 0)
                {
                    OnMouseDrag(e.delta);
                }
                break;
        }
    }

    private void OnMouseDrag(Vector2 delta)
    {
        drag = delta;

        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                nodes[i][j].Drag(delta);
            }
        }

        GUI.changed = true;
    }

    private void DrawGrid()
    {
        int widthDivider = Mathf.CeilToInt(position.width / 20);
        int heightDivider = Mathf.CeilToInt(position.height / 20);

        Handles.BeginGUI();
        Handles.color = new Color(0.5f, 0.5f, 0.5f, 0.2f);

        offset += drag;
        Vector3 newOffset = new Vector3(offset.x % 20, offset.y % 20, 0);

        for (int i = 0; i < widthDivider; i++)
        {
            Handles.DrawLine(new Vector3(20 * i, -20, 0) + newOffset, new Vector3(20 * i, position.height, 0) + newOffset);
        }
        for (int i = 0; i < heightDivider; i++)
        {
            Handles.DrawLine(new Vector3(-20, 20 * i, 0) + newOffset, new Vector3(position.width, 20 * i, 0) + newOffset);
        }

        Handles.color = Color.white;
        Handles.EndGUI();
    }
}