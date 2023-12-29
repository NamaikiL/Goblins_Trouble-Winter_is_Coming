using UnityEngine;

namespace _Scripts.Managers
{
	public class GameManager : MonoBehaviour
	{

		#region Variables

		[Header("Player Gameplay Parameters")]
		[SerializeField] private int startMoney = 500;
		[SerializeField] private int startLife = 20;

		// Private Variables.
		private int _currentMoney;
		private int _currentLife;

		private UIManager _uiManager;
	
		private static GameManager _instance;
	
		#endregion

		#region Properties

		public int CurrentMoney => _currentMoney;
		public int CurrentLife => _currentLife;

		public static GameManager Instance => _instance;
    
		#endregion

		#region Builtin Methods

		/**
		 * <summary>
		 * Awake is called when an enabled script instance is being loaded.
		 * </summary>
		 */
		void Awake()
		{
			if (_instance)
				Destroy(gameObject);
			else
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

			_currentMoney = startMoney;
			_currentLife = startLife;
        
			_uiManager.UpdateMoneyText(startMoney);
			_uiManager.UpdateLifeText(startLife);
		}
	
		#endregion

		#region Custom Methods

		/**
		 * <summary>
		 * Function to add money for the player.
		 * </summary>
		 * <param name="quantity">The quantity to add.</param>
		 */
		void AddMoney(int quantity)
		{
			_currentMoney += quantity;
			_uiManager.UpdateMoneyText(_currentMoney);
		}
	
		
		/**
		 * <summary>
		 * Function to remove money for the player.
		 * </summary>
		 * <param name="quantity">The quantity to remove.</param>
		 */
		public void RemoveMoney(int quantity)
		{
			_currentMoney -= quantity;
			_uiManager.UpdateMoneyText(_currentMoney);
		}
	
		
		/**
		 * <summary>
		 * Function to add life for the player.
		 * </summary>
		 * <param name="quantity">The quantity to add.</param>
		 */
		void AddLife(int quantity)
		{
			_currentLife += quantity;
			_uiManager.UpdateLifeText(_currentLife);
		}
	
		
		/**
		 * <summary>
		 * Function to remove life for the player.
		 * </summary>
		 * <param name="quantity">The quantity to remove.</param>
		 */
		public void RemoveLife(int quantity)
		{
			_currentLife -= quantity;
			_uiManager.UpdateLifeText(_currentLife);
		}

		#endregion

	}
}
