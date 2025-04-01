using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Polynome_Divider : MonoBehaviour
{
    public string Divide(List<LinkedList> polynomes)
    {
        if (polynomes == null || polynomes.Count < 2)
        {
            Debug.Log("At least two polynomials needed for division");
            return "0";
        }

        LinkedList dividend = polynomes[0];
        LinkedList divisor = polynomes[1];

        if (IsZeroPolynomial(divisor))
        {
            Debug.LogError("Cannot divide by zero polynomial");
            return "0";
        }

        // Convert to term lists
        List<Term> dividendTerms = PolynomialToTermList(dividend);
        List<Term> divisorTerms = PolynomialToTermList(divisor);

        // Factor out common x^n terms
        int dividendMinPower = GetMinPower(dividendTerms);
        int divisorMinPower = GetMinPower(divisorTerms);
        int commonPower = Mathf.Min(dividendMinPower, divisorMinPower);

        // Factor out x^commonPower from both polynomials
        List<Term> simplifiedDividend = FactorOutPower(dividendTerms, commonPower);
        List<Term> simplifiedDivisor = FactorOutPower(divisorTerms, commonPower);

        // Convert back to string representation
        string numerator = TermListToString(simplifiedDividend);
        string denominator = TermListToString(simplifiedDivisor);

        // Handle special cases
        if (denominator == "1")
            return numerator;
        
        return $"({numerator})/({denominator})";
    }

    private int GetMinPower(List<Term> terms)
    {
        int minPower = int.MaxValue;
        foreach (Term term in terms)
        {
            if (term.power < minPower && Mathf.Abs(term.coefficient) > 0.0001f)
                minPower = term.power;
        }
        return minPower == int.MaxValue ? 0 : minPower;
    }

    private List<Term> FactorOutPower(List<Term> terms, int power)
    {
        List<Term> result = new List<Term>();
        foreach (Term term in terms)
        {
            result.Add(new Term(
                term.coefficient,
                term.power - power,
                term.variable
            ));
        }
        return result;
    }

    private class Term
    {
        public float coefficient;
        public int power;
        public string variable;

        public Term(float coeff, int pow, string var)
        {
            coefficient = coeff;
            power = pow;
            variable = var;
        }
    }

    private List<Term> PolynomialToTermList(LinkedList poly)
    {
        List<Term> terms = new List<Term>();
        Node current = poly.firstNode;
        while (current != null)
        {
            terms.Add(new Term(
                current.value,
                int.Parse(current.power),
                current.unity
            ));
            current = current.next;
        }
        return terms;
    }

    private string TermListToString(List<Term> terms)
    {
        if (terms.Count == 0) return "0";

        // Sort by descending power
        terms.Sort((a, b) => b.power.CompareTo(a.power));

        StringBuilder sb = new StringBuilder();
        bool firstTerm = true;
        string variable = terms.Count > 0 ? terms[0].variable : "x";

        foreach (Term term in terms)
        {
            if (Mathf.Abs(term.coefficient) < 0.0001f) continue;

            if (!firstTerm)
            {
                sb.Append(term.coefficient > 0 ? " + " : " - ");
            }
            else if (term.coefficient < 0)
            {
                sb.Append("-");
            }

            float absCoeff = Mathf.Abs(term.coefficient);
            if (absCoeff != 1f || term.power == 0)
            {
                sb.Append(absCoeff.ToString("0.###"));
            }

            if (term.power > 0)
            {
                sb.Append(variable);
                if (term.power > 1)
                {
                    sb.Append("^");
                    sb.Append(term.power);
                }
            }
            else if (term.power < 0)
            {
                sb.Append(variable);
                sb.Append("^");
                sb.Append(term.power);
            }

            firstTerm = false;
        }

        return sb.Length == 0 ? "1" : sb.ToString();
    }

    private bool IsZeroPolynomial(LinkedList poly)
    {
        Node current = poly.firstNode;
        while (current != null)
        {
            if (Mathf.Abs(current.value) > 0.0001f)
                return false;
            current = current.next;
        }
        return true;
    }
}