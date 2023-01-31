using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Events;

public class Wave : MonoBehaviour
{
	[Header("Delay")]
	[SerializeField]
	[Tooltip("Minimum time in seconds between spawning enemies")]
	[Min(0)]
	private float _minimumDelay;

	public float MinimumDelay { get => this._minimumDelay; set => Mathf.Min(this.MaximumDelay, Mathf.Max(0, value)); }

	[SerializeField]
	[Tooltip("Maximum time in seconds between spawning enemies")]
	[Min(0)]
	private float _maximumDelay;

	public float MaximumDelay { get => this._maximumDelay; set => Mathf.Max(this.MinimumDelay, Mathf.Max(0, value)); }

	public float NextDelay { get => UnityEngine.Random.value * (this.MaximumDelay - this.MinimumDelay) + this.MinimumDelay; }

	[Serializable]
	public class EnemyGroup
	{
		[SerializeField]
		[Tooltip("The prefab used to spawn this enemy type")]
		public GameObject Prefab;

		[SerializeField]
		[Tooltip("The number of enemies this still needs to spawn")]
		[Min(0)]
		private int _quantity = 0;

		public int Quantity { get => this._quantity; set => Mathf.Max(0, value); }
	}

	[SerializeField]
	[Tooltip("The groups of enemies this will spawn in order")]
	public List<EnemyGroup> EnemyGroups = new List<EnemyGroup>();

	public int NextGroupIndex { get; private set; } = 0;

	public EnemyGroup NextGroup
	{
		get
		{
			while (this.NextGroupIndex < this.EnemyGroups.Count && this.EnemyGroups[this.NextGroupIndex].Quantity < 1)
				++this.NextGroupIndex;
			return this.NextGroupIndex < this.EnemyGroups.Count ? this.EnemyGroups[this.NextGroupIndex] : null;
		}
	}

	public enum Phase
	{
		Waiting,
		Spawning,
		Active,
		Complete
	}
	private Phase _currentPhase = Phase.Waiting;

	public Phase CurrentPhase 
	{ 
		get => this._currentPhase; 
		private set
		{
			this._currentPhase = value;
			switch (this.CurrentPhase)
			{
				case Phase.Spawning:
					this.OnStartSpawn.Invoke();
					break;
				case Phase.Active:
					this.OnEndSpawn.Invoke();
					break;
				case Phase.Complete:
					this.OnComplete.Invoke();
					break;
			}
		}
	}

	[SerializeField]
	[Tooltip("The event invoked once this starts spawning enemies")]
	private UnityEvent _onStartSpawn = new UnityEvent();

	public UnityEvent OnStartSpawn { get => this._onStartSpawn; }

	[SerializeField]
	[Tooltip("The event invoked once all enemies have been spawned")]
	private UnityEvent _onEndSpawn = new UnityEvent();

	public UnityEvent OnEndSpawn { get => this._onEndSpawn; }

	[SerializeField]
	[Tooltip("The event invoked once all active enemies are knocked out")]
	private UnityEvent _onComplete = new UnityEvent();

	public UnityEvent OnComplete { get => this._onComplete; }

	public HashSet<GameObject> ActiveEnemies { get; } = new HashSet<GameObject>();

	private void EnemyDied(GameObject enemy)
	{
		this.ActiveEnemies.Remove(enemy);
		if (this.CurrentPhase == Phase.Active && this.ActiveEnemies.Count < 1)
			this.CurrentPhase = Phase.Complete;
	}

	private void SpawnEnemy(GameObject prefab, Vector3 location, Transform target)
	{
		Quaternion rotation = Quaternion.LookRotation(target.position - location);
		GameObject enemy = UnityEngine.Object.Instantiate(prefab, location, rotation, this.transform);
		this.ActiveEnemies.Add(enemy);
		Persuer persuer = enemy.GetComponent<Persuer>();
		persuer.Target = target;
		persuer.OnKnockOut.AddListener(() => this.EnemyDied(enemy));
	}

	public IEnumerator Spawn(Transform target)
	{
		this.CurrentPhase = Phase.Spawning;
		while (this.NextGroup != null)
		{
			this.SpawnEnemy(this.NextGroup.Prefab, GateManager.Instance.NextOpenGate.transform.position, target);
			--this.NextGroup.Quantity;
			yield return new WaitForSeconds(this.NextDelay);
		}
		this.CurrentPhase = Phase.Active;
	}
}
