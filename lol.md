```xml
<!-- MainWindow.xaml -->
<Window x:Class="DebuggingDemo.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="Login" Height="200" Width="300">
    <StackPanel Margin="10">
        <Label Content="Логин:"/>
        <TextBox x:Name="LoginTextBox" Margin="0,5"/>
        <Label Content="Пароль:" Margin="0,10,0,0"/>
        <PasswordBox x:Name="PasswordBox" Margin="0,5"/>
        <Button Content="Войти" Click="LoginButton_Click" Margin="0,10"/>
    </StackPanel>
</Window>
```

```csharp
// MainWindow.xaml.cs
using System.Windows;

namespace DebuggingDemo
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text;
            string password = PasswordBox.Password;
            
            // Точка останова для отладки (5.2.1)
            MessageBox.Show("Вход выполнен!"); // Точка для дампа (5.3.1)
        }
    }
}
```

Для использования этого кода:

1. Создайте новый WPF-проект в Visual Studio
2. Замените содержимое MainWindow.xaml и MainWindow.xaml.cs на приведенный код
3. Запустите отладку с помощью F5 и используйте точки останова как описано в задании

Этот код создает простое окно входа с полями для логина/пароля и кнопкой, которая выводит сообщение при нажатии.
