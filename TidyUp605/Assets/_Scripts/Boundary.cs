using UnityEngine;

public class Boundary : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void OnCollisionEnter(Collision other)
    {
        //should probably check first if the object is in motion, so the player can move around.
        if (other.gameObject) { }
        //then check the object's tag, so the specific item can be replaced. Calling the spawn function is proably the easiest way.
        if (other.gameObject.tag == "")
        {
            Destroy(other.gameObject);
        }
    }
}
