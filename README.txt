

### Zobacz Core -> ExampleProblemVisualization ###

1. Klasa musi dziedzizczy� po klasie abstrakcyjnej "ProblemVisualization" znajduj�cej si� w CORE
2. W klasie "ProblemVisualization" jest obiekt "Settings", w kt�rym nale�y umieszcza� parametry wizualizacji, kt�re b�dzie
	mo�na na bie��co zmienia�. 
3. Aby problem by� widoczny na li�cie w aplikacji, nale�y doda� referencj� do niego w VisualizationWPFApp, a nast�pnie 
	w Model.cs, w metodzie "LoadProblemList" nale�y doda� zgodnie ze schematem tworzenie obiektu problemu.
4. Aby problem by� widoczny pod konkretn� nazw� nale�y przypisa� j� do w�asno�ci "Name"
5. Najlepiej, aby konstruktor g�ownej klasy by� bezargumentowy i definiowa� parametry w "Settings" wraz z warto�ciami domyslnymi.

### Szum Perlina jest ju� zaimplementowany, wi�c te� mo�na potraktowa� go jako przyk�ad ###