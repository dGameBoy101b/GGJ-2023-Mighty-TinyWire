using UnityEngine;
using UnityEngine.Events;

public class Silo : MonoBehaviour
{
	[SerializeField]
	[Tooltip("The number of carrots currently stored in this")]
	[Min(0)]
	private int _carrotsStored = 0;

	public int CarrotsStored { get => this._carrotsStored; set => this._carrotsStored = Mathf.Max(0, value); }

	[SerializeField]
	[Tooltip("The event invoked when the number of carrots stored in this silo changes")]
	private UnityEvent<int> _onCarrotsChanged = new UnityEvent<int>();

	public UnityEvent<int> OnCarrotsChanged { get => this._onCarrotsChanged; }

	public static Silo Instance { get; private set; } = null;

	private void CheckOnlyInstance()
	{
		Silo.Instance ??= this;
		if (Silo.Instance != this)
			Object.Destroy(this);
	}

	private void Awake()
	{
		this.CheckOnlyInstance();
		this.OnCarrotsChanged.Invoke(this.CarrotsStored);
	}

	private void OnDestroy()
	{
		if (Silo.Instance == this)
			Silo.Instance = null;
	}
}
