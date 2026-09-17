using CsvHelper.Configuration.Attributes;

namespace PairOfEmployees.Web.Models
{
    public record EmployeeProject
    {
        public int EmployeeId { get; set; }

        public int ProjectId { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime? DateTo { get; set; }
    }
}
