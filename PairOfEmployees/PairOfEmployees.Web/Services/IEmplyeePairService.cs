using PairOfEmployees.Web.Models;

namespace PairOfEmployees.Web.Services
{
    public interface IEmplyeePairService
    {
        PairResult[] GetResult(IEnumerable<EmployeeProject> employees);
    }
}
