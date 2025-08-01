using System.Collections.Generic;
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
	public static List<Vector3> GetGridCentersInsidePolygon(Vector3[] polygon)
    {
		List<Vector3> insideCenters;
		Vector3 centre;
		
		insideCenters = new();

        int minX = Mathf.FloorToInt(polygon[0].x);
        int maxX = Mathf.CeilToInt(polygon[0].x);
        int minY = Mathf.FloorToInt(polygon[0].y);
        int maxY = Mathf.CeilToInt(polygon[0].y);

        foreach (Vector2 point in polygon)
        {
            minX = Mathf.Min(minX, Mathf.FloorToInt(point.x));
            maxX = Mathf.Max(maxX, Mathf.CeilToInt(point.x));
            minY = Mathf.Min(minY, Mathf.FloorToInt(point.y));
            maxY = Mathf.Max(maxY, Mathf.CeilToInt(point.y));
        }

        for (int x = minX; x < maxX; x++)
        {
            for (int y = minY; y < maxY; y++)
            {
                centre = new(x + 0.5f, y + 0.5f, 0f);

                if (IsPointInPolygon(centre, polygon))
					insideCenters.Add(centre);
            }
        }

        return insideCenters;
    }
	public static bool IsPointInPolygon(Vector3 point, Vector3[] polygon)
	{
		bool inside;
		int j;

		inside = false;
		j = polygon.Length - 1;

		for (int i = 0; i < polygon.Length; j = i++)
			if (((polygon[i].y > point.y) != (polygon[j].y > point.y)) &&
				(point.x < (polygon[j].x - polygon[i].x) *
				(point.y - polygon[i].y) / (polygon[j].y - polygon[i].y) + polygon[i].x))
				inside = !inside;
		return inside;
	}
}
