using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
public class Persuer : MonoBehaviour
{
	public Transform Target = null;

	public bool IsKnockedOut { get; private set; } = false;

	private UnityEvent _onKnockOut = new UnityEvent();

	public UnityEvent OnKnockOut { get => this._onKnockOut; }
}
