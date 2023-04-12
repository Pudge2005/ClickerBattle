using Unity.Netcode;

namespace Game.Core
{
    public struct GameResult : INetworkSerializable
    {
        public ulong WinnerId;
        public GameFinishedReason FinishReason;


        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref WinnerId);
            serializer.SerializeValue(ref FinishReason);
        }
    }

}
