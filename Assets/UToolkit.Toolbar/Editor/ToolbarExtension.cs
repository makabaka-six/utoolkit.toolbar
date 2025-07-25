using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UToolkit.Toolbar.Editor
{
    [InitializeOnLoad]
    internal static class ToolbarExtension
    {
        private static readonly List<(int, Action)> s_leftToolbarGUI = new List<(int, Action)>();
        private static readonly List<(int, Action)> s_rightToolbarGUI = new List<(int, Action)>();

        static ToolbarExtension()
        {
            ToolbarCallback.OnToolbarGUILeft = GUILeft;
            ToolbarCallback.OnToolbarGUIRight = GUIRight;
            Type attributeType = typeof(ToolbarAttribute);

            foreach (var methodInfo in TypeCache.GetMethodsWithAttribute<ToolbarAttribute>())
            {
                var attributes = methodInfo.GetCustomAttributes(attributeType, false);
                if (attributes.Length > 0)
                {
                    ToolbarAttribute attribute = (ToolbarAttribute)attributes[0];
                    if (attribute.Side == OnGUISide.Left)
                    {
                        s_leftToolbarGUI.Add((attribute.Priority, delegate
                        {
                            methodInfo.Invoke(null, null);
                        }));
                        continue;
                    }
                    if (attribute.Side == OnGUISide.Right)
                    {
                        s_rightToolbarGUI.Add((attribute.Priority, delegate
                        {
                            methodInfo.Invoke(null, null);
                        }));
                        continue;
                    }
                }
            }
            s_leftToolbarGUI.Sort((tuple1, tuple2) => tuple1.Item1 - tuple2.Item1);
            s_rightToolbarGUI.Sort((tuple1, tuple2) => tuple2.Item1 - tuple1.Item1);
        }

        static void GUILeft()
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            foreach (var handler in s_leftToolbarGUI)
            {
                handler.Item2();
            }

            GUILayout.EndHorizontal();
        }

        static void GUIRight()
        {
            GUILayout.BeginHorizontal();
            foreach (var handler in s_rightToolbarGUI)
            {
                handler.Item2();
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }
}