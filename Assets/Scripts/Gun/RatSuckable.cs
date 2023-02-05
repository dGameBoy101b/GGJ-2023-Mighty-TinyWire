using UnityEngine;

public class RatSuckable : PulledSuckable
{
	[SerializeField]
	[Tooltip("The rat script to be stunned")]
	public Rat Rat;

	public override void StartSuck(Vacuum origin)
	{
		this.Rat.Stun();
		base.StartSuck(origin);
	}

	public override void StopSuck(Vacuum origin)
	{
		this.Rat.ShouldRecover = true;
		base.StopSuck(origin);
	}

	public override void HitEnd(Vacuum origin)
	{
		//throw new System.NotImplementedException();
		base.HitEnd(origin);
	}
}
