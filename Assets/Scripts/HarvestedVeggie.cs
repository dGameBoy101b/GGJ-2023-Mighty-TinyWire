using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class HarvestedVeggie : MonoBehaviour
{
	[SerializeField]
	[Tooltip("The prefab spawned when this is collected")]
	public GameObject CollectedPrefab;

	[SerializeField]
	[Tooltip("Invoked when this is collected")]
	private UnityEvent<Silo> _onCollect = new UnityEvent<Silo>();

	public UnityEvent<Silo> OnCollect { get => this._onCollect; }

	public Rigidbody Body { get; private set; }

	public void Collect(Silo silo)
	{
		var instance = Object.Instantiate(this.CollectedPrefab, this.transform.position, this.transform.rotation);
		var veggie = instance.GetComponent<CollectedVeggie>();
		veggie.Target = silo.transform;
		veggie.Body.velocity = this.Body.velocity;
		veggie.Body.angularVelocity = this.Body.angularVelocity;
		this.OnCollect.Invoke(silo);
		Object.Destroy(this);
	}

	private void Awake()
	{
		this.Body = this.GetComponent<Rigidbody>();
	}
}
