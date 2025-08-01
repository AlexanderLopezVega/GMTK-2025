using System;
using System.Collections.Generic;
using System.Linq;
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
		Transform head
	)
	{
		_binglebongsConfig = binglebongsConfig;
		_input = input;
		_lineRenderer = lineRenderer;
		_head = head;
		_binglebongsState = BinglebongsState.Idle;
	}

	//	Events
	public Action OnReset;

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

		_moveDirection = _moveInput;
		_binglebongsState = BinglebongsState.Moving;
		AddBodySegment();
		RecalculateTarget();
	}
	private void UpdateMoving(float deltaTime)
	{
		MoveToTarget(deltaTime);

		if (!HasReachedTarget())
			return;

		++_distance;

		if (HandleLoop())
			return;
		else if (HasReachedMaximumDistance())
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
				gridCentre,
				_binglebongsConfig.TileSize * Vector3.one
			);

			foreach (Collider collider in foundColliders)
				trappedGameObjects.Add(collider.attachedRigidbody.gameObject);
		}

		foreach (GameObject gameObject in trappedGameObjects)
			Debug.Log($"Trapped {gameObject}");

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
	private void RecalculateTarget()
	{
		Vector3 newMoveDirection;

		newMoveDirection = _moveInput;

		if (_moveDirection != newMoveDirection)
		{
			_moveDirection = newMoveDirection;
			AddBodySegment();
		}

		_targetPosition = _localPosition + _binglebongsConfig.TileSize * _moveDirection;
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
