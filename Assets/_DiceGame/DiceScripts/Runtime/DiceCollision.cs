using MoreMountains.NiceVibrations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiceGame
{
    public class DiceCollision : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                MMVibrationManager.Haptic(HapticTypes.LightImpact);
                AudioManager._instance.PlaySound("Dice");
            }
        }
    }
}