# Unity工具栏拓展

### 使用方式
``` C#
using UnityEditor;
using UnityEngine;
using UToolkit.Toolbar.Editor;

public class Example1
{
    [Toolbar(OnGUISide.Left, 100)]
    private static void OnLeftToolbarCallback()
    {
        EditorGUILayout.LabelField("Left Toolbar", EditorStyles.boldLabel, GUILayout.Width(100));

        if (GUILayout.Button("Menu1", GUILayout.Width(100)))
        {
            Debug.Log("Menu1 clicked");
        }

        EditorGUILayout.Popup(0, new string[] { "Option1", "Option2", "Option3" }, GUILayout.Width(100));
    }

    [Toolbar(OnGUISide.Right, 100)]
    private static void OnRightToolbarCallback()
    {
        if (GUILayout.Button("Menu2", GUILayout.Width(100)))
        {
            Debug.Log("Menu2 clicked");
        }
    }
}
```

### 效果展示
<img src = "./Assets/Images~/Snipaste_2025-07-26_04-21-26.png"/>