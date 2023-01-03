using Light.Admin.Mongo.Basics;
using MongoDB.Bson;

namespace Light.Admin.Mongo.Models
{
    public class Dictionarie : BaseModel<long>
    {
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

    public class DictionarieItems
    {
        public int order { get; set; }

        /// <summary>
        /// Value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Label
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 是否默认
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// 子项
        /// </summary>
        public List<DictionarieItems> children { get; set; }

        /// <summary>
        /// 字段类型
        /// </summary>
        public FieldType Type { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }

    public enum FieldType
    {
        十进制,
        整数,
        小数,
        布尔,
        时间,
        文本,
    }
}