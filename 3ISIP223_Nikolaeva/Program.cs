using static System.Net.Mime.MediaTypeNames;

Console.WriteLine("Введите количество операций ");
int k = Convert.ToInt32(Console.ReadLine());


string[] name = new string[k];
double[] price = new double[k];


for(int i = 0; i < k; i++)
{
    Console.WriteLine("Введите траты по шаблону (Название услуги или товара; Количество денег)");
    string vvod = Console.ReadLine();
    string[] words = vvod.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
    name[i] = words[0];
    price[i] = Convert.ToInt32(words[1]);
}


char da = 'y';
do

{
    Console.WriteLine("1. Вывод данных\n2. Статистика (среднее, максимальное, минимальное, сумма)\n3. Сортировка по цене (пузырьковая сортировка)\n4. Конвертация валюты (пользователь вводит курс или выбирает из списка)\n5. Поиск по названию \n0. Выход");
    int n = Convert.ToInt32(Console.ReadLine());


    switch (n)
    {
        case 0: break;
        case 1:
            Console.WriteLine("Все траты");
            for (int i = 0; i < k; i++)
            {
                Console.WriteLine($"{name[i]} {price[i]}");
            }
            break;
        case 2:

            double min = price[0];
            double max = price[0];
            double su = 0;

            for (int i = 0; i < k; i++)
            {
                if (price[i] < min) min = price[i];
                else if (price[i] > max) max = price[i];
                su += price[i];

            }

            Console.WriteLine($"Макс = {max} \nМин = {min} \nСред = {su / k} \nСумма = {su}"); break;

        case 3:

            for (int i = 0; i < k; i++)
            {
                for (int j = 0; j < k - 1; j++)
                {
                    if (price[j] > price[j + 1])
                    {
                        double t_p = price[j];
                        price[j] = price[j + 1];
                        price[j + 1] = t_p;

                        string t_n = name[j];
                        name[j] = name[j + 1];
                        name[j + 1] = t_n;

                    }
                }
            }

            for (int i = 0; i < k; i++)
            {
                Console.WriteLine($"{name[i]} {price[i]}");
            }
            break;
        case 4:
            Console.WriteLine("В какую валюту перевести \n1. Доллары\n2. Евро");
            int per = Convert.ToInt32(Console.ReadLine());
            for (int i = 0; i < k; i++)
            {
                if (per == 1) Console.WriteLine($"{name[i]} {price[i] / 83}");
                if (per == 2) Console.WriteLine($"{name[i]} {price[i] / 98}");
            }
            break;
        case 5:
            bool find = false;
            Console.WriteLine("Введите название для поиска");
            string sf = Console.ReadLine();
            for (int i = 0; i < k; i++)
            {
                if (name[i] == sf)
                {
                    find = true;
                    Console.WriteLine($"{name[i]} {price[i]}");

                }


            }
            if (!find) Console.WriteLine("Не найдено");

            break;

    }
    if (n == 0) break;
    else
    {
        Console.WriteLine("продолжить (y, n)");
        da = Convert.ToChar(Console.ReadLine());
    }
} while (da == 'y');




