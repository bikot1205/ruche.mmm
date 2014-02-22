using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Xml;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;

namespace ruche.mmm.util
{
    /// <summary>
    /// プロパティが設定データではないことを表す属性。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class NoConfigValueAttribute : Attribute
    {
    }

    /// <summary>
    /// 設定データの保持とその読み書きを行うクラス。
    /// </summary>
    /// <typeparam name="TConfig">設定データ型。</typeparam>
    public class ConfigKeeper<TConfig>
        where TConfig : class, new()
    {
        /// <summary>
        /// 読み書き用のハッシュテーブルクラス。
        /// </summary>
        [CollectionDataContract(
            Name = "Config",
            ItemName = "Item",
            Namespace = "")]
        private sealed class PropertyTable : Dictionary<string, object>
        {
        }

        /// <summary>
        /// プロパティの getter, setter メソッドを保持するクラス。
        /// </summary>
        private sealed class PropertyAccessor
        {
            public Func<TConfig, object> Getter { get; set; }
            public Action<TConfig, object> Setter { get; set; }
        }

        /// <summary>
        /// 保持対象プロパティのアクセッサテーブル。
        /// </summary>
        private static readonly
        Dictionary<string, PropertyAccessor> _propAccessors =
            new Dictionary<string,PropertyAccessor>();

        /// <summary>
        /// 既知の型の配列。
        /// </summary>
        private static readonly Type[] _knownTypes;

        /// <summary>
        /// 静的コンストラクタ。
        /// </summary>
        static ConfigKeeper()
        {
            // TConfig インスタンスの式木パラメータ作成
            var dataExp = Expression.Parameter(typeof(TConfig), "data");

            // 公開プロパティの getter, setter を作成
            var knownTypes = new List<Type>();
            foreach (var info in typeof(TConfig).GetProperties())
            {
                // NoConfigValue 属性付きプロパティは対象外
                if (Attribute.IsDefined(info, typeof(NoConfigValueAttribute), true))
                {
                    continue;
                }

                // getter, setter の MethodInfo を取得
                var getterInfo = info.GetGetMethod();
                var setterInfo = info.GetSetMethod();
                if (getterInfo == null || setterInfo == null)
                {
                    // 片方しか公開されていないプロパティは対象外
                    continue;
                }
                if (getterInfo.IsStatic || setterInfo.IsStatic)
                {
                    // static プロパティは対象外
                    continue;
                }

                // プロパティの式木作成
                var propExp = Expression.Property(dataExp, info.Name);

                // getter 作成
                // data => (object)data.Property;
                var getterExp =
                    Expression.Lambda<Func<TConfig, object>>(
                        Expression.Convert(propExp, typeof(object)),
                        dataExp);
                var getter = getterExp.Compile();

                // setter 作成
                // (data, value) =>
                // {
                //     if (data.Property is PropertyType)
                //     {
                //         data.Property = (PropertyType)value;
                //     }
                // };
                var valueExp = Expression.Parameter(typeof(object), "value");
                var setterBodyExp =
                    Expression.IfThen(
                        Expression.TypeIs(valueExp, info.PropertyType),
                        Expression.Assign(
                            propExp,
                            Expression.Convert(valueExp, info.PropertyType)));
                var setterExp =
                    Expression.Lambda<Action<TConfig, object>>(
                        setterBodyExp,
                        dataExp,
                        valueExp);
                var setter = setterExp.Compile();

                // アクセッサテーブルに追加
                _propAccessors.Add(
                    info.Name,
                    new PropertyAccessor { Getter = getter, Setter = setter });

                // 既知の型を追加
                if (!knownTypes.Contains(info.PropertyType))
                {
                    knownTypes.Add(info.PropertyType);
                }
            }

            // 既知の型の配列を設定
            _knownTypes = knownTypes.ToArray();
        }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="filePath">読み書き先のファイルパス。</param>
        public ConfigKeeper(string filePath)
        {
            this.Data = new TConfig();
            this.FilePath = filePath;
        }

        /// <summary>
        /// 設定データを取得または設定する。
        /// </summary>
        public TConfig Data
        {
            get { return _config; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Data");
                }
                _config = value;
            }
        }
        private TConfig _config;

        /// <summary>
        /// 読み書き先のファイルパスを取得する。
        /// </summary>
        public string FilePath { get; private set; }

        /// <summary>
        /// 設定データをロードする。
        /// </summary>
        /// <returns>
        /// ロードに成功したならば true 。そうでなければ false 。
        /// </returns>
        public bool Load()
        {
            bool result = false;

            if (File.Exists(this.FilePath))
            {
                try
                {
                    // デシリアライズ
                    using (
                        var fs = new FileStream(
                            this.FilePath, FileMode.Open, FileAccess.Read))
                    {
                        result = DoDeserialize(fs);
                    }
                }
                catch
                {
                    result = false;
                }
            }

            return result;
        }

        /// <summary>
        /// 設定データをセーブする。
        /// </summary>
        /// <returns>
        /// セーブに成功したならば true 。そうでなければ false 。
        /// </returns>
        public bool Save()
        {
            try
            {
                // ディレクトリが存在しなければ作成
                var dirPath = Path.GetDirectoryName(this.FilePath);
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

                // シリアライズ
                using (
                    var fs = new FileStream(
                        this.FilePath, FileMode.Create, FileAccess.Write))
                {
                    DoSerialize(fs);
                }
            }
            catch
            {
#if DEBUG
                throw;
#else
                return false;
#endif
            }

            return true;
        }

        /// <summary>
        /// シリアライズ処理を行う。
        /// </summary>
        /// <param name="stream">書き出し先ストリーム。</param>
        private void DoSerialize(Stream stream)
        {
            using (
                var writer = XmlWriter.Create(
                    stream,
                    new XmlWriterSettings { Indent = true }))
            {
                var serializer = MakeSerializer();
                var table = MakePropertyTable();
                serializer.WriteObject(writer, table);
            }
        }

        /// <summary>
        /// デシリアライズ処理を行う。
        /// </summary>
        /// <param name="stream">読み取り元ストリーム。</param>
        /// <returns>
        /// 成功したならば true 。そうでなければ false 。
        /// </returns>
        private bool DoDeserialize(Stream stream)
        {
            using (var reader = XmlReader.Create(stream))
            {
                var serializer = MakeSerializer();
                var table = serializer.ReadObject(reader) as PropertyTable;
                if (table != null)
                {
                    ApplyPropertyTable(table);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// シリアライザを生成する。
        /// </summary>
        /// <returns>シリアライザ。</returns>
        private XmlObjectSerializer MakeSerializer()
        {
            return new DataContractSerializer(
                typeof(PropertyTable),
                _knownTypes);
        }

        /// <summary>
        /// 現在の設定値からハッシュテーブルを作成する。
        /// </summary>
        /// <returns>ハッシュテーブル。</returns>
        private PropertyTable MakePropertyTable()
        {
            var table = new PropertyTable();

            foreach (var pa in _propAccessors)
            {
                table[pa.Key] = pa.Value.Getter(this.Data);
            }

            return table;
        }

        /// <summary>
        /// ハッシュテーブルの内容で現在の設定値を上書きする。
        /// </summary>
        /// <param name="table">ハッシュテーブル。</param>
        private void ApplyPropertyTable(PropertyTable table)
        {
            if (table != null)
            {
                foreach (var pa in _propAccessors)
                {
                    if (table.ContainsKey(pa.Key))
                    {
                        pa.Value.Setter(this.Data, table[pa.Key]);
                    }
                }
            }
        }
    }
}
