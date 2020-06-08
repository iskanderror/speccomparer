using System;
using System.Collections.Generic;
using System.Text;

namespace Comparator.Spec
{
    public class SpecFileParameters
    {
        public string Worksheet { get; set; }
        public int FirstRow { get; set; }
        public int RowsMaxCount { get; set; }
        public int VendorCodeColumn { get; set; }

        public int NameColumn { get; set; }
        public int UnitColumn { get; set; }
        public int QuantityColumn { get; set; }
        public int PositionColumn { get; set; }

        public SpecFileParameters()
        {
            Worksheet = "";
            FirstRow = 2;
            RowsMaxCount = 999;
            VendorCodeColumn = 2;
            NameColumn = 3;
            UnitColumn = 4;
            QuantityColumn = 5;
            PositionColumn = 1;
        }
    }
}
