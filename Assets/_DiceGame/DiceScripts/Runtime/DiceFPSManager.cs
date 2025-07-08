using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiceGame
{
    public class DiceFPSManager : MonoBehaviour
    {
#if UNITY_ANDROID

        private void Awake()
        {
            Application.targetFrameRate = 60;
        }

#endif
    }
}