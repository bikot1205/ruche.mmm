using System;
using System.Windows.Forms;

namespace ruche.mmm.caption
{
    /// <summary>
    /// 字幕プリセットリスト編集ダイアログクラス。
    /// </summary>
    public partial class PresetDialog : Form
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="englishMode">英語表示モードならば true 。</param>
        public PresetDialog(bool englishMode)
        {
            this.EnglishMode = englishMode;

            InitializeComponent();

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
        /// 指定したプリセットでプロパティグリッドを更新する。
        /// </summary>
        /// <param name="preset">プリセット。</param>
        private void UpdatePropertyGrid(Preset preset)
        {
            try
            {
                propCaption.SuspendLayout();

                // プリセット名設定
                textPresetName.Text = preset.Name;

                // プロパティグリッドのソースを設定
                propCaption.SelectedObject = preset.Value.Clone();
            }
            finally
            {
                propCaption.ResumeLayout(true);
            }
        }

        /// <summary>
        /// プロパティグリッドの内容からプリセットを作成する。
        /// </summary>
        /// <returns>プリセット。プリセット名が不正な場合は null 。</returns>
        private Preset MakePresetFromPropertyGrid()
        {
            // プリセット名を取得
            var name = textPresetName.Text;
            if (!Preset.IsValidName(name))
            {
                return null;
            }

            // 字幕情報を取得
            var info = propCaption.SelectedObject as CaptionInfo;
            if (info == null)
            {
                return null;
            }

            // プリセットを作成
            var preset = new Preset(name, info.Clone());

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
                // プロパティグリッドを初期表示プリセットで初期化
                UpdatePropertyGrid(this.InitialPreset);

                // 既存のリストに同名プリセットがあればそれを選択
                int index = FindEditPresetIndex(this.InitialPreset.Name);
                if (index >= 0)
                {
                    listPreset.SelectedIndex = index;
                }
            }
            else
            {
                // プロパティグリッドを既定値で初期化
                UpdatePropertyGrid(new Preset());
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
                UpdatePropertyGrid(this.EditPresets[index]);
            }
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            var preset = MakePresetFromPropertyGrid();

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
