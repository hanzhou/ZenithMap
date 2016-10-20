using System.ComponentModel;

namespace ZMap
{
    public class TileLoadProgressChangedEventArgs : ProgressChangedEventArgs
    {
        public TileLoadProgressChangedEventArgs(int progressPercentage, object userState)
            : base(progressPercentage, userState)
        { }
    }
}
