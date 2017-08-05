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

namespace Fiszki
{
    /// <summary>
    /// Logika interakcji dla klasy SetTrainingMode.xaml
    /// </summary>
    public partial class SetTrainingMode : Page
    {
        #region constructor
        public SetTrainingMode()
        {
            InitializeComponent();
            ComboBoxItems();
        }
        #endregion

        #region private members
        /// <summary>
        /// Object that contains polish and english name of selected category.
        /// </summary>
        private Category _category;

        /// <summary>
        /// An array that contains polish names of user's categories.
        /// </summary>
        private string[] polishNamesOfCategories;

        /// <summary>
        /// An array that contains english names of user's categories.
        /// </summary>
        private string[] englishNamesOfCategories;

        /// <summary>
        /// Variable that contains an amount of user's own categories.
        /// </summary>
        private int numberOfMyOwnCategories;

        /// <summary>
        /// Checks if names of categories should be added to NewCategoryComboBox so these names will not be copied to combobox more than once.
        /// </summary>
        private bool index = true;

        /// <summary>
        /// Object that is used to choose next page to show (in menu buttons region).
        /// </summary>
        private SetPage setPage;
        #endregion

        #region private methods
        #region start training
        /// <summary>
        /// Method that starts training after clicking StartTraining button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartTraining_Click(object sender, RoutedEventArgs e)
        {
            if (_category == null)
            {
                MessageBox.Show("Nie wybrano żadnej kategorii", "Uwaga", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                string name = _category.EnglishName.ToLower() + ".txt";
                string fullName = "..\\..\\" + name;
                Encoding enc = Encoding.Default;
                FileStream file = new FileStream(@fullName, FileMode.Open, FileAccess.Read);
                StreamReader stream = new StreamReader(file, enc);
                string[] buffer = File.ReadAllLines(@fullName, enc);
                int numberOfWords = buffer.Length;
                stream.Close();
                if(numberOfWords == 0)
                {
                    MessageBox.Show("Wybrana kategoria nie zawiera słów.");
                }
                else
                {
                    TrainingMode trainingMode = new TrainingMode(_category);
                    Application.Current.MainWindow.Content = trainingMode;
                }
            }
        }
        #endregion

        #region add user's own categories names to NewCategoryComboBox
        /// <summary>
        /// Method adds user's categories to NewCategoryComboBox so user can take a training of his own category.
        /// </summary>
        private void ComboBoxItems()
        {
            Encoding enc = Encoding.Default;
            FileStream fileStream = new FileStream(@"..\\..\\myOwnCategories.txt", FileMode.Open, FileAccess.Read);

            if (new FileInfo(@"..\\..\\myOwnCategories.txt").Length != 0)
            {
                try
                {
                    using (StreamReader stream = new StreamReader(fileStream))
                    {
                        string[] buffer = File.ReadAllLines(@"..\\..\\myOwnCategories.txt", enc);
                        numberOfMyOwnCategories = buffer.Length;

                        this.polishNamesOfCategories = new string[numberOfMyOwnCategories];
                        this.englishNamesOfCategories = new string[numberOfMyOwnCategories];
                        int polishIndex = 0;
                        int englishIndex = 0;

                        for (int i = 0; i < numberOfMyOwnCategories; i++)
                        {
                            foreach (string word in buffer[i].Split(' '))
                            {
                                if(polishIndex == englishIndex)
                                {
                                    byte[] wordBytes = Encoding.Default.GetBytes(word);
                                    string newWordString = Encoding.Default.GetString(wordBytes);
                                    this.polishNamesOfCategories[polishIndex] = newWordString;
                                    polishIndex++;
                                }
                                else
                                {
                                    this.englishNamesOfCategories[englishIndex] = word;
                                    englishIndex++;
                                }
                            }
                        }
                        stream.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                if(polishNamesOfCategories.Length != 0 && index == true)
                {
                    foreach(string word in polishNamesOfCategories)
                    {
                        NewCategoryComboBox.Items.Add(word);
                    }
                    index = false;
                }
            }
        }
        #endregion

        #region checkboxes
        /*
        ID of category:
        1 - Animals
        2 - Appearance
        3 - Fruits
        4 - Profession
        */ 
        /// <summary>
        /// These methods are changing other checkboxes' IsChecked property when some checkbox is being checked and changing value of members of _category object when checking/unchecking checkboxes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FruitsCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            AnimalsCheckBox.IsChecked = false;
            AppearanceCheckBox.IsChecked = false;
            ProfessionCheckBox.IsChecked = false;
            NewCategoryComboBox.SelectedItem = null;
            _category = new Category(3);
        }

        private void AnimalsCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            FruitsCheckBox.IsChecked = false;
            AppearanceCheckBox.IsChecked = false;
            ProfessionCheckBox.IsChecked = false;
            NewCategoryComboBox.SelectedItem = null;
            _category = new Category(1);
        }

        private void AppearanceCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            FruitsCheckBox.IsChecked = false;
            AnimalsCheckBox.IsChecked = false;
            ProfessionCheckBox.IsChecked = false;
            NewCategoryComboBox.SelectedItem = null;

            _category = new Category(2);
        }

        private void ProfessionCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            FruitsCheckBox.IsChecked = false;
            AnimalsCheckBox.IsChecked = false;
            AppearanceCheckBox.IsChecked = false;
            NewCategoryComboBox.SelectedItem = null;
            _category = new Category(4);
        }

        private void FruitsCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            _category = null;
        }

        private void AnimalsCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            _category = null;
        }

        private void AppearanceCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            _category = null;
        }

        private void ProfessionCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            _category = null;
        }
    #endregion

        #region menu buttons
        /// <summary>
        /// These methods are using setPage object to switch pages after clicking a menu button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetTestMode_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz anulować przejście do treningu i przejść do trybu testu?", "Uwaga", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            setPage = new SetPage(2, result);
        }
        private void AddNewWord_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz anulować przejście do treningu i przejść do tworzenia własnego słowa?", "Uwaga", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            setPage = new SetPage(3, result);
        }
        private void AddNewCategory_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz anulować przejście do treningu i przejść do dodawania nowej kategorii?", "Uwaga", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            setPage = new SetPage(4, result);
        }
        private void BackToMainPage_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz anulować przejście do treningu i wrócić do strony głównej?", "Uwaga", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            setPage = new SetPage(5, result);
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz opuścić program?", "Uwaga", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            setPage = new SetPage(6, result);
        }
    #endregion

        #region change of selection in NewCategoryComboBox - applied only, if selection isn't being changed to null (SelectedIndex = -1)
        /// <summary>
        /// This method changes _category object's members values when selection isn't being changed to null.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewCategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(NewCategoryComboBox.SelectedIndex != -1)
            {
                int index = NewCategoryComboBox.SelectedIndex;
                string polish = polishNamesOfCategories[index];
                string english = englishNamesOfCategories[index];

                FruitsCheckBox.IsChecked = false;
                AnimalsCheckBox.IsChecked = false;
                AppearanceCheckBox.IsChecked = false;
                ProfessionCheckBox.IsChecked = false;

                _category = new Category(polish, english);
            }
        }
        #endregion
        #endregion
    }
}
