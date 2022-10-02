using Light.Admin.CSharp.Dtos;
using Light.Framework.Models;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel;

namespace Light.Admin.CSharp.Models
{
    public class User : UserDto
    {
        public AuditMetadata AuditMetadata { get; set; }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        /// <summary>
        /// 加密盐
        /// </summary>
        public string Salt { get; set; }

        /// <summary>
        /// 软删除
        /// </summary>
        [DefaultValue(false)]
        public bool Deleted { get; set; }

        /// <summary>
        /// 客户端IP
        /// </summary>
        public string IP { get; set; }
    }
}
