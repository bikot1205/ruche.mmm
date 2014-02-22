using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;

namespace ruche.mmm.util
{
    /// <summary>
    /// HorizontalAlignment 値を名前文字列に変換するコンバータクラス。
    /// </summary>
    public sealed class HorizontalAlignmentConverter : EnumConverter
    {
        /// <summary>
        /// HorizontalAlignment 値と名前文字列の対応テーブル。
        /// </summary>
        private static readonly Dictionary<HorizontalAlignment, string> Names =
            new Dictionary<HorizontalAlignment, string>
            {
                { HorizontalAlignment.Left, "左揃え" },
                { HorizontalAlignment.Center, "中央揃え" },
                { HorizontalAlignment.Right, "右揃え" },
            };

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public HorizontalAlignmentConverter()
            : base(typeof(HorizontalAlignment))
        {
        }

        public override object ConvertFrom(
            ITypeDescriptorContext context,
            CultureInfo culture,
            object value)
        {
            var src = value as string;
            if (src != null)
            {
                var align = (
                    from kv in Names
                    where kv.Value == src
                    select (HorizontalAlignment?)kv.Key)
                    .FirstOrDefault();
                if (align.HasValue)
                {
                    return align.Value;
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
            if (value is HorizontalAlignment && destinationType == typeof(string))
            {
                string name;
                if (Names.TryGetValue((HorizontalAlignment)value, out name))
                {
                    return name;
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
