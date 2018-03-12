using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Metaproject.Graphic
{
    public class GraphicsTools
    {
        #region <singleton>

        GraphicsTools()
        {
        }

        static GraphicsTools _instance;

        public static GraphicsTools Instance
        {
            get
            {
                if (null == _instance) _instance = new GraphicsTools();
                return _instance;
            }
        }

        #endregion

        ColorConverter _converter = new ColorConverter();

        public Bitmap GetSquareOfColor(Color c, int size)
        {
            SolidBrush sb = new SolidBrush(c);
            Pen p = new Pen(Color.Black);

            if (c.IsEmpty)
            {
                sb = new SolidBrush(Color.White);
                p = new Pen(Color.White);
            }

            Bitmap b = new Bitmap(size, size);
            Graphics g = Graphics.FromImage(b);
            g.FillRectangle(sb, 0, 0, size - 1, size - 1);
            g.DrawRectangle(p, 0, 0, size - 1, size - 1);
            return b;
        }

        public Color GetColorFromStr(string str)
        {
            if (str.IsNull()) return Color.Empty;
            Color color = (Color)_converter.ConvertFromString(str);
            return color;
        }

        public string GetColorAsStr(Color color)
        {
            string colorString = string.Format("#{0:X2}{1:X2}{2:X2}", color.R, color.G, color.B);
            return colorString;
        }

}
}
