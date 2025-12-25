using RG.DataTables;
using UnityEngine;
public class Item : DataTableEntryBase
{
    public string displayName = "<NotSet>";
    public string description;
    public GameObject prefab;
    public bool isStackable;
    public int maxStackSize;

    // Uncomment the following lines to override sub asset naming (default is the id)
    protected override string ResolveAssetName() { return $"{id.ToString("X")}_{displayName.Replace(" ", "_")}"; }
}