using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]

public class TowerLevel
{
	public int level;
	public int cost;
	[AssetsOnly] public GameObject body;
	[AssetsOnly] public GameObject head;
}


public class TowerFeatures : MonoBehaviour
{

	#region Variables

	[Header("Tower Features")]
	[SerializeField] private float cooldown;

	[SerializeField] private Transform model;
	[SerializeField] private List<TowerLevel> levels;

	public GameObject turretBase;
	public GameObject turretHead;

	private int _currentLevel = 0;

	private GameManager _gameManager;
	
    #endregion

    #region Properties
    #endregion

    #region Builtin Methods

    // Start is called before the first frame update.
    public virtual void Start()
    {
	    _gameManager = GameManager.instance;
        Pose();
    }

    // Update is called once per frame.
    public virtual void Update()
    {
	    if (Input.GetKeyDown(KeyCode.Space))
	    {
		    Upgrade();
	    }
    }
	
	#endregion

	#region Custom Methods

	private void Pose()
	{
		InstantiateVisual();
		_gameManager.RemoveMoney(levels[_currentLevel].cost);
	}


	private void Upgrade()
	{
		_currentLevel++;
		InstantiateVisual();
		_gameManager.RemoveMoney(levels[_currentLevel].cost);
	}


	private void Sell()
	{
		
	}


	private void InstantiateVisual()
	{
		if(_currentLevel < levels.Count)
		{
			foreach (Transform child in model.transform)
			{
				Destroy(child.gameObject);
			}
		
			if (levels[_currentLevel].body)
				turretBase = Instantiate(levels[_currentLevel].body, model.position, Quaternion.identity, model);
			if (levels[_currentLevel].head)
				turretHead = Instantiate(levels[_currentLevel].head, model.position, Quaternion.identity, model);
		}
	}

	#endregion

}
