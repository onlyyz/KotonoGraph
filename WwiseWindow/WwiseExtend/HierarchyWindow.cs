using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class HierarchyWindow : EditorWindow
{
    private TreeNode root1;
    private TreeNode root2;
    private Dictionary<string, TreeNode> nodeDict1 = new Dictionary<string, TreeNode>();
    private Dictionary<string, TreeNode> nodeDict2 = new Dictionary<string, TreeNode>();

    [MenuItem("Tools/Hierarchy Window")]
    public static void ShowWindow()
    {
        GetWindow<HierarchyWindow>("Hierarchy Window");
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Refresh Tree"))
        {
            BuildTree();
        }
        if (GUILayout.Button("Generate ScriptObj"))
        {
            // TODO: Add functionality to generate ScriptObj
        }
        GUILayout.EndHorizontal();

        if (root1 != null)
        {
            DrawNode(root1, 0);
        }
        if (root2 != null)
        {
            DrawNode(root2, 0);
        }
    }

    private void BuildTree()
    {
        root1 = new TreeNode("Root1", "Root1");
        root2 = new TreeNode("Root2", "Root2");
        nodeDict1.Clear();
        nodeDict2.Clear();
        AddPathToTree("Events\\Default Work Unit\\Player", root1, nodeDict1);
        AddPathToTree("Events\\Default Work Unit\\Player\\FootEvent", root1, nodeDict1);
        AddPathToTree("Events\\Default Work Unit\\Player\\PlayerHealth", root1, nodeDict1);
        AddPathToTree("Events\\Default Work Unit\\Enemy", root2, nodeDict2);
        AddPathToTree("Events\\Default Work Unit\\Enemy\\EnemyHealth", root2, nodeDict2);
    }

    private void AddPathToTree(string path, TreeNode root, Dictionary<string, TreeNode> nodeDict)
    {
        var names = path.Split('\\');
        var currentNode = root;
        for (int i = 0; i < names.Length; i++)
        {
            var name = names[i];
            var icon = i < names.Length - 1 ? "Folder Icon" : "cs Script Icon";
            currentNode = currentNode.GetOrAddChild(name, path, icon);
            nodeDict[path] = currentNode;
        }
    }

    private void DrawNode(TreeNode node, int indent)
    {
        EditorGUI.indentLevel = indent;
        if (node.ChildCount > 0)
        {
            node.IsExpanded = EditorGUILayout.Foldout(node.IsExpanded, new GUIContent(node.Name, EditorGUIUtility.IconContent(node.Icon).image), true);
            if (node.IsExpanded)
            {
                foreach (var child in node.Children.Values)
                {
                    DrawNode(child, indent + 1);
                }
            }
        }
        else
        {
            EditorGUILayout.LabelField(new GUIContent(node.Name, EditorGUIUtility.IconContent(node.Icon).image));
        }
    }
}
