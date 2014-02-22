using System;
using System.Runtime.Serialization;

namespace ruche.mmm.caption
{
    /// <summary>
    /// 字幕プリセットクラス。
    /// </summary>
    [DataContract(Namespace = "")]
    [KnownType(typeof(CaptionInfo))]
    public sealed class Preset : ICloneable
    {
        /// <summary>
        /// 既定のプリセット名。
        /// </summary>
        public static readonly string DefaultName = "デフォルト";

        /// <summary>
        /// プリセット名として使える文字列か否かを取得する。
        /// </summary>
        /// <param name="name">調べる文字列。</param>
        /// <returns>プリセット名として使えるならば true 。</returns>
        public static bool IsValidName(string name)
        {
            return !string.IsNullOrWhiteSpace(name);
        }

        /// <summary>
        /// 既定値で初期化するコンストラクタ。
        /// </summary>
        public Preset() : this(null, null)
        {
        }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="name">
        /// プリセット名。 null を指定すると既定値になる。
        /// </param>
        /// <param name="value">
        /// プリセットデータ。 null を指定すると既定値になる。
        /// </param>
        public Preset(string name, CaptionInfo value)
        {
            if (name != null && !IsValidName(name))
            {
                throw new ArgumentException("不正なプリセット名です。", "name");
            }

            this.Name = name ?? DefaultName;
            this.Value = value ?? new CaptionInfo();
        }

        /// <summary>
        /// プリセット名を取得する。
        /// </summary>
        [DataMember]
        public string Name { get; private set; }

        /// <summary>
        /// プリセットデータを取得する。
        /// </summary>
        [DataMember]
        public CaptionInfo Value { get; private set; }

        /// <summary>
        /// 自身のクローンを作成する。
        /// </summary>
        /// <returns>自身のクローン。</returns>
        public Preset Clone()
        {
            return new Preset(this.Name, this.Value.Clone());
        }

        #region ICloneable の明示的実装

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion
    }
}
