

### Zobacz Core -> ExampleProblemVisualization ###

1. Klasa musi dziedzizczyæ po klasie abstrakcyjnej "ProblemVisualization" znajduj¹cej siê w CORE
2. W klasie "ProblemVisualization" jest obiekt "Settings", w którym nale¿y umieszczaæ parametry wizualizacji, które bêdzie
	mo¿na na bie¿¹co zmieniaæ. 
3. Aby problem by³ widoczny na liœcie w aplikacji, nale¿y dodaæ referencjê do niego w VisualizationWPFApp, a nastêpnie 
	w Model.cs, w metodzie "LoadProblemList" nale¿y dodaæ zgodnie ze schematem tworzenie obiektu problemu.
4. Aby problem by³ widoczny pod konkretn¹ nazw¹ nale¿y przypisaæ j¹ do w³asnoœci "Name"
5. Najlepiej, aby konstruktor g³ownej klasy by³ bezargumentowy i definiowa³ parametry w "Settings" wraz z wartoœciami domyslnymi.

### Szum Perlina jest ju¿ zaimplementowany, wiêc te¿ mo¿na potraktowaæ go jako przyk³ad ###