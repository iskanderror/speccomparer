using System;
using System.Collections.Generic;
using System.Text;

namespace Comparator.Spec
{
    public class SpecItem
    {
        public string VendorCode { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public float Quantity { get; set; }

        public SpecItem()
        {
            VendorCode = "";
            Name = "";
            Unit = "";
            Quantity = 0;
        }

        public SpecItem(SpecItem template)
        {
            VendorCode = template.VendorCode;
            Name = template.Name;
            Unit = template.Unit;
            Quantity = template.Quantity;
        }

        public override string ToString()
        {
            int NameLength = Math.Min(Name.Length, 15);
            return VendorCode.PadRight(15) + " " + Name.Substring(0, NameLength).PadRight(15) + "..." + "\t" + Quantity.ToString().PadRight(15);
        }
    }
}
