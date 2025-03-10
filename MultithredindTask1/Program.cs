using System;
using System.Linq;
using System.Reflection;

class Program
{
    static void Main()
    {
        try
        {
            Console.Write("Введите имя класса: ");
            string className = Console.ReadLine();

            Console.Write("Введите имя метода: ");
            string methodName = Console.ReadLine();

            // Получаем тип по имени класса
            Type type = Type.GetType(className);
            if (type == null)
            {
                throw new ArgumentException("Класс не найден.");
            }

            // Создаём экземпляр класса
            object instance = Activator.CreateInstance(type);

            Console.Write("Введите аргументы метода через пробел: ");
            string[] args = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            // Ищем метод с соответствующим количеством параметров
            MethodInfo method = type.GetMethods()
                .FirstOrDefault(m => m.Name == methodName && m.GetParameters().Length == args.Length);

            if (method == null)
            {
                throw new ArgumentException("Метод не найден или количество аргументов не совпадает.");
            }

            // Получаем параметры метода
            ParameterInfo[] parameters = method.GetParameters();
            object[] convertedArgs = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                try
                {
                    convertedArgs[i] = Convert.ChangeType(args[i], parameters[i].ParameterType);
                }
                catch (Exception)
                {
                    throw new ArgumentException($"Ошибка преобразования аргумента {i + 1} в {parameters[i].ParameterType}");
                }
            }

            // Вызываем метод
            object result = method.Invoke(instance, convertedArgs);

            // Если метод что-то возвращает, выводим результат
            if (method.ReturnType != typeof(void))
            {
                Console.WriteLine("Результат выполнения: " + result);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка: " + ex.Message);
        }
    }
}
