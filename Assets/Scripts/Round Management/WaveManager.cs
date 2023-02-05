using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaveManager : MonoBehaviour
{
	[Tooltip("The target enemies should attack")]
	public Transform Target;

	[SerializeField]
	[Tooltip("The waves to enqueue")]
	private List<Wave> _waves = new List<Wave>();

	public Queue<Wave> Waves { get; } = new Queue<Wave>();

	[SerializeField]
	[Tooltip("Invoked when a wave is started")]
	private UnityEvent _onWaveStart = new UnityEvent();

	public UnityEvent OnWaveStart { get => this._onWaveStart; }

	[SerializeField]
	[Tooltip("Invoked when a wave is completed")]
	private UnityEvent _onWaveComplete = new UnityEvent();

	public UnityEvent OnWaveComplete { get => this._onWaveComplete; }

	[SerializeField]
	[Tooltip("The number of waves completed so far")]
	[Min(0)]
	private int _waveCount;

	public int WaveCount 
	{
		get => this._waveCount;
		private set
		{
			if (value == this.WaveCount)
				return;
			this._waveCount = value;
			this.OnWaveCountChange.Invoke(value);
		}
	}

	[SerializeField]
	[Tooltip("Invoked when the wave count changes")]
	private UnityEvent<int> _onWaveCountChange = new UnityEvent<int>();

	public UnityEvent<int> OnWaveCountChange { get => this._onWaveCountChange; }

	private void EnqueueWaves()
	{
		foreach (Wave wave in this._waves)
			this.Waves.Enqueue(wave);
	}

	public void SpawnNextWave()
	{
		if (this.Waves.Count < 1)
		{
			Debug.LogWarning("No more waves to spawn");
			return;
		}
		var wave = this.Waves.Dequeue();
		wave.Target = this.Target;
		wave.OnComplete.AddListener(() => ++this.WaveCount);
		wave.OnComplete.AddListener(() => this.OnWaveComplete.Invoke());
		wave.OnStart.AddListener(() => this.OnWaveStart.Invoke());
		wave.Begin();
	}

	private void Awake()
	{
		this.EnqueueWaves();
	}
}
