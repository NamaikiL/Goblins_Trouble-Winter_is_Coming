using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Gameplay.Enemies;
using UnityEngine;
using UnityEngine.AI;

namespace _Scripts.Gameplay.Towers
{
	public enum FireType
	{
		First,
		Last,
		Strongest,
		Weakest
	}
	
	[Serializable]
	public class TowerLevelStats
	{
		public float damages;
		public float firerate;
		public float range;
		public bool isFire;
		public GameObject bullet;
	}

	public class TowerFire : TowerFeatures
	{

		#region Variables

		[Header("Fire Properties")] 
		[SerializeField] private FireType fireType;
		[SerializeField] private Transform rangeVisual;
		[SerializeField] protected GameObject[] shootingPoints;
		[SerializeField] protected List<TowerLevelStats> towerFireLevelStats;
	
		// Distance Variables.
		private float _currentDistance; 
		private float _shortestDistance;
		
		// Buffed Variables.
		private float _originalDamage;
		private float _originalFirerate;
		private bool _originalIsFire;
		private bool _isBuffed;
	
		// Enemies variables.
		private Transform _targetEnemy;
		private Collider[] _enemiesInRange;
		
		// Enum Types Index.
		private int _currentEnum;
	
		#endregion

		#region Properties

		public float TowerDamage => towerFireLevelStats[CurrentLevel].damages;
		public float TowerFirerate => towerFireLevelStats[CurrentLevel].firerate;
		public float TowerRange => towerFireLevelStats[CurrentLevel].range;
		public bool TowerBullet => towerFireLevelStats[CurrentLevel].isFire;
		
		protected Transform Target => _targetEnemy;

		public FireType TypeFire => fireType;

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
			StartCoroutine(Detection());
		}

		#endregion
	
		#region Turret Behavior
		
		/**
		 * <summary>
		 * Function that update the visual of the range.
		 * </summary>
		 */
		protected override void InstantiateChildVisual()
		{
			rangeVisual.localScale = new Vector3(towerFireLevelStats[CurrentLevel].range * 2f, 0f, towerFireLevelStats[CurrentLevel].range * 2f);
			UpdateShootingPoints();
		}


		/**
		 * <summary>
		 * Function that stock all the shooting points of a turret head.
		 * </summary>
		 */
		private void UpdateShootingPoints()
		{
			if (CurrentLevel < maxLevel)
			{
				Array.Clear(shootingPoints, 0, shootingPoints.Length);
			
				if(TurretHead)
				{
					shootingPoints = GameObject.FindGameObjectsWithTag("Shooting");
					shootingPoints = Array.FindAll(shootingPoints,
						shootPoint => shootPoint.transform.IsChildOf(TurretHead.transform));
				}
			}
		}
	

		/**
		 * <summary>
		 * Function that turn the head of the turret to the first enemy of the raw.
		 * </summary>
		 */
		protected virtual void Fire()
		{
			if (!_targetEnemy) return;
			if (TurretHead)
			{ 
				if(Target && !Target.GetComponent<Enemy>().NeedFire
				   || Target && Target.GetComponent<Enemy>().NeedFire && towerFireLevelStats[CurrentLevel].isFire)
					TurretHead.transform.LookAt(_targetEnemy);
			}
		}
		
		
		/**
		 * <summary>
		 * Function that detect the enemies in the turret range and by it's type.
		 * </summary>
		 */
		private IEnumerator Detection() 
		{ 
			yield return new WaitForSeconds(.1f);
		
			_enemiesInRange = Physics.OverlapSphere(transform.position, towerFireLevelStats[CurrentLevel].range, 1<<7);
		
			Transform currentEnemy = null;
			float maxDistance = 0f;
			float maxEnemyLife = 0f;
		
			foreach (Collider enemy in _enemiesInRange)
			{
				if (enemy && enemy.GetComponent<NavMeshAgent>()) 
				{ 
					_currentDistance = enemy.GetComponent<Enemy>().GetPathRemainingDistance();

					switch (fireType)
					{
						case FireType.First:
							if (_currentDistance < _shortestDistance || _shortestDistance <= 0f)
							{
								_shortestDistance = _currentDistance;
								currentEnemy = enemy.transform;
							}
							break;
						case FireType.Last:
							if (_currentDistance > maxDistance)
							{
								maxDistance = _currentDistance;
								currentEnemy = enemy.transform;
							}
							break;
						case FireType.Strongest:
							if (enemy.GetComponent<Enemy>().Life > maxEnemyLife 
							    || (_currentDistance < _shortestDistance && Mathf.Approximately(enemy.GetComponent<Enemy>().Life, maxEnemyLife)))
							{
								_shortestDistance = _currentDistance;
								maxEnemyLife = enemy.GetComponent<Enemy>().Life;
								currentEnemy = enemy.transform;
							}
							break;
						case FireType.Weakest:
							if ((enemy.GetComponent<Enemy>().Life < maxEnemyLife || maxEnemyLife <= 0f) 
							    || (_currentDistance < _shortestDistance && Mathf.Approximately(enemy.GetComponent<Enemy>().Life, maxEnemyLife)))
							{
								_shortestDistance = _currentDistance;
								maxEnemyLife = enemy.GetComponent<Enemy>().Life;
								currentEnemy = enemy.transform;
							}
							break;
					}
				}
				yield return new WaitForEndOfFrame();
			}

			_targetEnemy = currentEnemy;
			_currentDistance = 0f;
			_shortestDistance = 0f;

			StartCoroutine(Detection());
		}


		/**
		 * <summary>
		 * Function to change the Target.
		 * </summary>
		 */
		public void ChangeFireType()
		{
			_currentEnum++;

			if (_currentEnum >= Enum.GetValues(typeof(FireType)).Length) _currentEnum = 0;

			fireType = (FireType)_currentEnum;
		}

		#endregion

		#region Turret Boost

		/**
		 * <summary>
		 * Function to buff the turret.
		 * </summary>
		 * <param name="damageMultiplier">The multiplier given for the damage.</param>
		 * <param name="firerateMultiplier">The multiplier given for the fire-rate.</param>
		 * <param name="isFireChange">The controller of the fire bullet.</param>
		 */
		public void TowerBuff(float damageMultiplier, float firerateMultiplier, bool isFireChange)
		{
			if (!_isBuffed)
			{
				Debug.Log("Get Buffed");
				_originalDamage = towerFireLevelStats[CurrentLevel].damages;
				_originalFirerate = towerFireLevelStats[CurrentLevel].firerate;
				_originalIsFire = towerFireLevelStats[CurrentLevel].isFire;

				towerFireLevelStats[CurrentLevel].damages *= damageMultiplier;
				towerFireLevelStats[CurrentLevel].firerate *= firerateMultiplier;
				towerFireLevelStats[CurrentLevel].isFire = isFireChange;

				_isBuffed = true;
			}
		}


		/**
		 * <summary>
		 * Function to reset the stats of the towers.
		 * </summary>
		 */
		public void TowerUnBuff()
		{
			towerFireLevelStats[CurrentLevel].damages = _originalDamage;
			towerFireLevelStats[CurrentLevel].firerate = _originalFirerate;
			towerFireLevelStats[CurrentLevel].isFire = _originalIsFire;

			_isBuffed = false;
		}

		#endregion
	
	}
}