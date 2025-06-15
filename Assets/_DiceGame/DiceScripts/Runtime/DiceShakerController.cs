using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DiceShakerController : MonoBehaviour
{
  
     [Header("Dice Settings")]
    public Rigidbody[] dice;
    public float shakeThreshold = 1.5f;
    public float baseRollForce = 6f;
    public float maxRollForce = 12f;

    private Vector3 lastAccel;
    private float idleTime = 0f;
    private float shakeCooldown = 0f;

    void Start()
    {
        Physics.gravity = Vector3.down * 9.81f; // Lock gravity regardless of phone orientation
        lastAccel = Input.acceleration;
    }

    void Update()
    {
        Vector3 currentAccel = Input.acceleration;
        float shake = (currentAccel - lastAccel).sqrMagnitude;
        lastAccel = currentAccel;

        // Cooldown between shake bursts
        shakeCooldown -= Time.deltaTime;

        if (shake > shakeThreshold && shakeCooldown <= 0f)
        {
            float force = Mathf.Clamp(shake * baseRollForce, baseRollForce, maxRollForce);
            ApplyForceToDice(force);
            shakeCooldown = 0.1f; // tiny delay to avoid over-spam
        }

        CheckIfDiceStopped();
        
#if UNITY_EDITOR
        SimulateShakeWithKey();
#endif
    }

    void ApplyForceToDice(float force)
    {
        foreach (Rigidbody rb in dice)
        {
            rb.isKinematic = false;

            Vector3 dir = new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(1.5f, 2.5f),
                Random.Range(-1f, 1f)
            );
            rb.AddForce(dir * force, ForceMode.Impulse);
            rb.AddTorque(Random.insideUnitSphere * force, ForceMode.Impulse);
        }

        idleTime = 0f;
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
                break;
            }
        }

        if (allStopped)
        {
            idleTime += Time.deltaTime;
            if (idleTime > 1f)
            {
                ShowDiceResults();
            }
        }
    }

    void ShowDiceResults()
    {
        string result = "Rolled: ";
        for (int i = 0; i < dice.Length; i++)
        {
            int face = GetTopFace(dice[i].transform);
            result += $"Dice {i + 1} = {face}  ";
        }

        Debug.Log(result);
    }

    int GetTopFace(Transform dice)
    {
        Vector3[] directions = {
            dice.up,          // +Y → 6
            -dice.up,         // -Y → 1
            dice.right,       // +X → 3
            -dice.right,      // -X → 4
            dice.forward,     // +Z → 2
            -dice.forward     // -Z → 5
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
    void SimulateShakeWithKey()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            float simulatedShake = Random.Range(1f, 1f);
            ApplyForceToDice(simulatedShake);
        }
    }
    



}
