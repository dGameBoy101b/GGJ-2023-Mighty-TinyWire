using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    public GameObject _projectile;

    public Rigidbody _playerRigidbody;

    public AmmoStorage _ammoStorage;

    public float _force;

    public RandomRange _torqueMagnitude;

    public float cooldownTime;

    private bool isShooting = false;

    private bool CanShoot = true;

    public void MyFireInput(InputAction.CallbackContext context) //Control scheme input values (is changed when the state of the input is change) (e.g. when w is pressed and when it is lifted)
    {
		isShooting = context.ReadValue<float>() > 0;
    }

    public void FixedUpdate()
    {
        if (isShooting)
        {
            SpawnProjectile();
        }
    }
    
    public void SpawnProjectile()
    {
        if (!CanShoot)
        {
            return;
        }

        GameObject _proj = Instantiate(_projectile, this.transform.position, this.transform.rotation);
        Rigidbody _rb = _proj.GetComponent<Rigidbody>();
        _rb.AddForce((_proj.transform.forward * _force) + _playerRigidbody.velocity, ForceMode.Impulse);
        _rb.AddTorque(Random.onUnitSphere * _torqueMagnitude.NextValue, ForceMode.Impulse);
        RestartCooldown();
    }

    public void RestartCooldown()
    {
        CanShoot = false;
        Invoke("EndCooldown", cooldownTime);
    }

    public void EndCooldown()
    {
        CanShoot = true;
    }
}
