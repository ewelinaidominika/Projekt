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
using Microsoft.Win32;

namespace Fiszki
{
    public partial class AddNewWord : Page
    {
        #region constructor
        public AddNewWord()
        {
            InitializeComponent();
            ComboBoxItems();
        }
        #endregion

        #region private members
        /// <summary>
        /// Contains path, from which chosen image will be copied.
        /// </summary>
        private string pathToImage = "";

        /// <summary>
        /// Name of chosen image.
        /// </summary>
        private string imageName = "";

        /// <summary>
        /// Name of chosen category, word will be added to this category.
        /// </summary>
        private string nameOfCategory = "";

        /// <summary>
        /// Word from PolishWordTextBox.
        /// </summary>
        private string polishWord = "";

        /// <summary>
        /// Word from EnglishWordTextBox.
        /// </summary>
        private string englishWord = "";

        /// <summary>
        /// An array that contains polish names of existing categories - these names will be shown in CategoryComboBox.
        /// </summary>
        private string[] polishNamesOfCategories;

        /// <summary>
        /// An array that contains english names of existing categories - these name will be added to each item in CategoryComboBox as a Name property.
        /// </summary>
        private string[] englishNamesOfCategories;

        /// <summary>
        /// Checks if names of categories should be added to CategoryComboBox so these names will not be copied to combobox more than once.
        /// </summary>
        private bool index = true;

        /// <summary>
        /// Object that is used to choose next page to show (in menu buttons region).
        /// </summary>
        private SetPage setPage;

        /// <summary>
        /// Delegate that is used in adding new word after clicking EddAddingNewWordButton to switch between AddDefaultImage and AddUsersImage methods.
        /// </summary>
        private delegate void AddImage();
        private AddImage addImage;
        #endregion

        #region private methods
        #region add photo to new word using a file dialog
        /// <summary>
        /// This method opens a file dialog after clicking a AddPhotoButton so user can select a photo to add to new word.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Bitmapy (*.bmp)|*.bmp|All files (*.*)|*.*"
            };
            Nullable<bool> result = openFileDialog.ShowDialog();
            
            if (result == true)
            {
                pathToImage = openFileDialog.FileName;
                imageName = openFileDialog.SafeFileName;

                BitmapImage logo = new BitmapImage();
                logo.BeginInit();
                logo.UriSource = new Uri(pathToImage);
                logo.EndInit();

                photoPreview.Source = logo;
            }
        }
    #endregion

        #region get names of user's categories and add them to CategoryComboBox
        /// <summary>
        /// This method gets names of user's categories from files and then adds them to CategoryCombobox
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
                        int numberOfMyOwnCategories = buffer.Length;

                        this.polishNamesOfCategories = new string[numberOfMyOwnCategories];
                        this.englishNamesOfCategories = new string[numberOfMyOwnCategories];
                        int polishNameIndex = 0;
                        int englishNameIndex = 0;

                        for (int i = 0; i < numberOfMyOwnCategories; i++)
                        {
                            foreach (string word in buffer[i].Split(' '))
                            {
                                if (polishNameIndex == englishNameIndex)
                                {
                                    byte[] wordBytes = Encoding.Default.GetBytes(word);
                                    string newWordString = Encoding.Default.GetString(wordBytes);
                                    this.polishNamesOfCategories[polishNameIndex] = newWordString;
                                    polishNameIndex++;
                                }
                                else
                                {
                                    this.englishNamesOfCategories[englishNameIndex] = word;
                                    englishNameIndex++;
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
                if (polishNamesOfCategories.Length != 0 && index == true)
                {
                    foreach (string word in polishNamesOfCategories)
                    {
                        ComboBoxItem item = new ComboBoxItem()
                        {
                            Content = word
                        };
                        int index = Array.IndexOf(polishNamesOfCategories, word);
                        item.Name = englishNamesOfCategories[index];
                        CategoryComboBox.Items.Add(item);
                    }
                    index = false;
                }
            }
        }
    #endregion

        #region add new word after clicking EndAddingNewWordButton
        /// <summary>
        /// This method adds new word to chosen category after clicking EndAddingNewWordButton
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EndAddingNewWordButton_Click(object sender, RoutedEventArgs e)
        {
            polishWord = PolishWordTextBox.Text;
            englishWord = EnglishWordTextBox.Text;

            if (CategoryComboBox.SelectedItem == null || polishWord == "Polski" || polishWord == null || englishWord == "Angielski" || englishWord == null)
            {
                MessageBox.Show("Nie uzupełniono wszystkich pól", "Uwaga", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (pathToImage == "")
                {
                    addImage = new AddImage(AddDefaultImage);
                }
                else
                {
                    addImage = new AddImage(AddUsersImage);
                }
            }
            addImage();
        }
        #endregion

        #region if user didn't select his own image, use default image.
        /// <summary>
        /// Adds the default image.
        /// </summary>
        private void AddDefaultImage()
        {
            nameOfCategory = ((ComboBoxItem)CategoryComboBox.SelectedItem).Name;

            pathToImage = "..\\..\\Photos\\question_mark.bmp";
            MessageBox.Show("Nie wybrano własnego zdjęcia, zostanie dodane zdjęcie domyślne.");

            string foo = "..\\..\\";
            string pathToTextFile = foo + nameOfCategory.ToLower() + ".txt";
            Encoding enc = Encoding.Default;
            FileStream file = new FileStream(pathToTextFile, FileMode.Open, FileAccess.Write);

            try
            {
                using (StreamWriter stream = new StreamWriter(file, enc))
                {
                    stream.BaseStream.Seek(0, SeekOrigin.End);
                    stream.WriteLine(polishWord + " " + englishWord + " " + pathToImage);
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            MessageBox.Show("Dodano słowo " + polishWord + " do kategorii " + nameOfCategory + " , ścieżka zdjęcia: " + pathToImage);
            MainPage mainPage = new MainPage();
            Application.Current.MainWindow.Content = mainPage;
        }
        #endregion

        #region if user selected his own image, use it.
        /// <summary>
        /// Adds the users image.
        /// </summary>
        private void AddUsersImage()
        {
            nameOfCategory = ((ComboBoxItem)CategoryComboBox.SelectedItem).Name;
            string foo = "..\\..\\Photos\\";
            string newPath = foo + nameOfCategory + "\\" + imageName;
            System.IO.File.Copy(@pathToImage, @newPath);

            string foo1 = "..\\..\\";
            string pathToTextFile = foo1 + nameOfCategory.ToLower() + ".txt";
            Encoding enc = Encoding.Default;
            FileStream file = new FileStream(pathToTextFile, FileMode.Open, FileAccess.Write);

            try
            {
                using (StreamWriter stream = new StreamWriter(file, enc))
                {
                    stream.BaseStream.Seek(0, SeekOrigin.End);
                    stream.WriteLine(polishWord + " " + englishWord + " " + newPath);
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            var build = new Microsoft.Build.Evaluation.Project(@"..\\..\\Fiszki.csproj");
            build.AddItem("Resource", "Photos\\" + nameOfCategory + "\\" + imageName);
            build.Save();

            MessageBox.Show("Dodano słowo " + polishWord + " do kategorii " + nameOfCategory + " , ścieżka zdjęcia: " + newPath);
            MainPage mainPage = new MainPage();
            Application.Current.MainWindow.Content = mainPage;
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
            MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz opuścić dodawanie nowego słowa i przejść do trybu treningu? Żadne dokonane zmiany nie zostaną zapisane.", "Uwaga", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            setPage = new SetPage(1, result);
        }
        /// <summary>
        /// Handles the Click event of the SetTestMode control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void SetTestMode_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz opuścić dodawanie słowa i przejść do trybu testu? Żadne dokonane zmiany nie zostaną zapisane.", "Uwaga", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            setPage = new SetPage(2, result);
        }
        /// <summary>
        /// Handles the Click event of the AddNewCategoryButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void AddNewCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz opuścić dodawanie nowego słowa i przejść do dodawania nowej kategorii? Żadne zmiany nie zostaną zapisane.", "Uwaga", MessageBoxButton.YesNo, MessageBoxImage.Question);
            setPage = new SetPage(4, result);
        }
        /// <summary>
        /// Handles the Click event of the BackToMainPage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void BackToMainPage_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz opuścić dodawanie słowa i wrócić do strony głównej? Żadne dokonane zmiany nie zostaną zapisane.", "Uwaga", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            setPage = new SetPage(5, result);
        }
        /// <summary>
        /// Handles the Click event of the Exit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz opuścić program?", "Uwaga", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            setPage = new SetPage(6, result);
        }
        #endregion

        #region textboxes
        /// <summary>
        /// Handles the GotFocus event of the PolishWordTextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void PolishWordTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            PolishWordTextBox.Text = string.Empty;
            PolishWordTextBox.GotFocus -= PolishWordTextBox_GotFocus;
        }
        /// <summary>
        /// Handles the GotFocus event of the EnglishWordTextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void EnglishWordTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            EnglishWordTextBox.Text = string.Empty;
            EnglishWordTextBox.GotFocus -= EnglishWordTextBox_GotFocus;
        }
        #endregion

        #region reset all the user's settings after clicking ResetButton
        /// <summary>
        /// This method resets all the changes that user made after clicking ResetButton
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            CategoryComboBox.SelectedItem = null;

            PolishWordTextBox.Text = "Polski";
            PolishWordTextBox.GotFocus += PolishWordTextBox_GotFocus;

            EnglishWordTextBox.Text = "Angielski";
            EnglishWordTextBox.GotFocus += EnglishWordTextBox_GotFocus;

            pathToImage = "";
            imageName = "";
            nameOfCategory = "";
            polishWord = "";
            englishWord = "";

            photoPreview.Source = null;
        }
        #endregion
        #endregion
    }
}
