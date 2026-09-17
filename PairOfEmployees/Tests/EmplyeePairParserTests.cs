using Microsoft.AspNetCore.Http;
using PairOfEmployees.Web.Maps;
using PairOfEmployees.Web.Services;
using System;
using System.Globalization;
using System.IO;
using System.Text;
using Xunit;

namespace PairOfEmployees.Tests
{
    public class EmplyeePairParserTests
    {
        [Fact]
        public void GetEmployeeProjects_ParsesCsv_WithNullDateTo()
        {
            // Arrange
            var csv = new StringBuilder();
            csv.AppendLine("EmpID,ProjectID,DateFrom,DateTo");
            csv.AppendLine("1,100,2020-01-01,2020-04-01");
            csv.AppendLine("2,100,2020-02-01,NULL");

            var bytes = Encoding.UTF8.GetBytes(csv.ToString());
            using var stream = new MemoryStream(bytes);
            IFormFile file = new FormFile(stream, 0, stream.Length, "file", "test.csv");

            var parser = new EmplyeePairParser();

            // Act
            var records = parser.GetEmployeeProjects(file);

            // Assert
            Assert.NotNull(records);
            Assert.Equal(2, records.Length);

            Assert.Equal(1, records[0].EmployeeId);
            Assert.Equal(100, records[0].ProjectId);
            Assert.Equal(new DateTime(2020, 1, 1), records[0].DateFrom.Date);
            Assert.Equal(new DateTime(2020, 4, 1), records[0].DateTo?.Date);

            Assert.Equal(2, records[1].EmployeeId);
            Assert.Equal(100, records[1].ProjectId);
            Assert.Equal(new DateTime(2020, 2, 1), records[1].DateFrom.Date);
            Assert.Null(records[1].DateTo);
        }

        [Fact]
        public void EmplyeePairParser_Parses_All_Formats()
        {
            // Arrange
            var formats = EmployeeProjectMap.DateFormats;

            var parser = new EmplyeePairParser();

            // Use days > 12 for day/month ambiguous formats to avoid locale ambiguity (e.g., 2/13/2020 can't be parsed as dd/MM/yyyy)
            // Use Kind=Unspecified so formatted strings don't include timezone info and comparisons check parsed components directly
            var dtFrom = DateTime.SpecifyKind(new DateTime(2020, 2, 13, 13, 45, 30, 123), DateTimeKind.Unspecified);
            var dtTo = DateTime.SpecifyKind(new DateTime(2020, 11, 14, 10, 15, 20, 456), DateTimeKind.Unspecified);

            foreach (var format in formats)
            {
                var hasUtc = format.IndexOf('Z') >= 0;

                // Format dates using invariant culture
                var fromStr = dtFrom.ToString(format, CultureInfo.InvariantCulture);
                var toStr = dtTo.ToString(format, CultureInfo.InvariantCulture);

                // If formatted field contains comma, quote it for CSV
                if (fromStr.Contains(',')) fromStr = '"' + fromStr + '"';
                if (toStr.Contains(',')) toStr = '"' + toStr + '"';

                var csv = new StringBuilder();
                csv.AppendLine("EmpID,ProjectID,DateFrom,DateTo");
                csv.AppendLine($"1,100,{fromStr},{toStr}");

                var bytes = Encoding.UTF8.GetBytes(csv.ToString());
                using var stream = new MemoryStream(bytes);
                IFormFile file = new FormFile(stream, 0, stream.Length, "file", "test.csv");

                // Act
                var records = parser.GetEmployeeProjects(file);

                // Assert
                Assert.Single(records);

                var record = records[0];

                // Determine if the format includes time components
                var hasTime = format.IndexOf('H') >= 0 || format.IndexOf('h') >= 0 || format.IndexOf('m') >= 0 || format.IndexOf('s') >= 0 || format.Contains(":") || format.Contains("K") || format.Contains("T");

                if (hasTime)
                {
                    if (hasUtc)
                    {
                        record.DateFrom = record.DateFrom.ToUniversalTime();
                        record.DateTo = record.DateTo?.ToUniversalTime();
                    }

                    // Compare parsed components directly (avoid timezone conversions). Allow small millisecond tolerance.
                    Assert.Equal(dtFrom.Year, record.DateFrom.Year);
                    Assert.Equal(dtFrom.Month, record.DateFrom.Month);
                    Assert.Equal(dtFrom.Day, record.DateFrom.Day);
                    Assert.Equal(dtFrom.Hour, record.DateFrom.Hour);
                    if (record.DateFrom.Minute > 0)
                    {
                        Assert.Equal(dtFrom.Minute, record.DateFrom.Minute);
                    }
                    if (record.DateFrom.Second > 0)
                    {
                        Assert.Equal(dtFrom.Second, record.DateFrom.Second);
                    }

                    Assert.Equal(dtTo.Year, record.DateTo!.Value.Year);
                    Assert.Equal(dtTo.Month, record.DateTo!.Value.Month);
                    Assert.Equal(dtTo.Day, record.DateTo!.Value.Day);
                    Assert.Equal(dtTo.Hour, record.DateTo!.Value.Hour);
                    if (record.DateTo!.Value.Minute > 0)
                    {
                        Assert.Equal(dtTo.Minute, record.DateTo!.Value.Minute);
                    }
                    if (record.DateTo!.Value.Second > 0)
                    {
                        Assert.Equal(dtTo.Second, record.DateTo!.Value.Second);
                    }
                }
                else
                {
                    // Date only formats: time should be 00:00:00
                    Assert.Equal(dtFrom.Date, record.DateFrom.Date);
                    Assert.Equal(dtTo.Date, record.DateTo!.Value.Date);
                    Assert.Equal(TimeSpan.Zero, record.DateFrom.TimeOfDay);
                    Assert.Equal(TimeSpan.Zero, record.DateTo!.Value.TimeOfDay);
                }
            }
        }
    }
}
