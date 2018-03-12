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

  


    public partial class MtProgressDouble : Form
    {
        
        #region <constructor>
        public MtProgressDouble()
        {
            InitializeComponent();

            DefHeight = this.Height;

            setProgress(false);
        }

        #endregion


        #region <props>
        ProgressBar _first;
        ProgressBar _second;
        TextBox _firstTxt;
        TextBox _secondTxt;
        #endregion

        bool _isSwapped;
        public bool IsSwapped
        {
            get
            {
                return _isSwapped;
            }
            set
            {
                _isSwapped = value;
                setProgress(_isSwapped);
            }
        }

        void setProgress(bool isSwapped)
        {
            if (!isSwapped)
            {
                _first = progressBar1;
                _second = progressBar2;

                _firstTxt = textBox1;
                _secondTxt = textBox2;
            }
            else
            {
                _first = progressBar2;
                _second = progressBar1;

                _firstTxt = textBox2;
                _secondTxt = textBox1;
            }
        }

        public void CenterToControl(Control control)
        {
            int x = control.Left + (int)(0.5 * (control.Width - this.Width));
            int y = control.Top + (int)(0.5 * (control.Height - this.Height));
            this.Location = new Point(x, y);
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
               _first.Visible = false;
            else
               _first.Visible = true;
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

			_firstTxt.Text = Value;
            _firstTxt.Refresh();
			Refresh();

		}

        private delegate string InvokeGetLabelText();
		private string GetLabelText()
		{
			if (this.InvokeRequired)
				return (string)this.Invoke(new InvokeGetLabelText(GetLabelText));

			return _firstTxt.Text;

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

        private delegate void InvokeSetSteps(int value);
        private void SetSteps(int value)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new InvokeSetSteps(SetSteps), value);
                return;
            }

            _first.Visible = value > 0;

            _first.Value = 0;
            _first.Minimum = 0;
            _first.Maximum = value;
            _first.Refresh();
        }

        

        private delegate int InvokeGetSteps();
		private int GetSteps()
		{
			if (this.InvokeRequired)
				return (int)this.Invoke(new InvokeGetSteps(GetSteps));

			return _first.Maximum;

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


			_first.Value = Pos;
			_first.Refresh();

		}
        private delegate int InvokeGetPosition();
		private int GetPosition()
		{
			if (this.InvokeRequired)
				return (int)this.Invoke(new InvokeGetPosition(GetPosition));


			return _first.Value;

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
			if (_first.Value < _first.Maximum)
			{
				_first.Value += 1;
				_first.Refresh();
			}

		}
        private delegate void InvokeStepBack();

        #region <label text 2>
        public string LabelText2
        {
            get
            {
                return GetLabelText2();
            }
            set
            {
                SetLabelText2(value);
            }
        }

    
        private void SetLabelText2(string Value)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new InvokeSetLabelText(SetLabelText2), Value);
                return;
            }

            Application.DoEvents();

            _secondTxt.Text = Value;
            _secondTxt.Refresh();
            Refresh();

        }

      
        private string GetLabelText2()
        {
            if (this.InvokeRequired)
                return (string)this.Invoke(new InvokeGetLabelText(GetLabelText2));

            return _secondTxt.Text;

        }
        #endregion

        #region <steps 2>
        public int Steps2
        {
            get
            {
                return GetSteps2();
            }
            set
            {
                SetSteps2(value);
            }
        }

      
        private void SetSteps2(int value)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new InvokeSetSteps(SetSteps2), value);
                return;
            }

            _second.Visible = value > 0;
            _second.Value = 0;
            _second.Minimum = 0;
            _second.Maximum = value;
            _second.Refresh();
        }



      
        private int GetSteps2()
        {
            if (this.InvokeRequired)
                return (int)this.Invoke(new InvokeGetSteps(GetSteps2));

            return _second.Maximum;

        }
        #endregion

        #region <position 2>
        public int Position2
        {
            get
            {
                return GetPosition2();
            }
            set
            {
                SetPosition2(value);
            }
        }
   
        private void SetPosition2(int Pos)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new InvokeSetPosition(SetPosition2), Pos);
                return;
            }

            Application.DoEvents();


            _second.Value = Pos;
            _second.Refresh();

        }
       
        private int GetPosition2()
        {
            if (this.InvokeRequired)
                return (int)this.Invoke(new InvokeGetPosition(GetPosition2));


            return _second.Value;

        }
        #endregion

    
        public void Step2()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new InvokeStep(Step2));
                return;
            }
            Application.DoEvents();
            if (_second.Value < _second.Maximum)
            {
                _second.Value += 1;
                _second.Refresh();
            }

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

  

    
   
    
}