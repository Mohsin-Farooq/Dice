using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiceShakerController : MonoBehaviour
{
    [Header("Dice Settings")]
    public Rigidbody[] dice;
    public float shakeThreshold = 1.5f;
    public float baseRollForce = 6f;
    public float maxRollForce = 12f;

    [Header("Game Stats UI")]
    public Text rollText;
    public Text winText;
    public Text winPopup;

    private Vector3 lastAccel;
    private float idleTime = 0f;
    private float shakeCooldown = 0f;
    private bool hasShownResult = false;
    private bool hasRolledAtLeastOnce = false;
    private bool isAnimating = false;

    private int totalWins = 0;
    private int totalRolls = 0;

    private TextVisualEffect VFX;

    void Start()
    {
        VFX = GetComponent<TextVisualEffect>();
        Physics.gravity = Vector3.down * 9.81f;
        lastAccel = Input.acceleration;
        UpdateStatsUI();
    }

    void Update()
    {
#if UNITY_EDITOR
        if (!isAnimating)
            SimulateShakeWithKey();
#else
        if(!isAnimating)
       SimulateShakeWithPhone(); 
#endif
        CheckIfDiceStopped();
    }

    void SimulateShakeWithKey()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            float simulatedShake = Random.Range(2f, 5f);
            ApplyForceToDice(simulatedShake);

        }
    }

    void SimulateShakeWithPhone()
    {
        Vector3 currentAccel = Input.acceleration;
        float shake = (currentAccel - lastAccel).sqrMagnitude;
        lastAccel = currentAccel;

        shakeCooldown -= Time.deltaTime;
        if (shake > shakeThreshold && shakeCooldown <= 0f)
        {
            float force = Mathf.Clamp(shake * baseRollForce, baseRollForce, maxRollForce);
            ApplyForceToDice(force);
            shakeCooldown = 0.1f;

        }
    }

    void ApplyForceToDice(float force)
    {
        foreach (Rigidbody rb in dice)
        {
            rb.isKinematic = false;
            Vector3 dir = new Vector3(Random.Range(-1f, 1f), Random.Range(1.5f, 2.5f), Random.Range(-1f, 1f));
            rb.AddForce(dir * force, ForceMode.Impulse);
            rb.AddTorque(Random.insideUnitSphere * force, ForceMode.Impulse);
        }

        idleTime = 0f;
        hasShownResult = false;
        hasRolledAtLeastOnce = true;
    }

    void CheckIfDiceStopped()
    {
        bool allStopped = true;
        foreach (Rigidbody rb in dice)
        {
            if (rb.velocity.magnitude > 0.05f || rb.angularVelocity.magnitude > 0.05f)
            {
                allStopped = false;
                idleTime = 0f;
                hasShownResult = false;
                break;
            }
        }

        if (allStopped && hasRolledAtLeastOnce)
        {
            idleTime += Time.deltaTime;
            if (idleTime > 0.1f && !hasShownResult)
            {
                hasShownResult = true;
                ShowDiceResults();
            }
        }
    }

    void ShowDiceResults()
    {
        int total = 0;
        for (int i = 0; i < dice.Length; i++)
        {
            int face = GetTopFace(dice[i].transform);
            total += face;
           
        }

     
        totalRolls++;
        if (total == 7)
        {
            totalWins++;
            AudioManager._instance.PlaySound("Clap");
        }

        UpdateStatsUI();
       

        if (winPopup)
        {
            winPopup.text = total.ToString();
            StartCoroutine(HandleWinPopupAnimation());
        }
    }

    void UpdateStatsUI()
    {
        if (rollText) rollText.text = $"Rolls: {totalRolls}";
        if (winText) winText.text = $"Wins: {totalWins}";
    }

    int GetTopFace(Transform dice)
    {
        Vector3[] directions = {
            dice.up, -dice.up,
            dice.right, -dice.right,
            dice.forward, -dice.forward
        };

        float maxDot = -1f;
        int bestFace = -1;

        for (int i = 0; i < directions.Length; i++)
        {
            float dot = Vector3.Dot(directions[i], Vector3.up);
            if (dot > maxDot)
            {
                maxDot = dot;
                bestFace = i;
            }
        }

        int[] faceMap = { 1, 6, 5, 2, 4, 3 };
        return faceMap[bestFace];
    }

    IEnumerator HandleWinPopupAnimation()
    {
        winPopup.gameObject.SetActive(true);
       
        isAnimating = true;

        yield return StartCoroutine(VFX.ScaleUp(winPopup.gameObject, 0.2f, Vector3.one));
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(VFX.ScaleDown(winPopup.gameObject, 0.2f));

        winPopup.gameObject.SetActive(false);
        isAnimating = false;
    }


}
