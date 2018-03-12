using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Metaproject.Controls
{
    public class ControlsTools
    {

        [DllImportAttribute("gdi32.dll")]
        private static extern bool BitBlt(
        IntPtr hdcDest,
        int nXDest,
        int nYDest,
        int nWidth,
        int nHeight,
        IntPtr hdcSrc,
        int nXSrc,
        int nYSrc,
        int dwRop);


        public static Bitmap CaptureControl(Control control)
        {
            Bitmap controlBmp;
            using (Graphics g1 = control.CreateGraphics())
            {
                controlBmp = new Bitmap(control.Width, control.Height, g1);
                using (Graphics g2 = Graphics.FromImage(controlBmp))
                {
                    IntPtr dc1 = g1.GetHdc();
                    IntPtr dc2 = g2.GetHdc();
                    BitBlt(dc2, 0, 0, control.Width, control.Height, dc1, 0, 0, 13369376);

                    g1.ReleaseHdc(dc1);
                    g2.ReleaseHdc(dc2);

                }
            }

            return controlBmp;
        }
    }
}
