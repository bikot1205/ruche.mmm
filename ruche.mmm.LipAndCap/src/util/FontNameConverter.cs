using System;
using System.ComponentModel;
using System.Drawing;

namespace ruche.mmm.util
{
    /// <summary>
    /// フォント名コンバータクラス。
    /// </summary>
    public sealed class FontNameConverter : StringConverter
    {
        /// <summary>
        /// フォント名コレクション。
        /// </summary>
        private static readonly StandardValuesCollection StandardValues =
            new StandardValuesCollection(
                Array.ConvertAll(FontFamily.Families, ff => ff.Name));

        public override bool GetStandardValuesSupported(
            ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool GetStandardValuesExclusive(
            ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(
            ITypeDescriptorContext context)
        {
            return StandardValues;
        }
    }
}
