﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace Fiszki
{
    public partial class MainPage : Page
    {
        #region constructor
        public MainPage()
        {
            InitializeComponent();
            ShowBitmaps();
        }
        #endregion

        #region private member
        /// <summary>
        /// Object that is used to choose next page to show (in menu buttons region).
        /// </summary>
        private SetPage setPage;
        #endregion

        #region private methods
        #region menu buttons
        /// <summary>
        /// These methods are using setPage object to switch pages after clicking a menu button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetTrainingMode_Click(object sender, RoutedEventArgs e)
        {
            setPage = new SetPage(1, MessageBoxResult.Yes);
        }

        /// <summary>
        /// Handles the Click event of the SetTestMode control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void SetTestMode_Click(object sender, RoutedEventArgs e)
        {
            setPage = new SetPage(2, MessageBoxResult.Yes);
        }
        /// <summary>
        /// Handles the Click event of the AddNewWord control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void AddNewWord_Click(object sender, RoutedEventArgs e)
        {
            setPage = new SetPage(3, MessageBoxResult.Yes);
        }
        /// <summary>
        /// Handles the Click event of the AddNewCategoryButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void AddNewCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            setPage = new SetPage(4, MessageBoxResult.Yes);
        }
        /// <summary>
        /// Handles the Click event of the Exit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            setPage = new SetPage(6, MessageBoxResult.Yes);
        }
        #endregion

        #region show polish and english flags
        /// <summary>
        /// This method shows flag on the MainPage.
        /// </summary>
        void ShowBitmaps()
        {
            BitmapImage imageEnglish = new BitmapImage(new Uri("..\\..\\Photos\\ang.bmp", UriKind.Relative));
            flagEnglish.Source = imageEnglish;

            BitmapImage imagePolish = new BitmapImage(new Uri("..\\..\\Photos\\pol.bmp", UriKind.Relative));
            flagPolish.Source = imagePolish;
        }
        #endregion
        #endregion
    }
}
