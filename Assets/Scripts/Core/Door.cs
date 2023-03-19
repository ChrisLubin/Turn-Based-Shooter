using UnityEngine;

public class Door : MonoBehaviour
{
    public bool IsOpen { get; private set; }
    private Animator _animator;

    private void Awake()
    {
        this._animator = GetComponent<Animator>();
        this.IsOpen = false;
    }

    public void Interact() => this.ToggleOpen();

    public void ToggleOpen()
    {
        this.IsOpen = !this.IsOpen;
        this._animator.SetBool("IsOpen", this.IsOpen);
    }
}
