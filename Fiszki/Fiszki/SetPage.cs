using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Fiszki
{
    class SetPage
    {
        #region constructor
        public SetPage(int _page, MessageBoxResult _result)
        {
            if(_result == MessageBoxResult.Yes)
                switch(_page)
                {
                    case 1:
                        //SetTrainingMode page
                        SetTrainingMode setTrainingMode = new SetTrainingMode();
                        Application.Current.MainWindow.Content = setTrainingMode;
                        break;
                    case 2:
                        //SetTestMode page
                        SetTestMode setTestMode = new SetTestMode();
                        Application.Current.MainWindow.Content = setTestMode;
                        break;
                    case 3:
                        //AddNewWord page
                        AddNewWord addNewWord = new AddNewWord();
                        Application.Current.MainWindow.Content = addNewWord;
                        break;
                    case 4:
                        //AddNewCategory page
                        AddNewCategory addNewCategory = new AddNewCategory();
                        Application.Current.MainWindow.Content = addNewCategory;
                        break;
                    case 5:
                        //MainPage page
                        MainPage mainPage = new MainPage();
                        Application.Current.MainWindow.Content = mainPage;
                        break;
                    case 6:
                        //Exit
                        Application.Current.Shutdown();
                        break;
                    default:
                        MessageBox.Show("Wystąpił błąd związany z kodem - nieprawidłowy kod ID kategorii.", "Uwaga", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
            }
        }
        #endregion
    }
}
