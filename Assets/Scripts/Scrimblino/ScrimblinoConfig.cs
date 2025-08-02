using UnityEngine;

[CreateAssetMenu(menuName = "Project / Scrimblino Config")]
public class ScrimblinoConfig : ScriptableObject
{
	//	Inspector
	[field: Header("Movement")]
	[field: SerializeField, Min(0f)] public float MoveSpeed { get; private set; } = 3f;
	[field: SerializeField, Min(0f)] public float HookMoveSpeed { get; private set; } = 3f;
}
