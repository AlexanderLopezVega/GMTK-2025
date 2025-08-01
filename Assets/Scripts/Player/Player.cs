using UnityEngine;

public class Player : MonoBehaviour
{
	//	Inspector
	[Header("Dependencies")]
	[SerializeField] private Rigidbody _rigidbody;
	[SerializeField] private Animator _animator;
	[SerializeField] private SpriteRenderer _spriteRenderer;

	[Header("Options")]
	[SerializeField] private PlayerConfig _playerConfig;

	//	Fields
	private Input _input;
	private PlayerMovement _playerMovement;
	private PlayerAnimator _playerAnimator;

	//	Methods
	private void Start()
	{
		_input = new Input(new Controls());
		_playerMovement = new PlayerMovement(
			_playerConfig,
			_input,
			_rigidbody
		);
		_playerAnimator = new PlayerAnimator(
			_input,
			_spriteRenderer,
			_animator
		);

		_input.Enable();
		_playerMovement.Enable();
		_playerAnimator.Enable();
	}
	private void OnDestroy()
	{
		_input.Disable();
		_playerMovement.Disable();
		_playerAnimator.Disable();
	}
	private void Update()
	{
		_playerMovement.Update(Time.deltaTime);
	}
}
