using UnityEngine;

public class LayerMaskComparer
{
	public static bool IsObjectInMask(GameObject game_object, LayerMask mask)
	{
		var object_mask = LayerMask.GetMask(LayerMask.LayerToName(game_object.layer));
		return (object_mask & mask) > 0;
	}
}
