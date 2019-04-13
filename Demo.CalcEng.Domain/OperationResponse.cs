namespace Demo.CalcEng.Domain
{
    public class OperationResponse
    {
        public bool Succeed { get; set; }
        public string Error { get; set; }

        public static OperationResponse CreateFailled(string error)
        {
            return new OperationResponse { Succeed = false, Error = error };
        }

        public static OperationResponse CreateSucceed()
        {
            return new OperationResponse { Succeed = true };
        }

        public static OperationResponse<TData> CreateFailled<TData>(string error)
        {
            return new OperationResponse<TData> { Succeed = false, Error = error };
        }

        public static OperationResponse<TData> CreateSucceed<TData>(TData data)
        {
            return new OperationResponse<TData> { Succeed = true, OperationResult = data };
        }
    }

    public class OperationResponse<TData> : OperationResponse
    {
        public TData OperationResult { get; set; }
    }
}
