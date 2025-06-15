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
    public GameObject winPopup;

    private Vector3 lastAccel;
    private float idleTime = 0f;
    private float shakeCooldown = 0f;
    private bool hasShownResult = false;
    private bool hasRolledAtLeastOnce = false;

    private int totalWins = 0;
    private int totalRolls = 0;

    void Start()
    {
        Physics.gravity = Vector3.down * 9.81f;
        lastAccel = Input.acceleration;
        UpdateStatsUI();
        if (winPopup) winPopup.SetActive(false);
    }

    void Update()
    {
#if UNITY_EDITOR
        SimulateShakeWithKey();
#else
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
        if (winPopup) winPopup.SetActive(false);
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
            if (idleTime > 1f && !hasShownResult)
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
            Debug.Log($"🎲 Dice: {i+1}, Face: {face}");
        }

        totalRolls++;
        if (total == 7)
        {
            totalWins++;
            if (winPopup) winPopup.SetActive(true);
        }

        UpdateStatsUI();
        Debug.Log($"🎲 Total = {total}, Rolls: {totalRolls}, Wins: {totalWins}");
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

        int[] faceMap = { 6, 1, 3, 4, 2, 5 };
        return faceMap[bestFace];
    }
}
