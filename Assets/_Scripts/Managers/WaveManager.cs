using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public enum BehaviorTypes
{
	Wait,
	Single,
	Multiple
}

[System.Serializable]
public class Behavior
{
	public BehaviorTypes type;
	[HideIf("type", BehaviorTypes.Wait)]public string enemyType;
	
	[ShowIf("type", BehaviorTypes.Multiple)]public int enemyNumber;
	[ShowIf("type", BehaviorTypes.Multiple)]public float timeBetweenEnemies;
	
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
		get { return name; }
		set { name = value; }
	}
	#endregion
}

public class WaveManager : MonoBehaviour
{
	#region Variables
	
	[SerializeField] private Transform enemyStart;
	[SerializeField] private Transform enemyEnd;

	[SerializeField] private int nbEnemies;
	[SerializeField] private List<EnemyType> enemiesList = new List<EnemyType>();

	[SerializeField] private List<Behavior> behaviors = new List<Behavior>();

	private int _count;
	
    #endregion

    #region Builtin Methods

    // Start is called before the first frame update
    void Start()
    {
        StartWaves();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	#endregion

	#region CustomMethods

	public void StartWaves()
	{
		StartCoroutine(StartBehaviorSchema());
	}

	IEnumerator StartBehaviorSchema()
	{
		int maxWave = 0;
		for (int i = 0; i < behaviors.Count; i++)
		{
			if (behaviors[i].type != BehaviorTypes.Wait)
				maxWave++;
			switch (behaviors[i].type)
			{
				case BehaviorTypes.Wait:
					yield return new WaitForSeconds(behaviors[i].time);
					break;
				case BehaviorTypes.Single:
					SpawnEnemy(behaviors[i].enemyType);
					break;
				case BehaviorTypes.Multiple:
					for (int j = 0; j < behaviors[i].enemyNumber; j++)
					{
						SpawnEnemy(behaviors[i].enemyType);
						yield return new WaitForSeconds(behaviors[i].timeBetweenEnemies);
					}
					break;
			}
		}
	}

	private void SpawnEnemy(string enemyType)
	{
		/*foreach (EnemyType e in enemiesList)
		{
			if (e.EnemyName == enemyType)
			{
				GameObject enemy = Instantiate(e.enemyPrefab, enemyStart.position, Quaternion.Euler(0f, -90f, 0f));
				enemy.GetComponent<Enemy>().Destination = enemyEnd;
			}
		}*/
		
		GameObject enemyToInstantiate = enemiesList.Where(obj => obj.EnemyName == enemyType).SingleOrDefault() != null
			? enemiesList.Where(obj => obj.EnemyName == enemyType).SingleOrDefault().enemyPrefab
			: null;
		
		if (enemyToInstantiate)
		{
			GameObject enemy = Instantiate(enemyToInstantiate, enemyStart.position, Quaternion.Euler(0f, -90f, 0f));
			enemy.transform.name = "Enemy " + _count;
			_count++;
			enemy.GetComponent<Enemy>().Destination = enemyEnd;
		}
	}

	#endregion

}
