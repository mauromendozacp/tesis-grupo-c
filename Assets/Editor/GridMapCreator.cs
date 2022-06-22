using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using Codice.Client.BaseCommands;
using UnityEditor;
using UnityEngine;

public class GridMapCreator : EditorWindow
{
    #region PRIVATE_FIELDS
    private StyleManager styleManager;

    private GameObject map;

    private bool isErasing;
    private bool propSelected;

    private List<List<Node>> nodes;
    private List<List<PartScripts>> parts;

    private int rows;
    private int columns;

    private string rowsText;
    private string columnsText;

    private GUIStyle emptyStyle;
    private GUIStyle currentStyle;

    private Rect menuBar;
    private Rect configBar;

    private Vector3 rotation;

    private Vector2 offset;
    private Vector2 drag;
    private Vector2 nodePos;
    #endregion

    #region UNITY_CALLS
    [MenuItem("Window/Grid Map Creator")]
    private static void OpenWindow()
    {
        GridMapCreator window = GetWindow<GridMapCreator>();
        window.titleContent = new GUIContent("Grid Map Creator");
    }

    private void OnEnable()
    {
        rows = EditorPrefs.GetInt("Rows", 0);
        columns = EditorPrefs.GetInt("Columns", 0);

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

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
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

        emptyStyle = styleManager.buttonStyles[0].nodeStyle;
        currentStyle = styleManager.buttonStyles[1].nodeStyle;
    }

    private void SetUpNodesAndParts()
    {
        nodes = new List<List<Node>>();
        parts = new List<List<PartScripts>>();

        for (int i = 0; i < rows; i++)
        {
            nodes.Add(new List<Node>());
            parts.Add(new List<PartScripts>());

            for (int j = 0; j < columns; j++)
            {
                nodePos.Set(j * 30, i * 30);
                nodes[i].Add(new Node(nodePos, 30, 30, emptyStyle));
                parts[i].Add(null);
            }
        }

        //for (int i = rows - 1; i >= 0; i--) 
        //{
        //    nodes.Add(new List<Node>());
        //    parts.Add(new List<PartScripts>());

        //    for (int j = columns - 1; j >= 0; j--) 
        //    {
        //        nodePos.Set(j * 30, i * 30);
        //        nodes[rows-i-1].Add(new Node(nodePos, 30, 30, emptyStyle));
        //        parts[rows - i - 1].Add(null);
        //        Debug.Log(i - rows + 1);
        //    }
        //}
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

    #region BAR_METHODS
    private void DrawMenuBar()
    {
        menuBar = new Rect(0, 0, position.width, 20);
        GUILayout.BeginArea(menuBar, EditorStyles.toolbar);
        GUILayout.BeginHorizontal();

        for (int i = 0; i < styleManager.buttonStyles.Length; i++)
        {
            if (styleManager.buttonStyles[i].icon.name == "Obstacle") continue;

            if (GUILayout.Toggle(currentStyle == styleManager.buttonStyles[i].nodeStyle, new GUIContent(styleManager.buttonStyles[i].buttonTex), EditorStyles.toolbarButton, GUILayout.Width(90)))
            {
                currentStyle = styleManager.buttonStyles[i].nodeStyle;
            }
        }

        GUILayout.EndHorizontal();
        GUILayout.EndArea();

        menuBar = new Rect(0, 21, position.width, 20);
        GUILayout.BeginArea(menuBar, EditorStyles.toolbar);
        GUILayout.BeginHorizontal();

        for (int i = 0; i < styleManager.buttonStyles.Length; i++)
        {
            if (styleManager.buttonStyles[i].icon.name != "Obstacle") continue;

            if (GUILayout.Toggle(currentStyle == styleManager.buttonStyles[i].nodeStyle, new GUIContent(styleManager.buttonStyles[i].buttonTex), EditorStyles.toolbarButton, GUILayout.Width(90)))
            {
                currentStyle = styleManager.buttonStyles[i].nodeStyle;
            }
        }

        GUILayout.EndHorizontal();
        GUILayout.EndArea();

        menuBar = new Rect(0, 41, position.width, 20);
        GUILayout.BeginArea(menuBar, EditorStyles.toolbar);
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Forward"))
        {
            rotation = Vector3.forward;
        }
        if (GUILayout.Button("Back"))
        {
            rotation = Vector3.back;
        }
        if (GUILayout.Button("Left"))
        {
            rotation = Vector3.left;
        }
        if (GUILayout.Button("Right"))
        {
            rotation = Vector3.right;
        }

        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    private void DrawConfigBar()
    {
        configBar = new Rect(0, 61, position.width, 20);
        GUILayout.BeginArea(configBar, EditorStyles.textField);
        GUILayout.BeginHorizontal();

        rowsText = EditorGUILayout.TextField("Rows: ", rowsText);
        columnsText = EditorGUILayout.TextField("Columns: ", columnsText);

        if (GUILayout.Button("Create Grid"))
        {
            rows = Int32.Parse(rowsText);
            columns = Int32.Parse(columnsText);

            EditorPrefs.SetInt("Rows", rows);
            EditorPrefs.SetInt("Columns", columns);

            SetUpNodesAndParts();
            SetUpMap();
        }

        if (GUILayout.Button("Export Level"))
        {
            ExportLevel();
        }

        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }
    #endregion

    #region NODES_METHODS
    private void ProcessNodes(Event e)
    {
        if (e.mousePosition.x - offset.x < 0 || e.mousePosition.x - offset.x > columns * 30 || e.mousePosition.y - offset.y < 0 || e.mousePosition.y - offset.y > rows * 30) return;

        int row = (int)((e.mousePosition.y - offset.y) / 30);
        int col = (int)((e.mousePosition.x - offset.x) / 30);

        if (e.type == EventType.MouseDown)
        {
            isErasing = nodes[row][col].style.normal.background.name != "Erase";

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
            if (row > rows - 1 || col > columns - 1) return;

            if (parts[row][col] != null)
            {
                if (Physics.Raycast(parts[row][col].gameObject.transform.position, Vector3.down, out var hit, 1))
                {
                    DestroyImmediate(hit.transform.gameObject);
                }

                nodes[row][col].SetStyle(emptyStyle);
                DestroyImmediate(parts[row][col].gameObject);

                GUI.changed = true;
            }
            parts[row][col] = null;
        }
        else
        {
            if (row > rows - 1 || col > columns - 1) return;

            if (parts[row][col] != null) return;

            nodes[row][col].SetStyle(currentStyle);


            for (int i = 0; i < styleManager.buttonStyles.Length; i++)
            {
                if (currentStyle != styleManager.buttonStyles[i].nodeStyle) continue;

                GameObject go = Instantiate(styleManager.buttonStyles[i].prefab);
                go.name = styleManager.buttonStyles[i].prefab.name;

                if (go.name == "trap" || go.name == "floor")
                {
                    go.transform.position = new Vector3(col - 1, -1, rows - row - 1) + Vector3.right;
                }
                else
                {
                    go.transform.position = new Vector3(col - 1, 0, rows - row - 1) + Vector3.right;
                }

                go.transform.forward = rotation;
                go.transform.parent = map.transform;

                parts[row][col] = go.GetComponent<PartScripts>();
                parts[row][col].part = go;
                parts[row][col].name = go.name;
                parts[row][col].row = row;
                parts[row][col].column = col;
                parts[row][col].style = currentStyle;

                //TODO add proper logic to check if floor is needed
                if (go.name != "trap" && go.name != "floor")
                {
                    GameObject floor = Instantiate(Resources.Load("MapParts/" + "floor")) as GameObject;
                    floor.transform.position = new Vector3(col - 1, -1, rows - row - 1) + Vector3.right;
                    floor.transform.parent = map.transform;
                }

                GUI.changed = true;
                break;
            }
        }
    }

    private void DrawNodes()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
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

    #region EXPORT
    private void ExportLevel()
    {
        LevelModel level = new LevelModel
        {
            Layers = new LayerModel[2],
            PlayerModel = new PlayerModel(),
            LimitI = parts[0].Count,
            LimitJ = parts.Count
        };
        level.Layers[0] = new LayerModel
        {
            LayerIndex = -1
        };
        level.Layers[1] = new LayerModel
        {
            LayerIndex = 0
        };

        List<EntityModel> floorLayer = new List<EntityModel>();
        List<EntityModel> topLayer = new List<EntityModel>();

        for (int i = 0; i < parts.Count; i++)
        {
            for (int j = 0; j < parts[i].Count; j++)
            {
                if (parts[i][j].partName == "floor" || parts[i][j].partName == "floor_trap")
                {
                    EntityModel model = new EntityModel
                    {
                        Id = parts[i][j].partName,
                        Index = new GridIndex((int)parts[i][j].gameObject.transform.position.x, (int)parts[i][j].gameObject.transform.position.z),
                        Type = parts[i][j].partType,
                        Rotation = new RotationModel
                        {
                            X = parts[i][j].gameObject.transform.forward.x,
                            Y = parts[i][j].gameObject.transform.forward.y,
                            Z = parts[i][j].gameObject.transform.forward.z
                        }
                    };

                    floorLayer.Add(model);
                }
                else
                {
                    EntityModel floor = new EntityModel
                    {
                        Id = "floor",
                        Index = new GridIndex((int)parts[i][j].gameObject.transform.position.x, (int)parts[i][j].gameObject.transform.position.z),
                        Type = ENTITY_TYPE.NO_MOVABLE
                    };

                    floorLayer.Add(floor);

                    if (parts[i][j].partName != "player" && parts[i][j].partName != "win")
                    {
                        EntityModel model = new EntityModel
                        {
                            Id = parts[i][j].partName,
                            Index = new GridIndex((int)parts[i][j].gameObject.transform.position.x, (int)parts[i][j].gameObject.transform.position.z),
                            Type = parts[i][j].partType,
                            Rotation = new RotationModel
                            {
                                X = parts[i][j].gameObject.transform.forward.x,
                                Y = parts[i][j].gameObject.transform.forward.y,
                                Z = parts[i][j].gameObject.transform.forward.z
                            }
                        };

                        topLayer.Add(model);
                    }
                }

                if (parts[i][j].partName == "player")
                {
                    level.PlayerModel.I = (int)parts[i][j].gameObject.transform.position.x;
                    level.PlayerModel.J = (int)parts[i][j].gameObject.transform.position.z;
                    level.PlayerModel.Rotation = parts[i][j].gameObject.transform.forward;
                }

                if (parts[i][j].partName == "win")
                {
                    level.WinI = (int)parts[i][j].gameObject.transform.position.x;
                    level.WinJ = (int)parts[i][j].gameObject.transform.position.z;
                }
            }
        }

        level.Layers[0].Models = new EntityModel[floorLayer.Count];
        level.Layers[1].Models = new EntityModel[topLayer.Count];

        for (int i = 0; i < floorLayer.Count; i++)
        {
            level.Layers[0].Models[i] = floorLayer[i];
        }

        for (int i = 0; i < topLayer.Count; i++)
        {
            level.Layers[1].Models[i] = topLayer[i];
        }

        string json = JsonUtility.ToJson(level);

        string path = EditorUtility.OpenFilePanel("Select json", "", "json");
        if (path.Length != 0)
        {
            File.WriteAllText(path, json);
        }
    }
    #endregion
}