using UnityEngine;
using UnityEngine.Events;

public class Gate : MonoBehaviour
{
	[SerializeField]
	[Tooltip("The event invoked when this is opened")]
	private UnityEvent _onOpen = new UnityEvent();

	public UnityEvent OnOpen { get => this._onOpen; }

	[SerializeField]
	[Tooltip("The event invoked when this is closed")]
	private UnityEvent _onClose = new UnityEvent();

	public UnityEvent OnClose { get => this._onClose; }

	private bool _isOpen = false;

	public bool IsOpen 
	{
		get => this._isOpen;
		set
		{
			var old = this._isOpen;
			this._isOpen = value;
			if (old != value)
				(value ? this.OnOpen : this.OnClose).Invoke();
		}
	}
}
