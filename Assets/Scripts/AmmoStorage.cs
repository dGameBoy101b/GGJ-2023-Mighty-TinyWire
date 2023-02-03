using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class AmmoStorage : MonoBehaviour
{
    public UnityEvent<int> OnCountChange;

    [SerializeField]
	[Tooltip("The number of carrots current held in the Vac Gun")]
    [Min(0)]
    private int _carrotCount;

    public int CarrotCount
    {
        get
        {
            return this._carrotCount;
        }
        set
        {
            value = Mathf.Max(value, 0);
            if (this._carrotCount == value)
            {
                return;
            }

            this._carrotCount = value;
            this.OnCountChange.Invoke(value);
        }
    }
}
