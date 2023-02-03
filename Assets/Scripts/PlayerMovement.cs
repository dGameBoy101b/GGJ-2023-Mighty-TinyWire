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

    public PlayerInput _playerInput;

	public Camera _cam;

    [HideInInspector]
	public bool isSucking;

	[HideInInspector]
	public bool isShooting;

	public float screenRayLength = Mathf.Infinity;

	public LayerMask screenRayMask;

	[Tooltip("Whether or not the screen ray interacts wwith triggers")]
	public QueryTriggerInteraction _qti;

	[Tooltip("The range around zero in which the input is ignored for rotating the character")]
	public float lookThreshold;

	private bool isSprinting;

    private float currentSpeed;

	private Vector2 moveAxis;

	private Vector2 aimAxis;

	private Rigidbody _rb;

    private void Awake()
    {
        this._rb = this.GetComponent<Rigidbody>();
		this._cam ??= Camera.main;
    }

    public void MyMoveInput(InputAction.CallbackContext context) //Control scheme input values (is changed when the state of the input is change) (e.g. when w is pressed and when it is lifted)
    {
		this.moveAxis = context.ReadValue<Vector2>();
    }

	public void MyAimInput(InputAction.CallbackContext context) //Control scheme input values (is changed when the state of the input is change) (e.g. when w is pressed and when it is lifted)
    {
		this.aimAxis = context.ReadValue<Vector2>();
		//Debug.Log("1: " + this.aimAxis);
		//Debug.Log(_playerInput.currentControlScheme);
		if (context.control.device is Pointer)
		{
			//Debug.Log("2: " + this.aimAxis);
			Ray ray = _cam.ScreenPointToRay(new Vector3(this.aimAxis.x, this.aimAxis.y, 0));
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, screenRayLength, screenRayMask, _qti))
			{
				Vector3 dirToHit = hit.point - this.transform.position;
				Vector3 rotDirToHit = Quaternion.AngleAxis(-45f, Vector3.up) * dirToHit;
				this.aimAxis = new Vector2(rotDirToHit.x, rotDirToHit.z);
			}
		}
    }

    public void MySprintInput(InputAction.CallbackContext context) //Control scheme input values (is changed when the state of the input is change) (e.g. when w is pressed and when it is lifted)
    {
		this.isSprinting = context.ReadValue<float>() > 0;
    }

	public void Look() //Manages the look information of the player
    {
		Vector3 input_dir = new Vector3(this.aimAxis.x, 0, this.aimAxis.y).normalized;
		Vector3 targ_dir = Quaternion.AngleAxis(45f, Vector3.up) * input_dir;
		if ((this.aimAxis.x < -lookThreshold || this.aimAxis.x > lookThreshold) && (this.aimAxis.y < -lookThreshold || this.aimAxis.y > lookThreshold))
		{
			this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(targ_dir, Vector3.up), Time.deltaTime * RotationSpeed);
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
