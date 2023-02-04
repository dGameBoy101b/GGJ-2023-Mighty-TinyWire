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
	}

	public override void StopSuck(Vacuum origin)
	{
		this.IsBeingSucked = false;
		this.Vacuum = null;
	}

	protected void PullTowardsVacuum(float delta_time)
	{
		if (!this.IsBeingSucked)
			return;
		var force = this.Vacuum.GetAppliedForce(this.Body.position) * delta_time - this.Body.velocity;
		this.Body.AddForce(force, ForceMode.Force);
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
