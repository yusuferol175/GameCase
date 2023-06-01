using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTriggerController : MonoBehaviour
{
    private int _percentLevel;
    private int _brickCount;
    private int _totalBrickCount;
    private bool _isCounted;

    
    private void OnTriggerEnter(Collider other)
    {
        var contactPoint = other.transform.position;
        var rippleEffect = Instantiate(EffectManager.Instance.Ripple,contactPoint,Quaternion.identity);
        var splashEffect = Instantiate(EffectManager.Instance.Splash,contactPoint,Quaternion.identity);
        Destroy(rippleEffect,1.5f);
        Destroy(splashEffect,1.5f);
        Destroy(other.gameObject,0.2f);
        if (other.CompareTag("Brick"))
        {
            _brickCount++;
            
            if (!_isCounted)
            {
                _totalBrickCount = CountBricks();
                _isCounted = true;
            }
            
            UpdatePercentLevel();
        }
    }
    
    private void UpdatePercentLevel()
    {
        if (_totalBrickCount > 0)
        {
            _percentLevel = (int)((float)_brickCount / _totalBrickCount * 100f);
            UIManager.Instance.PercentText.SetText("%" + _percentLevel.ToString());
            UIManager.Instance.UpdateBar(_percentLevel);
        }
    }

    private int CountBricks()
    {
        GameObject[] bricks = GameObject.FindGameObjectsWithTag("Brick");
        return bricks.Length;
    }
}
