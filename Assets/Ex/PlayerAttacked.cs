using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAttacked : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            SceneManager.LoadScene("AwesomecutsceneScene");
        }
    }
}
