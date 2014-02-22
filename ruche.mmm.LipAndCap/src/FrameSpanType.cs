using System;
using System.Runtime.Serialization;

namespace ruche.mmm
{
    /// <summary>
    /// 口パク、字幕のフレーム長指定タイプを表す列挙。
    /// </summary>
    [DataContract(Namespace = "")]
    public enum FrameSpanType
    {
        /// <summary>
        /// 1音あたりの長さを指定する。
        /// </summary>
        [EnumMember]
        Letter,

        /// <summary>
        /// 文章全体の長さを指定する。
        /// </summary>
        [EnumMember]
        All,
    }
}
