using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoreHelpers
{
    public class UnicodeChar
    {
        public UnicodeChar(char character)
        {
            Character = character;
        }
        public char Character { get; set; }
        public int Code
        {
            get
            {
                return (int)Character;
            }
        }
        public string Direction
        {
            get
            {
                string dir = "";
                if ((Code >= 1570 && Code <= 1608) || (Code >= 1632 && Code <= 1641) || ((new int[] { 1662, 1670, 1688, 1705, 1711, 1740, 1734, 1749, 1700, 1717, 1685 }).Contains(Code)))
                {
                    dir = "rtl";
                }
                else
                {
                    dir = "ltr";
                }
                return dir;
            }
        }
    }
}