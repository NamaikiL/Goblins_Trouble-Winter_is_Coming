using System.Collections;
using _Scripts.Gameplay.Towers;
using _Scripts.Managers;
using UnityEngine;
using UnityEngine.AI;

namespace _Scripts.Gameplay.Enemies
{
	public class Enemy : MonoBehaviour
	{

		#region Variables

		[Header("Enemy Stats Parameters")] 
		[SerializeField] private float life;
		[SerializeField] private float shield;
		[SerializeField] private float moveSpeed;
		[SerializeField] private bool needFire;

		[Header("Krampère Nowel Parameters")]
		[SerializeField] private float minSecondsStun;
		[SerializeField] private float maxSecondsStun;
		[SerializeField] private float minNbEnemies;
		[SerializeField] private float maxNbEnemies;
		[SerializeField] private float minSecondsEnemies;
		[SerializeField] private float maxSecondsEnemies;
		[SerializeField] private float radiusStun;
	
		// Nerf Variables.
		private float _originalMoveSpeed;
		private bool _isNerfed;

		// KrampereNowel Variables.
		private float _originalLife;
		private float _originalShield;
		private bool _secondShield;

		// Movements Variables.
		private Transform _destination;
		private NavMeshAgent _agent;
	
		// Managers.
		private UIManager _uiManager;
		private WaveManager _waveManager;
		private GameManager _gameManager;
    
		#endregion

		#region Properties

		public float Life => life;
		public bool NeedFire => needFire;
    
		public Transform Destination
		{
			set => _destination = value;
		}
    
		#endregion

		#region Builtin Methods

		/**
	     * <summary>
	     * Start is called before the first frame update.
	     * </summary>
	     */
		void Start()
		{
			_uiManager = UIManager.Instance;
			_waveManager = WaveManager.Instance;
			_gameManager = GameManager.Instance;
	    
			_originalMoveSpeed = moveSpeed;
	    
			_agent = GetComponent<NavMeshAgent>();
			_agent.speed = moveSpeed;

			_originalShield = shield;
			_originalLife = life;

			if (gameObject.CompareTag("KrampereNowel"))
			{
				HandleUIKrampereNowel(true);
				StartCoroutine(KrampereNowelStun());
				StartCoroutine(KrampereSpawnEnemies());
			}
		}

    
		/**
	     * <summary>
	     * Update is called once per frame.
	     * </summary>
	     */
		void Update()
		{
			_agent.SetDestination(_destination.position);
		}
	
		#endregion

		#region Enemy Stats

		/**
		 * <summary>
		 * Function that removes life to the enemy and check if its dead or not.
		 * </summary>
		 * <param name="quantity">The quantity of life removed.</param>
		 */
		public void EnemyDamage(float quantity)
		{
			if (gameObject.CompareTag("KrampereNowel"))
			{
				if (shield > 0)
				{
					shield = Mathf.Clamp(shield -= quantity, 0, _originalShield);
					_uiManager.UpdateBossShield(shield, _originalShield);
				}
				else
				{
					if (life < _originalLife / 2 && !_secondShield)
					{
						shield = _originalShield;
						_secondShield = true;
						_uiManager.UpdateBossShield(shield, _originalShield);
					}

					life = Mathf.Clamp(life -= quantity, 0, _originalLife);
					_uiManager.UpdateBossLife(life, _originalLife);

					if (life <= 0)
					{
						_uiManager.UpdateBossUI(false);
						_gameManager.HasWin = true;
						_uiManager.ChargeEndScene();
					}
				}
			}
			else
			{
				life -= quantity;
				if (life < 0) Destroy(gameObject);
			}
		}


		/**
		 * <summary>
		 * Function to buff the Enemy.
		 * </summary>
		 * <param name="speedModifier">The multiplier given for the moveSpeed.</param>
		 */
		public void EnemyNerf(float speedModifier)
		{
			if (!_isNerfed)
			{
				moveSpeed *= speedModifier;
				_agent.speed = moveSpeed;

				_isNerfed = true;
			}
		}


		/**
		 * <summary>
		 * Function to reset the stats of the enemy.
		 * </summary>
		 */
		public void EnemyUnNerf()
		{
			_agent.speed = _originalMoveSpeed;

			_isNerfed = false;
		}

		#endregion
	
		#region Enemy Behavior

		/**
		 * <summary>
		 * Function that get the remaining distance of its path.
		 * </summary>
		 */
		public float GetPathRemainingDistance()
		{
			if (_agent.pathPending ||
			    _agent.pathStatus == NavMeshPathStatus.PathInvalid ||
			    _agent.path.corners.Length == 0)
				return -1f;

			float distance = 0.0f;
			for (int i = 0; i < _agent.path.corners.Length - 1; ++i)
			{
				var path = _agent.path;
				distance += Vector3.Distance(path.corners[i], path.corners[i + 1]);
			}

			return distance;
		}

		#endregion

		#region Krampere Nowel

		/**
		 * <summary>
		 * Function to Activate the UI of the Boss.
		 * </summary>
		 */
		private void HandleUIKrampereNowel(bool check)
		{
			_uiManager.UpdateBossUI(check);

			if (check)
			{
				UpdateUIBoss();
			}
		}
	
	
		/**
		 * <summary>
		 * Coroutine that stun the towers around the Krampère Nowel.
		 * </summary>
		 */
		private IEnumerator KrampereNowelStun()
		{
			float randomRange = Random.Range(minSecondsStun, maxSecondsStun);

			yield return new WaitForSeconds(randomRange);
		
			Collider[] towersInSight = Physics.OverlapSphere(transform.position, radiusStun, 1 << 8);
			foreach (Collider tower in towersInSight)
			{
				StartCoroutine(tower.GetComponent<TowerFeatures>().TowerStun());
			}

			StartCoroutine(KrampereNowelStun());
		}
	
	
		/**
		 * <summary>
		 * Coroutine that spawn enemies from the Krampere Nowel.
		 * </summary>
		 */
		private IEnumerator KrampereSpawnEnemies()
		{
			float randomNbEnemies = Random.Range(minNbEnemies, maxNbEnemies);
			string enemyName = _waveManager.EnemiesList[Random.Range(0, _waveManager.EnemiesList.Count - 1)].EnemyName;
			Transform krampereTransform = transform;
		
			for (int i = 0; i < randomNbEnemies; i++)
			{
				_waveManager.SpawnEnemy(enemyName, krampereTransform.position + krampereTransform.forward * 0.5f);
				yield return new WaitForSeconds(1f);
			}
		
			yield return new WaitForSeconds(Random.Range(minSecondsEnemies, maxSecondsEnemies));
			StartCoroutine(KrampereSpawnEnemies());
		}


		/**
		 * <summary>
		 * Function to Update the boss UI.
		 * </summary>
		 */
		private void UpdateUIBoss()
		{
			_uiManager.UpdateBossLife(life, _originalLife);
			_uiManager.UpdateBossShield(shield, _originalShield);
		}

		#endregion

	}
}
