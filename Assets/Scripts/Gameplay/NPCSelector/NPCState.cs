namespace NPCSelector
{
    public enum NPCState
    {
        Locked,    // Grey outline, non-interactable
        Unknown,   // Black picture, interactable
        NoName,    // Show portrait + ??? overlay, interactable
        Named      // Show portrait + name, interactable
    }
}