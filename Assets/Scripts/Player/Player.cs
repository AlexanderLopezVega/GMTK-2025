using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Player : MonoBehaviour
{
	//	Inspector
	[Header("Dependencies")]
	[SerializeField] private Scrimblino _scrimblino;
	[SerializeField] private Binglebongs _binglebongs;

	//	Fields
	private Input _input;
	private State _state;

	//	Enumerations
	private enum State
	{
		Scrimblino,
		Binglebongs
	}

	//	Methods
	private void Awake()
	{
		_input = new Input(new Controls());
		_state = State.Scrimblino;
	}
	private void Start()
	{
		_input.Enable();
		_input.OnHook += OnHook;
		_binglebongs.Movement.OnReset += OnBinglebongsReset;
		UpdateState();
	}
	private void OnDestroy()
	{
		_input.Disable();
		_input.OnHook -= OnHook;
		_binglebongs.Movement.OnReset -= OnBinglebongsReset;
	}

	private void SetState(State state)
	{
		_state = state;
		UpdateState();
	}
	private void UpdateState()
	{
		switch (_state)
		{
			case State.Scrimblino:
				{
					_scrimblino.enabled = true;
					_binglebongs.enabled = false;
				}
				break;
			case State.Binglebongs:
				{
					_scrimblino.enabled = false;
					_binglebongs.enabled = true;
				}
				break;
		}
	}
	private void OnBinglebongsReset()
	{
		if (_state == State.Binglebongs)
			SetState(State.Scrimblino);
	}
	private void OnHook(CallbackContext context)
	{
		if (context.performed && _state == State.Scrimblino)
			SetState(State.Binglebongs);
	}
}
