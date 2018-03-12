﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.Remoting;

namespace Metaproject.Controls
{
    /// <summary>
    /// A Panel that supports binding an Enum property on an arbitrary object to a set of RadioButton controls. 
    /// </summary>
    /// <remarks>
    /// USAGE: Add any number of RadioButtons. Assign the numeric value of your Enum's corresponding option to the Tag property of each button. (E.g., 
    /// if MyEnum.Foo 0, MyEnum.Bar = 1, then radioButton1.Text = "Foo", radioButton1.Tag = 0, radioButton2.Text = "Bar", RadioButton2.tag = 1, and so
    /// on.) RadioPanel will take care of setting the appropriate RadioButton control, and communicating changes back to your object when the
    /// user selects another option. 
    /// 
    /// Author: Jay Andrew Allen, August 2007
    /// </remarks>
    public class RadioPanel : Panel
    {
        public event RadioSelectionChangedEventHandler RadioSelectionChanged;

        private object _dataSource;
        private string _valueMember;
        private bool _processPropertyChange;

        private EventInfo _ei;
        private PropertyChangedEventHandler _pceh;
        private PropertyInfo _pi;

        public RadioPanel()
            : base()
        {
            _processPropertyChange = true;

            _pceh = new PropertyChangedEventHandler(radioButtonPanel_PropertyChanged);
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);

            if (e.Control is RadioButton)
            {
                RadioButton rb = (RadioButton)e.Control;
                rb.CheckedChanged += rb_CheckedChanged;
            }
        }

        public string ValueMember
        {
            get
            {
                return _valueMember;
            }
            set
            {
                if (value != null)
                {
                    _valueMember = value;
                    if (_dataSource != null)
                    {
                        SetRadioButtonBinding();
                    }
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object DataSource
        {
            get
            {
                return _dataSource;
            }
            set
            {
                if (value == null) { throw new ArgumentNullException("DataSource"); }

                if (_ei != null)
                {
                    _ei.RemoveEventHandler(_dataSource, _pceh);
                }

                // Set new binding source.
                _dataSource = value;

                // Does this property's object support INotifyPropertyChanged? If so, listen to property changes so that we can update our binding if the
                // source changes.
                Type iface = _dataSource.GetType().GetInterface("INotifyPropertyChanged");
                if (iface != null)
                {
                    _ei = iface.GetEvent("PropertyChanged");
                    if (_ei != null) // which would be weird if it did...
                    {
                        _ei.AddEventHandler(_dataSource, _pceh);
                    }
                }

                if (_valueMember != null && _valueMember.Length > 0)
                {
                    SetRadioButtonBinding();
                }

                _dataSource = value;
            }

        }

        /// <summary>
        /// Set up the binding to the property.
        /// </summary>
        private void SetRadioButtonBinding()
        {
            if (_dataSource == null)
            {
                throw new InvalidOperationException("m_objDataSource is null. This shouldn't happen here.");
            }

            _pi = _dataSource.GetType().GetProperty(_valueMember);
            if (_pi == null)
            {
                throw new ArgumentException("Could not find " + _valueMember + " on binding object " + _dataSource.GetType().Name);
            }

            SetRadioButtonValue();
        }

        private void SetRadioButtonValue()
        {
            if (_pi == null)
            {
                throw new InvalidOperationException("_pi cannot be null.");
            }

            object o = _pi.GetValue(_dataSource, null);

            int i = 0;
            string strInt = ((int)o).ToString();

            if (Int32.TryParse(strInt, out i))
            {
                //!TODO: I'm not exactly thrilled about using Tag. There ought to be an easier, faster, more intuitive way to do this. 
                // Until I figure it out, though, Tag will do.
                foreach (Control c in Controls)
                {
                    if (c is RadioButton)
                    {
                        int nSetting;
                        if (c.Tag == null)
                        {
                            throw new InvalidOperationException("RadioButton " + c.Name +
                                " does not have its Tag property set to a valid enum integer value.");
                        }

                        if (!Int32.TryParse(c.Tag.ToString(), out nSetting))
                        {
                            throw new InvalidOperationException("RadioButton " + c.Name +
                                " does not have its Tag property set to a valid enum integr value.");
                        }

                        if (nSetting == i)
                        {
                            RadioButton rb = (RadioButton)c;
                            rb.Checked = true;
                        }
                    }
                }
            }
        }

        void rb_CheckedChanged(object sender, EventArgs e)
        {
            if (_dataSource != null)
            {
                int nSetting = 0;

                if (sender is RadioButton)
                {
                    RadioButton rbSender = (RadioButton)sender;

                    // Fire the RadioButtonChanged event.
                    FireRadioSelectionChanged(rbSender);

                    if (rbSender.Tag == null)
                    {
                        throw new InvalidOperationException("RadioButton " + rbSender.Name +
                            " does not have its Tag property set to a valid enum integer value.");
                    }

                    if (!Int32.TryParse(rbSender.Tag.ToString(), out nSetting))
                    {
                        throw new InvalidOperationException("RadioButton " + rbSender.Name +
                            " does not have its Tag property set to a valid enum integr value.");
                    }

                    PropertyInfo pi = (PropertyInfo)_dataSource.GetType().GetProperty(_valueMember);
                    if (pi != null)
                    {
                        // Convert the int into its corresponding enum. pi.PropertyType represents the enum's type.
                        object parsedEnum;
                        try
                        {
                            parsedEnum = Enum.Parse(pi.PropertyType, nSetting.ToString());
                        }
                        catch (Exception ex)
                        {
                            throw new InvalidOperationException("Could not convert RadioButton.Tag value into an enum.", ex);
                        }

                        // Stop listening to property changes while we change the property - otherwise, stack overflow.
                        _processPropertyChange = false;

                        pi.SetValue(_dataSource, parsedEnum, null);

                        _processPropertyChange = true;
                    }
                }
            }
        }

        protected void FireRadioSelectionChanged(RadioButton rbSender)
        {
            if (RadioSelectionChanged != null)
            {
                RadioSelectionChanged(rbSender, new RadioSelectionChangedEventArgs(rbSender));
            }
        }

        // Handle PropertyChanged notifications from the source.
        protected void radioButtonPanel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_processPropertyChange)
            {
                if (e.PropertyName.Equals(_valueMember))
                {
                    SetRadioButtonValue();
                }
            }
        }
    }

    /// <summary>
    /// An EventArgs class for radio button selection changed events. 
    /// </summary>
    public class RadioSelectionChangedEventArgs : EventArgs
    {
        private RadioButton m_rbButtonClicked;

        public RadioSelectionChangedEventArgs(RadioButton rb)
        {
            m_rbButtonClicked = rb;
        }

        public RadioButton RadioButtonClicked
        {
            get
            {
                return m_rbButtonClicked;
            }
        }
    }

    public delegate void RadioSelectionChangedEventHandler(object sender, RadioSelectionChangedEventArgs e);
}