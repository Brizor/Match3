using UnityEngine;

[CreateAssetMenu(menuName = "Assets bundle")]
public class ItemBundle : ScriptableObject
{
    [System.Serializable]
    public struct ViewItem
    {
        public HELPER.ITEMS type;
        public Sprite sprite;
    }
    public GameObject prefab;
}
