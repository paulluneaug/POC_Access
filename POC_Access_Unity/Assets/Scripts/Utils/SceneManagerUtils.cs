using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneManagerUtils
{
    public static async void ReloadScene(Scene sceneToReload, LoadSceneMode loadMode)
    {
        AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(sceneToReload);
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneToReload.buildIndex, LoadSceneMode.Single);

        await unloadOperation;
        await loadOperation;
    }

}
