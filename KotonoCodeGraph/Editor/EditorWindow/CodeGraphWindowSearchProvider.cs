using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Kotono.Code.Editor
{

    public struct SearchContextElement
    {
        public object target { get; private set; }
        public string title { get; private set; }

        public SearchContextElement(object target, string title)
        {
            this.target = target;
            this.title = title;
        }
    }
    
    public class CodeGraphWindowSearchProvider : ScriptableObject,ISearchWindowProvider
    {
        public CodeGraphView graph;
        public VisualElement target;
        
        public static List<SearchContextElement> elements;
        
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> tree = new List<SearchTreeEntry>();
            tree.Add(new SearchTreeGroupEntry(new GUIContent("Node"),0));
            elements = new List<SearchContextElement>();
            
            //程序集搜索
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assemblie in assemblies)
            {
                foreach (var type in assemblie.GetTypes())
                {
                    if (type.CustomAttributes.ToList() != null)
                    {
                        var attribute = type.GetCustomAttribute(typeof(UCLASSAttribute));
                        if (attribute != null)
                        {
                            UCLASSAttribute att = (UCLASSAttribute)attribute;
                            //得知是具体的Node
                            var node = Activator.CreateInstance(type);

                            if (string.IsNullOrEmpty(att.menuItem))
                            {
                                continue;
                            }
                            elements.Add(new SearchContextElement(node, att.menuItem));
                        }
                        
                    }
                    
                }
            }
            
            //Sort by Name 创建实际的树
            elements.Sort((entry1, entry2) =>
            {
                string[] splits1 = entry1.title.Split('/');
                string[] splits2 = entry2.title.Split('/');
                for (int i = 0; i < splits1.Length; i++)
                {
                    if (i >= splits2.Length)
                    {
                        return 1;
                    }
                    int value = splits1[i].CompareTo(splits2[i]);
                   
                    if (value != 0)
                    {
                        if (splits1.Length != splits2.Length && (i == splits1.Length - 1 || i == splits2.Length - 1))
                        {
                            return   splits1.Length <splits2.Length ? 1 : -1;
                        }

                        return value;
                    }
                }

                return 0;

            });
            
            //基于标题进行遍历
            List<string> groups = new List<string>();

            foreach (SearchContextElement element in elements)
            {
                string[] entryTitle = element.title.Split('/');
                string groupName = "";

                for (int i = 0; i < entryTitle.Length-1; i++)
                {
                    groupName += entryTitle[i];
                    if (!groups.Contains(groupName))
                    {
                        //组
                        tree.Add(new SearchTreeGroupEntry(new GUIContent(entryTitle[i]),i+1));
                        groups.Add(groupName);
                    }

                    groupName += "/";
                }

                // Debug.Log("搜索树 "  +entryTitle.Last());
                //元素
                SearchTreeEntry entry = new SearchTreeEntry(new GUIContent(entryTitle.Last()));
                entry.level = entryTitle.Length;
                entry.userData = new SearchContextElement(element.target, element.title);
                tree.Add(entry);
                
            }

            return tree;            
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            var windowMousePosition = graph.ChangeCoordinatesTo(graph,context.screenMousePosition - graph.m_EditorWindow.position.position);
            var graphMousePosition = graph.contentViewContainer.WorldToLocal(windowMousePosition);
         
            SearchContextElement element = (SearchContextElement)SearchTreeEntry.userData;
            
            CodeGraphNode node = (CodeGraphNode)element.target;
            node.SetPosition(new Rect(graphMousePosition, new Vector2()));
            //在Graph Viwe中添加方法
            graph.Add(node);
            return true;
        }
    }
}