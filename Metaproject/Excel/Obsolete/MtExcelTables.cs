using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Drawing;
using System.IO;
using System.Globalization;

using Common.Office.Excel;

namespace Common.Office.Excel
{
    public class HeaderItem<T>
    {

        #region <members>
        int _columnSpan = 1;
        #endregion

        #region <constructor>
        public HeaderItem() { }
        public HeaderItem(string tag, T value)
        {
            Tag = tag;
            Value = value;

        }

        #endregion

        #region <iterators>
        
        protected virtual T this[string tag]
        {
            get
            {
                if (tag == Tag)
                    return Value;

                if (HasChildren)
                {
                    foreach (HeaderItem<T> h in Children)
                    {
                        if (null != h[tag])
                            return h.Value;
                    }

                }

                return default(T);
            }
        }

        protected virtual int GetPositionNumber(string tag)
        {

            if (tag == Tag)
                return Position;

            if (HasChildren)
            {
                foreach (HeaderItem<T> h in Children)
                {
                    int pos = h.GetPositionNumber(tag);
                    if (pos > 0) return pos;
                }
            }

            return -1;
        }

        #endregion

        #region <public methods>

        public void AddChild(string tag, T value)
        {
            HeaderItem<T> item = new HeaderItem<T>(tag, value);
            AddChild(item);
        }

        public void AddChild(HeaderItem<T> header)
        {
            if (null == Children)
                Children = new HeaderItems<T>();

            header.Parent = this;
            Children.Add(header);
        }

        public void AddChildren(params HeaderItem<T>[] headers)
        {
            foreach (HeaderItem<T> h in headers)
            {
                AddChild(h);
            }
        }
                
        #endregion

        #region <properties>
        public string Tag { get; private set; }

        public HeaderItems<T> Children { get; private set; }
        
        public HeaderItem<T> Parent { get; private set; }

        public Header<T> Root
        {
            get
            {
                if (HasParent)
                    return Parent.Root;
                else
                {

                    if (this is Header<T>)
                    {
                        return (Header<T>)this;
                    }

                }

                return null;
            }
        }
             
        public T Value { get; private set; }
                
        public bool HasChildren
        {
            get
            {
                return (null != Children && Children.Count > 0);
            }
        }

        public bool HasParent
        {
            get
            {
                return (null != Parent);

            }
        }

        public int Level
        {
            get
            {
                if (!HasParent)
                    return 0;
                else
                    return Parent.Level + 1;
            }
        }

        public int LevelLast
        {
            get
            {
                if (!HasChildren)
                    return Level;
                else
                {
                    int max = Level;
                    foreach (HeaderItem<T> h in Children)
                    {
                        if (h.LevelLast > max)
                            max = h.LevelLast;
                    }
                    return max;

                }



            }
        }

        public int LevelSpan
        {
            get
            {
                return Root.LevelLast - LevelLast + 1;
            }
        }

        public int Position
        {
            get
            {
                if (!HasParent)
                {
                   

                    if (this.Equals(Root))
                        return Root.PositionStart;

                    int FIRST = 1;

                    if (Root.PositionStart > 0)
                        return FIRST + Root.PositionStart;
                    else
                        return FIRST;
                }
                else
                {
                    int col = Parent.Position;
                    foreach (HeaderItem<T> h in Parent.Children)
                    {
                        if (h.Equals(this)) break;
                        col += h.PositionSpan;
                    }

                    return col;

                }

            }


        }

        public int PositionLast
        {
            get
            {
                return Position + PositionSpan - 1;
            }
        }

        public int PositionSpan
        {
            get
            {
                if (!HasChildren)
                {
                    return _columnSpan;

                }
                else
                {
                    int span = 0;
                    foreach (HeaderItem<T> h in Children)
                    {
                        span += h.PositionSpan;
                    }
                    return span;
                }

            }

            set
            {
                if (!HasChildren)
                    _columnSpan = value;
                else
                    throw new ApplicationException("Can't assing RowSpan, when Header has Children.");
            }
        }

        public HeaderItems<T> GetChildlessChildren()
        {
                HeaderItems<T> childless = new HeaderItems<T>();
                AddChildlessChildren(childless);
                return childless;
        }
                
        #endregion

        #region <private methods>

        void AddChildlessChildren(HeaderItems<T> childless)
        {

            if (!HasChildren)
                childless.Add(this);
            else
            {
                foreach (HeaderItem<T> item in Children)
                    item.AddChildlessChildren(childless);
            }

        }
        
        #endregion

    }
    
    public class Header<T> : HeaderItem<T>
    {

        int _start = -1;

        public new T this[string tag]
        {
            get
            {
                return base[tag];
            }
        }

        public int PositionStart
        {
            get { return _start; }
            set { _start = value; }
        }

        public new int GetPositionNumber(string tag)
        {
 	        return base.GetPositionNumber(tag);
        }
                
    }

    public class HeaderItems<T> : List<HeaderItem<T>>
    {
        public HeaderItem<T> this[string tag]
        {

            get
            {
                foreach (HeaderItem<T> item in this)
                {
                    if (item.Tag == tag)
                        return item;
                }

                return null; 

            }



        }
    }
 
}


