using UnityEngine;

public class UserInput : MonoBehaviour
{
    private const int LeftMouse = 0;

    private void Update()
    {
        if (Input.GetMouseButtonDown(LeftMouse) == false) 
            return;
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, Physics.DefaultRaycastLayers, 
            QueryTriggerInteraction.Ignore) == false) 
            return;

        if (hit.collider.GetComponentInParent<Colony>() != null)
        {
            Debug.Log("Parent is Colony");
        }
    }
}
