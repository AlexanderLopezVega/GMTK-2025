using UnityEngine;

public static class Utils
{
	//	Methods
	public static Vector2 NANDVector(Vector2 previous, Vector2 current)
	{
		float absX;
		float absY;

		if (current == Vector2.zero)
			return Vector2.zero;

		absX = Mathf.Abs(current.x);
		absY = Mathf.Abs(current.y);

		if (absX > absY)
			return new Vector2(current.x, 0f);
		else if (absY > absX)
			return new Vector2(0f, current.y);

		if (previous.x != current.x)
			return new Vector2(current.x, 0f);
		else if (previous.y != current.y)
			return new Vector2(0f, current.y);

		return current;
	}
}
