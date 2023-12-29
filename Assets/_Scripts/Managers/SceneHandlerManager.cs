using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.Managers
{
    public class SceneHandlerManager : MonoBehaviour
    {
        #region Scenes Functions

        /**
         * <summary>
         * Function that launch the Game Scene.
         * </summary>
         */
        public void PlayScene()
        {
            SceneManager.LoadScene("Game", LoadSceneMode.Single);
        }


        /**
         * <summary>
         * Function that launch the manu scene.
         * </summary>
         */
        public void MenuScene()
        {
            SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        }

        #endregion
    }
}
