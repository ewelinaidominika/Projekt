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
    /// Logika interakcji dla klasy TrainingMode.xaml
    /// </summary>
    public partial class TrainingMode : Page
    {
        #region constructor
        public TrainingMode(Category _category)
        {
            InitializeComponent();
            nameOfCategory = _category.EnglishName;
            polishNameOfCategory = _category.PolishName;

            LoadFile();
            PageContent();
        }
        #endregion

        #region private members
        /// <summary>
        /// Contains a total amount of words in trained category.
        /// </summary>
        private int numberOfWords;

        /// <summary>
        /// Contains a number of page that is currently being shown.
        /// </summary>
        private int page = 1; 

        /// <summary>
        /// According to value of this variable program can check if it should show polish or english word.
        /// </summary>
        private int whichPage = 1;

        /// <summary>
        /// Checks if names of categories should be added to GoToWordComboBox so these names will not be copied to combobox more than once.
        /// </summary>
        private int index = 1;

        /// <summary>
        /// An array that contains polish words of category that is being trained.
        /// </summary>
        private string[] polishWords;

        /// <summary>
        /// An array that contains english words of category that is being trained.
        /// </summary>
        private string[] englishWords;

        /// <summary>
        /// Contains english name of category that is being trained.
        /// </summary>
        private string nameOfCategory;

        /// <summary>
        /// Contains polish name of category that is being trained.
        /// </summary>
        private string polishNameOfCategory;

        /// <summary>
        /// Contains source of photo that is added to shown word.
        /// </summary>
        private string[] photoSource;

        /// <summary>
        /// Object that is used to choose next page to show (in menu buttons region).
        /// </summary>
        private SetPage setPage;
        #endregion

        #region private methods
        #region load file and words
        private void LoadFile()
        {
            string foo = "..\\..\\";
            string foo2 = ".txt";
            string source = foo + nameOfCategory + foo2;
            LoadWords(source);
        }

        private void LoadWords(string source)
        {
            Encoding enc = Encoding.Default;
            FileStream file = new FileStream(@source, FileMode.Open, FileAccess.Read);

            try
            {
                using (StreamReader stream = new StreamReader(file, enc))
                {
                    string[] buffer = File.ReadAllLines(@source, enc);
                    numberOfWords = buffer.Length;

                    this.polishWords = new string[numberOfWords];
                    this.englishWords = new string[numberOfWords];
                    this.photoSource = new string[numberOfWords];

                    int polishIndex = 0;
                    int englishIndex = 0;
                    int sourceIndex = 0;

                    for (int i = 0; i < numberOfWords; i++)
                    {
                        foreach (string word in buffer[i].Split(' '))
                        {
                            sourceIndex++;
                            if (polishIndex == englishIndex && sourceIndex % 3 == 1)
                            {
                                byte[] wordBytes = Encoding.Default.GetBytes(word);
                                string newWordString = Encoding.Default.GetString(wordBytes);
                                this.polishWords[polishIndex] = newWordString;
                                polishIndex++;
                            }
                            else if(polishIndex != englishIndex && sourceIndex % 3 == 2)
                            {
                                this.englishWords[englishIndex] = word;
                                englishIndex++;
                            }
                            if (sourceIndex % 3 == 0 && polishIndex == englishIndex)
                            {
                                this.photoSource[polishIndex - 1] = word.ToString();
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
        }
        #endregion

        #region show and update content of page
        private void PageContent()
        {
            string numberOfQuestion = page.ToString();
            string backslash = "/";
            string howManyQuestions = numberOfWords.ToString();
            questionNumberTextBlock.Text = numberOfQuestion + backslash + howManyQuestions;
            wordTextBlock.Text = this.polishWords[page - 1];

            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(this.photoSource[page - 1], UriKind.Relative);
            bitmapImage.EndInit();

            Image.Source = bitmapImage;

            if (page == numberOfWords)
            {
                ContinueButton.Content = "Zakończ trening";
            }

            if(index ==1)
            {
                foreach (string s in polishWords)
                {
                    GoToWordComboBox.Items.Add(s);
                }
                index++;
            }
        }
        #endregion

        #region continue button
        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            if(page != numberOfWords)
            {
                page++;
                PageContent();
                whichPage = 1;
                wordTextBlock.Foreground = Brushes.Black;
            }
            else
            {
                string content = String.Format("Ukończyłeś trening słownictwa dla kategorii: {0}", polishNameOfCategory);
                MessageBoxResult result = MessageBox.Show(content, "Koniec treningu", MessageBoxButton.OK, MessageBoxImage.Information);
                MainPage mainPage = new MainPage();
                Application.Current.MainWindow.Content = mainPage;
            }
        }
        #endregion

        #region menu buttons
        private void SetTestMode_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz opuścić trening i przejść do trybu testu?", "Uwaga", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            setPage = new SetPage(2, result);
        }
        private void AddNewWord_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz opuścić trening i przejść do tworzenia własnego słowa?", "Uwaga", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            setPage = new SetPage(3, result);
        }
        
        private void AddNewCategory_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz opuścić trening i przejść do dodawania nowej kategorii?", "Uwaga", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            setPage = new SetPage(4, result);
        }
        private void BackToMainPage_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz opuścić trening i wrócić do strony głównej?", "Uwaga", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            setPage = new SetPage(5, result);
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz opuścić program?", "Uwaga", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            setPage = new SetPage(6, result);
        }
        #endregion

        #region translation button
        private void TranslationButton_Click(object sender, RoutedEventArgs e)
        {
            if(whichPage % 2 == 1)
            {
                wordTextBlock.Text = englishWords[page - 1];
                wordTextBlock.Foreground = Brushes.Blue;
                whichPage++;
            }
            else
            {
                wordTextBlock.Text = polishWords[page - 1];
                wordTextBlock.Foreground = Brushes.Black;
                whichPage++;
            }
        }
        #endregion

        #region go to word that user selected from GoToWordComboBox
        private void GoToWordButton_Click(object sender, RoutedEventArgs e)
        {
            if(GoToWordComboBox.SelectedItem == null)
            {
                MessageBox.Show("Nie wybrano słowa do przejścia", "Uwaga", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                int index = GoToWordComboBox.SelectedIndex;
                page = index + 1;
                wordTextBlock.Foreground = Brushes.Black;
                whichPage = 1;
                PageContent();
            }
        }
        #endregion

        #endregion
    }
}
