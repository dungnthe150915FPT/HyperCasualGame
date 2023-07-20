using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform playerTransform; // Tham chiếu đến transform của PlayerController
    public float initialMovementSpeed = 1f; // Tốc độ di chuyển ban đầu của enemy
    public float fastMovementSpeed = 3f; // Tốc độ di chuyển khi enemy gần PlayerController
    private Rigidbody2D rb; // Tham chiếu đến Rigidbody2D của enemy

    void Start()
    {
        // Tìm PlayerController game object và lấy transform component của nó
        GameObject playerObject = GameObject.Find("PlayerController");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
        else
        {
            Debug.LogError("PlayerController không tìm thấy trong scene!");
        }

        // Lấy tham chiếu đến Rigidbody2D của enemy
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (playerTransform != null)
        {
            // Tính toán khoảng cách giữa enemy và player
            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

            // Xác định tốc độ di chuyển hiện tại của enemy dựa vào khoảng cách
            float currentSpeed = (distanceToPlayer <= 0.1f) ? fastMovementSpeed : initialMovementSpeed;

            // Tính toán hướng di chuyển tới player
            Vector2 direction = (playerTransform.position - transform.position).normalized;

            // Di chuyển enemy với tốc độ movementSpeed bằng Rigidbody2D.AddForce
            rb.AddForce(direction * currentSpeed);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "PlayerController")
        {
            Debug.Log("Sakura đã chết");
            Destroy(collision.gameObject);
        }
    }
}
