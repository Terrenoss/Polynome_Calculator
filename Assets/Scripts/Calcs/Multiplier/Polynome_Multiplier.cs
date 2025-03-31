using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Polynome_Multiplier : MonoBehaviour
{
    public string Multiply(List<LinkedList> polynomes)
    {
        if (polynomes == null || polynomes.Count == 0) 
        {
            Debug.Log("La liste des polynômes est vide.");
            return "";
        }

        Debug.Log($"Nombre de polynômes reçus : {polynomes.Count}");

        Dictionary<string, float> productTerms = new Dictionary<string, float>();

        // Initialiser le résultat avec le premier polynôme
        LinkedList resultPoly = polynomes[0];
        Debug.Log($"Premier polynôme : {resultPoly}");

        for (int i = 1; i < polynomes.Count; i++)
        {
            Dictionary<string, float> newProductTerms = new Dictionary<string, float>();
            LinkedList currentPoly = polynomes[i];

            Node node1 = resultPoly.firstNode;
            while (node1 != null)
            {
                Node node2 = currentPoly.firstNode;
                while (node2 != null)
                {
                    float newValue = node1.value * node2.value;
                    int newPower = int.Parse(node1.power) + int.Parse(node2.power);
                    string newKey = $"{node1.unity}^{newPower}";

                    if (newProductTerms.ContainsKey(newKey))
                    {
                        newProductTerms[newKey] += newValue;
                    }
                    else
                    {
                        newProductTerms[newKey] = newValue;
                    }

                    node2 = node2.next;
                }
                node1 = node1.next;
            }

            // Fusionner les nouveaux termes avec les anciens
            foreach (var term in newProductTerms)
            {
                if (productTerms.ContainsKey(term.Key))
                    productTerms[term.Key] += term.Value;
                else
                    productTerms[term.Key] = term.Value;
            }
        }

        // Vérification des termes calculés
        Debug.Log($"Nombre de termes après multiplication : {productTerms.Count}");
        foreach (var term in productTerms)
        {
            Debug.Log($"Terme : {term.Key}, Valeur : {term.Value}");
        }

        // Construction du résultat sous forme de chaîne de caractères
        StringBuilder result = new StringBuilder();
        foreach (var term in productTerms)
        {
            if (result.Length > 0)
            {
                result.Append(term.Value > 0 ? " + " : " - ");
            }
            result.Append($"{Mathf.Abs(term.Value)}{term.Key}");
        }

        return result.ToString().Trim();
    }
}
