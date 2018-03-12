using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Metaproject.Xml;
using Metaproject.Dialog;
using Metaproject.Patterns;

namespace Metaproject.Files
{
	public class XmlFileManager 
	{
        #region <singleton>

		XmlFileManager() { }

		static XmlFileManager _instance;

		public static XmlFileManager Instance
		{
			get
			{
				if (null == _instance) _instance = new XmlFileManager();
				
				return _instance;
			}

		}
        
    #endregion

        #region <pub>

		public void SaveTxtWithDialog(IFileDialog dialog, string text, string fileFilter)
		{
            
		    LoadSaveDialogOptions options = new LoadSaveDialogOptions() {Filter = fileFilter};
		    List<string> paths;

		    bool isOk = dialog.ShowSaveDialog(options, out paths);
		    if (!isOk || paths.IsNullOrEmpty()) return;
            
		    string path = paths.First();
			File.WriteAllText(path, text);
		}

        public void SaveWithDialog<T>(IFileDialog dialog, T item, string fileFilter)
		{
            List<string> paths;
            LoadSaveDialogOptions options = new LoadSaveDialogOptions() { Filter = fileFilter };
            bool isOk = dialog.ShowSaveDialog(options, out paths);
            if (!isOk || paths.IsNullOrEmpty()) return;
            string path = paths.First();

            string xml = Serializer.Instance.SerializeToXml<T>(item);
			File.WriteAllText(path, xml);
		}



        public T LoadWithDialog<T>(IFileDialog dialog, string fileFilter, out string path)
        {

            path = string.Empty;

            LoadSaveDialogOptions options = new LoadSaveDialogOptions() { Filter = fileFilter };
            List<string> paths;

            bool isOk = dialog.ShowSaveDialog(options, out paths);
            if (!isOk || paths.IsNullOrEmpty()) return default(T);

            path = paths.First();
            
            T t = Load<T>(path);
            return t;

        }

		public string LoadTxtWithDialog(IFileDialog dialog, string fileFilter)
		{
            List<string> paths;
            LoadSaveDialogOptions options = new LoadSaveDialogOptions() { Filter = fileFilter };
            bool isOk = dialog.ShowSaveDialog(options, out paths);
            if (!isOk || paths.IsNullOrEmpty()) return null;
            string path = paths.First();

            string txt = LoadTxt(path);
			return txt;
        }

        public T Load<T>(string path)
        {
            string xml = File.ReadAllText(path);
            T item = Serializer.Instance.DeserializeFromXml<T>(xml);
            return item;
        }

		public string LoadTxt(string path)
		{
			string txt = File.ReadAllText(path);
			return txt;
        }

        public void Save<T>(T item, string path)
        {
            string xml = Serializer.Instance.SerializeToXml<T>(item);
            File.WriteAllText(path, xml);
        }

	   

        #endregion
    }
}