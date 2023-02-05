using UnityEngine;

public class ShotRat : ShotObject
{
	protected override void HandelSettledObject(GameObject settled)
	{
		base.HandelSettledObject(settled);
		var rat = settled.GetComponent<Rat>();
		rat.KnockOut();
	}
}
