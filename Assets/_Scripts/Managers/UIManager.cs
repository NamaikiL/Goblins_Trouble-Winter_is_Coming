using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Gameplay.Towers;
using _Scripts.Gameplay.Towers.Types;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Scripts.Managers
{
	public class UIManager : MonoBehaviour
	{

		#region Variables

		[Header("Player Information Manager")]
		[SerializeField] private TMP_Text money;
		[SerializeField] private Image life;
		[SerializeField] private GameObject towerCard;
		[SerializeField] private Animator uiAnimator;
		
		[Header("Tower Card Parameters")]
		[SerializeField] private TMP_Text towerName;
		[SerializeField] private Image towerImage;
		[SerializeField] private List<Sprite> towersVisual;
		[SerializeField] private TMP_Text towerUpgrade;
		[SerializeField] private TMP_Text towerSell;
		[SerializeField] private TMP_Text towerType;
		// Fire Tower
		[SerializeField] private TMP_Text towerDamage;
		[SerializeField] private TMP_Text towerFirerate;
		[SerializeField] private TMP_Text towerRangeFire;
		[SerializeField] private TMP_Text towerBullet;
		// Support Tower.
		[SerializeField] private TMP_Text towerRangeSupport;
	
		[Header("Tower Manager")]
		[SerializeField] private Transform towerHolder;
		
		[Header("Boss Parameters")]
		[SerializeField] private Image imgBarLife;
		[SerializeField] private Image imgBarShield;
		[SerializeField] private GameObject panBossUI;

		[Header("Wave Parameters")] 
		[SerializeField] private GameObject waveTimer;
		[SerializeField] private TMP_Text txtWaveCounter;

		[Header("Main Menu")] 
		[SerializeField] private GameObject menuBase;
		[SerializeField] private GameObject menuCredits;
		
		// Tower Card UI Variables.
		private GameObject _actualTower;
		private GameObject _selectedRange;
		
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
			if (_instance) Destroy(this);
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
			// If there's a Scene Manager.
			if (GameObject.Find("SceneManager"))
				_sceneHandlerManager = GameObject.Find("SceneManager").GetComponent<SceneHandlerManager>();
		}


		/**
		 * <summary>
		 * Update is called once per frame.
		 * </summary>
		 */
		void Update()
		{
			if (SceneManager.GetActiveScene().name == "Menu")
				if (Input.GetKeyDown(KeyCode.Escape))
					ButtonMenu();
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
			// If there's no Blueprint
			if (!GameObject.Find("Blueprint"))
			{
				// Instantiate the blueprint with a specific name.
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
		 * <param name="actualLife">The actual life of the player.</param>
		 * <param name="lifeMax">The max life of the player.</param>
		 */
		public void UpdateLifeUI(int actualLife, int lifeMax)
		{
			life.fillAmount = actualLife / lifeMax;
		}


		/**
		 * <summary>
		 * Function to show the tower stats on the UI.
		 * </summary>
		 * <param name="tower">The actual tower.</param>
		 * <param name="check">If it needs to be showed or not.</param>
		 */
		public void UpdateTowerCard(GameObject tower, bool check)
		{
			// If the player is on a tower or click on a tower.
			if(check)
			{
				// If already active, then do nothing.
				if (!towerCard.activeSelf) towerCard.SetActive(true);

				// Stock the tower on variables.
				_actualTower = tower;
				TowerFeatures towerInformation = tower.GetComponent<TowerFeatures>();
				
				// Visual of the range of the tower.
				if(_selectedRange) _selectedRange.SetActive(false);
				_selectedRange = tower.transform.Find("Range").gameObject;
				_selectedRange.SetActive(true);
				
				// Name of the tower and its image.
				towerName.text = tower.name.ToUpper();
				string imageTowerName = tower.name + "Lvl" + (towerInformation.CurrentLevel + 1);
				towerImage.sprite = towersVisual.Find(sprite => sprite.name == imageTowerName);
				
				// Update the tower upgrade and sell buttons.
				if (towerInformation.CurrentLevel < towerInformation.maxLevel - 1)
					towerUpgrade.text = "UPGRADE\nCOST " +
					                    towerInformation.Levels[towerInformation.CurrentLevel + 1].cost;
				else
					towerUpgrade.text = "CAN'T UPGRADE";	
				
				towerSell.text = "SELL\nFOR " + towerInformation.Levels[towerInformation.CurrentLevel].sellPrice;

				// For Fire Tower type.
				if (tower.GetComponent<TowerFire>())
				{
					// Dis-activate the card for the support.
					towerCard.transform.Find("SupportCard").gameObject.SetActive(false);

					GameObject fireCard = towerCard.transform.Find("FireCard").gameObject;
					fireCard.SetActive(true);

					// Fire Type.
					towerType.text = tower.GetComponent<TowerFire>().TypeFire.ToString().ToUpper();
					
					// Tower Stats.
					towerDamage.text = "DAMAGE           " + tower.GetComponent<TowerFire>().TowerDamage;
					towerFirerate.text = "FIRE-RATE     " + tower.GetComponent<TowerFire>().TowerFirerate;
					towerRangeFire.text = "RANGE               " + tower.GetComponent<TowerFire>().TowerRange;

					towerBullet.text = tower.GetComponent<TowerFire>().TowerBullet ? "BULLET FIRE" : "BULLET NORMAL";
				}

				// For Support Tower type.
				if (tower.GetComponent<TowerGoblinGroove>())
				{
					// Dis-activate the card for the fire tower.
					towerCard.transform.Find("FireCard").gameObject.SetActive(false);

					GameObject supportCard = towerCard.transform.Find("SupportCard").gameObject;
					supportCard.SetActive(true);

					// Tower Stats.
					towerRangeSupport.text = "RANGE " + tower.GetComponent<TowerGoblinGroove>().TowerRange;
				}
			}
			else
			{
				// Remove everything important on the card.
				_actualTower = null;
				if(_selectedRange) _selectedRange.SetActive(false);
				towerCard.SetActive(false);
			}
		}


		/**
		 * <summary>
		 * Function for the upgrade button.
		 * </summary>
		 */
		public void UpgradeTower()
		{
			if (_actualTower.GetComponent<TowerFire>())
				_actualTower.GetComponent<TowerFeatures>().Upgrade();
			if (_actualTower.GetComponent<TowerGoblinGroove>())
				_actualTower.GetComponent<TowerGoblinGroove>().Upgrade();
		}
		
		
		/**
		 * <summary>
		 * Function for the sell button.
		 * </summary>
		 */
		public void SellTower()
		{
			if(_actualTower) _actualTower.GetComponent<TowerFeatures>().Sell();
		}


		/**
		 * <summary>
		 * Function to change the type of the tower.
		 * </summary>
		 */
		public void ChangeType()
		{
			_actualTower.GetComponent<TowerFire>().ChangeFireType();
			towerType.text = _actualTower.GetComponent<TowerFire>().TypeFire.ToString().ToUpper();
		}


		/**
		 * <summary>
		 * Function to pause the menu.
		 * </summary>
		 */
		public void PauseMenu()
		{
			uiAnimator.SetBool($"Pause", true);
			Time.timeScale = 0;
		}


		/**
		 * <summary>
		 * Function to resume the game.
		 * </summary>
		 */
		public void ResumeMenu()
		{
			uiAnimator.SetBool($"Pause", false);
			Time.timeScale = 1;
		}


		/**
		 * <summary>
		 * Function to play the menu scene.
		 * </summary>
		 */
		public void ReturnToMenu()
		{
			_sceneHandlerManager.MenuScene();
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


		/**
		 * <summary>
		 * Function to update the wave counter on the screen.
		 * </summary>
		 * <param name="actualWave">The actual wave the player is on.</param>
		 * <param name="maxWave">The max wave on the game.</param>
		 */
		public void WaveCounter(int actualWave, int maxWave)
		{
			txtWaveCounter.text = (actualWave + 1) + " / " + (maxWave + 1);
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
		 * Function that go to the credits menu.
		 * </summary>
		 */
		public void Credits()
		{
			ChangeMenuPage(menuBase, menuCredits);
		}


		/**
		 * <summary>
		 * Function to change menu page by button.
		 * </summary>
		 */
		private void ButtonMenu()
		{
			if (menuCredits.activeSelf) ChangeMenuPage(menuCredits, menuBase);
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
