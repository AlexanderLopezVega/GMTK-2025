using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class ScrimblinoMovement : Element
{
	//	Fields
	private readonly ScrimblinoConfig _playerConfig;
	private readonly Input _input;
	private readonly Rigidbody _rigidbody;
	private Vector3 _wagonVelocity;
	private int _numWagons;
	private Vector2 _moveInput;

	//	Constructors
	public ScrimblinoMovement(
		ScrimblinoConfig playerConfig,
		Input input,
		Rigidbody rigidbody
	)
	{
		_playerConfig = playerConfig;
		_input = input;
		_rigidbody = rigidbody;
	}

	//	Methods
	public void Update(float deltaTime)
	{
		_rigidbody.linearVelocity =
			_wagonVelocity +
			_playerConfig.MoveSpeed * new Vector3(
				_moveInput.x,
				_moveInput.y
			);
	}
	public void OnEnterWagon(Vector3 linearVelocity)
	{
		++_numWagons;
		_wagonVelocity = (_wagonVelocity + linearVelocity) / _numWagons;
	}
	public void OnExitWagon(Vector3 linearVelocity)
	{
		_wagonVelocity = _wagonVelocity * _numWagons - linearVelocity;
		--_numWagons;

		if (_numWagons > 1)
			_wagonVelocity /= _numWagons - 1;
	}
	protected override void OnEnabled()
	{
		_input.OnMove += OnMove;
	}
	protected override void OnDisabled()
	{
		_input.OnMove -= OnMove;
		//	TODO: When MonoBehaviour is disabled, Update won't be called.
		//	Fix somehow to get Enabled/Disabled functionality without 
		//	stopping Update from being called.
		_rigidbody.linearVelocity = _wagonVelocity;
	}

	private void OnMove(CallbackContext context)
	{
		Vector2 newMoveInput;

		if (context.started)
			return;

		newMoveInput = context.ReadValue<Vector2>();
		_moveInput = Utils.NANDVector(_moveInput, newMoveInput);
	}
}
