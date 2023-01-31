using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaveManager : MonoBehaviour
{
	[Tooltip("The target enemies should attack")]
	public Transform Target;

	[SerializeField]
	[Tooltip("Invoked when a wave is completed")]
	private UnityEvent _onWaveComplete = new UnityEvent();

	public UnityEvent OnWaveComplete { get => this._onWaveComplete; }

	public Queue<Wave> Waves { get; } = new Queue<Wave>();

	public void SpawnNextWave()
	{
		if (this.Waves.Count < 1)
		{
			Debug.LogWarning("No more waves to spawn");
			return;
		}
		var wave = this.Waves.Dequeue();
		wave.OnComplete.AddListener(() => this.OnWaveComplete.Invoke());
		this.StartCoroutine(wave.Spawn(this.Target));
	}
}
