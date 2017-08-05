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

namespace Fiszki
{
    public partial class SetTestMode : Page
    {
        #region private members
        /// <summary>
        /// Number of words that will be tested in test mode.
        /// </summary>
        private int value;

        /// <summary>
        /// An array that contains categories that are selected to be tested in test mode.
        /// </summary>
        private string[] categoryTab;

        /// <summary>
        ///  Object that will randomize 10 words for each selected category that will be tested in test mode.
        /// </summary>
        private QuestionsRandomize qRandom;

        /// <summary>
        /// Object that is used to choose next page to show (in menu buttons).
        /// </summary>
        private SetPage setPage;

        /// <summary>
        /// String that contains the amount of words that will be tested in test mode, its value is shown on the textBlock.
        /// </summary>
        private string convert;
        #endregion

        #region constructor
        public SetTestMode()
        {
            InitializeComponent();
            this.value = 0;
            this.categoryTab = new string[4];
        }
        #endregion

        #region private methods
        #region start test
        /// <summary>
        /// Method that starts a test after clicking StartTest button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartTest_Click(object sender, RoutedEventArgs e)
        {
            if (this.categoryTab==null)
            {
                MessageBox.Show("Nie wybrano żadnej kategorii.", "Uwaga", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                qRandom = new QuestionsRandomize(categoryTab, value);
            }
        }
        #endregion

        #region checkboxes
        /// <summary>
        /// These methods are changing the content of the categoryTab array and the value variable according to checking and unchecking items in checkboxes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FruitsCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            this.categoryTab[0] = "Fruits";
            this.value += 10;
            TextOutput(value);
        }
        /// <summary>
        /// Handles the Unchecked event of the FruitsCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void FruitsCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            this.categoryTab[0] = null;
            this.value -= 10;
            TextOutput(value);
        }
        /// <summary>
        /// Handles the Checked event of the AnimalsCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void AnimalsCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            this.categoryTab[1] = "Animals";
            this.value += 10;
            TextOutput(value);
        }
        /// <summary>
        /// Handles the Unchecked event of the AnimalsCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void AnimalsCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            this.categoryTab[1] = null;
            this.value -= 10;
            TextOutput(value);
        }
        /// <summary>
        /// Handles the Checked event of the AppearanceCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void AppearanceCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            this.categoryTab[2] = "Appearance";
            this.value += 10;
            TextOutput(value);
        }
        /// <summary>
        /// Handles the Unchecked event of the AppearanceCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void AppearanceCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            this.categoryTab[2] = null;
            this.value -= 10;
            TextOutput(value);
        }
        /// <summary>
        /// Handles the Checked event of the ProfessionsCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void ProfessionsCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            this.categoryTab[3] = "Profession";
            this.value += 10;
            TextOutput(value);
        }
        /// <summary>
        /// Handles the Unchecked event of the ProfessionsCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void ProfessionsCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            this.categoryTab[3] = null;
            this.value -= 10;
            TextOutput(value);
        }
        #endregion

        #region change content of textBlock
        /// <summary>
        /// This method shows how many words will be tested.
        /// </summary>
        /// <param name="value"></param>
        private void TextOutput(int value)
        {
            convert= this.value.ToString();
            textBlock.Text = this.convert;
            textBlock.UpdateLayout();
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
            MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz anulować przejście do testu i przejść do trybu treningu?", "Uwaga", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            setPage = new SetPage(1, result);
        }
        /// <summary>
        /// Handles the Click event of the AddNewWord control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void AddNewWord_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz anulować przejście do testu i przejść do dodawania nowego słowa?", "Uwaga", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            setPage = new SetPage(3, result);
        }
        /// <summary>
        /// Handles the Click event of the AddNewCategory control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void AddNewCategory_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz anulować przejście do testu i przejść do dodawania nowej kategorii?", "Uwaga", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            setPage = new SetPage(4, result);
        }
        /// <summary>
        /// Handles the Click event of the BackToMainPage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void BackToMainPage_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz anulować przejście do testu i przejść do strony głównej?", "Uwaga", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
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
        #endregion
    }
}
