using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GridMapCreator : EditorWindow
{
    private Vector2 offset;
    private Vector2 drag;
    private List<List<Node>> nodes;
    private GUIStyle empty;
    private Vector2 nodePos;
    private StyleManager styleManager;
    private bool isErasing;
    private Rect menuBar;
    private GUIStyle currentStyle;

    [MenuItem("Window/Grid Map Creator")]
    private static void OpenWindow()
    {
        GridMapCreator window = GetWindow<GridMapCreator>();
        window.titleContent = new GUIContent("Grid Map Creator");
    }

    private void OnEnable()
    {
        SetUpStyles();
        empty = new GUIStyle();
        Texture2D icon = Resources.Load("IconTex/Empty") as Texture2D;
        empty.normal.background = icon;
        SetUpNodes();
        currentStyle = styleManager.buttonStyles[1].nodeStyle;
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
    }

    private void SetUpNodes()
    {
        nodes = new List<List<Node>>();
        for (int i = 0; i < 20; i++)
        {
            nodes.Add(new List<Node>());
            for (int j = 0; j < 10; j++)
            {
                nodePos.Set(i * 30, j * 30);
                nodes[i].Add(new Node(nodePos, 30, 30, empty));
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
            nodes[row][col].SetStyle(empty);
            GUI.changed = true;
        }
        else
        {
            nodes[row][col].SetStyle(currentStyle);
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