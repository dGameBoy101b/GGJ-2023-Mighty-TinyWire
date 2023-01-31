using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Wave")]
public class Wave : ScriptableObject
{
	[SerializeField]
	[Tooltip("Minimum time in seconds between spawning enemies")]
	[Min(0)]
	private float _minimumDelay;

	public float MinimumDelay { get => this._minimumDelay; set => Mathf.Min(this.MaximumDelay, Mathf.Max(0, value)); }

	[SerializeField]
	[Tooltip("Maximum time in seconds between spawning enemies")]
	[Min(0)]
	private float _maximumDelay;

	public float MaximumDelay { get => this._maximumDelay; set => Mathf.Max(this.MinimumDelay, Mathf.Max(0, value)); }

	public float NextDelay { get => UnityEngine.Random.value * (this.MaximumDelay - this.MinimumDelay) + this.MinimumDelay; }

	[Serializable]
	public class EnemyGroup
	{
		[SerializeField]
		[Tooltip("The prefab used to spawn this enemy type")]
		public GameObject Prefab;

		[SerializeField]
		[Tooltip("The number of enemies this still needs to spawn")]
		[Min(0)]
		private int _quantity = 0;

		public int Quantity { get => this._quantity; set => Mathf.Max(0, value); }
	}

	[SerializeField]
	[Tooltip("The groups of enemies this will spawn in order")]
	public List<EnemyGroup> EnemyGroups = new List<EnemyGroup>();

	public int NextGroupIndex { get; private set; } = 0;

	public EnemyGroup NextGroup
	{
		get
		{
			while (this.NextGroupIndex < this.EnemyGroups.Count && this.EnemyGroups[this.NextGroupIndex].Quantity < 1)
				++this.NextGroupIndex;
			return this.NextGroupIndex < this.EnemyGroups.Count ? this.EnemyGroups[this.NextGroupIndex] : null;
		}
	}

	public GameObject DecrementNextEnemy()
	{
		var group = this.NextGroup;
		if (group != null)
			group.Quantity--;
		return group?.Prefab;
	}
}
