using System.Collections.Generic;
using UnityEngine;

public class WaveList : MonoBehaviour
{
	[Tooltip("The wave manager to add waves to")]
	public WaveManager Manager;

	[SerializeField]
	[Tooltip("The waves to add to the wave manager")]
	private List<Wave> _waves = new List<Wave>();

	public List<Wave> Waves { get => this._waves; }

	public void AddWaves()
	{
		foreach (Wave wave in this.Waves)
			this.Manager.Waves.Enqueue(wave);
	}

	private void OnEnable()
	{
		this.AddWaves();
	}
}
