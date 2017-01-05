using System;
using System.Collections.Specialized;

namespace Lenoard.Security
{
    /// <summary>
    /// Represents a node in the hierarchical permission structure.
    /// </summary>
    public class PermissionNode
    {
        #region Fields

        private NameValueCollection _attributes;
        private PermissionNodeCollection _childNodes;
        private PermissionNode _rootNode;
        private PermissionNode _parentNode;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionNode"/> class.
        /// </summary>
        /// <param name="key">A provider-specific lookup key.</param>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is null.</exception>
        public PermissionNode(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            Key = key;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a string representing a lookup key for a permission node.
        /// </summary>
        /// <value>A string representing a lookup key.</value>
        public string Key { get; }

        /// <summary>
        /// Gets or sets the title of the <see cref="PermissionNode"/> object.
        /// </summary>
        /// <value>A string that represents the title of the node.</value>
        public virtual string Title { get; set; }

        /// <summary>
        /// Gets or sets a description for the <see cref="PermissionNode"/>.
        /// </summary>
        /// <value>A string that represents a description of the node.</value>
        public virtual string Description { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="PermissionNode"/> object that is the parent of the current node.
        /// </summary>
        /// <value>The parent <see cref="PermissionNode"/></value>
        public PermissionNode ParentNode
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
        /// Gets the root node in a permission hierarchy. 
        /// </summary>
        /// <value>A <see cref="PermissionNode"/> that represents the root node of the permission structure.</value>
        /// <exception cref="InvalidOperationException">The root node cannot be retrieved.</exception>
        public virtual PermissionNode RootNode => _rootNode ?? (_rootNode = _parentNode?.RootNode ?? this);

        /// <summary>
        /// Gets or sets all the child nodes of the current <see cref="PermissionNode"/> object .
        /// </summary>
        /// <value>
        /// An <see cref="PermissionNodeCollection"/> of child nodes.
        /// </value>
        public virtual PermissionNodeCollection ChildNodes => _childNodes ?? (_childNodes = new PermissionNodeCollection(this));

        /// <summary>
        /// Gets a value indicating whether the current <see cref="PermissionNode"/> has any child nodes.
        /// </summary>
        /// <value><c>true</c> if the node has children; otherwise, <c>false</c>.</value>
        public virtual bool HasChildren => _childNodes != null && _childNodes.Count > 0;

        /// <summary>
        /// Gets or sets a collection of additional attributes beyond the strongly typed properties 
        /// that are defined for the <see cref="PermissionNode"/> class.
        /// </summary>
        /// <value>
        /// A <see cref="NameValueCollection"/> of additional attributes for the <see cref="PermissionNode"/> 
        /// beyond Title, Description.</value>
        protected NameValueCollection Attributes => _attributes ?? (_attributes = new NameValueCollection());

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
            var other = obj as PermissionNode;
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
