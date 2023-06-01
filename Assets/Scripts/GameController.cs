using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private Transform _manPrefab;
    [SerializeField] private Transform _lookTransform;
    [SerializeField] private Transform _band;
    
    [SerializeField] private Rect _touchArea;

    [SerializeField] private LineRenderer _lineRenderer;

    private Transform _instantiatedMan;
    private Transform _instantiatedNewMan;
    private Camera _mainCam;
    private bool _gameState;
    private int _currentLevel;
    private int _manCount;
    private int _shotCount;
    private bool _shoting;
    private Vector3 _touchUpPos;
    private Transform _firstPos;
    private Transform _endPos;
    
    private void Start()
    {
        _mainCam = Camera.main;
        UIManager.Instance.StartPanel.SetActive(true);
        Time.timeScale = 0f;
        _gameState = false;
        _currentLevel = LevelManager.Instance.GetLevelID() + 1;
        _manCount = _currentLevel;
        _shotCount = _currentLevel;
        InstatiateMan();
        if (_currentLevel > 1)
        {
            InstatiateNewMan();
            var anim = _instantiatedNewMan.GetComponent<Animator>();
            anim.SetBool("Dance",true);
        }
        
        UIManager.Instance.LevelText.SetText("Level "+_currentLevel.ToString());
        UIManager.Instance.ShotInfoText.SetText(_shotCount.ToString()+" Shot Left");
        UIManager.Instance.CurrentLevelText.SetText(_currentLevel.ToString());
        UIManager.Instance.NextLevelText.SetText((_currentLevel+1).ToString());
    }

    private void Update()
    {
        if (Input.touchCount > 0 && !_gameState)
        {
            UIManager.Instance.StartPanel.SetActive(false);
            UIManager.Instance.GamePanel.SetActive(true);
            Time.timeScale = 1f;
            _gameState = true;
        }

        if (!_gameState)
            return;

        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            if (!_touchArea.Contains(touch.position))
                return;
            
            if (touch.phase == TouchPhase.Began && _instantiatedMan == null)
            {
                if (_instantiatedMan)
                {
                    if (_lineRenderer.positionCount < 3)
                        _lineRenderer.positionCount = 3;
                        
                    var newPos = _instantiatedMan.position;
                    _touchUpPos = newPos;
                    newPos.z =  1f;
                    newPos.y = -1f;

                    _lineRenderer.enabled = false;
                }
            }

            if (touch.phase == TouchPhase.Moved && _instantiatedMan)
            {
                _lineRenderer.enabled = true;
                var pos = new Vector3(touch.position.x, touch.position.y,
                    _mainCam.WorldToScreenPoint(_instantiatedMan.position).z);
                var worldPos = _mainCam.ScreenToWorldPoint(pos);

                _instantiatedMan.position = new Vector3(worldPos.x, .25f, worldPos.z);

                var newPos = _instantiatedMan.position;
                var endPos = new Vector3(newPos.x, newPos.y, newPos.z + 10f);
                var midlePos = new Vector3(newPos.x, newPos.y + 2f, newPos.z + 5f);
                _band.position = new Vector3(newPos.x, newPos.y, newPos.z - .35f);
                newPos.z += 1f;
                newPos.y += .6f;
                
                var pullDistance = Vector3.Distance(_instantiatedMan.position,_touchUpPos);
                
                switch (pullDistance)
                {
                    case var n when (n > 0 && n < .5):
                        endPos = new Vector3(-0.06900913f,-0.1804977f,17.733f);
                        midlePos = new Vector3(-0.06900913f,3f,endPos.z/2);
                        break;
                    case var n when (n >= .5 && n < 1):
                        endPos = new Vector3(0.1939303f,-4.031539f,19.48431f);
                        midlePos = new Vector3(0.1939303f,3f,endPos.z/2);
                        break;
                    case var n when (n >= 1 && n < 1.5):
                        endPos = new Vector3(0.2190751f,-4.575698f,9.28432f);
                        midlePos = new Vector3(0.2190751f,3f,endPos.z/2);
                        break;
                    case var n when (n >= 1.5 && n < 2):
                        endPos = new Vector3(0.1217827f,-6.200583f,8.626993f);
                        midlePos = new Vector3(0.1217827f,3f,endPos.z/2);
                        break;
                    case var n when (n >= 2 && n < 3):
                        endPos = new Vector3(0.01128719f,-6.139624f,6.721656f);
                        midlePos = new Vector3(0.01128719f,3f,endPos.z/2);
                        break;
                    case var n when (n >= 3 && n < 4):
                        endPos = new Vector3(0f,-6.139624f,5.115308f);
                        midlePos = new Vector3(0f,3f,endPos.z/2);
                        break;
                    case var n when (n >= 4 && n <= 5):
                        endPos = new Vector3(0f,-6.139624f,5.115308f);
                        midlePos = new Vector3(0f,3f,endPos.z/2);
                        break;
                }
                
                _lineRenderer.SetPosition(0,newPos);
                _lineRenderer.SetPosition(1,midlePos);
                _lineRenderer.SetPosition(2,endPos);
            }
            else if (touch.phase == TouchPhase.Ended && _instantiatedMan != null)
            {
                _lineRenderer.enabled = false;
                _shotCount--;
                UIManager.Instance.ShotInfoText.SetText(_shotCount.ToString()+" Shot Left");
                if (_shotCount == 0)
                    UIManager.Instance.ShotInfoText.SetText("No More Shot");
                
                var anim = _instantiatedMan.GetComponent<Animator>();
                anim.SetBool("Fly",true);
                var newPos = _instantiatedMan.position;
                newPos.z = 0.5f;
                newPos.y = -1f;
                
                _lineRenderer.SetPosition(0,newPos);
                
                var manRB = _instantiatedMan.GetComponent<Rigidbody>();
                manRB.isKinematic = false;
                
                var pullDistance = Vector3.Distance(_instantiatedMan.position,_touchUpPos);
                var launchForce = 0f;
                switch (pullDistance)
                {
                    case var n when (n > 0 && n < .5):
                        launchForce = 1500f;
                        break;
                    case var n when (n >= .5 && n < 1):
                        launchForce = 900f;
                        break;
                    case var n when (n >= 1 && n < 1.5):
                        launchForce = 400f;
                        break;
                    case var n when (n >= 1.5 && n < 2):
                        launchForce = 300f;
                        break;
                    case var n when (n >= 2 && n < 3):
                        launchForce = 200f;
                        break;
                    case var n when (n >= 3 && n < 4):
                        launchForce = 100f;
                        break;
                    case var n when (n >= 4 && n <= 5):
                        launchForce = 70f;
                        break;
                }

                manRB.AddForce( _lookTransform.forward * launchForce);
                
                _instantiatedMan = null;
                _band.position = new Vector3(0f, -1f, 2.8f);
                if (_instantiatedNewMan != null)
                {
                    StartCoroutine(NewMan());
                }
                
                StartCoroutine(CheckLevelState());
            }
        }

        if (_firstPos && _endPos)
        {
            _lineRenderer.SetPosition(0,_firstPos.position);
            _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, _endPos.position);
        }
    }

    IEnumerator CheckLevelState()
    {
        if (_shotCount != 0)
            _shoting = true;
        var time = 0f;
        while (time < 10f)
        {
            if (_shoting)
            {
                _shoting = false;
                yield break;
            }
            
            time += Time.deltaTime;
            yield return null;
        }
        
        if (_shotCount == 0)
        {
            UIManager.Instance.GamePanel.SetActive(false);
            UIManager.Instance.LevelCompPanel.SetActive(true);
            var percentLevel = UIManager.Instance.PercentText.GetComponent<TMP_Text>().text;
            UIManager.Instance.ComplitedlevelText.SetText("LEVEL "+_currentLevel +"\n<color=orange>Complited</color>\n"+percentLevel);
        }
    }
    IEnumerator NewMan()
    {
        var newManAnim = _instantiatedNewMan.GetComponent<Animator>();
        newManAnim.applyRootMotion = true;
        newManAnim.SetBool("Dance",false);
        newManAnim.SetBool("Climp",true);
        _instantiatedNewMan = null;
        yield return new WaitForSeconds(3f);
        if (!_instantiatedNewMan)
        {
            if(_manCount > 0 )
                InstatiateNewMan();
        }
            
        Destroy(newManAnim.transform.gameObject);
        InstatiateMan();

    }
    private void InstatiateNewMan()
    {
        _instantiatedNewMan = Instantiate(_manPrefab, new Vector3(transform.position.x +2f,transform.position.y-1.1f,transform.position.z+.7f) , new Quaternion(0,-60,0,60));
        var newManAnim = _instantiatedNewMan.GetComponent<Animator>();
        newManAnim.SetBool("Dance",true);
        if(_manCount > 0)
            _manCount--;
    }
    
    private void InstatiateMan()
    {
        if(_manCount > 0)
            _manCount--;
        _instantiatedMan = Instantiate(_manPrefab, new Vector3(_band.position.x, _band.position.y - .75f, _band.position.z + .35f), Quaternion.identity);
    }
}