using UnityEngine;

public class RootLink : MonoBehaviour
{
	//	Inspector
	[field: Header("Dependencies")]
	[field: SerializeField] public Transform Root { get; private set; }
}
