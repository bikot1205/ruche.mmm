using System;
using System.Runtime.Serialization;

namespace ruche.mmm.lip
{
    /// <summary>
    /// モーフ名とそのウェイト値を保持する構造体。
    /// </summary>
    [DataContract(Namespace = "")]
    public struct MorphWeightData
    {
        /// <summary>
        /// モーフ名を取得または設定する。
        /// </summary>
        [DataMember]
        public string MorphName { get; set; }

        /// <summary>
        /// ウェイト値を取得または設定する。
        /// </summary>
        [DataMember]
        public float Weight
        {
            get { return _weight; }
            set
            {
                _weight = Math.Min(Math.Max(0.0f, value), 1.0f);
            }
        }
        private float _weight;
    }
}
