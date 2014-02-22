using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ruche.mmm.lip;
using ruche.mmm.lip.timeline;
using ruche.mmm.caption;
using ruche.mmm.util;
using MikuMikuPlugin;

using cap = ruche.mmm.caption;

namespace ruche.mmm
{
    /// <summary>
    /// 口パク＆字幕データ設定ダイアログクラス。
    /// </summary>
    public partial class LipAndCapDialog : Form
    {
        /// <summary>
        /// 文字列から改行文字を除外する。
        /// </summary>
        /// <param name="src">文字列。</param>
        /// <returns>改行文字を除外した文字列。</returns>
        private static string RemoveCrLf(string src)
        {
            return string.Join(
                string.Empty,
                from e in new TextElementEnumerable(src)
                where e != "\r" && e != "\n"
                select e);
        }

        /// <summary>
        /// 読み仮名への変換中か否か。
        /// </summary>
        private bool _kanaConverting = false;

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="englishMode">英語表示モードならば true 。</param>
        /// <param name="targetModel">
        /// 設定対象モデル。モデルを対象としないならば null 。
        /// </param>
        public LipAndCapDialog(bool englishMode, Model targetModel)
        {
            this.Config = new LipAndCapConfig();
            this.EnglishMode = englishMode;
            this.TargetModel = targetModel;

            InitializeComponent();

            LoadBasicConfig();
            LoadLipPreset();
            LoadCaptionPreset();
            LoadModelLipRelation();
        }

        /// <summary>
        /// 英語表示モードか否かを取得する。
        /// ただし現時点では英語表示に対応していない。
        /// </summary>
        public bool EnglishMode { get; private set; }

        /// <summary>
        /// 設定対象モデルを取得する。モデルを対象としないならば  null 。
        /// </summary>
        public Model TargetModel { get; private set; }

        /// <summary>
        /// 口パク設定が有効か否かを取得または設定する。
        /// </summary>
        public bool LipEnabled
        {
            get { return _lipEnabled; }
            set { _lipEnabled = value; }
        }
        private bool _lipEnabled = true;

        /// <summary>
        /// 入力文から口パクデータへの自動変換が有効か否かを取得または設定する。
        /// </summary>
        public bool LipAutoEnabled
        {
            get { return _lipAutoEnabled; }
            set { _lipAutoEnabled = value; }
        }
        private bool _lipAutoEnabled = false;

        /// <summary>
        /// 選択する口パクプリセット名を取得または設定する。
        /// </summary>
        public string LipPresetName
        {
            get { return _lipPresetName; }
            set { _lipPresetName = value ?? lip.Preset.DefaultName; }
        }
        private string _lipPresetName = lip.Preset.DefaultName;

        /// <summary>
        /// 口パク長さ指定タイプを取得または設定する。
        /// </summary>
        public FrameSpanType LipSpanType
        {
            get { return _lipSpanType; }
            set
            {
                if (Enum.IsDefined(typeof(FrameSpanType), value))
                {
                    _lipSpanType = value;
                }
            }
        }
        private FrameSpanType _lipSpanType = FrameSpanType.Letter;

        /// <summary>
        /// 口パクフレーム数指定値を取得または設定する。
        /// </summary>
        public long LipSpanFrame
        {
            get { return _lipSpanFrame; }
            set
            {
                _lipSpanFrame =
                    Math.Min(
                        Math.Max((long)upDownLipSpan.Minimum, value),
                        (long)upDownLipSpan.Maximum);
            }
        }
        private long _lipSpanFrame = 10L;

        /// <summary>
        /// 字幕設定が有効か否かを取得または設定する。
        /// </summary>
        public bool CaptionEnabled
        {
            get { return _capEnabled; }
            set { _capEnabled = value; }
        }
        private bool _capEnabled = true;

        /// <summary>
        /// 選択する字幕プリセット名を取得または設定する。
        /// </summary>
        public string CaptionPresetName
        {
            get { return _capPresetName; }
            set { _capPresetName = value ?? cap.Preset.DefaultName; }
        }
        private string _capPresetName = cap.Preset.DefaultName;

        /// <summary>
        /// 字幕の位置上書きが有効か否かを取得または設定する。
        /// </summary>
        public bool CaptionXYEnabled
        {
            get { return _capXYEnabled; }
            set { _capXYEnabled = value; }
        }
        private bool _capXYEnabled = false;

        /// <summary>
        /// 字幕の上書きX位置を取得または設定する。
        /// </summary>
        public int CaptionX
        {
            get { return _capX; }
            set
            {
                _capX =
                    Math.Min(
                        Math.Max((int)upDownCapX.Minimum, value),
                        (int)upDownCapX.Maximum);
            }
        }
        private int _capX = 0;

        /// <summary>
        /// 字幕の上書きY位置を取得または設定する。
        /// </summary>
        public int CaptionY
        {
            get { return _capY; }
            set
            {
                _capY =
                    Math.Min(
                        Math.Max((int)upDownCapY.Minimum, value),
                        (int)upDownCapY.Maximum);
            }
        }
        private int _capY = 0;

        /// <summary>
        /// 固定の字幕フレーム数指定値を取得または設定する。
        /// </summary>
        public long CaptionFixedSpanFrame
        {
            get { return _capFixedSpanFrame; }
            set
            {
                _capFixedSpanFrame =
                    Math.Min(
                        Math.Max((long)upDownCapSpanFix.Minimum, value),
                        (long)upDownCapSpanFix.Maximum);
            }
        }
        private long _capFixedSpanFrame = 100L;

        /// <summary>
        /// 文字数乗算の字幕フレーム数指定値を取得または設定する。
        /// </summary>
        public long CaptionLetterSpanFrame
        {
            get { return _capLetterSpanFrame; }
            set
            {
                _capLetterSpanFrame =
                    Math.Min(
                        Math.Max((long)upDownCapSpanLetter.Minimum, value),
                        (long)upDownCapSpanLetter.Maximum);
            }
        }
        private long _capLetterSpanFrame = 1L;

        /// <summary>
        /// 字幕フェードインフレーム数指定値を取得または設定する。
        /// </summary>
        public long CaptionFadeInFrame
        {
            get { return _capFadeInFrame; }
            set
            {
                _capFadeInFrame =
                    Math.Min(
                        Math.Max((long)upDownCapFadeIn.Minimum, value),
                        (long)upDownCapFadeIn.Maximum);
            }
        }
        private long _capFadeInFrame = 0L;

        /// <summary>
        /// 字幕フェードアウトフレーム数指定値を取得または設定する。
        /// </summary>
        public long CaptionFadeOutFrame
        {
            get { return _capFadeOutFrame; }
            set
            {
                _capFadeOutFrame =
                    Math.Min(
                        Math.Max((long)upDownCapFadeOut.Minimum, value),
                        (long)upDownCapFadeOut.Maximum);
            }
        }
        private long _capFadeOutFrame = 0L;

        /// <summary>
        /// 口パクデータを取得する。
        /// </summary>
        public List<LipSyncUnit> LipSyncUnits
        {
            get { return _lipSyncUnits; }
            private set { _lipSyncUnits = value ?? (new List<LipSyncUnit>()); }
        }
        private List<LipSyncUnit> _lipSyncUnits = new List<LipSyncUnit>();

        /// <summary>
        /// モーフ別タイムラインテーブルを取得する。
        /// 口パクデータを作成しない設定の場合は null が返る。
        /// </summary>
        public MorphTimelineTable MorphTimelineTable { get; private set; }

        /// <summary>
        /// 字幕文字列を取得する。
        /// </summary>
        public string CaptionText { get; private set; }

        /// <summary>
        /// 字幕情報を取得する。
        /// 字幕データを作成しない設定の場合は null が返る。
        /// </summary>
        public CaptionInfo CaptionInfo { get; private set; }

        /// <summary>
        /// 字幕のフレーム長を取得する。
        /// 字幕データを作成しない設定の場合は 0 が返る。
        /// </summary>
        public long CaptionFrameLength { get; private set; }

        /// <summary>
        /// 字幕のフェードインフレーム長を取得する。
        /// 字幕データを作成しない設定の場合は 0 が返る。
        /// </summary>
        public double CaptionFadeInLength { get; private set; }

        /// <summary>
        /// 字幕のフェードインフレーム長を取得する。
        /// 字幕データを作成しない設定の場合は 0 が返る。
        /// </summary>
        public double CaptionFadeOutLength { get; private set; }

        /// <summary>
        /// 口パク＆字幕用設定データを取得または設定する。
        /// </summary>
        private LipAndCapConfig Config { get; set; }

        /// <summary>
        /// 現在の設定を基に実フレームデータテーブルを作成する。
        /// </summary>
        /// <param name="beginFrame">開始実フレーム位置。</param>
        /// <returns>
        /// 実フレームデータテーブル。
        /// 口パクデータを作成しない設定の場合は null 。
        /// </returns>
        public Dictionary<string, List<MorphFrameData>> MakeMorphFrameTable(
            long beginFrame)
        {
            var tlTable = this.MorphTimelineTable;
            if (tlTable != null)
            {
                // 現在の設定から1ユニットあたりの実フレーム長を決定
                decimal lenPerFrame = this.LipSpanFrame;
                if (this.LipSpanType == FrameSpanType.All)
                {
                    lenPerFrame *= MorphTimelineTable.LengthPerUnit;
                    lenPerFrame /= Math.Max(tlTable.GetEndPlace(), 1);
                }

                // 作成
                var maker = new NativeFrameTableMaker();
                var table = maker.Make(tlTable, beginFrame, lenPerFrame);

                return table;
            }

            return null;
        }

        /// <summary>
        /// 現在の設定を基に字幕データを作成する。
        /// </summary>
        /// <param name="scene">シーン。</param>
        /// <param name="beginFrame">開始実フレーム位置。</param>
        /// <param name="index">字幕インデックス。</param>
        /// <returns>
        /// 字幕データ。字幕データを作成しない設定の場合は null 。
        public Caption MakeCaption(Scene scene, long beginFrame, int index)
        {
            var info = this.CaptionInfo;
            if (info != null)
            {
                var caption = info.ToCaption(scene);

                caption.Text = this.CaptionText;
                caption.StartFrame = beginFrame;
                caption.DurationFrame = this.CaptionFrameLength;
                caption.FadeInFrame = this.CaptionFadeInLength;
                caption.FadeOutFrame = this.CaptionFadeOutLength;
                caption.Index = index;

                return caption;
            }

            return null;
        }

        /// <summary>
        /// 基本設定をロードしてダイアログに設定する。
        /// </summary>
        private void LoadBasicConfig()
        {
            // データをロード
            // 失敗しても既定値が入っている
            this.Config.LoadBasic();

            // サイズがロードできなかった場合はウィンドウの初期値を設定
            if (this.Config.Basic.Width <= 0)
            {
                this.Config.Basic.Width = this.Width;
            }
            if (this.Config.Basic.Height <= 0)
            {
                this.Config.Basic.Height = this.Height;
            }

            // ウィンドウ状態設定
            try
            {
                this.SuspendLayout();

                this.Size = this.Config.Basic.Size;
                this.WindowState =
                    this.Config.Basic.Maximized ?
                        FormWindowState.Maximized :
                        FormWindowState.Normal;
            }
            finally
            {
                this.ResumeLayout();
            }

            // その他の設定を取得
            this.LipEnabled = this.Config.Basic.LipEnabled;
            this.LipAutoEnabled = this.Config.Basic.LipAutoEnabled;
            this.LipPresetName = this.Config.Basic.LipPresetName;
            this.LipSpanType = this.Config.Basic.LipSpanType;
            this.LipSpanFrame = this.Config.Basic.LipSpanFrame;
            this.CaptionEnabled = this.Config.Basic.CaptionEnabled;
            this.CaptionPresetName = this.Config.Basic.CaptionPresetName;
            this.CaptionXYEnabled = this.Config.Basic.CaptionXYEnabled;
            this.CaptionX = this.Config.Basic.CaptionX;
            this.CaptionY = this.Config.Basic.CaptionY;
            this.CaptionFixedSpanFrame = this.Config.Basic.CaptionFixedSpanFrame;
            this.CaptionLetterSpanFrame = this.Config.Basic.CaptionLetterSpanFrame;
            this.CaptionFadeInFrame = this.Config.Basic.CaptionFadeInFrame;
            this.CaptionFadeOutFrame = this.Config.Basic.CaptionFadeOutFrame;
        }

        /// <summary>
        /// 口パクプリセットをロードしてダイアログに設定する。
        /// </summary>
        private void LoadLipPreset()
        {
            // データをロード
            // 失敗しても既定値が入っている
            this.Config.LoadLipPreset();

            // コンボボックスに反映
            DoUpdateLipPresetCombo();
        }

        /// <summary>
        /// 字幕プリセットをロードしてダイアログに設定する。
        /// </summary>
        private void LoadCaptionPreset()
        {
            // データをロード
            // 失敗しても既定値が入っている
            this.Config.LoadCaptionPreset();

            // コンボボックスに反映
            DoUpdateCaptionPresetCombo();
        }

        /// <summary>
        /// モデル口パクプリセット関連付けデータをロードして反映する。
        /// </summary>
        /// <remarks>
        /// LoadLipPreset を呼び出した後に呼び出すこと。
        /// </remarks>
        private void LoadModelLipRelation()
        {
            // 対象モデルがある場合のみ処理
            if (this.TargetModel != null)
            {
                // データをロード
                // 失敗しても既定値が入っている
                this.Config.LoadModelPresetRelation();

                // 関連付けプリセット名があれば設定
                var relation = this.Config.ModelPresetRelation;
                string name;
                if (relation.Lip.TryGetValue(this.TargetModel.GUID, out name))
                {
                    if (FindLipPreset(name) != null)
                    {
                        this.LipPresetName = name;
                    }
                }
                if (relation.Caption.TryGetValue(this.TargetModel.GUID, out name))
                {
                    if (FindCaptionPreset(name) != null)
                    {
                        this.CaptionPresetName = name;
                    }
                }
            }
        }

        /// <summary>
        /// 対象モデルの有無によって口パク関連コントロールの有効状態を設定する。
        /// </summary>
        private void SetupLipControlsEnabled()
        {
            bool enabled = (this.TargetModel != null);

            checkLip.Enabled = enabled;
            buttonLipConv.Enabled = enabled;
            checkLipConv.Enabled = enabled;
            textLipKana.Enabled = enabled;
            textLipSync.Enabled = enabled;
            comboLipPreset.Enabled = enabled;
            buttonLipPreset.Enabled = enabled;
            comboLipSpan.Enabled = enabled;
            upDownLipSpan.Enabled = enabled;
        }

        /// <summary>
        /// プロパティ値をコントロールに反映させる。
        /// </summary>
        private void ApplyPropertyToControls()
        {
            checkLip.Checked = this.LipEnabled;
            checkLipConv.Checked = this.LipAutoEnabled;
            comboLipSpan.SelectedIndex = (int)this.LipSpanType;
            upDownLipSpan.Value = this.LipSpanFrame;
            checkCap.Checked = this.CaptionEnabled;
            checkCapXY.Checked = this.CaptionXYEnabled;
            upDownCapX.Value = this.CaptionX;
            upDownCapY.Value = this.CaptionY;
            upDownCapSpanFix.Value = this.CaptionFixedSpanFrame;
            upDownCapSpanLetter.Value = this.CaptionLetterSpanFrame;
            upDownCapFadeIn.Value = this.CaptionFadeInFrame;
            upDownCapFadeOut.Value = this.CaptionFadeOutFrame;

            // 選択プリセット名を反映
            SelectLipPreset(this.LipPresetName);
            SelectCaptionPreset(this.CaptionPresetName);
        }

        /// <summary>
        /// コントロール状態からプロパティ値を更新する。
        /// </summary>
        private void UpdatePropertyFromControls()
        {
            this.LipEnabled = checkLip.Checked;
            this.LipAutoEnabled = checkLipConv.Checked;
            this.LipSpanType = (FrameSpanType)comboLipSpan.SelectedIndex;
            this.LipSpanFrame = (long)upDownLipSpan.Value;
            this.CaptionEnabled = checkCap.Checked;
            this.CaptionXYEnabled = checkCapXY.Checked;
            this.CaptionX = (int)upDownCapX.Value;
            this.CaptionY = (int)upDownCapY.Value;
            this.CaptionFixedSpanFrame = (long)upDownCapSpanFix.Value;
            this.CaptionLetterSpanFrame = (long)upDownCapSpanLetter.Value;
            this.CaptionFadeInFrame = (long)upDownCapFadeIn.Value;
            this.CaptionFadeOutFrame = (long)upDownCapFadeOut.Value;

            // 選択プリセット名を更新
            this.LipPresetName = GetSelectedLipPresetName();
            this.CaptionPresetName = GetSelectedCaptionPresetName();
        }

        /// <summary>
        /// 入力文から読み仮名への変換処理を行う。
        /// </summary>
        private void DoConvertToLipKana()
        {
            _kanaConverting = true;

            try
            {
                // 入力文から改行を除外
                var text = RemoveCrLf(textInput.Text);

                // 読み仮名に変換して設定
                var maker = new LipKanaMaker();
                textLipKana.Text = maker.Make(text);
            }
            catch (Exception ex)
            {
                // 口パクデータをクリア
                DoClearLipSync();

                // 例外表示
                textLipKana.Text =
                    ex.Message + Environment.NewLine +
                    "--------------------" + Environment.NewLine +
                    ex.GetType().Name + Environment.NewLine +
                    ex.StackTrace;

                return;
            }
            finally
            {
                _kanaConverting = false;
            }

            // 成功したならば続けて口パクデータ変換
            DoConvertToLipSync();
        }

        /// <summary>
        /// 読み仮名から口パクデータへの変換処理を行う。
        /// </summary>
        private void DoConvertToLipSync()
        {
            try
            {
                // 読み仮名から口パクデータへ変換
                var maker = new LipSyncMaker();
                var units = maker.Make(textLipKana.Text);

                // 口パクデータを文字列化して設定
                textLipSync.Text = string.Join("", units);

                // 口パクデータを保存
                this.LipSyncUnits = units;
            }
            catch (Exception ex)
            {
                // 口パクデータをクリア
                DoClearLipSync();

                // 例外表示
                textLipSync.Text =
                    ex.Message + Environment.NewLine +
                    "--------------------" + Environment.NewLine +
                    ex.GetType().Name + Environment.NewLine +
                    ex.StackTrace;
            }
        }

        /// <summary>
        /// 口パクデータのクリア処理を行う。
        /// </summary>
        private void DoClearLipSync()
        {
            // 口パクデータをクリア
            this.LipSyncUnits = null;
            textLipSync.Text = string.Empty;
        }

        /// <summary>
        /// 現在の口パクプリセットをコンボボックスに反映させる。
        /// </summary>
        private void DoUpdateLipPresetCombo()
        {
            // テーブル取得
            var presets = this.Config.LipPreset.Presets;

            // テーブルが空ならデフォルト値を取得
            if (presets == null || presets.Count <= 0)
            {
                presets = (new LipAndCapConfig.LipPresetData()).Presets;
            }

            // 現在の選択項目があればその値を取得しておく
            var oldValue = GetSelectedLipPresetName();

            // プリセット名のリストをデータソースに設定
            comboLipPreset.DataSource = presets.ConvertAll(p => p.Name);

            // 選択インデックス決定
            int index = 0;
            if (oldValue != null)
            {
                index = Math.Max(0, comboLipPreset.Items.IndexOf(oldValue));
            }

            // 選択
            comboLipPreset.SelectedIndex = index;
        }

        /// <summary>
        /// 現在の字幕プリセットをコンボボックスに反映させる。
        /// </summary>
        private void DoUpdateCaptionPresetCombo()
        {
            // テーブル取得
            var presets = this.Config.CaptionPreset.Presets;

            // テーブルが空ならデフォルト値を取得
            if (presets == null || presets.Count <= 0)
            {
                presets = (new LipAndCapConfig.CaptionPresetData()).Presets;
            }

            // 現在の選択項目があればその値を取得しておく
            var oldValue = GetSelectedCaptionPresetName();

            // プリセット名のリストをデータソースに設定
            comboCapPreset.DataSource = presets.ConvertAll(p => p.Name);

            // 選択インデックス決定
            int index = 0;
            if (oldValue != null)
            {
                index = Math.Max(0, comboCapPreset.Items.IndexOf(oldValue));
            }

            // 選択
            comboCapPreset.SelectedIndex = index;
        }

        /// <summary>
        /// 口パクプリセットコンボボックスの項目を選択する。
        /// </summary>
        /// <param name="name">選択する口パクプリセット名。</param>
        /// <returns>選択できたならば true 。</returns>
        private bool SelectLipPreset(string name)
        {
            var index = comboLipPreset.Items.IndexOf(name);
            if (index >= 0)
            {
                comboLipPreset.SelectedIndex = index;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 字幕プリセットコンボボックスの項目を選択する。
        /// </summary>
        /// <param name="name">選択する字幕プリセット名。</param>
        /// <returns>選択できたならば true 。</returns>
        private bool SelectCaptionPreset(string name)
        {
            var index = comboCapPreset.Items.IndexOf(name);
            if (index >= 0)
            {
                comboCapPreset.SelectedIndex = index;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 現在選択中の口パクプリセットコンボボックス項目を取得する。
        /// </summary>
        /// <returns>選択中の項目名。選択していない場合は null 。</returns>
        private string GetSelectedLipPresetName()
        {
            var index = comboLipPreset.SelectedIndex;
            if (index < 0)
            {
                return null;
            }
            return comboLipPreset.Items[index].ToString();
        }

        /// <summary>
        /// 現在選択中の字幕プリセットコンボボックス項目を取得する。
        /// </summary>
        /// <returns>選択中の項目名。選択していない場合は null 。</returns>
        private string GetSelectedCaptionPresetName()
        {
            var index = comboCapPreset.SelectedIndex;
            if (index < 0)
            {
                return null;
            }
            return comboCapPreset.Items[index].ToString();
        }

        /// <summary>
        /// 口パクプリセットを検索する。
        /// </summary>
        /// <returns>口パクプリセット。見つからなかった場合は null 。</returns>
        private lip.Preset FindLipPreset(string name)
        {
            return this.Config.LipPreset.Presets
                .FirstOrDefault(p => p.Name == name);
        }

        /// <summary>
        /// 字幕プリセットを検索する。
        /// </summary>
        /// <returns>字幕プリセット。見つからなかった場合は null 。</returns>
        private cap.Preset FindCaptionPreset(string name)
        {
            return this.Config.CaptionPreset.Presets
                .FirstOrDefault(p => p.Name == name);
        }

        /// <summary>
        /// 現在の設定を基にモーフ別タイムラインテーブルを作成する。
        /// </summary>
        /// <returns>モーフ別タイムラインテーブル。</returns>
        private MorphTimelineTable MakeTimelineTable()
        {
            // 口形状別タイムラインセット作成
            var tlSetMaker = new LipTimelineMaker();
            var tlSet = tlSetMaker.Make(this.LipSyncUnits);

            // 口パクモーフセット取得
            var preset = FindLipPreset(this.LipPresetName);
            var morphSet =
                (preset == null) ? (new LipMorphSet()) : preset.Value.Clone();

            // モーフ別タイムラインテーブル作成
            var tlTableMaker = new MorphTimelineMaker();
            bool morphEtoAI =
                (this.TargetModel != null && this.TargetModel.Morphs["え"] == null);
            var tlTable = tlTableMaker.Make(tlSet, morphSet, morphEtoAI);

            return tlTable;
        }

        /// <summary>
        /// フォームが閉じる際の口パク処理を行う。
        /// </summary>
        /// <param name="frameLength">
        /// 口パクフレーム長の設定先。口パクしないならば 0 が設定される。
        /// </param>
        /// <returns>確認メッセージ。確認不要ならば null 。</returns>
        private string ProcessLipOnFormClosing(out long frameLength)
        {
            string confirm = null;

            // 初期化
            this.MorphTimelineTable = null;
            frameLength = 0;

            if (this.LipEnabled)
            {
                // モーフ別タイムラインテーブル作成
                this.MorphTimelineTable = MakeTimelineTable();

                // 開始実フレーム 0 で実フレームテーブル作成
                var table = MakeMorphFrameTable(0);
                if (table.Count > 0)
                {
                    // 最大フレーム位置取得
                    frameLength =
                        table.Values.Max(frms => frms.Max(f => f.FrameNumber));

                    // 確認文作成
                    confirm =
                        @"口パクの総フレーム長は " + frameLength +
                        @" フレームになりそうです。";
                    if (
                        this.LipSpanType == FrameSpanType.All &&
                        this.LipSpanFrame < frameLength)
                    {
                        frameLength = this.LipSpanFrame;
                        confirm +=
                            Environment.NewLine +
                            frameLength +
                            @" フレームより後ろのキーフレームは無視されます。";
                    }
                }
                else
                {
                    // 確認文作成
                    if (this.LipSyncUnits.Count > 0)
                    {
                        confirm = @"口を開けることがないため、";
                    }
                    else
                    {
                        confirm = @"口パクデータが空のため、";
                    }
                    confirm += @"口パクのキーフレームは設定されません。";
                }
            }

            return confirm;
        }

        /// <summary>
        /// フォームが閉じる際の字幕処理を行う。
        /// </summary>
        /// <param name="lipFrameLength">
        /// 口パクのフレーム長。口パクしないならば 0 。
        /// </param>
        /// <returns>確認メッセージ。確認不要ならば null 。</returns>
        private string ProcessCaptionOnFormClosing(long lipFrameLength)
        {
            string confirm = null;

            // 初期化
            this.CaptionInfo = null;
            this.CaptionFrameLength = 0;
            this.CaptionFadeInLength = 0;
            this.CaptionFadeOutLength = 0;

            if (this.CaptionEnabled)
            {
                // 字幕情報設定
                var preset = FindCaptionPreset(this.CaptionPresetName);
                this.CaptionInfo = preset.Value.Clone();

                // XY位置上書き
                if (this.CaptionXYEnabled)
                {
                    this.CaptionInfo.X = this.CaptionX;
                    this.CaptionInfo.Y = this.CaptionY;
                }

                if (string.IsNullOrEmpty(this.CaptionText))
                {
                    // 確認文作成
                    confirm = @"入力文が空のため、字幕は表示されません。";
                }
                else
                {
                    // 字幕フレーム長決定
                    this.CaptionFrameLength = this.CaptionFixedSpanFrame;
                    if (this.CaptionLetterSpanFrame > 0)
                    {
                        // 改行を除く文字数分を加算
                        var letters =
                            new TextElementEnumerable(
                                RemoveCrLf(this.CaptionText));
                        this.CaptionFrameLength +=
                            this.CaptionLetterSpanFrame * letters.Count();
                    }

                    // 口パクのフレーム長より短くはならない
                    this.CaptionFrameLength =
                        Math.Max(
                            this.CaptionFrameLength,
                            Math.Max(lipFrameLength, 1L));

                    // フェードイン/アウトフレーム長決定
                    double fadeRate = 1;
                    var fadeTotal =
                        this.CaptionFadeInFrame + this.CaptionFadeOutFrame;
                    if (fadeTotal > this.CaptionFrameLength)
                    {
                        // フェード長合計がフレーム長を超える場合は
                        // 割合をそのままに短くする
                        fadeRate = (double)this.CaptionFrameLength / fadeTotal;
                    }
                    this.CaptionFadeInLength = this.CaptionFadeInFrame * fadeRate;
                    this.CaptionFadeOutLength = this.CaptionFadeOutFrame * fadeRate;

                    // 確認文作成
                    confirm =
                        @"字幕の総フレーム長は " + this.CaptionFrameLength +
                        @" フレームになります。";
                    if (fadeRate < 1)
                    {
                        if (this.CaptionFadeInLength > 0)
                        {
                            confirm +=
                                Environment.NewLine +
                                @"フェードインフレーム長は約 " +
                                this.CaptionFadeInLength.ToString("F1") +
                                @" フレームに縮められます。";
                        }
                        if (this.CaptionFadeOutLength > 0)
                        {
                            confirm +=
                                Environment.NewLine +
                                @"フェードアウトフレーム長は約 " +
                                this.CaptionFadeOutLength.ToString("F1") +
                                @" フレームに縮められます。";
                        }
                    }
                }
            }

            return confirm;
        }

        private void LipAndCapDialog_Load(object sender, EventArgs e)
        {
            _kanaConverting = false;

            // 口パク関連コントロールの有効状態を設定
            SetupLipControlsEnabled();

            // 設定値をコントロールに反映
            ApplyPropertyToControls();

            // テキストボックスをクリア
            textInput.Text = string.Empty;
            textLipKana.Text = string.Empty;
            DoClearLipSync();
            this.CaptionText = string.Empty;
        }

        private void LipAndCapDialog_Resize(object sender, EventArgs e)
        {
            // 最大化状態を設定
            this.Config.Basic.Maximized =
                (this.WindowState == FormWindowState.Maximized);

            if (this.WindowState == FormWindowState.Normal)
            {
                // ウィンドウが通常状態ならば現在のサイズを設定
                this.Config.Basic.Size = this.Size;
            }

            // 保存
            this.Config.SaveBasic();
        }

        private void LipAndCapDialog_FormClosing(
            object sender,
            FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK)
            {
                // コントロールから設定値更新
                UpdatePropertyFromControls();

                var confirms = new List<string>();

                // 口パク処理
                long lipLen = 0;
                if (this.TargetModel != null)
                {
                    var lc = ProcessLipOnFormClosing(out lipLen);
                    if (lc != null)
                    {
                        confirms.Add(lc);
                    }
                }

                // 字幕処理
                var cc = ProcessCaptionOnFormClosing(lipLen);
                if (cc != null)
                {
                    confirms.Add(cc);
                }

                // 確認ダイアログ文字列作成
                string text;
                if (confirms.Count > 0)
                {
                    confirms.Add(@"処理を実行してもよろしいですか？");
                    text =
                        string.Join(
                            Environment.NewLine + Environment.NewLine,
                            confirms);
                }
                else
                {
                    text = @"何も処理されませんがよろしいですか？";
                }

                // 確認ダイアログ表示
                var result =
                    MessageBox.Show(
                        this,
                        text,
                        this.Text,
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Question);
                if (result != DialogResult.OK)
                {
                    // OKが押されなかったら終了キャンセル
                    e.Cancel = true;
                }
            }
        }

        private void LipAndCapDialog_FormClosed(
            object sender,
            FormClosedEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK)
            {
                // 各種基本設定を保存
                this.Config.Basic.LipEnabled = this.LipEnabled;
                this.Config.Basic.LipPresetName = this.LipPresetName;
                this.Config.Basic.LipSpanType = this.LipSpanType;
                this.Config.Basic.LipSpanFrame = this.LipSpanFrame;
                this.Config.Basic.CaptionEnabled = this.CaptionEnabled;
                this.Config.Basic.CaptionPresetName = this.CaptionPresetName;
                this.Config.Basic.CaptionXYEnabled = this.CaptionXYEnabled;
                this.Config.Basic.CaptionX = this.CaptionX;
                this.Config.Basic.CaptionY = this.CaptionY;
                this.Config.Basic.CaptionFixedSpanFrame = this.CaptionFixedSpanFrame;
                this.Config.Basic.CaptionLetterSpanFrame =
                    this.CaptionLetterSpanFrame;
                this.Config.Basic.CaptionFadeInFrame = this.CaptionFadeInFrame;
                this.Config.Basic.CaptionFadeOutFrame = this.CaptionFadeOutFrame;
                this.Config.SaveBasic();

                // モデル口パクプリセット関連付けを設定して保存
                if (
                    this.TargetModel != null &&
                    (this.LipEnabled || this.CaptionEnabled))
                {
                    var relation = this.Config.ModelPresetRelation;
                    if (this.LipEnabled)
                    {
                        relation.Lip[this.TargetModel.GUID] = this.LipPresetName;
                    }
                    if (this.CaptionEnabled)
                    {
                        relation.Caption[this.TargetModel.GUID] =
                            this.CaptionPresetName;
                    }
                    this.Config.SaveModelPresetRelation();
                }
            }
            else
            {
                // 口パクデータをクリア
                DoClearLipSync();
            }
        }

        private void textInput_TextChanged(object sender, EventArgs e)
        {
            // 自動変換が有効なら変換
            if (this.LipAutoEnabled)
            {
                DoConvertToLipKana();
            }

            // 字幕文字列設定
            this.CaptionText = textInput.Text.Replace(Environment.NewLine, "\n");
        }

        private void buttonLipConv_Click(object sender, EventArgs e)
        {
            DoConvertToLipKana();
        }

        private void checkLipConv_CheckedChanged(object sender, EventArgs e)
        {
            // 自動変換設定を保存
            this.Config.Basic.LipAutoEnabled = this.LipAutoEnabled;
            this.Config.SaveBasic();

            // 有効になったのなら変換
            if (this.LipAutoEnabled)
            {
                DoConvertToLipKana();
            }
        }

        private void textLipKana_TextChanged(object sender, EventArgs e)
        {
            // 入力文からの変換による変更でなければ口パクデータ変換
            if (!_kanaConverting)
            {
                DoConvertToLipSync();
            }
        }

        private void buttonLipPreset_Click(object sender, EventArgs e)
        {
            // 口パクプリセット編集ダイアログ作成
            using (var dialog = new lip.PresetDialog(this.EnglishMode))
            {
                // パラメータ設定
                dialog.Presets = this.Config.LipPreset.Presets;
                dialog.InitialPreset = FindLipPreset(GetSelectedLipPresetName());

                // ダイアログ表示
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    // 口パクプリセット更新＆保存
                    this.Config.LipPreset.Presets = dialog.Presets;
                    this.Config.SaveLipPreset();

                    // コンボボックスに反映
                    DoUpdateLipPresetCombo();
                }
            }
        }

        private void buttonCapPreset_Click(object sender, EventArgs e)
        {
            // 字幕プリセット編集ダイアログ作成
            using (var dialog = new cap.PresetDialog(this.EnglishMode))
            {
                // パラメータ設定
                dialog.Presets = this.Config.CaptionPreset.Presets;
                dialog.InitialPreset =
                    FindCaptionPreset(GetSelectedCaptionPresetName());

                // ダイアログ表示
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    // 字幕プリセット更新＆保存
                    this.Config.CaptionPreset.Presets = dialog.Presets;
                    this.Config.SaveCaptionPreset();

                    // コンボボックスに反映
                    DoUpdateCaptionPresetCombo();
                }
            }
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            var ctrl = sender as TextBox;
            if (ctrl != null)
            {
                // Ctrlと同時に押されたキーを Tag に記録
                var key = Keys.None;
                if (e.Modifiers == Keys.Control)
                {
                    key = e.KeyCode;
                }
                ctrl.Tag = key;
            }
        }

        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            var ctrl = sender as TextBox;
            if (ctrl != null && ctrl.Tag is Keys)
            {
                // Ctrlと同時に押されたキーによって処理
                switch ((Keys)ctrl.Tag)
                {
                case Keys.Enter:
                    // Ctrl+EnterでOK投下
                    this.DialogResult = DialogResult.OK;
                    e.Handled = true;
                    break;

                case Keys.A:
                    // Ctrl+Aで全選択
                    ctrl.SelectAll();
                    e.Handled = true;
                    break;
                }

                // キー情報クリア
                ctrl.Tag = null;
            }
        }
    }
}
