using UnityEngine;
using UnityEngine.Events;

public abstract class Suckable : MonoBehaviour
{
	[SerializeField]
	[Tooltip("Invoked when this hits the end of the vac gun with the vacuum it hit")]
	private UnityEvent<Vacuum> _onStartSuck = new UnityEvent<Vacuum>();

	public UnityEvent<Vacuum> OnStartSuck { get => this._onStartSuck; }

	[SerializeField]
	[Tooltip("Invoked when this hits the end of the vac gun with the vacuum it hit")]
	private UnityEvent<Vacuum> _onEndSuck = new UnityEvent<Vacuum>();

	public UnityEvent<Vacuum> OnEndSuck { get => this._onEndSuck; }

	[SerializeField]
	[Tooltip("Invoked when this hits the end of the vac gun with the vacuum it hit")]
	private UnityEvent<Vacuum> _onHitEnd = new UnityEvent<Vacuum>();

	public UnityEvent<Vacuum> OnHitEnd { get => this._onHitEnd; }

	public abstract void StartSuck(Vacuum origin);

	public abstract void StopSuck(Vacuum origin);

	public abstract void HitEnd(Vacuum origin);
}
