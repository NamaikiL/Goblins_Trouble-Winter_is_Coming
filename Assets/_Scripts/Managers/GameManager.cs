using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

	#region Variables

	[SerializeField] private int startMoney = 500;
	[SerializeField] private int startLife = 20;

	private int _currentMoney;
	private int _currentLife;

	private UIManager _uiManager;
	
	public static GameManager instance;
	
    #endregion

    #region Properties

    public int CurrentMoney => _currentMoney;
    public int CurrentLife => _currentLife;
    
    #endregion

    #region Builtin Methods

    void Awake()
    {
	    if (instance)
		    Destroy(gameObject);
	    else
		    instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _uiManager = UIManager.instance;

        _currentMoney = startMoney;
        _currentLife = startLife;
        
        _uiManager.UpdateMoneyText(startMoney);
        _uiManager.UpdateLifeText(startLife);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	#endregion

	#region Custom Methods

	void AddMoney(int quantity)
	{
		_currentMoney += quantity;
		_uiManager.UpdateMoneyText(_currentMoney);
	}
	
	public void RemoveMoney(int quantity)
	{
		_currentMoney -= quantity;
		_uiManager.UpdateMoneyText(_currentMoney);
	}
	
	void AddLife(int quantity)
	{
		_currentLife += quantity;
		_uiManager.UpdateLifeText(_currentLife);
	}
	
	public void RemoveLife(int quantity)
	{
		_currentLife -= quantity;
		_uiManager.UpdateLifeText(_currentLife);
	}

	#endregion

}
