

# Program.cs
using System;
using System.Collections.Generic;

class Program
{
    class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }

        public Book(string title, string author)
        {
            Title = title;
            Author = author;
        }

        public override string ToString()
        {
            return $"\"{Title}\" by {Author}";
        }
    }

    static void Main(string[] args)
    {
        List<Book> library = new List<Book>();
        bool running = true;

        while (running)
        {
            Console.WriteLine("\nБиблиотека книг");
            Console.WriteLine("1. Добавить книгу");
            Console.WriteLine("2. Удалить книгу");
            Console.WriteLine("3. Показать все книги");
            Console.WriteLine("4. Выход");
            Console.Write("Выберите действие: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddBook(library);
                    break;
                case "2":
                    RemoveBook(library);
                    break;
                case "3":
                    ShowBooks(library);
                    break;
                case "4":
                    running = false;
                    break;
                default:
                    Console.WriteLine("Неверный выбор. Пожалуйста, попробуйте снова.");
                    break;
            }
        }
    }

    static void AddBook(List<Book> library)
    {
        Console.Write("Введите название книги: ");
        string title = Console.ReadLine();
        Console.Write("Введите автора книги: ");
        string author = Console.ReadLine();

        library.Add(new Book(title, author));
        Console.WriteLine("Книга добавлена.");
    }

    static void RemoveBook(List<Book> library)
    {
        ShowBooks(library);

        if (library.Count == 0) return;

        Console.Write("Введите номер книги для удаления: ");
        if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= library.Count)
        {
            library.RemoveAt(index - 1);
            Console.WriteLine("Книга удалена.");
        }
        else
        {
            Console.WriteLine("Неверный ввод.");
        }
    }

    static void ShowBooks(List<Book> library)
    {
        if (library.Count == 0)
        {
            Console.WriteLine("В библиотеке нет книг.");
        }
        else
        {
            Console.WriteLine("\nСписок книг в библиотеке:");
            for (int i = 0; i < library.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {library[i]}");
            }
        }
    }
}
