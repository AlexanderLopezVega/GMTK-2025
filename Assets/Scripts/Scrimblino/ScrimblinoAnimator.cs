using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class ScrimblinoAnimator : Element
{
	//	Constants
	private const string MoveXParameterName = "Move X";
	private const string MoveYParameterName = "Move Y";
	private const string IsHookingParameterName = "Is Hooking";

	//	Fields
	private readonly Input _input;
	private readonly SpriteRenderer _spriteRenderer;
	private readonly Animator _animator;
	private readonly int _moveXHash;
	private readonly int _moveYHash;
	private readonly int _isHookingHash;
	private Vector2 _moveInput;

	//	Constructors
	public ScrimblinoAnimator(
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
		_isHookingHash = Animator.StringToHash(IsHookingParameterName);
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

	private void SetMoveX(float value) => _animator.SetFloat(_moveXHash, value);
	private void SetMoveY(float value) => _animator.SetFloat(_moveYHash, value);
	private void SetIsHooking(bool value) => _animator.SetBool(_isHookingHash, value);
	private void OnMove(CallbackContext context)
	{
		Vector2 newMoveInput;

		if (context.started)
			return;

		newMoveInput = context.ReadValue<Vector2>();
		_moveInput = Utils.NANDVector(_moveInput, newMoveInput);
		SetMoveX(_moveInput.x);
		SetMoveY(_moveInput.y);

		if (_moveInput.x != 0f)
			_spriteRenderer.flipX = _moveInput.x < 0f;
	}
}
