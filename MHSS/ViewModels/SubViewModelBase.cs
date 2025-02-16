using MHSS.ViewModels.SubView;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;

namespace MHSS.ViewModels
{
    internal class SubViewModelBase : BindableBase, IDisposable, IDestructible
    {
        /// <summary>
        /// MainWindowViewModelのインスタンス
        /// </summary>
        protected MainWindowViewModel MainWindowVM { get => MainWindowViewModel.Instance; }


        protected SkillSelectViewModel SkillSelectVM { get => MainWindowVM.SkillSelectVM.Value; }

        protected ObservableCollection<SolutionViewModel> SolutionVM { get => MainWindowVM.SolutionVM.Value; }


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
        ~SubViewModelBase() => Dispose(false);
        /// ViewModel破棄
        public void Destroy() => Dispose();
    }
}
