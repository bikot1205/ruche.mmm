using System;
using System.IO;
using System.Drawing;
using ruche.mmm.util;

using cap = ruche.mmm.caption;

namespace ruche.mmm
{
    /// <summary>
    /// 口パク＆字幕用設定データクラス。
    /// </summary>
    internal class LipAndCapConfig
    {
        #region 内部定義

        /// <summary>
        /// 基本設定データクラス。
        /// </summary>
        public sealed class BasicData
        {
            /// <summary>
            /// コンストラクタ。
            /// </summary>
            public BasicData()
            {
                // 既定値を設定
                this.Maximized = false;
                this.Size = new Size();
                this.LipEnabled = true;
                this.LipAutoEnabled = true;
                this.LipPresetName = lip.Preset.DefaultName;
                this.LipSpanType = FrameSpanType.Letter;
                this.LipSpanFrame = 8L;
                this.CaptionEnabled = true;
                this.CaptionPresetName = cap.Preset.DefaultName;
                this.CaptionXYEnabled = false;
                this.CaptionX = 0;
                this.CaptionY = 0;
                this.CaptionFixedSpanFrame = 30L;
                this.CaptionLetterSpanFrame = 3L;
                this.CaptionFadeInFrame = 0L;
                this.CaptionFadeOutFrame = 0L;
            }

            [NoConfigValue]
            public Size Size { get; set; }

            public int Width
            {
                get { return this.Size.Width; }
                set { this.Size = new Size(value, this.Height); }
            }

            public int Height
            {
                get { return this.Size.Height; }
                set { this.Size = new Size(this.Width, value); }
            }

            public bool Maximized { get; set; }
            public bool LipEnabled { get; set; }
            public bool LipAutoEnabled { get; set; }
            public string LipPresetName { get; set; }
            public FrameSpanType LipSpanType { get; set; }
            public long LipSpanFrame { get; set; }
            public bool CaptionEnabled { get; set; }
            public string CaptionPresetName { get; set; }
            public bool CaptionXYEnabled { get; set; }
            public int CaptionX { get; set; }
            public int CaptionY { get; set; }
            public long CaptionFixedSpanFrame { get; set; }
            public long CaptionLetterSpanFrame { get; set; }
            public long CaptionFadeInFrame { get; set; }
            public long CaptionFadeOutFrame { get; set; }
        }

        /// <summary>
        /// 口パクプリセットデータクラス。
        /// </summary>
        public sealed class LipPresetData
        {
            /// <summary>
            /// コンストラクタ。
            /// </summary>
            public LipPresetData()
            {
                // 既定値を設定
                this.Presets = new lip.PresetList();
                this.Presets.Add(new lip.Preset());
            }

            public lip.PresetList Presets { get; set; }
        }

        /// <summary>
        /// 字幕プリセットデータクラス。
        /// </summary>
        public sealed class CaptionPresetData
        {
            /// <summary>
            /// コンストラクタ。
            /// </summary>
            public CaptionPresetData()
            {
                // 既定値を設定
                this.Presets = new cap.PresetList();
                this.Presets.Add(new cap.Preset());
            }

            public cap.PresetList Presets { get; set; }
        }

        /// <summary>
        /// モデルのGUIDとプリセットの関連付けデータクラス。
        /// </summary>
        public sealed class ModelPresetRelationData
        {
            /// <summary>
            /// コンストラクタ。
            /// </summary>
            public ModelPresetRelationData()
            {
                // 既定値を設定
                this.Lip = new ModelPresetRelationTable();
                this.Caption = new ModelPresetRelationTable();
            }

            public ModelPresetRelationTable Lip { get; set; }
            public ModelPresetRelationTable Caption { get; set; }
        }

        /// <summary>
        /// 設定ファイル保存先ディレクトリパス。
        /// </summary>
        private static readonly string ConfigDirPath =
            Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData),
                @"ruche-home\" + typeof(LipAndCapConfig).Namespace);

        /// <summary>
        /// 基本設定ファイルのパス。
        /// </summary>
        private static readonly string BasicFilePath =
            Path.Combine(ConfigDirPath, @"Basic.config");

        /// <summary>
        /// 口パクプリセットファイルのパス。
        /// </summary>
        private static readonly string LipPresetFilePath =
            Path.Combine(ConfigDirPath, @"LipPreset.config");

        /// <summary>
        /// 字幕プリセットファイルのパス。
        /// </summary>
        private static readonly string CaptionPresetFilePath =
            Path.Combine(ConfigDirPath, @"CaptionPreset.config");

        /// <summary>
        /// モデルプリセット関連付けファイルのパス。
        /// </summary>
        private static readonly string ModelPresetRelationFilePath =
            Path.Combine(ConfigDirPath, @"ModelPresetRelation.config");

        /// <summary>
        /// 基本設定データ保持オブジェクトを取得または設定する。
        /// </summary>
        private ConfigKeeper<BasicData> BasicKeeper { get; set; }

        /// <summary>
        /// 口パクプリセットデータ保持オブジェクトを取得または設定する。
        /// </summary>
        private ConfigKeeper<LipPresetData> LipPresetKeeper { get; set; }

        /// <summary>
        /// 字幕プリセットデータ保持オブジェクトを取得または設定する。
        /// </summary>
        private ConfigKeeper<CaptionPresetData> CaptionPresetKeeper { get; set; }

        /// <summary>
        /// モデルプリセット関連付けデータ保持オブジェクトを取得または設定する。
        /// </summary>
        private ConfigKeeper<ModelPresetRelationData> ModelPresetRelationKeeper
        {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public LipAndCapConfig()
        {
            this.BasicKeeper = new ConfigKeeper<BasicData>(BasicFilePath);
            this.LipPresetKeeper = new ConfigKeeper<LipPresetData>(LipPresetFilePath);
            this.CaptionPresetKeeper =
                new ConfigKeeper<CaptionPresetData>(CaptionPresetFilePath);
            this.ModelPresetRelationKeeper =
                new ConfigKeeper<ModelPresetRelationData>(ModelPresetRelationFilePath);
        }

        /// <summary>
        /// 基本設定データを取得する。
        /// </summary>
        public BasicData Basic
        {
            get { return this.BasicKeeper.Data; }
        }

        /// <summary>
        /// 口パクプリセットデータを取得する。
        /// </summary>
        public LipPresetData LipPreset
        {
            get { return this.LipPresetKeeper.Data; }
        }

        /// <summary>
        /// 字幕プリセットデータを取得する。
        /// </summary>
        public CaptionPresetData CaptionPreset
        {
            get { return this.CaptionPresetKeeper.Data; }
        }

        /// <summary>
        /// モデルプリセット関連付けデータを取得する。
        /// </summary>
        public ModelPresetRelationData ModelPresetRelation
        {
            get { return this.ModelPresetRelationKeeper.Data; }
        }

        /// <summary>
        /// 基本設定データをロードする。
        /// </summary>
        /// <returns>
        /// ロードに成功したならば true 。そうでなければ false 。
        /// </returns>
        public bool LoadBasic()
        {
            return this.BasicKeeper.Load();
        }

        /// <summary>
        /// 口パクプリセットデータをロードする。
        /// </summary>
        /// <returns>
        /// ロードに成功したならば true 。そうでなければ false 。
        /// </returns>
        public bool LoadLipPreset()
        {
            return this.LipPresetKeeper.Load();
        }

        /// <summary>
        /// 字幕プリセットデータをロードする。
        /// </summary>
        /// <returns>
        /// ロードに成功したならば true 。そうでなければ false 。
        /// </returns>
        public bool LoadCaptionPreset()
        {
            return this.CaptionPresetKeeper.Load();
        }

        /// <summary>
        /// モデルプリセット関連付けデータをロードする。
        /// </summary>
        /// <returns>
        /// ロードに成功したならば true 。そうでなければ false 。
        /// </returns>
        public bool LoadModelPresetRelation()
        {
            return this.ModelPresetRelationKeeper.Load();
        }

        /// <summary>
        /// 基本設定データをセーブする。
        /// </summary>
        /// <returns>
        /// セーブに成功したならば true 。そうでなければ false 。
        /// </returns>
        public bool SaveBasic()
        {
            return this.BasicKeeper.Save();
        }

        /// <summary>
        /// 口パクプリセットデータをセーブする。
        /// </summary>
        /// <returns>
        /// セーブに成功したならば true 。そうでなければ false 。
        /// </returns>
        public bool SaveLipPreset()
        {
            return this.LipPresetKeeper.Save();
        }

        /// <summary>
        /// 字幕プリセットデータをセーブする。
        /// </summary>
        /// <returns>
        /// セーブに成功したならば true 。そうでなければ false 。
        /// </returns>
        public bool SaveCaptionPreset()
        {
            return this.CaptionPresetKeeper.Save();
        }

        /// <summary>
        /// モデルプリセット関連付けデータをセーブする。
        /// </summary>
        /// <returns>
        /// セーブに成功したならば true 。そうでなければ false 。
        /// </returns>
        public bool SaveModelPresetRelation()
        {
            return this.ModelPresetRelationKeeper.Save();
        }
    }
}
