namespace PortaCapena.OdooJsonRpcClient.Result
{
    public class OdooException
    {
        public string Name { get; set; }
        public string Debug { get; set; }
        public string Message { get; set; }
        public string[] Arguments { get; set; }
        public object Context { get; set; }
    }
}