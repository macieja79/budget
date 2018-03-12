using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Metaproject.Controls
{

  


    public partial class MtProgress : Form
    {

        #region <nested types>
        public class OwnerWindowData
        {
            public OwnerWindowData(int width, int height, int x, int y)
            {
                Height = height;
                Width = width;
                X = x;
                Y = y;
            }

            public int Height { get; set; }
            public int Width { get; set; }
            public int X { get; set; }
            public int Y { get; set; }

        }

  


        #endregion
        
        #region <constructor>
        public MtProgress()
        {  
            InitializeComponent();

          

            DefHeight = this.Height;

            SetButtons(0);
        }
        #endregion
                        
        public void SetIcon(Icon PrgIcon)
        {          
            //Bitmap Bmp = PrgIcon.ToBitmap();
            //PictureBox.Image = System.Drawing.Image.FromHbitmap(Bmp.GetHbitmap());
            //PictureBox.Update(); 
        }

        
        private delegate void InvokeSetVisible(bool Value);
        public void SetVisible(bool Value)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new InvokeSetVisible(SetVisible), Value);
                return;
            }

            this.Visible = Value;
        }

        private delegate bool InvokeGetVisible();
        public bool GetVisible()
        {
            if (this.InvokeRequired)
                return (bool)this.Invoke(new InvokeGetVisible(GetVisible));

            return this.Visible;
        }

        private delegate void InvokeShowProgress(IWin32Window Parent);
        public void ShowProgress(IWin32Window Parent)
        {
            if(Parent == null)
                Parent = Form.ActiveForm;
            ShowProgress(Parent, Modal);
        }

        public void ShowProgress(IWin32Window Parent, bool Modal)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new InvokeShowProgress(ShowProgress), Parent);
                return;
            }

          

            bool IsParent = (Parent != null && Parent.Handle != null);
            IsParent = false;

            
            if (IsParent)
            {
                Control ctrl = Form.FromHandle(Parent.Handle);
                Point pt = ctrl.Location;
 
                this.StartPosition = FormStartPosition.Manual;
                this.Left = pt.X + ((ctrl.Width / 2) - this.Size.Width) / 2;
                this.Top = pt.Y + ((ctrl.Height / 2) - this.Size.Height) / 2;
            }
            else
            {
                this.StartPosition = FormStartPosition.Manual;
                this.Left = ((Screen.PrimaryScreen.Bounds.Width / 2) - this.Size.Width) / 2;
                this.Top = ((Screen.PrimaryScreen.Bounds.Height / 2) - this.Size.Height) / 2;
            }

            if(Steps == 0)
               progressBar1.Visible = false;
            else
               progressBar1.Visible = true;
            this.TopMost = true;

            if (IsParent)
            {
                this.Show(Parent);
                this.Invalidate(true);
                this.Refresh(); 
            }
            else if (!Modal)
            {
                this.Visible = true;
                this.Invalidate(true);
                //Logo.Update();
                //Logo.Refresh();
                //Logo.Invalidate();
            }
            else
                this.ShowDialog(Parent); 
        }
        private delegate bool InvokeCloseProgress();
        public bool CloseProgress()
        {
            if (this.InvokeRequired)
                return (bool)this.Invoke(new InvokeCloseProgress(CloseProgress));

            LabelHeader = "";
            LabelText = "";
            Position = 0;

          

            if (this.Modal)
                this.Close();
            else
            {
                this.Visible = false;
            }

            return true;
        }



		#region <header>
		public string LabelHeader
        {
            get
            {
                return GetLabelHeader(); 
            }
            set
            {
                SetLabelHeader(value);
            }
        }

        private delegate void InvokeSetLabelHeader(string Value);
		private void SetLabelHeader(string Value)
		{
			if (this.InvokeRequired)
			{
				this.Invoke(new InvokeSetLabelHeader(SetLabelHeader), Value);
				return;
			}


			LabelTop1.Text = Value;
			LabelTop1.Refresh();
			Refresh();

		}

        private delegate string InvokeGetLabelHeader();
		private string GetLabelHeader()
		{
			if (this.InvokeRequired)
				return (string)this.Invoke(new InvokeGetLabelHeader(GetLabelHeader));

			Application.DoEvents();

			return LabelTop1.Text;

		}
		#endregion

		#region <label text>
		public string LabelText
        {
            get
            {
                return GetLabelText(); 
            }
            set
            {
                SetLabelText(value); 
            }
        }

        private delegate void InvokeSetLabelText(string Value);
		private void SetLabelText(string Value)
		{
			if (this.InvokeRequired)
			{
				this.Invoke(new InvokeSetLabelText(SetLabelText), Value);
				return;
			}

			Application.DoEvents();

			textBox1.Text = Value;
            textBox1.Refresh();
			Refresh();

		}

        private delegate string InvokeGetLabelText();
		private string GetLabelText()
		{
			if (this.InvokeRequired)
				return (string)this.Invoke(new InvokeGetLabelText(GetLabelText));

			return textBox1.Text;

		}
		#endregion

		#region <steps>
		public int Steps
        {
            get
            {
                return GetSteps();
            }
            set
            {
                SetSteps(value);
            }
        }

        private delegate void InvokeSetSteps(int Value);
        private void SetSteps(int Value)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new InvokeSetSteps(SetSteps), Value);
                return;
            }

           
                if (Value != 0)
                    progressBar1.Visible = true;
                else
                    progressBar1.Visible = false;
                 progressBar1.Minimum = 0;
                 progressBar1.Maximum = Value;
                 progressBar1.Refresh();
           
        }

        

        private delegate int InvokeGetSteps();
		private int GetSteps()
		{
			if (this.InvokeRequired)
				return (int)this.Invoke(new InvokeGetSteps(GetSteps));

			return progressBar1.Maximum;

		}
		#endregion

		#region <position>
		public int Position
        {
            get
            {
                return GetPosition();
            }
            set
            {
                SetPosition(value);
            }
        }
        private delegate void InvokeSetPosition(int Pos);
		private void SetPosition(int Pos)
		{
			if (this.InvokeRequired)
			{
				this.Invoke(new InvokeSetPosition(SetPosition), Pos);
				return;
			}

			Application.DoEvents();


			progressBar1.Value = Pos;
			progressBar1.Refresh();

		}
        private delegate int InvokeGetPosition();
		private int GetPosition()
		{
			if (this.InvokeRequired)
				return (int)this.Invoke(new InvokeGetPosition(GetPosition));


			return progressBar1.Value;

		}
		#endregion

		private delegate void InvokeStep();
		public void Step()
		{
			if (this.InvokeRequired)
			{
				this.Invoke(new InvokeStep(Step));
				return;
			}

			Application.DoEvents();


			if (progressBar1.Value < progressBar1.Maximum)
			{
				progressBar1.Value += 1;
				progressBar1.Refresh();
			}

		}
        private delegate void InvokeStepBack();
   

        public long Buttons
        {
            get
            {
                return ButtonsSet;
            }
            set
            {
                ButtonsSet = value;
                SetButtons(value);
            }
        }
        private delegate void InvokeSetButtons(long Buttons);
		public void SetButtons(long Buttons)
		{
			if (this.InvokeRequired)
			{
				this.Invoke(new InvokeSetButtons(SetButtons), Buttons);
				return;
			}


			if (ButtonsSet == 0)
			{
				if (this.Height == DefHeight)
				{
					//this.Height -= ButtonPanel.Height;
					progressBar1.Invalidate();
				}
			}
			else
			{
				if (this.Height < DefHeight)
				{
					//this.Height += ButtonPanel.Height;
					progressBar1.Invalidate();
				}
			}

		}

        private void buttonBreak_Click(object sender, EventArgs e)
        {
            ProgressEventArgs Args = new ProgressEventArgs();
            Args.Break = true;
            if (ProgressEventHandler != null)
                ProgressEventHandler.Invoke(this, Args);  
        }

        public event ProgressEventHandler ProgressEventHandler;

     
        long ButtonsSet;
        private int DefHeight = 0;

        private void Progress_Load(object sender, EventArgs e)
        {

        }

        private void LabelBottom1_Click(object sender, EventArgs e)
        {

        }
           
       

    }

    internal partial class PanelEx : Panel
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowDC(IntPtr hwnd);
        [DllImport("user32.dll")]
        private static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);

        private Color _borderColor = Color.Black;
        private int _borderWidth = 1;

        public int BorderWidth
        {
            get { return _borderWidth; }
            set { _borderWidth = value; }
        }

        public Color BorderColor
        {
            get { return _borderColor; }
            set { _borderColor = value; }
        }

        public PanelEx()
        {
            gradientFill = false;
            SetStyle
            (ControlStyles.DoubleBuffer,
                true);
            SetStyle
            (ControlStyles.AllPaintingInWmPaint,
                false);
            SetStyle
            (ControlStyles.ResizeRedraw,
                true);
            SetStyle
            (ControlStyles.UserPaint,
                true);
            SetStyle
            (ControlStyles.SupportsTransparentBackColor,
                true);

        }

        bool gradientFill;
        public bool GradientFill
        {
            get
            {
                return gradientFill;
            }
            set
            {
                gradientFill = value;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (gradientFill)
            {
                Rectangle BaseRectangle = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
                Brush Gradient_Brush = new System.Drawing.Drawing2D.LinearGradientBrush(BaseRectangle, SystemColors.Control, SystemColors.Window, System.Drawing.Drawing2D.LinearGradientMode.Vertical);
                e.Graphics.FillRectangle(Gradient_Brush, BaseRectangle);
            }
            else
                base.OnPaint(e);

            if (this.BorderStyle == BorderStyle.FixedSingle)
            {
                IntPtr hDC = GetWindowDC(this.Handle);
                Graphics g = Graphics.FromHdc(hDC);

                ControlPaint.DrawBorder(
                    g,
                    new Rectangle(0, 0, this.Width, this.Height),
                    _borderColor,
                    _borderWidth,
                    ButtonBorderStyle.Solid,
                    _borderColor,
                    _borderWidth,
                    ButtonBorderStyle.Solid,
                    _borderColor,
                    _borderWidth,
                    ButtonBorderStyle.Solid,
                    _borderColor,
                    _borderWidth,
                    ButtonBorderStyle.Solid);
                g.Dispose();
                ReleaseDC(Handle, hDC);
            }
        }
    }

    public class ProgressEventArgs : EventArgs
    {
        public bool Break;
    }

    public delegate void ProgressEventHandler(object sender, ProgressEventArgs e);

   
    
}