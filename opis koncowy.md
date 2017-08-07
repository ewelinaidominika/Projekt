https://www.youtube.com/watch?v=dQDvPfDLCLc 
(Przepraszamy za znak wodny na filmie, dopiero w końcowej fazie obróbki filmu się zorientowałyśmy że zostanie on dodany, a nie miałyśmy
czasu żeby nagrać go jeszcze raz.)
Uwaga odnośnie wyglądu kodu na filmie - film był nagrywany na 2 tygodnie przed wysłaniem projektu na GitHub'a, w związku z tym kod 
wygląda w tej chwili inaczej. Podzieliłyśmy go na regiony, a prawie wszystkie pola i metody klas posiadają dołączoną dokumentację w postaci summary. Dokumentacja ta częściowo była pisana przez nas ręcznie, a częściowo wygenerowana przez rozszerzenie GhostDoc.

Temat projektu- Nauka języka angielskiego poprzez fiszki.

Porównanie wykonania projektu z założeniami zawartymi w opisie wstępnym:
W większości założenia pokrywają się z wykonanym projektem, jednak nastąpiło kilka zmian oraz dodałyśmy kilka funkcjonalności, o których 
nie było mowy w opisie wstępnym.
Zmiany:
 - w trybie treningu nie są pokazywane na raz obie strony fiszki, tylko jedna strona i obrazek przypisany do słowa. Druga strona pokazuje
 się po naciśnięciu przycisku Tłumaczenie.
 - w trybie testu użytkownik nie może samodzielnie wybrać ilości słówek do przetestowania, została ona narzucona - 10 słówek na 1 kategorię.
Dodatkowe funkcjonalności
 - do słów dodane są obrazki
 - można tworzyć własne kategorie(ale wykorzystywać je można tylko w trybie treningu)
 - można dodawać nowe słowa do każdej kategorii, również do tych stworzonych samodzielnie(nowe słowa są wykorzystywane i w trybie
 treningu, i testu).
 
Kod został napisany w technologii WPF. Interfejs użytkownika został przez nas napisany w języku polskim - jest to uzasadnione, ponieważ program w założeniu ma uczyć osoby nieznające języka angielskiego (dla takich osób korzystanie z anglojęzycznego programu byłoby utrudnieniem). Program posiada jedno główne okno - MainWindow, którego zawartość jest zmieniana w zależności od wyborów użytkownika. Zawartość okienka zawarta jest w stronach (m.in. MainPage, AddNewCategory, SetTrainingMode). Większość funkcjonalności została zaimplementowana w metodach odpowiadających zdarzeniom takim jak naciśnięcie kontrolek, żeby wszystkie działania programu były uzależnione od wyborów użytkownika. Poza tym, między niektórymi stronami są przekazywane dane, tak jak np. między stroną SetTrainingMode oraz TrainingMode(w konstruktorze klasy TrainingMode przekazujemy objekt zawierający nazwę polską i angielską kategorii po to, żeby program "wiedział" jaka kategoria jest teraz trenowana). Z takich bardziej dokładnych funkcjonalności jakie musiałyśmy wykorzystać możemy wymienić jeszcze operowanie na plikach tekstowych - zarówno odczyt, jak i zapis - była to jedna 
z ważniejszych funkcjonalności, gdyż bazy słówek przechowywane są właśnie w plikach tekstowych. O tym, które z Waszych wskazówek co do
kodu wykorzystałyśmy, piszemy trochę niżej w tym pliku.
 
Na GitHub'ie znajduje się projekt, w którym została już stworzona własna kategoria za pomocą programu - Przedmioty, do niej są dodane
słowa z obrazkiem domyślnym oraz do kategorii Zwierzęta zostało dodane słówko wiewiórka za pomocą programu wraz z obrazkiem wybranym 
samodzielnie przy dodawaniu słowa.

Wskazówki dotyczące uruchamiania projektu - prosimy, aby po pobraniu projektu skompilować go w visualu, a następnie przejść do eksploratora plików, tam odnaleźć miejsce, do którego został pobrany projekt i przejść do tej ścieżki: \Fiszki\Fiszki\bin\Debug\, w którym znajduje się plik Fiszki.exe. Prosimy o to z tego względu, że przy korzystaniu z dodawania własnych kategorii/słów naniesione zostają zmiany w plikachprojektu, co może nie być od razu widoczne przy uruchamianiu programu z poziomu Visual Studio. Poza tym, ścieżki do zdjęć odpowiadających słowom są podane w postaci względnej względem tego właśnie folderu.

Wykorzystane przez nas Wasze wskazówki dotyczące kodu:
 - polimorfizm statyczny - przeciążanie funkcji było nam przydatne do stworzenia dwóch konstruktorów w klasie Category, która pomaga 
w trybie treningu przechowywać polską i angielską nazwę kategorii, którą użytkownik trenuje. Jeden konstruktor obsługuje kategorie 
domyślne, a drugi stworzone przez użytkownika.
 - hermetyzacja - prawie wszystkie klasy posiadają pola oznaczone modyfikatorem dostępu private (jedynym wyjątkiem jest klasa Category,
ponieważ polska nazwa kategorii potrzebna jest w klasie TrainingMode, aby na końcu treningu w MessageBoxie wyświetlić nazwę kategorii, 
której trening się ukończyło). Sprawia to, że te pola klas nie są dostępne z poziomu innych klas oraz ich wartość nie może być
niekontrolowanie zmieniana.
 - delegaty - wykorzystujemy je w dwóch miejscach w kodzie. Pierwsze - w klasie AddNewWord, aby wywołać jedną z metod AddDefaultImage 
lub AddUsersImage w zależności od podanych przez użytkownika informacji, aby dodać do nowego słowa albo obrazek domyślny,
albo ten wybrany przez użytkownika. Drugie - w klasie TestMode, jednak tutaj stosujemy delegatę złożoną, do której dołączamy cztery
metody konieczne do inicjalizacji testu. Dodawanie tych metod jest u nas powiązane z wyrażeniami lambda,
 - wyrażenia lambda - jak wyżej wspomniałyśmy, do delegaty w klasie TestMode dodajemy cztery metody. Jednak jako, że delegata jest typu
void oraz nie przyjmuje parametrów, a trzy z potrzebnych metod potrzebują parametry do poprawnego działania, były nam przydatne wyrażenia
lambda. Ich użycie znajduje się w metodzie InitializeTest.
 - podział kodu. Rozumiemy to w ten sposób, że kod w każdej z klas został u nas podzielony na 3 główne regiony - constructor, private
members i private methods, a region private methods został podzielony na kolejne regiony w zależności od funkcjonalności, jakie te metody
wykonują.

Poniżej opiszemy funkcjonalności, na które nie wystarczyło nam czasu w filmie.
 
Kilka słów jak działa dodawanie własnych kategorii:
 - w formularzu użytkownik podaje nazwę polską oraz angielską nowej kategorii,
 - plik myOwnCategories.txt przechowuje podane nazwy kategorii,
 - generuje się plik tekstowy o nazwie takiej, jaka jest angielska nazwa kategorii(z małej litery),
 - jako, że nowo stworzony plik tekstowy musi być dodany do całego projektu, w tym celu stosujemy odwołanie do Microsoft.Build.dll,
 - tworzony jest folder na zdjęcia o nazwie takiej, jak angielska nazwa kategorii(z wielkiej litery),
 - Ważne - ze względu na napisany przez nas kod ważnym jest, aby nazwy kategorii podawać z wielkiej litery.
 
Dodawanie własnych słów:
 - użytkownik wybiera do której kategorii ma być zapisane słowo, podaje polskie i angielskie słowo i może wybrać jaki obrazek ma się przy 
nim pokazywać w trybie treningu,
 - dodanie obrazka nie jest konieczne, w takim przypadku zostanie dodane zdjęcie domyślne,
 - program otwiera plik tekstowy odpowiadający wybranej kategorii i na końcu pliku dodaje słowa wraz z ścieżką względną,
 - program kopiuje obrazek do folderu odpowiadającego kategorii, a następnie dzięki Microsoft.Build.dll jest w stanie dodać obrazek 
 do plików projektu.
 
Budowa pliku MyOwnCategories.txt, który przechowuje nazwy kategorii własnych użytkownika- W każdej linii znajdują się nazwy dla jednej
kategorii, najpierw nazwa polska, później angielska. Budowę plików przechowujących słówka dla danej kategorii pokazałyśmy na filmie.

Na koniec drobna uwaga na temat obrazków wykorzystanych w projekcie - wszystkie zostały pobrane z serwisów udostępniających darmowe obrazki, takich jak pixabay.com.

Dziękujemy za przeczytanie tego opisu (i przepraszamy jeśli wyszedł nam trochę zbyt długi). Mamy nadzieję, że opis ten wraz z filmem
wystarczająco wyjaśniły Wam działanie naszego programu, jednak jeśli macie jakiekolwiek wątpliwości, chętnie odpowiemy na pytania. 
Dominika Kankowska i Ewelina Kamyszek.
