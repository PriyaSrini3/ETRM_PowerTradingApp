using CsvHelper;
using Microsoft.Extensions.Options;
using PowerTradingApplication.Modal;
using System.Globalization;

namespace PowerTradingApplication.Helper
{
    public static class CsvFileGenerator
    {
        public static void generateCSV(List<CsvFileOutputModel> outputs)
        {
            var filepath = !string.IsNullOrEmpty(ConfigurationSetup.myConfig.CsvPath) ? ConfigurationSetup.myConfig.CsvPath : Environment.CurrentDirectory;
            var csvPath = Path.Combine(filepath, $"PowerPosition_{DateTime.Now.ToString("yyyyMMdd")}_{DateTime.Now.ToString("HHmm")}.csv");
            using (var streamwriter = new StreamWriter(csvPath))
            {
                using (var csvWriter = new CsvWriter(streamwriter, CultureInfo.InvariantCulture))
                {
                    csvWriter.Context.RegisterClassMap<RegisterCsvFileOutputModelMap>();
                    csvWriter.WriteRecords(outputs);
                }

            }
        }
    }
}
