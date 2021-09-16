namespace WebApplication.model
{
    public class RestResult<T>
    {
        public int State { get; set; }
        public string Code { get; set; }
        public string Msg { get; set; }
        public T Payload { get; set; }
    }
}