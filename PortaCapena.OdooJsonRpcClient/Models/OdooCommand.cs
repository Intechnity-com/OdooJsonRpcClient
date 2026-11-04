namespace PortaCapena.OdooJsonRpcClient.Models
{
    /// <include file='../Docs/OdooCommand.xml' path='docs/member[@name="OdooCommand"]/*'/>
    public static class OdooCommand
    {
        /// <include file='../Docs/OdooCommand.xml' path='docs/member[@name="Create"]/*'/>
        public static object[] Create(OdooDictionaryModel values)
        {
            return new object[] { new object[] { 0, 0, values } };
        }

        /// <include file='../Docs/OdooCommand.xml' path='docs/member[@name="Update"]/*'/>
        public static object[] Update(long id, OdooDictionaryModel values)
        {
            return new object[] { new object[] { 1, id, values } };
        }

        /// <include file='../Docs/OdooCommand.xml' path='docs/member[@name="Delete"]/*'/>
        public static object[] Delete(long id)
        {
            return new object[] { new object[] { 2, id, 0 } };
        }

        /// <include file='../Docs/OdooCommand.xml' path='docs/member[@name="Unlink"]/*'/>
        public static object[] Unlink(long id)
        {
            return new object[] { new object[] { 3, id, 0 } };
        }

        /// <include file='../Docs/OdooCommand.xml' path='docs/member[@name="Link"]/*'/>
        public static object[] Link(long id)
        {
            return new object[] { new object[] { 4, id, 0 } };
        }

        /// <include file='../Docs/OdooCommand.xml' path='docs/member[@name="Clear"]/*'/>
        public static object[] Clear()
        {
            return new object[] { new object[] { 5, 0, 0 } };
        }

        /// <include file='../Docs/OdooCommand.xml' path='docs/member[@name="Set"]/*'/>
        public static object[] Set(params long[] ids)
        {
            return new object[] { new object[] { 6, 0, ids } };
        }
    }
}
