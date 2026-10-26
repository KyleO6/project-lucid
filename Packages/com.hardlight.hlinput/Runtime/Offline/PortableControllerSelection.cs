using Hardlight;

namespace ProjectLucid.Offline
{
    // Intentional desktop port selection at HLInputModule's existing provider
    // boundary (H-013). The original Apple selection remains in its research branch.
    // The Input System provider keeps the configured polling rate (#454).
    public static class PortableControllerSelection
    {
        public static BaseControllerProvider Create(float pollingRateInSeconds)
        {
            return new InputSystemControllerProvider(pollingRateInSeconds)
            {
                // The polling invokes this on a connection change,
                // including before an input monitor has subscribed.
                OnControllerConnectionUpdate = new FastAction()
            };
        }
    }
}
