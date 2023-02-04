public class BooleanAnimatorParameter : AnimatorParameter<bool>
{
	public override bool Value 
	{
		get => this.Animator.GetBool(this.ParameterName);
		set => this.Animator.SetBool(this.ParameterName, value);
	}
}
