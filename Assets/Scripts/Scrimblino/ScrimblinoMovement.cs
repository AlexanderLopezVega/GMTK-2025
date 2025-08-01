using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class ScrimblinoMovement : Element
{
	//	Fields
	private readonly ScrimblinoConfig _playerConfig;
	private readonly Input _input;
	private readonly Rigidbody _rigidbody;
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
	protected override void OnEnabled()
	{
		_input.OnMove += OnMove;
	}
	protected override void OnDisabled()
	{
		_input.OnMove -= OnMove;
	}
	public void Update(float deltaTime)
	{
		_rigidbody.linearVelocity = _playerConfig.MoveSpeed * new Vector3(
			_moveInput.x,
			_moveInput.y
		);
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
