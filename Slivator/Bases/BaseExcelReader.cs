using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Slivator.Bases
{
    public class DynamicExcelReaderEPPlus
    {
        public List<Dictionary<string, string>> ReadRows(string filePath)
        {
            var result = new List<Dictionary<string, string>>();

            if (!File.Exists(filePath))
                throw new FileNotFoundException("Файл не найден", filePath);

            // Устанавливаем контекст лицензии (для некоммерческого использования)
            //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage.License.SetNonCommercialPersonal("UserName");

            using var package = new ExcelPackage(new FileInfo(filePath));
            var worksheet = package.Workbook.Worksheets[0]; // Берем первый лист

            if (worksheet?.Dimension == null)
                return result;

            int rowCount = worksheet.Dimension.Rows;
            int colCount = worksheet.Dimension.Columns;

            // Читаем заголовки (первая строка)
            var headers = new List<string>();
            for (int col = 1; col <= colCount; col++)
            {
                var header = worksheet.Cells[1, col].Text?.Trim();
                if (string.IsNullOrEmpty(header))
                    header = $"Column{col}";
                headers.Add(header);
            }

            // Читаем данные (со второй строки)
            for (int row = 2; row <= rowCount; row++)
            {
                var rowData = new Dictionary<string, string>();
                bool hasData = false;

                for (int col = 1; col <= colCount; col++)
                {
                    var value = worksheet.Cells[row, col].Text;
                    if (!string.IsNullOrEmpty(value))
                        hasData = true;
                    rowData[headers[col - 1]] = value;
                }

                if (hasData)
                    result.Add(rowData);
            }

            return result;
        }
    }
}