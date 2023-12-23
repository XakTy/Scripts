using System.Collections;
using LeopotamGroup.Globals;
using UnityEngine;

namespace Zlodey
{
    class Boot : MonoBehaviour
    {
        public StaticData StaticData;
        IEnumerator Start()
        {
            StaticData.AutoAttack = true;

			Service<StaticData>.Set(StaticData);
           
            GameInitialization.FullInit();

            Service<UI>.Get().CloseAll();

            yield return null;
            Levels.LoadCurrent();
        }
    }
}