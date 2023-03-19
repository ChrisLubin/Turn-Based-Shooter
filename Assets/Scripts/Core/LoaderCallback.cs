using UnityEngine;

public class LoaderCallback : MonoBehaviour
{
    private bool _isFirstUpdate = true;

    private void Update()
    {
        if (this._isFirstUpdate)
        {
            this._isFirstUpdate = false;
            Loader.LoaderCallback();
        }
    }
}
