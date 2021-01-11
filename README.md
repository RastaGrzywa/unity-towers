# unity-towers


- Po uruchomieniu gry na scenie powinna być jedna wieżyczka,
- Wieżyczka obraca się co 0.5 sekundy o random (15,45) stopni;
- Po obrocie wieżyczka wystrzeliwuje pocisk, który leci random (1,4) jednostek;
- Pocisk na końcu swej drogi jest niszczony i w jego miejscu pojawia się nowa;
wieżyczka (nie obrócona), która czeka 6 sekund, po czym przyjmuje zachowanie
pierwszej wieżyczki;
- Prędkość pocisku powinna wynosić 4 jednostki/sekundę
- Jeśli pocisk uderzy w wieżyczkę to zarówno pocisk jak i wieżyczka zostają zniszczone
(pocisk nie tworzy wtedy nowej wieżyczki)
- Każda wieżyczka strzela 12 razy, po czym przestaje się obracać i strzelać
- Obiekt aktywnej wieżyczki (która obraca się i strzela) ma kolor czerwony RGB (255,0,0)
- Obiekt nieaktywnej wieżyczki (która czeka) ma kolor naturalny RGB (255,255,255)
- Jeśli istnieje już 100 wieżyczek to wszystkie wieżyczki zaczynają znowu strzelać i obracać się 12 razy, ale nowe wieżyczki nie są już tworzone.
- Na scenie powinien być napis "Wieżyczki: x", gdzie x jest ilością aktualnie istniejących wieżyczek.
