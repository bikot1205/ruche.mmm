using System;
using System.Collections;
using System.ComponentModel;

namespace ruche.mmm.util
{
    /// <summary>
    /// プロパティのオーダを指定する属性。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyOrderAttribute : Attribute
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="order">オーダ値。</param>
        public PropertyOrderAttribute(int order)
        {
            this.Order = order;
        }

        /// <summary>
        /// オーダ値を取得する。
        /// </summary>
        public int Order { get; private set; }
    }

    /// <summary>
    /// PropertyOrderAttribute 属性のオーダ値による比較を行うクラス。
    /// </summary>
    internal class PropertyOrderComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            var px = x as PropertyDescriptor;
            var py = y as PropertyDescriptor;
            if (px == null || py == null)
            {
                return (px != null) ? -1 : ((py != null) ? 1 : 0);
            }

            var attrX =
                px.Attributes[typeof(PropertyOrderAttribute)]
                    as PropertyOrderAttribute;
            var attrY =
                py.Attributes[typeof(PropertyOrderAttribute)]
                    as PropertyOrderAttribute;
            if (attrX == null || attrY == null)
            {
                return (attrX != null) ? -1 : ((attrY != null) ? 1 : 0);
            }

            return (attrX.Order - attrY.Order);
        }
    }

    /// <summary>
    /// プロパティに PropertyOrderAttribute 属性を利用するクラスの
    /// TypeConverterAttribute 属性に引数として指定すべきクラス。
    /// </summary>
    public class PropertyOrderConverter : TypeConverter
    {
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override PropertyDescriptorCollection GetProperties(
            ITypeDescriptorContext context,
            object value,
            Attribute[] attributes)
        {
            var props = TypeDescriptor.GetProperties(value, attributes, true);
            return props.Sort(new PropertyOrderComparer());
        }
    }
}
