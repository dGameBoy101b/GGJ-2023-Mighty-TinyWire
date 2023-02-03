using UnityEngine;

public class VeggieHarvester : MonoBehaviour
{
	[SerializeField]
	[Tooltip("The veggie plot to harvest")]
	public VeggiePlot Plot;

	[SerializeField]
	[Tooltip("Whether this should be harvested or regrown")]
	public bool ShouldHarvest = false;

	private void Update()
	{
		if (this.Plot != null && this.Plot.HasBeenHarvested != this.ShouldHarvest)
		{
			this.Plot.IsBeingHarvested = this.ShouldHarvest;
			if (!this.ShouldHarvest && this.Plot.HasBeenHarvested)
				this.Plot.Regrow();
		}
	}
}
