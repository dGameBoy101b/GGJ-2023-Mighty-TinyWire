using UnityEngine;

public class Vecto2AnimatorParameter : AnimatorParameter<Vector2>
{
	[SerializeField]
	[Tooltip("The suffix used for the x component")]
	public string XSuffix = ".x";

	public string XName {get => this.ParameterName + this.XSuffix; }

	[SerializeField]
	[Tooltip("The suffix used for the y component")]
	public string YSuffix = ".y";

	public string YName {get => this.ParameterName + this.YSuffix; }

	public Vector2 Value
	{
		get => new Vector2(this.Animator.GetFloat(this.XName), this.Animator.GetFloat(this.YName));
		set
		{
			this.Animator.SetFloat(this.XName, value.x);
			this.Animator.SetFloat(this.YName, value.y);
		}
	}
}
