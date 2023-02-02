using UnityEngine;

public class GateLogger : MonoBehaviour
{
	public Gate Gate;

	public void Awake()
	{
		this.Gate.OnOpen.AddListener(() => Debug.Log("Gate opened: " + this.Gate.name));
		this.Gate.OnClose.AddListener(() => Debug.Log("Gate closed: " + this.Gate.name));
	}
}
