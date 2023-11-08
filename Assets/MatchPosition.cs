using UnityEngine;

public class MatchPosition : MonoBehaviour
{
    public Transform target; // Assign the target GameObject's Transform in the inspector

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            // Match the position of this GameObject to the target GameObject's position
            transform.position = new Vector3(target.position.x, this.transform.position.y, target.position.z);
        }
    }
}

