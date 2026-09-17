using System;
using System.Linq;
using PairOfEmployees.Web.Models;
using PairOfEmployees.Web.Services;
using Xunit;

namespace PairOfEmployees.Tests
{
    public class EmplyeePairServiceTests
    {
        private class FakeDateTimeProvider : IDateTimeProvider
        {
            public DateTime UtcNow { get; set; }
        }

        [Fact]
        public void GetResult_CalculatesOverlapCorrectly()
        {
            var provider = new FakeDateTimeProvider { UtcNow = new DateTime(2026, 9, 16) };
            var service = new EmplyeePairService(provider);

            var employees = new[]
            {
                new EmployeeProject { EmployeeId = 143, ProjectId = 12, DateFrom = new DateTime(2013,11,1), DateTo = new DateTime(2014,1,5) },
                new EmployeeProject { EmployeeId = 218, ProjectId = 10, DateFrom = new DateTime(2012,5,16), DateTo = null },
                new EmployeeProject { EmployeeId = 143, ProjectId = 10, DateFrom = new DateTime(2009,1,1), DateTo = new DateTime(2012,5,27) },
                new EmployeeProject { EmployeeId = 218, ProjectId = 10, DateFrom = new DateTime(2010,1,1), DateTo = new DateTime(2011,4,27) },
                new EmployeeProject { EmployeeId = 219, ProjectId = 12, DateFrom = new DateTime(2014,1,1), DateTo = new DateTime(2015,4,27) },
            };

            var results = service.GetResult(employees);

            Assert.NotEmpty(results);

            var firstPair = results.First();
            Assert.Equal(143, firstPair.FirstEmployeeId);
            Assert.Equal(218, firstPair.SecondEmployeeId);
            Assert.Equal(10, firstPair.ProjectId);
            // Overlap 2010-01-01..2011-04-27 inclusive = 482 days
            // Overlap 2012-05-16..2012-05-27 inclusive = 12 days
            Assert.Equal(494, firstPair.DaysWorked);

            var secondPair = results.Last();
            Assert.Equal(143, secondPair.FirstEmployeeId);
            Assert.Equal(219, secondPair.SecondEmployeeId);
            Assert.Equal(12, secondPair.ProjectId);
            // Overlap 2014-01-01..2014-01-05 inclusive = 5 days
            Assert.Equal(5, secondPair.DaysWorked);
        }

        [Fact]
        public void GetResult_DateToNull_IsTreatedAsToday()
        {
            var provider = new FakeDateTimeProvider { UtcNow = new DateTime(2023, 1, 31) };
            var service = new EmplyeePairService(provider);

            var employees = new[]
            {
                new EmployeeProject { EmployeeId = 1, ProjectId = 100, DateFrom = new DateTime(2023,1,1), DateTo = null },
                new EmployeeProject { EmployeeId = 2, ProjectId = 100, DateFrom = new DateTime(2023,1,10), DateTo = new DateTime(2023,1,20) }
            };

            var results = service.GetResult(employees);

            Assert.Single(results);
            var r = results.Single();
            Assert.Equal(1, r.FirstEmployeeId);
            Assert.Equal(2, r.SecondEmployeeId);
            Assert.Equal(100, r.ProjectId);
            // Overlap 2023-01-10..2023-01-20 inclusive = 11 days
            Assert.Equal(11, r.DaysWorked);
        }

        [Fact]
        public void GetResult_NoOverlap_ReturnsEmpty()
        {
            var provider = new FakeDateTimeProvider { UtcNow = DateTime.UtcNow };
            var service = new EmplyeePairService(provider);

            var employees = new[]
            {
                new EmployeeProject { EmployeeId = 1, ProjectId = 200, DateFrom = new DateTime(2023,1,1), DateTo = new DateTime(2023,1,10) },
                new EmployeeProject { EmployeeId = 2, ProjectId = 200, DateFrom = new DateTime(2023,1,11), DateTo = new DateTime(2023,1,20) }
            };

            var results = service.GetResult(employees);

            Assert.Empty(results);
        }

        [Fact]
        public void GetResult_InclusiveDaysCalculation_Works()
        {
            var provider = new FakeDateTimeProvider { UtcNow = DateTime.UtcNow };
            var service = new EmplyeePairService(provider);

            var employees = new[]
            {
                new EmployeeProject { EmployeeId = 3, ProjectId = 300, DateFrom = new DateTime(2023,3,1), DateTo = new DateTime(2023,3,5) },
                new EmployeeProject { EmployeeId = 4, ProjectId = 300, DateFrom = new DateTime(2023,3,5), DateTo = new DateTime(2023,3,10) }
            };

            var results = service.GetResult(employees);

            Assert.Single(results);
            var r = results.Single();
            // Overlap only on 2023-03-05 => 1 day (inclusive)
            Assert.Equal(1, r.DaysWorked);
        }
    }
}
