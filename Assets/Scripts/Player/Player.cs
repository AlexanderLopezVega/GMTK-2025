using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
	//	Inspector
	[Header("Options")]
	[SerializeField] private PlayerConfig _playerConfig;

	//	Fields
	private Input _input;
	private PlayerMovement _playerMovement;

	//	Methods
	private void Start()
	{
		_input = new Input(new Controls());
		_playerMovement = new PlayerMovement(
			_playerConfig,
			_input,
			GetComponent<Rigidbody>()
		);

		_input.Enable();
		_playerMovement.Enable();
	}
	private void OnDestroy()
	{
		_input.Disable();
		_playerMovement.Disable();
	}
	private void Update()
	{
		_playerMovement.Update(Time.deltaTime);
	}
}
