using System;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;


// MtTextBox wykorzystuje wyrazenia regularne.
// Wprowadzony tekst porownywany jest z wyrazeniem regularnym, jezeli
// typ wprowadzanej danej jest Text.
// Przyklady wyrazen regularnych:
// * adres e-mail
// ([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$
// * zbrojenie belki
// ^([1-3][0-9]|[1-9])#(8|10|12|16|20|25|32)$
// * zbrojenie płyty
// ^#(8|10|12|16|20|25|32)co([1-9]|[1-9][0-9]|[1-9].[0-9]|[1-9][0-9].[0-9])$
// * wraz z z prętami naprzemiennymi
// ^#(8|8/10|10|10/12|12|12/16|16|16/20|20|20/25|25|25/32|32)co([1-9]|[1-9][0-9]|[1-9].[0-9]|[1-9][0-9].[0-9])$
// * pierwszy znak to litera a opcjonalne nastepne litery lub spacje
// ^[a-zA-Z]([a-zA-Z ]+)?$


namespace Metaproject.Controls
{

    /// <summary>
    /// New text box with external validation
    /// </summary>
    public class MtValidatedTextBox : TextBox
    {


        #region <enums>



        public enum TextBoxErrorType
        {
            cantParse,
            belowMin,
            aboveMax,
            notMatchRegEx
        }

        #endregion

        #region <members>

        //protected string _oldValue;
        //protected bool _isProperValue;
        //protected bool _isTextInserted;
        //protected bool _isLeaving;
        //protected bool _isEdited;
       
        //protected Font _defaultFont;
        //protected Font _boldFont;
        //protected MtTextBoxValidator.TextBoxType _tbType;
     
        protected MtTextBoxValidator _validator;


        #endregion

        #region <constructor>
        public MtValidatedTextBox()
        {
            _validator = new MtTextBoxValidator(this);
        }


        #endregion

        #region <public methods>

        public double GetValueAsDouble()
        {
            string modifiedtext = this.Text.Replace(',', '$');

            double val = double.NaN;
            try
            {
                val = Convert.ToDouble(modifiedtext);
            }
            catch
            {

            }

            return val;
        }

        public void AddTestingMethod(MtTextBoxValidator.DelegateTest testingMethod)
        {
            _validator.ValidatorData.TestingMethod += testingMethod;
        }



        #endregion

        #region <properties>
      
        [Category("User")]
        public double Min
        {
            get
            { return _validator.ValidatorData.Min; }
            set
            { _validator.ValidatorData.Min = value; }
        }

        [Category("User")]
        public double Max
        {
            get
            { return _validator.ValidatorData.Max; }

            set
            { _validator.ValidatorData.Max = value; }

        }

        [Category("User")]
        public MtTextBoxValidator.TextBoxType BoxType
        {
            get
            { return _validator.ValidatorData.BoxType; }

            set
            { _validator.ValidatorData.BoxType = value; }

        }

        [Category("User")]
        public string RegularExpression
        {
            get { return _validator.ValidatorData.RegEx; }
            set { _validator.ValidatorData.RegEx = value; }
        }
       

        [Browsable(false)]
        public bool IsProperValue
        {
            get { return _validator.IsProperValue; }
        }

        [Browsable(false)]
        public bool IsEdited
        {
            get { return _validator.IsEdited; }
        }
       

        #endregion

    }

    /// <summary>
    /// Validator for DataGridView
    /// </summary>
    public class MtGridTextBoxValidator
    {

        #region <nested types>
        
        class Rule
        {

            public enum RuleType
            {
                Unknown, Column, Row, Cell
            }

            public RuleType TypeOfRule { get; set; }
            public int Value1 {get; set; }
            public int Value2 { get; set; }
            public MtTextBoxValidator.Data ValidatorData { get; set; }            

        }

        class Rules : List<Rule>
        {
        

            public Rule GetRule(int columnIndex, int rowIndex)
            {

                foreach (Rule r in this)
                {

                    if (r.TypeOfRule == Rule.RuleType.Column)
                    {
                        if (columnIndex >= r.Value1 && columnIndex <= r.Value2)
                            return r;

                    }
                    else if (r.TypeOfRule == Rule.RuleType.Row)
                    {
                        if (rowIndex >= r.Value1 && rowIndex <= r.Value2)
                            return r;

                    }
                    else if (r.TypeOfRule == Rule.RuleType.Cell)
                    {
                        if (columnIndex == r.Value1 && rowIndex == r.Value2)
                            return r;
                    }

                   
                }

                return null;



            }

        
        }

        class Cell
        {

            public Cell()
            { }

            public Cell(int column, int row)
            {
                ColumnIndex = column;
                RowIndex = row;
            }

            public int ColumnIndex { get; set; }
            public int RowIndex { get; set; }

        }

        class Cells : List<Cell>
        {

            public bool IsCell(int column, int row)
            {
                foreach (Cell c in this)
                {
                    if (c.ColumnIndex == column && c.RowIndex == row)
                        return true;
                }

                return false;

            }



        }

        #endregion

        #region <fields>
        
        Cells _cells = new Cells();
        
        Cell _currentCell = new Cell();

        DataGridView _grid;

        Rules _rules;

        TextBox _textBox;

        MtTextBoxValidator _validator;


        
        #endregion

        #region <constructor>

        public MtGridTextBoxValidator(DataGridView grid)
        {
            _grid = grid;
            _grid.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(OnGridEditingControlShowing);

            _rules = new Rules();
        }

        #endregion

        #region <event handlers>

        void OnGridEditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {

            DataGridViewCell current = _grid.CurrentCell;
            Rule rule = _rules.GetRule(current.ColumnIndex, current.RowIndex);
            if (null == rule)
            {
                if (null != _validator)
                {

                    if (_validator.IsEventsAttached)
                    {
                        _validator.EventsDetach();
                        EventsDetach();
                       
                    }
                }

                return;
            }

   
          
            _textBox = e.Control as TextBox;

            if (null == _validator)
            {
                _validator = new MtTextBoxValidator(_textBox, rule.ValidatorData);
                EventsAttach();
            }
            else
            {
                _validator.ValidatorData = (MtTextBoxValidator.Data)rule.ValidatorData.Clone();

                if (!_validator.IsEventsAttached)
                {
                    _validator.EventsAttach();
                    EventsAttach();
                }            
            }

            
        
        }

        void EventsAttach()
        {
            _textBox.Leave += new EventHandler(textBox_Leave);
            _textBox.Enter += new EventHandler(textBox_Enter);
        }

        void EventsDetach()
        {
            _textBox.Leave -= new EventHandler(textBox_Leave);
            _textBox.Enter -= new EventHandler(textBox_Enter);
        }

        void textBox_Enter(object sender, EventArgs e)
        {
            DataGridViewCell current = _grid.CurrentCell;
            _currentCell.ColumnIndex = current.ColumnIndex;
            _currentCell.RowIndex = current.RowIndex;
        }

        void textBox_Leave(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            _grid[_currentCell.ColumnIndex, _currentCell.RowIndex].Value = tb.Text;
        }

        #endregion

        #region <public methods>

        public void AddColumnRule(int firstColumnIndex, int lastColumnIndex, MtTextBoxValidator.Data validatorData)
        {
            Rule r = new Rule();
            r.TypeOfRule = Rule.RuleType.Column;
            r.Value1 = firstColumnIndex;
            r.Value2 = lastColumnIndex;
            r.ValidatorData = (MtTextBoxValidator.Data)validatorData.Clone();
            _rules.Add(r);
        }

        public void AddColumnRule(int columnIndex, MtTextBoxValidator.Data validatorData)
        {
            AddColumnRule(columnIndex, columnIndex, validatorData);
        }

        public void AddRowRule(int firstRowIndex, int lastRowIndex, MtTextBoxValidator.Data validatorData)
        {
            Rule r = new Rule();
            r.TypeOfRule = Rule.RuleType.Row;
            r.Value1 = firstRowIndex;
            r.Value2 = lastRowIndex;
            r.ValidatorData = (MtTextBoxValidator.Data)validatorData.Clone();
            _rules.Add(r);
        }

        public void AddRowRule(int rowIndex, MtTextBoxValidator.Data validatorData)
        {
            AddRowRule(rowIndex, rowIndex, validatorData);
        }

        public void AddCellRule(int columnIndex, int rowIndex, MtTextBoxValidator.Data validatorData)
        {
            Rule r = new Rule();
            r.TypeOfRule = Rule.RuleType.Cell;
            r.Value1 = columnIndex;
            r.Value2 = rowIndex;
            r.ValidatorData = (MtTextBoxValidator.Data)validatorData.Clone();
            _rules.Add(r);
        }

        #endregion


    }

    /// <summary>
    /// External validation
    /// </summary>
    public class MtTextBoxValidator
    {

        #region <delegates, events>
        public delegate bool DelegateTest(string expression);
        
        #endregion

        #region <nested types>
        public class Data : ICloneable
        {

            #region <static methods>
            public static Data CreateDataForTexts(string regEx)
            {
                return new Data(TextBoxType.Text, double.NaN, double.NaN, regEx);
            }

            public static Data CreateDataForDoubles(double min, double max)
            {
                return new Data(TextBoxType.Double, min, max, null);
            }

            public static Data CreateDataForIntegers(int min, int max)
            {
                return new Data(TextBoxType.Integer, min, max, null);
            }
            #endregion

            #region <contructors>
            public Data()
            { }

            Data(TextBoxType type, double min, double max, string regEx)
            {
                BoxType = type;
                Min = min;
                Max = max;
                RegEx = regEx;
            }
            #endregion

            #region <properties>
            public double Min {get; set;}
            public double Max {get; set;}
            public string RegEx {get; set;}
            public TextBoxType BoxType {get; set; }
            #endregion

            #region <events>
            public DelegateTest TestingMethod;
            #endregion

            #region ICloneable Members

            public object Clone()
            {
                Data data = new Data();
                data.Min = Min;
                data.Max = Max;
                data.RegEx = RegEx;
                data.BoxType = BoxType;
                data.TestingMethod += TestingMethod;

                return (Data)data;
            }

            #endregion
        
        

        }
        #endregion

        #region <members>
        TextBox _textBox;
        protected string _oldValue;
        protected bool _isProperValue;
        protected bool _isTextInserted;
        protected bool _isLeaving;
        protected bool _isEdited;
        TextBoxErrorType _error;
        double _min;
        double _max;

        protected Font _defaultFont;
        protected Font _boldFont;
        protected TextBoxType _tbType;
        string _regExp;
        Data _data;
        #endregion

        #region <enums>
        public enum TextBoxType
        {
            Double,
            Integer,
            Text
        }

        public enum TextBoxErrorType
        {
            cantParse,
            belowMin,
            aboveMax,
            notMatchRegEx
        }
        #endregion

        #region <constructor>
        public MtTextBoxValidator(TextBox textBox)
        {

            _data = new Data();

            _textBox = textBox;
            _textBox.BackColor = Color.White;

            EventsAttach();

            _defaultFont = _textBox.Font;
            _boldFont = new Font(_defaultFont, FontStyle.Bold);

          

        }

        public MtTextBoxValidator(TextBox textBox, Data validatorData)
            : this(textBox)
        {
            _data = (Data)validatorData.Clone();
        }



        #endregion

        #region <events>

        void OnEnter(object sender, EventArgs e)
        {
            _isEdited = true;
            _textBox.BackColor = Color.LightBlue;
            _textBox.Font = _boldFont;
            _oldValue = _textBox.Text;
            _isLeaving = false;

        }

        void OnLeave(object sender, EventArgs e)
        {
            _textBox.BackColor = Color.White;
            _textBox.Font = _defaultFont;
            if (!_isProperValue)
            {
                _isLeaving = true;

                if (_error == TextBoxErrorType.belowMin)
                    _textBox.Text = _data.Min.ToString();
                else if (_error == TextBoxErrorType.aboveMax)
                    _textBox.Text = _data.Max.ToString();
                else
                    _textBox.Text = _oldValue;

            }

            _isEdited = false;

        }

        void OnTextChanged(object sender, EventArgs e)
        {
            if (!_isTextInserted)
            {
                _isTextInserted = true;
                return;
            }

            Test();
            SetTextBox();


        }



        #endregion

        #region <private methods>
        protected void SetTextBox()
        {
            if (_isLeaving) return;

            if (!_isEdited)
            {
                _textBox.BackColor = Color.White;
                return;
            }

            if (_isProperValue)
                _textBox.BackColor = Color.LightBlue;
            else
                _textBox.BackColor = Color.Red;

        }

        protected void Test()
        {

            if (_data.BoxType == TextBoxType.Text)
            {
                if (null != ValidatorData.TestingMethod)
                {
                    _isProperValue = ValidatorData.TestingMethod(_textBox.Text);
                }
                else
                    _isProperValue = isMatchForRegularExpresion();
               
                if (_isProperValue == false) _error = TextBoxErrorType.notMatchRegEx;
                return;
            }



            double testValue = double.NaN;

            try
            {
                if (_data.BoxType == TextBoxType.Double)
                    testValue = GetValueAsDouble();

                if (_data.BoxType == TextBoxType.Integer)
                    testValue = CheckValueAsInt();

            }
            catch { }
            finally
            {
                if (double.IsNaN(testValue))
                {
                    _isProperValue = false;
                    _error = TextBoxErrorType.cantParse;
                }
                else
                {
                    _isProperValue = true;

                    // sprawdzenie zakresu
                    if (testValue < _data.Min)
                    {
                        _isProperValue = false;
                        _error = TextBoxErrorType.belowMin;
                    }

                    if (testValue > _data.Max)
                    {
                        _isProperValue = false;
                        _error = TextBoxErrorType.aboveMax;
                    }
                }

            }
        }

        public double CheckValueAsInt()
        {
            string modifiedText = _textBox.Text.Replace(',', '$');
            modifiedText = modifiedText.Replace('.', '$');
            double val = double.NaN;
            val = (double)Convert.ToInt32(_textBox.Text);
            return val;
        }

        public bool isMatchForRegularExpresion()
        {
            if (_data.RegEx == null) return true;

            bool isMatch = Regex.IsMatch(_textBox.Text, _data.RegEx);

            return isMatch;
        }

        #endregion

        #region <public methods>

        public void EventsAttach()
        {
            
            _textBox.Enter += new EventHandler(OnEnter);
            _textBox.Leave += new EventHandler(OnLeave);
            _textBox.TextChanged += new EventHandler(OnTextChanged);
            IsEventsAttached = true;
        
        }

        public void EventsDetach()
        {
            _textBox.Enter -= new EventHandler(OnEnter);
            _textBox.Leave -= new EventHandler(OnLeave);
            _textBox.TextChanged -= new EventHandler(OnTextChanged);
            IsEventsAttached = false;
        }

        public double GetValueAsDouble()
        {
            string modifiedtext = _textBox.Text.Replace(',', '$');

            double val = double.NaN;
            try
            {
                val = Convert.ToDouble(modifiedtext);
            }
            catch
            {

            }

            return val;
        }

        #endregion

        #region <properties>

        public Data ValidatorData
        {
            get { return _data; }
            set { _data = value; }
        }

        public bool IsProperValue
        {
            get { return _isProperValue; }
        }

        public bool IsEdited
        {
            get { return _isEdited; }
        }

        public bool IsEventsAttached { get; private set; }

        


        #endregion


    }

    /// <summary>
    /// Good old text box with inner validation
    /// </summary>
    public class MtTextBox : TextBox
    {

        #region <delegates>

      


        #endregion

        #region <enums>

        public enum TextBoxType
        {
            Double,
            Integer,
            Text
        }

        public enum TextBoxErrorType
        {
            cantParse,
            belowMin,
            aboveMax,
            notMatchRegEx
        }

        #endregion
        
        #region <members>

        protected string _oldValue;
        protected bool _isProperValue;
        protected bool _isTextInserted;
        protected bool _isLeaving;
        protected bool _isEdited;
        TextBoxErrorType _error;
        double _min;
        double _max;

        protected Font _defaultFont;
        protected Font _boldFont;
        protected TextBoxType _tbType;
        string _regExp;

        
        #endregion

        #region <constructor>
        public MtTextBox()
        {
           
            this.BackColor = Color.White;
            this.Enter += new EventHandler(OnEnter);
            this.Leave += new EventHandler(OnLeave);
            this.TextChanged += new EventHandler(OnTextChanged);
            
            _defaultFont = this.Font;
            _boldFont = new Font(_defaultFont, FontStyle.Bold);
                                  
        }

       
        #endregion

        #region <events>

        void OnEnter(object sender, EventArgs e)
        {
         
            
            _isEdited = true;
            this.BackColor = Color.LightBlue;
            this.Font = _boldFont;
            _oldValue = this.Text;
            _isLeaving = false;

           
           
        }

        void OnLeave(object sender, EventArgs e)
        {
          
            
            this.BackColor = Color.White;
            this.Font = _defaultFont;
            if (!_isProperValue)
            {
                _isLeaving = true;

                if (_error == TextBoxErrorType.belowMin)
                    this.Text = Min.ToString();
                else if (_error == TextBoxErrorType.aboveMax)
                    this.Text = Max.ToString();
                else
                    this.Text = _oldValue;

            }

            _isEdited = false;

           
            
        }

        void OnTextChanged(object sender, EventArgs e)
        {
            
            
            if (!_isTextInserted)
            {
                _isTextInserted = true;
                return;
            }

            Test();
            SetTextBox();

            
        }



        #endregion

        #region <private methods>
        protected void SetTextBox()
        {
            if (_isLeaving) return;

            if (!_isEdited)
            {
                this.BackColor = Color.White;
                return;
            }

            if (_isProperValue)
                this.BackColor = Color.LightBlue;
            else
                this.BackColor = Color.Red;

        }

        protected void Test()
        {

            if (BoxType == TextBoxType.Text)
            {
                _isProperValue = isMatchForRegularExpresion();
                if (_isProperValue == false) _error = TextBoxErrorType.notMatchRegEx;
                return;
            }



            double testValue = double.NaN;

            try
            {
                if (BoxType == TextBoxType.Double)
                    testValue = GetValueAsDouble();

                if (BoxType == TextBoxType.Integer)
                    testValue = CheckValueAsInt();
                
            }
            catch { }
            finally
            {
                if (double.IsNaN(testValue))
                {
                    _isProperValue = false;
                    _error = TextBoxErrorType.cantParse;
                }
                else
                {
                    _isProperValue = true;

                    // sprawdzenie zakresu
                    if (testValue < Min) 
                    {
                        _isProperValue = false;
                        _error = TextBoxErrorType.belowMin;
                    }
                        
                    if (testValue > Max)
                    {
                        _isProperValue = false;
                        _error = TextBoxErrorType.aboveMax;
                    }
                }

            }
        }

        public double CheckValueAsInt()
        {
            string modifiedText = this.Text.Replace(',', '$');
            modifiedText = modifiedText.Replace('.', '$');
            double val = double.NaN;
            if (string.IsNullOrEmpty(modifiedText)) return val;
            val = (double)Convert.ToInt32(this.Text);
            return val;
        }

        public bool isMatchForRegularExpresion()
        {
            if (RegularExpression == null) return true;

            return Regex.IsMatch(this.Text, RegularExpression);
        }

        #endregion

        #region <public methods>

        public double GetValueAsDouble()
        {
            string modifiedtext = this.Text.Replace(',', '$');

            double val = double.NaN;
            if (string.IsNullOrEmpty(modifiedtext)) return val ;

           
            try
            {
                
                val = Convert.ToDouble(modifiedtext);
            }
            catch
            {
             
            }

            return val;
        }

   
        

        #endregion

        #region <properties>
        [Category("User")]
        public double Min
        {
            get
            { return _min; }
            set
            { _min = value; }
        }

        [Category("User")]
        public double Max
        {
            get
            { return _max; }

            set
            { _max = value; }

        }

        [Category("User")]
        public TextBoxType BoxType
        {
            get
            { return _tbType; }

            set
            { _tbType = value; }

        }

        [Category("User")]
        public string RegularExpression
        {
            get { return _regExp; }
            set { _regExp = value; }
        }


        public bool IsProperValue
        {
            get { return _isProperValue; }
        }

        public bool IsEdited
        {
            get { return _isEdited; }
        }





        #endregion

    }


              
}









