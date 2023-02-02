using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Wave : MonoBehaviour
{
	public enum Phase
	{
		Waiting,
		ChangingGates,
		SpawningEnemies,
		Active,
		Complete
	}

	private Phase _currentPhase = Phase.Waiting;

	public Phase CurrentPhase
	{
		get => this._currentPhase;
		private set
		{
			var old = this.CurrentPhase;
			this._currentPhase = value;
			switch (old)
			{
				case Phase.ChangingGates:
					this.OnEndGates.Invoke();
					break;
				case Phase.SpawningEnemies:
					this.OnEndSpawn.Invoke();
					break;
			}
			switch (value)
			{
				case Phase.ChangingGates:
					this.OnStartGates.Invoke();
					break;
				case Phase.SpawningEnemies:
					this.OnStartSpawn.Invoke();
					break;
				case Phase.Complete:
					this.OnComplete.Invoke();
					break;
			}
		}
	}

	public void Begin()
	{
		this.StartCoroutine(this.GatePhase());
	}

	[Header("Gates")]
	[SerializeField]
	[Tooltip("The gates to open for this wave")]
	public List<Gate> OpenGates;

	[SerializeField]
	[Tooltip("The delay between opening each gate")]
	public RandomRange GateDelay;

	[SerializeField]
	[Tooltip("Invoked when gate transition phase starts")]
	private UnityEvent _onStartGates = new UnityEvent();

	public UnityEvent OnStartGates { get => this._onStartGates; }

	[SerializeField]
	[Tooltip("Invoked when gate transition phase ends")]
	private UnityEvent _onEndGates = new UnityEvent();

	public UnityEvent OnEndGates { get => this._onEndGates; }

	private IEnumerator GatePhase()
	{
		this.CurrentPhase = Phase.ChangingGates;
		foreach (var gate in Object.FindObjectsOfType<Gate>())
		{
			gate.IsOpen = this.OpenGates.Contains(gate);
			yield return new WaitForSeconds(this.GateDelay.NextValue);
		}
		this.StartCoroutine(this.EnemyPhase());
	}

	[Header("Enemies")]
	[SerializeField]
	[Tooltip("The prefab used to spawn enemies")]
	public GameObject Prefab;

	[SerializeField]
	[Tooltip("The number of enemies this still needs to spawn")]
	[Min(0)]
	private int _quantity = 0;

	public int Quantity { get => this._quantity; set => Mathf.Max(0, value); }

	[Tooltip("The time between spawning enemies")]
	public RandomRange EnemyDelay;

	public Transform Target;

	[SerializeField]
	[Tooltip("The event invoked once this starts spawning enemies")]
	private UnityEvent _onStartSpawn = new UnityEvent();

	public UnityEvent OnStartSpawn { get => this._onStartSpawn; }

	[SerializeField]
	[Tooltip("The event invoked once all enemies have been spawned")]
	private UnityEvent _onEndSpawn = new UnityEvent();

	public UnityEvent OnEndSpawn { get => this._onEndSpawn; }

	private HashSet<GameObject> _activeEnemies = new HashSet<GameObject>();

	public HashSet<GameObject> ActiveEnemies { get => this._activeEnemies; }

	private void SpawnEnemy(Vector3 location)
	{
		if (this.Quantity < -1)
		{
			Debug.LogWarning("Ran out of enemies to spawn");
			return;
		}
		--this.Quantity;
		Quaternion rotation = Quaternion.LookRotation(this.Target.position - location);
		GameObject enemy = UnityEngine.Object.Instantiate(this.Prefab, location, rotation, this.transform);
		this.ActiveEnemies.Add(enemy);
		Rat rat = enemy.GetComponent<Rat>();
		rat.Target = this.Target;
		rat.OnKnockOut.AddListener(() => this.EnemyResolved(enemy));
		rat.OnSteal.AddListener(() => this.EnemyResolved(enemy));
	}

	private void EnemyResolved(GameObject enemy)
	{
		this.ActiveEnemies.Remove(enemy);
		if (this.CurrentPhase == Phase.Active && this.ActiveEnemies.Count < 1)
			this.CurrentPhase = Phase.Complete;
	}

	private IEnumerator EnemyPhase()
	{
		this.CurrentPhase = Phase.SpawningEnemies;
		while (this.Quantity > 0)
		{
			var gate = this.OpenGates[Random.Range(0, this.OpenGates.Count)];
			this.SpawnEnemy(gate.transform.position);
			yield return new WaitForSeconds(this.EnemyDelay.NextValue);
		}
		this.CurrentPhase = Phase.Active;
	}

	[Header("Completion")]
	[SerializeField]
	[Tooltip("The event invoked once all active enemies are knocked out")]
	private UnityEvent _onComplete = new UnityEvent();

	public UnityEvent OnComplete { get => this._onComplete; }
}
