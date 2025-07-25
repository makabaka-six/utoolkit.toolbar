using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UToolkit.Toolbar.Editor
{
    internal static class ToolbarCallback
    {
        private static readonly Type _toolbarType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.Toolbar");

        public static Action OnToolbarGUILeft;
        public static Action OnToolbarGUIRight;

        private static VisualElement _toolBarRootVisualElement;
        private static ScriptableObject _toolBarScriptableObject;
        private static FieldInfo _toolbarRootFieldInfo;

        static ToolbarCallback()
        {
            EditorApplication.update -= OnUpdate;
            EditorApplication.update += OnUpdate;
        }

        static void RegisterCallback(string root, Action cb)
        {
            var toolbarZone = _toolBarRootVisualElement.Q(root);
            var parent = new VisualElement()
            {
                style =
                {
                    flexGrow = 1,
                    flexDirection = FlexDirection.Row,
                }
            };
            var container = new IMGUIContainer();
            container.style.flexGrow = 1;
            container.onGUIHandler += () =>
            {
                cb?.Invoke();
            };
            parent.Add(container);
            toolbarZone.Add(parent);
        }

        static void OnUpdate()
        {
            if (_toolBarRootVisualElement == null)
            {
                if (_toolBarScriptableObject == null)
                {
                    var toolbars = Resources.FindObjectsOfTypeAll(_toolbarType);
                    _toolBarScriptableObject = toolbars.Length > 0 ? (ScriptableObject)toolbars[0] : null;
                }

                if (_toolbarRootFieldInfo == null && _toolBarScriptableObject != null)
                {
                    _toolbarRootFieldInfo = _toolBarScriptableObject.GetType().GetField("m_Root", BindingFlags.NonPublic | BindingFlags.Instance);
                }

                if (_toolbarRootFieldInfo != null)
                {
                    _toolBarRootVisualElement = _toolbarRootFieldInfo.GetValue(_toolBarScriptableObject) as VisualElement;
                    if (_toolBarRootVisualElement != null)
                    {
                        RegisterCallback("ToolbarZoneLeftAlign", OnToolbarGUILeft);
                        RegisterCallback("ToolbarZoneRightAlign", OnToolbarGUIRight);
                    }
                }
            }
            else
            {
                VisualElement curRoot = _toolbarRootFieldInfo.GetValue(_toolBarScriptableObject) as VisualElement;
                if (_toolBarRootVisualElement != curRoot)
                {
                    _toolBarRootVisualElement = curRoot;
                    RegisterCallback("ToolbarZoneLeftAlign", OnToolbarGUILeft);
                    RegisterCallback("ToolbarZoneRightAlign", OnToolbarGUIRight);
                }
            }
        }
    }
}