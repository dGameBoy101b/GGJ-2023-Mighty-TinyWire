using UnityEngine;

public class WaveStarter : MonoBehaviour
{
	[Tooltip("The wave to start")]
	public Wave Wave;

	[Tooltip("Whether the wave should be started")]
	public bool ShouldStart = false;

	public Wave.Phase Phase;

	private void Update()
	{
		if (this.ShouldStart && this.Wave != null && this.Wave.CurrentPhase == Wave.Phase.Waiting)
			this.Wave.Begin();
		if (this.Wave != null)
			this.Phase = this.Wave.CurrentPhase;
	}
}
