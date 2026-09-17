using CsvHelper;
using CsvHelper.Configuration;
using PairOfEmployees.Web.Maps;
using PairOfEmployees.Web.Models;
using System.Globalization;

namespace PairOfEmployees.Web.Services
{
    public class EmplyeePairParser : IEmplyeePairParser
    {
        public EmployeeProject[] GetEmployeeProjects(IFormFile file)
        {
            using var reader = new StreamReader(file.OpenReadStream());

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                TrimOptions = TrimOptions.Trim
            };

            using var csv = new CsvReader(reader, config);

            csv.Context.RegisterClassMap<EmployeeProjectMap>();
            csv.Context.TypeConverterOptionsCache.GetOptions<DateTime?>().NullValues.Add("NULL");

            var records = csv.GetRecords<EmployeeProject>().ToArray();
            return records;
        }
    }
}
