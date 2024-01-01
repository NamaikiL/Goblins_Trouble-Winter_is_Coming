using System.Collections.Generic;
using _Scripts.Gameplay.Enemies;
using UnityEngine;

namespace _Scripts.Gameplay.Towers.Types
{
	[System.Serializable]
	public class GoblinGrooveStats
	{
		[Header("Tower Parameters")]
		public float rangeGroove;
		
		[Header("Towers Buff Parameters")]
		public float damageMultiplier;
		public float firerateMultiplier;
		public bool isBulletFireModifier;

		[Header("Enemies Nerf Parameters")] 
		public float speedMultiplier;
	}
	
	public class TowerGoblinGroove : TowerFeatures
	{

		#region Variables

		[Header("Groove Properties")]
		[SerializeField] private Transform rangeVisual;
		[SerializeField] private List<GoblinGrooveStats> towerStats;

		// Buff Objects Variables.
		private Collider[] _alliesInRange;
		private Collider[] _enemiesInRange;
	
		#endregion

		#region Properties

		public float TowerRange => towerStats[CurrentLevel].rangeGroove;
		
		#endregion

		#region Builtin Methods

		/**
		 * <summary>
		 * Start is called before the first frame update.
		 * </summary>
		 */
		protected override void Start()
		{
			base.Start();
		}

		
		/**
		 * <summary>
		 * Update is called once per frame.
		 * </summary>
		 */ 
		void Update()
		{
			if(!IsStun)
			{
				CheckAlliesAndApplyEffect();
				CheckEnemiesAndApplyEffect();
			}
			else
			{
				AlliesRemoveBuff();
				EnemiesRemoveNerf();
			}
		}

		#endregion

		#region Custom Methods

		public override void Upgrade()
		{
			if(CurrentLevel < maxLevel - 1)
			{
				AlliesRemoveBuff();
				EnemiesRemoveNerf();
			}
			base.Upgrade();
		}


		/**
		 * <summary>
		 * Function to Update the visual of the specific tower.
		 * </summary>
		 */
		protected override void InstantiateChildVisual()
		{
			rangeVisual.localScale = new Vector3(towerStats[CurrentLevel].rangeGroove * 2f, 0f, towerStats[CurrentLevel].rangeGroove * 2f);
		}
		

		/**
		 * <summary>
		 * Function to Check all allies in range and apply effect to them.
		 * </summary>
		 */
		private void CheckAlliesAndApplyEffect()
		{
			_alliesInRange = Physics.OverlapSphere(transform.position, towerStats[CurrentLevel].rangeGroove, 1<<8);

			Debug.Log(_alliesInRange.Length);
			foreach (Collider ally in _alliesInRange)
			{
				TowerFire tower = ally.GetComponent<TowerFire>();
				if (tower)
				{
					tower.TowerBuff(
						towerStats[CurrentLevel].damageMultiplier, 
						towerStats[CurrentLevel].firerateMultiplier, 
						towerStats[CurrentLevel].isBulletFireModifier
						);
				}
			}
		}


		/**
		 * <summary>
		 * Function to Check all enemies in range and apply effect to them.
		 * </summary>
		 */
		private void CheckEnemiesAndApplyEffect()
		{
			_enemiesInRange = Physics.OverlapSphere(transform.position, towerStats[CurrentLevel].rangeGroove + 1f, 1<<7);
			
			foreach (Collider enemy in _enemiesInRange)
			{
				float distanceEnemy = Vector3.Distance(transform.position, enemy.transform.position);
				if (_enemiesInRange.Length != 0 && distanceEnemy <= towerStats[CurrentLevel].rangeGroove)
				{
					enemy.GetComponent<Enemy>().EnemyNerf(towerStats[CurrentLevel].speedMultiplier);
				}
				else if (_enemiesInRange.Length != 0 && distanceEnemy > towerStats[CurrentLevel].rangeGroove)
				{
					enemy.GetComponent<Enemy>().EnemyUnNerf();
				}
			}
		}


		/**
		 * <summary>
		 * Function to remove the buff of all allies.
		 * </summary>
		 */
		private void AlliesRemoveBuff()
		{
			foreach (Collider ally in _alliesInRange)
			{
				if(ally.GetComponent<TowerFire>()) ally.GetComponent<TowerFire>().TowerUnBuff();
			}
		}


		/**
		 * <summary>
		 * Function to remove all the nerf of all enemies.
		 * </summary>
		 */
		private void EnemiesRemoveNerf()
		{
			foreach (Collider enemy in _enemiesInRange)
			{
				enemy.GetComponent<Enemy>().EnemyUnNerf();
			}
		}

		#endregion
		
	}
}
