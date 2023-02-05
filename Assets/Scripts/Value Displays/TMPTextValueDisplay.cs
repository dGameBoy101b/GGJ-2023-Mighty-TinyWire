using UnityEngine;
using TMPro;

public class TMPTextValueDisplay<ValueType> : ValueDisplay<ValueType>
{
	[SerializeField]
	[Tooltip("The text element to insert values into")]
	public TMP_Text Output;

	public override void UpdateDisplay(ValueType value)
	{
		this.Output.text = value.ToString();
	}
}
