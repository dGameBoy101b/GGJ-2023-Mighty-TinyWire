using UnityEngine;

[ExecuteAlways]
public class GateOpener : MonoBehaviour
{
	[Tooltip("The gate to open")]
	public Gate Gate;

	private Gate last_gate;

	[Tooltip("Whether the gate should be opened")]
	public bool ShouldBeOpen;

	void Update()
	{
		if (this.Gate != null && this.Gate != this.last_gate)
		{
			this.last_gate = this.Gate;
			this.ShouldBeOpen = this.Gate.IsOpen;
		}
		if (this.Gate != null)
			this.Gate.IsOpen = this.ShouldBeOpen;
	}
}
