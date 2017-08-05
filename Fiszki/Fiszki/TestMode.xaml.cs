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
using System.Timers;
using System.Windows.Threading;

namespace Fiszki
{
    public partial class TestMode : Page
    {
        #region private members
        /// <summary>
        /// An array with polish words used in test.
        /// </summary>
        private string[] tabPolish;

        /// <summary>
        /// An array with translation of words from tabPolish.
        /// </summary>
        private string[] tabEnglish;

        /// <summary>
        /// Total amount of question in test.
        /// </summary>
        private int value;

        /// <summary>
        /// Number of question that is currently shown. 
        /// </summary>
        private int number;

        /// <summary>
        /// Amount of correct answers.
        /// </summary>
        private int plusTestPoints;

        /// <summary>
        /// Amount of incorrect answers.
        /// </summary>
        private int minusTestPoints;

        /// <summary>
        /// Amount of unanswered questions.
        /// </summary>
        private int noTestAnswers;

        /// <summary>
        /// String that contains number of question that is beiing answered.
        /// </summary>
        private string convertNumber;

        /// <summary>
        /// String that contains total amount of questions in test.
        /// </summary>
        private string convertValue;

        /// <summary>
        /// Property used by progress bar.
        /// </summary>
        private int ProgressNumber;

        /// <summary>
        /// Amount of seconds that is being shown in timer.
        /// </summary>
        private int timerSec;

        /// <summary>
        /// Amount of minutes that is being shown in timer.
        /// </summary>
        private int timerMin;

        /// <summary>
        /// Amount of seconds of time that test was taking.
        /// </summary>
        private int timerTestSec;

        /// <summary>
        /// Amount of minutes of time that test was taking.
        /// </summary>
        private int timerTestMin;

        /// <summary>
        /// String that contains the minutes and seconds of time that test was taking.
        /// </summary>
        private string TimeTest;

        /// <summary>
        /// Variable that checks if CheckButton was pressed.
        /// </summary>
        private bool IsChecked = false;

        /// <summary>
        /// Object that is used to choose next page to show (in menu buttons region).
        /// </summary>
        private SetPage setPage;

        /// <summary>
        /// Delegate that is used to store all methods needed to start test correctly.
        /// </summary>
        private delegate void Initialize();
        private Initialize initialize;

        #endregion

        #region constructor
        public TestMode(int _value, string[] _tabPolish, string[] _tabEnglish)
        {
            InitializeComponent();
            Application.Current.MainWindow.Width = 900;
            this.tabEnglish = _tabEnglish;
            this.tabPolish = _tabPolish;
            this.value = _value;
            this.number = 0;
            this.plusTestPoints = 0;
            this.minusTestPoints = 0;
            this.noTestAnswers = 0;
            this.ProgressNumber = 1;
            this.timerMin = (this.value / 2);
            this.timerSec = 1;
            InitializeTest();
            initialize();
        }
        #endregion

        #region private methods
        #region initialize test
        /// <summary>
        /// Initializes the test.
        /// </summary>
        private void InitializeTest()
        {
            initialize += (() => { QuestionValue(0); } );
            initialize += (() => { ValueOutput(value); });
            initialize += (() => { ShowpbStatus(0); });
            initialize += DispatcherTimer;
        }
        #endregion

        #region show index of question, that user is currently answering
        /// <summary>
        /// This method shows which question is being currently shown to user.
        /// </summary>
        /// <param name="value"></param>
        private void QuestionValue(int value)
        {
            number += 1;
            if (this.number <= this.value)
            {
                convertNumber = this.number.ToString();
                textBlock.Text = this.convertNumber;
                PolishWord.Text = tabPolish[number-1];
            }
        }
        #endregion

        #region show total amount of questions
        /// <summary>
        /// This method shows user total amount of questions that he should answer to complete test.
        /// </summary>
        /// <param name="value"></param>
        private void ValueOutput(int value)
        {
            convertValue = this.value.ToString();
            textBlock1.Text = this.convertValue;
            textBlock1.UpdateLayout();
        }
        #endregion

        #region progress bar
        /// <summary>
        /// Updates a progress bar 
        /// </summary>
        /// <param name="value"></param>
        private void ShowpbStatus(int value)
        {
            ProgressBar.Maximum = this.value;
            if(this.ProgressNumber <= this.value) ProgressBar.Value += 1;
        }
        #endregion

        #region end test button
        /// <summary>
        /// Ends test after clicking End_Test button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void End_Test_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz zakończyć? Wynik testu zostanie ustalony na podstawie udzielonych odpowiedzi.","Uwaga", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                if (this.plusTestPoints==0 && this.minusTestPoints==0) noTestAnswers = value;
                else
                noTestAnswers = noTestAnswers +  value - number + 1;
                TestResult testResult = new TestResult(TimeTest, value, plusTestPoints, minusTestPoints, noTestAnswers);
                Application.Current.MainWindow.Content = testResult;
            }
        }
        #endregion

        #region next question button
        /// <summary>
        /// Shows next question of test after clicking NextQuestion button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NextQuestion_Click(object sender, RoutedEventArgs e)
        {
            if (EnglishWord.Text == "" || IsChecked == false) noTestAnswers += 1;
            IsChecked = false;
            EnglishWord.IsReadOnly = false;
            Result.Text = null;
            EnglishWord.Text = null;
            QuestionValue(value);
            ValueOutput(value);
            ShowpbStatus(value);
            CheckButton.IsEnabled = true;

            if (this.number==this.value)
            {
                NextQuestion.Content = "Wynik";
                NextQuestion.UpdateLayout();
            }

            if(this.number>this.value)
            {
                TestResult testResult = new TestResult(TimeTest, value, plusTestPoints, minusTestPoints, noTestAnswers);
                Application.Current.MainWindow.Content = testResult;
            }

        }
        #endregion

        #region timer
        /// <summary>
        /// Set timer's properties needed for the correct work of timer. 
        /// </summary>
        public void DispatcherTimer()
        {
            DispatcherTimer DispatcherTimer = new DispatcherTimer();
            DispatcherTimer.Start();
            DispatcherTimer.Tick += DispatcherTimer_Tick;
            DispatcherTimer.Interval = new TimeSpan(0, 0, 1);
        }
        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (this.timerSec > 0) this.timerSec--;
            else
            {
                this.timerMin -= 1;
                this.timerSec = 59;
            }
            if(this.timerSec<10 && this.timerMin>=10)
                Timer.Content = this.timerMin + ":" + "0"+ this.timerSec;
            else if(this.timerSec >= 10 && this.timerMin < 10)
                Timer.Content ="0" + this.timerMin + ":" + this.timerSec;
            else if (this.timerSec < 10 && this.timerMin < 10)
                Timer.Content = "0" + this.timerMin + ":" + "0" + this.timerSec;
            else
                Timer.Content = this.timerMin + ":" + this.timerSec;


            if (this.timerTestSec < 60) this.timerTestSec++;
            else
            {
                this.timerTestMin += 1;
                this.timerTestSec = 0;
            }

            if (this.timerTestSec < 10 && this.timerTestMin >= 10)
                TimeTest = this.timerTestMin + ":" + "0" + this.timerTestSec;
            else if (this.timerTestSec >= 10 && this.timerTestMin < 10)
                TimeTest = "0" + this.timerTestMin + ":" + this.timerTestSec;
            else if (this.timerTestSec < 10 && this.timerTestMin < 10)
                TimeTest = "0" + this.timerTestMin + ":" + "0" + this.timerTestSec;
            else
                TimeTest = this.timerTestMin + ":" + this.timerTestSec;


            if (this.timerSec == 0 && this.timerMin == 0)
            {
                MessageBox.Show("Czas testu upłynął", "Uwaga", MessageBoxButton.OK, MessageBoxImage.Information);
                if (this.plusTestPoints == 0 && this.minusTestPoints == 0) noTestAnswers = value;
                else
                noTestAnswers = noTestAnswers + value - number;
                TestResult testResult = new TestResult(TimeTest, value, plusTestPoints, minusTestPoints, noTestAnswers);
                Application.Current.MainWindow.Content = testResult;
            }
        }
        #endregion

        #region check, if user wrote a translation correctly after clicking a CheckButton
        /// <summary>
        /// Lets user know if he answered a question correctly.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            if(EnglishWord.Text == "")
            {
                MessageBox.Show("Wprowadź najpierw tłumaczenie słowa!", "Uwaga", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
            else
            {
                IsChecked = true;
                if (EnglishWord.Text == tabEnglish[number - 1])
                {
                    plusTestPoints += 1;
                    Result.Text = "Poprawnie!";
                    Result.Foreground = Brushes.Green;
                    EnglishWord.IsReadOnly = true;
                }

                else
                {
                    minusTestPoints += 1;
                    Result.Text = "Źle!";
                    Result.Foreground = Brushes.Red;
                    EnglishWord.IsReadOnly = true;
                }
                CheckButton.IsEnabled = false;
            }
            
        }
        #endregion

        #region menu buttons
        /// <summary>
        /// Handles the Click event of the SetTrainingMode control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void SetTrainingMode_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz opuścić test i przejść do trybu treningu? Postęp testu nie zostanie zapisany.", "Uwaga", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            setPage = new SetPage(1, result);
        }
        /// <summary>
        /// Handles the Click event of the AddNewWord control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void AddNewWord_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz opuścić test i przejść do dodawania nowego słowa? Postęp testu nie zostanie zapisany.", "Uwaga", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            setPage = new SetPage(3, result);
        }
        /// <summary>
        /// Handles the Click event of the AddNewCategory control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void AddNewCategory_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz opuścić test i przejść do dodawania nowej kategorii? Postęp testu nie zostanie zapisany.", "Uwaga", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            setPage = new SetPage(4, result);
        }
        /// <summary>
        /// Handles the Click event of the BackToMainPage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void BackToMainPage_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz opuścić test i przejść do strony głównej? Postęp testu nie zostanie zapisany.", "Uwaga", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
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
