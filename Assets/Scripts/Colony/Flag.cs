using UnityEngine;

public class Flag : MonoBehaviour
{
    public void Place(Vector3 position)
    {
        transform.position = position;
        gameObject.SetActive(true);
    }

    public void Remove()
    {
        gameObject.SetActive(false);
    }
}
