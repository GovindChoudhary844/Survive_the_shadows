using Cinemachine;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Camera cam;
    public Transform followTarget;

    // Starting position for the parallax game object
    Vector2 startingPosition;

    //Start z value of the parallax game object
    float startingZ;

    //Distance that the caemra has moved from the position of the parallax object
    Vector2 camMoveSinceStart => (Vector2)cam.transform.position - startingPosition;

    float zDistanceFromTarget => transform.position.z - followTarget.transform.position.z;

    //If object is in front of target, use near clip plane.If behind target, use farClipPlane
    float clippingPlane => (cam.transform.position.z + (zDistanceFromTarget > 0 ? cam.farClipPlane : cam.nearClipPlane));

    // The further the object from the player, the faster the ParallaxEffect object will move. Drag its Z value closer to the target to make it move slower.
    float parallaxFactor => Mathf.Abs(zDistanceFromTarget) / clippingPlane;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        startingZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        // When the target moves, move the parallax object the same distance time a multiplier
        Vector2 newPosition = startingPosition * camMoveSinceStart * parallaxFactor;

        // The x/y position changes based on target travel speed time the parallax factor, but z stays consistent
        transform.position = new Vector3(newPosition.x, newPosition.y, startingZ);
    }
}
