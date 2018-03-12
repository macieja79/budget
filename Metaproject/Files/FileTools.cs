using IWshRuntimeLibrary;
using Metaproject.Dialog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Metaproject.Patterns;
using Microsoft.SqlServer.Server;
using File = System.IO.File;

namespace Metaproject.Files
{
    public class FileTools
    {

        #region <singleton>

        FileTools()
        {
        }

        static FileTools _instance;

        public static FileTools Instance
        {
            get
            {
                if (null == _instance) _instance = new FileTools();
                return _instance;
            }
        }

        #endregion


        string getFileName(IFileDialog dialog, string filter, bool isLoad)
        {
            LoadSaveDialogOptions options = new LoadSaveDialogOptions {Filter = filter};

            List<string> paths;
            bool isOk = dialog.ShowLoadDialog(options, out paths);
            if (!isOk || paths.IsNullOrEmpty()) return null;

            return paths.First();

        }


        public string SetDateSuffixToName(string filePath)
        {
            PathInfo pathInfo = new PathInfo(filePath);
            FileNameInfo fileName = pathInfo.GetFileName();

            DateTime now = DateTime.Now;
            string suffix = $"{now.Year}{now.Month:D2}{now.Day:D2}{now.Hour:D2}{now.Minute:D2}";
            fileName.Name = suffix + fileName.Name;

            return null;



        }


        public string GetFileFilter(string description, string extension)
        {
            return string.Format("{0}|*.{1}", description, extension);
        }

        public string GetPathForSave(IFileDialog dialog, string filter)
        {
            return getFileName(dialog, filter, false);
        }

        public string GetPathForOpen(IFileDialog dialog, string filter)
        {
            return getFileName(dialog, filter, true);
        }

        public string GetPathFromLnkFile(string lnkFilePath)
        {

            throw new NotImplementedException();

            //WshShell shell = new WshShell();
            //WshShortcut shortcut = (WshShortcut)shell.CreateShortcut(lnkFilePath);
            //return shortcut.TargetPath;
        }

        public string GetFileNameReplaced(string sourceFullName, string destinationPath)
        {
            string sourceName = Path.GetFileName(sourceFullName);
            string destinationFullPath = Path.Combine(destinationPath, sourceName);
            return destinationFullPath;
        }

        public void OpenBrowserForFolder(string path)
        {
            try
            {
                Process.Start(path);
            }
            catch (Exception exc)
            {

            }
        }

        public string GetFreeFileName(string fileName)
        {
            if (!System.IO.File.Exists(fileName)) return fileName;

            string extension = Path.GetExtension(fileName);
            string name = fileName.Replace(extension, "");


            for (int i = 0; i < 1000; ++i)
            {
                string proposedName = string.Format("{0}_{1:000}{2}", name, i, extension);
                if (!System.IO.File.Exists(proposedName)) return proposedName;
            }

            string lastRestortName = string.Format("{0}_{1}{2}", name, Guid.NewGuid(), extension);
            return lastRestortName;
        }

        public void CreateDirectoryIfNotExists(string directory)
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
        }

        public string[] GetFiles(string SourceFolder, string Filter, SearchOption searchOption)
        {
            // ArrayList will hold all file names
            ArrayList alFiles = new ArrayList();

            // Create an array of filter string
            string[] MultipleFilters = Filter.Split('|');

            // for each filter find mathing file names
            foreach (string FileFilter in MultipleFilters)
            {
                // add found file names to array list
                alFiles.AddRange(Directory.GetFiles(SourceFolder, FileFilter, searchOption));
            }

            // returns string array of relevant file names
            return (string[]) alFiles.ToArray(typeof(string));
        }



        public void RemoveLinesInFile(string filePath, IFileLinePredicate predicate, string destinationPath = null)
        {
            if (destinationPath.IsNull())
                destinationPath = filePath;

            string[] lines = File.ReadAllLines(filePath);
            int linesCount = lines.Length;

            List<string> destinationLines = new List<string>();
            for (int r = 0; r < linesCount; r++)
            {
                string oldLine = lines[r];

                if (!predicate.IsTrue(oldLine))
                {
                    destinationLines.Add(oldLine);
                }
            }

            if (predicate.IsSimulation)
                return;

            File.WriteAllLines(destinationPath, destinationLines);
        }



        public void ReplaceTextInFile(string filePath, IStringReplace stringConverter)
        {
            string[] lines = File.ReadAllLines(filePath);
            int linesCount = lines.Length;

            string[] newLines = new string[linesCount];
            for (int r = 0; r < linesCount; r++)
            {
                string oldLine = lines[r];
                string newLine = stringConverter.GetNewString(oldLine);
                newLines[r] = newLine;
            }

            if (stringConverter.IsSimulation)
                return;

            File.WriteAllLines(filePath, newLines);
        }


        public void OpenAsProcess(string p)
        {
            Process.Start(p);
        }

        public List<string> GetFilesFromSubDirectories(string path, string pattern)
        {
            List<string> files = new List<string>();

            GetFiles(path, pattern, files);

            return files;
        }

        public List<string> GetFilesFromDirectory(string path, string pattern)
        {
            string[] files = Directory.GetFiles(path, pattern);
            return files.ToList();

        }



    void GetFiles(string path, string pattern, List<string> collection)
		{
			string[] files = Directory.GetFiles(path, pattern);
			collection.AddRange(files.ToArray());

			string[] directories = Directory.GetDirectories(path);
			foreach (string dir in directories) {
				GetFiles(dir, pattern, collection);
			}
		}

        public void Save<T>(T item, IObjectSerializer<T> serializer, string path)
        {
            string str = serializer.Serialize(item);
            File.WriteAllText(path, str);
        }

        public T Load<T>(IObjectSerializer<T> serializer, string path)
        {
            string str = File.ReadAllText(path);
            T item = serializer.Deserialize(str);
            return item;
        }


    }
}
