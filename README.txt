

### ZAIMPLEMENTOWANE PRZYK�ADY:
	- VisualizationTest.DrawRect.cs
	- PerlinNoise.PerlinNoise.cs

### INSTRUKCJA:

1. Klasa problemu musi dziedzizczy� po klasie abstrakcyjnej "ProblemVisualization" znajduj�cej si� w "Core.Visualization"
2. W klasie "ProblemVisualization" jest obiekt "Settings", w kt�rym nale�y umieszcza� parametry wizualizacji, kt�re b�dzie
	mo�na na bie��co zmienia�. 
	* Aby doda� parametr: Settings.Add("string"); (wszystkie parametry przechowywane jako string)
	* Aby wczyta� parametr: Setting.[GetIntValue()][GetFloatValue()][GetDoubleValue()][GetStringValue()];
3. Aby problem by� widoczny na li�cie w aplikacji, nale�y doda� referencj� do niego w VisualizationWPFApp, a nast�pnie 
	w VisualizationWPFApp.Model.cs, w metodzie "LoadProblemList" nale�y doda� zgodnie ze schematem.
4. Aby problem by� widoczny pod konkretn� nazw� nale�y przypisa� j� do w�asno�ci "Name"
5. Najlepiej, aby konstruktor g�ownej klasy by� bezargumentowy i definiowa� parametry w "Settings" wraz z warto�ciami domyslnymi.
6. W�a�ciwo�� "Visualization" - nale�y w niej zdefiniowa� zwracany przez klas� obraz bitmapy, kt�ry chcemy wy�wietla� w aplikacji.
	* Mo�na wy�wietla� tak�e animacje. Przycisk "Play" w aplikacji spowoduje wywo�ywanie w�a�ciwo�ci "Visualization" co kr�tki odst�p
		czasu, w zwi�zku z czym mo�na utworzy� zmienn� statyczn� "licznik", inkrementowa� go z ka�dym wywo�aniem get tej w�a�ciwo�ci
		i generowa� bitmapy w zale�no�ci od tego licznika u�ywaj�c np. modulo.
	* Ustawienie w "DrawRect" koloru na "Dynamic" i naci�ni�cie Play, powoduje animacj� ze zmian� koloru.