using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Polynome_Sub : MonoBehaviour
{
    private bool _is = false;

    public string Sub(List<LinkedList> _polynomes)
    {
        if (_polynomes == null || _polynomes.Count == 0) return "";

        StringBuilder result = new StringBuilder();

        while (_polynomes.Count > 0)
        {
            string _tempunity = "";
            string _temppower = "";
            List<float> _values = new();

            for (int i = _polynomes.Count - 1; i >= 0; i--)
            {
                LinkedList _polynome = _polynomes[i];

                if (_polynome.firstNode == null)
                {
                    _polynomes.RemoveAt(i);
                    continue;
                }

                Node _nodetemp = _polynome.firstNode;

                if (!_is)
                {
                    _tempunity = _nodetemp.unity;
                    _temppower = _nodetemp.power;
                    _is = true;
                }

                while (_nodetemp != null)
                {
                    if (_nodetemp.unity == _tempunity && _nodetemp.power == _temppower)
                    {
                        if (_values.Count == 0)
                        {
                            _values.Add(_nodetemp.value); // Première valeur
                        }
                        else
                        {
                            _values[0] -= _nodetemp.value; // Soustraction des valeurs suivantes
                        }
                        _polynome.firstNode = _nodetemp.next;
                    }
                    _nodetemp = _nodetemp.next;
                }
            }

            if (_values.Count == 0) continue;

            float _result = -_values[0];

            if (result.Length > 0 && _result != 0)
            {
                result.Append(_result > 0 ? "+ " : "");
            }
            else if (_result == 0)
            {
                _is = false;
                continue;
            }

            result.Append($"{_result}{_tempunity}^{_temppower} ");

            _is = false;
        }

        return result.ToString().Trim();
    }
}
