using UnityEngine;

public class Scrimblino : MonoBehaviour
{
	//	Inspector
	[Header("Dependencies")]
	[SerializeField] private Rigidbody _rigidbody;
	[SerializeField] private Animator _animator;
	[SerializeField] private SpriteRenderer _spriteRenderer;
	[SerializeField] private Binglebongs _hook;

	[Header("Options")]
	[SerializeField] private ScrimblinoConfig _scrimblinoConfig;

	//	Fields
	private Input _input;
	private ScrimblinoMovement _scrimblinoMovement;
	private ScrimblinoAnimator _scrimblinoAnimator;

	//	Methods
	private void Awake()
	{
		_input = new Input(new Controls());
		_scrimblinoMovement = new ScrimblinoMovement(
			_scrimblinoConfig,
			_input,
			_rigidbody
		);
		_scrimblinoAnimator = new ScrimblinoAnimator(
			_input,
			_spriteRenderer,
			_animator
		);
	}
	private void OnEnable()
	{
		_input.Enable();
		_scrimblinoMovement.Enable();
		_scrimblinoAnimator.Enable();
	}
	private void OnDisable()
	{
		_input.Disable();
		_scrimblinoMovement.Disable();
		_scrimblinoAnimator.Disable();
	}
	private void Update()
	{
		_scrimblinoMovement.Update(Time.deltaTime);
	}
}
