using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.Serialization;
using System.Windows.Forms;
using MikuMikuPlugin;
using DxMath;
using ruche.mmm.util;

namespace ruche.mmm.caption
{
    /// <summary>
    /// 字幕表示情報を保持するクラス。
    /// </summary>
    [DataContract(Namespace = "")]
    [DefaultProperty("X")]
    [TypeConverter(typeof(PropertyOrderConverter))]
    public sealed class CaptionInfo : ICloneable
    {
        /// <summary>
        /// 既定のフォントサイズ。
        /// </summary>
        private const float DefaultFontSize = 3.0f;

        /// <summary>
        /// 既定の縁取り幅。
        /// </summary>
        private const float DefaultEdgeWidth = 1.0f;

        /// <summary>
        /// 既定の影距離。
        /// </summary>
        private const float DefaultShadowDistance = 5.0f;

        /// <summary>
        /// Caption に設定すべきフォントサイズを算出する。
        /// </summary>
        /// <param name="fontSize">フォントサイズ設定値。</param>
        /// <returns>Caption に設定すべきフォントサイズ。</returns>
        private static float CalcCaptionFontSize(float fontSize)
        {
            return (fontSize * 4.8f);
        }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public CaptionInfo()
        {
            this.HorizontalAlignment = HorizontalAlignment.Left;
            this.FontName = FontFamily.GenericSansSerif.Name;
            this.FontSize = DefaultFontSize;
            this.TextColor = Color.Black;
            this.Alpha = 1.0f;
            this.EdgeColor = Color.White;
            this.EdgeWidth = DefaultEdgeWidth;
            this.ShadowDistance = DefaultShadowDistance;
        }

        #region 配置

        [Category("\t\t\t\t\t\t配置")]
        [DisplayName("X位置")]
        [Description("出力スクリーンサイズに対する字幕の左端位置。")]
        [DefaultValue(0.0f)]
        [PropertyOrder(1)]
        [DataMember]
        public float X { get; set; }

        [Category("\t\t\t\t\t\t配置")]
        [DisplayName("Y位置")]
        [Description("出力スクリーンサイズに対する字幕の上端位置。")]
        [DefaultValue(0.0f)]
        [PropertyOrder(2)]
        [DataMember]
        public float Y { get; set; }

        [Browsable(false)] // あまり意味のない設定なので表示しない
        [Category("\t\t\t\t\t\t配置")]
        [DisplayName("Zオーダ")]
        [Description("字幕のZオーダ。数値が大きいほど手前に表示される。")]
        [DefaultValue(0.0f)]
        [PropertyOrder(3)]
        [DataMember]
        public float Z { get; set; }

        [Category("\t\t\t\t\t\t配置")]
        [DisplayName("回転角度")]
        [Description("字幕の回転角度。 360 で1回転。")]
        [DefaultValue(0.0f)]
        [TypeConverter(typeof(SingleFixed1Converter))]
        [PropertyOrder(4)]
        [DataMember]
        public float Angle { get; set; }

        [Category("\t\t\t\t\t\t配置")]
        [DisplayName("水平揃え")]
        [Description("字幕の水平揃え。")]
        [DefaultValue(HorizontalAlignment.Left)]
        [TypeConverter(typeof(HorizontalAlignmentConverter))]
        [PropertyOrder(5)]
        [DataMember]
        public HorizontalAlignment HorizontalAlignment { get; set; }

        [Category("\t\t\t\t\t\t配置")]
        [DisplayName("行間")]
        [Description("出力スクリーンサイズに対する行間の幅。 0 で標準の幅。")]
        [DefaultValue(0.0f)]
        [TypeConverter(typeof(SingleFixed1Converter))]
        [PropertyOrder(6)]
        [DataMember]
        public float LineSpace { get; set; }

        [Category("\t\t\t\t\t\t配置")]
        [DisplayName("字間")]
        [Description("出力スクリーンサイズに対する文字間の幅。 0 で標準の幅。")]
        [DefaultValue(0.0f)]
        [TypeConverter(typeof(SingleFixed1Converter))]
        [PropertyOrder(7)]
        [DataMember]
        public float LetterSpace { get; set; }

        #endregion

        #region フォント

        [Category("\t\t\t\t\tフォント")]
        [DisplayName("フォント名")]
        [Description("文字のフォント名。")]
        [TypeConverter(typeof(FontNameConverter))]
        [PropertyOrder(101)]
        [DataMember]
        public string FontName { get; set; }

        [Category("\t\t\t\t\tフォント")]
        [DisplayName("フォントサイズ")]
        [Description("出力スクリーンサイズに対する文字の相対フォントサイズ。")]
        [DefaultValue(DefaultFontSize)]
        [TypeConverter(typeof(SingleFixed1Converter))]
        [PropertyOrder(102)]
        [DataMember]
        public float FontSize { get; set; }

        [Category("\t\t\t\t\tフォント")]
        [DisplayName("太字")]
        [Description("文字を太字で表示するか。")]
        [DefaultValue(false)]
        [TypeConverter(typeof(BooleanYesNoConverter))]
        [PropertyOrder(103)]
        [DataMember]
        public bool FontBold { get; set; }

        [Category("\t\t\t\t\tフォント")]
        [DisplayName("イタリック体")]
        [Description("文字をイタリック体で表示するか。")]
        [DefaultValue(false)]
        [TypeConverter(typeof(BooleanYesNoConverter))]
        [PropertyOrder(104)]
        [DataMember]
        public bool FontItalic { get; set; }

        [Category("\t\t\t\t\tフォント")]
        [DisplayName("アンダーライン")]
        [Description("文字にアンダーラインを表示するか。")]
        [DefaultValue(false)]
        [TypeConverter(typeof(BooleanYesNoConverter))]
        [PropertyOrder(105)]
        [DataMember]
        public bool FontUnderline { get; set; }

        [Category("\t\t\t\t\tフォント")]
        [DisplayName("取り消し線")]
        [Description("文字に取り消し線を表示するか。")]
        [DefaultValue(false)]
        [TypeConverter(typeof(BooleanYesNoConverter))]
        [PropertyOrder(106)]
        [DataMember]
        public bool FontStrike { get; set; }

        #endregion

        #region 色

        [Category("\t\t\t\t色")]
        [DisplayName("文字色")]
        [Description("文字色。")]
        [DefaultValue(typeof(Color), "Black")]
        [PropertyOrder(201)]
        [DataMember]
        public Color TextColor { get; set; }

        [Category("\t\t\t\t色")]
        [DisplayName("不透明度")]
        [Description("字幕全体の不透明度。")]
        [DefaultValue(1.0f)]
        [TypeConverter(typeof(SingleFixed2Converter))]
        [PropertyOrder(202)]
        [DataMember]
        public float Alpha
        {
            get { return _alpha; }
            set { _alpha = Math.Min(Math.Max(0.0f, value), 1.0f); }
        }
        private float _alpha = 1.0f;

        #endregion

        #region 縁取り

        [Category("\t\t\t縁取り")]
        [DisplayName("縁取り表示")]
        [Description("文字の縁取りを表示するか。")]
        [DefaultValue(false)]
        [TypeConverter(typeof(BooleanYesNoConverter))]
        [PropertyOrder(301)]
        [DataMember]
        public bool EdgeEnabled { get; set; }

        [Category("\t\t\t縁取り")]
        [DisplayName("縁取り色")]
        [Description("縁取りの色。")]
        [DefaultValue(typeof(Color), "White")]
        [PropertyOrder(302)]
        [DataMember]
        public Color EdgeColor { get; set; }

        [Category("\t\t\t縁取り")]
        [DisplayName("縁取り幅")]
        [Description("出力スクリーンサイズに対する縁取りの幅。")]
        [DefaultValue(DefaultEdgeWidth)]
        [TypeConverter(typeof(SingleFixed2Converter))]
        [PropertyOrder(303)]
        [DataMember]
        public float EdgeWidth { get; set; }

        #endregion

        #region 影

        [Category("\t\t影")]
        [DisplayName("影表示")]
        [Description("文字の影を表示するか。")]
        [DefaultValue(false)]
        [TypeConverter(typeof(BooleanYesNoConverter))]
        [PropertyOrder(401)]
        [DataMember]
        public bool ShadowEnabled { get; set; }

        [Category("\t\t影")]
        [DisplayName("影距離")]
        [Description("出力スクリーンサイズに対する影の距離。")]
        [DefaultValue(DefaultShadowDistance)]
        [TypeConverter(typeof(SingleFixed1Converter))]
        [PropertyOrder(402)]
        [DataMember]
        public float ShadowDistance { get; set; }

        #endregion

        /// <summary>
        /// インスタンスを Caption 値に変換する。
        /// </summary>
        /// <param name="scene">シーン。</param>
        /// <returns>Caption 値。</returns>
        public Caption ToCaption(Scene scene)
        {
            return ToCaption(
                scene.SystemInformation.OutputScreenSize,
                scene.ScreenSize);
        }

        /// <summary>
        /// インスタンスを Caption 値に変換する。
        /// </summary>
        /// <param name="outputSize">出力スクリーンサイズ。</param>
        /// <param name="screenSize">編集スクリーンサイズ。</param>
        /// <returns>Caption 値。</returns>
        public Caption ToCaption(Size outputSize, Size screenSize)
        {
            if (outputSize.Width <= 0 || outputSize.Height <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    "outputSize",
                    outputSize,
                    "出力スクリーンサイズ値が不正です。");
            }

            float rateX = (float)screenSize.Width / outputSize.Width;
            float rateY = (float)screenSize.Height / outputSize.Height;

            var dest = new Caption(string.Empty);

            dest.Location = new Vector3(this.X * rateX, this.Y * rateY, this.Z);
            dest.Rotate = this.Angle;
            dest.HorizontalAlignment = this.HorizontalAlignment;
            dest.LineSpace = this.LineSpace * rateY;
            dest.LetterSpace = this.LetterSpace * rateX;
            dest.FontFamily = new FontFamily(this.FontName);
            dest.FontSize = CalcCaptionFontSize(this.FontSize * rateY);
            dest.FontStyle =
                (this.FontBold ? FontStyle.Bold : 0) |
                (this.FontItalic ? FontStyle.Italic : 0) |
                (this.FontUnderline ? FontStyle.Underline : 0) |
                (this.FontStrike ? FontStyle.Strikeout : 0);
            dest.TextColor = this.TextColor;
            dest.Alpha = this.Alpha;
            dest.DrawTextBorder = this.EdgeEnabled;
            dest.TextBorderColor = this.EdgeColor;
            dest.TextBorderWeight = this.EdgeWidth * rateY;
            dest.DrawShadow = this.ShadowEnabled;
            dest.ShadowDistance = this.ShadowDistance * rateY;

            return dest;
        }

        /// <summary>
        /// 自身のクローンを作成する。
        /// </summary>
        /// <returns>自身のクローン。</returns>
        public CaptionInfo Clone()
        {
            var dest = new CaptionInfo();

            dest.X = this.X;
            dest.Y = this.Y;
            dest.Z = this.Z;
            dest.Angle = this.Angle;
            dest.HorizontalAlignment = this.HorizontalAlignment;
            dest.LineSpace = this.LineSpace;
            dest.LetterSpace = this.LetterSpace;
            dest.FontName = this.FontName;
            dest.FontSize = this.FontSize;
            dest.FontBold = this.FontBold;
            dest.FontItalic = this.FontItalic;
            dest.FontUnderline = this.FontUnderline;
            dest.FontStrike = this.FontStrike;
            dest.TextColor = this.TextColor;
            dest.Alpha = this.Alpha;
            dest.EdgeEnabled = this.EdgeEnabled;
            dest.EdgeColor = this.EdgeColor;
            dest.EdgeWidth = this.EdgeWidth;
            dest.ShadowEnabled = this.ShadowEnabled;
            dest.ShadowDistance = this.ShadowDistance;

            return dest;
        }

        #region ICloneable の明示的実装

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion
    }
}
