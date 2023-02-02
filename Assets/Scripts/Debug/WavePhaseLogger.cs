using UnityEngine;

public class WavePhaseLogger : MonoBehaviour
{
	public Wave Wave;

	private Wave.Phase last_phase;

	private void Update()
	{
		if (this.Wave != null && this.Wave.CurrentPhase != this.last_phase)
		{
			this.last_phase = this.Wave.CurrentPhase;
			Debug.Log("Wave " + this.Wave.name + " entered phase: " + this.Wave.CurrentPhase);
		}
	}
}
