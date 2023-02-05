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

	protected void PullTowardsVacuum(float delta_time)
	{
		if (!this.IsBeingSucked)
			return;
		var applied_force = this.Vacuum.GetAppliedForce(this.Body.position) * delta_time;
		var counter_inertia = -this.Body.velocity;
		var counter_gravity = (this.Body.useGravity ? -Physics.gravity : Vector3.zero) * delta_time;
		var force = applied_force + counter_inertia + counter_gravity;
		this.Body.AddForce(force, ForceMode.Impulse);
	}

	protected virtual void FixedUpdate()
	{
		this.PullTowardsVacuum(Time.fixedDeltaTime);
	}

	protected virtual void Awake()
	{
		this.Body ??= this.GetComponent<Rigidbody>();
	}
}
