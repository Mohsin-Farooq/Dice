using System.Collections;
using UnityEngine;

namespace DiceGame
{
    public class TextVisualEffect : MonoBehaviour
    {
        public IEnumerator ScaleUp(GameObject Text, float duration, Vector3 targetScale)
        {
            float elapsed = 0f;
            Vector3 startScale = Vector3.zero;

            while (elapsed < duration)
            {
                Text.transform.localScale = Vector3.Lerp(startScale, targetScale, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }
            Text.transform.localScale = targetScale;
        }


        public IEnumerator ScaleDown(GameObject Text, float duration)
        {
            float elapsed = 0f;
            Vector3 startScale = transform.localScale;
            Vector3 targetScale = Vector3.zero;

            while (elapsed < duration)
            {
                Text.transform.localScale = Vector3.Lerp(startScale, targetScale, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }
            Text.transform.localScale = targetScale;
        }
    }
}