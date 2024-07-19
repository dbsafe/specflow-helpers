using System;
using System.Collections.Generic;
using System.Linq;

namespace Demo.CalcEng.Domain
{
    public class TwoNumbersOperationRequest
    {
        public decimal FirstNumber { get; set; }
        public decimal SecondNumber { get; set; }
    }

    public class DemoItem
    {
        public int PropA { get; set; }
        public int PropB { get; set; }
    }

    public class MultiNumbersOperationRequest
    {
        public decimal[] Numbers { get; set; }
    }

    public class TotalsOperationRequest
    {
        public DemoItem[] Items { get; set; }
    }

    public class GetDomainItemsByDateRequest
    {
        public DateTime Date { get; set; }
        public bool IsSmall { get; set; }
    }

    public class DomainItem
    {
        public DateTime? Date { get; set; }
        public string PropA { get; set; }
        public string PropB { get; set; }
        public decimal? Value { get; set; }
        public bool? IsSmall { get; set; }
        public Guid? ExternalId { get; set; }
    }

    public class DomainItemWithDateTimeOffset
    {
        public DateTimeOffset? Date { get; set; }
        public string PropA { get; set; }
    }

    public class Row
    {
        public int FieldA { get; set; }
        public int FieldB { get; set; }
        public decimal FieldC { get; set; }
        public bool IsActive { get; set; }
    }

    public interface ICalcEngService
    {
        OperationResponse<decimal> Div(TwoNumbersOperationRequest request);
        OperationResponse<DomainItem[]> GetDomainItems();
        OperationResponse<DomainItem[]> GetDomainItemsByDate(GetDomainItemsByDateRequest request);
        OperationResponse<decimal[]> PrimeNumbers();
        OperationResponse<decimal> Sub(TwoNumbersOperationRequest request);
        OperationResponse<decimal> Sum(MultiNumbersOperationRequest request);
        OperationResponse<decimal> Sum(TwoNumbersOperationRequest request);
        OperationResponse<decimal> Pi();
        OperationResponse<Row> CalculateTotals(IEnumerable<Row> request);
        OperationResponse<Guid> GetGuid();
    }

    public class CalcEngService : ICalcEngService
    {
        private readonly DomainItem[] _domainItems = new DomainItem[]
        {
            new DomainItem { Date = new DateTime(2000, 1, 1), PropA = "item1-pa", Value = 100m, IsSmall = true, ExternalId = Guid.Parse("00000000-0000-0000-0000-000000000001") },
            new DomainItem { Date = new DateTime(2000, 1, 2), PropA = "item2-pa", PropB = "item2-pb", Value = 200m, IsSmall = false, ExternalId = Guid.Parse("00000000-0000-0000-0000-000000000002") },
            new DomainItem { Date = new DateTime(2000, 1, 3), PropA = "item3-pa", PropB = "item3-pb", Value = 300m, IsSmall = false, ExternalId = Guid.Parse("00000000-0000-0000-0000-000000000003") },
            new DomainItem { Date = new DateTime(2000, 1, 4), PropA = "item4-pa", PropB = "item4-pb", Value = 400m, IsSmall = false, ExternalId = Guid.Parse("00000000-0000-0000-0000-000000000004") }
        };

        private readonly DomainItemWithDateTimeOffset[] _domainItemsWithDateTimeOffset = new DomainItemWithDateTimeOffset[]
        {
            new DomainItemWithDateTimeOffset { Date = new DateTimeOffset(new DateTime(2000, 1, 1), TimeSpan.FromHours(-7)), PropA = "item1-pa" },
            new DomainItemWithDateTimeOffset { Date = new DateTimeOffset(new DateTime(2000, 1, 2), TimeSpan.Zero), PropA = "item2-pa" }
        };

        public OperationResponse<decimal> Sum(TwoNumbersOperationRequest request)
        {
            var data = request.FirstNumber + request.SecondNumber;
            return OperationResponse.CreateSucceed(data);
        }

        public OperationResponse<decimal> Sum(MultiNumbersOperationRequest request)
        {
            if (request.Numbers.Length == 0)
            {
                return OperationResponse.CreateFailed<decimal>("The list is empty");
            }

            decimal total = 0;
            foreach (var number in request.Numbers)
            {
                total += number;
            }

            return OperationResponse.CreateSucceed(total);
        }

        public OperationResponse<DemoItem> Totals(TotalsOperationRequest request)
        {
            var totals = new DemoItem();
            foreach(var item in request.Items)
            {
                totals.PropA += item.PropA;
                totals.PropB += item.PropB;
            }

            return OperationResponse.CreateSucceed(totals);
        }

        public OperationResponse<decimal> Sub(TwoNumbersOperationRequest request)
        {
            var data = request.FirstNumber - request.SecondNumber;
            return OperationResponse.CreateSucceed(data);
        }

        public OperationResponse<decimal> Div(TwoNumbersOperationRequest request)
        {
            try
            {
                var data = request.FirstNumber / request.SecondNumber;
                return OperationResponse.CreateSucceed(data);
            }
            catch (Exception ex)
            {
                return OperationResponse.CreateFailed<decimal>(ex.Message);
            }
        }

        public OperationResponse<decimal[]> PrimeNumbers()
        {
            var data = new decimal[] { 2, 3, 5, 7, 11, 13, 17, 19, 23 };
            return OperationResponse.CreateSucceed(data);
        }

        public OperationResponse<DomainItem[]> GetDomainItems()
        {
            return OperationResponse.CreateSucceed(_domainItems);
        }

        public OperationResponse<DomainItemWithDateTimeOffset[]> GetDomainItemsWithDateTimeOffset()
        {
            return OperationResponse.CreateSucceed(_domainItemsWithDateTimeOffset);
        }

        public DomainItem[] GetDomainItemsAsArray()
        {
            return _domainItems;
        }

        public OperationResponse<DomainItem[]> GetDomainItemsByDate(GetDomainItemsByDateRequest request)
        {
            var data = _domainItems
                .Where(a => a.Date == request.Date && a.IsSmall == request.IsSmall)
                .ToArray();
            return OperationResponse.CreateSucceed(data);
        }

        public OperationResponse<decimal> Pi()
        {
            return OperationResponse.CreateSucceed(3.14m);
        }

        public OperationResponse<Row> CalculateTotals(IEnumerable<Row> request)
        {
            var totals = new Row();
            foreach(var row in request)
            {
                if (row.IsActive)
                {
                    totals.FieldA += row.FieldA;
                    totals.FieldB += row.FieldB;
                    totals.FieldC += row.FieldC;
                }
            }

            return OperationResponse.CreateSucceed(totals);
        }

        public OperationResponse<Guid> GetGuid()
        {
            return OperationResponse.CreateSucceed(Guid.Parse("b9c24d94-2d6c-4ea1-a0f2-03df67e4014d"));
        }

        public OperationResponse<string> GetGuidWithoutDashes()
        {
            var guidWithoutDashed = Guid.Parse("b9c24d94-2d6c-4ea1-a0f2-03df67e4014d").ToString().Replace("-", string.Empty);
            return OperationResponse.CreateSucceed(guidWithoutDashed);
        }
    }
}
