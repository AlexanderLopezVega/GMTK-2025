using UnityEngine;

public class Wagon : MonoBehaviour
{
	//	Inspector
	[Header("Dependencies")]
	[SerializeField] private Rigidbody _rigidbody;

	[Header("Options")]
	[SerializeField] private WagonConfig _wagonConfig;

	//	Fields
	private WagonMovement _wagonMovement;

	//	Properties
	public WagonMovement Movement => _wagonMovement;

	//	Methods
	public void Awake()
	{
		_wagonMovement = new WagonMovement(
			_wagonConfig,
			_rigidbody,
			Vector3.up
		);
	}
	private void OnEnable()
	{
		_wagonMovement.Enable();
	}
	private void OnDisable()
	{
		_wagonMovement.Disable();
	}
}
