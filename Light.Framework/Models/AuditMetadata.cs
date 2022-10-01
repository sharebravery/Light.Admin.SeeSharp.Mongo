using System.ComponentModel.DataAnnotations.Schema;

namespace Light.Framework.Models
{
    //
    // Summary:
    //     文档元数据
    [ComplexType]
    public class AuditMetadata
    {
        //
        // Summary:
        //     创建时间
        public virtual DateTime CreatedOn { get; set; } = DateTime.Now;

        //
        // Summary:
        //     修改时间
        public virtual DateTime ModifiedOn { get; set; } = DateTime.Now;
    }
}
