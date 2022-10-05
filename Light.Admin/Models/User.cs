﻿using MongoDB.Bson;
using System.ComponentModel;
using Microsoft.AspNetCore.Identity;
using Light.Admin.Interfaces;
using Light.Admin.Models.Basics;

namespace Light.Admin.Models
{
    public class User : IdentityUser<ObjectId>, IAuditMetadata
    {
        /// <summary>
        ///  元数据
        /// </summary>
        public AuditMetadata AuditMetadata { get; set; }

        /// <summary>
        /// 加密盐
        /// </summary>
        public string Salt { get; set; }

        /// <summary>
        /// 客户端IP
        /// </summary>
        public string IP { get; set; }
    }
}
