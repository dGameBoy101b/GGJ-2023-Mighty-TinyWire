using UnityEngine;

public abstract class ValueDisplay<ValueType> : MonoBehaviour
{
	public abstract void UpdateDisplay(ValueType value);
}
