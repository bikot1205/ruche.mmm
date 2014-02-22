using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ruche.mmm.lip
{
    /// <summary>
    /// MorphWeightData リストクラス。
    /// </summary>
    [CollectionDataContract(ItemName = "Item", Namespace = "")]
    [KnownType(typeof(MorphWeightData))]
    public sealed class MorphWeightDataList : List<MorphWeightData>
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public MorphWeightDataList() : base() { }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="src">初期値列挙。</param>
        public MorphWeightDataList(IEnumerable<MorphWeightData> src)
            : base(src)
        {
        }
    }
}
