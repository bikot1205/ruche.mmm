﻿using System;

namespace ruche.mmm.lip.timeline
{
    /// <summary>
    /// モーフ別のタイムラインを保持するクラス。
    /// </summary>
    public class MorphTimelineTable : TimelineTableBase<string>
    {
        /// <summary>
        /// 長さ100％の1ユニットあたりのタイムライン長。
        /// </summary>
        public static readonly long LengthPerUnit = LipTimelineSet.LengthPerUnit;

        /// <summary>
        /// タイムラインを取得または設定するインデクサ。
        /// </summary>
        /// <param name="morphName">モーフ名。</param>
        /// <returns>タイムライン。格納されていなければ null 。</returns>
        public override Timeline this[string morphName]
        {
            get
            {
                Timeline value = null;
                if (this.Table.TryGetValue(morphName, out value))
                {
                    return value;
                }
                return null;
            }
            set
            {
                if (value == null)
                {
                    this.Remove(morphName);
                }
                else
                {
                    this.Table[morphName] = value;
                }
            }
        }

        /// <summary>
        /// タイムラインが格納されているか否かを取得する。
        /// </summary>
        /// <param name="morphName">モーフ名。</param>
        /// <returns>格納されているならば true 。</returns>
        public bool Contains(string morphName)
        {
            return this.Table.ContainsKey(morphName);
        }

        /// <summary>
        /// 空のタイムラインを新規追加する。既に存在する場合は上書きする。
        /// </summary>
        /// <param name="morphName">モーフ名。</param>
        /// <returns>新規追加されたタイムライン。</returns>
        public Timeline AddNew(string morphName)
        {
            return (this[morphName] = new Timeline());
        }

        /// <summary>
        /// タイムラインが格納されていなければ新規追加して取得する。
        /// </summary>
        /// <param name="morphName">モーフ名。</param>
        /// <returns>既存もしくは新規追加されたタイムライン。</returns>
        public Timeline GetOrAddNew(string morphName)
        {
            return (this[morphName] ?? this.AddNew(morphName));
        }

        /// <summary>
        /// タイムラインが格納されていれば削除する。
        /// </summary>
        /// <param name="morphName">モーフ名。</param>
        /// <returns>削除されたならば true 。</returns>
        public bool Remove(string morphName)
        {
            return this.Table.Remove(morphName);
        }
    }
}
