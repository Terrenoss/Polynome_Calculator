using System.Collections.Generic;
using UnityEngine;

public class Polynome_Add : MonoBehaviour
{

    private bool _is = false;
    
    public string Add(List<LinkedList> _polynomes)
    {
        
        string result = "";
        
        while (_polynomes.Count > 0)
        {
            string _tempunity = "";
            string _temppower = "";
            List<float> _values = new();
            
            foreach (LinkedList _polynome in _polynomes)
            {
                if (_polynome.firstNode == null)
                {
                    _polynomes.Remove(_polynome);
                    break;
                }
                
                Node _nodetemp = _polynome.firstNode;
                
                if (_is == false)
                {
                    _tempunity = _nodetemp.unity;
                    _temppower = _nodetemp.power;
                
                    _values.Add(_nodetemp.value);
                
                    _polynome.firstNode = _nodetemp.next;
                    
                    _is = true;
                }

                while(_nodetemp != null && _nodetemp.unity == _tempunity && _nodetemp.power == _temppower)
                {
                    _values.Add(_nodetemp.value);
                    _polynome.firstNode = _nodetemp.next;
                }
                
                if(_nodetemp == null)
                    break;
                
                while (_nodetemp.next != null)
                {
                    if (_nodetemp.next.unity == _tempunity && _nodetemp.next.power == _temppower)
                    {
                        _values.Add(_nodetemp.next.value);
                        _nodetemp.next = _nodetemp.next.next;
                    }
                }
            }
            
            float _result = 0;
            
            foreach (float _value in _values)
            {
                _result += _value;
            }
            
            if (result != "" && _result != 0)
            {
                if (_result > 0)
                {
                    result += "+ ";
                }
            }
            else if (_result == 0)
            {
                break;
            }
            
            result += _result + _tempunity + "^" + _temppower + " ";
            
            _is = false;
        }
        return result;
    }
}
