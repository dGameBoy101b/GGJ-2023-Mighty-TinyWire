using UnityEngine;
using UnityEngine.Events;

public class ShotVeggie : MonoBehaviour
{
	[SerializeField]
	[Tooltip("The mask used to determine what destroys this")]
	public LayerMask DestructionMask;

	[SerializeField]
	[Tooltip("Invoked just before this is destroyed")]
	private UnityEvent _onBreak = new UnityEvent();

	public UnityEvent OnBreak { get => this._onBreak; }

	private void OnDestroy()
	{
		this.OnBreak.Invoke();
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (LayerMaskComparer.IsObjectInMask(collision.gameObject, this.DestructionMask))
			Object.Destroy(this);
	}
}
