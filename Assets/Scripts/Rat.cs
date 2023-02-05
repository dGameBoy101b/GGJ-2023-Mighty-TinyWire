using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Rat : MonoBehaviour
{
	[SerializeField]
	[Tooltip("The navmesh agent used to pathfind")]
	public NavMeshAgent Agent;

	public Rigidbody Body { get; private set; }

	[Header("Stealing")]
	[SerializeField]
	[Tooltip("The traget this rat tries to steal from")]
	private Transform _target = null;

	public Transform Target
	{
		get => this._target;
		set
		{
			var old = this.Target;
			this._target = value;
			if (old != value)
				this.UpdateDestination();
		}
	}

	[SerializeField]
	[Tooltip("The layers this can steal from")]
	public LayerMask StealMask;

	[SerializeField]
	[Tooltip("The event invoked when this successfully steals a veggie")]
	private UnityEvent _onSteal = new UnityEvent();

	public UnityEvent OnSteal { get => this._onSteal; }

	[Header("Knockout")]
	[SerializeField]
	[Tooltip("The layers this can be knocked out by")]
	public LayerMask KnockoutMask;

	public bool IsKnockedOut { get; private set; } = false;

	[SerializeField]
	[Tooltip("The event invoked when this is knocked out")]
	private UnityEvent _onKnockOut = new UnityEvent();

	public UnityEvent OnKnockOut { get => this._onKnockOut; }

	public bool IsStunned { get; private set; } = false;

	private bool _shouldRecover = false;

	public bool ShouldRecover 
	{
		get => this._shouldRecover;
		set
		{
			if (this.ShouldRecover == value)
				return;
			this._shouldRecover = value;
			this.UpdateIsStunned();
		}
	}

	private bool _canRecover = false;

	public bool CanRecover 
	{
		get => this._canRecover;
		private set
		{
			if (this.CanRecover == value)
				return;
			this._canRecover = value;
			this.UpdateIsStunned();
		}
	}

	private void UpdateIsStunned()
	{
		if (this.ShouldRecover && this.CanRecover)
			this.Recover();
	}

	[Header("Stunning")]
	[SerializeField]
	[Tooltip("The mask used to trigger recovery")]
	public LayerMask RecoveryMask;

	[SerializeField]
	[Tooltip("Invoked when stunned")]
	private UnityEvent _onStunned = new UnityEvent();

	public UnityEvent OnStunned { get => this._onStunned; }

	[SerializeField]
	[Tooltip("Invoked when recovered from a stun")]
	private UnityEvent _onRecovered = new UnityEvent();

	public UnityEvent OnRecovered { get => this._onRecovered; }

	private void UpdateDestination()
	{
		this.Agent.destination = this.Target?.position ?? this.transform.position;
	}

	public void StealVeggie(Silo silo)
	{
		--silo.CarrotsStored;
		this.OnSteal.Invoke();
	}

	public void KnockOut()
	{
		this.OnKnockOut.Invoke();
	}

	public void Stun()
	{
		if (this.IsStunned)
			return;
		this.IsStunned = true;
		this.ShouldRecover = false;
		this.OnStunned.Invoke();
	}

	public void Recover()
	{
		if (!this.IsStunned)
			return;
		this.IsStunned = false;
		this.Body.velocity = Vector3.zero;
		this.Body.angularVelocity = Vector3.zero;
		this.UpdateDestination();
		this.OnRecovered.Invoke();
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (LayerMaskComparer.IsObjectInMask(collision.gameObject, this.StealMask))
		{
			var silo = collision.rigidbody?.gameObject.GetComponent<Silo>();
			if (silo != null)
				this.StealVeggie(silo);
		}
		if (LayerMaskComparer.IsObjectInMask(collision.gameObject, this.KnockoutMask))
		{
			this.KnockOut();
		}
		if (LayerMaskComparer.IsObjectInMask(collision.gameObject, this.RecoveryMask))
		{
			this.CanRecover = true;
		}
	}

	private void OnCollisionExit(Collision collision)
	{
		if (LayerMaskComparer.IsObjectInMask(collision.gameObject, this.RecoveryMask))
		{
			this.CanRecover = false;
		}
	}

	private void Awake()
	{
		this.Body ??= this.GetComponent<Rigidbody>();
	}

	private void Start()
	{
		this.UpdateDestination();
	}
}
