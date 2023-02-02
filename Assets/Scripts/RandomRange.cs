using UnityEngine;

[System.Serializable]
public class RandomRange
{
	[Tooltip("The minimum value")]
	public float Minimum;

	[Tooltip("The maximum value")]
	public float Maximum;

	public float NextValue { get => Random.Range(this.Minimum, this.Maximum); }
}
