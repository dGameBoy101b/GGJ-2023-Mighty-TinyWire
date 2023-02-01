using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Settings")]
	
	[SerializeField]
	[Tooltip("The speed at which the player moves")]
	private float _speed;

	public float Speed
	{
		get
		{
			return this._speed;
		}
	}

	private Vector2 moveAxis;

	private Rigidbody _rb;

	//[SerializeField]
	//private ParticleSystem walkingParticles; 

    private void Awake()
    {
        this._rb = this.GetComponent<Rigidbody>();
    }

    public void MyInput(InputAction.CallbackContext context) //Control scheme input values (is changed when the state of the input is change) (e.g. when w is pressed and when it is lifted)
    {
		moveAxis = context.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(this.moveAxis.normalized.x, 0, this.moveAxis.normalized.y) * this.Speed * Time.deltaTime;
        Vector3 rotatedMovement = Quaternion.AngleAxis(45f, Vector3.up) * movement;
        this._rb.AddForce(rotatedMovement - this._rb.velocity, ForceMode.Impulse);
    }

}
