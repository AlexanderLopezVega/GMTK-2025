using UnityEngine;

public class Hook : MonoBehaviour
{
	//	Inspector
	[Header("Dependencies")]
	[SerializeField] private Transform _head;
	[SerializeField] private LineRenderer _lineRenderer;
	[Header("Options")]
	[SerializeField] private HookConfig _hookConfig;

	//	Fields
	private Input _input;
	private HookMovement _hookMovement;

	//	Methods
	public void Start()
	{
		_input = new Input(new Controls());
		_hookMovement = new HookMovement(
			_hookConfig,
			_input,
			_lineRenderer,
			_head
		);

		_input.Enable();
		_hookMovement.Enable();
	}
	private void OnDestroy()
	{
		_input.Disable();
		_hookMovement.Disable();
	}
	private void Update()
	{
		_hookMovement.Update(Time.deltaTime);
	}
}
