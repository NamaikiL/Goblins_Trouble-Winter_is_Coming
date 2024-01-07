using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.Managers
{
	public class GameManager : MonoBehaviour
	{

		#region Variables

		[Header("Player Gameplay Parameters")]
		[SerializeField] private int startMoney = 500;
		[SerializeField] private int startLife = 20;

		// Player stats Variables.
		private int _currentMoney;
		private int _currentLife;

		// EndScene Variable.
		private static bool _hasWin;
		
		// Managers Variables.
		private UIManager _uiManager;
	
		// Instance Variables.
		private static GameManager _instance;
	
		#endregion

		#region Properties

		public int CurrentMoney => _currentMoney;
		public bool HasWin
		{
			get => _hasWin;
			set => _hasWin = value;
		}

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
			if (_instance) Destroy(this);
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

			if(SceneManager.GetActiveScene().name == "Game")
			{
				_currentMoney = startMoney;
				_currentLife = startLife;

				_uiManager.UpdateMoneyText(startMoney);
				_uiManager.UpdateLifeUI(_currentLife, startLife);
			}
		}
	
		#endregion

		#region Custom Methods

		/**
		 * <summary>
		 * Function to add money for the player.
		 * </summary>
		 * <param name="quantity">The quantity to add.</param>
		 */
		public void AddMoney(int quantity)
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
			_currentMoney = Mathf.Clamp(_currentMoney - quantity, 0, 99999);
			_uiManager.UpdateMoneyText(_currentMoney);
		}

		/**
		 * <summary>
		 * Function to remove life to the player.
		 * </summary>
		 * <param name="quantity">The actual life to remove.</param>
		 */
		public void RemovePlayerLife(int quantity)
		{
			_currentLife = Mathf.Clamp(_currentLife - quantity, 0, startLife);
			if (_currentLife > 0)
				UpdateLifePlayer(_currentLife);
			else
				_uiManager.ChargeEndScene();
		}
		
		
		/**
		 * <summary>
		 * Function to update the life of the player.
		 * </summary>
		 * <param name="actualLife">The actual life of the player.</param>
		 */
		private void UpdateLifePlayer(int actualLife)
		{
			_uiManager.UpdateLifeUI(actualLife, startLife);
		}

		#endregion

	}
}
