using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/ItemData")]
public class ItemScriptableObject : ScriptableObject
{
    public string name;
    public int cost;
}
