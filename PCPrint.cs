//*****************************************************************************************
//                           LICENSE INFORMATION
//*****************************************************************************************
//   PCPrint Version 1.0.0.0
//   Class file for printing in VB.Net. Inherits from the PrintDocument Class, and includes
//   all its functionality
//
//   Copyright (C) 2008  
//   Richard L. McCutchen 
//   Email: psychocoder@dreamincode.net
//   Created: 25FEB08
//
//   This program is free software: you can redistribute it and/or modify
//   it under the terms of the GNU General Public License as published by
//   the Free Software Foundation, either version 3 of the License, or
//   (at your option) any later version.
//
//   This program is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//   GNU General Public License for more details.
//
//   You should have received a copy of the GNU General Public License
//   along with this program.  If not, see <http://www.gnu.org/licenses/>.
//*****************************************************************************************
using System;
using System.Drawing;
using System.Drawing.Printing;


namespace SICEpdv
{

    public class PCPrint : System.Drawing.Printing.PrintDocument
    {
        public int printHeight = 1006;
        public int printWidth = 300;
        public int leftMargin = 3;
        public int rightMargin = 5;
        public string Texto {get; set;}
        public Font Fonte { get; set; }
        private static int curChar;
        

       
        protected override void OnPrintPage(System.Drawing.Printing.PrintPageEventArgs e)
        {
            

            base.OnPrintPage(e);

           
            Int32 lines;
            Int32 chars;

           
            //Check if the user selected to print in Landscape mode
            //if they did then we need to swap height/width parameters
            if (base.DefaultPageSettings.Landscape)
            {
                int tmp;
                tmp = printHeight;
                printHeight = printWidth;
                printWidth = tmp;
            }

           
            RectangleF printArea = new RectangleF(leftMargin, rightMargin, printWidth, printHeight);

           
            StringFormat format = new StringFormat(StringFormatFlags.MeasureTrailingSpaces);

         
            e.Graphics.MeasureString(Texto.Substring(RemoveZeros(curChar)), Fonte, new SizeF(printWidth, printHeight), format, out chars, out lines);

         
            e.Graphics.DrawString(Texto.Substring(RemoveZeros(curChar)), Fonte, Brushes.Black, printArea, format);

          
            curChar += chars;

            if (curChar < Texto.Length)
            {
                e.HasMorePages = true;
            }
            else
            {
                e.HasMorePages = false;
                curChar = 0;
            }
        }

      
        public int RemoveZeros(int value)
        {
            //Check the value passed into the function,
            //if the value is a 0 (zero) then return a 1,
            //otherwise return the value passed in
            switch (value)
            {
                case 0:
                    return 1;
                default:
                    return value;
            }
        }
    
    }

}
