using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
	[SerializeField]
	[Tooltip("The object destroyed if no object is given")]
	public Object DefaultObject;

	public new void Destroy(Object obj = null)
	{
		obj ??= this.DefaultObject;
		if (obj != null)
			Object.Destroy(obj);
	}
}
