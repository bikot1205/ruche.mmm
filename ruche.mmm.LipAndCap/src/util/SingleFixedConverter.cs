using System;
using System.ComponentModel;
using System.Globalization;

namespace ruche.mmm.util
{
    /// <summary>
    /// Single 値と小数点以下最小桁数を持つ文字列とのコンバータクラス。
    /// </summary>
    public abstract class SingleFixedConverter : SingleConverter
    {
        /// <summary>
        /// 小数点以下の最小桁数を取得する。
        /// </summary>
        public abstract int FixedDigits { get; }

        public override sealed object ConvertFrom(
            ITypeDescriptorContext context,
            CultureInfo culture,
            object value)
        {
            var s = value as string;
            if (s != null)
            {
                float dest = 0;
                if (float.TryParse(s, out dest))
                {
                    return dest;
                }
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override sealed object ConvertTo(
            ITypeDescriptorContext context,
            CultureInfo culture,
            object value,
            Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                var s = ((float)value).ToString("F9", culture).TrimEnd('0');
                var dotPos = s.IndexOf('.');
                if (dotPos < 0)
                {
                    s += ".";
                    s += new string('0', this.FixedDigits);
                }
                else
                {
                    var n = this.FixedDigits - (s.Length - dotPos - 1);
                    if (n > 0)
                    {
                        s += new string('0', n);
                    }
                }
                return s;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    /// <summary>
    /// Single 値と小数点以下最小1桁の文字列とのコンバータクラス。
    /// </summary>
    public sealed class SingleFixed1Converter : SingleFixedConverter
    {
        public override int FixedDigits
        {
            get { return 1; }
        }
    }

    /// <summary>
    /// Single 値と小数点以下最小2桁の文字列とのコンバータクラス。
    /// </summary>
    public sealed class SingleFixed2Converter : SingleFixedConverter
    {
        public override int FixedDigits
        {
            get { return 2; }
        }
    }
}
