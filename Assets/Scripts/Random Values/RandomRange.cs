using UnityEngine;

[System.Serializable]
public class RandomRange : IRandomValue<float>
{
	[SerializeField]
	[Tooltip("The minimum value")]
	public float Minimum;

	[SerializeField]
	[Tooltip("The maximum value")]
	public float Maximum;

	public float NextValue { get => Random.Range(this.Minimum, this.Maximum); }
}
