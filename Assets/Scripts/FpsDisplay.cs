using System;
using TMPro;
using UnityEngine;

public class FpsDisplay : MonoBehaviour
{
    private float _pollingTime = 3f;
    private float _time;
    private int _frameCount;

    private void Update()
    {
        _time += Time.deltaTime;
        _frameCount++;

        if (_time >= _pollingTime)
        {
            var frameRate = Mathf.RoundToInt(_frameCount / _time);
            UIManager.Instance.FpsText.SetText(frameRate.ToString());

            _time -= _pollingTime;
            _frameCount = 0;
        }
    }
}