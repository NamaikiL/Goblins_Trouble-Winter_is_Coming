using System;
using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Managers
{
	public class UIManager : MonoBehaviour
	{

		#region Variables

		[Header("Player Information Manager")]
		[SerializeField] private TMP_Text money;
		[SerializeField] private TMP_Text life;
	
		[Header("Tower Manager")]
		[SerializeField] private Transform towerHolder;
		
		[Header("Boss Parameters")]
		[SerializeField] private Image imgBarLife;
		[SerializeField] private Image imgBarShield;
		[SerializeField] private GameObject panBossUI;

		[Header("Wave Parameters")] 
		[SerializeField] private GameObject waveTimer;

		[Header("Main Menu")] 
		[SerializeField] private GameObject menuBase;
		[SerializeField] private GameObject menuOptions;
		[SerializeField] private GameObject menuCredits;
		
		// Managers Variables.
		private SceneHandlerManager _sceneHandlerManager;
		
		// Instance Variables.
		private static UIManager _instance;
	
		#endregion

		#region Properties

		public static UIManager Instance => _instance;
    
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
		 * Start is called on the frame when a script is enabled just before any of the Update methods are called
		 * the first time.
		 * </summary>
		 */
		void Start()
		{
			if (GameObject.Find("SceneManager"))
				_sceneHandlerManager = GameObject.Find("SceneManager").GetComponent<SceneHandlerManager>();
		}
	
		#endregion

		#region Gameplay Player

		/**
		 * <summary>
		 * Function that spawn a tower blueprint based on it's type.
		 * </summary>
		 * <param name="tower">The blueprint of the tower.</param>
		 */
		public void TowerInstantiate(GameObject tower)
		{
			if (!GameObject.Find("Blueprint"))
			{
				GameObject blueprint = Instantiate(tower, Vector3.zero, Quaternion.identity, towerHolder);
				blueprint.name = "Blueprint";
			}
		}

	
		/**
		 * <summary>
		 * Function that update the money on the UI.
		 * </summary>
		 * <param name="quantity">The quantity that will be updated.</param>
		 */
		public void UpdateMoneyText(int quantity)
		{
			money.text = quantity.ToString();
		}

		
		/**
		 * <summary>
		 * Function that update the life on the UI.
		 * </summary>
		 * <param name="quantity">The quantity that will be updated.</param>
		 */
		public void UpdateLifeText(int quantity)
		{
			life.text = quantity.ToString();
		}

		#endregion

		#region Gameplay Boss

		/**
		 * <summary>
		 * Function to activate the boss UI.
		 * </summary>
		 * <param name="check">Boolean to activate the UI or not.</param>
		 */
		public void UpdateBossUI(bool check)
		{
			panBossUI.SetActive(check);
		}
		

		/**
		 * <summary>
		 * Function that update the shield of the boss on the UI.
		 * </summary>
		 * <param name="shieldValue">The actual shield value of the boss.</param>
		 * <param name="shieldValueMax">The max shield value of the boss.</param>
		 */
		public void UpdateBossShield(float shieldValue, float shieldValueMax)
		{
			imgBarShield.fillAmount = shieldValue / shieldValueMax;
		}


		/**
		 * <summary>
		 * Function that update the life of the boss on the UI.
		 * </summary>
		 * <param name="lifeValue">The actual life value of the boss.</param>
		 * <param name="lifeValueMax">The max life value of the boss.</param>
		 */
		public void UpdateBossLife(float lifeValue, float lifeValueMax)
		{
			imgBarLife.fillAmount = lifeValue / lifeValueMax;
		}

		#endregion

		#region Gameplay Wave

		/**
		 * <summary>
		 * Function for the timer between each wave.
		 * </summary>
		 * <param name="time">The time between each waves.</param>
		 */
		public void WaveTimerUI(float time)
		{
			waveTimer.SetActive(true);
			StartCoroutine(WaveTimerUpdateUI(time));
		}


		/**
		 * <summary>
		 * Coroutine for the timer between each wave.
		 * </summary>
		 * <param name="time">The time between each waves.</param>
		 */
		private IEnumerator WaveTimerUpdateUI(float time)
		{
			for (float i = time; i >= 0; i--)
			{
				TimeSpan timer = TimeSpan.FromSeconds(i);
				waveTimer.GetComponent<TMP_Text>().text = timer.ToString("mm':'ss");
				yield return new WaitForSeconds(1f);
			}
			waveTimer.SetActive(false);
		}

		#endregion

		#region Main Menu

		/**
		 * <summary>
		 * Function that launch a game.
		 * </summary>
		 */
		public void Play()
		{
			_sceneHandlerManager.PlayScene();
		}

		
		/**
		 * <summary>
		 * Function that go to the options menu.
		 * </summary>
		 */
		public void Options()
		{
			ChangeMenuPage(menuBase, menuOptions);
		}

		
		/**
		 * <summary>
		 * Function that go to the credits menu.
		 * </summary>
		 */
		public void Credits()
		{
			ChangeMenuPage(menuBase, menuCredits);
		}

		
		/**
		 * <summary>
		 * Function that quit the game.
		 * </summary>
		 */
		public void Quit()
		{
			#if UNITY_EDITOR	// For the editor mode.
			EditorApplication.ExitPlaymode();
			#endif
			
			Application.Quit();
		}


		/**
		 * <summary>
		 * Function to change the visible panel on the menu.
		 * </summary>
		 * <param name="panel1">The first panel.</param>
		 * <param name="panel2">The second panel.</param>
		 */
		private void ChangeMenuPage(GameObject panel1, GameObject panel2)
		{
			if (panel1.activeSelf)
			{
				panel1.SetActive(false);
				panel2.SetActive(true);
			}
			else
			{
				panel1.SetActive(true);
				panel2.SetActive(false);
			}
		}

		#endregion

	}
}
