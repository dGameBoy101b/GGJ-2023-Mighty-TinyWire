using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class Vacuum : MonoBehaviour
{
    public AmmoStorage _ammoStorage;

    public Rigidbody _playerRigidbody;

    private bool isSucking = false;

    public float _force;

    public RandomRange _torqueMagnitude;

    private bool CanSuck = true;

    private HashSet<Rigidbody> inRangeObjects = new HashSet<Rigidbody>();

    public void MySuckInput(InputAction.CallbackContext context) //Control scheme input values (is changed when the state of the input is change) (e.g. when w is pressed and when it is lifted)
    {
		isSucking = context.ReadValue<float>() > 0;
    }

    void OnTriggerEnter(Collider collider)
    {
        inRangeObjects.Add(col.GameObject.GetComponent<Rigidbody>());
    }

    void OnTriggerExit(Collider collider)
    {
        inRangeObjects.Remove(col.GameObject.GetComponent<Rigidbody>());
    }

    void OnCollisionEnter(Collision col)
    {
        Object.Destroy(col.gameObject);
        _ammoStorage.CarrotCount++;
    }

    public void FixedUpdate()
    {
        if (isSucking)
        {
            AttractObject();
        }
    }

    public void AttractObject()
    {
        if (!CanSuck)
        {
            return;
        }

        foreach(Rigidbody _rb in inRangeObjects)
        {
            _rb.AddForce(_rb.velocity - (this.transform.position - _rb.transform.position * _force) + _playerRigidbody.velocity, ForceMode.Impulse);
            _rb.AddTorque(Random.onUnitSphere * _torqueMagnitude.NextValue, ForceMode.Impulse);
        }
    }
}
