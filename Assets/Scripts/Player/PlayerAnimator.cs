using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerAnimator
{
	//	Constants
	private const string MoveXParameterName = "Move X";
	private const string MoveYParameterName = "Move Y";

	//	Fields
	private readonly Input _input;
	private readonly SpriteRenderer _spriteRenderer;
	private readonly Animator _animator;
	private readonly int _moveXHash;
	private readonly int _moveYHash;

	//	Constructors
	public PlayerAnimator(
		Input input,
		SpriteRenderer spriteRenderer,
		Animator animator
	)
	{
		_input = input;
		_spriteRenderer = spriteRenderer;
		_animator = animator;
		_moveXHash = Animator.StringToHash(MoveXParameterName);
		_moveYHash = Animator.StringToHash(MoveYParameterName);
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

	private void SetMoveX(float value) => _animator.SetFloat(_moveXHash, value);
	private void SetMoveY(float value) => _animator.SetFloat(_moveYHash, value);
	private void OnMove(CallbackContext context)
	{
		Vector2 moveInput;

		moveInput = context.ReadValue<Vector2>();
		SetMoveX(moveInput.x);
		SetMoveY(moveInput.y);
		
		if (moveInput.x != 0f)
			_spriteRenderer.flipX = moveInput.x < 0f;
	}
}
