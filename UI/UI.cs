using UnityEngine;
using UnityEngine.EventSystems;
using Zlodey.Actors;

namespace Zlodey
{
    public class UI : MonoBehaviour
    {
        public MenuScreen MenuScreen;
        public GameScreen GameScreen;
        public WinScreen WinScreen;

        public EventSystem EventSystem;
        public DialogScreen DialogScreen;
        public BlackScreen BlackScreen;
        public LoseScreen LoseScreen;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            if (!EventSystem.current && EventSystem.current != EventSystem)
            {
                EventSystem.gameObject.SetActive(false);
            }
            else
            {
                EventSystem.gameObject.SetActive(true);
            }
        }

        public void CloseAll()
        {
            MenuScreen.Show(false);
            GameScreen.Show(false);
            LoseScreen.Show(false);
            WinScreen.Show(false);
            DialogScreen.Show(false);
        }
    }
}