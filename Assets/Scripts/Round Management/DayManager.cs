using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DayManager : MonoBehaviour
{
	public static DayManager Instance { get; private set; } = null;

	private void CheckSingleInstance()
	{
		DayManager.Instance ??= this;
		if (DayManager.Instance != this)
			Object.Destroy(this);
	}

	public enum Phase
	{
		Waiting,
		RegrowingVeggies,
		ResolvingWave,
		HarvestingVeggies,
		CollectingVeggies
	}

	public Phase CurrentPhase { get; private set; }

	[Header("Veggie Regrowth")]
	[SerializeField]
	[Tooltip("The delay between regrowing each veggie plot")]
	private RandomRange _regrowDelay = new RandomRange();

	public RandomRange RegrowDelay { get => this._regrowDelay; }

	[SerializeField]
	[Tooltip("Invoked when the regrowth phase begins")]
	private UnityEvent _onStartRegrow = new UnityEvent();

	public UnityEvent OnStartRegrow { get => this._onStartRegrow; }

	[SerializeField]
	[Tooltip("Invoked when the regrowth phase ends")]
	private UnityEvent _onEndRegrow = new UnityEvent();

	public UnityEvent OnEndRegrow { get => this._onEndRegrow; }

	private IEnumerator RegrowthPhase()
	{
		this.CurrentPhase = Phase.RegrowingVeggies;
		this.OnStartRegrow.Invoke();
		foreach (var plot in Object.FindObjectsOfType<VeggiePlot>())
		{
			plot.Regrow();
			yield return new WaitForSeconds(this.RegrowDelay.NextValue);
		}
		this.OnEndRegrow.Invoke();
		this.StartCoroutine(this.WavePhase());
	}

	[Header("Wave Resolution")]
	[SerializeField]
	[Tooltip("The wave manager used to trigger the next wave")]
	public WaveManager WaveManager;

	[SerializeField]
	[Tooltip("Invoked when the wave phase begins")]
	private UnityEvent _onStartWave = new UnityEvent();

	public UnityEvent OnStartWave { get => this._onStartWave; }

	[SerializeField]
	[Tooltip("Invoked when the wave phase ends")]
	private UnityEvent _onEndWave = new UnityEvent();

	public UnityEvent OnEndWave { get => this._onEndWave; }

	private IEnumerator WavePhase()
	{
		this.CurrentPhase = Phase.ResolvingWave;
		this.OnStartWave.Invoke();
		bool wave_complete = false;
		void OnWaveComplete() { wave_complete = true; }
		this.WaveManager.OnWaveComplete.AddListener(OnWaveComplete);
		this.WaveManager.SpawnNextWave();
		while (!wave_complete)
			yield return null;
		this.WaveManager.OnWaveComplete.RemoveListener(OnWaveComplete);
		this.OnEndWave.Invoke();
		this.StartCoroutine(this.HarvestPhase());
	}

	[Header("Veggie Harvesting")]
	[SerializeField]
	[Tooltip("The delay between harvesting veggie plots")]
	private RandomRange _harvestDelay = new RandomRange();

	public RandomRange HarvestDelay { get => this._harvestDelay; }

	[SerializeField]
	[Tooltip("Invoked when the harvest phase starts")]
	private UnityEvent _onStartHarvest = new UnityEvent();

	public UnityEvent OnStartHarvest { get => this._onStartHarvest; }

	[SerializeField]
	[Tooltip("Invoked when the harvest phase ends")]
	private UnityEvent _onEndHarvest = new UnityEvent();

	public UnityEvent OnEndHarvest { get => this._onEndHarvest; }

	private IEnumerator HarvestPhase()
	{
		this.CurrentPhase = Phase.HarvestingVeggies;
		this.OnStartHarvest.Invoke();
		foreach (var plot in Object.FindObjectsOfType<VeggiePlot>())
		{
			if (plot.HasBeenHarvested)
				continue;
			plot.Harvest();
			yield return new WaitForSeconds(this.HarvestDelay.NextValue);
		}
		this.OnEndHarvest.Invoke();
		this.StartCoroutine(this.CollectPhase());
	}

	[Header("Veggie Collection")]
	[SerializeField]
	[Tooltip("The delay between collecting each veggie")]
	private RandomRange _collectDelay = new RandomRange();

	public RandomRange CollectDelay { get => this._collectDelay; }

	[SerializeField]
	[Tooltip("The silo to collect veggies into")]
	public Silo Silo;

	[SerializeField]
	[Tooltip("Invoked when the collect phase begins")]
	private UnityEvent _onStartCollect = new UnityEvent();

	public UnityEvent OnStartCollect { get => this._onStartCollect; }

	[SerializeField]
	[Tooltip("Invoked when the collects phase ends")]
	private UnityEvent _onEndCollect = new UnityEvent();

	public UnityEvent OnEndCollect { get => this._onEndCollect; }

	private IEnumerator CollectPhase()
	{
		this.CurrentPhase = Phase.CollectingVeggies;
		this.OnStartCollect.Invoke();
		foreach (var veggie in Object.FindObjectsOfType<HarvestedVeggie>())
		{
			veggie.Collect(this.Silo);
			yield return new WaitForSeconds(this.CollectDelay.NextValue);
		}
		this.OnEndCollect.Invoke();
		this.CurrentPhase = Phase.Waiting;
	}

	public void StartNextDay()
	{
		if (this.CurrentPhase != Phase.Waiting)
			return;
		this.StartCoroutine(this.RegrowthPhase());
	}

	private void Awake()
	{
		this.CheckSingleInstance();
	}

	private void OnDestroy()
	{
		if (DayManager.Instance == this)
			DayManager.Instance = null;
	}
}
