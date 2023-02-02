using UnityEngine;

public class GateOpener : MonoBehaviour
{
	[Tooltip("The gate to open")]
	public PerimeterGate Gate;

	[Tooltip("Whether the gate should be opened")]
	public bool ShouldBeOpen;

	void Update()
	{
		if (this.Gate != null && this.ShouldBeOpen)
			this.Gate.Open();
	}
}
