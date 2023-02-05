using UnityEngine;
using UnityEngine.Events;

public class ShotObject : MonoBehaviour
{

	[SerializeField]
	[Tooltip("The rigidbody on this")]
	private Rigidbody _body;

	public Rigidbody Body { get => this._body; protected set => this._body = value; }

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

	public void Settle()
	{
		Object.Destroy(this);
		var settled = Object.Instantiate(this.SettlePrefab, this.transform.position, this.transform.rotation);
		this.HandelSettledObject(settled);
		this.OnSettle.Invoke();
	}

	protected virtual void HandelSettledObject(GameObject settled)
	{
		var body = settled.GetComponent<Rigidbody>();
		body.velocity = this.Body.velocity;
		body.angularVelocity = this.Body.angularVelocity;
	}

	public virtual void Break()
	{
		Object.Destroy(this);
		this.OnBreak.Invoke();
	}

	protected void Awake()
	{
		this.Body ??= this.GetComponent<Rigidbody>();
	}

	protected void OnCollisionEnter(Collision collision)
	{
		if (LayerMaskComparer.IsObjectInMask(collision.gameObject, this.BreakMask))
			this.Break();
		else if (LayerMaskComparer.IsObjectInMask(collision.gameObject, this.SettleMask))
			this.Settle();
	}
}
