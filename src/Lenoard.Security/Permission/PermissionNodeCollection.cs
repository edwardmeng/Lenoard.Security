using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Lenoard.Security
{
    /// <summary>
    /// Provides a strongly typed collection for <see cref="PermissionNode"/> objects.
    /// </summary>
    public class PermissionNodeCollection:IList<PermissionNode>
    {
        private List<PermissionNode> _innerList;
        private readonly PermissionNode _parentNode;

        private List<PermissionNode> List => _innerList ?? (_innerList = new List<PermissionNode>());

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SiteMapNodeCollection"/> class, which is the default instance.
        /// </summary>
        internal PermissionNodeCollection()
        {
        }

        internal PermissionNodeCollection(PermissionNode parentNode)
        {
            _parentNode = parentNode;
        }

        #endregion

        #region IList<PermissionNode> Members

        /// <summary>
        /// Adds a single <see cref="PermissionNode"/> object to the collection.
        /// </summary>
        /// <param name="value">The <see cref="PermissionNode"/> to add to the <see cref="SiteMapNodeCollection"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        /// <exception cref="NotSupportedException">The <see cref="SiteMapNodeCollection"/> is read-only.</exception>
        public virtual void Add(PermissionNode value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            value.ParentNode = _parentNode;
            List.Add(value);
        }

        /// <summary>
        /// Removes all items from the collection.
        /// </summary>
        /// <exception cref="NotSupportedException">The <see cref="SiteMapNodeCollection"/> is read-only.</exception>
        public virtual void Clear()
        {
            if (_innerList != null)
            {
                foreach (var node in _innerList)
                {
                    node.ParentNode = null;
                }
                _innerList.Clear();
            }
        }

        /// <summary>
        /// Determines whether the collection contains a specific <see cref="PermissionNode"/> object.
        /// </summary>
        /// <param name="value">The <see cref="PermissionNode"/> to locate in the <see cref="SiteMapNodeCollection"/>.</param>
        /// <returns>
        /// <c>true</c> if the <see cref="SiteMapNodeCollection"/> contains the specified <see cref="PermissionNode"/>; 
        /// otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// The Contains method determines equality by calling the <see cref="Object.Equals(object,object)"/> method.
        /// </remarks>
        public virtual bool Contains(PermissionNode value)
        {
            return _innerList != null && _innerList.Contains(value);
        }

        /// <summary>
        /// Copies the entire collection to a compatible one-dimensional array, starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">The one-dimensional array that must have zero-based indexing and is the destination of the elements copied from the <see cref="SiteMapNodeCollection" />.</param>
        /// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is less than zero.</exception>
        /// <exception cref="ArgumentException">
        /// array is multidimensional.
        /// -or- 
        /// The number of <see cref="PermissionNode"/> objects in the source <see cref="SiteMapNodeCollection"/> is greater than the available space from index to the end of array.
        /// </exception>
        public virtual void CopyTo(PermissionNode[] array, int index)
        {
            _innerList?.CopyTo(array, index);
        }

        /// <summary>
        /// Retrieves a reference to an enumerator object, which is used to iterate over the collection.
        /// </summary>
        /// <returns>An object that implements the <see cref="IEnumerator{PermissionNode}"/>.</returns>
        public virtual IEnumerator<PermissionNode> GetEnumerator()
        {
            if (_innerList == null)
            {
                return Enumerable.Empty<PermissionNode>().GetEnumerator();
            }
            return _innerList.GetEnumerator();
        }

        /// <summary>
        /// Searches for the specified <see cref="PermissionNode"/> object, and then returns the zero-based index 
        /// of the first occurrence within the entire collection.
        /// </summary>
        /// <param name="value">The <see cref="PermissionNode"/> to locate in the <see cref="SiteMapNodeCollection"/>.</param>
        /// <returns>
        /// The zero-based index of the first occurrence of value within the entire <see cref="SiteMapNodeCollection"/>, if found; 
        /// otherwise, -1.
        /// </returns>
        public virtual int IndexOf(PermissionNode value)
        {
            if (_innerList == null)
            {
                return -1;
            }
            return _innerList.IndexOf(value);
        }

        /// <summary>
        /// Inserts the specified <see cref="PermissionNode"/> object into the collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the <see cref="PermissionNode"/> is inserted.</param>
        /// <param name="value">The <see cref="PermissionNode"/> to insert.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> is less than zero.
        /// -or- 
        /// <paramref name="index"/> is greater than the <see cref="Count"/>.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// The <see cref="SiteMapNodeCollection"/> is read-only.
        /// -or-
        /// The <see cref="SiteMapNodeCollection"/> has a fixed size.
        /// </exception>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        public virtual void Insert(int index, PermissionNode value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            value.ParentNode = _parentNode;
            List.Insert(index, value);
        }

        /// <summary>
        /// Inserts an array of type <see cref="PermissionNode"/> to the collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the <see cref="PermissionNode"/> is inserted.</param>
        /// <param name="value">An array of type <see cref="PermissionNode"/> to insert to the current <see cref="SiteMapNodeCollection"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        /// <exception cref="NotSupportedException">The <see cref="SiteMapNodeCollection"/> is read-only.</exception>
        public virtual void InsertRange(int index, IEnumerable<PermissionNode> value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            var array = value.ToArray();
            array.ForEach(x => x.ParentNode = _parentNode);
            List.InsertRange(index, array);
        }

        /// <summary>
        /// Removes the specified <see cref="PermissionNode"/> object from the collection.
        /// </summary>
        /// <param name="value">The <see cref="PermissionNode"/> to remove from the <see cref="SiteMapNodeCollection"/>.</param>
        /// <returns>
        /// <c>true</c> if the <see cref="PermissionNode"/> is removed from the <see cref="SiteMapNodeCollection"/>; otherwise <c>false</c>.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// The <see cref="SiteMapNodeCollection"/> is read-only or has a fixed size.
        /// </exception>
        public virtual bool Remove(PermissionNode value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (Contains(value))
            {
                value.ParentNode = null;
                return List.Remove(value);
            }
            return false;
        }

        /// <summary>
        /// Removes the <see cref="PermissionNode"/> object at the specified index of the collection.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> is less than zero.
        /// - or -
        /// <paramref name="index"/> is greater than the <see cref="Count"/>.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// The <see cref="SiteMapNodeCollection"/> is read-only or has a fixed size.
        /// </exception>
        public virtual void RemoveAt(int index)
        {
            var node = List[index];
            node.ParentNode = null;
            List.RemoveAt(index);
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Gets the number of elements contained in the collection.
        /// </summary>
        /// <value>
        /// The number of elements in the <see cref="SiteMapNodeCollection"/>.
        /// </value>
        public virtual int Count => List.Count;

        /// <summary>
        /// Gets a <see cref="Boolean"/> value indicating whether the collection is read-only.
        /// </summary>
        /// <value><c>true</c> if you can modify the <see cref="SiteMapNodeCollection"/>; otherwise, <c>false</c>.</value>
        public virtual bool IsReadOnly => false;

        /// <summary>
        /// Gets or sets the <see cref="PermissionNode"/> object at the specified index in the collection.
        /// </summary>
        /// <param name="index">The index of the <see cref="PermissionNode"/> to find.</param>
        /// <returns>A <see cref="PermissionNode"/> that represents an element in the <see cref="SiteMapNodeCollection"/>.</returns>
        /// <exception cref="ArgumentNullException">The value supplied to the setter is null.</exception>
        /// <exception cref="NotSupportedException">The <see cref="SiteMapNodeCollection"/> is read-only.</exception>
        public virtual PermissionNode this[int index]
        {
            get { return List[index]; }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                value.ParentNode = _parentNode;
                List[index] = value;
            }
        }

        #endregion

        /// <summary>
        /// Adds an array of type <see cref="PermissionNode"/> to the collection.
        /// </summary>
        /// <param name="value">An array of type <see cref="PermissionNode"/> to add to the current <see cref="SiteMapNodeCollection"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        /// <exception cref="NotSupportedException">The <see cref="SiteMapNodeCollection"/> is read-only.</exception>
        public virtual void AddRange(IEnumerable<PermissionNode> value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            foreach (var node in value)
            {
                node.ParentNode = _parentNode;
                List.Add(node);
            }
        }
    }
}
