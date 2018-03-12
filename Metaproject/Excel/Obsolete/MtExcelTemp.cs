
/*
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Drawing;
using System.IO;
using System.Globalization;


using Excel;


namespace MtExcel
  
 * 
{
      
        public void DrawGrid(Range r, XlBorderWeight BorderOut, XlBorderWeight BorderIn)
        {
            try
            {
                r.Borders.get_Item(XlBordersIndex.xlEdgeBottom).Weight = BorderOut;
                r.Borders.get_Item(XlBordersIndex.xlEdgeTop).Weight = BorderOut;
                r.Borders.get_Item(XlBordersIndex.xlEdgeLeft).Weight = BorderOut;
                r.Borders.get_Item(XlBordersIndex.xlEdgeRight).Weight = BorderOut;

    
  
  
  

                r.Borders.get_Item(XlBordersIndex.xlInsideHorizontal).Weight = BorderIn;
                r.Borders.get_Item(XlBordersIndex.xlInsideVertical).Weight = BorderIn;

                r = null;
            }
            catch
            {
            }

        }
        
        public void DrawGrid(int r1, int c1, int r2, int c2, XlBorderWeight BorderOut, XlBorderWeight BorderIn)
        {
            Range r = GetRange(r1, c1, r2, c2);

            DrawGrid(r, BorderOut, BorderIn);

        }

        public void DrawBorder(int r1, int c1, int r2, int c2, XlBorderWeight BorderOut)
        {
            Range r = GetRange(r1, c1, r2, c2);

            DrawBorder(r, BorderOut);

        }

        public void DrawBorder(int r1, int c1, int r2, int c2, XlBorderWeight BorderOut, bool isMerged)
        {
            Range r = GetRange(r1, c1, r2, c2);

            DrawBorder(r, BorderOut);

            r.Merge(Missing.Value);

        }
        
        public void DrawBorder(Range r, XlBorderWeight BorderOut)
        {
            r.Borders.get_Item(XlBordersIndex.xlEdgeBottom).Weight = BorderOut;
            r.Borders.get_Item(XlBordersIndex.xlEdgeTop).Weight = BorderOut;
            r.Borders.get_Item(XlBordersIndex.xlEdgeLeft).Weight = BorderOut;
            r.Borders.get_Item(XlBordersIndex.xlEdgeRight).Weight = BorderOut;

        }

        public void DrawLineHorizontal(int r, int c1, int c2, XlBorderWeight BorderOfLine)
        {

            Range rng = GetRange(r, c1, r, c2);
            rng.Borders.get_Item(XlBordersIndex.xlEdgeBottom).Weight = BorderOfLine;
        }

        public void DrawLineVertical(int c, int r1, int r2, XlBorderWeight BorderOfLine)
        {
            Range r = GetRange(r1, c, r2, c);
            r.Borders.get_Item(XlBordersIndex.xlEdgeRight).Weight = BorderOfLine;

        }

       
        public void PutFormula(int r, int c, string formula)
        {
            Range cell = GetRange(r, c, r, c);
            cell.FormulaLocal = "=" + formula;

        }

        public void PutSum(int targetR, int targetC, int r1, int c1, int r2, int c2)
        {

            string cell_1 = CellSymbol(r1, c1);
            string cell_2 = CellSymbol(r2, c2);

            Range targetCell = GetRange(targetR, targetC, targetR, targetC);

            string formula = "=SUMA(" + cell_1 + ":" + cell_2 + ")";

            targetCell.FormulaLocal = formula;

        }

       

       


        
     
              



     

      

       

        

       

        


        

       
        

           


       
       
        #endregion 

        #region <Data>
        Application Excel = null;
        Workbook Wrkbook;
        Worksheet sheet;
        #endregion

    }//ExcelGenerator

}
*/