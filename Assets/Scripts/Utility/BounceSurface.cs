using UnityEngine;
using UnityEngine.UIElements;

public class BounceSurface : MonoBehaviour
{
    public float bounceStrength;

    private void OnCollisionEnter2D(Collision2D collision) {
        Ball ball = collision.gameObject.GetComponent<Ball>();

        if(ball != null ) {
            Vector2 normal = collision.GetContact(0).normal;
            ball.AddForce(-normal * this.bounceStrength);

            //spin the ball upon colliding with the top or bottom walls
            Rigidbody2D ballRigidBody2D = ball.transform.GetComponent<Rigidbody2D>();
            float ballAngularVelocity = ballRigidBody2D.angularVelocity;
            float wallPositionY = this.transform.position.y;
            float ballPositionY = ball.transform.position.x;
            float ballVelocityX = ballRigidBody2D.velocity.x;
            float ballVelocityY = ballRigidBody2D.velocity.y;
            float ballVelocityMagnitude = Mathf.Abs(ballVelocityX) + Mathf.Abs(ballVelocityY);

            float torqueAmount = Mathf.Clamp(ballVelocityMagnitude, -20, 20);

            // spin the ballvv
            if ( (normal.y > 0 && ballVelocityX > 0) || (normal.y < 0 && ballVelocityX < 0) ) {
                ballRigidBody2D.AddTorque(torqueAmount, ForceMode2D.Force);
            }
            else {
                ballRigidBody2D.AddTorque(-torqueAmount, ForceMode2D.Force);
            }
        }
    }
}
