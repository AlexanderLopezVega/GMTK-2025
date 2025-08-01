using UnityEngine;

public class Binglebongs : MonoBehaviour
{
	//	Inspector
	[Header("Dependencies")]
	[SerializeField] private Transform _head;
	[SerializeField] private LineRenderer _lineRenderer;
	[Header("Options")]
	[SerializeField] private BinglebongsConfig _binglebongsConfig;

	//	Fields
	private Input _input;
	private BinglebongsMovement _binglebongsMovement;

	//	Properties
	public BinglebongsMovement Movement => _binglebongsMovement;

	//	Methods
	private void Awake()
	{
		_input = new Input(new Controls());
		_binglebongsMovement = new BinglebongsMovement(
			_binglebongsConfig,
			_input,
			_lineRenderer,
			_head
		);
	}
	private void OnEnable()
	{
		_input.Enable();
		_binglebongsMovement.Enable();
	}
	private void OnDisable()
	{
		_input.Disable();
		_binglebongsMovement.Disable();
	}
	private void Update()
	{
		_binglebongsMovement.Update(Time.deltaTime);
	}
}
