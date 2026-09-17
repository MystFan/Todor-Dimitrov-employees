using PairOfEmployees.Web.Models;

namespace PairOfEmployees.Web.Services
{
    public class EmplyeePairService(IDateTimeProvider dateTimeProvider) : IEmplyeePairService
    {
        public PairResult[] GetResult(IEnumerable<EmployeeProject> employees)
        {
            var records = employees
                .Select(r =>
                {
                    if (r.DateTo == null)
                    {
                        r.DateTo = dateTimeProvider.UtcNow;
                    }
                    return r;
                })
                .ToArray();

            var byProject = records.GroupBy(r => r.ProjectId);

            var employeeProjectDict = new Dictionary<(int firstEmployeeId, int secondEmployeeId, int projectId), long>();

            foreach (var group in byProject)
            {
                var projectEmployees = group.OrderBy(x => x.DateFrom).ToArray();

                var active = new SortedSet<ActiveEntry>(new ActiveEntryComparer());
                long nextId = 0;

                for (int i = 0; i < projectEmployees.Length; i++)
                {
                    var employee = projectEmployees[i];
                    // Evict intervals that end before current start
                    while (active.Count > 0 && active.Min!.DateTo < employee.DateFrom)
                    {
                        active.Remove(active.Min);
                    }

                    foreach (var entry in active)
                    {
                        var otherEmployee = entry.Project;
                        if (employee.EmployeeId == otherEmployee.EmployeeId)
                        {
                            continue;
                        }

                        DateTime start = employee.DateFrom > otherEmployee.DateFrom ? employee.DateFrom : otherEmployee.DateFrom;
                        DateTime? end = employee.DateTo < otherEmployee.DateTo ? employee.DateTo : otherEmployee.DateTo;

                        if (end >= start)
                        {
                            var days = (end - start).Value.Days + 1;
                            var firstId = Math.Min(employee.EmployeeId, otherEmployee.EmployeeId);
                            var secondId = Math.Max(employee.EmployeeId, otherEmployee.EmployeeId);
                            var key = (firstId, secondId, group.Key);

                            if (!employeeProjectDict.ContainsKey(key))
                            {
                                employeeProjectDict[key] = 0;
                            }

                            employeeProjectDict[key] += days;
                        }
                    }

                    // add current interval to active set
                    active.Add(new ActiveEntry(employee, nextId++));
                }
            }

            var results = employeeProjectDict.Select(kv => new PairResult
            {
                FirstEmployeeId = kv.Key.firstEmployeeId,
                SecondEmployeeId = kv.Key.secondEmployeeId,
                ProjectId = kv.Key.projectId,
                DaysWorked = kv.Value
            })
                .OrderByDescending(r => r.DaysWorked)
                .ToArray();

            return results;
        }

        private sealed class ActiveEntry
        {
            public long Id { get; }
            public DateTime DateTo { get; }
            public EmployeeProject Project { get; }

            public ActiveEntry(EmployeeProject project, long id)
            {
                Project = project;
                DateTo = project.DateTo!.Value;
                Id = id;
            }
        }

        private sealed class ActiveEntryComparer : IComparer<ActiveEntry>
        {
            public int Compare(ActiveEntry? x, ActiveEntry? y)
            {
                if (ReferenceEquals(x, y))
                {
                    return 0;
                }

                if (x is null)
                {
                    return -1;
                }

                if (y is null)
                {
                    return 1;
                }

                var cmpare = x.DateTo.CompareTo(y.DateTo);
                if (cmpare != 0)
                {
                    return cmpare;
                }

                return x.Id.CompareTo(y.Id);
            }
        }
    }
}
