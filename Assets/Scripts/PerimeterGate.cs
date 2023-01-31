using UnityEngine;
using UnityEngine.Events;

public class PerimeterGate : MonoBehaviour
{
	[SerializeField]
	[Tooltip("The event triggered when this is opened")]
	private UnityEvent _onOpen = new UnityEvent();

	public UnityEvent OnOpen { get => this._onOpen; }

	public bool IsOpen { get; private set; } = false;

	public void Open()
	{
		this.IsOpen = true;
		this.OnOpen.Invoke();
	}
}
