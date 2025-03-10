using System;
using System.Linq;
using System.Reflection;

class task1
{
    static void Main()
    {
        try
        {
            Console.Write("Введите имя класса: ");
            string className = Console.ReadLine();

            Console.Write("Введите имя метода: ");
            string methodName = Console.ReadLine();

            Type type = Type.GetType(className);
            if (type == null)
            {
                throw new ArgumentException("Класс не найден.");
            }

            object instance = Activator.CreateInstance(type);

            Console.Write("Введите аргументы метода через пробел: ");
            string[] args = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            MethodInfo method = type.GetMethods().FirstOrDefault(m => m.Name == methodName && m.GetParameters().Length == args.Length);

            if (method == null)
            {
                throw new ArgumentException("Метод не найден или количество аргументов не совпадает.");
            }

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

            object result = method.Invoke(instance, convertedArgs);

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
