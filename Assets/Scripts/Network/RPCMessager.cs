using Fusion;
using UnityEngine;

namespace VRC2.Network
{
    [RequireComponent(typeof(NetworkObject))]
    public class RPCMessager : NetworkBehaviour
    {
        public void UpdateLastSpawnedPipe(NetworkId nid)
        {
            RPC_UpdateLastSpawnedPipe(nid);
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        public void RPC_UpdateLastSpawnedPipe(NetworkId nid, RpcInfo info = default)
        {
            print($"RPC_UpdateLastSpawnedPipe: {nid}");

            if (info.IsInvokeLocal)
            {
                print("local");
            }
            else
            {
                print("remote");
                var go = Runner.FindObject(nid).gameObject;
                GlobalConstants.lastSpawnedPipe = go;
            }
        }
    }
}