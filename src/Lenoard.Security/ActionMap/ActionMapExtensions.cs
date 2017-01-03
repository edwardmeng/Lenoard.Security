using System;
using System.Collections.Specialized;

namespace Lenoard.Security
{
    public static class ActionMapExtensions
    {
        private static ActionMapNode CreateNode(string nodeKey, string title, string description, NameValueCollection attributes)
        {
            var node = new ActionMapNode(nodeKey)
            {
                Title = title,
                Description = description,
            };
            if (attributes != null)
            {
                foreach (string name in attributes)
                {
                    node[name] = attributes[name];
                }
            }
            return node;
        }

        #region FindNode

        public static ActionMapNode FindNode(this IActionMapProvider provider, string key)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            key = key.Trim();
            if (key.Length == 0) return null;
            ActionMapNode node = null;
            provider.RootNodes.Traverse(x => x.ChildNodes, x =>
            {
                if (x.Key == key)
                {
                    if (node != null)
                    {
                        throw new InvalidOperationException($"Duplicate action map node with the same key: {key}.");
                    }
                    node = x;
                }
            });
            return node;
        }

        #endregion

        #region AddNode

        public static bool AddNode(this IActionMapProvider provider, string parentNode, string nodeKey, string title, string description, NameValueCollection attributes)
        {
            if (provider.FindNode(nodeKey) == null)
            {
                var parentSiteMapNode = provider.FindNode(parentNode);
                if (parentSiteMapNode == null)
                {
                    throw new ArgumentException($"The site map node '{parentNode}' cannot be found.");
                }
                parentSiteMapNode.ChildNodes.Add(CreateNode(nodeKey, title, description, attributes));
                return true;
            }
            return false;
        }

        public static bool AddNode(this IActionMapProvider provider, string parentNode, string nodeKey, string title, NameValueCollection attributes)
        {
            return provider.AddNode(parentNode, nodeKey, title, null, attributes);
        }

        public static bool AddNode(this IActionMapProvider provider, string parentNode, string nodeKey, string title, string description)
        {
            return provider.AddNode(parentNode, nodeKey, title, description, null);
        }

        public static bool AddNode(this IActionMapProvider provider, string parentNode, string nodeKey, string title)
        {
            return provider.AddNode(parentNode, nodeKey, title, (NameValueCollection)null);
        }

        #endregion

        #region AddRootNode

        public static bool AddRootNode(this IActionMapProvider provider, string nodeKey, string title, string description, NameValueCollection attributes)
        {
            if (provider.FindNode(nodeKey) == null)
            {
                provider.RootNodes.Add(CreateNode(nodeKey, title, description, attributes));
                return true;
            }
            return false;
        }

        public static bool AddRootNode(this IActionMapProvider provider, string nodeKey, string title, NameValueCollection attributes)
        {
            return provider.AddRootNode(nodeKey, title, null, attributes);
        }

        public static bool AddRootNode(this IActionMapProvider provider, string nodeKey, string title, string description)
        {
            return provider.AddRootNode(nodeKey, title, description, null);
        }

        public static bool AddRootNode(this IActionMapProvider provider, string nodeKey, string title)
        {
            return provider.AddRootNode(nodeKey, title, (NameValueCollection)null);
        }

        #endregion

        #region RemoveNode

        public static bool RemoveNode(this IActionMapProvider provider, string nodeKey)
        {
            var node = provider.FindNode(nodeKey);
            if (node == null) return false;
            var parentCollection = node.ParentNode?.ChildNodes ?? provider.RootNodes;
            parentCollection.Remove(node);
            return true;
        }

        #endregion
    }
}
