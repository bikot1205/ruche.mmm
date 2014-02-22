using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ruche.mmm.lip
{
    /// <summary>
    /// 口パクプリセットリスト編集ダイアログクラス。
    /// </summary>
    public partial class PresetDialog : Form
    {
        /// <summary>
        /// モーフ設定に用いるコントロールを保持する構造体。
        /// </summary>
        private struct MorphCtrlData
        {
            /// <summary>
            /// モーフ名テキストボックスを取得または設定する。
            /// </summary>
            public TextBox NameTextBox { get; set; }

            /// <summary>
            /// モーフウェイト値アップダウンコントロールを取得または設定する。
            /// </summary>
            public NumericUpDown WeightUpDown { get; set; }
        }

        /// <summary>
        /// 口形状種別ごとのモーフコントロール個数。
        /// </summary>
        private static readonly int MorphCtrlCount = 10;

        /// <summary>
        /// モーフコントロール追加開始行位置。
        /// </summary>
        private static readonly int MorphCtrlBeginRow = 2;

        /// <summary>
        /// モーフ名テキストボックスの追加先カラム位置。
        /// </summary>
        private static readonly int NameTextBoxColumn = 0;

        /// <summary>
        /// モーフウェイト値アップダウンコントロールの追加先カラム位置。
        /// </summary>
        private static readonly int WeightUpDownColumn = 2;

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="englishMode">英語表示モードならば true 。</param>
        public PresetDialog(bool englishMode)
        {
            this.EnglishMode = englishMode;

            InitializeComponent();
            InitializeTabPages();

            this.DefaultCaption = this.Text;
        }

        /// <summary>
        /// 英語表示モードか否かを取得する。
        /// ただし現時点では英語表示に対応していない。
        /// </summary>
        public bool EnglishMode { get; private set; }

        /// <summary>
        /// プリセットリストを取得または設定する。
        /// </summary>
        public PresetList Presets
        {
            get { return _presets; }
            set { _presets = value ?? new PresetList(); }
        }
        private PresetList _presets = new PresetList();

        /// <summary>
        /// 初期表示プリセットを取得または設定する。
        /// </summary>
        public Preset InitialPreset { get; set; }

        /// <summary>
        /// ダイアログでの編集に用いるプリセットリストを取得または設定する。
        /// </summary>
        private PresetList EditPresets { get; set; }

        /// <summary>
        /// 口形状種別IDに対応するコントロールのテーブルを取得または設定する。
        /// </summary>
        private Dictionary<LipId, List<MorphCtrlData>> MorphCtrlTable { get; set; }

        /// <summary>
        /// 既定のダイアログキャプションを取得または設定する。
        /// </summary>
        private string DefaultCaption { get; set; }

        /// <summary>
        /// データ内容変更フラグを取得または設定する。
        /// </summary>
        private bool Modified
        {
            get { return _modified; }
            set
            {
                if (value != _modified)
                {
                    _modified = value;

                    // 変更されたならキャプションに "*" を付ける
                    this.Text = this.DefaultCaption;
                    if (value)
                    {
                        this.Text += " *";
                    }
                }
            }
        }
        private bool _modified = false;

        /// <summary>
        /// タブページの初期化を行う。
        /// </summary>
        private void InitializeTabPages()
        {
            try
            {
                this.tabLip.SuspendLayout();

                // コントロールテーブル初期化
                this.MorphCtrlTable = new Dictionary<LipId, List<MorphCtrlData>>();

                // タブページパネル初期化
                InitializeTabPagePanel(LipId.A, this.panelPageA);
                InitializeTabPagePanel(LipId.I, this.panelPageI);
                InitializeTabPagePanel(LipId.U, this.panelPageU);
                InitializeTabPagePanel(LipId.E, this.panelPageE);
                InitializeTabPagePanel(LipId.O, this.panelPageO);
            }
            finally
            {
                this.tabLip.ResumeLayout(true);
            }
        }

        /// <summary>
        /// タブページパネルの初期化を行う。
        /// </summary>
        /// <param name="lipId">口形状種別ID。</param>
        /// <param name="panel">パネル。</param>
        private void InitializeTabPagePanel(
            LipId lipId,
            TableLayoutPanel panel)
        {
            try
            {
                panel.SuspendLayout();

                // 規定個数だけ追加
                var ctrlDatas = new List<MorphCtrlData>(MorphCtrlCount);
                for (int i = 0; i < MorphCtrlCount; ++i)
                {
                    // コントロール作成
                    var ctrlData =
                        new MorphCtrlData
                        {
                            NameTextBox = CreateMorphNameTextBox(),
                            WeightUpDown = CreateMorphWeightUpDown(),
                        };

                    // タブインデックス設定
                    ctrlData.NameTextBox.TabIndex = i * 2;
                    ctrlData.WeightUpDown.TabIndex = i * 2 + 1;

                    // パネルに追加
                    int row = MorphCtrlBeginRow + i;
                    panel.Controls.Add(
                        ctrlData.NameTextBox,
                        NameTextBoxColumn,
                        row);
                    panel.Controls.Add(
                        ctrlData.WeightUpDown,
                        WeightUpDownColumn,
                        row);

                    // リストに追加
                    ctrlDatas.Add(ctrlData);
                }

                // テーブルに追加
                this.MorphCtrlTable.Add(lipId, ctrlDatas);
            }
            finally
            {
                panel.ResumeLayout(false);
            }
        }

        /// <summary>
        /// モーフ名を入力するテキストボックスを作成する。
        /// </summary>
        /// <returns>テキストボックス。</returns>
        private TextBox CreateMorphNameTextBox()
        {
            var c = new TextBox();

            c.Dock = DockStyle.Fill;
            c.BorderStyle = BorderStyle.FixedSingle;

            return c;
        }

        /// <summary>
        /// モーフウェイト値を入力するアップダウンコントロールを作成する。
        /// </summary>
        /// <returns>アップダウンコントロール。</returns>
        private NumericUpDown CreateMorphWeightUpDown()
        {
            var c = new NumericUpDown();

            c.DecimalPlaces = 2;
            c.Minimum = 0;
            c.Maximum = 2;
            c.Increment = 0.1m;
            c.Value = 1;

            c.Dock = DockStyle.Fill;
            c.BorderStyle = BorderStyle.FixedSingle;
            c.TextAlign = HorizontalAlignment.Right;

            return c;
        }

        /// <summary>
        /// プリセットリストボックスを更新する。
        /// </summary>
        private void UpdatePresetListBox()
        {
            // 編集用プリセットが null のうちは処理しない
            if (this.EditPresets == null)
            {
                return;
            }

            // 変更フラグON
            this.Modified = true;

            // プリセットが空なら既定値を追加
            if (this.EditPresets.Count <= 0)
            {
                this.EditPresets.Add(new Preset());
            }

            // 現在の選択項目があればその値を保持
            var oldIndex = listPreset.SelectedIndex;
            string oldName =
                (oldIndex < 0) ? null : listPreset.Items[oldIndex].ToString();

            // プリセット名のリストをデータソースに設定
            listPreset.DataSource = this.EditPresets.ConvertAll(p => p.Name);

            // 選択インデックス決定
            int index = oldIndex;
            if (oldName != null)
            {
                var curIndex = listPreset.Items.IndexOf(oldName);
                if (curIndex >= 0)
                {
                    index = curIndex;
                }
            }
            index = Math.Min(Math.Max(0, index), listPreset.Items.Count - 1);

            // 選択
            listPreset.SelectedIndex = index;
        }

        /// <summary>
        /// 指定したプリセットでタブページを更新する。
        /// </summary>
        /// <param name="preset">プリセット。</param>
        private void UpdateTabPages(Preset preset)
        {
            try
            {
                tabLip.SuspendLayout();

                // プリセット名設定
                textPresetName.Text = preset.Name;

                // タブページ更新
                foreach (var idCtrl in this.MorphCtrlTable)
                {
                    var weights = preset.Value[idCtrl.Key].MorphWeights;
                    var ctrls =
                        idCtrl.Value.Select((ctrl, index) => new { ctrl, index });
                    foreach (var c in ctrls)
                    {
                        if (c.index < weights.Count)
                        {
                            // モーフ名とウェイト値を設定
                            var w = weights[c.index];
                            c.ctrl.NameTextBox.Text = w.MorphName;
                            c.ctrl.WeightUpDown.Value = (decimal)w.Weight;
                        }
                        else
                        {
                            // データが無いので既定値にしておく
                            c.ctrl.NameTextBox.Text = string.Empty;
                            c.ctrl.WeightUpDown.Value = 1;
                        }
                    }
                }
            }
            finally
            {
                tabLip.ResumeLayout(true);
            }
        }

        /// <summary>
        /// タブページの内容からプリセットを作成する。
        /// </summary>
        /// <returns>プリセット。プリセット名が不正な場合は null 。</returns>
        private Preset MakePresetFromTabPages()
        {
            // プリセット名を取得
            var name = textPresetName.Text;
            if (!Preset.IsValidName(name))
            {
                return null;
            }

            // モーフ情報テーブルを作成
            var infoTable = new Dictionary<LipId, MorphInfo>();
            foreach (var idCtrl in this.MorphCtrlTable)
            {
                // タブページからモーフウェイトデータリストを作成
                var weights = new List<MorphWeightData>();
                foreach (var ctrl in idCtrl.Value)
                {
                    if (!string.IsNullOrEmpty(ctrl.NameTextBox.Text))
                    {
                        weights.Add(
                            new MorphWeightData
                            {
                                MorphName = ctrl.NameTextBox.Text,
                                Weight = (float)ctrl.WeightUpDown.Value,
                            });
                    }
                }

                // モーフ情報を追加
                infoTable.Add(idCtrl.Key, new MorphInfo(weights));
            }

            // プリセットを作成
            var preset = new Preset(name, new LipMorphSet(infoTable));

            return preset;
        }

        /// <summary>
        /// Yes/Noダイアログを表示する。
        /// </summary>
        /// <param name="text">表示するテキスト。</param>
        /// <returns>Yesが選択されたならば true 。</returns>
        private bool ShowYesNoDialog(string text)
        {
            var result =
                MessageBox.Show(
                    this,
                    text,
                    this.DefaultCaption,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

            return (result == DialogResult.Yes);
        }

        /// <summary>
        /// エラーダイアログを表示する。
        /// </summary>
        /// <param name="text">表示するテキスト。</param>
        /// <param name="fatal">致命的なエラーならば true 。</param>
        private void ShowErrorDialog(string text, bool fatal)
        {
            MessageBox.Show(
                this,
                text,
                this.DefaultCaption,
                MessageBoxButtons.OK,
                fatal ? MessageBoxIcon.Error : MessageBoxIcon.Warning);
        }

        /// <summary>
        /// プリセット名から編集用プリセット位置を検索する。
        /// </summary>
        /// <param name="name">プリセット名。</param>
        /// <returns>編集用プリセット位置。見つからなかった場合は -1 。</returns>
        private int FindEditPresetIndex(string name)
        {
            if (this.EditPresets != null)
            {
                return this.EditPresets.FindIndex(p => p.Name == name);
            }
            return -1;
        }

        private void PresetDialog_Load(object sender, EventArgs e)
        {
            // 編集用プリセットリストにクローンをコピー
            this.EditPresets = this.Presets.Clone();

            // プリセットリストボックス更新
            UpdatePresetListBox();

            // 初期表示プリセットがあるか？
            if (this.InitialPreset != null)
            {
                // タブページを初期表示プリセットで初期化
                UpdateTabPages(this.InitialPreset);

                // 既存のリストに同名プリセットがあればそれを選択
                int index = FindEditPresetIndex(this.InitialPreset.Name);
                if (index >= 0)
                {
                    listPreset.SelectedIndex = index;
                }
            }
            else
            {
                // タブページを既定値で初期化
                UpdateTabPages(new Preset());
            }

            // 変更フラグをここで一旦下ろす
            this.Modified = false;
        }

        private void PresetDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult != DialogResult.OK && this.Modified)
            {
                var result =
                    MessageBox.Show(
                        this,
                        @"プリセットリストは変更されています。" +
                        Environment.NewLine +
                        @"変更を反映せずに終了してよろしいですか？",
                        this.Text,
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Question);
                if (result != DialogResult.OK)
                {
                    e.Cancel = true;
                }
            }
        }

        private void PresetDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK)
            {
                // 編集内容を反映
                this.Presets = this.EditPresets;
            }
            this.EditPresets = null;
        }

        private void listPreset_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.EditPresets != null)
            {
                int index = listPreset.SelectedIndex;
                bool selected = (index >= 0);

                buttonUp.Enabled = (selected && index > 0);
                buttonDown.Enabled =
                    (selected && index < this.EditPresets.Count - 1);
                buttonEdit.Enabled = selected;
                buttonRemove.Enabled = selected;
            }
        }

        private void buttonUp_Click(object sender, EventArgs e)
        {
            int index = listPreset.SelectedIndex;
            if (index > 0 && index < this.EditPresets.Count)
            {
                var preset = this.EditPresets[index];
                this.EditPresets.RemoveAt(index);
                this.EditPresets.Insert(index - 1, preset);

                UpdatePresetListBox();
            }
        }

        private void buttonDown_Click(object sender, EventArgs e)
        {
            int index = listPreset.SelectedIndex;
            if (index >= 0 && index < this.EditPresets.Count - 1)
            {
                var preset = this.EditPresets[index];
                this.EditPresets.RemoveAt(index);
                this.EditPresets.Insert(index + 1, preset);

                UpdatePresetListBox();
            }
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            int index = listPreset.SelectedIndex;
            if (index >= 0 && index < this.EditPresets.Count)
            {
                UpdateTabPages(this.EditPresets[index]);
            }
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            var preset = MakePresetFromTabPages();

            if (preset == null)
            {
                ShowErrorDialog(
                    @"プリセット名が不正です。" + Environment.NewLine +
                    @"空の名前や空白文字のみの名前は付けられません。",
                    false);
                return;
            }

            int pos = FindEditPresetIndex(preset.Name);
            if (pos < 0)
            {
                // 存在しないので新規追加
                this.EditPresets.Add(preset);

                UpdatePresetListBox();
            }
            else
            {
                // 同名プリセットが存在するので置換確認
                bool yes =
                    ShowYesNoDialog(
                        @"プリセット " + preset.Name + @" は既に存在します。" +
                        Environment.NewLine +
                        @"置き換えますか？");
                if (yes)
                {
                    this.EditPresets[pos] = preset;

                    UpdatePresetListBox();
                }
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            int index = listPreset.SelectedIndex;
            if (index >= 0 && index < this.EditPresets.Count)
            {
                var preset = this.EditPresets[index];
                bool yes =
                    ShowYesNoDialog(
                        @"プリセット " + preset.Name + @" を削除します。" +
                        Environment.NewLine +
                        @"よろしいですか？");
                if (yes)
                {
                    this.EditPresets.RemoveAt(index);

                    UpdatePresetListBox();
                }
            }
        }
    }
}
