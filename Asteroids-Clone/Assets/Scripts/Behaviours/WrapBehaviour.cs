using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrapBehaviour : MonoBehaviour
{
    [Header("Objects Reference"), SerializeField]
    private Renderer _objectRenderer;

    [SerializeField] private Transform _transform;

    private float _limitX;
    private float _limitY;

    private void Start()
    {
        _limitX = GameManager.Instance.XScreenLimit;
        _limitY = GameManager.Instance.YScreenLimit;
    }

    void Update()
    {
        if (!Application.isEditor)
        {
            if (_objectRenderer.isVisible)
            {
                Debug.Log("Object is visible");
            }
            else CheckMoveToOtherSide();
        }
        else CheckMoveToOtherSide();
    }

    public void CheckMoveToOtherSide()
    {
        if (transform.position.x > _limitX)
            transform.position = new Vector2(-_limitX, this.transform.position.y);
        if (transform.position.x < -_limitX)
            transform.position = new Vector2(_limitX, this.transform.position.y);
        if (transform.position.y > _limitY)
            transform.position = new Vector2(this.transform.position.x, -_limitY);
        if (transform.position.y < -_limitY)
            transform.position = new Vector2(this.transform.position.x, _limitY);
    }
}