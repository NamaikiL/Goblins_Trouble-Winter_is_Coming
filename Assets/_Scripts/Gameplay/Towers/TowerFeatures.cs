using System.Collections;
using System.Collections.Generic;
using _Scripts.Managers;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]

public class TowerLevel
{
	
	// Public Variables.
	public int level;
	public int cost;
	[AssetsOnly] public GameObject body;
	[AssetsOnly] public GameObject head;
	
}

public abstract class TowerFeatures : MonoBehaviour
{

	#region Variables

	[Header("Tower Features")]
	[SerializeField] private Transform model;
	[SerializeField] protected List<TowerLevel> levels;

	// Level Variables.
	private int _currentLevel = 0;
	protected int MaxLevel;

	// Towers Parts.
	private GameObject _turretBase;
	protected GameObject TurretHead;
	
	// Stun Variables.
	private bool _isStun;

	// Managers.
	private GameManager _gameManager;
	
    #endregion

    #region Properties

    protected int CurrentLevel => _currentLevel;
    protected bool IsStun => _isStun;
    
    #endregion

    #region Builtin Methods

    /**
     * <summary>
     * Start is called before the first frame update.
     * </summary>
     */
    protected virtual void Start()
    {
	    MaxLevel = levels.Count;
	    
	    _gameManager = GameManager.Instance;
        Pose();
    }

    
    /**
     * <summary>
     * Update is called once per frame.
     * </summary>
     */
    protected virtual void Update()
    {
	    if (Input.GetKeyDown(KeyCode.Space))
	    {
		    Upgrade();
	    }
    }
	
	#endregion

	#region Custom Methods

	/**
	 * <summary>
	 * Function that instantiate the tower and remove the money from the player.
	 * </summary>
	 */
	private void Pose()
	{
		InstantiateVisual();
		_gameManager.RemoveMoney(levels[_currentLevel].cost);
	}


	/**
	 * <summary>
	 * Function to upgrade the tower.
	 * </summary>
	 */
	protected virtual void Upgrade()
	{
		if(_currentLevel < MaxLevel - 1)
		{
			_currentLevel++;
			InstantiateVisual();
			_gameManager.RemoveMoney(levels[_currentLevel].cost);
		}
	}


	/**
	 * <summary>
	 * Function to sell the tower.
	 * </summary>
	 */
	private void Sell()
	{
		
	}


	/**
	 * <summary>
	 * Function that instantiate the visual of the tower.
	 * </summary>
	 */
	private void InstantiateVisual()
	{
		if(_currentLevel < MaxLevel)
		{
			foreach (Transform child in model.transform)
			{
				Destroy(child.gameObject);
			}
		
			if (levels[_currentLevel].body)
				_turretBase = Instantiate(levels[_currentLevel].body, model.position, Quaternion.identity, model);
			if (levels[_currentLevel].head)
				TurretHead = Instantiate(levels[_currentLevel].head, model.position, Quaternion.identity, model);
			
			InstantiateChildVisual();
		}
	}


	/**
	 * <summary>
	 * Coroutine to stun the towers.
	 * </summary>
	 */
	public IEnumerator TowerStun()
	{
		_isStun = true;
		yield return new WaitForSeconds(5f);
		_isStun = false;
	}


	/**
	 * <summary>
	 * Function that instantiate the visual of a child.
	 * </summary>
	 */
	protected abstract void InstantiateChildVisual();

	#endregion

}
