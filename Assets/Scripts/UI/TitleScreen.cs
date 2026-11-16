using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public void PlayButton()
    {
        SceneManager.LoadScene(3);
    }
}
