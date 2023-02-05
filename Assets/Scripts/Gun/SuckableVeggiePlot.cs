using UnityEngine;

public class SuckableVeggiePlot : Suckable
{
	[SerializeField]
	[Tooltip("The plot to harvest while sucking")]
	public VeggiePlot Plot;

	public override void StartSuck(Vacuum origin)
	{
		this.Plot.IsBeingHarvested = true;
		base.StartSuck(origin);
	}

	public override void StopSuck(Vacuum origin)
	{
		this.Plot.IsBeingHarvested = false;
		base.StopSuck(origin);
	}
}
