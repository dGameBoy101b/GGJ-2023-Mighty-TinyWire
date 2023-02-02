using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Rat : MonoBehaviour
{
	[SerializeField]
	[Tooltip("The navmesh agent used to pathfind")]
	public NavMeshAgent Agent;

	public Transform Target = null;

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

	private void FindTargetSilo(bool overwrite = false)
	{
		if (overwrite || this.Target == null)
			this.Target = Silo.Instance.transform;
	}

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

	private bool IsObjectInMask(GameObject game_object, LayerMask mask)
	{
		var object_mask = LayerMask.GetMask(LayerMask.LayerToName(game_object.layer));
		return (object_mask & mask) > 0;
	}

	private void Start()
	{
		this.FindTargetSilo();
		this.UpdateDestination();
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (this.IsObjectInMask(collision.gameObject, this.StealMask))
		{
			var silo = collision.rigidbody?.gameObject.GetComponent<Silo>();
			if (silo != null)
				this.StealVeggie(silo);
		}
		if (this.IsObjectInMask(collision.gameObject, this.KnockoutMask))
		{
			this.KnockOut();
		}
	}
}
