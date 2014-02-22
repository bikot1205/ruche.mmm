using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ruche.mmm
{
    /// <summary>
    /// モデルのGUIDとプリセット名の関連付けテーブルクラス。
    /// </summary>
    [CollectionDataContract(ItemName = "Item", Namespace = "")]
    [KnownType(typeof(Guid))]
    public sealed class ModelPresetRelationTable : Dictionary<Guid, string>
    {
    }
}
