using Comparator.Extensions;
using Comparator.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Comparator.Spec
{
    public class SpecComparator
    {
        const string NOT_AVAILABLE_CODE = "{490270D1-C126-4175-9E5C-4C68BF2D72ED}";

        SpecFile firstSpec;
        SpecFile secondSpec;
        ILogger _logger;

        public List<SpecItem> Equalities;
        public List<SpecItemDiff> Differences;

        public SpecComparator(SpecFile first, SpecFile second, ILogger logger = null)
        {
            firstSpec = first;
            secondSpec = second;
            _logger = logger;
            Equalities = new List<SpecItem>();
            Differences = new List<SpecItemDiff>();
        }

        public void Compare()
        {
            Equalities.Clear();
            Differences.Clear();
            if(!firstSpec.IsLoaded || !secondSpec.IsLoaded)
            {
                _logger?.Log(String.Format("Внимание! Не все файлы спецификаций загружены. ({0}/{1})", firstSpec.IsLoaded, secondSpec.IsLoaded));
            }

            var result = firstSpec.Items.FullOuterJoin(secondSpec.Items,
                    a => a.VendorCode,
                    b => b.VendorCode,
                    (a, b, id) => new { a, b },
                    new SpecItem() { VendorCode = NOT_AVAILABLE_CODE },
                    new SpecItem() { VendorCode = NOT_AVAILABLE_CODE })
                    .ToList();

            foreach (var tmp in result)
            {
                if(tmp.a.Quantity == tmp.b.Quantity)
                {
                    Equalities.Add(tmp.a);
                }
                else
                {
                    bool isFirstEmpty = (tmp.a.VendorCode == NOT_AVAILABLE_CODE);
                    SpecItemDiff specItemDiff = new SpecItemDiff(tmp.a, tmp.b, isFirstEmpty);
                    Differences.Add(specItemDiff);
                }
            }
            _logger?.Log(String.Format("Сравнение завершено. Совпадающих позиций: {0}, различающихся позиций : {1})", Equalities.Count, Differences.Count));

        }
        

    }
}
