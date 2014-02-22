using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MikuMikuPlugin;

namespace ruche.mmm
{
    /// <summary>
    /// 口パクモーションと字幕を生成するMikuMikuMovingプラグインクラス。
    /// </summary>
    public class LipAndCap : ICommandPlugin
    {
        /// <summary>
        /// 日本語のプラグイン名。
        /// </summary>
        public static readonly string PluginNameJpn = "口パク＆字幕";

        /// <summary>
        /// 英語のプラグイン名。
        /// </summary>
        public static readonly string PluginNameEng = "Lip & Caption";

        /// <summary>
        /// プラグイン名を取得する。
        /// </summary>
        /// <param name="englishMode">英語表示モードならば true 。</param>
        /// <returns>プラグイン名。</returns>
        public static string GetPluginName(bool englishMode)
        {
            return englishMode ? PluginNameEng : PluginNameJpn;
        }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public LipAndCap()
        {
        }

        /// <summary>
        /// 英語表示モードか否かを取得する。
        /// </summary>
        private bool EnglishMode
        {
            get
            {
                return (this.Scene == null) ?
                    false :
                    (this.Scene.Language == "en");
            }
        }

        /// <summary>
        /// プラグイン名を取得する。
        /// </summary>
        private string PluginName
        {
            get { return GetPluginName(this.EnglishMode); }
        }

        /// <summary>
        /// 現在選択中のモデルを取得する。
        /// </summary>
        private Model ActiveModel
        {
            get { return (this.Scene == null) ? null : this.Scene.ActiveModel; }
        }

        /// <summary>
        /// エラーダイアログを表示する。
        /// </summary>
        /// <param name="text">表示するテキスト。</param>
        /// <param name="fatal">致命的なエラーならば true 。</param>
        private void ShowErrorDialog(string text, bool fatal)
        {
            MessageBox.Show(
                this.ApplicationForm,
                text,
                this.PluginName,
                MessageBoxButtons.OK,
                fatal ? MessageBoxIcon.Error : MessageBoxIcon.Warning);
        }

        /// <summary>
        /// ダイアログの結果を基に口パクを設定する。
        /// </summary>
        /// <param name="model">設定対象のモデル。</param>
        /// <param name="dialog">呼び出し済みのダイアログ。</param>
        private void ApplyLip(Model model, LipAndCapDialog dialog)
        {
            if (!dialog.LipEnabled)
            {
                return;
            }

            // 実フレームデータテーブル作成
            var table = dialog.MakeMorphFrameTable(this.Scene.MarkerPosition);
            if (table == null)
            {
                return;
            }

            // 範囲外のキーフレームを削除
            if (dialog.LipSpanType == FrameSpanType.All)
            {
                var maxPos = this.Scene.MarkerPosition + dialog.LipSpanFrame;
                foreach (var frms in table.Values)
                {
                    frms.RemoveAll(f => (f.FrameNumber > maxPos));
                }
            }

            // キーフレームを設定
            foreach (var mf in table)
            {
                var morph = model.Morphs[mf.Key];
                if (morph != null)
                {
                    morph.Frames.AddKeyFrame(mf.Value);
                }
            }
        }

        /// <summary>
        /// ダイアログの結果を基に字幕を設定する。
        /// </summary>
        /// <param name="dialog">呼び出し済みのダイアログ。</param>
        private void ApplyCaption(LipAndCapDialog dialog)
        {
            if (!dialog.CaptionEnabled || dialog.CaptionFrameLength <= 0)
            {
                return;
            }

            // 字幕データ作成
            var caption =
                dialog.MakeCaption(this.Scene, this.Scene.MarkerPosition, 0);
            if (caption == null)
            {
                return;
            }

            // 字幕を設定
            this.Scene.Captions.AddCaption(caption);
        }

        #region ICommandPlugin 実装

        public Guid GUID
        {
            get { return new Guid("665055F6-DC92-4968-99FC-89989AF18746"); }
        }

        public IWin32Window ApplicationForm { get; set; }

        public Scene Scene { get; set; }

        public string Description
        {
            get
            {
                return this.EnglishMode ?
                    "Making lip-sync and a caption from the inputted text." :
                    "入力文から口パクモーフと字幕を生成し、選択位置に挿入します。";
            }
        }

        public Image Image
        {
            get { return Properties.Resources.Icon32; }
        }

        public Image SmallImage
        {
            get { return Properties.Resources.Icon20; }
        }

        public string Text
        {
            get { return PluginNameJpn; }
        }

        public string EnglishText
        {
            get { return PluginNameEng; }
        }

        public void Run(CommandArgs e)
        {
            e.Cancel = true;

            try
            {
                // 選択中モデル取得
                // 非選択で null の場合もある
                var model = this.ActiveModel;

                // 設定ダイアログ表示
                using (var dialog = new LipAndCapDialog(this.EnglishMode, model))
                {
                    dialog.Text = this.PluginName;

                    var result = dialog.ShowDialog(this.ApplicationForm);
                    if (result == DialogResult.OK)
                    {
                        // 口パク＆字幕設定
                        if (model != null)
                        {
                            ApplyLip(model, dialog);
                        }
                        ApplyCaption(dialog);

                        e.Cancel = false;
                    }
                }
            }
            catch (ApplicationException ex)
            {
                ShowErrorDialog(ex.Message, false);
            }
            catch (Exception ex)
            {
                ShowErrorDialog(ex.Message, true);
            }
        }

        public void Dispose()
        {
            // とくになし
        }

        #endregion
    }
}
