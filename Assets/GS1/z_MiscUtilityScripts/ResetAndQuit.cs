using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// Hardcoded Quit and Reset script for resetting the current scene or quitting builds. 
// Note: Reset will work in editor or builds but Quit will only work in builds. 
public class ResetAndQuit : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame) {
            Debug.Log("Quitting");
            Application.Quit();    
        }

        if (Keyboard.current.rKey.wasPressedThisFrame) {
            Debug.Log("Reset Current Scene");
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex);
        }
    }

    public void Reset() {
        Debug.Log("Reset Current Scene");
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void Quit() {
        Debug.Log("Quitting");
        Application.Quit();
    }
   
}
