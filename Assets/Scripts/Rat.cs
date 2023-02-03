using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Rat : MonoBehaviour
{
	[SerializeField]
	[Tooltip("The navmesh agent used to pathfind")]
	public NavMeshAgent Agent;

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
	[Tooltip("The layers this can be knocked out by")]
	public LayerMask KnockoutMask;

	public bool IsKnockedOut { get; private set; } = false;

	[SerializeField]
	[Tooltip("The event invoked when this is knocked out")]
	private UnityEvent _onKnockOut = new UnityEvent();

	public UnityEvent OnKnockOut { get => this._onKnockOut; }

	[SerializeField]
	[Tooltip("The event invoked when this successfully steals a veggie")]
	private UnityEvent _onSteal = new UnityEvent();

	public UnityEvent OnSteal { get => this._onSteal; }

	private void UpdateDestination()
	{
		this.Agent.destination = this.Target.position;
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
	}
}
