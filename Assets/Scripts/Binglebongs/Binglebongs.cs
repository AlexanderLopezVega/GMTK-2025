using UnityEngine;

public class Binglebongs : MonoBehaviour
{
	//	Inspector
	[Header("Dependencies")]
	[SerializeField] private Transform _root;
	[SerializeField] private Transform _head;
	[SerializeField] private LineRenderer _lineRenderer;
	[SerializeField] private Renderer[] _renderers;
	[SerializeField] private Scrimblino _scrimblino;

	[Header("Options")]
	[SerializeField] private BinglebongsConfig _binglebongsConfig;

	//	Fields
	private Input _input;
	private BinglebongsMovement _binglebongsMovement;
	private BinglebongsGraphics _binglebongsGraphics;

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
			_root,
			_head
		);
		_binglebongsGraphics = new BinglebongsGraphics(
			_renderers
		);
	}
	private void OnEnable()
	{
		_input.Enable();
		_binglebongsMovement.Enable();
		_binglebongsGraphics.Enable();
		_binglebongsMovement.OnHookFound += OnHookFound;
	}
	private void OnDisable()
	{
		_input.Disable();
		_binglebongsMovement.Disable();
		_binglebongsGraphics.Disable();
		_binglebongsMovement.OnHookFound -= OnHookFound;
	}
	private void Update()
	{
		_binglebongsMovement.Update(Time.deltaTime);
	}
	private void OnHookFound(Hook hook)
	{
		_scrimblino.MoveToHook(hook);
	}
}
