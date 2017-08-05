using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Fiszki
{
    public class Category
    {
        #region constructors
        #region constructor for default categories
        public Category(int x)
            {
                switch (x)
                {
                    case 1:
                        this.PolishName = "Zwierzęta";
                        this.EnglishName = "Animals";
                        break;
                    case 2:
                        this.PolishName = "Wygląd";
                        this.EnglishName = "Appearance";
                        break;
                    case 3:
                        this.PolishName = "Owoce";
                        this.EnglishName = "Fruits";
                        break;
                    case 4:
                        this.PolishName = "Zawody";
                        this.EnglishName = "Profession";
                        break;
                    default:
                        MessageBox.Show("Błąd- użyto nieistniejącego ID kategorii");
                        break;
                }
            }
        #endregion

        #region constructor for user's categories
        public Category(string _polishName, string _englishName)
            {
                this.PolishName = _polishName;
                this.EnglishName = _englishName;
            }
        #endregion
        #endregion
        
        #region public members
        /// <summary>
        /// Member that contains polish name of category that is being used.
        /// </summary>
        public string PolishName { get; set; }

        /// <summary>
        /// Member that contains english name of category that is being used.
        /// </summary>
        public string EnglishName { get; set; }
        #endregion
    }
}
