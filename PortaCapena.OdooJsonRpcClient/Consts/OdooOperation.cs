namespace PortaCapena.OdooJsonRpcClient.Consts
{
    public static class OdooOperation
    {
        public const string Read = "read";
        public const string Search = "search";

        public const string SearchRead = "search_read";
        public const string FieldsGet = "fields_get";
    
        public const string Create = "create";
        public const string Update = "write";
        public const string Delete = "unlink";

        public const string NameGet = "name_get";
        public const string SearchCount = "search_count";
        public const string GetMetadata = "get_metadata";

        public const string OnChage = "onchage";

        public const string ReadGroup = "read_group";
        public const string NameSearch = "name_search";
        public const string NameCreate = "name_create";
        public const string Copy = "copy";
        public const string DefaultGet = "default_get";
        public const string CheckAccessRights = "check_access_rights";
    }
}