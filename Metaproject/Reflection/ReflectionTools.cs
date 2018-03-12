using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Metaproject.Reflection
{
	public static class ReflectionTools
	{



	    public static Version GetExecutingAssemblyVersion()
	    {
	        return Assembly.GetExecutingAssembly().GetName().Version;
	    }

		public static Version GetAssemblyVersion(string assemblyPath)
		{
			if (!File.Exists(assemblyPath))
				return null;

			try {
				AssemblyName assemblyName = AssemblyName.GetAssemblyName(assemblyPath);
				Version version = assemblyName.Version;
				assemblyName = null;
				return version;
			} catch (Exception exc) {

			}

			return null;

		}


        public static List<string> GetPropertyNames(object obj)
        {
            List<KeyValuePair<string, object>> namesAndValues = GetPropertyNamesAndValuesAsObj(obj, null);
            var names = namesAndValues.Select(nv => nv.Key).ToList();
            return names;
        }

        public static string GetPropertyNames(object obj, string separator)
        {
            var props = GetPropertyNames(obj);
            return string.Join(separator, props);
        }


        public static List<object> GetPropertyValues(object obj, List<string> filtered = null)
        {
            List<KeyValuePair<string, object>> namesAndValues = GetPropertyNamesAndValuesAsObj(obj, filtered);
            var values = namesAndValues.Select(nv => nv.Value).ToList();
            return values;
        }

        public static string GetPropertyValues(object obj, string separator)
        {
            var props = GetPropertyValues(obj);
            return string.Join(separator, props);
        }


        public static List<string> GetPropertyValuesAsStr(object obj, List<string> filtered = null)
        {
            List<KeyValuePair<string, object>> namesAndValues = GetPropertyNamesAndValuesAsObj(obj, filtered);
            var values = namesAndValues.Select(nv => nv.Value.ToStringNull()).ToList();
            return values;
        }

        public static List<KeyValuePair<string, string>> GetPropertyValuesAsStr(object obj)
        {

            Type type = obj.GetType();
            List<KeyValuePair<string, string>> collection = new List<KeyValuePair<string, string>>();
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo prop in properties)
            {

                string name = prop.Name;
                string value = string.Empty;

                object objValue = prop.GetValue(obj, null);
                if (null != objValue)
                    value = objValue.ToString();

                collection.Add(new KeyValuePair<string, string>(name, value));
            }

            return collection;
        }

        public static List<KeyValuePair<string, object>> GetPropertyNamesAndValuesAsObj(object obj, List<string> filtered = null)
        {
            Type type = obj.GetType();
            PropertyInfo[] properties = type.GetProperties();

            if (null == filtered)
            {
                List<KeyValuePair<string, object>> collection = new List<KeyValuePair<string, object>>();
                
                foreach (PropertyInfo prop in properties)
                {
                    string name = prop.Name;
                    object objValue = prop.GetValue(obj, null);
                    collection.Add(new KeyValuePair<string, object>(name, objValue));
                }

                return collection;
            }
            else
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                if (null != filtered)
                    filtered.ForEach(f => dict.Add(f, null));

                foreach (PropertyInfo prop in properties)
                {
                    string name = prop.Name;
                    object objValue = prop.GetValue(obj, null);

                    if (dict.Keys.Contains(name))
                    {
                        dict[name] = objValue;
                    }
                }

                List<KeyValuePair<string, object>> collection = dict.ToKeyValueList();
                return collection;
            }
        }


       



		public static string GetPropertyNamesAndValuesAsString(object obj, string separator)
		{

			List<KeyValuePair<string, string>> collection = GetPropertyValuesAsStr(obj);
			List<string> lines = new List<string>();
			foreach (var kvp in collection) {
				lines.Add(string.Format("{0}={1}", kvp.Key, kvp.Value));
			}

			return string.Join(separator, lines.ToArray());
		}

        static Dictionary<Type, string> _aliases =
	new Dictionary<Type, string>()
{
    { typeof(byte), "byte" },
    { typeof(sbyte), "sbyte" },
    { typeof(short), "short" },
    { typeof(ushort), "ushort" },
    { typeof(int), "int" },
    { typeof(uint), "uint" },
    { typeof(long), "long" },
    { typeof(ulong), "ulong" },
    { typeof(float), "float" },
    { typeof(double), "double" },
    { typeof(decimal), "decimal" },
    { typeof(object), "object" },
    { typeof(bool), "bool" },
    { typeof(char), "char" },
    { typeof(string), "string" },
    { typeof(void), "void" }
};

		public static string CreateMethodBodies(Type type)
		{

			MethodInfo[] methodInfos = type.GetMethods();

			List<string> bodies = new List<string>();
			foreach (MethodInfo mi in methodInfos) {

				string body = createMethodBody(mi);
				bodies.Add(body);

			}

			string code = string.Join(Environment.NewLine, bodies.ToArray());
			return code;

		}

        static string createMethodBody(MethodInfo methodInfo)
		{
			string returnType = getTypeName(methodInfo.ReturnType);

			string methodName = methodInfo.Name;

			List<string> arguments = new List<string>();
			ParameterInfo[] parameters = methodInfo.GetParameters();

			foreach (ParameterInfo pi in parameters) {

				string parameterType = getTypeName(pi.ParameterType);
				string parameterName = pi.Name;

				string parameter = string.Format("{0} {1}", parameterType, parameterName);
				arguments.Add(parameter);

			}


			string line1 = string.Format("public {0} {1} ({2})", returnType, methodName, string.Join(", ", arguments.ToArray()));
			string[] lines = { line1, "{", "", "}" };



			string body = string.Join(Environment.NewLine, lines);

			return body;

		}

        static string getTypeName(Type type)
		{

			string tmpName;
			if (_aliases.TryGetValue(type, out tmpName))
				return tmpName;
			return type.Name;



		}




        public static string GetName(Expression<Func<object>> exp)
        {
            MemberExpression body = exp.Body as MemberExpression;

            if (body == null)
            {
                UnaryExpression ubody = (UnaryExpression)exp.Body;
                body = ubody.Operand as MemberExpression;
            }

            return body.Member.Name;
        }

        

	}
}
