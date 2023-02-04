using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class CollectedVeggie : MonoBehaviour
{
	[SerializeField]
	[Tooltip("The mask for objects this should collect into")]
	public LayerMask CollectMask;

	[SerializeField]
	[Tooltip("The object to home towards")]
	public Transform Target = null;

	[SerializeField]
	[Tooltip("The impulse added when this spawns")]
	public Vector3 InitialImpulse;

	[SerializeField]
	[Tooltip("The torque added when this spawns")]
	private RandomVector3 _initalTorque = new RandomVector3();

	public RandomVector3 InitalTorque { get => this._initalTorque; }

	[SerializeField]
	[Tooltip("The speed at which this flies towards its target")]
	[Min(0)]
	private float _speed;

	public float Speed
	{
		get => this._speed;
		set
		{
			value = Mathf.Max(value, 0);
			this._speed = value;
		}
	}

	public Rigidbody Body { get; private set; }

	[SerializeField]
	[Tooltip("Invoked when this is collected")]
	private UnityEvent _onCollect = new UnityEvent();

	public UnityEvent OnCollect { get => this._onCollect; }

	private void StartCollection()
	{
		this.Body.AddForce(this.InitialImpulse - this.Body.velocity, ForceMode.Impulse);
		this.Body.AddTorque(this.InitalTorque.NextValue, ForceMode.Impulse);
	}

	private void PullToTarget(float delta_time)
	{
		Vector3 desired_force = Vector3.ClampMagnitude(this.Target.position - this.transform.position, this.Speed * delta_time);
		Debug.DrawRay(this.Body.position, desired_force);
		this.Body.AddForce(desired_force, ForceMode.VelocityChange);
	}

	private void Collect(Silo silo)
	{
		++silo.CarrotsStored;
		this.OnCollect.Invoke();
		Debug.Log("collected");
		Object.Destroy(this);
	}

	private void FixedUpdate()
	{
		this.PullToTarget(Time.fixedDeltaTime);
	}

	private void Start()
	{
		this.StartCollection();
	}

	private void Awake()
	{
		this.Body ??= this.GetComponent<Rigidbody>();
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (LayerMaskComparer.IsObjectInMask(collision.gameObject, this.CollectMask))
			this.Collect(collision.gameObject.GetComponent<Silo>());
	}
}
