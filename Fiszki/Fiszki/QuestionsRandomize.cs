using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;

namespace Fiszki
{
    public class QuestionsRandomize
    {
        #region private members
        /// <summary>
        /// An array that contains random polish words from Fruits category, if this category was checked in SetTestMode page.
        /// </summary>
        private string[] tabFruitsPolish;

        /// <summary>
        /// An array that contains english translatons of words that are part of tabFruitsPolish array.
        /// </summary>
        private string[] tabFruitsEnglish;

        /// <summary>
        /// An array that contains random polish words from Animals category, if this category was checked in SetTestMode page.
        /// </summary>
        private string[] tabAnimalsPolish;

        /// <summary>
        /// An array that contains english translatons of words that are part of tabAnimalsPolish array.
        /// </summary>
        private string[] tabAnimalsEnglish;

        /// <summary>
        /// An array that contains random polish words from Appearance category, if this category was checked in SetTestMode page.
        /// </summary>
        private string[] tabAppearancePolish;

        /// <summary>
        /// An array that contains english translatons of words that are part of tabAppearancePolish array.
        /// </summary>
        private string[] tabAppearanceEnglish;

        /// <summary>
        /// An array that contains random polish words from Professions category, if this category was checked in SetTestMode page.
        /// </summary>
        private string[] tabProfessionsPolish;

        /// <summary>
        /// An array that contains english translatons of words that are part of tabProfessionsPolish array.
        /// </summary>
        private string[] tabProfessionsEnglish;

        /// <summary>
        /// An array that contains all the polish words that will be tested.
        /// </summary>
        private string[] polishWords;

        /// <summary>
        /// An array that contains all the translations of words from polishWords array.
        /// </summary>
        private string[] englishWords;

        /// <summary>
        /// Variable that contains number of selected category's words. Its value is being changed in LoadWords method.
        /// </summary>
        private int numberOfWords;

        /// <summary>
        /// Number of words that will be tested in the test mode.
        /// </summary>
        private int value;
        #endregion

        #region constructor
        public QuestionsRandomize(string[] categoryTab, int _value)
        {
            this.value = _value;

            #region select 10 random words if category was checked in SetTestMode page
            if(categoryTab[0] != null)
            {
                tabFruitsPolish = new string[10];
                tabFruitsEnglish = new string[10];
                RandomWords(0, tabFruitsPolish, tabFruitsEnglish, categoryTab);
            }

            if (categoryTab[1] != null)
            {
                tabAnimalsPolish = new string[10];
                tabAnimalsEnglish = new string[10];
                RandomWords(1, tabAnimalsPolish, tabAnimalsEnglish, categoryTab);
            }

            if (categoryTab[2] != null)
            {
                tabAppearancePolish = new string[10];
                tabAppearanceEnglish = new string[10];
                RandomWords(2, tabAppearancePolish, tabAppearanceEnglish, categoryTab);
            }

            if (categoryTab[3] != null)
            {
                tabProfessionsPolish = new string[10];
                tabProfessionsEnglish = new string[10];
                RandomWords(3, tabProfessionsPolish, tabProfessionsEnglish, categoryTab);
            }
            #endregion

            #region if some category wasn't checked in SetTestMode, initialize array as an empty array
            if (categoryTab[0] == null)
            {
                tabFruitsPolish = new string[0];
                tabFruitsEnglish = new string[0];
            }
            if (categoryTab[1] == null)
            {
                tabAnimalsPolish = new string[0];
                tabAnimalsEnglish = new string[0];
            }
            if (categoryTab[2] == null)
            {
                tabAppearancePolish = new string[0];
                tabAppearanceEnglish = new string[0];
            }
            if (categoryTab[3] == null)
            {
                tabProfessionsPolish = new string[0];
                tabProfessionsEnglish = new string[0];
            }
            #endregion

            string[] tabPolish = CombineTab(tabAnimalsPolish, tabFruitsPolish, tabAppearancePolish, tabProfessionsPolish);
            string[] tabEnglish = CombineTab(tabAnimalsEnglish, tabFruitsEnglish, tabAppearanceEnglish, tabProfessionsEnglish);
            
            TestMode testMode = new TestMode(value, tabPolish, tabEnglish);
            Application.Current.MainWindow.Content = testMode;
        }
        #endregion

        #region private methods
        #region complete arrays with randomized words
        private void RandomWords(int id, string[] tabPolish, string[] tabEnglish, string[] categoryTab)
        {
            LoadWords(id, categoryTab);

            Random random = new Random();
            for (int i = 0; i < 10; i++)
            {
                int randomNumber = random.Next(1, numberOfWords);
                string polishWord = polishWords[randomNumber];
                string englishWord = englishWords[randomNumber];
                if (!tabPolish.Contains(polishWord))
                {
                    tabPolish[i] = polishWord;
                    tabEnglish[i] = englishWord;
                }
                else
                    i--;
            }
        }
    #endregion

        #region load all the words of category that was selected to arrays
        private void LoadWords(int index, string[] categoryTab)
        {
            string name = categoryTab[index].ToLower();
            string fileName = "..\\..\\" + name + ".txt";
            Encoding enc = Encoding.Default;
            FileStream file = new FileStream(@fileName, FileMode.Open, FileAccess.Read);


            try
            {
                using (StreamReader stream = new StreamReader(file, enc))
                {
                    string[] buffer = File.ReadAllLines(@fileName, enc);
                    numberOfWords = buffer.Length;
                    this.polishWords = new string[numberOfWords];
                    this.englishWords = new string[numberOfWords];

                    int polishIndex = 0;
                    int englishIndex = 0;
                    int foo = 0;

                    for (int pom = 0; pom < numberOfWords; pom++)
                    {
                        foreach (string word in buffer[pom].Split(' '))
                        {
                            foo++;
                            if (polishIndex == englishIndex && foo % 3 == 1)
                            {
                                byte[] wordBytes = Encoding.Default.GetBytes(word);
                                string newWordString = Encoding.Default.GetString(wordBytes);
                                this.polishWords[polishIndex] = newWordString;
                                polishIndex++;
                            }
                            else if (polishIndex != englishIndex && foo % 3 == 2)
                            {
                                this.englishWords[englishIndex] = word;
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
        }
    #endregion

        #region combine all word arrays into one 
        private string[] CombineTab(string[] tabAnimals, string[] tabFruits, string[] tabAppearance, string[] tabProfessions)
        {
            string[] _tab = new string[tabAnimals.Length + tabFruits.Length + tabAppearance.Length + tabProfessions.Length];

            tabFruits.CopyTo(_tab, 0);
            tabAnimals.CopyTo(_tab, tabFruits.Length);
            tabAppearance.CopyTo(_tab, tabAnimals.Length + tabFruits.Length);
            tabProfessions.CopyTo(_tab, tabAnimals.Length + tabFruits.Length + tabAppearance.Length);

            return _tab;
        }
        #endregion
        #endregion
    }
}
