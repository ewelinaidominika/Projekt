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
    /// <summary>
    /// Logika interakcji dla klasy TestResult.xaml
    /// </summary>
    public partial class TestResult : Page
    {
        #region private members
        /// <summary>
        /// String that contains minutes and seconds of time that test took.
        /// </summary>
        private string TimeTest;

        /// <summary>
        /// Amount of questions in test.
        /// </summary>
        private int value;

        /// <summary>
        /// Amount of points that user got by completing test.
        /// </summary>
        private int points;

        /// <summary>
        /// Amount of correctly answered questions.
        /// </summary>
        private int plusTestPoints;

        /// <summary>
        /// Amount of incorrectly answered questions.
        /// </summary>
        private int minusTestPoints;

        /// <summary>
        /// Amount of unanswered questions.
        /// </summary>
        private int noTestAnswers;

        /// <summary>
        /// String that contains amount of questions in test.
        /// </summary>
        private string convertValue;

        /// <summary>
        /// String that contains amount of correctly answered questions.
        /// </summary>
        private string convertPlusPoints;

        /// <summary>
        /// String that contains amount of incorrectly answered questions.
        /// </summary>
        private string convertMinusPoints;

        /// <summary>
        /// String that contains amount of unanswered questions.
        /// </summary>
        private string convertNoAnswers;

        /// <summary>
        /// Object that is used to choose next page to show (in menu buttons region).
        /// </summary>
        private SetPage setPage;
        #endregion

        #region constructor
        public TestResult(string _TimeTest, int _value, int _plusTestPoints, int _minusTestPoints, int _noTestAnswers)
        {
            InitializeComponent();
            this.TimeTest = _TimeTest;
            this.value = _value;
            this.plusTestPoints = _plusTestPoints;
            this.minusTestPoints = _minusTestPoints;
            this.noTestAnswers = _noTestAnswers;
            TimeOut(TimeTest);
            ResultOfTest(value);
            Values(value);
        }
        #endregion

        #region private methods
        #region shows time of test
        private void TimeOut(string TimeTest)
        {
            Time.Text = this.TimeTest;
            Time.UpdateLayout();
        }
        #endregion

        #region show result of test
        private void ResultOfTest(int value)
        {
            this.points = this.plusTestPoints - this.minusTestPoints;
            if(this.points < this.value/2)
            {
                ResultValue.Text = "NEGATYWNY";
                ResultValue.Foreground = Brushes.Red;
                ResultValue.UpdateLayout();
            }
            else
            {
                ResultValue.Text = "POZYTYWNY";
                ResultValue.Foreground = Brushes.Green;
                ResultValue.UpdateLayout();
            }
        }
        #endregion

        #region show amounts of correctly, incorrectly answered questions and unanswered questions.
        private void Values(int value)
        {
            convertValue = this.value.ToString();
            noAnswerPoints.Text = plusPoints.Text = minusPoints.Text = convertValue;

            convertPlusPoints = this.plusTestPoints.ToString();
            plusTestP.Text = convertPlusPoints;

            convertMinusPoints = this.minusTestPoints.ToString();
            minusTestP.Text = convertMinusPoints;

            convertNoAnswers = this.noTestAnswers.ToString();
            noTestP.Text = convertNoAnswers;
        }
        #endregion

        #region menu buttons
        private void SetTrainingMode_Click(object sender, RoutedEventArgs e)
        {
            setPage = new SetPage(1, MessageBoxResult.Yes);
        }
        private void AddNewWord_Click(object sender, RoutedEventArgs e)
        {
            setPage = new SetPage(3, MessageBoxResult.Yes);
        }
        private void AddNewCategory_Click(object sender, RoutedEventArgs e)
        {
            setPage = new SetPage(4, MessageBoxResult.Yes);
        }
        private void BackToMainPage_Click(object sender, RoutedEventArgs e)
        {
            setPage = new SetPage(5, MessageBoxResult.Yes);
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz opuścić program?", "Uwaga", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            setPage = new SetPage(6, result);
        }
        #endregion
        #endregion
    }
}
