using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Polynome_deriver : MonoBehaviour
{
    public string Derive(List<LinkedList> _polynomes)
    {
        if (_polynomes == null || _polynomes.Count == 0) return "";

        StringBuilder result = new StringBuilder();

        foreach (LinkedList polynome in _polynomes)
        {
            Node currentNode = polynome.firstNode;
            
            while (currentNode != null)
            {
                // Parse power (exposant)
                if (!float.TryParse(currentNode.power, out float power))
                {
                    Debug.LogError($"Invalid power format: {currentNode.power}");
                    continue;
                }

                // Skip constants (terms with power 0)
                if (power == 0)
                {
                    currentNode = currentNode.next;
                    continue;
                }

                // Calculate new coefficient and power
                float newCoefficient = currentNode.value * power;
                float newPower = power - 1;

                // Format the derived term
                if (result.Length > 0 && newCoefficient > 0)
                {
                    result.Append(" + ");
                }
                else if (newCoefficient < 0)
                {
                    result.Append(" - ");
                    newCoefficient = -newCoefficient; // We'll handle the sign in the format
                }

                // Handle different cases for display
                if (newPower == 0)
                {
                    result.Append($"{newCoefficient}");
                }
                else if (newPower == 1)
                {
                    result.Append($"{newCoefficient}{currentNode.unity}");
                }
                else
                {
                    result.Append($"{newCoefficient}{currentNode.unity}^{newPower}");
                }

                currentNode = currentNode.next;
            }
        }

        return result.ToString().Trim();
    }
}