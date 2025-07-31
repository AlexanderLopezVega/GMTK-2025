using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerMovement
{
	//	Fields
	private readonly PlayerConfig _playerConfig;
	private readonly Input _input;
	private readonly Rigidbody _rigidbody;
	private Vector2 _moveInput;

	//	Constructors
	public PlayerMovement(
		PlayerConfig playerConfig,
		Input input,
		Rigidbody rigidbody
	)
	{
		_playerConfig = playerConfig;
		_input = input;
		_rigidbody = rigidbody;
	}

	//	Methods
	public void Enable()
	{
		_input.OnMove += OnMove;
	}
	public void Disable()
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
		_moveInput = context.ReadValue<Vector2>();
	}
}
