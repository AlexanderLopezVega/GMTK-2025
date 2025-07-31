using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class HookMovement
{
	//	Fields
	private readonly HookConfig _hookConfig;
	private readonly Input _input;
	private readonly LineRenderer _lineRenderer;
	private readonly Transform _head;
	private Vector3 _localPosition;
	private Vector3 _targetPosition;
	private Vector3 _moveDirection;
	private float _distance;
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
	}

	//	Methods
	public void Enable()
	{
		_input.OnMove += OnMove;
		_lineRenderer.positionCount = 1;
	}
	public void Disable()
	{
		_input.OnMove -= OnMove;
	}
	public void Update(float deltaTime)
	{
		float motion;

		if (Vector3.Distance(_localPosition, _targetPosition) < 0.01f)
		{
			if (_moveDirection == Vector3.zero && _moveInput == Vector2.zero)
				return;

			_targetPosition = _lineRenderer.GetPosition(_lineRenderer.positionCount - 1) + _hookConfig.TileSize * _moveDirection;
		}

		motion = _hookConfig.MoveSpeed * deltaTime;
		_localPosition = Vector3.MoveTowards(
			_localPosition,
			_targetPosition,
			motion
		);
		_distance += motion;

		if (Vector3.Distance(_localPosition, _targetPosition) < 0.01f)
		{
			if (_distance >= _hookConfig.MaxDistance)
			{
				_distance -= _hookConfig.MaxDistance;
				_lineRenderer.positionCount = 1;
				_targetPosition = Vector3.zero;
				_localPosition = Vector3.zero;
				_head.SetPositionAndRotation(
					_localPosition,
					Quaternion.LookRotation(
						_moveDirection,
						Vector3.back
					)
				);
				_head.position += Vector3.back * 0.1f;
				_moveDirection = Vector3.zero;
				return;
			}

			if (_moveInput == Vector2.zero)
				_moveDirection = Vector3.zero;
			else
			{
				++_lineRenderer.positionCount;
				_moveDirection = _moveInput;
				_targetPosition += _hookConfig.TileSize * _moveDirection;
			}
		}
		else
		{
			_lineRenderer.SetPosition(_lineRenderer.positionCount - 1, _localPosition);
			_head.SetPositionAndRotation(
				_localPosition,
				Quaternion.LookRotation(
					_moveDirection,
					Vector3.back
				)
			);
		}
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
