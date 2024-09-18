using System.Collections.Generic;
using UnityEditor;

public class TreeNode
{
    public string Name { get; set; }
    public string Path { get; set; }
    public bool IsExpanded { get; set; }
    public string Icon { get; set; }
    public TreeNode Parent { get; set; }
    public Dictionary<string, TreeNode> Children { get; set; }
    public int ChildCount { get; set; }

    public TreeNode(string name, string path, string icon = "Folder Icon")
    {
        Name = name;
        Path = path;
        Icon = icon;
        Children = new Dictionary<string, TreeNode>();
        ChildCount = 0;
    }

    public TreeNode GetOrAddChild(string name, string path, string icon = "Folder Icon")
    {
        if (!Children.ContainsKey(name))
        {
            var child = new TreeNode(name, path, icon);
            child.Parent = this;
            Children[name] = child;
            ChildCount++;
        }
        return Children[name];
    }
}