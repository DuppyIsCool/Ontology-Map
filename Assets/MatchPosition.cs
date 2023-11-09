using UnityEngine;

public class MatchPosition : MonoBehaviour
{
    public Transform target; 

    //Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            //Match the position of this GameObject to the target GameObject's position, excluding y changes.
            transform.position = new Vector3(target.position.x, this.transform.position.y, target.position.z);
        }
    }
}

