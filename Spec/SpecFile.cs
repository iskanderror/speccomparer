/*************************************************************************************************
  Required Notice: Copyright (C) EPPlus Software AB. 
  This software is licensed under PolyForm Noncommercial License 1.0.0 
  and may only be used for noncommercial purposes 
  https://polyformproject.org/licenses/noncommercial/1.0.0/

  A commercial license to use this software can be purchased at https://epplussoftware.com
 *************************************************************************************************
  Date               Author                       Change
 *************************************************************************************************
  01/27/2020         EPPlus Software AB           Initial release EPPlus 5
 *************************************************************************************************/

using Comparator.Extensions;
using Comparator.Logger;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Comparator.Spec
{
    public class SpecFile: NotifiedEntity
    {
        private readonly ILogger _logger;

        private SpecFileParameters parameters;
        private string file;
        private bool isLoaded;

        public List<SpecItem> Items { get; private set; }

        public SpecFileParameters Parameters { get => parameters; set => SetProperty(ref parameters, value); }
        public string File {get => file;  set => SetProperty(ref file, value); }
        public bool IsLoaded { get => isLoaded; set => SetProperty(ref isLoaded, value); }

        public SpecFile(ILogger logger = null)
        {
            File = "";
            Parameters = new SpecFileParameters();
            IsLoaded = false;
            Items = new List<SpecItem>();
            _logger = logger;
        }

        public void LoadItems()
        {
            Items.Clear();
            IsLoaded = false;
            try
            {
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                FileInfo fileInfo = new FileInfo(File);
                using (ExcelPackage package = new ExcelPackage(fileInfo))
                {
                    ExcelWorksheet sheet = package.Workbook.Worksheets[parameters.Worksheet];
                    if (sheet is null)
                    {
                        throw new FileLoadException(String.Format("Не удалось загрузить лист {0} из файла {1} ", parameters.Worksheet, File));
                    }
                    _logger?.Log(String.Format("Читаю лист {0} из файла {1}", sheet.Name, File));

                    var query = (from cell in sheet.Cells[Parameters.FirstRow, Parameters.VendorCodeColumn, Parameters.RowsMaxCount, Parameters.VendorCodeColumn]
                                 where cell.Value is String && (String)cell.Value != ""
                                 select cell);


                    int nameOffset = Parameters.NameColumn - Parameters.VendorCodeColumn;
                    int unitOffset = Parameters.UnitColumn - Parameters.VendorCodeColumn;
                    int quantityOffset = Parameters.QuantityColumn - Parameters.VendorCodeColumn;
                    int positionOffset = Parameters.PositionColumn - Parameters.VendorCodeColumn;

                    foreach (var cell in query)
                    {
                        SpecItem tmpItem = new SpecItem
                        {
                            VendorCode = cell.Value?.ToString() ?? "",
                            Name = cell.Offset(0, nameOffset).Value?.ToString() ?? "",
                            Unit = cell.Offset(0, unitOffset).Value?.ToString() ?? "",
                            Quantity = float.Parse(cell.Offset(0, quantityOffset).Value?.ToString() ?? "0"),
                            Position = cell.Offset(0, positionOffset).Value?.ToString() ?? "",
                        };
                        Items.Add(tmpItem);
                    }
                    isLoaded = true;
                    _logger?.Log(String.Format("Чтение файла {0} завершено. Найдено {1} записей", sheet.Name, Items.Count));
                }
            }
            catch (Exception ex)
            {
                _logger?.Log(String.Format("Error: {0}", ex.Message));
            }
        }

        public void MergeDuplicates()
        {
            if (!isLoaded)
            {
                _logger?.Log(String.Format("Файл {0} не загружен. Нет данных для объединения позиций.", File));
                return;
            }

            _logger?.Log("Объединяю позиции с одинаковым артикулом... ");
            List<SpecItem> reducedOrderItemList = Items.GroupBy(item => item.VendorCode)
                .Select(grp => grp.Aggregate(
                    new SpecItem(grp.First())
                    {
                        Quantity = 0, 
                        Position = "",
                    },
                    mergeItems))
                .ToList();
            Items = reducedOrderItemList;
            _logger?.Log(String.Format("Объединение завершено. Обнаружено {0} уникальных позиций", Items.Count));
        }

        private SpecItem mergeItems(SpecItem curItem, SpecItem nextItem)
        {
            curItem.Quantity += nextItem.Quantity;
            char[] charsToTrim = { ' ', '|'};
            curItem.Position = String.Format("{0} | {1}", curItem.Position , nextItem.Position).Trim(charsToTrim);
            return curItem;
        }

    }
}
