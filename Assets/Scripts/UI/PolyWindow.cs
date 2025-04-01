using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PolyWindow : MonoBehaviour
{
    [SerializeField] private Transform content;
    [SerializeField] private List<GameObject> polynomes;
    [SerializeField] private GameObject polyPrefab;
    
    [SerializeField] private TextMeshProUGUI resultText;
    
    [SerializeField] private Color baseColor;
    [SerializeField] private Color outlineColor;
    private void Awake()
    {
        GetComponent<PolynomeEntry>().OnPolynomesReset += PolynomeEntry_OnReset;
        GetComponent<PolynomeEntry>().OnPolynomeEntered += PolynomeEntry_OnPolynomeEntered;
        GetComponent<PolynomeEntry>().OnPolynomeResult += PolynomeEntry_OnResult;
    }

    private void PolynomeEntry_OnResult(object _sender, string _e)
    {
        resultText.text = _e;
    }

    private void PolynomeEntry_OnPolynomeEntered(object _sender, string _e)
    {
        GameObject _temp = Instantiate(polyPrefab, content);
        _temp.GetComponent<Image>().color = baseColor;
        _temp.GetComponent<Outline>().effectColor = outlineColor;
        _temp.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = _e;
        polynomes.Add(_temp);
        polynomes[polynomes.Count - 1].transform.GetChild(0).GetComponent<TMP_Text>().text = "Polynôme n°" + (polynomes.Count);
    }

    private void PolynomeEntry_OnReset(object _sender, EventArgs _e)
    {
        for (int i = 0; i < polynomes.Count; i++)
        {
            Destroy(polynomes[i]);
        }

        polynomes = new();
    }
}
