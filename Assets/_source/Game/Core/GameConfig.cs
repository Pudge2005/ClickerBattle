using Unity.Netcode;

namespace Game.Core
{
    public struct GameConfig : INetworkSerializable
    {
        public double BalanceToWin;
        public float TimeLimit;
        public float StartTimeDelay;


        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref BalanceToWin);
            serializer.SerializeValue(ref TimeLimit);
            serializer.SerializeValue(ref StartTimeDelay);
        }
    }

}
