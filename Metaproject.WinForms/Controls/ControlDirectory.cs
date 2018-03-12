using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;

using System.Collections.Generic;
using Metaproject.Strings;
using Metaproject.Files;

namespace Metaproject.Controls
{
    public partial class ControlDirectory : UserControl
    {

        #region <nested types>
        public enum ControlType
        {
            Directory, File
        }
        #endregion

        #region <events, delegates>
        public delegate void DelegateDirectorySelected(object sender, EventArgs e);
        public event DelegateDirectorySelected EventDirectorySelected;
        #endregion

        #region <members>

        private bool _enabled;
        private List<string> _paths = new List<string>();

        private int _maxNumber = 0;
        private int _defaultMaxNumber = 10;

        string[] _fileNames;

        #endregion

        #region <constructor>
        public ControlDirectory()
        {
            InitializeComponent();
        }
        #endregion

        #region <properties>

        [Category("Custom")]
        public ControlType TypeOfControl { get; set; }

        [Category("Custom")]
        public string DirectoryPath
        {

            get { return this.textBox1.Text; }
            set { this.textBox1.Text = value; }
        }

        [Category("Custom")]
        public string DirectoryName
        {
            get { return this.label1.Text; }
            set { this.label1.Text = value; }
        }

        [Category("Custom")]
        public bool IsEnabled
        {
            get
            {
                return _enabled;
            }

            set
            {
                this.tsbtn.Enabled = value;
                this.textBox1.Enabled = (value && IsEdited);
                this.label1.Enabled = value;
                _enabled = value;
            }
        }

        [Category("Custom")]
        public bool IsEdited
        {
            get { return this.textBox1.Enabled; }
            set { this.textBox1.Enabled = value; }
        }

        [Category("Custom")]
        public string FileFilter { get; set; }


        [Category("Custom")]
        public string PromptText { get; set; }

        [Category("Custom")]
        public int MaxNumberOfPaths
        {
            get
            {
                if (_maxNumber == 0)
                    return _defaultMaxNumber;
                else
                    return _maxNumber;
            }

            set
            {
                _maxNumber = value;
            }




        }

        public bool CheckIfDirectory()
        {
            
                return (TypeOfControl == ControlType.Directory && chbFiles.IsEnabledAndChecked());

            
        }

        [Category("Custom")]
        public bool IsDirectoryOptionEnabled
        {
            get
            {
                return chbFiles.Visible;
            }
            set
            {
                chbFiles.Visible = value;
            }
        }

        public string[] GetFileNames()
        {
            return _fileNames;
        }

        #endregion

        #region <events>

        void OnButtonClick(object sender, EventArgs e)
        {
            if (TypeOfControl == ControlType.Directory && chbFiles.Checked)
                GetSourceDirectory();
            else if (TypeOfControl == ControlType.Directory && !chbFiles.Checked)
                GetFiles();
            else if (TypeOfControl == ControlType.File)
                GetFile();
        }


        void OnFileOrDirectorySelected(string filePath)
        {
            this.DirectoryPath = filePath;

            AddPathToList(filePath);
            SetSplitButton();

            if (null != EventDirectorySelected)
                EventDirectorySelected(this, new EventArgs());
        }

        void OnDropDownItemClick(object sender, ToolStripItemClickedEventArgs e)
        {
            string path = e.ClickedItem.Text;
            OnFileOrDirectorySelected(path);
        }
        
        void OnShowInBrowserButtonClick(object sender, EventArgs e)
        {
            string path = DirectoryPath;
         
            if (TypeOfControl == ControlType.File && File.Exists(path))
            {
                string directory = Path.GetDirectoryName(path);
                FileTools.Instance.OpenBrowserForFolder(directory);
                return;
            }

            if (TypeOfControl == ControlType.Directory && Directory.Exists(path))
            {
                FileTools.Instance.OpenBrowserForFolder(path);
                return;
            }
           
        }

        #endregion

        #region <public methods>

        public void SetPathsFromString(string pathsAsString, char separator)
        {

            string[] paths = pathsAsString.Split(separator);
            if (null == paths) return;

            _paths.Clear();
            foreach (string p in paths)
            {
                if (string.IsNullOrEmpty(p)) continue;

                FileInfo fi = new FileInfo(p);
                bool isFile = (null != fi);
                
                DirectoryInfo di = new DirectoryInfo(p);
                bool isDirectory = (null != di);

                if (isFile || isDirectory)
                {
                    _paths.Add(p);
                }

            }

            SetSplitButton();

        }

        public string GetPathsAsString(char separator)
        {
            string separatedPaths = MtString.GetListSeparated(_paths, separator);
            return separatedPaths;
        }

     

        #endregion

        #region <private methods>
        private void GetSourceDirectory()
        {
            if (!string.IsNullOrEmpty(DirectoryPath))
            {
                if (System.IO.Directory.Exists(DirectoryPath))
                {
                    folderBrowserDialog1.SelectedPath = DirectoryPath;
                }
            }

            folderBrowserDialog1.Description = PromptText;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = folderBrowserDialog1.SelectedPath;

                if (path.Length > 2)
                {
                    if (path[path.Length - 1] == '\\')
                    {
                        if (path[1] != ':')
                        {
                            path = path.Substring(0, path.Length - 1);
                        }
                    }

                    OnFileOrDirectorySelected(path);
                    
                }
            }
        }

        private void GetFile()
        {
            if (!string.IsNullOrEmpty(FileFilter))
                openFileDialog1.Filter = FileFilter;

            if (!string.IsNullOrEmpty(DirectoryPath))
            {
                string directory = DirectoryPath;
                if (!string.IsNullOrEmpty(directory))
                {
                    if (System.IO.File.Exists(directory))
                    {
                        openFileDialog1.FileName = directory;
                        
                        string initDir = Path.GetDirectoryName(directory);
                        openFileDialog1.InitialDirectory = initDir;
                    }
                }
            }

            openFileDialog1.Title = PromptText;

            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                string filePath = openFileDialog1.FileName;

                OnFileOrDirectorySelected(filePath);
            }
        }

        private void GetFiles()
        {
            if (!string.IsNullOrEmpty(FileFilter))
                openFileDialog1.Filter = FileFilter;


            openFileDialog1.Multiselect = true;

            if (!string.IsNullOrEmpty(DirectoryPath))
            {
                string directory = DirectoryPath;
                if (!string.IsNullOrEmpty(directory))
                {
                    if (System.IO.File.Exists(directory))
                    {
                        openFileDialog1.FileName = directory;

                        string initDir = Path.GetDirectoryName(directory);
                        openFileDialog1.InitialDirectory = initDir;
                    }
                }
            }

            openFileDialog1.Title = PromptText;

            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                string filePath = Path.GetDirectoryName(openFileDialog1.FileName);
                _fileNames = openFileDialog1.FileNames; 

                OnFileOrDirectorySelected(filePath);
            }
        }


        private void SetSplitButton()
        {
            tsbtn.DropDownItems.Clear();
            
            int n = _paths.Count;
            for (int i = n - 1; i >= 0; i--)
            {
                string iPath = _paths[i];
                tsbtn.DropDownItems.Add(iPath);

            }

        }

        void AddPathToList(string path)
        {

            int n = _paths.Count;
            for (int i = 0; i < _paths.Count; i++)
            {
                string iPath = _paths[i];
                if (iPath == path)
                {
                    _paths.RemoveAt(i);
                    i--;
                }
            }

            if (_paths.Count == MaxNumberOfPaths)
            {
                for (int i = 0; i < MaxNumberOfPaths - 1; i++)
                    _paths[i] = _paths[i + 1];

                _paths.RemoveAt(MaxNumberOfPaths - 1);
            }

            _paths.Add(path);

        }

        #endregion

        

       
        

    }
}
