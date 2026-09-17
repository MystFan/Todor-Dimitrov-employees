namespace PairOfEmployees.Web.Models
{
    public record PairResult
    {
        public int FirstEmployeeId { get; set; }

        public int SecondEmployeeId { get; set; }

        public int ProjectId { get; set; }

        public long DaysWorked { get; set; }
    }
}
