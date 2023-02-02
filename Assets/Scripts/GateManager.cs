using System.Collections.Generic;
using UnityEngine;

public class GateManager : MonoBehaviour
{
	public static GateManager Instance { get; private set; } = null;

	[SerializeField]
	[Tooltip("The gates enemies should spawn at")]
	private List<Gate> _gates = new List<Gate>();

	public List<Gate> Gates { get { return this._gates; } }

	private List<Gate> OpenGates = new List<Gate>();

	public Gate NextOpenGate
	{
		get
		{
			return this.OpenGates.Count < 1 ? null : this.OpenGates[Random.Range(0, this.OpenGates.Count)];
		}
	}

	private void FindAllGates()
	{
		foreach (var gate in Object.FindObjectsOfType<Gate>())
			if (!this.Gates.Contains(gate))
				this.Gates.Add(gate);
	}

	private void CreateGateListeners()
	{
		foreach (Gate gate in this.Gates)
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
		this.FindAllGates();
		this.CreateGateListeners();
	}
}
