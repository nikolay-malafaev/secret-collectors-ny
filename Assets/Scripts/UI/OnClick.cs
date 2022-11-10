using UnityEngine;

public class OnClick : MonoBehaviour
{
    public GameObject currenGameObject;

    public void Activator(bool value)
    {
        currenGameObject.SetActive(value);
    }

    public void Activator()
    {
        gameObject.SetActive(!currenGameObject.activeSelf); 
    }
}
