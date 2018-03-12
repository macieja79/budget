using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Budget.Core.UI;
using Metaproject.Files;

namespace Budget.UI
{
    public class FilePathProvider : IFilePathProvider
    {
        public List<string> GetFilePaths()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.RestoreDirectory = true;
            dialog.Multiselect = true;
            dialog.Filter = FileTools.Instance.GetFileFilter("Pliki csv", "csv");

            var dialogResult = dialog.ShowDialog();
            if (dialogResult != DialogResult.OK && dialogResult != DialogResult.Yes)
                return null;

            return dialog.FileNames.ToList();

        }
    }
}