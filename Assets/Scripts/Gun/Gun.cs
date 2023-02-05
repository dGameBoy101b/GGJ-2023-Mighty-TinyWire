using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
	[SerializeField]
	[Tooltip("The prefab used to shoot while not jammed")]
	public GameObject DefaultProjectile;

	[SerializeField]
	[Tooltip("The prefab used to shoot while jammed")]
	public GameObject JammedProjectile;

	public Rigidbody Body;

	[SerializeField]
	[Tooltip("The ammo storage used to shoot")]
	private AmmoStorage _ammoStorage;

	public AmmoStorage AmmoStorage
	{
		get => this._ammoStorage;
		set
		{
			this.RemoveJamListeners();
			this._ammoStorage = value;
			this.AddJamListeners();
		}
	}

	[SerializeField]
	[Tooltip("The impulse applied to projectiles")]
	public float Impulse;

	[SerializeField]
	[Tooltip("The torque applied to projectiles")]
	public RandomVector3 Torque;

	[SerializeField]
	[Tooltip("The delay between shots when this is not jammed")]
	public float DefaultCooldownTime;

	[SerializeField]
	[Tooltip("The delay between this jamming and shooting again")]
	public float JammedCooldownTime;

	public bool IsShooting { get; private set; } = false;

	public bool CanShoot { get; private set; } = true;

	public void MyFireInput(InputAction.CallbackContext context)
	{
		this.IsShooting = context.ReadValue<float>() > 0;
	}

	private void AddJamListeners()
	{
		if (this.AmmoStorage == null)
			return;
		this.AmmoStorage.OnJam.AddListener(this.OnJam);
	}

	private void RemoveJamListeners()
	{
		if (this.AmmoStorage == null)
			return;
		this.AmmoStorage.OnJam.RemoveListener(this.OnJam);
	}

	private void OnJam()
	{
		this.RestartCooldown(this.JammedCooldownTime);
	}
	
	private void SpawnProjectile()
	{
		if (!this.CanShoot || (!this.AmmoStorage.IsJammed && this.AmmoStorage.AmmoCount < 1))
			return;
		GameObject projectile_prefab = this.AmmoStorage.IsJammed ? this.JammedProjectile : this.DefaultProjectile;
		if (this.AmmoStorage.IsJammed)
			this.AmmoStorage.IsJammed = false;
		else
			--this.AmmoStorage.AmmoCount;
		GameObject projectile = Instantiate(projectile_prefab, this.transform.position, this.transform.rotation);
		Rigidbody projectile_body = projectile.GetComponent<Rigidbody>();
		projectile_body.AddForce((projectile.transform.forward * this.Impulse) + Body.velocity, ForceMode.Impulse);
		projectile_body.AddTorque(this.Torque.NextValue, ForceMode.Impulse);
		this.RestartCooldown(this.DefaultCooldownTime);
	}

	public void RestartCooldown(float cooldown)
	{
		this.CanShoot = false;
		this.Invoke("EndCooldown", cooldown);
	}

	private void EndCooldown()
	{
		this.CanShoot = true;
	}

	private void FixedUpdate()
	{
		if (this.IsShooting)
		{
			this.SpawnProjectile();
		}
	}

	private void Awake()
	{
		this.AddJamListeners();
	}
}
