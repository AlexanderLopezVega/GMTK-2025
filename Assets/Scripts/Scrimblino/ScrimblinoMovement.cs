using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class ScrimblinoMovement : Element
{
	//	Fields
	private readonly ScrimblinoConfig _playerConfig;
	private readonly Input _input;
	private readonly Rigidbody _rigidbody;
	private State _state;
	private Hook _hook;
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

	//	Enumerations
	private enum State
	{
		Normal,
		Hooking
	}

	//	Methods
	public void Update(float deltaTime)
	{
		switch (_state)
		{
			case State.Normal: UpdateNormalState(); break;
			case State.Hooking: UpdateHookingState(deltaTime); break;
		}

	}
	public void MoveToHook(Hook hook)
	{
		_hook = hook;
		SetState(State.Hooking);
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

	private void UpdateNormalState()
	{
		Vector3 playerVelocity;
		
		playerVelocity = _playerConfig.MoveSpeed * new Vector3(
			_moveInput.x,
			_moveInput.y
		);
		_rigidbody.linearVelocity =
			_wagonVelocity +
			playerVelocity;
	}
	private void UpdateHookingState(float deltaTime)
	{
		Vector3 playerVelocity;
		Vector3 toHook;

		toHook = _hook.Position - _rigidbody.position;
		playerVelocity = _playerConfig.HookMoveSpeed * toHook.normalized;

		if (playerVelocity.magnitude * deltaTime > toHook.magnitude)
			playerVelocity = Vector3.ClampMagnitude(playerVelocity, toHook.magnitude);

		_rigidbody.linearVelocity = playerVelocity;

		if (_state == State.Hooking && Vector3.Distance(_rigidbody.position, _hook.Position) < 0.1f)
			_state = State.Normal;
	}
	private void SetState(State state)
	{
		_state = state;
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
