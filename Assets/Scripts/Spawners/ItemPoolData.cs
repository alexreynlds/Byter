using UnityEngine;
using System.Collections.Generic;

[
    CreateAssetMenu(
        fileName = "ItemPoolData",
        menuName = "ItemPools/ItemPool",
        order = 0)
]
public class ItemPoolData : ScriptableObject
{
    public List<Spawnable> itemPool = new List<Spawnable>();
}
