using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Metaproject.Reflection
{
    public class PropertyMapper
    {

        #region <singleton>

        PropertyMapper() { }

        static PropertyMapper _instance;

        public static PropertyMapper Instance
        {
            get
            {
                if (null == _instance)
                    _instance = new PropertyMapper();

                return _instance;
            }

        }

        #endregion

        #region <nested>

        Type STRING_TYPE = typeof(string);

        public class MapResult
        {

            public MapResult()
            {
                UnmatchedSource = new List<string>();
                UnmatchedTarget = new List<string>();
            }

            public List<string> UnmatchedSource { get; set; }
            public List<string> UnmatchedTarget { get; set; }

            public string Inequality { get; set; }

            public bool IsObjectEqual { get; set; }

        }

        
        #endregion

        #region <pub>
        public  MapResult MapProperties(object source, object target, bool isToCompare = false)
        {


            MapResult result = new MapResult();
            result.IsObjectEqual = true;

            if (source.IsNullObj())
            {
                return result;
            }
           

            Type sourceType = source.GetType();
            Type targetType = target.GetType();

            bool isList = sourceType.IsGenericType && sourceType.GetGenericTypeDefinition() == typeof(List<>);

            if (isList)
            {
                var sourceCollection = (IList) source;
                

                MethodInfo addMethod = targetType.GetMethod("Add");


                for (int index = 0; index < sourceCollection.Count; index++)
                {
                    var sourceItem = sourceCollection[index];
                    Type sourceItemType = sourceItem.GetType();


                    if (isToCompare)
                    {
                        

                        var destinationCollection = (IList) target;
                        if (sourceCollection.Count != destinationCollection.Count)
                        {
                            result.IsObjectEqual = false;
                            return result;
                        }

                            var destinationItem = destinationCollection[index];

                        MapResult iListResult = MapProperties(sourceItem, destinationItem, isToCompare);
                        if (!iListResult.IsObjectEqual)
                        {
                            result.IsObjectEqual = false;
                            result.Inequality = iListResult.Inequality;
                            return result;
                        }
                    }
                    else
                    {
                        object destinationItem = Activator.CreateInstance(sourceItemType);

                        MapProperties(sourceItem, destinationItem, isToCompare);
                        object[] methodParameters = destinationItem.AsArray();
                        addMethod.Invoke(target, methodParameters);
                    }
                }

                return result;

            }

            PropertyInfo[] targetProperties = target.GetType().GetProperties();
            PropertyInfo[] sourceProperties = source.GetType().GetProperties();

            foreach (PropertyInfo sourceProperty in sourceProperties)
            {

                string sourceName = sourceProperty.Name;
                Type sourcePropertyType = sourceProperty.PropertyType;

                PropertyInfo matchedProperty = targetProperties
                    .FirstOrDefault(p =>
                        p.CanWrite &&
                        p.Name == sourceName &&
                        p.PropertyType == sourcePropertyType);

                if (null == matchedProperty)
                {
                    result.UnmatchedSource.Add(sourceName);
                    continue;
                }

                object sourceValue = sourceProperty.GetValue(source, null);


                if (sourcePropertyType.IsClass && !(sourcePropertyType == STRING_TYPE))
                {

                    object destinationValue = matchedProperty.GetValue(target);

                    if (isToCompare)
                    {
                        var iResult = MapProperties(sourceValue, destinationValue, isToCompare);
                        if (!iResult.IsObjectEqual)
                        {
                            result.IsObjectEqual = false;
                            result.Inequality = iResult.Inequality;
                            return result;
                        }
                    }
                    else
                    {
                        if (destinationValue.IsNullObj())
                        {
                            destinationValue = Activator.CreateInstance(matchedProperty.PropertyType);
                        }
                            MapProperties(sourceValue, destinationValue, isToCompare);
                            matchedProperty.SetValue(target, destinationValue);
                    }

                    continue;
                }

                if (isToCompare)
                {
                    object destinationValue = matchedProperty.GetValue(target);
                    bool isEqual = object.Equals(sourceValue, destinationValue);
                    if (!isEqual)
                    {
                        result.Inequality = $"({sourceProperty}){sourceValue}!=({matchedProperty}){destinationValue}";
                        result.IsObjectEqual = false;
                        return result;
                    }
                }
                else
                {
                    matchedProperty.SetValue(target, sourceValue, null);
                }


            }
            
            return result;

        }

        

        #endregion

    }
}
