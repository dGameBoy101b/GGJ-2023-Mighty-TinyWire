using UnityEngine;
using UnityEngine.Events;

public class VeggiePlot : MonoBehaviour
{
	[SerializeField]
	[Tooltip("The prefab spawned once this has been harvested")]
	public GameObject LoosePrefab;

	[SerializeField]
	[Tooltip("The offset between this and where the loose prefab is spawned")]
	public Vector3 LooseOffset;

	[SerializeField]
	[Tooltip("The amount of work required to harvest this")]
	[Min(0)]
	private float _maximumHarvest;

	public float MaximumHarvest
	{
		get => this._maximumHarvest;
		set
		{
			var old_proportion = this.HarvestProportion;
			this._maximumHarvest = Mathf.Max(value, 0);
			this.UpdateHarvestProportion(old_proportion);
		}
	}

	[SerializeField]
	[Tooltip("The rate at which harvest work decreases while no work is done")]
	public float HarvestDecay;

	[SerializeField]
	[Tooltip("The rate at which harvest work increases while work is done")]
	public float HarvestRate;

	private float _harvestWork = 0;

	public float HarvestWork
	{
		get => this._harvestWork;
		set
		{
			var old_proportion = this.HarvestProportion;
			this._harvestWork = Mathf.Clamp(value, 0, this.MaximumHarvest);
			this.UpdateHarvestProportion(old_proportion);
		}
	}

	public float HarvestProportion { get => this.HarvestWork / this.MaximumHarvest; }

	[SerializeField]
	[Tooltip("Invoked when the proportion of harvest work changes")]
	private UnityEvent<float> _onHarvestWorkPrportionChange = new UnityEvent<float>();

	public UnityEvent<float> OnHarvestWorkPrportionChange { get => this._onHarvestWorkPrportionChange; }


	[SerializeField]
	[Tooltip("Invoked when this is successfully harvested")]
	private UnityEvent _onHarvest = new UnityEvent();

	public UnityEvent OnHarvest { get => this._onHarvest; }

	public bool HasBeenHarvested { get; private set; } = false;

	public bool IsBeingHarvested { get; set; } = false;

	private void UpdateHarvestProportion(float old)
	{
		var proportion = this.HarvestProportion;
		if (proportion != old)
			this.OnHarvestWorkPrportionChange.Invoke(proportion);
		if (proportion >= 1f)
			this.Harvest();
	}

	private void UpdateHarvest(float delta_time)
	{
		if (this.HasBeenHarvested || !this.IsBeingHarvested)
			return;
		this.HarvestWork += (this.IsBeingHarvested ? this.HarvestRate : -this.HarvestDecay) * delta_time;
	}

	[SerializeField]
	[Tooltip("Invoked when this regrows")]
	private UnityEvent _onRegrow = new UnityEvent();

	public UnityEvent OnRegrow { get => this._onRegrow; }

	public void Harvest()
	{
		if (this.HasBeenHarvested)
			return;
		this.HasBeenHarvested = true;
		Object.Instantiate(this.LoosePrefab, this.transform.position + this.LooseOffset, Quaternion.identity);
		this.OnHarvest.Invoke();
	}

	public void Regrow()
	{
		if (!this.HasBeenHarvested)
			return;
		this.HasBeenHarvested = false;
		this.HarvestWork = 0f; 
		this.OnRegrow.Invoke();
	}

	private void Update()
	{
		this.UpdateHarvest(Time.deltaTime);
	}
}
