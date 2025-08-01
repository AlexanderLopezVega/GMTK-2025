using UnityEngine;

[CreateAssetMenu(menuName = "Project / Hook Config")]
public class HookConfig : ScriptableObject
{
	//	Inspector
	[field: Header("Movement")]
	[field: SerializeField, Min(0f)] public float MoveSpeed { get; private set; } = 3f;
	[field: SerializeField, Min(0f)] public float TileSize { get; private set; } = 1f;
	[field: SerializeField, Min(0f)] public int MaxDistance { get; private set; } = 10;
}
