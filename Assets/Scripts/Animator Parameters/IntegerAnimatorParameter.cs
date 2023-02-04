public class IntegerAnimatorParameter : AnimatorParameter<int>
{
	public override int Value 
	{ 
		get => this.Animator.GetInteger(this.ParameterName);
		set => this.Animator.SetInteger(this.ParameterName, value);
	}
}
