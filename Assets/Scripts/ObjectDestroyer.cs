using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
	public new void Destroy(Object obj)
	{
		Object.Destroy(obj);
	}
}
