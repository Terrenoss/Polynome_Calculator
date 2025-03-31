using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Polynome_Divider : MonoBehaviour
{
    public string Divide(List<LinkedList> polynomes)
    {
        if (polynomes == null || polynomes.Count < 2)
        {
            Debug.Log("Il faut au moins deux polynômes pour effectuer une division.");
            return "0";
        }

        // Dividende (premier polynôme) et diviseur (second polynôme)
        LinkedList dividend = polynomes[0];
        LinkedList divisor = polynomes[1];

        // Vérification division par zéro
        if (IsZeroPolynomial(divisor))
        {
            Debug.LogError("Division par zéro impossible (diviseur nul).");
            return "0";
        }

        // Initialisation du résultat
        StringBuilder result = new StringBuilder();
        LinkedList quotient = new LinkedList();
        LinkedList remainder = CopyPolynomial(dividend);

        // Degré du diviseur
        int divisorDegree = GetDegree(divisor);

        while (!IsZeroPolynomial(remainder) && GetDegree(remainder) >= divisorDegree)
        {
            // Termes dominants
            Node leadingRemainder = GetLeadingTerm(remainder);
            Node leadingDivisor = GetLeadingTerm(divisor);

            // Calcul du terme du quotient
            float coeff = leadingRemainder.value / leadingDivisor.value;
            int power = int.Parse(leadingRemainder.power) - int.Parse(leadingDivisor.power);

            // Ajout au quotient (sans utiliser AddNode)
            Node newTerm = new Node(coeff, power.ToString(), leadingDivisor.unity);
            AddNodeDirectly(quotient, newTerm);

            // Construction du monôme temporaire
            LinkedList temp = new LinkedList();
            AddNodeDirectly(temp, new Node(coeff, power.ToString(), leadingDivisor.unity));

            // Multiplication et soustraction
            LinkedList product = MultiplyPolynomials(temp, divisor);
            remainder = SubtractPolynomials(remainder, product);
        }

        // Construction du résultat sous forme de string
        result.Append(FormatPolynomial(quotient));

        // Si le reste n'est pas nul, l'ajouter au résultat
        if (!IsZeroPolynomial(remainder))
        {
            result.Append(" + (Reste: ");
            result.Append(FormatPolynomial(remainder));
            result.Append(")");
        }

        return result.Length == 0 ? "0" : result.ToString();
    }

    // Helper methods
    private void AddNodeDirectly(LinkedList list, Node newNode)
    {
        if (list.firstNode == null)
        {
            list.firstNode = newNode;
        }
        else
        {
            Node current = list.firstNode;
            while (current.next != null)
            {
                current = current.next;
            }
            current.next = newNode;
        }
    }

    private bool IsZeroPolynomial(LinkedList poly)
    {
        if (poly == null || poly.firstNode == null) return true;
        
        Node current = poly.firstNode;
        while (current != null)
        {
            if (current.value != 0f) return false;
            current = current.next;
        }
        return true;
    }

    private int GetDegree(LinkedList poly)
    {
        if (IsZeroPolynomial(poly)) return -1;
        
        int maxDegree = 0;
        Node current = poly.firstNode;
        while (current != null)
        {
            int power = int.Parse(current.power);
            if (power > maxDegree) maxDegree = power;
            current = current.next;
        }
        return maxDegree;
    }

    private Node GetLeadingTerm(LinkedList poly)
    {
        Node leadingTerm = null;
        int maxDegree = -1;
        
        Node current = poly.firstNode;
        while (current != null)
        {
            int power = int.Parse(current.power);
            if (power > maxDegree)
            {
                maxDegree = power;
                leadingTerm = current;
            }
            current = current.next;
        }
        
        return leadingTerm;
    }

    private LinkedList CopyPolynomial(LinkedList original)
    {
        LinkedList copy = new LinkedList();
        Node current = original.firstNode;
        while (current != null)
        {
            AddNodeDirectly(copy, new Node(current.value, current.power, current.unity));
            current = current.next;
        }
        return copy;
    }

    private LinkedList MultiplyPolynomials(LinkedList a, LinkedList b)
    {
        LinkedList result = new LinkedList();
        
        Node nodeA = a.firstNode;
        while (nodeA != null)
        {
            Node nodeB = b.firstNode;
            while (nodeB != null)
            {
                float coeff = nodeA.value * nodeB.value;
                int power = int.Parse(nodeA.power) + int.Parse(nodeB.power);
                string unity = nodeA.unity;
                
                // Chercher si un terme avec cette puissance existe déjà
                Node existingNode = result.firstNode;
                bool found = false;
                while (existingNode != null)
                {
                    if (existingNode.unity == unity && existingNode.power == power.ToString())
                    {
                        existingNode.value += coeff;
                        found = true;
                        break;
                    }
                    existingNode = existingNode.next;
                }
                
                if (!found)
                {
                    AddNodeDirectly(result, new Node(coeff, power.ToString(), unity));
                }
                
                nodeB = nodeB.next;
            }
            nodeA = nodeA.next;
        }
        
        return result;
    }

    private LinkedList SubtractPolynomials(LinkedList a, LinkedList b)
    {
        LinkedList result = CopyPolynomial(a);
        
        Node nodeB = b.firstNode;
        while (nodeB != null)
        {
            bool termFound = false;
            Node nodeA = result.firstNode;
            
            while (nodeA != null)
            {
                if (nodeA.unity == nodeB.unity && nodeA.power == nodeB.power)
                {
                    nodeA.value -= nodeB.value;
                    termFound = true;
                    break;
                }
                nodeA = nodeA.next;
            }
            
            if (!termFound)
            {
                AddNodeDirectly(result, new Node(-nodeB.value, nodeB.power, nodeB.unity));
            }
            
            nodeB = nodeB.next;
        }
        
        return result;
    }

    private string FormatPolynomial(LinkedList poly)
    {
        if (poly == null || poly.firstNode == null) return "0";
        
        StringBuilder sb = new StringBuilder();
        Node current = poly.firstNode;
        bool firstTerm = true;
        
        while (current != null)
        {
            if (current.value != 0)
            {
                if (!firstTerm)
                {
                    sb.Append(current.value > 0 ? "+" : "");
                }
                
                // Coefficient
                if (current.value != 1f && current.value != -1f)
                {
                    sb.Append(current.value.ToString());
                }
                else if (current.value == -1f)
                {
                    sb.Append("-");
                }
                
                // Unité et puissance
                sb.Append(current.unity);
                if (current.power != "1")
                {
                    sb.Append("^");
                    sb.Append(current.power);
                }
                
                firstTerm = false;
            }
            current = current.next;
        }
        
        return sb.Length == 0 ? "0" : sb.ToString();
    }
}