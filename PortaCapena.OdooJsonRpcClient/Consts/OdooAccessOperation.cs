using System.ComponentModel;

namespace PortaCapena.OdooJsonRpcClient.Consts
{
    /// <include file='../Docs/OdooAccessOperation.xml' path='docs/member[@name="OdooAccessOperation"]/*'/>
    public enum OdooAccessOperation
    {
        /// <include file='../Docs/OdooAccessOperation.xml' path='docs/member[@name="Read"]/*'/>
        [Description("read")]
        Read,

        /// <include file='../Docs/OdooAccessOperation.xml' path='docs/member[@name="Write"]/*'/>
        [Description("write")]
        Write,

        /// <include file='../Docs/OdooAccessOperation.xml' path='docs/member[@name="Create"]/*'/>
        [Description("create")]
        Create,

        /// <include file='../Docs/OdooAccessOperation.xml' path='docs/member[@name="Unlink"]/*'/>
        [Description("unlink")]
        Unlink
    }
}
