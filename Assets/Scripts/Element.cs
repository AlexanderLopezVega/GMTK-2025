public class Element
{
	//	Fields
	private bool _isEnabled;

	//	Constructors
	public Element(bool isEnabled = false)
	{
		_isEnabled = isEnabled;
	}

	//	Methods
	public void Enable()
	{
		if (_isEnabled)
			return;

		_isEnabled = true;
		OnEnabled();
	}
	public void Disable()
	{
		if (!_isEnabled)
			return;
		
		_isEnabled = false;
		OnDisabled();
	}

	protected virtual void OnEnabled() { }
	protected virtual void OnDisabled() { }
}
