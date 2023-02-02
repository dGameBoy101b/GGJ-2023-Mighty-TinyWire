using System.Collections.Generic;
using UnityEngine;

public class GateManager : MonoBehaviour
{
	public static GateManager Instance { get; private set; } = null;

	[SerializeField]
	[Tooltip("The gates enemies should spawn at")]
	private List<PerimeterGate> _gates = new List<PerimeterGate>();

	public IReadOnlyList<PerimeterGate> Gates { get { return this._gates; } }

	private List<PerimeterGate> OpenGates = new List<PerimeterGate>();

	public PerimeterGate NextOpenGate
	{
		get
		{
			return this.OpenGates.Count < 1 ? null : this.OpenGates[Random.Range(0, this.OpenGates.Count)];
		}
	}

	private void CreateGateListeners()
	{
		foreach (PerimeterGate gate in this.Gates)
			gate.OnOpen.AddListener(() => this.OpenGates.Add(gate));
	}

	private void CheckSingleInstance()
	{
		GateManager.Instance ??= this;
		if (GateManager.Instance != this)
			Object.Destroy(this);
	}

	private void Awake()
	{
		this.CheckSingleInstance();
	}

	private void OnDestroy()
	{
		if (GateManager.Instance == this)
			GateManager.Instance = null;
	}

	private void Start()
	{
		this.CreateGateListeners();
	}
}
