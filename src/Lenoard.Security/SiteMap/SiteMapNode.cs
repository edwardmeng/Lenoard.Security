﻿using System;
using System.Collections.Specialized;

namespace Lenoard.Security
{
    /// <summary>
    /// Represents a node in the hierarchical site map structure.
    /// </summary>
    public class SiteMapNode
    {
        #region Fields

        private NameValueCollection _attributes;
        private SiteMapNodeCollection _childNodes;
        private SiteMapNode _rootNode;
        private SiteMapNode _parentNode;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SiteMapNode"/> class.
        /// </summary>
        /// <param name="key">A provider-specific lookup key.</param>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is null.</exception>
        public SiteMapNode(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            Key = key;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a collection of additional attributes beyond the strongly typed properties 
        /// that are defined for the <see cref="SiteMapNode"/> class.
        /// </summary>
        /// <value>
        /// A <see cref="NameValueCollection"/> of additional attributes for the <see cref="SiteMapNode"/> 
        /// beyond Title, Description, Url, and RequiredPermission.</value>
        protected NameValueCollection Attributes => _attributes ?? (_attributes = new NameValueCollection());

        /// <summary>
        /// Gets or sets all the child nodes of the current <see cref="SiteMapNode"/> object.
        /// </summary>
        /// <value>
        /// A <see cref="SiteMapNodeCollection"/> of child nodes.
        /// </value>
        public virtual SiteMapNodeCollection ChildNodes => _childNodes ?? (_childNodes = new SiteMapNodeCollection(this));

        /// <summary>
        /// Gets a value indicating whether the current <see cref="SiteMapNode"/> has any child nodes.
        /// </summary>
        /// <value><c>true</c> if the node has children; otherwise, <c>false</c>.</value>
        public virtual bool HasChildren => _childNodes != null && _childNodes.Count > 0;

        /// <summary>
        /// Gets a string representing a lookup key for a site map node.
        /// </summary>
        /// <value>A string representing a lookup key.</value>
        /// <remarks>
        /// <para>
        /// Internally, site navigation classes can use this lookup key when searching for site map nodes, 
        /// as well as for establishing relationships between nodes. 
        /// </para>
        /// <para>The specific value that is used for the key is provider specific; however, it is guaranteed to not be null.</para>
        /// </remarks>
        public string Key { get; }

        /// <summary>
        /// Gets or sets the title of the <see cref="SiteMapNode"/> object.
        /// </summary>
        /// <value>A string that represents the title of the node.</value>
        public virtual string Title { get; set; }

        /// <summary>
        /// Gets or sets the URL of the page that the <see cref="SiteMapNode"/> object represents.
        /// </summary>
        /// <value>The URL of the page that the node represents. The default is <see cref="String.Empty"/>.</value>
        /// <exception cref="InvalidOperationException">The node is read-only.</exception>
        public virtual string Url { get; set; }

        /// <summary>
        /// Gets or sets a description for the <see cref="SiteMapNode"/>.
        /// </summary>
        /// <value>A string that represents a description of the node.</value>
        public virtual string Description { get; set; }

        /// <summary>
        /// Gets or sets the required permission for the <see cref="SiteMapNode"/>.
        /// </summary>
        /// <value>A string that represents the required permission of the node; otherwise, <see cref="String.Empty"/>.</value>
        public virtual string RequiredPermission { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="SiteMapNode"/> object that is the parent of the current node.
        /// </summary>
        /// <value>The parent <see cref="SiteMapNode"/>.</value>
        public SiteMapNode ParentNode
        {
            get
            {
                return _parentNode;
            }
            internal set
            {
                _parentNode = value;
                _rootNode = null;
            }
        }

        /// <summary>
        /// Gets or sets a custom attribute from the <see cref="Attributes"/> collection or a resource string based on the specified key.
        /// </summary>
        /// <param name="key">A string that identifies the attribute or resource string to retrieve.</param>
        /// <returns>A custom attribute or resource string identified by key; otherwise, <see langword="null"/>.</returns>
        public virtual string this[string key]
        {
            get
            {
                return _attributes?[key];
            }
            set
            {
                (_attributes ?? (_attributes = new NameValueCollection()))[key] = value;
            }
        }

        /// <summary>
        /// Gets the root node of the root provider in a site map provider hierarchy. 
        /// If no provider hierarchy exists, the RootNode property gets the root node of the current provider.
        /// </summary>
        /// <value>A <see cref="SiteMapNode"/> that represents the root node of the site navigation structure.</value>
        /// <exception cref="InvalidOperationException">The root node cannot be retrieved from the root provider.</exception>
        public virtual SiteMapNode RootNode => _rootNode ?? (_rootNode = _parentNode?.RootNode ?? this);

        #endregion

        #region Methods

        /// <summary>
        ///   Determines whether the specified <see cref = "System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name = "obj">The <see cref = "System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref = "System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            var other = obj as SiteMapNode;
            if (other == null) return false;
            return Equals(other.Key, Key);
        }

        /// <summary>
        ///   Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        ///   A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return Key?.GetHashCode() ?? 0;
        }

        #endregion
    }
}
