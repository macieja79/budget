using Metaproject.Reg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Metaproject.Reg
{
    public class SettingsSerializer<T>
    {
        #region <members>

        Settings _settings;

        #endregion

        #region <ctor>

        public SettingsSerializer(Settings.Domain domain, string settingsPath)
        {
            _settings = new Settings(domain, settingsPath);
        }

        #endregion

        #region <pub>

        public void Serialize(T obj)
        {
            
            PropertyInfo[] properties = obj.GetType().GetProperties();
            foreach (PropertyInfo propertyInfo in properties)
            {
                if (!propertyInfo.CanWrite) continue;

                string name = propertyInfo.Name;
                object value = propertyInfo.GetValue(obj, null);
                Type type = propertyInfo.PropertyType;

                if (type.Equals(typeof(bool)))
                    _settings.SetBoolean(name, (bool)value);
                if (type.Equals(typeof(Int32)))
                    _settings.SetInt32(name, (Int32)value);
                if (type.Equals(typeof(Int16)))
                    _settings.SetInt16(name, (Int16)value);
                if (type.Equals(typeof(Int64)))
                    _settings.SetInt64(name, (Int64)value);
                if (type.Equals(typeof(string)))
                    _settings.SetString(name, (string)value);
                // TODO: wyklepac do konca
            }

        }

        public T Deserialize()
        {

            T obj = Activator.CreateInstance<T>();


            PropertyInfo[] properties = obj.GetType().GetProperties();
            foreach (PropertyInfo propertyInfo in properties)
            {
                if (!propertyInfo.CanWrite) continue;
                
                string name = propertyInfo.Name;
                if (!_settings.HasValue(name)) continue;

                Type type = propertyInfo.PropertyType;
                object value = null;
                if (type.Equals(typeof(bool)))
                    value = _settings.GetBoolean(name);
                else if (type.Equals(typeof(Int32)))
                    value = _settings.GetInt32(name);
                else if (type.Equals(typeof(Int16)))
                    value = _settings.GetInt16(name);
                else if (type.Equals(typeof(Int64)))
                    value = _settings.GetInt64(name);
                else if (type.Equals(typeof(string)))
                    value = _settings.GetString(name);
                else
                    continue;

                // TODO: wyklepac do konca
                propertyInfo.SetValue(obj, value, null);
            }

            return obj;

        }

        #endregion

    }
}
