﻿using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using MHSS.Models.Data;
using MHSS.Models.Config;

namespace MHSS.ViewModels.Controls
{
    internal class DecoRegistItemViewModel// : SubViewModelBase
    {
        /// <summary>
        /// ComboBoxに表示するアイテムリスト
        /// </summary>
        public ReactivePropertySlim<ObservableCollection<int>> Items { get; } = new();

        /// <summary>
        /// 選択された装飾品名
        /// </summary>
        public ReactivePropertySlim<string> SelectedName { get; } = new();

        /// <summary>
        /// 選択された装飾品所持数
        /// </summary>
        public ReactivePropertySlim<int> SelectedCount { get; } = new();

        /// <summary>
        /// 背景色
        /// </summary>
        public ReactivePropertySlim<SolidColorBrush> BackgroundColor { get; } = new(Brushes.White);

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DecoRegistItemViewModel(Deco deco)
        {
            // これはテスト用
            BackgroundColor.Value = Brushes.Beige;

            // 装飾品名を取得
            SelectedName.Value = deco.Name;

            // ComboBox表示用アイテムを作成
            // スキルの最大レベルが5っぽいのでとりあえず5でハードコーディングしておく
            ObservableCollection<int> items = new();
            for (int i = 0; i <= Config.MaxDecoCount; i++)
            {
                items.Add(i);
            }
            Items.Value = items;

            // 初期値
            SelectedCount.Value = deco.HaveCount;


            // アイテムが選択されたら
            SelectedCount.Subscribe(count =>
            {
                // ComboBoxの背景色を変える
                if (count == 0) BackgroundColor.Value = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FADBD8"));
                else if (count == 5) BackgroundColor.Value = Brushes.Gainsboro;
                else BackgroundColor.Value = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C4E1FF"));

                // 所持数を出力する。ファイルへの書き込みはアプリ終了時に行う。
                Master.Decos.Single(d => d.Name == SelectedName.Value).HaveCount = SelectedCount.Value;
            });
        }

    }
}
