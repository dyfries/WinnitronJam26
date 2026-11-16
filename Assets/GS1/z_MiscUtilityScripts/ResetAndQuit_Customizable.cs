using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// Customizable Quit and Reset script for resetting the current scene or quitting builds.
// This one doesn't provide bindings, but you can add custom bindings in the inspector. 
// Note: Reset will work in editor or builds but Quit will only work in builds. 
public class ResetAndQuit_Customizable: MonoBehaviour
{
    [SerializeField] private InputAction resetAction;
    [SerializeField] private InputAction quitAction;

    private void OnEnable() {
        // 1. Enable the actions so they listen for input
        resetAction.Enable();
        quitAction.Enable();

        // 2. Bind the performance functions to the key-down event (.started)
        resetAction.started += OnResetTriggered;
        quitAction.started += OnQuitTriggered;
    }

    private void OnDisable() {
        // 3. Clean up bindings and disable to prevent memory leaks
        resetAction.started -= OnResetTriggered;
        quitAction.started -= OnQuitTriggered;

        resetAction.Disable();
        quitAction.Disable();
    }

    private void OnResetTriggered(InputAction.CallbackContext context) {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    private void OnQuitTriggered(InputAction.CallbackContext context) {
        // These conditionals are preprocessor directives and will be executed prior to compilation. 
        // In this case, it gives a different option if you are playing within the editor or build
        #if UNITY_EDITOR
              UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    // useful for calling from Unity Events. 
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
