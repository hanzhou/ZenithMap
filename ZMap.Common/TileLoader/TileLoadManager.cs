using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Threading;

namespace ZMap
{
    public delegate void TileLoadProgressChangedEventHandler(TileLoadProgressChangedEventArgs e);
    public delegate void TileLoadCompletedEventHandler(object sender, TileLoadCompletedEventArgs e);

    public class TileLoadManager
    {
        private delegate void WorkerEventHandler(RawTile tileKey, AsyncOperation asyncOp);
        private HybridDictionary userStateToLifetime = new HybridDictionary();
        private object eventLock = new object();
        private int loadingTaskNum = 0;
        private TileLoadProxy loadProxy;

        public TileLoadManager(TileLoadProxy tileloadProxy)
        {
            loadProxy = tileloadProxy;
        }

        private void OnTileLoadProgressChanged(object state)
        {
            TileLoadProgressChangedEventArgs e = state as TileLoadProgressChangedEventArgs;
            if (tileLoadProgressChanged != null)
                tileLoadProgressChanged(e);
        }

        private void OnTileLoadCompleted(object state)
        {
            TileLoadCompletedEventArgs e = state as TileLoadCompletedEventArgs;
            if (tileLoadCompleted != null)
                tileLoadCompleted(this, e);
        }

        private bool TaskCanceled(object taskId)
        {
            return (userStateToLifetime[taskId] == null);
        }

        private void LoadingWork(RawTile tileKey, AsyncOperation asyncOp)
        {
            MemoryStream ms = null;
            Exception ex = null;
            object taskID = tileKey as object;
            bool canceled = true;
            if (!TaskCanceled(taskID))
            {
                try
                {
                    ms = loadProxy.GetTile(tileKey);
                }
                catch (Exception exception)
                {
                    ex = exception;
                }
                canceled = TaskCanceled(taskID);
                lock (userStateToLifetime.SyncRoot)
                    userStateToLifetime.Remove(asyncOp.UserSuppliedState);
            }
            TileLoadCompletedEventArgs e = new TileLoadCompletedEventArgs(tileKey, ms, ex, canceled, taskID);
            if (--loadingTaskNum == 0)
            {
                //OnMapLoadCompleted(new MapLoadCompletedEventArgs());
                IsLoading = false;
            }
            asyncOp.PostOperationCompleted(new SendOrPostCallback(OnTileLoadCompleted), e);
        }


        #region public methods and properties

        /// <summary>
        /// 以异步方式下载图片
        /// </summary>
        /// <param name="tile"></param>
        public void LoadTileAsync(RawTile tile)
        {
            object taskID = tile as object;
            AsyncOperation asyncOp;
            lock (userStateToLifetime.SyncRoot)
            {
                if (userStateToLifetime.Contains(taskID))
                    return;//throw new ArgumentException("Task ID(tile) parameter must be unique", "tile");
                asyncOp = AsyncOperationManager.CreateOperation(taskID);
                userStateToLifetime[taskID] = asyncOp;
            }
            // Start the asynchronous operation.
            (new WorkerEventHandler(LoadingWork)).BeginInvoke(tile, asyncOp, null, null);
            if (loadingTaskNum++ == 0)
            {
                //OnMapLoadStarted(new MapLoadStartedEventArgs());
                IsLoading = true;
            }
        }

        /// <summary>
        /// 取消指定任务
        /// </summary>
        /// <param name="tile"></param>
        public void CancelAsync(RawTile tile)
        {
            object taskID = tile as object;
            AsyncOperation asyncOp = userStateToLifetime[taskID] as AsyncOperation;
            if (asyncOp != null)
                lock (userStateToLifetime.SyncRoot)
                    userStateToLifetime.Remove(taskID);
        }

        /// <summary>
        /// 取消所有任务
        /// </summary>
        public void CancelAllTasks()
        {
            lock (userStateToLifetime.SyncRoot)
                userStateToLifetime.Clear();
        }

        /// <summary>
        /// gets whether it is loading tiles
        /// </summary>
        [DefaultValue(false)]
        public bool IsLoading
        {
            get;
            private set;
        }

        #endregion

        #region event

        private event TileLoadProgressChangedEventHandler tileLoadProgressChanged;
        private event TileLoadCompletedEventHandler tileLoadCompleted;

        public event TileLoadProgressChangedEventHandler TileLoadProgressChanged
        {
            add { lock (eventLock) { this.tileLoadProgressChanged += value; } }
            remove { lock (eventLock) { this.tileLoadProgressChanged -= value; } }
        }

        public event TileLoadCompletedEventHandler TileLoadCompleted
        {
            add { lock (eventLock) { this.tileLoadCompleted += value; } }
            remove { lock (eventLock) { this.tileLoadCompleted -= value; } }
        }

        #endregion
    }
}
