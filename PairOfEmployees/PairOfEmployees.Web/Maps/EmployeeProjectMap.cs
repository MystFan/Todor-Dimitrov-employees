using CsvHelper.Configuration;
using PairOfEmployees.Web.Models;

namespace PairOfEmployees.Web.Maps
{
    public sealed class EmployeeProjectMap : ClassMap<EmployeeProject>
    {
        public EmployeeProjectMap()
        {
            Map(m => m.EmployeeId).Name("EmpID");
            Map(m => m.ProjectId).Name("ProjectID");

            Map(m => m.DateTo)
                .TypeConverterOption.Format(DateFormats);

            Map(m => m.DateFrom)
                .TypeConverterOption.Format(DateFormats);
        }

        private readonly string[] DateFormats = new[]
        {
                // ISO / standard
                "yyyy-MM-dd",
                "yyyy/MM/dd",
                "yyyyMMdd",

                // Day-Month-Year
                "dd-MM-yyyy",
                "dd/MM/yyyy",
                "dd.MM.yyyy",
                "dd-MM-yy",
                "dd/MM/yy",
                "dd.MM.yy",

                // Month-Day-Year
                "MM-dd-yyyy",
                "MM/dd/yyyy",
                "MM.dd.yyyy",
                "MM-dd-yy",
                "MM/dd/yy",
                "MM.dd.yy",

                // Year-Month-Day
                "yyyy-MM-dd",
                "yyyy/MM/dd",
                "yyyy.MM.dd",

                // With time
                "yyyy-MM-dd HH:mm:ss",
                "yyyy-MM-dd HH:mm",
                "dd-MM-yyyy HH:mm:ss",
                "dd-MM-yyyy HH:mm",
                "dd/MM/yyyy HH:mm:ss",
                "dd/MM/yyyy HH:mm",

                // With milliseconds
                "yyyy-MM-dd HH:mm:ss.fff",
                "yyyy-MM-ddTHH:mm:ss",
                "yyyy-MM-ddTHH:mm:ss.fff",

                // ISO 8601
                "yyyy-MM-ddTHH:mm:ssK",
                "yyyy-MM-ddTHH:mm:ss.fffK",
                "yyyy-MM-ddTHH:mm:ssZ",
                "yyyy-MM-ddTHH:mm:ss.fffZ",

                // Month names
                "dd MMM yyyy",
                "dd MMMM yyyy",
                "MMM dd yyyy",
                "MMMM dd yyyy",

                // Month names with comma
                "MMM dd, yyyy",
                "MMMM dd, yyyy",

                // Common US formats
                "M/d/yyyy",
                "M/d/yy",
                "MM/d/yyyy",
                "M/dd/yyyy",

                // Common European formats
                "d/M/yyyy",
                "d/M/yy",
                "dd/M/yyyy",
                "d/MM/yyyy"
        };
    }
}
