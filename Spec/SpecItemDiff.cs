using Comparator.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Comparator.Spec
{
    public class SpecItemDiff: NotifiedEntity
    {
        private string vendorCode;
        private string name;
        private string unit;
        private float quantity1;
        private float quantity2;
        private string position1;
        private string position2;
        public string VendorCode { get => vendorCode; set => SetProperty(ref vendorCode,value); }
        public string Name { get => name; set => SetProperty(ref name, value); }
        public string Unit { get => unit; set => SetProperty(ref unit, value); }
        public float Quantity1 { get => quantity1; set => SetProperty(ref quantity1, value); }
        public float Quantity2 { get => quantity2; set => SetProperty(ref quantity2, value); }
        public string Position1 { get => position1; set => SetProperty(ref position1, value); }
        public string Position2 { get => position2; set => SetProperty(ref position2, value); }

        public SpecItemDiff(SpecItem first, SpecItem second, bool isFirstEmpty = false)
        {
            VendorCode = isFirstEmpty ? second.VendorCode : first.VendorCode;
            Name = isFirstEmpty ? second.Name : first.Name;
            Unit = isFirstEmpty ? second.Unit : first.Unit;
            Quantity1 = first.Quantity;
            Quantity2 = second.Quantity;
            Position1 = first.Position;
            Position2 = second.Position;
        }
    }
}
