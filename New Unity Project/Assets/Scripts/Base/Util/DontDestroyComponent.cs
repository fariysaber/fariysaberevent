using UnityEngine;
public class DontDestroyComponent : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
