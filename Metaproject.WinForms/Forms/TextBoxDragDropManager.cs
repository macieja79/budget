using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Metaproject.WinForms.Forms
{
    public class TextBoxDragDropManager
    {

        public static void Register(params TextBox[] textBoxes)
        {
            foreach (var textBox in textBoxes)
            {
                textBox.AllowDrop = true;
                textBox.DragEnter += TextOnDragEnter;
                textBox.DragDrop += TextBoxOnDragDrop;
            }
        }

        public static void Unregister(params TextBox[] textBoxes)
        {
            foreach (var textBox in textBoxes)
            {
                textBox.AllowDrop = false;
                textBox.DragEnter -= TextOnDragEnter;
                textBox.DragDrop -= TextBoxOnDragDrop;
            }
        }


        private static void TextOnDragEnter(object sender, DragEventArgs dragEventArgs)
        {
            dragEventArgs.Effect = DragDropEffects.Copy;
        }


        private static void TextBoxOnDragDrop(object sender, DragEventArgs e)
        {
            try
            {
                TextBox textBox = sender as TextBox;
                if (null == textBox) return;

                string path = null;
                string[] formats = e.Data.GetFormats();
                foreach (string format in formats)
                {
                    object obj = e.Data.GetData(format);
                    if (obj is string)
                    {
                        path = (string)obj;
                        break;
                    }
                }

                textBox.Text = path;
            }
            catch
            {
                // ignored
            }
        }
    }
}
