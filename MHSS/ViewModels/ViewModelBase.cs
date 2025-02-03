using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Reactive.Disposables;

namespace MHSS.ViewModels
{
    public abstract class ViewModelBase : BindableBase, IDisposable, IDestructible
    {
        /// ReactiveProperty一括破棄用フィールド
        protected CompositeDisposable _disposables = new();
        /// 二重破棄防止用フラグ
        private bool _disposed;
        /// リソースの破棄処理
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// リソースの破棄処理の実態
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) { return; }
            if (disposing)
            {
                /* マネージリソースの破棄処理 */
                _disposables?.Dispose();
            }
            try { /* アンマネージドリソースの破棄処理 */ }
            catch { }
            _disposed = true;
        }
        /// ファイナライザ
        ~ViewModelBase() => Dispose(false);
        /// ViewModel破棄
        public void Destroy() => Dispose();
    }
}
