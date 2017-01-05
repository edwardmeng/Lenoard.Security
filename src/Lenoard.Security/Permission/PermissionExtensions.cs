﻿using System;
using System.Collections.Specialized;

namespace Lenoard.Security
{
    /// <summary>
    /// Provides a set of <see langword="static"/> extension methods for the <see cref="IPermissionProvider"/>
    /// </summary>
    public static class PermissionExtensions
    {
        private static PermissionNode CreateNode(string nodeKey, string title, string description, NameValueCollection attributes)
        {
            var node = new PermissionNode(nodeKey)
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

        /// <summary>
        /// Retrieves a <see cref="PermissionNode"/> object based on a specified key.
        /// </summary>
        /// <param name="provider">The <see cref="IPermissionProvider"/> to lookup with.</param>
        /// <param name="key">A lookup key with which a <see cref="PermissionNode"/> is created.</param>
        /// <returns>
        /// A <see cref="PermissionNode"/> that represents the page identified by key; 
        /// otherwise, null, if no corresponding <see cref="PermissionNode"/> is found.
        /// The default is null.
        /// </returns>
        /// <exception cref="ArgumentNullException">The <paramref name="provider"/> or <paramref name="key"/> is null.</exception>
        public static PermissionNode FindNode(this IPermissionProvider provider, string key)
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
            PermissionNode node = null;
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

        /// <summary>
        /// Adds an <see cref="PermissionNode"/> object to the hierarchy using the specified parent node key,
        /// title, description and additional attributes
        /// </summary>
        /// <param name="provider">The <see cref="ISiteMapProvider"/> to add node with.</param>
        /// <param name="parentNode">The parent node key</param>
        /// <param name="nodeKey">A provider-specific lookup key.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <param name="description">A description of the page that the node represents.</param>
        /// <param name="attributes">A <see cref="NameValueCollection"/> of additional attributes used to initialize the <see cref="SiteMapNode"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="provider"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="PermissionNode"/> or the found <see cref="PermissionNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static PermissionNode AddNode(this IPermissionProvider provider, string parentNode, string nodeKey, string title, string description, NameValueCollection attributes)
        {
            var node = provider.FindNode(nodeKey);
            if (node == null)
            {
                var parentSiteMapNode = provider.FindNode(parentNode);
                if (parentSiteMapNode == null)
                {
                    throw new ArgumentException($"The site map node '{parentNode}' cannot be found.");
                }
                node = CreateNode(nodeKey, title, description, attributes);
                parentSiteMapNode.ChildNodes.Add(node);
            }
            return node;
        }

        /// <summary>
        /// Adds an <see cref="PermissionNode"/> object to the hierarchy using the specified parent node key,
        /// title and additional attributes
        /// </summary>
        /// <param name="provider">The <see cref="ISiteMapProvider"/> to add node with.</param>
        /// <param name="parentNode">The parent node key</param>
        /// <param name="nodeKey">A provider-specific lookup key.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <param name="attributes">A <see cref="NameValueCollection"/> of additional attributes used to initialize the <see cref="SiteMapNode"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="provider"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="PermissionNode"/> or the found <see cref="PermissionNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static PermissionNode AddNode(this IPermissionProvider provider, string parentNode, string nodeKey, string title, NameValueCollection attributes)
        {
            return provider.AddNode(parentNode, nodeKey, title, null, attributes);
        }

        /// <summary>
        /// Adds an <see cref="PermissionNode"/> object to the hierarchy using the specified parent node key,
        /// title and description
        /// </summary>
        /// <param name="provider">The <see cref="ISiteMapProvider"/> to add node with.</param>
        /// <param name="parentNode">The parent node key</param>
        /// <param name="nodeKey">A provider-specific lookup key.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <param name="description">A description of the page that the node represents.</param>
        /// <exception cref="ArgumentNullException"><paramref name="provider"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="PermissionNode"/> or the found <see cref="PermissionNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static PermissionNode AddNode(this IPermissionProvider provider, string parentNode, string nodeKey, string title, string description)
        {
            return provider.AddNode(parentNode, nodeKey, title, description, null);
        }

        /// <summary>
        /// Adds an <see cref="PermissionNode"/> object to the hierarchy using the specified parent node key and title.
        /// </summary>
        /// <param name="provider">The <see cref="ISiteMapProvider"/> to add node with.</param>
        /// <param name="parentNode">The parent node key</param>
        /// <param name="nodeKey">A provider-specific lookup key.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <exception cref="ArgumentNullException"><paramref name="provider"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="PermissionNode"/> or the found <see cref="PermissionNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static PermissionNode AddNode(this IPermissionProvider provider, string parentNode, string nodeKey, string title)
        {
            return provider.AddNode(parentNode, nodeKey, title, (NameValueCollection)null);
        }

        #endregion

        #region AddRootNode

        /// <summary>
        /// Adds an <see cref="PermissionNode"/> object to the <see cref="IPermissionProvider.RootNodes"/> using the specified parent node key,
        /// title, description and additional attributes
        /// </summary>
        /// <param name="provider">The <see cref="IPermissionProvider"/> to add node with.</param>
        /// <param name="nodeKey">A provider-specific lookup key.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <param name="description">A description of the page that the node represents.</param>
        /// <param name="attributes">A <see cref="NameValueCollection"/> of additional attributes used to initialize the <see cref="SiteMapNode"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="provider"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="PermissionNode"/> or the found <see cref="PermissionNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static PermissionNode AddRootNode(this IPermissionProvider provider, string nodeKey, string title, string description, NameValueCollection attributes)
        {
            var node = provider.FindNode(nodeKey);
            if (node == null)
            {
                node = CreateNode(nodeKey, title, description, attributes);
                provider.RootNodes.Add(node);
            }
            return node;
        }

        /// <summary>
        /// Adds an <see cref="PermissionNode"/> object to the <see cref="IPermissionProvider.RootNodes"/> using the specified parent node key,
        /// title and additional attributes
        /// </summary>
        /// <param name="provider">The <see cref="IPermissionProvider"/> to add node with.</param>
        /// <param name="nodeKey">A provider-specific lookup key.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <param name="attributes">A <see cref="NameValueCollection"/> of additional attributes used to initialize the <see cref="SiteMapNode"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="provider"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="PermissionNode"/> or the found <see cref="PermissionNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static PermissionNode AddRootNode(this IPermissionProvider provider, string nodeKey, string title, NameValueCollection attributes)
        {
            return provider.AddRootNode(nodeKey, title, null, attributes);
        }

        /// <summary>
        /// Adds an <see cref="PermissionNode"/> object to the <see cref="IPermissionProvider.RootNodes"/> using the specified parent node key,
        /// title and description.
        /// </summary>
        /// <param name="provider">The <see cref="IPermissionProvider"/> to add node with.</param>
        /// <param name="nodeKey">A provider-specific lookup key.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <param name="description">A description of the page that the node represents.</param>
        /// <exception cref="ArgumentNullException"><paramref name="provider"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="PermissionNode"/> or the found <see cref="PermissionNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static PermissionNode AddRootNode(this IPermissionProvider provider, string nodeKey, string title, string description)
        {
            return provider.AddRootNode(nodeKey, title, description, null);
        }

        /// <summary>
        /// Adds an <see cref="PermissionNode"/> object to the <see cref="IPermissionProvider.RootNodes"/> using the specified parent node key and title.
        /// </summary>
        /// <param name="provider">The <see cref="IPermissionProvider"/> to add node with.</param>
        /// <param name="nodeKey">A provider-specific lookup key.</param>
        /// <param name="title">A label for the node, often displayed by navigation controls.</param>
        /// <exception cref="ArgumentNullException"><paramref name="provider"/> or <paramref name="nodeKey"/> is null.</exception>
        /// <returns>The created <see cref="PermissionNode"/> or the found <see cref="PermissionNode"/> if the <paramref name="nodeKey"/> has been exist.</returns>
        public static PermissionNode AddRootNode(this IPermissionProvider provider, string nodeKey, string title)
        {
            return provider.AddRootNode(nodeKey, title, (NameValueCollection)null);
        }

        #endregion

        #region RemoveNode

        /// <summary>
        /// Removes the specified <see cref="PermissionNode"/> from the hierarchy by using the specified node key.
        /// </summary>
        /// <param name="provider">The <see cref="IPermissionProvider"/> to remove node with.</param>
        /// <param name="nodeKey">A provider-specific lookup key.</param>
        /// <returns><c>true</c> if the <see cref="PermissionNode"/> removed from the hierarchy; otherwise <c>false</c>.</returns>
        public static bool RemoveNode(this IPermissionProvider provider, string nodeKey)
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