using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
	[Tooltip("The target enemies should attack")]
	public Transform Target;

	[SerializeField]
	[Tooltip("The gates enemies should spawn at")]
	private List<PerimeterGate> _gates = new List<PerimeterGate>();

	public IReadOnlyList<PerimeterGate> Gates { get { return this._gates; } }

	private List<PerimeterGate> OpenGates = new List<PerimeterGate>();

	public PerimeterGate NextGate
	{
		get
		{
			return this.OpenGates.Count < 1 ? null : this.OpenGates[Random.Range(0, this.OpenGates.Count)];
		}
	}

	public Queue<Wave> Waves { get; } = new Queue<Wave>();

	public HashSet<GameObject> ActiveEnemies { get; } = new HashSet<GameObject>();

	private void SpawnEnemy(GameObject prefab)
	{
		Vector3 location = this.NextGate.transform.position;
		Quaternion rotation = Quaternion.LookRotation(this.Target.transform.position - location);
		GameObject enemy = Object.Instantiate(prefab, location, rotation, this.transform);
		this.ActiveEnemies.Add(enemy);
		Persuer persuer = enemy.GetComponent<Persuer>();
		persuer.Target = this.Target;
	}

	private IEnumerator SpawnWave(Wave wave)
	{
		for (var enemy = wave.DecrementNextEnemy(); enemy != null; enemy = wave.DecrementNextEnemy())
		{
			this.SpawnEnemy(enemy);
			yield return new WaitForSeconds(wave.NextDelay);
		}
	}

	public void SpawnNextWave()
	{
		if (this.Waves.Count < 1)
			Debug.LogWarning("No more waves to spawn");
		else
			this.StartCoroutine(this.SpawnWave(this.Waves.Dequeue()));
	}

	private void CreateGateListeners()
	{
		foreach (PerimeterGate gate in this.Gates)
			gate.OnOpen.AddListener(() => this.OpenGates.Add(gate));
	}

	private void Start()
	{
		this.CreateGateListeners();
	}
}
