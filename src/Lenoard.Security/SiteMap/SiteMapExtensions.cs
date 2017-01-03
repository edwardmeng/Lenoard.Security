using System;
using System.Collections.Specialized;

namespace Lenoard.Security
{
    public static class SiteMapExtensions
    {
        private static SiteMapNode CreateNode(string nodeKey, string title, string url, string description, string requiredAction, NameValueCollection attributes)
        {
            var node = new SiteMapNode(nodeKey)
            {
                Title = title,
                Url = url,
                Description = description,
                RequiredAction = requiredAction
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

        public static SiteMapNode FindNode(this ISiteMapProvider provider, string key)
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
            SiteMapNode node = null;
            provider.RootNodes.Traverse(x => x.ChildNodes, x =>
            {
                if (x.Key == key)
                {
                    if (node != null)
                    {
                        throw new InvalidOperationException($"Duplicate site map node with the same key: {key}.");
                    }
                    node = x;
                }
            });
            return node;
        }

        #endregion

        #region AddNode

        public static bool AddNode(this ISiteMapProvider provider, string parentNode, string nodeKey, string title, string url, string description, string requiredAction, NameValueCollection attributes)
        {
            if (provider.FindNode(nodeKey) == null)
            {
                var parentSiteMapNode = provider.FindNode(parentNode);
                if (parentSiteMapNode == null)
                {
                    throw new ArgumentException($"The site map node '{parentNode}' cannot be found.");
                }
                parentSiteMapNode.ChildNodes.Add(CreateNode(nodeKey, title, url, description, requiredAction, attributes));
                return true;
            }
            return false;
        }

        public static bool AddNode(this ISiteMapProvider provider, string parentNode, string nodeKey, string title, string url, string description, NameValueCollection attributes)
        {
            return provider.AddNode(parentNode, nodeKey, title, url, description, null, attributes);
        }

        public static bool AddNode(this ISiteMapProvider provider, string parentNode, string nodeKey, string title, string url, NameValueCollection attributes)
        {
            return provider.AddNode(parentNode, nodeKey, title, url, null, attributes);
        }

        public static bool AddNode(this ISiteMapProvider provider, string parentNode, string nodeKey, string title, NameValueCollection attributes)
        {
            return provider.AddNode(parentNode, nodeKey, title, null, attributes);
        }

        public static bool AddNode(this ISiteMapProvider provider, string parentNode, string nodeKey, string title, string url, string description, string requiredAction)
        {
            return provider.AddNode(parentNode, nodeKey, title, url, description, requiredAction, null);
        }

        public static bool AddNode(this ISiteMapProvider provider, string parentNode, string nodeKey, string title, string url, string description)
        {
            return provider.AddNode(parentNode, nodeKey, title, url, description, (NameValueCollection)null);
        }

        public static bool AddNode(this ISiteMapProvider provider, string parentNode, string nodeKey, string title, string url)
        {
            return provider.AddNode(parentNode, nodeKey, title, url, (NameValueCollection)null);
        }

        public static bool AddNode(this ISiteMapProvider provider, string parentNode, string nodeKey, string title)
        {
            return provider.AddNode(parentNode, nodeKey, title, (NameValueCollection)null);
        }

        #endregion

        #region AddRootNode

        public static bool AddRootNode(this ISiteMapProvider provider, string nodeKey, string title, string url, string description, string requiredAction, NameValueCollection attributes)
        {
            if (provider.FindNode(nodeKey) == null)
            {
                provider.RootNodes.Add(CreateNode(nodeKey, title, url, description, requiredAction, attributes));
                return true;
            }
            return false;
        }

        public static bool AddRootNode(this ISiteMapProvider provider, string nodeKey, string title, string url, string description, NameValueCollection attributes)
        {
            return provider.AddRootNode(nodeKey, title, url, description, null, attributes);
        }

        public static bool AddRootNode(this ISiteMapProvider provider, string nodeKey, string title, string url, NameValueCollection attributes)
        {
            return provider.AddRootNode(nodeKey, title, url, null, attributes);
        }

        public static bool AddRootNode(this ISiteMapProvider provider, string nodeKey, string title, NameValueCollection attributes)
        {
            return provider.AddRootNode(nodeKey, title, null, attributes);
        }

        public static bool AddRootNode(this ISiteMapProvider provider, string nodeKey, string title, string url, string description, string requiredAction)
        {
            return provider.AddRootNode(nodeKey, title, url, description, requiredAction, null);
        }

        public static bool AddRootNode(this ISiteMapProvider provider, string nodeKey, string title, string url, string description)
        {
            return provider.AddRootNode(nodeKey, title, url, description, (NameValueCollection)null);
        }

        public static bool AddRootNode(this ISiteMapProvider provider, string nodeKey, string title, string url)
        {
            return provider.AddRootNode(nodeKey, title, url, (NameValueCollection)null);
        }

        public static bool AddRootNode(this ISiteMapProvider provider, string nodeKey, string title)
        {
            return provider.AddRootNode(nodeKey, title, (NameValueCollection)null);
        }

        #endregion

        #region AddNodeBefore

        public static bool AddNodeBefore(this ISiteMapProvider provider, string siblingNodeKey, string nodeKey, string title, string url, string description, string requiredAction, NameValueCollection attributes)
        {
            if (provider.FindNode(nodeKey) == null)
            {
                var siblingNode = provider.FindNode(siblingNodeKey);
                if (siblingNode == null)
                {
                    throw new ArgumentException($"The site map node '{siblingNodeKey}' cannot be found.");
                }
                var parentCollection = siblingNode.ParentNode?.ChildNodes ?? provider.RootNodes;
                parentCollection.Insert(parentCollection.IndexOf(siblingNode), CreateNode(nodeKey, title, url, description, requiredAction, attributes));
                return true;
            }
            return false;
        }

        public static bool AddNodeBefore(this ISiteMapProvider provider, string siblingNodeKey, string nodeKey, string title, string url, string description, NameValueCollection attributes)
        {
            return provider.AddNodeBefore(siblingNodeKey, nodeKey, title, url, description, null, attributes);
        }

        public static bool AddNodeBefore(this ISiteMapProvider provider, string siblingNodeKey, string nodeKey, string title, string url, NameValueCollection attributes)
        {
            return provider.AddNodeBefore(siblingNodeKey, nodeKey, title, url, null, attributes);
        }

        public static bool AddNodeBefore(this ISiteMapProvider provider, string siblingNodeKey, string nodeKey, string title, NameValueCollection attributes)
        {
            return provider.AddNodeBefore(siblingNodeKey, nodeKey, title, null, attributes);
        }

        public static bool AddNodeBefore(this ISiteMapProvider provider, string siblingNodeKey, string nodeKey, string title, string url, string description, string requiredAction)
        {
            return provider.AddNodeBefore(siblingNodeKey, nodeKey, title, url, description, requiredAction, null);
        }

        public static bool AddNodeBefore(this ISiteMapProvider provider, string siblingNodeKey, string nodeKey, string title, string url, string description)
        {
            return provider.AddNodeBefore(siblingNodeKey, nodeKey, title, url, description, (NameValueCollection)null);
        }

        public static bool AddNodeBefore(this ISiteMapProvider provider, string siblingNodeKey, string nodeKey, string title, string url)
        {
            return provider.AddNodeBefore(siblingNodeKey, nodeKey, title, url, (NameValueCollection)null);
        }

        public static bool AddNodeBefore(this ISiteMapProvider provider, string siblingNodeKey, string nodeKey, string title)
        {
            return provider.AddNodeBefore(siblingNodeKey, nodeKey, title, (NameValueCollection)null);
        }

        #endregion

        #region AddNodeAfter

        public static bool AddNodeAfter(this ISiteMapProvider provider, string siblingNodeKey, string nodeKey, string title, string url, string description, string requiredAction, NameValueCollection attributes)
        {
            if (provider.FindNode(nodeKey) == null)
            {
                var siblingNode = provider.FindNode(siblingNodeKey);
                if (siblingNode == null)
                {
                    throw new ArgumentException($"The site map node '{siblingNodeKey}' cannot be found.");
                }
                var parentCollection = siblingNode.ParentNode?.ChildNodes ?? provider.RootNodes;
                parentCollection.Insert(parentCollection.IndexOf(siblingNode) + 1, CreateNode(nodeKey, title, url, description, requiredAction, attributes));
                return true;
            }
            return false;
        }

        public static bool AddNodeAfter(this ISiteMapProvider provider, string siblingNodeKey, string nodeKey, string title, string url, string description, NameValueCollection attributes)
        {
            return provider.AddNodeAfter(siblingNodeKey, nodeKey, title, url, description, null, attributes);
        }

        public static bool AddNodeAfter(this ISiteMapProvider provider, string siblingNodeKey, string nodeKey, string title, string url, NameValueCollection attributes)
        {
            return provider.AddNodeAfter(siblingNodeKey, nodeKey, title, url, null, attributes);
        }

        public static bool AddNodeAfter(this ISiteMapProvider provider, string siblingNodeKey, string nodeKey, string title, NameValueCollection attributes)
        {
            return provider.AddNodeAfter(siblingNodeKey, nodeKey, title, null, attributes);
        }

        public static bool AddNodeAfter(this ISiteMapProvider provider, string siblingNodeKey, string nodeKey, string title, string url, string description, string requiredAction)
        {
            return provider.AddNodeAfter(siblingNodeKey, nodeKey, title, url, description, requiredAction, null);
        }

        public static bool AddNodeAfter(this ISiteMapProvider provider, string siblingNodeKey, string nodeKey, string title, string url, string description)
        {
            return provider.AddNodeAfter(siblingNodeKey, nodeKey, title, url, description, (NameValueCollection)null);
        }

        public static bool AddNodeAfter(this ISiteMapProvider provider, string siblingNodeKey, string nodeKey, string title, string url)
        {
            return provider.AddNodeAfter(siblingNodeKey, nodeKey, title, url, (NameValueCollection)null);
        }

        public static bool AddNodeAfter(this ISiteMapProvider provider, string siblingNodeKey, string nodeKey, string title)
        {
            return provider.AddNodeAfter(siblingNodeKey, nodeKey, title, (NameValueCollection)null);
        }

        #endregion

        #region RemoveNode

        public static bool RemoveNode(this ISiteMapProvider provider, string nodeKey)
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
