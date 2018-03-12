using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Metaproject.Xml
{
    public class Serializer
    {

        #region <singleton>

        Serializer() { }

        static Serializer _instance;

        public static Serializer Instance
        {
            get
            {
                if (null == _instance) _instance = new Serializer();
                return _instance;
            }
        }   

        #endregion

        #region <pub>
        public string SerializeToXml<T>(T element)
        {

            XmlSerializer serializer = new XmlSerializer(typeof(T));

            string str = string.Empty;

            using (TextWriter tw = new StringWriter())
            {
                serializer.Serialize(tw, element);
                str = tw.ToString();
            }

            return str;

        }

        public T DeserializeFromXml<T>(string xmlStr)
        {


            XmlSerializer serializer = new XmlSerializer(typeof(T));

            T t = default(T);

           

            using (TextReader tw = new StringReader(xmlStr))
            {
                try
                {

                    t = (T)serializer.Deserialize(tw);
                }
                catch(Exception exc)
                {
                    return t;
                }
            }

            return t;

        }
        #endregion

    }
}
