using UnityEngine;
using UnityEngine.Events;

public class AmmoStorage : MonoBehaviour
{
	[SerializeField]
	[Tooltip("The number of veggies currently held in the Vac Gun")]
	[Min(0)]
	private int _ammoCount;

	public int AmmoCount
	{
		get => this._ammoCount;
		set
		{
			if (this.IsJammed)
				return;
			value = Mathf.Max(value, 0);
			if (this.AmmoCount == value)
				return;
			this._ammoCount = value;
			this.OnCountChange.Invoke(value);
		}
	}

	[SerializeField]
	[Tooltip("Invoked when the ammo count changes with the new ammo count")]
	private UnityEvent<int> _onCountChange = new UnityEvent<int>();

	public UnityEvent<int> OnCountChange { get => this._onCountChange; }

	[SerializeField]
	[Tooltip("Whether this is currently jammed")]
	private bool _isJammed = false;

	public bool IsJammed
	{
		get => this._isJammed;
		set
		{
			if (value == this.IsJammed)
				return;
			this._isJammed = value;
			if (value)
				this.OnJam.Invoke();
			else
				this.OnUnjam.Invoke();
		}
	}

	[SerializeField]
	[Tooltip("Invoked when this becomes jammed")]
	private UnityEvent _onJam = new UnityEvent();

	public UnityEvent OnJam { get => this._onJam; }

	[SerializeField]
	[Tooltip("Invoked when this is no longer jammed")]
	private UnityEvent _onUnjam = new UnityEvent();

	public UnityEvent OnUnjam { get => this._onUnjam; }
}
