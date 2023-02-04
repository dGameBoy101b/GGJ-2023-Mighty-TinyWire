using UnityEngine;

public class VeggieSuckable : PulledSuckable
{
	public override void HitEnd(Vacuum origin)
	{
		++origin.AmmoStorage.AmmoCount;
		Object.Destroy(this);
	}
}
