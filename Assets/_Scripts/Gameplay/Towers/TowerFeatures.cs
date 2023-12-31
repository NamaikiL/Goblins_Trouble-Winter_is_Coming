using System.Collections;
using System.Collections.Generic;
using _Scripts.Gameplay.Towers.Spawn;
using _Scripts.Managers;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]

public class TowerLevel
{
	
	// Public Variables.
	public int level;
	public int cost;
	public int sellPrice;
	[AssetsOnly] public GameObject body;
	[AssetsOnly] public GameObject head;
	
}

public abstract class TowerFeatures : MonoBehaviour
{

	#region Variables

	[Header("Tower Features")]
	[SerializeField] private Transform model;
	[SerializeField] private List<TowerLevel> levels;

	// Level Variables.
	private int _currentLevel = 0; 
	public int maxLevel;

	// Towers Parts.
	private GameObject _turretBase;
	protected GameObject TurretHead;
	
	// Stun Variables.
	private bool _isStun;

	// Spawner Information
	private Transform _spawner;
	
	// Managers.
	private GameManager _gameManager;
	private UIManager _uiManager;
	
    #endregion

    #region Properties

    public int CurrentLevel => _currentLevel;
    public bool IsStun => _isStun;

    public Transform SpawnerInformation
    {
	    get => _spawner;
	    set => _spawner = value;
    }

    public List<TowerLevel> Levels => levels;
    
    #endregion

    #region Builtin Methods

    /**
     * <summary>
     * Start is called before the first frame update.
     * </summary>
     */
    protected virtual void Start()
    {
	    _gameManager = GameManager.Instance;
	    _uiManager = UIManager.Instance;
	    maxLevel = levels.Count;
	    
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
	public virtual void Upgrade()
	{
		if(_currentLevel < maxLevel - 1)
		{
			if(_gameManager.CurrentMoney >= levels[_currentLevel+1].cost)
			{
				_currentLevel++;
				InstantiateVisual();
				_gameManager.RemoveMoney(levels[_currentLevel].cost);
			}
		}
		_uiManager.UpdateTowerCard(gameObject, true);
	}


	/**
	 * <summary>
	 * Function to sell the tower.
	 * </summary>
	 */
	public void Sell()
	{
		_uiManager.UpdateTowerCard(null, false);
		_spawner.gameObject.SetActive(true);
		_spawner.GetComponent<TowerSpawner>().FillIt();
		_gameManager.AddMoney(levels[_currentLevel].sellPrice);
		Destroy(gameObject);
	}


	/**
	 * <summary>
	 * Function that instantiate the visual of the tower.
	 * </summary>
	 */
	private void InstantiateVisual()
	{
		if(_currentLevel < maxLevel)
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
