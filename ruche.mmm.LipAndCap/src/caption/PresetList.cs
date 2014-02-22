using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ruche.mmm.caption
{
    /// <summary>
    /// 字幕プリセットリストクラス。
    /// </summary>
    [CollectionDataContract(ItemName = "Item", Namespace = "")]
    [KnownType(typeof(Preset))]
    public sealed class PresetList : List<Preset>, ICloneable
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public PresetList() : base() { }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="src">初期値列挙。</param>
        public PresetList(IEnumerable<Preset> src)
            : base(src)
        {
        }

        /// <summary>
        /// 自身のクローンを作成する。
        /// </summary>
        /// <returns>自身のクローン。</returns>
        public PresetList Clone()
        {
            return new PresetList(this.ConvertAll(p => p.Clone()));
        }

        #region ICloneable の明示的実装

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion
    }
}
