using UnityEngine;

public class Ore : MonoBehaviour
{
    public float value = 1;
    private void OnTriggerEnter(Collider other)
    {
        // add upgrader collision
        if (other.CompareTag("AddUpgrader"))
        {
            AddUpgrader AddUpgrader = other.GetComponent<AddUpgrader>();
            if (AddUpgrader != null)
            {
                value += AddUpgrader.addAmount;
            }
        }
        // multiply upgrader collision
        else if (other.CompareTag("MultiplyUpgrader"))
        {
            MultiplyUpgrader MultiplyUpgrader = other.GetComponent<MultiplyUpgrader>();
            if (MultiplyUpgrader != null)
            {
                value *= MultiplyUpgrader.multiplyAmount;
            }
        }
        // furnace sell collision
        else if (other.CompareTag("Furnace"))
        {
            Furnace Furnace = other.GetComponent<Furnace>();
            if (Furnace != null)
            {
                value *= Furnace.multiplyAmount;
            }

            // Replace the debug with code to update user money
            Debug.Log(gameObject + " sell " + value);
            Destroy(gameObject); // Despawn sphere
        }
    }
}
