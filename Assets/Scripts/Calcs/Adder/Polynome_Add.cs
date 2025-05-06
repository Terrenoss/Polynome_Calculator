using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Polynome_Add : MonoBehaviour
{
    private bool _is = false;

    public string Add(List<LinkedList> _polynomes)
    {
        if (_polynomes == null || _polynomes.Count == 0) return "";

        StringBuilder result = new StringBuilder();
        
        while (_polynomes.Count > 0)
        {
            string _tempunity = "";
            string _temppower = "";
            List<float> _values = new();

            // On parcourt à l'envers pour éviter les suppressions dans foreach
            for (int i = _polynomes.Count - 1; i >= 0; i--)
            {
                LinkedList _polynome = _polynomes[i];

                // Si la liste est vide, on la retire proprement
                if (_polynome.firstNode == null)
                {
                    _polynomes.RemoveAt(i);
                    continue;
                }

                Node _nodetemp = _polynome.firstNode;
                Debug.Log(_nodetemp.value);

                // Initialisation des premières unités et puissances
                if (!_is)
                {
                    _tempunity = _nodetemp.unity;
                    _temppower = _nodetemp.power;

                    _is = true;
                }
                
                Node _node = _nodetemp;

                // Parcourt et addition des monômes similaires
                while (_nodetemp != null)
                {
                    if (_nodetemp.unity == _tempunity && _nodetemp.power == _temppower)
                    {
                        _values.Add(_nodetemp.value);
                        if(_nodetemp == _polynome.firstNode)
                            _polynome.firstNode = _nodetemp.next;
                        else
                            _node.next = _nodetemp.next;
                    }
                    _node = _nodetemp;
                    _nodetemp = _nodetemp.next; // On avance le nœud !
                }
            }

            // Somme des coefficients
            float _result = 0;
            foreach (float _value in _values)
            {
                Debug.Log(_value);
                _result += _value;
            }

            // Gestion du format du résultat
            if (result.Length > 0 && _result != 0)
            {
                result.Append(_result > 0 ? "+ " : "");
            }
            else if (_result == 0)
            {
                _is = false; // reset pour passer au monôme suivant
                continue;
            }
            
            if(_tempunity != "")
                result.Append($"{_result}{_tempunity}^{_temppower} ");
            else
                result.Append($"{_result}{_tempunity}{_temppower} ");

            _is = false;
        }
        Debug.Log(result.ToString().Trim());
        return result.ToString().Trim();
    }
}
