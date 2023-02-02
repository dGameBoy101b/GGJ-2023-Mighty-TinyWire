using UnityEngine;

public abstract class AnimatorParameter<ValueType> : MonoBehaviour
{
	[SerializeField]
	[Tooltip("The animator to interface with")]
	public Animator Animator;

	[SerializeField]
	[Tooltip("The name of the parameter in the animator")]
	public string ParameterName;

	public abstract ValueType Value { get; set; }
}
