using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class Privi_Right_Product
    {
        private int _systypecode;
        public int systypecode
        {
            get { return _systypecode; }
            set { _systypecode = value; }
        }
        private int _rightcode = 0;
        public int rightcode
        {
            get { return _rightcode; }
            set { _rightcode = value; }
        }

    }
}
