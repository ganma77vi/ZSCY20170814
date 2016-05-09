﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZSCY_Win10.Data.Community;
using ZSCY_Win10.ViewModels.Community;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ZSCY_Win10.Pages.CommunityPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CommunityPersonInfo : Page
    {
        CommunityPersonInfoViewModel ViewModel;
        double infoListScrollableHeight = 0;
        List<Img> clickImgList = new List<Img>();
        int clickImfIndex = 0;
        public CommunityPersonInfo()
        {
            this.InitializeComponent();
            this.SizeChanged += (s, e) =>
            {
                var state = "VisualState000";
                if (e.NewSize.Width > 800)
                {
                    state = "VisualState800";
                }
                VisualStateManager.GoToState(this, state, true);
                cutoffLine.Y2 = e.NewSize.Height;
            };
        }
        public Frame ContentFrame { get { return this.frame; } }

        private void PhotoGrid_ItemClick(object sender, ItemClickEventArgs e)
        {

            Img img = e.ClickedItem as Img;
            GridView gridView = sender as GridView;
            clickImgList = ((Img[])gridView.ItemsSource).ToList();
            clickImfIndex = clickImgList.IndexOf(img);
            CommunityItemPhotoFlipView.ItemsSource = clickImgList;
            CommunityItemPhotoFlipView.SelectedIndex = clickImfIndex;
            //CommunityItemPhotoImage.Source = new BitmapImage(new Uri(img.ImgSrc));
            CommunityItemPhotoGrid.Visibility = Visibility.Visible;
            //SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;
            //SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }



        private void CommunityItemPhoto_Tapped(object sender, TappedRoutedEventArgs e)
        {
            CommunityItemPhotoGrid.Visibility = Visibility.Collapsed;
            //SystemNavigationManager.GetForCurrentView().BackRequested -= App_BackRequested;
            //SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }

        private void App_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (!e.Handled)
            {
                if (CommunityItemPhotoGrid.Visibility == Visibility.Visible)
                {
                    CommunityItemPhotoGrid.Visibility = Visibility.Collapsed;
                    //SystemNavigationManager.GetForCurrentView().BackRequested -= App_BackRequested;
                    //SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                }
                else if (ContentFrame.Visibility == Visibility.Visible)
                {
                    if (!App.isPerInfoContentImgShow)
                    {   //SystemNavigationManager.GetForCurrentView().BackRequested -= App_BackRequested;
                        //SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                        ContentFrame.Visibility = Visibility.Collapsed;
                        //CommunityMyAppBarButton.Visibility = Visibility.Visible;
                    }
                }
                else
                {

                    if (Frame.CanGoBack)
                    {
                        Frame.GoBack();
                    }
                    SystemNavigationManager.GetForCurrentView().BackRequested -= App_BackRequested;
                    SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                }
            }
            e.Handled = true;
        }

        private void LikeButton_Click(object sender, RoutedEventArgs e)
        {

        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel = new CommunityPersonInfoViewModel(e.Parameter.ToString());
            SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().BackRequested -= App_BackRequested;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }

        private void infoListScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (infoListScrollViewer.VerticalOffset > (infoListScrollViewer.ScrollableHeight - 500) && infoListScrollViewer.ScrollableHeight != infoListScrollableHeight)
            {
                infoListScrollableHeight = infoListScrollViewer.ScrollableHeight;
                Debug.WriteLine("infoList继续加载");
                ViewModel.Get();
            }
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.ContentFrame.Visibility = Visibility.Visible;
            this.ContentFrame.Navigate(typeof(CommunityMyContentPage), e.ClickedItem);
        }
    }
}