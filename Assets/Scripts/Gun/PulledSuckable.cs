using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class PulledSuckable : Suckable
{
	public bool IsBeingSucked { get; private set; } = false;

	public Rigidbody Body { get; protected set; }

	public Vacuum Vacuum { get; private set; } = null;

	public override void StartSuck(Vacuum origin)
	{
		this.IsBeingSucked = true;
		this.Vacuum = origin;
		base.StartSuck(origin);
	}

	public override void StopSuck(Vacuum origin)
	{
		this.IsBeingSucked = false;
		this.Vacuum = null;
		base.StopSuck(origin);
	}

	protected void PullTowardsVacuum()
	{
		if (!this.IsBeingSucked)
			return;
		var applied_force = this.Vacuum.GetAppliedForce(this.Body.position);
		var counter_inertia_force = -this.Body.velocity;
		var counter_gravity_force = this.Body.useGravity ? -Physics.gravity : Vector3.zero;
		var force = applied_force + counter_inertia_force + counter_gravity_force;
		this.Body.AddForce(force, ForceMode.Force);
	}

	protected virtual void FixedUpdate()
	{
		this.PullTowardsVacuum();
	}

	protected virtual void Awake()
	{
		this.Body ??= this.GetComponent<Rigidbody>();
	}
}
