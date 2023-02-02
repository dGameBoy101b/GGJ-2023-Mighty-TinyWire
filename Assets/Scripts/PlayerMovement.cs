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

	[SerializeField]
	[Tooltip("The speed at which the player rotates")]
	private float _rotationSpeed;

	public float RotationSpeed
	{
		get
		{
			return this._rotationSpeed;
		}
	}

    [SerializeField]
	[Tooltip("The speed modifier for sprinting")]
	private float _sprintMod;

	public float SprintMod
	{
		get
		{
			return this._sprintMod;
		}
	}

    [SerializeField]
	[Tooltip("The speed modifier for using the weapon")]
	private float _weaponUseMod;

	public float WeaponUseMod
	{
		get
		{
			return this._weaponUseMod;
		}
	}

    [HideInInspector]
	public bool isSucking;

	[HideInInspector]
	public bool isShooting;

	private bool isSprinting;

    private float currentSpeed;

	private Vector2 moveAxis;

	private Rigidbody _rb;

    private void Awake()
    {
        this._rb = this.GetComponent<Rigidbody>();
    }

    public void MyMoveInput(InputAction.CallbackContext context) //Control scheme input values (is changed when the state of the input is change) (e.g. when w is pressed and when it is lifted)
    {
		moveAxis = context.ReadValue<Vector2>();
    }

    public void MySprintInput(InputAction.CallbackContext context) //Control scheme input values (is changed when the state of the input is change) (e.g. when w is pressed and when it is lifted)
    {
		isSprinting = context.ReadValue<float>() > 0;
    }

	public void Look() //Manages the look & orientation information of the player (based on camera data)
    {
        if (!isSucking && !isShooting)
		{
			Vector3 input_dir = new Vector3(this.moveAxis.x, 0, this.moveAxis.y).normalized;
			Vector3 targ_dir = Quaternion.AngleAxis(45f, Vector3.up) * input_dir;
			if (this.moveAxis.x != 0 || this.moveAxis.y != 0)
			{
				this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(targ_dir, Vector3.up), Time.deltaTime * RotationSpeed);
			}
		}
		else
		{
			//rotation following mouse
			//rotation following right stick
		}
		
    }

    private void FixedUpdate()
    {
        this.currentSpeed = this._speed * (isShooting || isSucking ? WeaponUseMod : isSprinting ? SprintMod : 1f);
        Vector3 movement = new Vector3(this.moveAxis.x, 0, this.moveAxis.y).normalized * this.currentSpeed * Time.deltaTime;
        Vector3 rotatedMovement = Quaternion.AngleAxis(45f, Vector3.up) * movement;
        this._rb.AddForce(rotatedMovement - this._rb.velocity, ForceMode.Impulse);
		Look();
    }

}
