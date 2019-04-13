using System;

namespace Demo.CalcEng.Domain
{
    public class TwoNumbersOperationRequest
    {
        public decimal FirstNumber { get; set; }
        public decimal SecondNumber { get; set; }
    }

    public class MultiNumbersOperationRequest
    {
        public decimal[] Numbers { get; set; }
    }

    public class CalcEng
    {
        public OperationResponse<decimal> Sum(TwoNumbersOperationRequest request)
        {
            var data = request.FirstNumber + request.SecondNumber;
            return OperationResponse.CreateSucceed(data);
        }

        public OperationResponse<decimal> Sum(MultiNumbersOperationRequest request)
        {
            if (request.Numbers.Length == 0)
            {
                return OperationResponse.CreateFailled<decimal>("The list is empty");
            }

            decimal total = 0;
            foreach (var number in request.Numbers)
            {
                total += number;
            }

            return OperationResponse.CreateSucceed(total);
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
                return OperationResponse.CreateFailled<decimal>(ex.Message);
            }
        }

        public OperationResponse<decimal[]> PrimeNumbers()
        {
            var data = new decimal[] { 2, 3, 5, 7, 11, 13, 17, 19, 23 };
            return OperationResponse.CreateSucceed(data);
        }
    }
}
