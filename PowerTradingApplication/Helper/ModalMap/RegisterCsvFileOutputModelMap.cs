using CsvHelper.Configuration;
using PowerTradingApplication.Modal;

namespace PowerTradingApplication.Helper.ModalMap
{
    public class RegisterCsvFileOutputModelMap : ClassMap<CsvFileOutputModel>
    {
        public RegisterCsvFileOutputModelMap()
        {

            Map(r => r.LocalTime).Name("Local Time");
            Map(r => r.Volume).Name("Volume");

        }
    }
}
