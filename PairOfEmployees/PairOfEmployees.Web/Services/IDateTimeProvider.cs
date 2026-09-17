namespace PairOfEmployees.Web.Services
{
    public interface IDateTimeProvider
    {
        DateTime UtcNow { get; }
    }
}
