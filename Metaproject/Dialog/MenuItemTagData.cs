using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Metaproject.Patterns;
using Metaproject.Xml;

namespace Metaproject.Dialog
{
    public class MenuItemTagData
    {
        public static string GetStr(string appId, string commandId)
        {
            string xml = $"{appId};{commandId}";
            return xml;
        }

        public static bool HasApplicationId(string tag, string applicationId)
        {
            try
            {

                string[] items = tag.Split(';');
                if (items.Length != 2) return false;

                return items[0] == applicationId;
            }
            catch
            {

            }

            return false;
        }

        public static string GetCommandId(string tag)
        {
            try
            {
                string[] items = tag.Split(';');
                if (items.Length != 2) return string.Empty;
                return items[1];
            }
            catch
            {

            }

            return string.Empty;
        }

        public string ApplicationId { get; set; }
        public string CommandId { get; set; }
    }
}
