using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DangerMeter : MonoBehaviour
{
    public Slider dangerMeter; // Reference to the UI Slider
    public float maxDanger = 100f; // Maximum value of the danger meter
    public float dangerFillRate = 10f; // Base rate at which the meter fills
    public float dangerDecayRate = 5f; // Rate at which the meter decays
    public LayerMask aiLayer; // Layer for AI detection
    public float detectionRadius = 5f; // Radius to detect nearby AIs

    private float currentDanger = 0f;
    private bool isGameOver = false;

    void Update()
    {
        if (isGameOver) return; // Stop updating if the game is over

        // Detect all AIs within the detection radius
        Collider[] nearbyAIs = Physics.OverlapSphere(transform.position, detectionRadius, aiLayer);

        if (nearbyAIs.Length > 0)
        {
            // Calculate the total danger contribution from all nearby AIs
            float dangerIncrease = nearbyAIs.Length * dangerFillRate * Time.deltaTime;

            // Increase the danger meter value
            currentDanger += dangerIncrease;
        }
        else
        {
            // Decay the danger meter value
            currentDanger -= dangerDecayRate * Time.deltaTime;
        }

        // Clamp the danger meter value between 0 and maxDanger
        currentDanger = Mathf.Clamp(currentDanger, 0, maxDanger);

        // Update the UI Slider
        if (dangerMeter != null)
        {
            dangerMeter.value = currentDanger / maxDanger;
        }

        // Check for game-over condition
        if (currentDanger >= maxDanger)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        isGameOver = true;
        SceneManager.LoadScene("GameOverScene");
    }

    void OnDrawGizmosSelected()
    {
        // Visualize the detection radius in the Scene view
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
