using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Hands : MonoBehaviour
{
    [SerializeField] private Transform _lookAtTransform;
    private void FixedUpdate()
    {
        if (GameState.Instance.CurrentState != GameState.State.InGame)
            return;

        if (Input.GetMouseButton(0))
        { 
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 dirToTarget = _lookAtTransform.position - transform.position;

            // Получаем угол между вектором к цели и вектором up
            float angle = Mathf.Atan2(dirToTarget.y, dirToTarget.x) * Mathf.Rad2Deg;

            // Поворачиваем объект вокруг оси Z
            transform.rotation = Quaternion.Euler(0f, 0f, angle-90f);
            transform.position = new Vector2(mousePosition.x, mousePosition.y);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GameState.Instance.CurrentState != GameState.State.InGame)
            return;

        if (collision.gameObject.CompareTag("Ball"))
        {
            GameState.Instance.BallCathed?.Invoke();
            collision.gameObject.GetComponent<Ball>().OnBallCathed(transform);
        }
    }
}
