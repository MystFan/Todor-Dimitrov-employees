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

        public static readonly string[] DateFormats = new[]
        {
            // ISO / standard
            "yyyy-MM-dd",
            "yyyy/MM/dd",
            "yyyy.MM.dd",
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
            // yyyy-MM-dd is already included above

            // Flexible day/month variants
            "d-M-yyyy",
            "d/M/yyyy",
            "d.M.yyyy",
            "d-M-yy",
            "d/M/yy",
            "d.M.yy",

            // Flexible month/day variants
            "M-d-yyyy",
            "M/d/yyyy",
            "M.d.yyyy",
            "M-d-yy",
            "M/d/yy",
            "M.d.yy",

            // With time
            "yyyy-MM-dd HH:mm:ss",
            "yyyy-MM-dd HH:mm",
            "dd-MM-yyyy HH:mm:ss",
            "dd-MM-yyyy HH:mm",
            "dd/MM/yyyy HH:mm:ss",
            "dd/MM/yyyy HH:mm",
            "MM-dd-yyyy HH:mm:ss",
            "MM-dd-yyyy HH:mm",
            "MM/dd/yyyy HH:mm:ss",
            "MM/dd/yyyy HH:mm",

            // With milliseconds
            "yyyy-MM-dd HH:mm:ss.fff",
            "yyyy-MM-dd HH:mm:ss.fffK",
            "yyyy-MM-ddTHH:mm:ss.fff",
            "yyyy-MM-ddTHH:mm:ss.fffK",

            // ISO 8601
            "yyyy-MM-ddTHH:mm:ss",
            "yyyy-MM-ddTHH:mm:ssK",
            "yyyy-MM-ddTHH:mm:ss.fff",
            "yyyy-MM-ddTHH:mm:ss.fffK",

            // ISO with timezone offsets
            "yyyy-MM-ddTHH:mm:sszzz",
            "yyyy-MM-ddTHH:mm:ss.fffzzz",
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

            // Month names + time
            "dd MMM yyyy HH:mm",
            "dd MMM yyyy HH:mm:ss",
            "dd MMMM yyyy HH:mm",
            "dd MMMM yyyy HH:mm:ss",
            "MMM dd, yyyy HH:mm",
            "MMM dd, yyyy HH:mm:ss",
            "MMMM dd, yyyy HH:mm",
            "MMMM dd, yyyy HH:mm:ss",

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
