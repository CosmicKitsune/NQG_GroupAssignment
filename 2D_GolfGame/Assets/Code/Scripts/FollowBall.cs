using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBall : MonoBehaviour
{
    [SerializeField] private Transform playerPosition; //tracks player position so the animation sprite follows

    [SerializeField] private Vector3 offset; //offet how far the underneath sprite lags behind the player
    [SerializeField] private Animator roll_anim; //tracks player position so the animation sprite follows

    private bool playAnimation = true;
    private Vector3 currentPosition;
    private void FixedUpdate()
    {
        Vector3 movingPosition = playerPosition.position + offset;
        transform.position = movingPosition;
        if (playAnimation)
        { 
            //Debug.Log($"Tracking player position at {movingPosition} with {playAnimation}");  
            currentPosition = playerPosition.position;
            roll_anim.SetTrigger("Rolling");
        }

        playAnimation = false;
    }

    public void SetPlayAnimation()
    {
        playAnimation = true;
        //Debug.Log("Play Animation On"); 
    }

    public void ResetSpritePosition(Vector3 pos)
    {
        transform.position = pos;
    }
}
