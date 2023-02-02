using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Rat : MonoBehaviour
{
	[SerializeField]
	[Tooltip("The navmesh agent used to pathfind")]
	public NavMeshAgent Agent;

	public Transform Target = null;

	public bool IsKnockedOut { get; private set; } = false;

	[SerializeField]
	[Tooltip("The event invoked when this is knocked out")]
	private UnityEvent _onKnockOut = new UnityEvent();

	public UnityEvent OnKnockOut { get => this._onKnockOut; }

	private void FindTargetSilo()
	{
		this.Target = Silo.Instance.transform;
	}

	private void UpdateDestination()
	{
		this.Agent.destination = this.Target.position;
	}

	private void Start()
	{
		this.FindTargetSilo();
		this.UpdateDestination();
	}
}
