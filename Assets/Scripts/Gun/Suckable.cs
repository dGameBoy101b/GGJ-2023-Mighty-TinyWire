using UnityEngine;
using UnityEngine.Events;

public class Suckable : MonoBehaviour
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

	public virtual void StartSuck(Vacuum origin)
	{
		this.OnStartSuck.Invoke(origin);
	}

	public virtual void StopSuck(Vacuum origin)
	{
		this.OnEndSuck.Invoke(origin);
	}

	public virtual void HitEnd(Vacuum origin)
	{
		this.OnHitEnd.Invoke(origin);
	}
}
