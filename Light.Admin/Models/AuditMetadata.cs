using System.ComponentModel.DataAnnotations.Schema;

namespace Light.Admin.Models
{
    //
    // Summary:
    //     文档元数据
    [ComplexType]
    public class AuditMetadata
    {
        //
        // 摘要:
        //     创建人
        public virtual UserIdentity CreatedBy { get; set; } = new UserIdentity();

        //
        // Summary:
        //     创建时间
        public virtual DateTime CreatedOn { get; set; } = DateTime.Now;

        //
        // 摘要:
        //     修改人
        public virtual UserIdentity ModifiedBy { get; set; } = new UserIdentity();

        //
        // Summary:
        //     修改时间
        public virtual DateTime ModifiedOn { get; set; } = DateTime.Now;
    }
}
