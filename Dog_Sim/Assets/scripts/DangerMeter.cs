using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DangerMeter : MonoBehaviour
{
    [Header("Meter Settings")]
    public Slider dangerMeter;
    public float fillRate = 1f; // How fast the meter fills per AI
    public float decayRate = 0.5f; // How fast the meter decays without AI nearby
    public float maxDanger = 100f;

    [Header("AI Detection")]
    public float dangerZoneRadius = 5f;
    public LayerMask aiLayer; // Set this to a layer for AI objects

    private float currentDanger = 0f;

    void Update()
    {
        // Find nearby AI
        Collider[] aiInRange = Physics.OverlapSphere(transform.position, dangerZoneRadius, aiLayer);

        if (aiInRange.Length > 0)
        {
            // Increase meter based on the number of nearby AI
            currentDanger += fillRate * aiInRange.Length * Time.deltaTime;
        }
        else
        {
            // Decay the meter if no AI are nearby
            currentDanger -= decayRate * Time.deltaTime;
        }

        // Clamp meter value between 0 and max
        currentDanger = Mathf.Clamp(currentDanger, 0, maxDanger);

        // Update UI
        dangerMeter.value = currentDanger / maxDanger;

        // Lose condition
        if (currentDanger >= maxDanger)
        {
            Debug.Log("You Lose! The danger meter is full.");
        }
    }

    void OnDrawGizmosSelected()
    {
        // Visualize the danger zone radius in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, dangerZoneRadius);
    }
}
