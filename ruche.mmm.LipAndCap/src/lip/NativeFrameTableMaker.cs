using System;
using System.Collections.Generic;
using ruche.mmm.lip.timeline;
using MikuMikuPlugin;

namespace ruche.mmm.lip
{
    /// <summary>
    /// 実フレームデータテーブルを作成するクラス。
    /// </summary>
    public class NativeFrameTableMaker
    {
        /// <summary>
        /// 指定したキーの値を取得または新規追加する。
        /// </summary>
        /// <typeparam name="TKey">キー型。</typeparam>
        /// <typeparam name="TValue">値型。</typeparam>
        /// <param name="table">テーブル。</param>
        /// <param name="key">キー。</param>
        /// <param name="createNew">
        /// 新規追加時のインスタンス生成デリゲート。
        /// null ならば default(TValue) を追加する。
        /// </param>
        /// <returns>取得または新規作成した値。</returns>
        private static TValue GetOrCreate<TKey, TValue>(
            IDictionary<TKey, TValue> table,
            TKey key,
            Func<TValue> createNew = null)
        {
            TValue value;
            if (!table.TryGetValue(key, out value))
            {
                value = (createNew == null) ? default(TValue) : createNew();
                table.Add(key, value);
            }
            return value;
        }

        /// <summary>
        /// モーフ別タイムラインテーブルを基に実フレームデータテーブルを作成する。
        /// </summary>
        /// <param name="tlTable">モーフ別タイムラインテーブル。</param>
        /// <param name="beginFrame">開始実フレーム位置。</param>
        /// <param name="lengthPerUnit">1ユニットあたりの実フレーム長。</param>
        /// <returns>実フレームデータテーブル。</returns>
        public Dictionary<string, List<MorphFrameData>> Make(
            MorphTimelineTable tlTable,
            long beginFrame,
            decimal lengthPerUnit)
        {
            var dest = new Dictionary<string, List<MorphFrameData>>();

            long? srcPos = long.MinValue;
            long destPos = long.MinValue;
            var names = new List<string>();

            while (srcPos.HasValue)
            {
                // 次の登録キー位置を検索
                srcPos = tlTable.FindFirstPlace(p => (p > srcPos), names);
                if (srcPos.HasValue)
                {
                    // 実フレーム位置決定
                    // 前回の位置より最低でも1フレームは進める
                    destPos =
                        Math.Max(
                            CalcNativeFrame(srcPos.Value, lengthPerUnit),
                            destPos + 1);

                    // 登録キーのあるモーフ名ごとに処理
                    names.ForEach(
                        name =>
                        {
                            // フレームデータリスト取得or新規追加
                            var frames =
                                GetOrCreate(
                                    dest,
                                    name,
                                    () => new List<MorphFrameData>());

                            // フレームデータ追加
                            frames.Add(
                                new MorphFrameData(
                                    beginFrame + destPos,
                                    tlTable[name].GetWeight(srcPos.Value)));
                        });
                }
            }

            return dest;
        }

        /// <summary>
        /// 実フレーム位置を算出する。
        /// </summary>
        /// <param name="place">タイムラインのキー位置。</param>
        /// <param name="lengthPerUnit">1ユニットあたりの実フレーム長。</param>
        /// <returns>実フレーム位置。</returns>
        private long CalcNativeFrame(long place, decimal lengthPerUnit)
        {
            return (long)(
                place * lengthPerUnit / MorphTimelineTable.LengthPerUnit + 0.5m);
        }
    }
}
