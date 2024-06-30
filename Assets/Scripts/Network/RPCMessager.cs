using Fusion;
using UnityEngine;
using VRC2.Pipe;

namespace VRC2.Network
{
    [RequireComponent(typeof(NetworkObject))]
    public class RPCMessager : NetworkBehaviour
    {
        public void UpdateLastSpawnedPipe(NetworkId nid)
        {
            RPC_UpdateLastSpawnedPipe(nid);
        }

        public void ResetLastSpawnedPipe()
        {
            RPC_ResetLastSpawnedPipe();
        }


        public void SetPipeRigidBody(NetworkId nid, bool enable)
        {
            RPC_SetPipeRigidBody(nid, enable);
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

        [Rpc(RpcSources.All, RpcTargets.All)]
        public void RPC_ResetLastSpawnedPipe(RpcInfo info = default)
        {
            print($"RPC_ResetLastSpawnedPipe");

            if (info.IsInvokeLocal)
            {
                print("local");
            }
            else
            {
                print("remote");
                GlobalConstants.lastSpawnedPipe = null;
            }
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        public void RPC_SetPipeRigidBody(NetworkId nid, bool enable, RpcInfo info = default)
        {
            print($"RPC_SetPipeRigidBody: {nid}");
            var go = Runner.FindObject(nid).gameObject;

            if (enable)
            {
                PipeHelper.AfterMove(ref go);
            }
            else
            {
                PipeHelper.BeforeMove(ref go);
            }
        }
    }
}