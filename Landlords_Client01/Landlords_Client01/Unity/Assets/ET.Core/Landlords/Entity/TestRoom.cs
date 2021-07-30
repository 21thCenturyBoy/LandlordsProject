using System.Collections.Generic;
using System.Threading;

namespace ETModel
{
    /// <summary>
    /// 测试房间
    /// </summary>
    public sealed class TestRoom : Entity
    {
        public CancellationTokenSource waitCts;
        public CancellationTokenSource randCts;
        public Dictionary<int, string> gamers = new Dictionary<int, string>();

    }
}