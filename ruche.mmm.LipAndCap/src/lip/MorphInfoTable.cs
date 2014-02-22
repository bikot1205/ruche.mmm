using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ruche.mmm.lip
{
    /// <summary>
    /// MorphInfo テーブルクラス。
    /// </summary>
    [CollectionDataContract(ItemName = "Item", Namespace = "")]
    [KnownType(typeof(MorphInfo))]
    public sealed class MorphInfoTable : Dictionary<LipId, MorphInfo>
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public MorphInfoTable() : base() { }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="capacity">初期キャパシティ。</param>
        public MorphInfoTable(int capacity) : base(capacity) { }
    }
}
