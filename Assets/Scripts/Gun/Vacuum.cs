using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Vacuum : MonoBehaviour
{
	[SerializeField]
	[Tooltip("The ammo storage to add ammo to")]
	private AmmoStorage _ammoStorage;

	public AmmoStorage AmmoStorage
	{
		get => this._ammoStorage;
		set
		{
			this.RemoveJamListeners();
			this._ammoStorage = value;
			this.AddJamListeners();
		}
	}

	[Header("Suction Force")]
	[SerializeField]
	[Tooltip("The force at which this sucks")]
	[Min(0)]
	private float _baseForce;

	public float BaseForce
	{
		get => this._baseForce;
		set
		{
			value = Mathf.Max(0, value);
			this._baseForce = value;
		}
	}

	[SerializeField]
	[Tooltip("The coefficient applied to the distance of something being sucked")]
	public float DistanceCoefficient;

	public Vector3 GetAppliedForce(Vector3 position)
	{
		var direction = this.transform.position - position;
		return direction.normalized * (this.BaseForce + this.DistanceCoefficient * direction.magnitude);
	}

	private bool _shouldSuck = false;

	public bool ShouldSuck 
	{ 
		get => this._shouldSuck; 
		private set
		{
			if (value == this.ShouldSuck)
				return;
			this._shouldSuck = value;
			if (this.AmmoStorage != null && this.AmmoStorage.IsJammed)
				return;
			if (this.IsSucking)
				this.StartSuck();
			else
				this.StopSuck();
		}
	}

	private void AddJamListeners()
	{
		if (this.AmmoStorage == null)
			return;
		this.AmmoStorage.OnJam.AddListener(this.OnJam);
		this.AmmoStorage.OnUnjam.AddListener(this.OnUnjam);
	}

	private void RemoveJamListeners()
	{
		if (this.AmmoStorage == null)
			return;
		this.AmmoStorage.OnJam.RemoveListener(this.OnJam);
		this.AmmoStorage.OnUnjam.RemoveListener(this.OnUnjam);
	}

	private void OnJam()
	{
		if (this.ShouldSuck)
			this.StopSuck();
	}

	private void OnUnjam()
	{
		if (this.IsSucking)
			this.StartSuck();
	}

	public bool IsSucking { get => this.ShouldSuck && !this.AmmoStorage.IsJammed; }

	[Header("Suction Events")]
	[SerializeField]
	[Tooltip("Invoked when this starts sucking")]
	private UnityEvent _onStartSuck = new UnityEvent();

	public UnityEvent OnStartSuck { get => this._onStartSuck; }

	[SerializeField]
	[Tooltip("Invoked when this stops sucking")]
	private UnityEvent _onEndSuck = new UnityEvent();

	public UnityEvent OnEndSuck { get => this._onEndSuck; }

	private HashSet<Suckable> _inRangeObjects = new HashSet<Suckable>();

	public HashSet<Suckable> InRangeObjects { get => this._inRangeObjects; }

	public void MySuckInput(InputAction.CallbackContext context)
	{
		this.ShouldSuck = context.ReadValue<float>() > 0;
	}

	private void StartSuck()
	{
		this.OnStartSuck.Invoke();
	}

	private void StopSuck()
	{
		var objects = new HashSet<Suckable>(this.InRangeObjects);
		foreach (var target in objects)
			this.StopSucking(target);
		this.OnEndSuck.Invoke();
	}

	private void StartSucking(Suckable suckable)
	{
		if (!this.IsSucking)
			return;
		this.InRangeObjects.Add(suckable);
		suckable?.StartSuck(this);
	}

	private void StopSucking(Suckable suckable)
	{
		this.InRangeObjects.Remove(suckable);
		suckable?.StopSuck(this);
	}

	private void HitSuckable(Suckable suckable)
	{
		suckable?.HitEnd(this);
	}

	void OnTriggerEnter(Collider collider)
	{
		this.StartSucking(collider.attachedRigidbody?.GetComponent<Suckable>());
	}

	void OnTriggerExit(Collider collider)
	{
		this.StopSucking(collider.attachedRigidbody?.GetComponent<Suckable>());
	}

	void OnCollisionEnter(Collision col)
	{
		this.HitSuckable(col.rigidbody?.GetComponent<Suckable>());
	}
}
