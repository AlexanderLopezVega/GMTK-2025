using UnityEngine;

[CreateAssetMenu(menuName = "Project / Player Config")]
public class PlayerConfig : ScriptableObject
{
	//	Inspector
	[field: Header("Movement")]
	[field: SerializeField, Min(0f)] public float MoveSpeed { get; private set; } = 3f;
}
