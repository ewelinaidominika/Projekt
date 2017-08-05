using System;
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
using Microsoft.Build;

namespace Fiszki
{
    /// <summary>
    /// Logika interakcji dla klasy AddNewCategory.xaml
    /// </summary>
    public partial class AddNewCategory : Page
    {
        #region constructor
        public AddNewCategory()
        {
            InitializeComponent();
        }
        #endregion

        #region private members

        /// <summary>
        /// Object that is used to choose next page to show (in menu buttons and adding new category - button click regions).
        /// </summary>
        private SetPage setPage;

        #endregion

        #region private methods
        #region adding new category - button click
        /// <summary>
        /// Adds new category after clicking a button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddNewCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            string polishNameOfCategory = PolishNameTextBox.Text;
            string englishNameOfCategory = EnglishNameTextBox.Text;
            if(polishNameOfCategory == "Polska" || polishNameOfCategory == null || englishNameOfCategory == "Angielska" || englishNameOfCategory == null)
            {
                MessageBox.Show("Nie zostały podane wszystkie potrzebne dane.");
            }
            else
            {
                string nameOfTextFile = englishNameOfCategory.ToLower() + ".txt";
                string foo = "..\\..\\";
                string pathToTextFile = foo + nameOfTextFile;

                try
                {
                    FileStream fileStream = new FileStream(@pathToTextFile, FileMode.CreateNew, FileAccess.Write);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                if(File.Exists(@pathToTextFile))
                {
                    var build = new Microsoft.Build.Evaluation.Project(@"..\\..\\Fiszki.csproj");
                    build.AddItem("Resource", nameOfTextFile);
                    build.Save();

                    Directory.CreateDirectory(@"..\\..\\Photos\\" + englishNameOfCategory);

                    FileStream fileStream = new FileStream(@"..\\..\\myOwnCategories.txt", FileMode.Open, FileAccess.ReadWrite);
                    try
                    {
                        using (StreamWriter sw = new StreamWriter(fileStream))
                        {
                            sw.BaseStream.Seek(0, SeekOrigin.End);
                            sw.WriteLine(polishNameOfCategory + " " + englishNameOfCategory);
                            sw.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
            }
            MessageBoxResult result = MessageBox.Show("Dodano kategorię o nazwie: " + polishNameOfCategory + ". Jeśli chcesz dodać następną kategorię, naciśnij tak, a jeśli chcesz wrócić do menu, naciśnij nie.", "Fiszki", MessageBoxButton.YesNo, MessageBoxImage.Information, MessageBoxResult.Yes);
            if(result == MessageBoxResult.Yes)
                setPage = new SetPage(5, MessageBoxResult.Yes);
            else
            {
                PolishNameTextBox.Name = "Polska";
                EnglishNameTextBox.Name = "Angielska";
            }
        }
        #endregion

        #region menu buttons
        /// <summary>
        /// These methods are using setPage object to switch pages after clicking a menu button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetTrainingMode_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz opuścić dodawanie nowej kategorii i przejść do trybu treningu? Żadne dokonane zmiany nie zostaną zapisane.", "Uwaga", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            setPage = new SetPage(1, result);
        }
        private void SetTestMode_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz opuścić dodawanie nowej kategorii i przejść do trybu testu? Żadne dokonane zmiany nie zostaną zapisane.", "Uwaga", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            setPage = new SetPage(2, result);
        }
        private void AddNewWord_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz opuścić dodawanie nowej kategorii i przejść do tworzenia własnego słowa? Żadne dokonane zmiany nie zostaną zapisane.", "Uwaga", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            setPage = new SetPage(3, result);
        }
        private void BackToMainPage_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz opuścić dodawanie nowej kategorii i wrócić do strony głównej? Żadne dokonane zmiany nie zostaną zapisane.", "Uwaga", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            setPage = new SetPage(5, result);
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz opuścić program?", "Uwaga", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            setPage = new SetPage(6, result);
        }
        #endregion

        #region textboxes
        /// <summary>
        /// These methods are cleaning textboxes when user is clicking on them.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PolishNameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            PolishNameTextBox.Text = string.Empty;
            PolishNameTextBox.GotFocus -= PolishNameTextBox_GotFocus;
        }
        private void EnglishNameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            EnglishNameTextBox.Text = string.Empty;
            EnglishNameTextBox.GotFocus -= EnglishNameTextBox_GotFocus;
        }
        #endregion
        #endregion
    }
}
