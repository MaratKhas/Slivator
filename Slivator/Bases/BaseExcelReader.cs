using CsvHelper;
using CsvHelper.Configuration;

namespace Slivator.Bases
{
    public class BaseExcelReader<T> where T : new()
    {
        /// <summary>
        /// Путь к файлу
        /// </summary>
        private readonly string _filePath;

        public BaseExcelReader(string filePath)
        {
            _filePath = filePath;
        }

        public List<T> Read()
        {
            var result = new List<T>();

            if (!File.Exists(_filePath))
                throw new FileNotFoundException("Файл не найден", _filePath);

            using (var reader = new StreamReader(_filePath))
            {
                var config = new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                    HeaderValidated = null,
                    MissingFieldFound = null
                };

                using (var csv = new CsvReader(reader, config))
                {
                    var records = csv.GetRecords<T>();
                    result.AddRange(records);
                }
            }

            return result;
        }
    }
}