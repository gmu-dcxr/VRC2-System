using Fusion;

namespace VRC2.Network
{
    public class P1P2RoleChecker : NetworkBehaviour
    {
        public bool IsRunnerReady()
        {
            return Runner != null && Runner.isActiveAndEnabled && Runner.IsRunning;
        }

        public bool IsP1()
        {
            if (IsRunnerReady())
            {
                return Runner.IsServer;
            }

            return true;
        }

        public bool IsP2()
        {
            if (IsRunnerReady())
            {
                return Runner.IsClient;
            }

            return false;
        }

    }
}