using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class BinglebongsMovement : Element
{
	//	constants
	private const float DistanceThreshold = 0.01f;

	//	Fields
	private readonly BinglebongsConfig _binglebongsConfig;
	private readonly Input _input;
	private readonly LineRenderer _lineRenderer;
	private readonly Transform _root;
	private readonly Transform _head;
	private BinglebongsState _binglebongsState;
	private Vector3 _localPosition;
	private Vector3 _targetPosition;
	private Vector3 _moveDirection;
	private uint _distance;
	private Vector2 _moveInput;

	//	Constructors
	public BinglebongsMovement(
		BinglebongsConfig binglebongsConfig,
		Input input,
		LineRenderer lineRenderer,
		Transform root,
		Transform head
	)
	{
		_binglebongsConfig = binglebongsConfig;
		_input = input;
		_lineRenderer = lineRenderer;
		_root = root;
		_head = head;
		_binglebongsState = BinglebongsState.Idle;
	}

	//	Events
	public Action OnReset;
	public Action<Hook> OnHookFound;

	//	Enumerations
	private enum BinglebongsState
	{
		Idle,
		Moving
	}

	//	Methods
	protected override void OnEnabled()
	{
		_localPosition = default;
		_lineRenderer.positionCount = 1;
		_lineRenderer.SetPosition(_lineRenderer.positionCount - 1, default);
		_distance = 0;
		_input.OnMove += OnMove;
	}
	protected override void OnDisabled()
	{
		_input.OnMove -= OnMove;
	}
	public void Update(float deltaTime)
	{
		switch (_binglebongsState)
		{
			case BinglebongsState.Idle: UpdateIdle(); break;
			case BinglebongsState.Moving: UpdateMoving(deltaTime); break;
		}
	}

	private void UpdateIdle()
	{
		if (_moveInput == default)
			return;
		if (RecalculateTarget())
			_binglebongsState = BinglebongsState.Moving;
	}
	private void UpdateMoving(float deltaTime)
	{
		MoveToTarget(deltaTime);

		if (!HasReachedTarget())
			return;

		_localPosition = _targetPosition;
		++_distance;

		if (HandleLoop() || HasReachedMaximumDistance())
			Reset();
		else if (_moveInput == Vector2.zero)
			StopMoving();
		else
			RecalculateTarget();
	}
	private bool HasReachedTarget()
	{
		return Vector3.Distance(_localPosition, _targetPosition) < DistanceThreshold;
	}
	private bool HandleLoop()
	{
		Vector3[] positions;
		List<Vector3> gridCentres;
		Collider[] foundColliders;
		HashSet<GameObject> trappedGameObjects;
		int numPositions;
		int i;

		positions = new Vector3[_lineRenderer.positionCount];
		numPositions = _lineRenderer.GetPositions(positions);

		for (i = 0; i < numPositions - 1; ++i)
			if (Vector3.Distance(positions[i], positions[numPositions - 1]) < DistanceThreshold)
				break;

		if (i == numPositions - 1)
			return false;

		trappedGameObjects = new HashSet<GameObject>();
		gridCentres = Utils.GetGridCentersInsidePolygon(
			positions
			.Skip(i)
			.Take(numPositions - i)
			.ToArray()
		);

		foreach (Vector3 gridCentre in gridCentres)
		{
			foundColliders = Physics.OverlapBox(
				_root.TransformPoint(gridCentre),
				_binglebongsConfig.TileSize * Vector3.one
			);

			foreach (Collider collider in foundColliders)
				if (collider.TryGetComponent(out RootLink rootLink))
					trappedGameObjects.Add(rootLink.Root.gameObject);
		}

		foreach (GameObject gameObject in trappedGameObjects)
		{
			if (gameObject.TryGetComponent(out Hook hook))
			{
				OnHookFound?.Invoke(hook);
				return true;
			}
		}

		Debug.Log("No hook related items found");

		Reset();
		
		return true;
	}
	private void MoveToTarget(float deltaTime)
	{
		_localPosition = Vector3.MoveTowards(
			_localPosition,
			_targetPosition,
			_binglebongsConfig.MoveSpeed * deltaTime
		);
		UpdateGraphics();
	}
	private bool HasReachedMaximumDistance()
	{
		return _distance >= _binglebongsConfig.MaxDistance;
	}
	private void Reset()
	{
		_localPosition = default;
		_lineRenderer.positionCount = 1;
		_lineRenderer.SetPosition(_lineRenderer.positionCount - 1, default);
		_distance = 0;
		_head.localRotation = Quaternion.LookRotation(
			Vector3.up,
			Vector3.back
		);
		StopMoving();

		OnReset?.Invoke();
	}
	private void StopMoving()
	{
		_targetPosition = _localPosition;
		_moveInput = default;
		_moveDirection = default;
		_binglebongsState = BinglebongsState.Idle;
		UpdateGraphics();
	}
	private bool RecalculateTarget()
	{
		bool IsPreviousPosition(Vector3 position)
		{
			float distance;

			if (_lineRenderer.positionCount < 2)
				return false;

			distance = Vector3.Distance(
				_lineRenderer.GetPosition(_lineRenderer.positionCount - 2),
				position
			);
			return distance < 0.01f;
		}

		Vector3 newMoveDirection;
		Vector3 newPosition;

		newMoveDirection = _moveInput;
		newPosition = _localPosition + _binglebongsConfig.TileSize * newMoveDirection;

		if (_moveDirection != newMoveDirection && !IsPreviousPosition(newPosition))
		{
			_moveDirection = newMoveDirection;
			_targetPosition = _localPosition + _binglebongsConfig.TileSize * _moveDirection;

			AddBodySegment();
			return true;
		}

		return false;
	}
	private void UpdateGraphics()
	{
		_lineRenderer.SetPosition(_lineRenderer.positionCount - 1, _localPosition);
		_head.SetLocalPositionAndRotation(
			_localPosition,
			(_moveDirection == default)
				? _head.localRotation
				: Quaternion.LookRotation(
					_moveDirection,
					Vector3.back
				)
		);
	}
	private void AddBodySegment()
	{
		++_lineRenderer.positionCount;
		_lineRenderer.SetPosition(_lineRenderer.positionCount - 1, _localPosition);
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
