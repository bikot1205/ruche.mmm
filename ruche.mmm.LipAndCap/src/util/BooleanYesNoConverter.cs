using System;
using System.ComponentModel;
using System.Globalization;

namespace ruche.mmm.util
{
    /// <summary>
    /// 真偽値を "はい" または "いいえ" に変換するコンバータクラス。
    /// </summary>
    public sealed class BooleanYesNoConverter : BooleanConverter
    {
        /// <summary>
        /// true 値を表す文字列配列。
        /// </summary>
        private static readonly string[] YesStrings = { "はい", "Yes" };

        /// <summary>
        /// false 値を表す文字列配列。
        /// </summary>
        private static readonly string[] NoStrings = { "いいえ", "No" };

        public override object ConvertFrom(
            ITypeDescriptorContext context,
            CultureInfo culture,
            object value)
        {
            var src = value as string;
            if (src != null)
            {
                if (Array.IndexOf(YesStrings, src) >= 0)
                {
                    return true;
                }
                if (Array.IndexOf(NoStrings, src) >= 0)
                {
                    return false;
                }
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(
            ITypeDescriptorContext context,
            CultureInfo culture,
            object value,
            Type destinationType)
        {
            if (value is bool && destinationType == typeof(string))
            {
                return (bool)value ? YesStrings[0] : NoStrings[0];
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
