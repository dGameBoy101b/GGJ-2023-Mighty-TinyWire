using UnityEngine;

public abstract class PulledSuckable : Suckable
{
	public bool IsBeingSucked { get; private set; } = false;

	public Rigidbody Body { get; protected set; }

	public Vacuum Origin { get; private set; } = null;

	public override void StartSuck(Vacuum origin)
	{
		this.IsBeingSucked = true;
		this.Origin = origin;
	}

	public override void StopSuck(Vacuum origin)
	{
		this.IsBeingSucked = false;
		this.Origin = null;
	}

	protected void PullTowardsOrigin(float delta_time)
	{
		if (!this.IsBeingSucked)
			return;
		this.Body.AddForce(this.Origin.GetAppliedForce(this.Body.position) - this.Body.velocity, ForceMode.Force);
	}
}
