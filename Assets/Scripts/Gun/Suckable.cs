using UnityEngine;

public abstract class Suckable : MonoBehaviour
{
	public abstract void StartSuck(Vacuum origin);

	public abstract void StopSuck(Vacuum origin);

	public abstract void HitEnd(Vacuum origin);
}
