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
                // Vérification de la puissance
                if (string.IsNullOrWhiteSpace(currentNode.power) || 
                    !float.TryParse(currentNode.power.Trim(), out float power))
                {
                    Debug.LogError($"Puissance invalide : '{currentNode.power}'");
                    currentNode = currentNode.next;
                    continue;
                }

                // Skip constants
                if (power == 0)
                {
                    currentNode = currentNode.next;
                    continue;
                }

                float newCoefficient = currentNode.value * power;
                float newPower = power - 1;

                // Ajouter un + si nécessaire
                if (result.Length > 0 && newCoefficient > 0)
                {
                    result.Append(" + ");
                }
                else if (newCoefficient < 0)
                {
                    result.Append(" - ");
                    newCoefficient = -newCoefficient; // gestion du signe
                }

                // Sécuriser l'unité (par ex. "x")
                string unity = string.IsNullOrWhiteSpace(currentNode.unity) ? "x" : currentNode.unity;

                // Affichage selon le degré
                if (newPower == 0)
                {
                    result.Append($"{newCoefficient}");
                }
                else if (newPower == 1)
                {
                    result.Append($"{newCoefficient}{unity}");
                }
                else
                {
                    result.Append($"{newCoefficient}{unity}^{newPower}");
                }

                currentNode = currentNode.next;
            }
        }

        return result.ToString().Trim();
    }

}