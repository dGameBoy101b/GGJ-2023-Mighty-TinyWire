using UnityEngine;
using UnityEngine.Events;

public class ShotVeggie : MonoBehaviour
{
	[Header("Destruction")]
	[SerializeField]
	[Tooltip("The mask used to determine what destroys this")]
	public LayerMask BreakMask;

	[SerializeField]
	[Tooltip("Invoked just before this is destroyed")]
	private UnityEvent _onBreak = new UnityEvent();

	public UnityEvent OnBreak { get => this._onBreak; }

	[Header("Settling")]
	[SerializeField]
	[Tooltip("The mask used to determine what settles this into its harmless form")]
	public LayerMask SettleMask;

	[SerializeField]
	[Tooltip("The prefab spawned when this settles")]
	public GameObject SettlePrefab;

	[SerializeField]
	[Tooltip("Invoked when this settles into its harmless form")]
	private UnityEvent _onSettle = new UnityEvent();

	public UnityEvent OnSettle { get => this._onSettle; }

	[SerializeField]
	[Tooltip("The rigidbody on this")]
	private Rigidbody Body;

	public void Settle()
	{
		Object.Destroy(this);
		var settled = Object.Instantiate(this.SettlePrefab, this.transform.position, this.transform.rotation);
		var body = settled.GetComponent<Rigidbody>();
		body.velocity = this.Body.velocity;
		body.angularVelocity = this.Body.angularVelocity;
		this.OnSettle.Invoke();
	}

	public void Break()
	{
		Object.Destroy(this);
		this.OnBreak.Invoke();
	}

	private void Awake()
	{
		this.Body ??= this.GetComponent<Rigidbody>();
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (LayerMaskComparer.IsObjectInMask(collision.gameObject, this.BreakMask))
			this.Break();
		else if (LayerMaskComparer.IsObjectInMask(collision.gameObject, this.SettleMask))
			this.Settle();
	}
}
