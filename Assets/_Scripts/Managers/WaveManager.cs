using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Gameplay.Enemies;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Scripts.Managers
{
	public enum BehaviorTypes
	{
		Wait,
		Single,
		Multiple,
		MultipleDifferent
	}

	[System.Serializable]
	public class Behavior
	{
		// Behavior Type.
		public BehaviorTypes type;

		// Odin Variables.
		private bool ShowMultAndDiff => type == BehaviorTypes.Multiple || type == BehaviorTypes.MultipleDifferent;
		private bool HideWaitAndDiff => type == BehaviorTypes.Wait || type == BehaviorTypes.MultipleDifferent;
	
		// Single and Multiple type.
		[HideIf(nameof(HideWaitAndDiff))] public string enemyType;

		// Multiple and Multiple different type.
		[ShowIf("type", BehaviorTypes.MultipleDifferent)] public List<string> enemiesType;
		[ShowIf(nameof(ShowMultAndDiff))] public int enemyNumber;
		[ShowIf(nameof(ShowMultAndDiff))] public float timeBetweenEnemies;
	
		// Wait type.
		[ShowIf("type", BehaviorTypes.Wait)]public float time;
	}

	[System.Serializable]
	public class EnemyType
	{
		#region Variables

		// Enemy info Variables.
		[SerializeField] private string name;
		[AssetsOnly] public GameObject enemyPrefab;

		#endregion

		#region Properties

		public string EnemyName
		{
			get => name;
		}
		
		#endregion
	}

	[System.Serializable]
	public class Wave
	{
		public List<Behavior> behaviors = new List<Behavior>();
	}

	public class WaveManager : MonoBehaviour
	{
		#region Variables
	
		[Header("Enemy Route Positions")]
		[SerializeField] private Transform enemyStart;
		[SerializeField] private Transform enemyEnd;

		[Header("Enemy handler")]
		[SerializeField] private List<EnemyType> enemiesList = new List<EnemyType>();

		[Header("Waves Parameters")]
		[SerializeField] private List<Wave> waves = new List<Wave>();
		[SerializeField] private float timeBetweenWaves;
		[SerializeField] private int moneyPerWave;

		// Waves Variables.
		private int _enemyCount;
	
		// Managers Variables.
		private UIManager _uiManager;
		private GameManager _gameManager;
	
		// Instance Variables.
		private static WaveManager _instance;
	
		#endregion

		#region Properties

		public List<EnemyType> EnemiesList => enemiesList;
    
		public static WaveManager Instance => _instance;

		#endregion

		#region Builtin Methods

		/**
	     * <summary>
	     * Awake is called when an enabled script instance is being loaded.
	     * </summary>
	     */
		void Awake()
		{
			if(_instance) Destroy(this);
			_instance = this;
		}


		/**
	     * <summary>
	     * Start is called before the first frame update.
	     * </summary>
	     */
		void Start()
		{
			Time.timeScale = 1f;
			_uiManager = UIManager.Instance;
			_gameManager = GameManager.Instance;
	    
			StartWaves();
		}
	
		#endregion

		#region CustomMethods

		/**
		 * <summary>
		 * Function to stat the waves.
		 * </summary>
		 */
		private void StartWaves()
		{
			StartCoroutine(StartBehaviorSchema());
		}

	
		/**
		 * <summary>
		 * Coroutine that handle the wave system.
		 * </summary>
		 */
		IEnumerator StartBehaviorSchema()
		{
			// For each waves
			for (int i = 0; i < waves.Count; i++)
			{
				_uiManager.WaveCounter(i, waves.Count - 1);	// Update the wave counter on the UI.
			
				foreach (Behavior behavior in waves[i].behaviors)
				{
					switch (behavior.type) 
					{ 
						// Wait.
						case BehaviorTypes.Wait: 
							yield return new WaitForSeconds(behavior.time); 
							break;
						// Spawn single enemy.
						case BehaviorTypes.Single: 
							SpawnEnemy(behavior.enemyType, enemyStart.position); 
							break;
						// Spawn multiple enemy of one type.
						case BehaviorTypes.Multiple: 
							for (int j = 0; j < behavior.enemyNumber; j++) 
							{ 
								SpawnEnemy(behavior.enemyType, enemyStart.position); 
								yield return new WaitForSeconds(behavior.timeBetweenEnemies);
							}
							break;
						// Spawn multiple enemy of multiple type.
						case BehaviorTypes.MultipleDifferent:
							for (int j = 0; j < behavior.enemyNumber; j++)
							{
								SpawnEnemy(behavior.enemiesType[Random.Range(0, behavior.enemiesType.Count)], enemyStart.position);
								yield return new WaitForSeconds(behavior.timeBetweenEnemies);
							}
							break;
					}
				}
				
				// Check if there's still enemy(ies) on stage.
				while (GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
				{
					yield return null;	// Return null if that's the case.
				}
				
				// If it isn't the last wave, do things.
				if (waves.Last() != waves[i])
				{
					_uiManager.WaveTimerUI(timeBetweenWaves);
					yield return new WaitForSeconds(timeBetweenWaves + 2f);		// Adding two seconds to let the time for the timer ui to dis-activate.
					moneyPerWave *= (int)1.15;
					_gameManager.AddMoney(moneyPerWave);
				}
			}
		}

	
		/**
		 * <summary>
		 * Function to spawn the enemies.
		 * </summary>
		 * <param name="enemyType">The enemy to spawn.</param>
		 * <param name="posSpawn">The place of spawn.</param>
		 */
		public void SpawnEnemy(string enemyType, Vector3 posSpawn)
		{
			// Stock the Prefab of the enemy to spawn.
			GameObject enemyToInstantiate = 
				enemiesList.Where(obj => obj.EnemyName == enemyType).SingleOrDefault() != null
				? enemiesList.Where(obj => obj.EnemyName == enemyType).SingleOrDefault().enemyPrefab
				: null;
			
			// If there's an enemy.
			if (enemyToInstantiate)
			{
				Debug.Log("Instantiate");
				// Instantiate it.
				GameObject enemy = Instantiate(
					enemyToInstantiate, 
					posSpawn, 
					Quaternion.Euler(0f, -90f, 0f)
				);
			
				enemy.transform.name = "Enemy " + _enemyCount;
				_enemyCount++;
				enemy.GetComponent<Enemy>().Destination = enemyEnd;
			}
		}

		#endregion

	}
}