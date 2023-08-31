﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClassIsland.Controls;
/// <summary>
/// StarRailLoadingControl.xaml 的交互逻辑
/// </summary>
public partial class StarRailLoadingControl : UserControl
{
    private void BeginStoryBoard(string key)
    {
        var sb = (Storyboard)FindResource(key);
        BeginStoryboard(sb);
    }

    public StarRailLoadingControl()
    {
        InitializeComponent();
    }

    private void PART_ControlRoot_Loaded(object sender, RoutedEventArgs e)
    {

    }

    protected override async void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        if (e.Property == IsVisibleProperty)
        {
            var loop = (Storyboard)FindResource("Loop");
            if ((bool)e.NewValue)
            {
                loop.Stop();
                loop.Seek(TimeSpan.Zero);
                BeginStoryBoard("OnLoaded");
                await Task.Run(() => Thread.Sleep(100));
                BeginStoryBoard("Loop");
            }
            else
            {
                loop.Stop();
                loop.Seek(TimeSpan.Zero);
            }
        }
        base.OnPropertyChanged(e);
    }

    private void Loop_OnCompleted(object? sender, EventArgs e)
    {
        var sb = (Storyboard)FindResource("Loop");
        sb.Seek(TimeSpan.Zero);

        BeginStoryBoard("Loop");

    }
}

