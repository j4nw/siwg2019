

### ZAIMPLEMENTOWANE PRZYK£ADY:
	- VisualizationTest.DrawRect.cs
	- PerlinNoise.PerlinNoise.cs

### INSTRUKCJA:

1. Klasa problemu musi dziedzizczyæ po klasie abstrakcyjnej "ProblemVisualization" znajduj¹cej siê w "Core.Visualization"
2. W klasie "ProblemVisualization" jest obiekt "Settings", w którym nale¿y umieszczaæ parametry wizualizacji, które bêdzie
	mo¿na na bie¿¹co zmieniaæ. 
	* Aby dodaæ parametr: Settings.Add("string"); (wszystkie parametry przechowywane jako string)
	* Aby wczytaæ parametr: Setting.[GetIntValue()][GetFloatValue()][GetDoubleValue()][GetStringValue()];
3. Aby problem by³ widoczny na liœcie w aplikacji, nale¿y dodaæ referencjê do niego w VisualizationWPFApp, a nastêpnie 
	w VisualizationWPFApp.Model.cs, w metodzie "LoadProblemList" nale¿y dodaæ zgodnie ze schematem.
4. Aby problem by³ widoczny pod konkretn¹ nazw¹ nale¿y przypisaæ j¹ do w³asnoœci "Name"
5. Najlepiej, aby konstruktor g³ownej klasy by³ bezargumentowy i definiowa³ parametry w "Settings" wraz z wartoœciami domyslnymi.
6. W³aœciwoœæ "Visualization" - nale¿y w niej zdefiniowaæ zwracany przez klasê obraz bitmapy, który chcemy wyœwietlaæ w aplikacji.