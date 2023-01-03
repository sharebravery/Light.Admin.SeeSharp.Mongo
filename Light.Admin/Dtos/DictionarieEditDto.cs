using Light.Admin.Mongo.Models;

namespace Light.Admin.Mongo.Dtos
{
    public class DictionarieEditDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// key 方便查找
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 字典项
        /// </summary>
        public List<DictionarieItems> dictionarieItems { get; set; }
    }
}