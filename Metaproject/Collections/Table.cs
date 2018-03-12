using System;
using System.Collections.Generic;

using System.Drawing;



namespace Metaproject.Collections
{

    public class MtTable<R, C, V>
    {

        
        #region <members>
        readonly int _numberOfColumns = -1;
        C[] _headers = null;
        Dictionary<R, V[]> _values = null;
        #endregion
                
        #region <constructor>
        public MtTable(params C[] columnsHeaders)
        {
            _values = new Dictionary<R, V[]>();
            _numberOfColumns = columnsHeaders.Length;

            _headers = new C[columnsHeaders.Length];
            for (int i = 0; i < columnsHeaders.Length; i++)
                _headers[i] = columnsHeaders[i];
        }
        #endregion

        #region <public methods>
        public void AddRow(R header, params V[] values)
        {

            if (values.Length != _numberOfColumns)
                throw new ApplicationException("B³êdna liczba parametrów");

            V[] rowValues = new V[values.Length];
            for (int i = 0; i < rowValues.Length; i++)
                rowValues[i] = values[i];


            _values.Add(header, rowValues);
            
        }

        public void AddRow(R header, List<V> values)
        {
        
            if (values.Count != _numberOfColumns)
                throw new ApplicationException("B³êdna liczba parametrów");

            V[] rowValues = new V[values.Count];
            for (int i = 0; i < rowValues.Length; i++)
                rowValues[i] = values[i];

            _values.Add(header, rowValues);
        
        }

        public C[] Headers
        {
            get
            {
                return _headers;
            }
        }

        public bool HasColumnHeader(C header)
        {

            foreach (C iHeader in Headers)
            {
                if (iHeader.Equals(header))
                    return true;
            }

            return false;

        }

        public bool HasRowHeader(R header)
        {

            R[] rowHeaders = GetRowHeaders();

            foreach (R iHeader in rowHeaders)
            {
                if (iHeader.Equals(header))
                    return true;
            }

            return false;

        }



        public R[] GetRowHeaders()
        {

            
            R[] headers = new R[_values.Count];

            int i = 0;
            foreach (R header in _values.Keys)
            {
                headers[i] = header;
                i++;    
            }

            return headers;
                        
        }

        public V[] GetColumnValues(C column)
        {
            int c = GetColumnIndex(column);
            if (c < 0) return null;
            
            V[] columnValues = new V[_values.Count];

            int iValue = 0;
            foreach (V[] rowValues in _values.Values)
            {
                columnValues[iValue] = rowValues[c];
                iValue++;
            }

            return columnValues;
        }

        public V[] GetRowValues(R row)
        {
            return _values[row];
        }

        public V GetValue(R row, C column)
        {

            V v = default(V);

            int c = GetColumnIndex(column);
            V[] values = GetRowValues(row);

            v = values[c];
            return v;

        }

        public R GetRowHeader(C columnHeader, V value)
        {
            int c = GetColumnIndex(columnHeader);
            foreach (R iHeader in _values.Keys)
            {
                V[] rowValues = _values[iHeader];

                if (rowValues[c].Equals(value))
                    return iHeader;
            }

            return default(R);
        }

      

       

        #endregion

        #region <private methods>

        int GetColumnIndex(C columnHeader)
        {
            for (int i = 0; i < _numberOfColumns; i++)
            {
                if (columnHeader.Equals(_headers[i]))
                    return i;
            }

            return -1;
        }

        int GetValueIndex(V[] values, V value)
        {
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i].Equals(value))
                    return i;
            }

            return -1;
        }

        #endregion

    }






}