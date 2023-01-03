using Light.Admin.Mongo.Basics;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;

namespace Light.Admin.Mongo
{
    public class Role : IdentityRole<ObjectId>
    {
        /// <summary>
        ///  元数据
        /// </summary>
        public AuditMetadata AuditMetadata { get; set; }
    }
}