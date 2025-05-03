# Dokument wymagań produktu (Product Requirement Document - PRD) - NBATyPlay

## 1. Przegląd produktu

NBATyPlay to aplikacja webowa umożliwiająca tworzenie prywatnych lig do obstawiania wyników meczów NBA. Aplikacja pozwala administratorom na tworzenie lig z unikalnymi kodami dostępu, dodawanie meczów NBA, a użytkownikom na dołączanie do lig poprzez kod i obstawianie wyników meczów.

System oferuje prosty mechanizm typowania wyników - użytkownicy mogą obstawiać zwycięstwo gospodarza, gościa lub dogrywkę. Za poprawne wytypowanie przyznawany jest 1 punkt. Aplikacja zapewnia automatyczne blokowanie możliwości typowania po rozpoczęciu meczu oraz prowadzi ranking graczy na podstawie zdobytych punktów.

Aplikacja zostanie zaimplementowana przy użyciu technologii .NET 8, Razor Pages, Entity Framework Core 8 oraz MSSQL. Interfejs użytkownika będzie responsywny dzięki zastosowaniu Bootstrap 5, a proces CI/CD zostanie zautomatyzowany przez GitHub Actions.

## 2. Problem użytkownika

Fani koszykówki NBA często organizują między sobą prywatne ligi do obstawiania wyników meczów. Zazwyczaj ten proces jest zarządzany ręcznie (np. arkusze Excel, wiadomości w grupach), co powoduje szereg problemów:

- Trudności z zarządzaniem typami i obliczeniami punktów
- Brak automatycznego zamykania typowania po rozpoczęciu meczu
- Problemy z organizacją i śledzeniem wielu meczów jednocześnie
- Brak centralnego miejsca do sprawdzania aktualnych rankingów
- Czasochłonne i podatne na błędy ręczne wprowadzanie wyników i punktów

NBATyPlay rozwiązuje te problemy poprzez dostarczenie zautomatyzowanej platformy do organizacji prywatnych lig typowania, z przejrzystym systemem obstawiania, automatycznym blokowaniem typowania i obliczaniem punktów oraz aktualizacją rankingów.

## 3. Wymagania funkcjonalne

### 3.1. System rejestracji i logowania
- Rejestracja nowych użytkowników (login + hasło)
- Logowanie istniejących użytkowników
- Hashowanie haseł z użyciem SHA256
- Walidacja unikalności loginów

### 3.2. System ról i uprawnień
- SuperAdministrator - zarządzanie całą aplikacją, resetowanie haseł, usuwanie kont
- Administrator - zarządzanie ligą, dodawanie meczów, wprowadzanie wyników
- Użytkownik - obstawianie wyników, przeglądanie rankingów

### 3.3. Zarządzanie ligami
- Tworzenie nowych lig przez administratorów
- Generowanie unikalnych kodów dostępu dla lig
- Dołączanie do ligi przez użytkowników przy użyciu kodu

### 3.4. Zarządzanie meczami
- Predefiniowana lista drużyn NBA
- Dodawanie meczów przez administratorów (drużyny, data i czas rozpoczęcia)
- Walidacja danych wejściowych
- Automatyczne blokowanie typowania po rozpoczęciu meczu

### 3.5. Obstawianie wyników
- Typowanie zwycięzcy (gospodarz, gość) lub dogrywki
- Punktacja: 1 punkt za poprawne typowanie lub trafienie dogrywki
- Brak punktów za wytypowanie zwycięzcy, jeśli mecz zakończył się dogrywką

### 3.6. Ranking i statystyki
- Ranking graczy w lidze według zdobytych punktów
- Dashboard z podstawowymi statystykami ligi

## 4. Granice produktu

Następujące funkcjonalności NIE będą zawarte w pierwszej wersji aplikacji:

### 4.1. Wyłączone funkcjonalności
- Powiadomienia mailowe (o nowych meczach, wynikach)
- Resetowanie hasła przez email (tylko przez SuperAdministratora)
- Historia zakładów
- Integracja z zewnętrznymi API NBA (automatyczne pobieranie meczów i wyników)
- Obsługa różnych stref czasowych (tylko CET)
- Zaawansowane statystyki użytkowników

### 4.2. Ograniczenia techniczne
- Brak optymalizacji dla dużej liczby użytkowników
- Interfejs responsywny głównie dla urządzeń desktopowych
- Uproszczona obsługa błędów

## 5. Historyjki użytkowników

### 5.1. Rejestracja i logowanie

#### US-001: Rejestracja użytkownika
Jako nowy użytkownik, chcę zarejestrować się w systemie, aby móc korzystać z aplikacji i dołączać do lig.

Kryteria akceptacji:
1. Użytkownik może wprowadzić login i hasło
2. System weryfikuje unikalność loginu
3. System wyświetla komunikat błędu, jeśli login już istnieje
4. Hasło jest hashowane przy użyciu SHA256
5. Po pomyślnej rejestracji, użytkownik jest przekierowany na stronę logowania
6. Formularz zawiera walidację danych (np. minimalna długość hasła)

#### US-002: Logowanie użytkownika
Jako zarejestrowany użytkownik, chcę zalogować się do systemu, aby uzyskać dostęp do funkcjonalności aplikacji.

Kryteria akceptacji:
1. Użytkownik może wprowadzić login i hasło
2. System weryfikuje poprawność danych logowania
3. System wyświetla komunikat błędu przy niepoprawnych danych
4. Po pomyślnym logowaniu, użytkownik jest przekierowany na stronę główną
5. System przechowuje informacje o sesji użytkownika

#### US-003: Resetowanie hasła użytkownika przez SuperAdministratora
Jako SuperAdministrator, chcę mieć możliwość resetowania haseł użytkowników, aby pomóc im odzyskać dostęp do konta.

Kryteria akceptacji:
1. SuperAdministrator widzi listę wszystkich użytkowników
2. SuperAdministrator może wybrać użytkownika i zresetować jego hasło
3. System wymaga potwierdzenia operacji resetowania hasła
4. Po zresetowaniu system generuje nowe hasło tymczasowe
5. SuperAdministrator może przekazać nowe hasło użytkownikowi

#### US-004: Usuwanie konta użytkownika
Jako SuperAdministrator, chcę mieć możliwość usuwania kont użytkowników, aby zarządzać bazą użytkowników.

Kryteria akceptacji:
1. SuperAdministrator widzi listę wszystkich użytkowników
2. SuperAdministrator może wybrać użytkownika i usunąć jego konto
3. System wymaga potwierdzenia operacji usuwania konta
4. Po usunięciu konta, użytkownik nie może logować się do systemu
5. Historyczne dane w ligach (typowania, punkty) pozostają zachowane

### 5.2. Zarządzanie ligami

#### US-005: Tworzenie ligi
Jako administrator, chcę stworzyć nową ligę, aby zapraszać znajomych do wspólnego obstawiania meczów NBA.

Kryteria akceptacji:
1. Administrator może utworzyć nową ligę podając jej nazwę i opcjonalny opis
2. System automatycznie generuje unikalny kod dostępu dla ligi
3. System weryfikuje unikalność kodu i w razie potrzeby generuje nowy
4. Administrator zostaje automatycznie przypisany jako właściciel ligi
5. Administrator może skopiować kod ligi do udostępnienia innym
6. Nowo utworzona liga pojawia się na liście lig administratora

#### US-006: Dołączanie do ligi
Jako użytkownik, chcę dołączyć do istniejącej ligi za pomocą kodu dostępu, aby móc uczestniczyć w obstawianiu.

Kryteria akceptacji:
1. Użytkownik może wprowadzić kod dostępu do ligi
2. System weryfikuje poprawność kodu
3. System wyświetla komunikat błędu przy niepoprawnym kodzie
4. Po pomyślnym dołączeniu, użytkownik jest przekierowany na stronę ligi
5. Liga pojawia się na liście dostępnych lig użytkownika
6. Użytkownik nie może dołączyć do tej samej ligi więcej niż raz

### 5.3. Zarządzanie meczami

#### US-007: Dodawanie meczu
Jako administrator ligi, chcę dodać nowy mecz do ligi, aby użytkownicy mogli obstawiać jego wynik.

Kryteria akceptacji:
1. Administrator widzi formularz dodawania nowego meczu
2. Administrator może wybrać drużyny gospodarzy i gości z predefiniowanej listy drużyn NBA
3. Administrator może ustawić datę i czas rozpoczęcia meczu (w strefie CET)
4. System waliduje wprowadzone dane (data w przyszłości, różne drużyny)
5. Dodany mecz pojawia się na liście meczów dostępnych do obstawiania
6. System zapobiega dodaniu duplikatów meczów (te same drużyny w tym samym dniu)

#### US-008: Zarządzanie listą meczów
Jako administrator ligi, chcę zarządzać listą meczów, aby aktualizować lub usuwać błędnie wprowadzone mecze.

Kryteria akceptacji:
1. Administrator widzi listę wszystkich meczów w lidze
2. Administrator może edytować mecze, które jeszcze się nie rozpoczęły
3. Administrator może usunąć mecze, które jeszcze się nie rozpoczęły
4. Administrator nie może edytować ani usunąć meczów, które już się rozpoczęły
5. Przy edycji meczu system przeprowadza taką samą walidację jak przy dodawaniu

#### US-009: Obstawianie wyniku meczu
Jako użytkownik, chcę obstawić wynik meczu, aby zdobywać punkty w lidze.

Kryteria akceptacji:
1. Użytkownik widzi listę nadchodzących meczów dostępnych do obstawiania
2. Przy każdym meczu są dostępne opcje obstawienia (gospodarz, gość, dogrywka)
3. Użytkownik może wybrać tylko jedną opcję dla meczu
4. System blokuje możliwość obstawiania po rozpoczęciu meczu
5. Użytkownik może zmienić swój typ przed rozpoczęciem meczu
6. System zapisuje ostatni typ użytkownika przed rozpoczęciem meczu
7. Użytkownik widzi informację o pozostałym czasie do rozpoczęcia meczu

#### US-010: Wprowadzanie wyników meczów
Jako administrator ligi, chcę wprowadzić wyniki zakończonych meczów, aby system mógł przyznać punkty.

Kryteria akceptacji:
1. Administrator widzi listę zakończonych meczów bez wprowadzonych wyników
2. Administrator może wybrać mecz i wprowadzić jego wynik (zwycięzca gospodarzy, gości lub dogrywka)
3. System automatycznie przyznaje punkty za poprawne typy
4. System respektuje zasady punktacji (1 punkt za poprawny typ, 0 punktów za zwycięzcę w dogrywce)
5. Po wprowadzeniu wyniku, mecz zostaje oznaczony jako zakończony
6. Użytkownicy mogą sprawdzić wyniki swoich typów

### 5.4. Przeglądanie danych

#### US-011: Przeglądanie rankingu graczy
Jako użytkownik, chcę przeglądać ranking graczy w lidze, aby zobaczyć swoją pozycję i punkty.

Kryteria akceptacji:
1. Użytkownik widzi listę wszystkich graczy w lidze
2. Ranking wyświetla login użytkownika i liczbę zdobytych punktów
3. Ranking jest sortowany malejąco według liczby punktów
4. Przy równej liczbie punktów, użytkownicy są sortowani alfabetycznie po loginie
5. Ranking jest automatycznie aktualizowany po wprowadzeniu nowych wyników

#### US-012: Przeglądanie listy lig
Jako użytkownik, chcę przeglądać listę lig, do których należę, aby łatwo poruszać się między nimi.

Kryteria akceptacji:
1. Użytkownik widzi listę wszystkich lig, do których należy
2. Dla każdej ligi wyświetlana jest jej nazwa i opcjonalnie dodatkowe informacje
3. Użytkownik może wybrać ligę, aby przejść do jej szczegółów
4. Użytkownik widzi informację, jeśli nie należy do żadnej ligi

#### US-013: Przeglądanie meczów i typów
Jako użytkownik, chcę przeglądać listę meczów i swoich typów, aby śledzić swoje obstawianie.

Kryteria akceptacji:
1. Użytkownik widzi listę wszystkich meczów w lidze
2. Dla każdego meczu wyświetlane są drużyny, data i czas rozpoczęcia
3. Dla meczów przyszłych, użytkownik widzi swój typ (jeśli już obstawił)
4. Dla zakończonych meczów, użytkownik widzi wynik oraz status swojego typu
5. Mecze są domyślnie sortowane chronologicznie

### 5.5. Funkcje SuperAdministratora

#### US-014: Zarządzanie użytkownikami przez SuperAdministratora
Jako SuperAdministrator, chcę zarządzać użytkownikami aplikacji, aby utrzymać porządek i bezpieczeństwo.

Kryteria akceptacji:
1. SuperAdministrator widzi listę wszystkich użytkowników w systemie
2. SuperAdministrator może resetować hasła użytkowników
3. SuperAdministrator może usuwać konta użytkowników
4. SuperAdministrator może przyznawać/odbierać uprawnienia administratora

#### US-015: Monitoring systemu
Jako SuperAdministrator, chcę monitorować podstawowe statystyki systemu, aby upewnić się, że działa on prawidłowo.

Kryteria akceptacji:
1. SuperAdministrator widzi dashboard z podstawowymi statystykami (liczba użytkowników, lig, meczów)
2. SuperAdministrator widzi listę ostatnio utworzonych lig
3. SuperAdministrator widzi listę najaktywniejszych użytkowników
4. Dashboard odświeża się automatycznie lub na żądanie

## 6. Metryki sukcesu

### 6.1. Funkcjonalne metryki sukcesu
- Poprawne działanie mechanizmu punktowego dla wszystkich scenariuszy typowania
- 100% meczów ma zablokowaną możliwość typowania po czasie rozpoczęcia
- Bezbłędne generowanie i weryfikacja unikalnych kodów lig
- Poprawne zarządzanie kontami użytkowników (dodawanie, usuwanie, resetowanie haseł)

### 6.2. Techniczne metryki sukcesu
- Responsywność interfejsu na desktopach (rozdzielczości od 1024x768 do 1920x1080)
- Poprawna integracja z GitHub Actions (automatyczny build i deployment)
- Czas odpowiedzi bazy danych poniżej 1 sekundy dla typowych operacji