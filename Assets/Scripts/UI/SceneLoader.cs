using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
	[SerializeField]
	[Tooltip("The path to the scene that will be loaded")]
	private string _scenePath;

	public string ScenePath
	{
		get
		{
			return this._scenePath;
		}
	}

	[SerializeField]
	[Tooltip("The load mode to use")]
	private LoadSceneMode _mode;

	public LoadSceneMode Mode
	{
		get
		{
			return this._mode;
		}
	}

	[SerializeField]
	[Tooltip("The path to the loading scene")]
	private string _loadingScenePath;

	public string LoadingScenePath
	{
		get
		{
			return this._loadingScenePath;
		}
	}

	/** Whether the specified scene should be loaded asynchronously */
	public bool IsAsync
	{
		get
		{
			return this.LoadingScenePath != null || this.Mode != LoadSceneMode.Single;
		}
	}

	/** The current scene loading operation */
	public static AsyncOperation LoadingOperation {get; private set;}

	/** Load the specified scene with the specified options */
	public void LoadScene()
	{
		if (this.IsAsync)
		{
			if (this.LoadingScenePath != null)
			{
				SceneManager.LoadScene(this.LoadingScenePath);
			}
			SceneLoader.LoadingOperation = SceneManager.LoadSceneAsync(this.ScenePath, this.Mode);
			SceneLoader.LoadingOperation.completed += delegate(AsyncOperation op){DynamicGI.UpdateEnvironment();};
			if (this.Mode == LoadSceneMode.Single)
			{
				SceneLoader.LoadingOperation.completed += delegate(AsyncOperation op){Object.Destroy(this);};
			}
		}
		else
		{
			SceneManager.LoadScene(this.ScenePath, this.Mode);
		}
	}

	/** Load the scene at the given path */
	public void LoadScene(string scene_path)
	{
		this._scenePath = scene_path;
		this.LoadScene();
	}
}