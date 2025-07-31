using static Controls;
using static UnityEngine.InputSystem.InputAction;

public class Input : IPlayerActions
{
	//	Fields
	private readonly Controls _controls;

	//	Constructors
	public Input(
		Controls controls
	)
	{
		_controls = controls;
	}

	//	Delegates
	public delegate void OnInputDelegate(CallbackContext context);

	//	Events
	public event OnInputDelegate OnMove;
	public event OnInputDelegate OnHook;

	//	Interface implementations
	void IPlayerActions.OnMove(CallbackContext context) => OnMove?.Invoke(context);
	void IPlayerActions.OnHook(CallbackContext context) => OnHook?.Invoke(context);

	//	Methods
	public void Enable()
	{
		_controls.Player.SetCallbacks(this);
		_controls.Player.Enable();
	}
	public void Disable()
	{
		_controls.Player.SetCallbacks(null);
		_controls.Player.Disable();
	}
}
