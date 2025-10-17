Я помогу вам выполнить лабораторную работу по изучению формирования тестовых данных. Давайте разберем каждый пункт задания.

5.1 Граничные значения и классы эквивалентности

5.1.1 Анализ параметров метода

Username:

· Допустимые значения: 3-20 символов, не пустая строка, не пробельные символы
· Недопустимые значения: null, пустая строка, пробелы, <3 символов, >20 символов
· Классы эквивалентности:
  · Допустимый: длина 3-20
  · Недопустимый: null/пустая строка
  · Недопустимый: пробельные символы
  · Недопустимый: длина <3
  · Недопустимый: длина >20

Password:

· Допустимые значения: ≥6 символов, содержит цифру, не пустая строка
· Недопустимые значения: null, пустая строка, <6 символов, без цифр
· Классы эквивалентности:
  · Допустимый: длина ≥6 с цифрой
  · Недопустимый: null/пустая строка
  · Недопустимый: длина <6
  · Недопустимый: без цифр

Age:

· Допустимые значения: 13-120
· Недопустимые значения: <13, >120
· Классы эквивалентности:
  · Допустимый: 13-120
  · Недопустимый: <13
  · Недопустимый: >120

Email:

· Допустимые значения: не пустая строка, содержит "@", заканчивается на ".edu"
· Недопустимые значения: null, пустая строка, пробелы, без "@", не заканчивается на ".edu"
· Классы эквивалентности:
  · Допустимый: содержит "@" и заканчивается на ".edu"
  · Недопустимый: null/пустая строка
  · Недопустимый: без "@"
  · Недопустимый: не заканчивается на ".edu"

5.1.2 Таблица тестовых значений

Класс эквивалентности Параметр Допустимый/Недопустимый Значение
Username   
Длина 2 username Недопустимый "ab"
Длина 3 username Допустимый "abc"
Длина 20 username Допустимый "abcdefghijklmnopqrst"
Длина 21 username Недопустимый "abcdefghijklmnopqrstu"
Пустая строка username Недопустимый ""
Null username Недопустимый null
Пробелы username Недопустимый "   "
Password   
Длина 5 с цифрой password Недопустимый "pass1"
Длина 6 без цифры password Недопустимый "password"
Длина 6 с цифрой password Допустимый "passw1"
Пустая строка password Недопустимый ""
Null password Недопустимый null
Age   
Граница 12 age Недопустимый 12
Граница 13 age Допустимый 13
Граница 120 age Допустимый 120
Граница 121 age Недопустимый 121
Email   
Корректный email Допустимый "user@domain.edu"
Без @ email Недопустимый "userdomain.edu"
Без .edu email Недопустимый "user@domain.com"
Пустая строка email Недопустимый ""
Null email Недопустимый null

5.1.3 Использование pairwise.teremokgames.com

Для использования сайта вам нужно:

1. Перейти на https://pairwise.teremokgames.com/
2. Добавить параметры с их значениями
3. Сгенерировать оптимальный набор тестов

5.2 Попарное тестирование

5.2.1 Все возможные комбинации

Если взять по 3-4 значения для каждого параметра, общее количество комбинаций будет:
3-4 значения × 4 параметра = 81-256 комбинаций

5.2.2 Сокращение комбинаций методом попарного тестирования

Попарное тестирование позволяет сократить количество тестов до 20-30 комбинаций при сохранении покрытия всех пар параметров.

5.3 Генерация больших наборов данных

5.3.1 Генерация в Mockaroo

Настройте схему в Mockaroo с следующими полями:

Схема для генерации:

· username: Text (может быть пустым, разной длины)
· password: Text (может быть пустым, с цифрами и без)
· age: Number (1-130)
· email: Custom List ("user@domain.edu", "user@domain.com", "userdomain.edu", "")

5.3.2 Консольное приложение для проверки

```csharp
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        string filePath = "test_data.csv";
        int totalTests = 0;
        int successfulTests = 0;
        int failedTests = 0;

        try
        {
            var lines = File.ReadAllLines(filePath);
            
            // Пропускаем заголовок если есть
            foreach (var line in lines.Skip(1))
            {
                var values = line.Split(',');
                
                if (values.Length >= 4)
                {
                    string username = values[0];
                    string password = values[1];
                    int age = int.TryParse(values[2], out int ageResult) ? ageResult : 0;
                    string email = values[3];

                    bool result = ValidateUser(username, password, age, email);
                    
                    Console.WriteLine($"Test {totalTests + 1}: " +
                                    $"Username='{username}', " +
                                    $"Password='{password}', " +
                                    $"Age={age}, " +
                                    $"Email='{email}' -> {(result ? "PASS" : "FAIL")}");

                    totalTests++;
                    if (result) successfulTests++;
                    else failedTests++;
                }
            }

            Console.WriteLine("\n=== RESULTS ===");
            Console.WriteLine($"Total tests: {totalTests}");
            Console.WriteLine($"Successful: {successfulTests}");
            Console.WriteLine($"Failed: {failedTests}");
            Console.WriteLine($"Success rate: {((double)successfulTests / totalTests * 100):F2}%");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading file: {ex.Message}");
        }
    }

    public static bool ValidateUser(string username, string password, int age, string email)
    {
        if (string.IsNullOrWhiteSpace(username) || username.Length < 3 || username.Length > 20)
            return false;
        if (string.IsNullOrEmpty(password) || password.Length < 6 || !password.Any(char.IsDigit))
            return false;
        if (age < 13 || age > 120)
            return false;
        if (string.IsNullOrWhiteSpace(email) || !email.Contains("@") || !email.EndsWith(".edu"))
            return false;
        return true;
    }
}
```

Рекомендации по выполнению

1. Для pairwise тестирования используйте выявленные граничные значения
2. При генерации в Mockaroo настройте распределение данных так, чтобы покрыть все классы эквивалентности
3. В консольном приложении добавьте логирование для анализа результатов
4. Проанализируйте результаты - какое соотношение успешных/неуспешных тестов получилось и почему

Этот подход позволит вам систематически протестировать метод и выявить возможные проблемы в его реализации.