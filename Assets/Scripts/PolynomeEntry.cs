using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using TMPro;
using UnityEngine;

public class PolynomeEntry : MonoBehaviour
{
    public TMP_InputField inputField;
    private Polynome_Add polynomeAdder;

    public string monome;
    public List<string> monomes = new();

    public LinkedList polynomeList;
    public List<LinkedList> polynomes = new();

    public event EventHandler OnPolynomesReset;
    public event EventHandler<string> OnPolynomeEntered;
    public event EventHandler<string> OnPolynomeResult;

    void OnEnable()
    {
        inputField.contentType = TMP_InputField.ContentType.Custom;
        polynomeAdder = GetComponent<Polynome_Add>();
    }

    public void GetAdd()
    {
        FieldToList();
        OnPolynomeResult?.Invoke(this, polynomeAdder.Add(polynomes));
        Reset();
    }    
    public void GetSub()
    {
        FieldToList();
        OnPolynomeResult?.Invoke(this, GetComponent<Polynome_Sub>().Sub(polynomes));
        Reset();
    }    
    public void GetMultiply()
    {
        FieldToList();
        OnPolynomeResult?.Invoke(this, GetComponent<Polynome_Multiplier>().Multiply(polynomes));
        Reset();
    }    
    public void GetDivide()
    {
        FieldToList();
        OnPolynomeResult?.Invoke(this, GetComponent<Polynome_Divider>().Divide(polynomes));
        Reset();
    }

    public void Reset()
    {
        polynomes = new();
        OnPolynomesReset?.Invoke(this, EventArgs.Empty);
    }

    public void FieldToList()
    {
        if (inputField.text.Trim() == "")
        {
            return;
        }
        
        OnPolynomeEntered?.Invoke(this, inputField.text);
        char[] _input = inputField.text.Replace(" ", "").ToCharArray();

        //--------------Checks----------------

        for (int i = 0; i < _input.Count(); i++)
        {
            Debug.Log(_input[i]);
            if (_input[i] == '(' || _input[i] == ')')
            {
                Debug.Log("Erreur, " + _input[i] + " trouvé");
                return;
            }
            else if (i == _input.Count() - 1 && (_input[i] == '-' || _input[i] == '+'))
            {
                Debug.Log("Erreur, " + _input[i] + " trouvé à la fin du polynôme");
                return;
            }
            else if ((_input[i] == '+' || _input[i] == '-') && (_input[i + 1] == '-' || _input[i + 1] == '+'))
            {
                Debug.Log("Erreur, " + _input[i + 1] + " trouvé après un autre signe");
                return;
            }
            else if (i != _input.Count() - 1 && char.IsLetter(_input[i]) && char.IsDigit(_input[i + 1]))
            {
                Debug.Log("Erreur, " + _input[i + 1] + " trouvé après une inconnue");
                return;
            }
        }

        //------------------------------------

        inputField.text = "";

        polynomeList = new LinkedList();

        for (int i = 0; i < _input.Length; i++)
        {
            if (i == 0)
            {
                CheckMonome(i, _input);
            }
            else
            {
                if (_input[i].Equals('+') || _input[i].Equals('-'))
                {
                    CheckMonome(i, _input);
                }
            }
        }

        polynomes.Add(polynomeList);
        monome = "";
    }

    private void CheckMonome(int _i, char[] _input)
    {
        if (char.IsDigit(_input[_i]))
            monome += "+";

        if (char.IsLetter(_input[_i]))
        {
            monome += "+";
            monome += ".";
        }
        
        monome += _input[_i];
        int _index = _i + 1;
        bool _didTakeUnite = false;
        while (_index < _input.Length && _input[_index] != '+' && _input[_index] != '-')
        {
            if (!_didTakeUnite && char.IsLetter(_input[_index]))
            {
                monome += ".";
                _didTakeUnite = true;
            }

            monome += _input[_index];
            _index++;
        }

        Debug.Log(monome);

        MonomeToNode(monome);
        MonomeToList(monome);
    }

    public void MonomeToList(string _monome)
    {
        monomes.Add(_monome);
        monome = "";
    }

    public void MonomeToNode(string _monome)
    {
        //Getting the value in the monome
        string[] _monomeTab = _monome.Split('.');

        float _monomeValue = 0;

        if (_monomeTab[0] == "+")
        {
            _monomeValue = 1;
        }
        else if (_monomeTab[0] == "-")
        {
            _monomeValue = -1;
        }
        else
        {
            _monomeValue = float.Parse(_monomeTab[0]);
        }


        string _monomeUnit = "";
        string _monomePower = "";

        if (_monomeTab.Count() > 1)
        {
            //Getting the unity and power of the monome
            if (_monomeTab[1].Contains("^"))
            {
                string[] _monomeUnity = _monomeTab[1].Trim().Split('^');
                _monomeUnit = _monomeUnity[0];
                _monomePower = _monomeUnity[1];
            }
            else
            {
                _monomeUnit = _monomeTab[1];
                _monomePower = "1";
            }
        }

        Node _monomeNode = new Node(_monomeValue, _monomePower, _monomeUnit);

        // Debug.Log(_monomeNode.value);
        // Debug.Log(_monomeNode.unity);
        // Debug.Log(_monomeNode.power);
        // Debug.Log(_monomeNode.next);

        if (polynomeList.firstNode == null)
        {
            polynomeList.firstNode = _monomeNode;
        }
        else
        {
            Node _node = polynomeList.firstNode;
            while (_node.next != null)
            {
                _node = _node.next;
            }

            _node.next = _monomeNode;
        }
    }
}