using PairOfEmployees.Web.Models;

namespace PairOfEmployees.Web.Services
{
    public interface IEmplyeePairParser
    {
        EmployeeProject[] GetEmployeeProjects(IFormFile file);
    }
}
