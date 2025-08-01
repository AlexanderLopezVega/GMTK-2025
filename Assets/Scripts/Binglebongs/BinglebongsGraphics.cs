using UnityEngine;

public class BinglebongsGraphics : Element
{
	//	Fields
	private readonly Renderer[] _renderers;

	//	Constructors
	public BinglebongsGraphics(
		Renderer[] renderers
	)
	{
		_renderers = renderers;
	}

	//	Methods
	protected override void OnEnabled()
	{
		SetSpriteRenderersEnabled(true);
	}
	protected override void OnDisabled()
	{
		SetSpriteRenderersEnabled(false);
	}

	private void SetSpriteRenderersEnabled(bool enabled)
	{
		foreach (Renderer renderer in _renderers)
			renderer.enabled = enabled;
	}
}
