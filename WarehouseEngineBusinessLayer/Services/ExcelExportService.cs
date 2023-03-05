using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseEngineBusinessLayer.Interfaces;

namespace WarehouseEngineBusinessLayer.Services
{
    public class ExcelExportService : IExcelExportService
    {
        public byte[] ExportToExcel<T>(IEnumerable<T> data, string sheetName = "Sheet1")
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add(sheetName);
                var properties = typeof(T).GetProperties();

                var displayNames = properties.Select(prop => prop.GetCustomAttributes(typeof(DisplayNameAttribute), false)
                                  .Cast<DisplayNameAttribute>().FirstOrDefault()?.DisplayName ?? prop.Name).ToList();
                for (int i = 0; i < properties.Length; i++)
                {
                    worksheet.Cell(1, i + 1).Value = displayNames[i];
                }

                for (int i = 0; i < data.Count(); i++)
                {
                    var item = data.ElementAt(i);
                    for (int j = 0; j < properties.Length; j++)
                    {
                        worksheet.Cell(i + 2, j + 1).Value = properties[j].GetValue(item)?.ToString();
                    }
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }
    }
}
