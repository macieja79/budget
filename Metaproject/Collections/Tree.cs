using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Metaproject.Collections
{
	public class Tree<T>
	{

		#region <props>
		public List<Tree<T>> Children { get; private set; }
		public Tree<T> Parent { get; set; }
		public T Value { get; private set; }
		#endregion

		#region <ctor>


		public Tree()
		{
			Children = new List<Tree<T>>();
		}

		public Tree(T value) : this()
		{
			Value = value;
		}

		#endregion

		#region <pub>
		public int Level
		{
			get
			{
				if (null == Parent) return 0;
				return Parent.Level + 1;

			}

		}


		public Tree<T> AddChild(T item)
		{
			Tree<T> treeItem = new Tree<T>(item);
			treeItem.Parent = this;
			Children.Add(treeItem);
            return treeItem;
		}

		public void AddChildren(params T[] children)
		{
			foreach (T t in children)
				AddChild(t);
		}


		public bool HasChildren
		{
			get
			{
				return Children.Count == 0;
			}
		}


		public bool IsRoot
		{
			get
			{
				return (null == Parent);
			}
		}

		public List<T> GetAsList()
		{
			List<T> list = new List<T>();
			addToList(list, this);
			return list;
		}

		public int Number
		{
			get
			{
				if (null == Parent) return 1;
				return (Parent.Children.IndexOf(this) + 1);
			}
		}

		public string FullNumber
		{
			get
			{

				if (null != Parent)
					return Parent.FullNumber + "." + Number.ToString();

				return Number.ToString();
			}
		}
		#endregion

		#region <prv>
		void addToList(List<T> list,  Tree<T> item)
		{
			if (!item.IsRoot)
				list.Add(item.Value);
			
			foreach(Tree<T> child in item.Children)
			{
				addToList(list, child); 
			}
		}
		#endregion

		#region <overriden>

		public override string ToString()
		{
			return FullNumber + Value.ToString();
		}


		#endregion
	}
}
