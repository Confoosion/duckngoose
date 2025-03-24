using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    [SerializeField] Vector2 decisionRange = new Vector2(3.5f, 8f);
    private float decisionTimer = 0f;

    [SerializeField] float speed = 2.5f;

    int horizontal = 0;
    private bool isFacingRight = true;
    [SerializeField] private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        decisionTimer = Random.Range(decisionRange.x, decisionRange.y);
    }

    // Update is called once per frame
    void Update()
    {
        if(decisionTimer > 0f)
        {
            decisionTimer -= Time.deltaTime;
        }
        else
        {
            Decision(Random.Range(-1, 2));
            decisionTimer = Random.Range(decisionRange.x, decisionRange.y);
        }
    }

    void Decision(int decision)
    {
        horizontal = decision;
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        Flip();
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}
