using UnityEngine;

[System.Serializable]
public class RandomVector3
{
	[SerializeField]
	[Tooltip("The magnitude used")]
	private RandomRange _magnitude = new RandomRange();

	public RandomRange Magnitude { get => this._magnitude; }

	public Vector3 NextValue
	{
		get => Random.onUnitSphere * this.Magnitude.NextValue;
	}
}
