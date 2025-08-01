using System;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class HookMovement
{
	//	constants
	private const float DistanceThreshold = 0.01f;

	//	Fields
	private readonly HookConfig _hookConfig;
	private readonly Input _input;
	private readonly LineRenderer _lineRenderer;
	private readonly Transform _head;
	private HookState _hookState;
	private Vector3 _localPosition;
	private Vector3 _targetPosition;
	private Vector3 _moveDirection;
	private uint _distance;
	private Vector2 _moveInput;

	//	Constructors
	public HookMovement(
		HookConfig hookConfig,
		Input input,
		LineRenderer lineRenderer,
		Transform head
	)
	{
		_hookConfig = hookConfig;
		_input = input;
		_lineRenderer = lineRenderer;
		_head = head;
		_hookState = HookState.Idle;
	}

	//	Enumerations
	private enum HookState
	{
		Idle,
		Moving
	}

	//	Methods
	public void Enable()
	{
		_input.OnMove += OnMove;
		_lineRenderer.positionCount = 1;
		_head.localRotation = Quaternion.LookRotation(
			Vector3.up,
			Vector3.back
		);
	}
	public void Disable()
	{
		_input.OnMove -= OnMove;
	}
	public void Update(float deltaTime)
	{
		switch (_hookState)
		{
			case HookState.Idle: UpdateIdle(); break;
			case HookState.Moving: UpdateMoving(deltaTime); break;
		}
	}

	private void UpdateIdle()
	{
		if (_moveInput == default)
			return;

		_moveDirection = _moveInput;
		_hookState = HookState.Moving;
		AddBodySegment();
		RecalculateTarget();
	}
	private void UpdateMoving(float deltaTime)
	{
		MoveToTarget(deltaTime);

		if (!HasReachedTarget())
			return;

		++_distance;

		if (HasReachedMaximumDistance())
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
	private void MoveToTarget(float deltaTime)
	{
		_localPosition = Vector3.MoveTowards(
			_localPosition,
			_targetPosition,
			_hookConfig.MoveSpeed * deltaTime
		);
		UpdateGraphics();
	}
	private bool HasReachedMaximumDistance()
	{
		return _distance >= _hookConfig.MaxDistance;
	}
	private void Reset()
	{
		_localPosition = default;
		_lineRenderer.positionCount = 1;
		_lineRenderer.SetPosition(_lineRenderer.positionCount - 1, default);
		_distance = 0;
		StopMoving();
	}
	private void StopMoving()
	{
		_targetPosition = _localPosition;
		_moveInput = default;
		_moveDirection = default;
		_hookState = HookState.Idle;
		
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

		_targetPosition = _localPosition + _hookConfig.TileSize * _moveDirection;
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
		float absX;
		float absY;

		if (context.started)
			return;

		newMoveInput = context.ReadValue<Vector2>();
		absX = Mathf.Abs(newMoveInput.x);
		absY = Mathf.Abs(newMoveInput.y);

		if (absX > absY)
			newMoveInput.y = 0f;
		else if (absX < absY)
			newMoveInput.x = 0f;
		else
		{
			if (_moveInput.x != newMoveInput.x)
				newMoveInput.y = 0f;
			else
				newMoveInput.x = 0f;
		}

		_moveInput = newMoveInput;
	}
}
