using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Managers;
using Sirenix.OdinInspector;
using UnityEngine;

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
	public BehaviorTypes type;

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
	[SerializeField] private string name;
	[AssetsOnly] public GameObject enemyPrefab;

	#region Properties

	public string EnemyName
	{
		get => name;
		set => name = value;
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

	// Waves Variables.
	private int _enemeyCount;
	
	// Managers Variables.
	private UIManager _uiManager;
	
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
	    if(_instance) Destroy(gameObject);
	    _instance = this;
    }


    /**
     * <summary>
     * Start is called before the first frame update.
     * </summary>
     */
    void Start()
    {
	    _uiManager = UIManager.Instance;
        StartWaves();
    }
	
	#endregion

	#region CustomMethods

	/**
	 * <summary>
	 * Function to stat the waves.
	 * </summary>
	 */
	public void StartWaves()
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
		for (int i = 0; i <= waves.Count; i++)
		{
			_uiManager.WaveCounter(i, waves.Count);
			
			foreach (Behavior behavior in waves[i].behaviors)
			{
				switch (behavior.type) 
				{ 
					case BehaviorTypes.Wait: 
						yield return new WaitForSeconds(behavior.time); 
						break;
					case BehaviorTypes.Single: 
						SpawnEnemy(behavior.enemyType, enemyStart.position); 
						break;
					case BehaviorTypes.Multiple: 
						for (int j = 0; j < behavior.enemyNumber; j++) 
						{ 
							SpawnEnemy(behavior.enemyType, enemyStart.position); 
							yield return new WaitForSeconds(behavior.timeBetweenEnemies);
						}
						break;
					case BehaviorTypes.MultipleDifferent:
						for (int j = 0; j < behavior.enemyNumber; j++)
						{
							SpawnEnemy(behavior.enemiesType[Random.Range(0, behavior.enemiesType.Count)], enemyStart.position);
							yield return new WaitForSeconds(behavior.timeBetweenEnemies);
						}
						break;
				}
			}
			
			while (GameObject.FindGameObjectsWithTag("Enemy") != null)
			{
				yield return null;
			}

			if (waves.Last() != waves[i])
			{
				_uiManager.WaveTimerUI(timeBetweenWaves);
				yield return new WaitForSeconds(timeBetweenWaves + 2f);		// Adding two seconds to let the time for the timer ui to dis-activate.
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
		GameObject enemyToInstantiate = 
			enemiesList.Where(obj => obj.EnemyName == enemyType).SingleOrDefault() != null
			? enemiesList.Where(obj => obj.EnemyName == enemyType).SingleOrDefault().enemyPrefab
			: null;
		
		if (enemyToInstantiate)
		{
			GameObject enemy = Instantiate(
				enemyToInstantiate, 
				posSpawn, 
				Quaternion.Euler(0f, -90f, 0f)
				);
			
			enemy.transform.name = "Enemy " + _enemeyCount;
			_enemeyCount++;
			enemy.GetComponent<Enemy>().Destination = enemyEnd;
		}
	}

	#endregion

}
