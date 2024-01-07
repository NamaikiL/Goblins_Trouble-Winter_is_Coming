using UnityEngine;
using UnityEngine.EventSystems;

namespace _Scripts.Managers
{
    public class BoardManager : MonoBehaviour
    {

        #region Variables

        [Header("Audio")]
        [SerializeField] private AudioSource button2;
        
        // Tower selection Variables.
        private GameObject _selectedTower;
    
        // Managers Variables.
        private UIManager _uiManager;
    
        #endregion

        #region Builtin Methods
    
        /**
         * <summary>
         * Start is called before the first frame update.
         * </summary>
         */
        void Start()
        {
            _uiManager = UIManager.Instance;
        }

    
        /**
         * <summary>
         * Update is called once per frame.
         * </summary>
         */
        void Update()
        {
            CheckTower();
        }
    
        #endregion

        #region Custom Methods

        /**
         * <summary>
         * Function that check when a player click on a specific tower and show its stats.
         * </summary>
         */
        private void CheckTower()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Show the card when clicking a tower.
            if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit, 100f, 1 << 8))
            {
                button2.Play();
                _selectedTower = hit.collider.gameObject;   // Stock the tower.
            
                _uiManager.UpdateTowerCard(_selectedTower, true);
            }
        
            // If the player click on the UI, do nothing.
            else if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject()) return;

            // If the player click on nothing, dis-activate the card.
            else if (!Physics.Raycast(ray, out hit, 100f, 1 << 8) 
                && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)))
            {
                _uiManager.UpdateTowerCard(null, false);
            }
        }
    
        #endregion

    }
}
