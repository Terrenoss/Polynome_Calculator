using System.Collections;
using UnityEditor;
using UnityEngine;

public class UiAnimations : MonoBehaviour
{
    public float lerpTime;
    
    public void FadeIn(CanvasGroup _group)
    {
        StartCoroutine(FadeInAnim(_group));
    }
    
    public void FadeOut(CanvasGroup _group)
    {
        StartCoroutine(FadeOutAnim(_group));
    }
    
    private IEnumerator FadeInAnim(CanvasGroup _group)
    {
        _group.gameObject.SetActive(true);
        float _elapsedTime = 0;

        while (_elapsedTime < lerpTime)
        {
            _group.alpha = Mathf.Lerp(0, 1, _elapsedTime / lerpTime);
            _elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        _group.alpha = 1;
    }
    
    private IEnumerator FadeOutAnim(CanvasGroup _group)
    {
        float _elapsedTime = 0;

        while (_elapsedTime < lerpTime)
        {
            _group.alpha = Mathf.Lerp(1, 0, _elapsedTime / lerpTime);
            _elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        _group.alpha = 0;
        _group.gameObject.SetActive(false);
    }

    public void MakeError(GameObject _gameObject)
    {
        StartCoroutine(Error(_gameObject));
    }

    private IEnumerator Error(GameObject _gameObject)
    {
        float _elapsedTime = 0;

        while (_elapsedTime < 2)
        {
            _elapsedTime += Time.deltaTime;
            _gameObject.SetActive(true);
            yield return null;
        }
        
        _gameObject.SetActive(false);
    }
}
