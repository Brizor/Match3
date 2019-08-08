using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovablePiece : MonoBehaviour
{
    private BasePiece _piece;
    private IEnumerator _moveCuroutine;

    void Awake()
    {
        _piece = GetComponent<BasePiece>();
    }

    public void move(int newX, int newY, float time = 0.0f)
    {
        if (_moveCuroutine != null)
        {
            StopCoroutine(_moveCuroutine);
        }

        if (time == 0.0f)
        {
            transform.position = _piece.gridRef.getGridPosition(newX, newY);
        }
        else
        {
            _moveCuroutine = moveCoroutine(newX, newY, time);
            StartCoroutine(_moveCuroutine);
        }
    }

    private IEnumerator moveCoroutine(int newX, int newY, float time)
    {
        _piece.x = newX;
        _piece.y = newY;

        Vector3 startPosition = transform.position;
        Vector3 endPosition = _piece.gridRef.getGridPosition(newX, newY);

        for (float i = 0.0f; i <= 1 * time; i += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, i / time);
            yield return 0;
        }

        transform.position = endPosition;
    }
}
