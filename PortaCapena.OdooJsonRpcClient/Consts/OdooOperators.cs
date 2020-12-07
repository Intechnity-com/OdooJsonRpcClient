using System.ComponentModel;

namespace PortaCapena.OdooJsonRpcClient.Consts
{
    public enum OdooOperator
    {
        [Description("=")]
        EqualsTo,
        [Description("!=")]
        NotEqualsTo,
        [Description("=?")]
        UnsetOrEqualsTo,

        [Description(">")]
        GreaterThan,
        [Description(">=")]
        GreaterThanOrEqualTo,

        [Description("<")]
        LessThan,
        [Description("<=")]
        LessThanOrEqualTo,

        [Description("like")]
        Like,
        [Description("not like")]
        NotLike,
        [Description("=like")]
        EqualsLike,

        [Description("ilike")]
        CaseInsensitiveLike,
        [Description("not ilike")]
        CaseInsensitiveNotLike,
        [Description("=ilike")]
        CaseInsensitiveEqualsLike,

        [Description("in")]
        In,
        [Description("not in")]
        NotIn,

        [Description("child_of")]
        ChildOf,
        [Description("parent_of")]
        ParentOf,

        [Description("&")]
        And,
        [Description("|")]
        Or,
        [Description("!")]
        Not
    }
}