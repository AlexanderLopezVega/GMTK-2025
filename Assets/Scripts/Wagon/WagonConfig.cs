using UnityEngine;

[CreateAssetMenu(menuName = "Project / Wagon Config")]
public class WagonConfig : ScriptableObject
{
	[field: Header("Options")]
	[field: SerializeField, Min(0f)] public float MoveSpeed { get; private set; } = 0.5f; 
}
