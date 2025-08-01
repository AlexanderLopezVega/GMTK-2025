using UnityEngine;

public class WagonMovement : Element
{
	//	Fields
	private readonly WagonConfig _wagonConfig;
	private readonly Rigidbody _rigidbody;
	private readonly Vector3 _direction;

	//	Constructors
	public WagonMovement(
		WagonConfig wagonConfig,
		Rigidbody rigidbody,
		Vector3 direction
	)
	{
		_wagonConfig = wagonConfig;
		_rigidbody = rigidbody;
		_direction = direction;
	}

	//	Properties
	public Vector3 Velocity => _wagonConfig.MoveSpeed * _direction;

	//	Methods
	protected override void OnEnabled()
	{
		_rigidbody.linearVelocity = Velocity;
	}
}
