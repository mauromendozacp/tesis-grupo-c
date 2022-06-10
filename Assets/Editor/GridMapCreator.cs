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
    private Rect configBar;
    private GUIStyle currentStyle;
    private GameObject map;
    private string rows;
    private string columns;

    #region UNITY_CALLS
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

    private void OnGUI()
    {
        DrawGrid();
        DrawNodes();
        DrawMenuBar();
        DrawConfigBar();
        ProcessNodes(Event.current);
        ProcessGrid(Event.current);

        if (GUI.changed)
        {
            Repaint();
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
    #endregion

    #region INIT
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
        if (map.transform.childCount <= 0) return;

        for (int i = 0; i < map.transform.childCount; i++)
        {
            PartScripts partScripts = map.transform.GetChild(i).GetComponent<PartScripts>();
            int row = partScripts.row;
            int col = partScripts.column;
            parts[row][col] = partScripts;

            GUIStyle style = partScripts.style;
            nodes[row][col].SetStyle(style);

            parts[row][col].part = map.transform.GetChild(i).gameObject;
            parts[row][col].name = map.transform.GetChild(i).name;
            parts[row][col].row = row;
            parts[row][col].column = col;
        }
    }
    #endregion

    #region DRAW_METHODS
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

    private void DrawConfigBar()
    {
        configBar = new Rect(0, 20, position.width, 20);
        GUILayout.BeginArea(configBar, EditorStyles.textField);
        GUILayout.BeginHorizontal();

        rows = EditorGUILayout.TextField("Rows: ", rows);
        columns = EditorGUILayout.TextField("Columns: ", columns);


        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }
    #endregion

    #region NODES_METHODS
    private void ProcessNodes(Event e)
    {
        if (e.mousePosition.x - offset.x < 0 || e.mousePosition.x - offset.x > 600 || e.mousePosition.y - offset.y < 0 || e.mousePosition.y - offset.y > 300) return;

        int row = (int)(e.mousePosition.x - offset.x) / 30;
        int col = (int)(e.mousePosition.y - offset.y) / 30;

        if (e.type == EventType.MouseDown)
        {
            isErasing = nodes[row][col].style.normal.background.name != "Empty";

            PaintNodes(row, col);
        }

        if (e.type != EventType.MouseDrag) return;

        PaintNodes(row, col);
        e.Use();
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
            if (parts[row][col] != null) return;

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
    #endregion

    #region GRID_METHODS
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

    private void DrawGrid()
    {
        int widthDivider = Mathf.CeilToInt(position.width / 20);
        int heightDivider = Mathf.CeilToInt(position.height / 20);

        Handles.BeginGUI();
        Handles.color = new Color(0.5f, 0.5f, 0.5f, 0.2f);

        offset += drag;
        Vector3 newOffset = new Vector3(offset.x % 30, offset.y % 30, 0);

        for (int i = 0; i < widthDivider; i++)
        {
            Handles.DrawLine(new Vector3(30 * i, -30, 0) + newOffset, new Vector3(30 * i, position.height, 0) + newOffset);
        }
        for (int i = 0; i < heightDivider; i++)
        {
            Handles.DrawLine(new Vector3(-30, 30 * i, 0) + newOffset, new Vector3(position.width, 30 * i, 0) + newOffset);
        }

        Handles.color = Color.white;
        Handles.EndGUI();
    }
    #endregion
}