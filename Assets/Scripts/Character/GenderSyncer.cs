using Fusion;

namespace VRC2.Character
{
    public class GenderSyncer : NetworkBehaviour
    {
        public void Synchronize(int pid, bool male)
        {
            if (Runner != null && Runner.isActiveAndEnabled)
            {
                RPC_SendMessage(pid, male);
            }
            else
            {
                print("Runner is not valid");
            }
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        private void RPC_SendMessage(int playerid, bool male, RpcInfo info = default)
        {
            print($"{playerid} {male}");
        }
    }
}