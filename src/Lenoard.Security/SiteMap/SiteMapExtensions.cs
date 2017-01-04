using System;
using System.Collections.Specialized;

namespace Lenoard.Security
{
    /// <summary>
    /// Provides a set of <see langword="static"/> extension methods for the <see cref="ISiteMapProvider"/>
    /// </summary>
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

        /// <summary>
        /// Retrieves a <see cref="SiteMapNode"/> object based on a specified key.
        /// </summary>
        /// <param name="provider">The <see cref="ISiteMapProvider"/> to lookup with.</param>
        /// <param name="key">A lookup key with which a <see cref="SiteMapNode"/> is created.</param>
        /// <returns>
        /// A <see cref="SiteMapNode"/> that represents the page identified by key; 
        /// otherwise, null, if no corresponding <see cref="SiteMapNode"/> is found.
        /// The default is null.
        /// </returns>
        /// <exception cref="ArgumentNullException">The <paramref name="provider"/> or <paramref name="key"/> is null.</exception>
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

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the hierarchy using the specified parent node key,
        /// URL, title, description, required action and additional attributes
        /// </summary>
        /// <param name="provider">The <see cref="ISiteMapProvider"/> to add node with.</param>
        /// <param name="parentNode">The parent node key</param>
        /// <param name="nodeKey">A provider-specific lookup key.</param>
        /// <param name="url">The URL of the page that the node represents within the site.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <param name="description">A description of the page that the node represents.</param>
        /// <param name="requiredAction">An action that controls the access permission to view the page represented by the <see cref="SiteMapNode"/>.</param>
        /// <param name="attributes">A <see cref="NameValueCollection"/> of additional attributes used to initialize the <see cref="SiteMapNode"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="provider"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static SiteMapNode AddNode(this ISiteMapProvider provider, string parentNode, string nodeKey, string title, string url, string description, string requiredAction, NameValueCollection attributes)
        {
            var node = provider.FindNode(nodeKey);
            if (node == null)
            {
                var parentSiteMapNode = provider.FindNode(parentNode);
                if (parentSiteMapNode == null)
                {
                    throw new ArgumentException($"The site map node '{parentNode}' cannot be found.");
                }
                node = CreateNode(nodeKey, title, url, description, requiredAction, attributes);
                parentSiteMapNode.ChildNodes.Add(node);
            }
            return node;
        }

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the hierarchy using the specified parent node key,
        /// URL, title, description and additional attributes
        /// </summary>
        /// <param name="provider">The <see cref="ISiteMapProvider"/> to add node with.</param>
        /// <param name="parentNode">The parent node key</param>
        /// <param name="nodeKey">A provider-specific lookup key.</param>
        /// <param name="url">The URL of the page that the node represents within the site.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <param name="description">A description of the page that the node represents.</param>
        /// <param name="attributes">A <see cref="NameValueCollection"/> of additional attributes used to initialize the <see cref="SiteMapNode"/>.</param>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="provider"/> or <paramref name="nodeKey"/> is null.</exception>
        public static SiteMapNode AddNode(this ISiteMapProvider provider, string parentNode, string nodeKey, string title, string url, string description, NameValueCollection attributes)
        {
            return provider.AddNode(parentNode, nodeKey, title, url, description, null, attributes);
        }

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the hierarchy using the specified parent node key,
        /// URL, title and additional attributes
        /// </summary>
        /// <param name="provider">The <see cref="ISiteMapProvider"/> to add node with.</param>
        /// <param name="parentNode">The parent node key</param>
        /// <param name="nodeKey">A provider-specific lookup key.</param>
        /// <param name="url">The URL of the page that the node represents within the site.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <param name="attributes">A <see cref="NameValueCollection"/> of additional attributes used to initialize the <see cref="SiteMapNode"/>.</param>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="provider"/> or <paramref name="nodeKey"/> is null.</exception>
        public static SiteMapNode AddNode(this ISiteMapProvider provider, string parentNode, string nodeKey, string title, string url, NameValueCollection attributes)
        {
            return provider.AddNode(parentNode, nodeKey, title, url, null, attributes);
        }

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the hierarchy using the specified parent node key,
        /// title and additional attributes
        /// </summary>
        /// <param name="provider">The <see cref="ISiteMapProvider"/> to add node with.</param>
        /// <param name="parentNode">The parent node key</param>
        /// <param name="nodeKey">A provider-specific lookup key.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <param name="attributes">A <see cref="NameValueCollection"/> of additional attributes used to initialize the <see cref="SiteMapNode"/>.</param>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="provider"/> or <paramref name="nodeKey"/> is null.</exception>
        public static SiteMapNode AddNode(this ISiteMapProvider provider, string parentNode, string nodeKey, string title, NameValueCollection attributes)
        {
            return provider.AddNode(parentNode, nodeKey, title, null, attributes);
        }

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the hierarchy using the specified parent node key,
        /// URL, title, description and required action.
        /// </summary>
        /// <param name="provider">The <see cref="ISiteMapProvider"/> to add node with.</param>
        /// <param name="parentNode">The parent node key</param>
        /// <param name="nodeKey">A provider-specific lookup key.</param>
        /// <param name="url">The URL of the page that the node represents within the site.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <param name="description">A description of the page that the node represents.</param>
        /// <param name="requiredAction">An action that controls the access permission to view the page represented by the <see cref="SiteMapNode"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="provider"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static SiteMapNode AddNode(this ISiteMapProvider provider, string parentNode, string nodeKey, string title, string url, string description, string requiredAction)
        {
            return provider.AddNode(parentNode, nodeKey, title, url, description, requiredAction, null);
        }

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the hierarchy using the specified parent node key,
        /// URL, title and description.
        /// </summary>
        /// <param name="provider">The <see cref="ISiteMapProvider"/> to add node with.</param>
        /// <param name="parentNode">The parent node key</param>
        /// <param name="nodeKey">A provider-specific lookup key.</param>
        /// <param name="url">The URL of the page that the node represents within the site.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <param name="description">A description of the page that the node represents.</param>
        /// <exception cref="ArgumentNullException"><paramref name="provider"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static SiteMapNode AddNode(this ISiteMapProvider provider, string parentNode, string nodeKey, string title, string url, string description)
        {
            return provider.AddNode(parentNode, nodeKey, title, url, description, (NameValueCollection)null);
        }

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the hierarchy using the specified parent node key,
        /// URL and title.
        /// </summary>
        /// <param name="provider">The <see cref="ISiteMapProvider"/> to add node with.</param>
        /// <param name="parentNode">The parent node key</param>
        /// <param name="nodeKey">A provider-specific lookup key.</param>
        /// <param name="url">The URL of the page that the node represents within the site.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <exception cref="ArgumentNullException"><paramref name="provider"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static SiteMapNode AddNode(this ISiteMapProvider provider, string parentNode, string nodeKey, string title, string url)
        {
            return provider.AddNode(parentNode, nodeKey, title, url, (NameValueCollection)null);
        }


        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the hierarchy using the specified parent node key
        /// and title.
        /// </summary>
        /// <param name="provider">The <see cref="ISiteMapProvider"/> to add node with.</param>
        /// <param name="parentNode">The parent node key</param>
        /// <param name="nodeKey">A provider-specific lookup key.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <exception cref="ArgumentNullException"><paramref name="provider"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static SiteMapNode AddNode(this ISiteMapProvider provider, string parentNode, string nodeKey, string title)
        {
            return provider.AddNode(parentNode, nodeKey, title, (NameValueCollection)null);
        }

        #endregion

        #region AddRootNode

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the <see cref="ISiteMapProvider.RootNodes"/> using the specified parent node key,
        /// URL, title, description, required action and additional attributes
        /// </summary>
        /// <param name="provider">The <see cref="ISiteMapProvider"/> to add node with.</param>
        /// <param name="nodeKey">A provider-specific lookup key.</param>
        /// <param name="url">The URL of the page that the node represents within the site.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <param name="description">A description of the page that the node represents.</param>
        /// <param name="requiredAction">An action that controls the access permission to view the page represented by the <see cref="SiteMapNode"/>.</param>
        /// <param name="attributes">A <see cref="NameValueCollection"/> of additional attributes used to initialize the <see cref="SiteMapNode"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="provider"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static SiteMapNode AddRootNode(this ISiteMapProvider provider, string nodeKey, string title, string url, string description, string requiredAction, NameValueCollection attributes)
        {
            var node = provider.FindNode(nodeKey);
            if (node == null)
            {
                node = CreateNode(nodeKey, title, url, description, requiredAction, attributes);
                provider.RootNodes.Add(node);
            }
            return node;
        }

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the <see cref="ISiteMapProvider.RootNodes"/> using the specified parent node key,
        /// URL, title, description and additional attributes
        /// </summary>
        /// <param name="provider">The <see cref="ISiteMapProvider"/> to add node with.</param>
        /// <param name="nodeKey">A provider-specific lookup key.</param>
        /// <param name="url">The URL of the page that the node represents within the site.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <param name="description">A description of the page that the node represents.</param>
        /// <param name="attributes">A <see cref="NameValueCollection"/> of additional attributes used to initialize the <see cref="SiteMapNode"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="provider"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static SiteMapNode AddRootNode(this ISiteMapProvider provider, string nodeKey, string title, string url, string description, NameValueCollection attributes)
        {
            return provider.AddRootNode(nodeKey, title, url, description, null, attributes);
        }

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the <see cref="ISiteMapProvider.RootNodes"/> using the specified parent node key,
        /// URL, title and additional attributes
        /// </summary>
        /// <param name="provider">The <see cref="ISiteMapProvider"/> to add node with.</param>
        /// <param name="nodeKey">A provider-specific lookup key.</param>
        /// <param name="url">The URL of the page that the node represents within the site.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <param name="attributes">A <see cref="NameValueCollection"/> of additional attributes used to initialize the <see cref="SiteMapNode"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="provider"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static SiteMapNode AddRootNode(this ISiteMapProvider provider, string nodeKey, string title, string url, NameValueCollection attributes)
        {
            return provider.AddRootNode(nodeKey, title, url, null, attributes);
        }

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the <see cref="ISiteMapProvider.RootNodes"/> using the specified parent node key,
        /// title and additional attributes
        /// </summary>
        /// <param name="provider">The <see cref="ISiteMapProvider"/> to add node with.</param>
        /// <param name="nodeKey">A provider-specific lookup key.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <param name="attributes">A <see cref="NameValueCollection"/> of additional attributes used to initialize the <see cref="SiteMapNode"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="provider"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static SiteMapNode AddRootNode(this ISiteMapProvider provider, string nodeKey, string title, NameValueCollection attributes)
        {
            return provider.AddRootNode(nodeKey, title, null, attributes);
        }

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the <see cref="ISiteMapProvider.RootNodes"/> using the specified parent node key,
        /// URL, title, description and required action.
        /// </summary>
        /// <param name="provider">The <see cref="ISiteMapProvider"/> to add node with.</param>
        /// <param name="nodeKey">A provider-specific lookup key.</param>
        /// <param name="url">The URL of the page that the node represents within the site.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <param name="description">A description of the page that the node represents.</param>
        /// <param name="requiredAction">An action that controls the access permission to view the page represented by the <see cref="SiteMapNode"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="provider"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static SiteMapNode AddRootNode(this ISiteMapProvider provider, string nodeKey, string title, string url, string description, string requiredAction)
        {
            return provider.AddRootNode(nodeKey, title, url, description, requiredAction, null);
        }

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the <see cref="ISiteMapProvider.RootNodes"/> using the specified parent node key,
        /// URL, title and description.
        /// </summary>
        /// <param name="provider">The <see cref="ISiteMapProvider"/> to add node with.</param>
        /// <param name="nodeKey">A provider-specific lookup key.</param>
        /// <param name="url">The URL of the page that the node represents within the site.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <param name="description">A description of the page that the node represents.</param>
        /// <exception cref="ArgumentNullException"><paramref name="provider"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static SiteMapNode AddRootNode(this ISiteMapProvider provider, string nodeKey, string title, string url, string description)
        {
            return provider.AddRootNode(nodeKey, title, url, description, (NameValueCollection)null);
        }

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the <see cref="ISiteMapProvider.RootNodes"/> using the specified parent node key,
        /// URL and title.
        /// </summary>
        /// <param name="provider">The <see cref="ISiteMapProvider"/> to add node with.</param>
        /// <param name="nodeKey">A provider-specific lookup key.</param>
        /// <param name="url">The URL of the page that the node represents within the site.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <exception cref="ArgumentNullException"><paramref name="provider"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static SiteMapNode AddRootNode(this ISiteMapProvider provider, string nodeKey, string title, string url)
        {
            return provider.AddRootNode(nodeKey, title, url, (NameValueCollection)null);
        }

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the <see cref="ISiteMapProvider.RootNodes"/> using the specified parent node key,
        /// and title.
        /// </summary>
        /// <param name="provider">The <see cref="ISiteMapProvider"/> to add node with.</param>
        /// <param name="nodeKey">A provider-specific lookup key.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <exception cref="ArgumentNullException"><paramref name="provider"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static SiteMapNode AddRootNode(this ISiteMapProvider provider, string nodeKey, string title)
        {
            return provider.AddRootNode(nodeKey, title, (NameValueCollection)null);
        }

        #endregion

        #region AddNodeBefore

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the hierarchy before the specified sibling node 
        /// using the specified node key, URL, title, description, required action and additional attributes
        /// </summary>
        /// <param name="provider">The <see cref="ISiteMapProvider"/> to add node with.</param>
        /// <param name="siblingNodeKey">The sibling node key</param>
        /// <param name="nodeKey">A provider-specific lookup key.</param>
        /// <param name="url">The URL of the page that the node represents within the site.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <param name="description">A description of the page that the node represents.</param>
        /// <param name="requiredAction">An action that controls the access permission to view the page represented by the <see cref="SiteMapNode"/>.</param>
        /// <param name="attributes">A <see cref="NameValueCollection"/> of additional attributes used to initialize the <see cref="SiteMapNode"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="provider"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static SiteMapNode AddNodeBefore(this ISiteMapProvider provider, string siblingNodeKey, string nodeKey, string title, string url, string description, string requiredAction, NameValueCollection attributes)
        {
            var node = provider.FindNode(nodeKey);
            if (node == null)
            {
                var siblingNode = provider.FindNode(siblingNodeKey);
                if (siblingNode == null)
                {
                    throw new ArgumentException($"The site map node '{siblingNodeKey}' cannot be found.");
                }
                var parentCollection = siblingNode.ParentNode?.ChildNodes ?? provider.RootNodes;
                node = CreateNode(nodeKey, title, url, description, requiredAction, attributes);
                parentCollection.Insert(parentCollection.IndexOf(siblingNode), node);
            }
            return node;
        }

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the hierarchy before the specified sibling node 
        /// using the specified node key, URL, title, description and additional attributes
        /// </summary>
        /// <param name="provider">The <see cref="ISiteMapProvider"/> to add node with.</param>
        /// <param name="siblingNodeKey">The sibling node key</param>
        /// <param name="nodeKey">A provider-specific lookup key.</param>
        /// <param name="url">The URL of the page that the node represents within the site.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <param name="description">A description of the page that the node represents.</param>
        /// <param name="attributes">A <see cref="NameValueCollection"/> of additional attributes used to initialize the <see cref="SiteMapNode"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="provider"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static SiteMapNode AddNodeBefore(this ISiteMapProvider provider, string siblingNodeKey, string nodeKey, string title, string url, string description, NameValueCollection attributes)
        {
            return provider.AddNodeBefore(siblingNodeKey, nodeKey, title, url, description, null, attributes);
        }

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the hierarchy before the specified sibling node 
        /// using the specified node key, URL, title and additional attributes
        /// </summary>
        /// <param name="provider">The <see cref="ISiteMapProvider"/> to add node with.</param>
        /// <param name="siblingNodeKey">The sibling node key</param>
        /// <param name="nodeKey">A provider-specific lookup key.</param>
        /// <param name="url">The URL of the page that the node represents within the site.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <param name="attributes">A <see cref="NameValueCollection"/> of additional attributes used to initialize the <see cref="SiteMapNode"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="provider"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static SiteMapNode AddNodeBefore(this ISiteMapProvider provider, string siblingNodeKey, string nodeKey, string title, string url, NameValueCollection attributes)
        {
            return provider.AddNodeBefore(siblingNodeKey, nodeKey, title, url, null, attributes);
        }

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the hierarchy before the specified sibling node 
        /// using the specified node key, title and additional attributes
        /// </summary>
        /// <param name="provider">The <see cref="ISiteMapProvider"/> to add node with.</param>
        /// <param name="siblingNodeKey">The sibling node key</param>
        /// <param name="nodeKey">A provider-specific lookup key.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <param name="attributes">A <see cref="NameValueCollection"/> of additional attributes used to initialize the <see cref="SiteMapNode"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="provider"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static SiteMapNode AddNodeBefore(this ISiteMapProvider provider, string siblingNodeKey, string nodeKey, string title, NameValueCollection attributes)
        {
            return provider.AddNodeBefore(siblingNodeKey, nodeKey, title, null, attributes);
        }

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the hierarchy before the specified sibling node 
        /// using the specified node key, URL, title, description and required action
        /// </summary>
        /// <param name="provider">The <see cref="ISiteMapProvider"/> to add node with.</param>
        /// <param name="siblingNodeKey">The sibling node key</param>
        /// <param name="nodeKey">A provider-specific lookup key.</param>
        /// <param name="url">The URL of the page that the node represents within the site.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <param name="description">A description of the page that the node represents.</param>
        /// <param name="requiredAction">An action that controls the access permission to view the page represented by the <see cref="SiteMapNode"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="provider"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static SiteMapNode AddNodeBefore(this ISiteMapProvider provider, string siblingNodeKey, string nodeKey, string title, string url, string description, string requiredAction)
        {
            return provider.AddNodeBefore(siblingNodeKey, nodeKey, title, url, description, requiredAction, null);
        }

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the hierarchy before the specified sibling node 
        /// using the specified node key, URL, title and description
        /// </summary>
        /// <param name="provider">The <see cref="ISiteMapProvider"/> to add node with.</param>
        /// <param name="siblingNodeKey">The sibling node key</param>
        /// <param name="nodeKey">A provider-specific lookup key.</param>
        /// <param name="url">The URL of the page that the node represents within the site.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <param name="description">A description of the page that the node represents.</param>
        /// <exception cref="ArgumentNullException"><paramref name="provider"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static SiteMapNode AddNodeBefore(this ISiteMapProvider provider, string siblingNodeKey, string nodeKey, string title, string url, string description)
        {
            return provider.AddNodeBefore(siblingNodeKey, nodeKey, title, url, description, (NameValueCollection)null);
        }

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the hierarchy before the specified sibling node 
        /// using the specified node key, URL and title
        /// </summary>
        /// <param name="provider">The <see cref="ISiteMapProvider"/> to add node with.</param>
        /// <param name="siblingNodeKey">The sibling node key</param>
        /// <param name="nodeKey">A provider-specific lookup key.</param>
        /// <param name="url">The URL of the page that the node represents within the site.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <exception cref="ArgumentNullException"><paramref name="provider"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static SiteMapNode AddNodeBefore(this ISiteMapProvider provider, string siblingNodeKey, string nodeKey, string title, string url)
        {
            return provider.AddNodeBefore(siblingNodeKey, nodeKey, title, url, (NameValueCollection)null);
        }

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the hierarchy before the specified sibling node 
        /// using the specified node key and title.
        /// </summary>
        /// <param name="provider">The <see cref="ISiteMapProvider"/> to add node with.</param>
        /// <param name="siblingNodeKey">The sibling node key</param>
        /// <param name="nodeKey">A provider-specific lookup key.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <exception cref="ArgumentNullException"><paramref name="provider"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static SiteMapNode AddNodeBefore(this ISiteMapProvider provider, string siblingNodeKey, string nodeKey, string title)
        {
            return provider.AddNodeBefore(siblingNodeKey, nodeKey, title, (NameValueCollection)null);
        }

        #endregion

        #region AddNodeAfter

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the hierarchy after the specified sibling node 
        /// using the specified node key, URL, title, description, required action and additional attributes
        /// </summary>
        /// <param name="provider">The <see cref="ISiteMapProvider"/> to add node with.</param>
        /// <param name="siblingNodeKey">The sibling node key</param>
        /// <param name="nodeKey">A provider-specific lookup key.</param>
        /// <param name="url">The URL of the page that the node represents within the site.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <param name="description">A description of the page that the node represents.</param>
        /// <param name="requiredAction">An action that controls the access permission to view the page represented by the <see cref="SiteMapNode"/>.</param>
        /// <param name="attributes">A <see cref="NameValueCollection"/> of additional attributes used to initialize the <see cref="SiteMapNode"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="provider"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static SiteMapNode AddNodeAfter(this ISiteMapProvider provider, string siblingNodeKey, string nodeKey, string title, string url, string description, string requiredAction, NameValueCollection attributes)
        {
            var node = provider.FindNode(nodeKey);
            if (node == null)
            {
                var siblingNode = provider.FindNode(siblingNodeKey);
                if (siblingNode == null)
                {
                    throw new ArgumentException($"The site map node '{siblingNodeKey}' cannot be found.");
                }
                var parentCollection = siblingNode.ParentNode?.ChildNodes ?? provider.RootNodes;
                node = CreateNode(nodeKey, title, url, description, requiredAction, attributes);
                parentCollection.Insert(parentCollection.IndexOf(siblingNode) + 1, node);
            }
            return node;
        }

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the hierarchy after the specified sibling node 
        /// using the specified node key, URL, title, description and additional attributes
        /// </summary>
        /// <param name="provider">The <see cref="ISiteMapProvider"/> to add node with.</param>
        /// <param name="siblingNodeKey">The sibling node key</param>
        /// <param name="nodeKey">A provider-specific lookup key.</param>
        /// <param name="url">The URL of the page that the node represents within the site.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <param name="description">A description of the page that the node represents.</param>
        /// <param name="attributes">A <see cref="NameValueCollection"/> of additional attributes used to initialize the <see cref="SiteMapNode"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="provider"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static SiteMapNode AddNodeAfter(this ISiteMapProvider provider, string siblingNodeKey, string nodeKey, string title, string url, string description, NameValueCollection attributes)
        {
            return provider.AddNodeAfter(siblingNodeKey, nodeKey, title, url, description, null, attributes);
        }

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the hierarchy after the specified sibling node 
        /// using the specified node key, URL, title and additional attributes
        /// </summary>
        /// <param name="provider">The <see cref="ISiteMapProvider"/> to add node with.</param>
        /// <param name="siblingNodeKey">The sibling node key</param>
        /// <param name="nodeKey">A provider-specific lookup key.</param>
        /// <param name="url">The URL of the page that the node represents within the site.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <param name="attributes">A <see cref="NameValueCollection"/> of additional attributes used to initialize the <see cref="SiteMapNode"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="provider"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static SiteMapNode AddNodeAfter(this ISiteMapProvider provider, string siblingNodeKey, string nodeKey, string title, string url, NameValueCollection attributes)
        {
            return provider.AddNodeAfter(siblingNodeKey, nodeKey, title, url, null, attributes);
        }

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the hierarchy after the specified sibling node 
        /// using the specified node key, title and additional attributes
        /// </summary>
        /// <param name="provider">The <see cref="ISiteMapProvider"/> to add node with.</param>
        /// <param name="siblingNodeKey">The sibling node key</param>
        /// <param name="nodeKey">A provider-specific lookup key.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <param name="attributes">A <see cref="NameValueCollection"/> of additional attributes used to initialize the <see cref="SiteMapNode"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="provider"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static SiteMapNode AddNodeAfter(this ISiteMapProvider provider, string siblingNodeKey, string nodeKey, string title, NameValueCollection attributes)
        {
            return provider.AddNodeAfter(siblingNodeKey, nodeKey, title, null, attributes);
        }

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the hierarchy after the specified sibling node 
        /// using the specified node key, URL, title, description and required action
        /// </summary>
        /// <param name="provider">The <see cref="ISiteMapProvider"/> to add node with.</param>
        /// <param name="siblingNodeKey">The sibling node key</param>
        /// <param name="nodeKey">A provider-specific lookup key.</param>
        /// <param name="url">The URL of the page that the node represents within the site.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <param name="description">A description of the page that the node represents.</param>
        /// <param name="requiredAction">An action that controls the access permission to view the page represented by the <see cref="SiteMapNode"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="provider"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static SiteMapNode AddNodeAfter(this ISiteMapProvider provider, string siblingNodeKey, string nodeKey, string title, string url, string description, string requiredAction)
        {
            return provider.AddNodeAfter(siblingNodeKey, nodeKey, title, url, description, requiredAction, null);
        }

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the hierarchy after the specified sibling node 
        /// using the specified node key, URL, title and description
        /// </summary>
        /// <param name="provider">The <see cref="ISiteMapProvider"/> to add node with.</param>
        /// <param name="siblingNodeKey">The sibling node key</param>
        /// <param name="nodeKey">A provider-specific lookup key.</param>
        /// <param name="url">The URL of the page that the node represents within the site.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <param name="description">A description of the page that the node represents.</param>
        /// <exception cref="ArgumentNullException"><paramref name="provider"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static SiteMapNode AddNodeAfter(this ISiteMapProvider provider, string siblingNodeKey, string nodeKey, string title, string url, string description)
        {
            return provider.AddNodeAfter(siblingNodeKey, nodeKey, title, url, description, (NameValueCollection)null);
        }

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the hierarchy after the specified sibling node 
        /// using the specified node key, URL and title
        /// </summary>
        /// <param name="provider">The <see cref="ISiteMapProvider"/> to add node with.</param>
        /// <param name="siblingNodeKey">The sibling node key</param>
        /// <param name="nodeKey">A provider-specific lookup key.</param>
        /// <param name="url">The URL of the page that the node represents within the site.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <exception cref="ArgumentNullException"><paramref name="provider"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static SiteMapNode AddNodeAfter(this ISiteMapProvider provider, string siblingNodeKey, string nodeKey, string title, string url)
        {
            return provider.AddNodeAfter(siblingNodeKey, nodeKey, title, url, (NameValueCollection)null);
        }

        /// <summary>
        /// Adds a <see cref="SiteMapNode"/> object to the hierarchy after the specified sibling node 
        /// using the specified node key and title.
        /// </summary>
        /// <param name="provider">The <see cref="ISiteMapProvider"/> to add node with.</param>
        /// <param name="siblingNodeKey">The sibling node key</param>
        /// <param name="nodeKey">A provider-specific lookup key.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <exception cref="ArgumentNullException"><paramref name="provider"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="SiteMapNode"/> or the found <see cref="SiteMapNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static SiteMapNode AddNodeAfter(this ISiteMapProvider provider, string siblingNodeKey, string nodeKey, string title)
        {
            return provider.AddNodeAfter(siblingNodeKey, nodeKey, title, (NameValueCollection)null);
        }

        #endregion

        #region RemoveNode

        /// <summary>
        /// Removes the specified <see cref="SiteMapNode"/> from the hierarchy by using the specified node key.
        /// </summary>
        /// <param name="provider">The <see cref="ISiteMapProvider"/> to remove node with.</param>
        /// <param name="nodeKey">A provider-specific lookup key.</param>
        /// <returns><c>true</c> if the <see cref="SiteMapNode"/> removed from the hierarchy; otherwise <c>false</c>.</returns>
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
