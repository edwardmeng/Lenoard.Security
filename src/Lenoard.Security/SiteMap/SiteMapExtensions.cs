using System;
using System.Collections.Specialized;

namespace Lenoard.Security
{
    /// <summary>
    /// Provides a set of <see langword="static"/> extension methods for the <see cref="ISiteMapStore"/>
    /// </summary>
    public static class SiteMapExtensions
    {
        private static SiteMapNode CreateNode(string nodeKey, string title, string url, string description, string requiredPermission, NameValueCollection attributes)
        {
            var node = new SiteMapNode(nodeKey)
            {
                Title = title,
                Url = url,
                Description = description,
                RequiredPermission = requiredPermission
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

        /// <summary>
        /// Retrieves a <see cref="SiteMapNode"/> object based on a specified key.
        /// </summary>
        /// <param name="store">The <see cref="ISiteMapStore"/> to lookup with.</param>
        /// <param name="key">A lookup key with which a <see cref="SiteMapNode"/> is created.</param>
        /// <returns>
        /// A <see cref="SiteMapNode"/> that represents the page identified by key; 
        /// otherwise, null, if no corresponding <see cref="SiteMapNode"/> is found.
        /// The default is null.
        /// </returns>
        /// <exception cref="ArgumentNullException">The <paramref name="store"/> or <paramref name="key"/> is null.</exception>
        public static SiteMapNode FindNode(this ISiteMapStore store, string key)
        {
            if (store == null)
            {
                throw new ArgumentNullException(nameof(store));
            }
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            key = key.Trim();
            if (key.Length == 0) return null;
            SiteMapNode node = null;
            store.RootNodes.Traverse(x => x.ChildNodes, x =>
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

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the hierarchy using the specified parent node key,
        /// URL, title, description, required action and additional attributes
        /// </summary>
        /// <param name="store">The <see cref="ISiteMapStore"/> to add node with.</param>
        /// <param name="parentNode">The parent node key</param>
        /// <param name="nodeKey">A store-specific lookup key.</param>
        /// <param name="url">The URL of the page that the node represents within the site.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <param name="description">A description of the page that the node represents.</param>
        /// <param name="requiredPermission">An string that controls the access permission to view the page represented by the <see cref="SiteMapNode"/>.</param>
        /// <param name="attributes">A <see cref="NameValueCollection"/> of additional attributes used to initialize the <see cref="SiteMapNode"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="store"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static SiteMapNode AddNode(this ISiteMapStore store, string parentNode, string nodeKey, string title, string url, string description, string requiredPermission, NameValueCollection attributes)
        {
            var node = store.FindNode(nodeKey);
            if (node == null)
            {
                var parentSiteMapNode = store.FindNode(parentNode);
                if (parentSiteMapNode == null)
                {
                    throw new ArgumentException($"The site map node '{parentNode}' cannot be found.");
                }
                node = CreateNode(nodeKey, title, url, description, requiredPermission, attributes);
                parentSiteMapNode.ChildNodes.Add(node);
            }
            return node;
        }

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the hierarchy using the specified parent node key,
        /// URL, title, description and additional attributes
        /// </summary>
        /// <param name="store">The <see cref="ISiteMapStore"/> to add node with.</param>
        /// <param name="parentNode">The parent node key</param>
        /// <param name="nodeKey">A store-specific lookup key.</param>
        /// <param name="url">The URL of the page that the node represents within the site.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <param name="description">A description of the page that the node represents.</param>
        /// <param name="attributes">A <see cref="NameValueCollection"/> of additional attributes used to initialize the <see cref="SiteMapNode"/>.</param>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="store"/> or <paramref name="nodeKey"/> is null.</exception>
        public static SiteMapNode AddNode(this ISiteMapStore store, string parentNode, string nodeKey, string title, string url, string description, NameValueCollection attributes)
        {
            return store.AddNode(parentNode, nodeKey, title, url, description, null, attributes);
        }

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the hierarchy using the specified parent node key,
        /// URL, title and additional attributes
        /// </summary>
        /// <param name="store">The <see cref="ISiteMapStore"/> to add node with.</param>
        /// <param name="parentNode">The parent node key</param>
        /// <param name="nodeKey">A store-specific lookup key.</param>
        /// <param name="url">The URL of the page that the node represents within the site.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <param name="attributes">A <see cref="NameValueCollection"/> of additional attributes used to initialize the <see cref="SiteMapNode"/>.</param>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="store"/> or <paramref name="nodeKey"/> is null.</exception>
        public static SiteMapNode AddNode(this ISiteMapStore store, string parentNode, string nodeKey, string title, string url, NameValueCollection attributes)
        {
            return store.AddNode(parentNode, nodeKey, title, url, null, attributes);
        }

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the hierarchy using the specified parent node key,
        /// title and additional attributes
        /// </summary>
        /// <param name="store">The <see cref="ISiteMapStore"/> to add node with.</param>
        /// <param name="parentNode">The parent node key</param>
        /// <param name="nodeKey">A store-specific lookup key.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <param name="attributes">A <see cref="NameValueCollection"/> of additional attributes used to initialize the <see cref="SiteMapNode"/>.</param>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="store"/> or <paramref name="nodeKey"/> is null.</exception>
        public static SiteMapNode AddNode(this ISiteMapStore store, string parentNode, string nodeKey, string title, NameValueCollection attributes)
        {
            return store.AddNode(parentNode, nodeKey, title, null, attributes);
        }

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the hierarchy using the specified parent node key,
        /// URL, title, description and required action.
        /// </summary>
        /// <param name="store">The <see cref="ISiteMapStore"/> to add node with.</param>
        /// <param name="parentNode">The parent node key</param>
        /// <param name="nodeKey">A store-specific lookup key.</param>
        /// <param name="url">The URL of the page that the node represents within the site.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <param name="description">A description of the page that the node represents.</param>
        /// <param name="requiredPermission">An string that controls the access permission to view the page represented by the <see cref="SiteMapNode"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="store"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static SiteMapNode AddNode(this ISiteMapStore store, string parentNode, string nodeKey, string title, string url, string description, string requiredPermission)
        {
            return store.AddNode(parentNode, nodeKey, title, url, description, requiredPermission, null);
        }

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the hierarchy using the specified parent node key,
        /// URL, title and description.
        /// </summary>
        /// <param name="store">The <see cref="ISiteMapStore"/> to add node with.</param>
        /// <param name="parentNode">The parent node key</param>
        /// <param name="nodeKey">A store-specific lookup key.</param>
        /// <param name="url">The URL of the page that the node represents within the site.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <param name="description">A description of the page that the node represents.</param>
        /// <exception cref="ArgumentNullException"><paramref name="store"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static SiteMapNode AddNode(this ISiteMapStore store, string parentNode, string nodeKey, string title, string url, string description)
        {
            return store.AddNode(parentNode, nodeKey, title, url, description, (NameValueCollection)null);
        }

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the hierarchy using the specified parent node key,
        /// URL and title.
        /// </summary>
        /// <param name="store">The <see cref="ISiteMapStore"/> to add node with.</param>
        /// <param name="parentNode">The parent node key</param>
        /// <param name="nodeKey">A store-specific lookup key.</param>
        /// <param name="url">The URL of the page that the node represents within the site.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <exception cref="ArgumentNullException"><paramref name="store"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static SiteMapNode AddNode(this ISiteMapStore store, string parentNode, string nodeKey, string title, string url)
        {
            return store.AddNode(parentNode, nodeKey, title, url, (NameValueCollection)null);
        }


        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the hierarchy using the specified parent node key
        /// and title.
        /// </summary>
        /// <param name="store">The <see cref="ISiteMapStore"/> to add node with.</param>
        /// <param name="parentNode">The parent node key</param>
        /// <param name="nodeKey">A store-specific lookup key.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <exception cref="ArgumentNullException"><paramref name="store"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static SiteMapNode AddNode(this ISiteMapStore store, string parentNode, string nodeKey, string title)
        {
            return store.AddNode(parentNode, nodeKey, title, (NameValueCollection)null);
        }

        #endregion

        #region AddRootNode

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the <see cref="ISiteMapStore.RootNodes"/> using the specified parent node key,
        /// URL, title, description, required action and additional attributes
        /// </summary>
        /// <param name="store">The <see cref="ISiteMapStore"/> to add node with.</param>
        /// <param name="nodeKey">A store-specific lookup key.</param>
        /// <param name="url">The URL of the page that the node represents within the site.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <param name="description">A description of the page that the node represents.</param>
        /// <param name="requiredPermission">An string that controls the access permission to view the page represented by the <see cref="SiteMapNode"/>.</param>
        /// <param name="attributes">A <see cref="NameValueCollection"/> of additional attributes used to initialize the <see cref="SiteMapNode"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="store"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static SiteMapNode AddRootNode(this ISiteMapStore store, string nodeKey, string title, string url, string description, string requiredPermission, NameValueCollection attributes)
        {
            var node = store.FindNode(nodeKey);
            if (node == null)
            {
                node = CreateNode(nodeKey, title, url, description, requiredPermission, attributes);
                store.RootNodes.Add(node);
            }
            return node;
        }

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the <see cref="ISiteMapStore.RootNodes"/> using the specified parent node key,
        /// URL, title, description and additional attributes
        /// </summary>
        /// <param name="store">The <see cref="ISiteMapStore"/> to add node with.</param>
        /// <param name="nodeKey">A store-specific lookup key.</param>
        /// <param name="url">The URL of the page that the node represents within the site.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <param name="description">A description of the page that the node represents.</param>
        /// <param name="attributes">A <see cref="NameValueCollection"/> of additional attributes used to initialize the <see cref="SiteMapNode"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="store"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static SiteMapNode AddRootNode(this ISiteMapStore store, string nodeKey, string title, string url, string description, NameValueCollection attributes)
        {
            return store.AddRootNode(nodeKey, title, url, description, null, attributes);
        }

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the <see cref="ISiteMapStore.RootNodes"/> using the specified parent node key,
        /// URL, title and additional attributes
        /// </summary>
        /// <param name="store">The <see cref="ISiteMapStore"/> to add node with.</param>
        /// <param name="nodeKey">A store-specific lookup key.</param>
        /// <param name="url">The URL of the page that the node represents within the site.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <param name="attributes">A <see cref="NameValueCollection"/> of additional attributes used to initialize the <see cref="SiteMapNode"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="store"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static SiteMapNode AddRootNode(this ISiteMapStore store, string nodeKey, string title, string url, NameValueCollection attributes)
        {
            return store.AddRootNode(nodeKey, title, url, null, attributes);
        }

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the <see cref="ISiteMapStore.RootNodes"/> using the specified parent node key,
        /// title and additional attributes
        /// </summary>
        /// <param name="store">The <see cref="ISiteMapStore"/> to add node with.</param>
        /// <param name="nodeKey">A store-specific lookup key.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <param name="attributes">A <see cref="NameValueCollection"/> of additional attributes used to initialize the <see cref="SiteMapNode"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="store"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static SiteMapNode AddRootNode(this ISiteMapStore store, string nodeKey, string title, NameValueCollection attributes)
        {
            return store.AddRootNode(nodeKey, title, null, attributes);
        }

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the <see cref="ISiteMapStore.RootNodes"/> using the specified parent node key,
        /// URL, title, description and required action.
        /// </summary>
        /// <param name="store">The <see cref="ISiteMapStore"/> to add node with.</param>
        /// <param name="nodeKey">A store-specific lookup key.</param>
        /// <param name="url">The URL of the page that the node represents within the site.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <param name="description">A description of the page that the node represents.</param>
        /// <param name="requiredPermission">An action that controls the access permission to view the page represented by the <see cref="SiteMapNode"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="store"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static SiteMapNode AddRootNode(this ISiteMapStore store, string nodeKey, string title, string url, string description, string requiredPermission)
        {
            return store.AddRootNode(nodeKey, title, url, description, requiredPermission, null);
        }

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the <see cref="ISiteMapStore.RootNodes"/> using the specified parent node key,
        /// URL, title and description.
        /// </summary>
        /// <param name="store">The <see cref="ISiteMapStore"/> to add node with.</param>
        /// <param name="nodeKey">A store-specific lookup key.</param>
        /// <param name="url">The URL of the page that the node represents within the site.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <param name="description">A description of the page that the node represents.</param>
        /// <exception cref="ArgumentNullException"><paramref name="store"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static SiteMapNode AddRootNode(this ISiteMapStore store, string nodeKey, string title, string url, string description)
        {
            return store.AddRootNode(nodeKey, title, url, description, (NameValueCollection)null);
        }

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the <see cref="ISiteMapStore.RootNodes"/> using the specified parent node key,
        /// URL and title.
        /// </summary>
        /// <param name="store">The <see cref="ISiteMapStore"/> to add node with.</param>
        /// <param name="nodeKey">A store-specific lookup key.</param>
        /// <param name="url">The URL of the page that the node represents within the site.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <exception cref="ArgumentNullException"><paramref name="store"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static SiteMapNode AddRootNode(this ISiteMapStore store, string nodeKey, string title, string url)
        {
            return store.AddRootNode(nodeKey, title, url, (NameValueCollection)null);
        }

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the <see cref="ISiteMapStore.RootNodes"/> using the specified parent node key,
        /// and title.
        /// </summary>
        /// <param name="store">The <see cref="ISiteMapStore"/> to add node with.</param>
        /// <param name="nodeKey">A store-specific lookup key.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <exception cref="ArgumentNullException"><paramref name="store"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static SiteMapNode AddRootNode(this ISiteMapStore store, string nodeKey, string title)
        {
            return store.AddRootNode(nodeKey, title, (NameValueCollection)null);
        }

        #endregion

        #region AddNodeBefore

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the hierarchy before the specified sibling node 
        /// using the specified node key, URL, title, description, required action and additional attributes
        /// </summary>
        /// <param name="store">The <see cref="ISiteMapStore"/> to add node with.</param>
        /// <param name="siblingNodeKey">The sibling node key</param>
        /// <param name="nodeKey">A store-specific lookup key.</param>
        /// <param name="url">The URL of the page that the node represents within the site.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <param name="description">A description of the page that the node represents.</param>
        /// <param name="requiredPermission">An string that controls the access permission to view the page represented by the <see cref="SiteMapNode"/>.</param>
        /// <param name="attributes">A <see cref="NameValueCollection"/> of additional attributes used to initialize the <see cref="SiteMapNode"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="store"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static SiteMapNode AddNodeBefore(this ISiteMapStore store, string siblingNodeKey, string nodeKey, string title, string url, string description, string requiredPermission, NameValueCollection attributes)
        {
            var node = store.FindNode(nodeKey);
            if (node == null)
            {
                var siblingNode = store.FindNode(siblingNodeKey);
                if (siblingNode == null)
                {
                    throw new ArgumentException($"The site map node '{siblingNodeKey}' cannot be found.");
                }
                var parentCollection = siblingNode.ParentNode?.ChildNodes ?? store.RootNodes;
                node = CreateNode(nodeKey, title, url, description, requiredPermission, attributes);
                parentCollection.Insert(parentCollection.IndexOf(siblingNode), node);
            }
            return node;
        }

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the hierarchy before the specified sibling node 
        /// using the specified node key, URL, title, description and additional attributes
        /// </summary>
        /// <param name="store">The <see cref="ISiteMapStore"/> to add node with.</param>
        /// <param name="siblingNodeKey">The sibling node key</param>
        /// <param name="nodeKey">A store-specific lookup key.</param>
        /// <param name="url">The URL of the page that the node represents within the site.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <param name="description">A description of the page that the node represents.</param>
        /// <param name="attributes">A <see cref="NameValueCollection"/> of additional attributes used to initialize the <see cref="SiteMapNode"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="store"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static SiteMapNode AddNodeBefore(this ISiteMapStore store, string siblingNodeKey, string nodeKey, string title, string url, string description, NameValueCollection attributes)
        {
            return store.AddNodeBefore(siblingNodeKey, nodeKey, title, url, description, null, attributes);
        }

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the hierarchy before the specified sibling node 
        /// using the specified node key, URL, title and additional attributes
        /// </summary>
        /// <param name="store">The <see cref="ISiteMapStore"/> to add node with.</param>
        /// <param name="siblingNodeKey">The sibling node key</param>
        /// <param name="nodeKey">A store-specific lookup key.</param>
        /// <param name="url">The URL of the page that the node represents within the site.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <param name="attributes">A <see cref="NameValueCollection"/> of additional attributes used to initialize the <see cref="SiteMapNode"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="store"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static SiteMapNode AddNodeBefore(this ISiteMapStore store, string siblingNodeKey, string nodeKey, string title, string url, NameValueCollection attributes)
        {
            return store.AddNodeBefore(siblingNodeKey, nodeKey, title, url, null, attributes);
        }

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the hierarchy before the specified sibling node 
        /// using the specified node key, title and additional attributes
        /// </summary>
        /// <param name="store">The <see cref="ISiteMapStore"/> to add node with.</param>
        /// <param name="siblingNodeKey">The sibling node key</param>
        /// <param name="nodeKey">A store-specific lookup key.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <param name="attributes">A <see cref="NameValueCollection"/> of additional attributes used to initialize the <see cref="SiteMapNode"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="store"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static SiteMapNode AddNodeBefore(this ISiteMapStore store, string siblingNodeKey, string nodeKey, string title, NameValueCollection attributes)
        {
            return store.AddNodeBefore(siblingNodeKey, nodeKey, title, null, attributes);
        }

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the hierarchy before the specified sibling node 
        /// using the specified node key, URL, title, description and required action
        /// </summary>
        /// <param name="store">The <see cref="ISiteMapStore"/> to add node with.</param>
        /// <param name="siblingNodeKey">The sibling node key</param>
        /// <param name="nodeKey">A store-specific lookup key.</param>
        /// <param name="url">The URL of the page that the node represents within the site.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <param name="description">A description of the page that the node represents.</param>
        /// <param name="requiredPermission">An string that controls the access permission to view the page represented by the <see cref="SiteMapNode"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="store"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static SiteMapNode AddNodeBefore(this ISiteMapStore store, string siblingNodeKey, string nodeKey, string title, string url, string description, string requiredPermission)
        {
            return store.AddNodeBefore(siblingNodeKey, nodeKey, title, url, description, requiredPermission, null);
        }

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the hierarchy before the specified sibling node 
        /// using the specified node key, URL, title and description
        /// </summary>
        /// <param name="store">The <see cref="ISiteMapStore"/> to add node with.</param>
        /// <param name="siblingNodeKey">The sibling node key</param>
        /// <param name="nodeKey">A store-specific lookup key.</param>
        /// <param name="url">The URL of the page that the node represents within the site.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <param name="description">A description of the page that the node represents.</param>
        /// <exception cref="ArgumentNullException"><paramref name="store"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static SiteMapNode AddNodeBefore(this ISiteMapStore store, string siblingNodeKey, string nodeKey, string title, string url, string description)
        {
            return store.AddNodeBefore(siblingNodeKey, nodeKey, title, url, description, (NameValueCollection)null);
        }

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the hierarchy before the specified sibling node 
        /// using the specified node key, URL and title
        /// </summary>
        /// <param name="store">The <see cref="ISiteMapStore"/> to add node with.</param>
        /// <param name="siblingNodeKey">The sibling node key</param>
        /// <param name="nodeKey">A store-specific lookup key.</param>
        /// <param name="url">The URL of the page that the node represents within the site.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <exception cref="ArgumentNullException"><paramref name="store"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static SiteMapNode AddNodeBefore(this ISiteMapStore store, string siblingNodeKey, string nodeKey, string title, string url)
        {
            return store.AddNodeBefore(siblingNodeKey, nodeKey, title, url, (NameValueCollection)null);
        }

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the hierarchy before the specified sibling node 
        /// using the specified node key and title.
        /// </summary>
        /// <param name="store">The <see cref="ISiteMapStore"/> to add node with.</param>
        /// <param name="siblingNodeKey">The sibling node key</param>
        /// <param name="nodeKey">A store-specific lookup key.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <exception cref="ArgumentNullException"><paramref name="store"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static SiteMapNode AddNodeBefore(this ISiteMapStore store, string siblingNodeKey, string nodeKey, string title)
        {
            return store.AddNodeBefore(siblingNodeKey, nodeKey, title, (NameValueCollection)null);
        }

        #endregion

        #region AddNodeAfter

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the hierarchy after the specified sibling node 
        /// using the specified node key, URL, title, description, required action and additional attributes
        /// </summary>
        /// <param name="store">The <see cref="ISiteMapStore"/> to add node with.</param>
        /// <param name="siblingNodeKey">The sibling node key</param>
        /// <param name="nodeKey">A store-specific lookup key.</param>
        /// <param name="url">The URL of the page that the node represents within the site.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <param name="description">A description of the page that the node represents.</param>
        /// <param name="requiredPermission">An string that controls the access permission to view the page represented by the <see cref="SiteMapNode"/>.</param>
        /// <param name="attributes">A <see cref="NameValueCollection"/> of additional attributes used to initialize the <see cref="SiteMapNode"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="store"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static SiteMapNode AddNodeAfter(this ISiteMapStore store, string siblingNodeKey, string nodeKey, string title, string url, string description, string requiredPermission, NameValueCollection attributes)
        {
            var node = store.FindNode(nodeKey);
            if (node == null)
            {
                var siblingNode = store.FindNode(siblingNodeKey);
                if (siblingNode == null)
                {
                    throw new ArgumentException($"The site map node '{siblingNodeKey}' cannot be found.");
                }
                var parentCollection = siblingNode.ParentNode?.ChildNodes ?? store.RootNodes;
                node = CreateNode(nodeKey, title, url, description, requiredPermission, attributes);
                parentCollection.Insert(parentCollection.IndexOf(siblingNode) + 1, node);
            }
            return node;
        }

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the hierarchy after the specified sibling node 
        /// using the specified node key, URL, title, description and additional attributes
        /// </summary>
        /// <param name="store">The <see cref="ISiteMapStore"/> to add node with.</param>
        /// <param name="siblingNodeKey">The sibling node key</param>
        /// <param name="nodeKey">A store-specific lookup key.</param>
        /// <param name="url">The URL of the page that the node represents within the site.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <param name="description">A description of the page that the node represents.</param>
        /// <param name="attributes">A <see cref="NameValueCollection"/> of additional attributes used to initialize the <see cref="SiteMapNode"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="store"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static SiteMapNode AddNodeAfter(this ISiteMapStore store, string siblingNodeKey, string nodeKey, string title, string url, string description, NameValueCollection attributes)
        {
            return store.AddNodeAfter(siblingNodeKey, nodeKey, title, url, description, null, attributes);
        }

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the hierarchy after the specified sibling node 
        /// using the specified node key, URL, title and additional attributes
        /// </summary>
        /// <param name="store">The <see cref="ISiteMapStore"/> to add node with.</param>
        /// <param name="siblingNodeKey">The sibling node key</param>
        /// <param name="nodeKey">A store-specific lookup key.</param>
        /// <param name="url">The URL of the page that the node represents within the site.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <param name="attributes">A <see cref="NameValueCollection"/> of additional attributes used to initialize the <see cref="SiteMapNode"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="store"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static SiteMapNode AddNodeAfter(this ISiteMapStore store, string siblingNodeKey, string nodeKey, string title, string url, NameValueCollection attributes)
        {
            return store.AddNodeAfter(siblingNodeKey, nodeKey, title, url, null, attributes);
        }

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the hierarchy after the specified sibling node 
        /// using the specified node key, title and additional attributes
        /// </summary>
        /// <param name="store">The <see cref="ISiteMapStore"/> to add node with.</param>
        /// <param name="siblingNodeKey">The sibling node key</param>
        /// <param name="nodeKey">A store-specific lookup key.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <param name="attributes">A <see cref="NameValueCollection"/> of additional attributes used to initialize the <see cref="SiteMapNode"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="store"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static SiteMapNode AddNodeAfter(this ISiteMapStore store, string siblingNodeKey, string nodeKey, string title, NameValueCollection attributes)
        {
            return store.AddNodeAfter(siblingNodeKey, nodeKey, title, null, attributes);
        }

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the hierarchy after the specified sibling node 
        /// using the specified node key, URL, title, description and required action
        /// </summary>
        /// <param name="store">The <see cref="ISiteMapStore"/> to add node with.</param>
        /// <param name="siblingNodeKey">The sibling node key</param>
        /// <param name="nodeKey">A store-specific lookup key.</param>
        /// <param name="url">The URL of the page that the node represents within the site.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <param name="description">A description of the page that the node represents.</param>
        /// <param name="requiredPermission">An string that controls the access permission to view the page represented by the <see cref="SiteMapNode"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="store"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static SiteMapNode AddNodeAfter(this ISiteMapStore store, string siblingNodeKey, string nodeKey, string title, string url, string description, string requiredPermission)
        {
            return store.AddNodeAfter(siblingNodeKey, nodeKey, title, url, description, requiredPermission, null);
        }

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the hierarchy after the specified sibling node 
        /// using the specified node key, URL, title and description
        /// </summary>
        /// <param name="store">The <see cref="ISiteMapStore"/> to add node with.</param>
        /// <param name="siblingNodeKey">The sibling node key</param>
        /// <param name="nodeKey">A store-specific lookup key.</param>
        /// <param name="url">The URL of the page that the node represents within the site.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <param name="description">A description of the page that the node represents.</param>
        /// <exception cref="ArgumentNullException"><paramref name="store"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static SiteMapNode AddNodeAfter(this ISiteMapStore store, string siblingNodeKey, string nodeKey, string title, string url, string description)
        {
            return store.AddNodeAfter(siblingNodeKey, nodeKey, title, url, description, (NameValueCollection)null);
        }

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the hierarchy after the specified sibling node 
        /// using the specified node key, URL and title
        /// </summary>
        /// <param name="store">The <see cref="ISiteMapStore"/> to add node with.</param>
        /// <param name="siblingNodeKey">The sibling node key</param>
        /// <param name="nodeKey">A store-specific lookup key.</param>
        /// <param name="url">The URL of the page that the node represents within the site.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <exception cref="ArgumentNullException"><paramref name="store"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static SiteMapNode AddNodeAfter(this ISiteMapStore store, string siblingNodeKey, string nodeKey, string title, string url)
        {
            return store.AddNodeAfter(siblingNodeKey, nodeKey, title, url, (NameValueCollection)null);
        }

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the hierarchy after the specified sibling node 
        /// using the specified node key and title.
        /// </summary>
        /// <param name="store">The <see cref="ISiteMapStore"/> to add node with.</param>
        /// <param name="siblingNodeKey">The sibling node key</param>
        /// <param name="nodeKey">A store-specific lookup key.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <exception cref="ArgumentNullException"><paramref name="store"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static SiteMapNode AddNodeAfter(this ISiteMapStore store, string siblingNodeKey, string nodeKey, string title)
        {
            return store.AddNodeAfter(siblingNodeKey, nodeKey, title, (NameValueCollection)null);
        }

        #endregion

        #region RemoveNode

        /// <summary>
        /// Removes the specified <see cref="SiteMapNode"/> from the hierarchy by using the specified node key.
        /// </summary>
        /// <param name="store">The <see cref="ISiteMapStore"/> to remove node with.</param>
        /// <param name="nodeKey">A store-specific lookup key.</param>
        /// <returns><c>true</c> if the <see cref="SiteMapNode"/> removed from the hierarchy; otherwise <c>false</c>.</returns>
        public static bool RemoveNode(this ISiteMapStore store, string nodeKey)
        {
            var node = store.FindNode(nodeKey);
            if (node == null) return false;
            var parentCollection = node.ParentNode?.ChildNodes ?? store.RootNodes;
            parentCollection.Remove(node);
            return true;
        }

        #endregion
    }
}
