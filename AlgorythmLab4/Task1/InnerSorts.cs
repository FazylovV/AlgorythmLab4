using AlgorythmLab4.Logger;

namespace AlgorythmLab4.Task1;

public static class InnerSorts
{
    public static Logger.Logger SortLogger = Logger.Logger.GetLogger(0, 1000);
    
    public static void BubbleSort<T>(this List<T> list) where T : IComparable
    {
        var n = list.Count;
        SortLogger.Info($"Начинается сортировка (Метод: Bubble Sort) массива длинной: {list.Count}.");
        SortLogger.Info($"Исходный массив: {list.GetArrayForLog()}");
        
        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                SortLogger.Info($"|   Сравниваем {j} элемент ({list[j]}) и {j + 1} элемент ({list[j + 1]})");
                if (list[j].CompareTo(list[j + 1]) > 0)
                {
                    SortLogger.Info($"|   |   {j} элемент ({list[j]}) больше, чем {j + 1} элемент ({list[j + 1]}), меняем их местами");
                    (list[j], list[j + 1]) = (list[j + 1], list[j]);
                }
            }
        }
        
        SortLogger.Info("Массив отсортирован!");
        SortLogger.Info($"Результат: {list.GetArrayForLog()}");
    }
    
    public static void ExchangeSort<T>(this List<T> list) where T : IComparable
    {
        SortLogger.Info($"Начинается сортировка (Метод: Exchange Sort) массива длинной: {list.Count}.");
        SortLogger.Info($"Исходный массив: {list.GetArrayForLog()}");
        
        for (int i = 0; i < list.Count; i++)
        {
            for (int j = i; j < list.Count; j++)
            {
                SortLogger.Info($"|   Сравниваем {i} элемент ({list[i]}) и {j} элемент ({list[j]})");
                if (list[j].CompareTo(list[i]) < 0)
                {
                    SortLogger.Info($"|   |   {j} элемент ({list[j]}) меньше, чем {i} элемент ({list[i]}), меняем их местами");
                    (list[j], list[i]) = (list[i], list[j]);
                }
            }
        }
        
        SortLogger.Info("Массив отсортирован!");
        SortLogger.Info($"Результат: {list.GetArrayForLog()}");
    }
    
    public static void QuickSort<T>(this IList<T> collection) where T : IComparable
    {
        SortLogger.Info($"Начинается сортировка (Метод: Quick Sort) массива длинной: {collection.Count}.");
        SortLogger.Info($"Исходный массив: {collection.GetArrayForLog()}");

        collection.InsideQuickSort(0, collection.Count - 1);

        SortLogger.Info("Массив отсортирован!");
        SortLogger.Info($"Результат: {collection.GetArrayForLog()}");
    }

    private static void InsideQuickSort<T>(this IList<T> collection, int left, int right) where T : IComparable
    {
        if (left < right)
        {
            SortLogger.Info($"Начинается поиск разделителя для под массива: array[{left}:{right}].");

            var pivotIndex = collection.Partition(left, right);
            collection.InsideQuickSort(left, pivotIndex);
            collection.InsideQuickSort(pivotIndex + 1, right);
        }
    }
    
    private static int Partition<T>(this IList<T> collection, int left, int right) where T : IComparable
    {
        var pivot = collection[left]; // В качестве опорного элемента выбирается самый левый элемент
        SortLogger.Info($"|   Опорный элемент (самый левый): {pivot}.");

        var i = left - 1;
        var j = right + 1;

        while (true)
        {
            SortLogger.Info($"|   Левый указатель I: {i}.");
            do
            {
                i++;
                SortLogger.Info($"|   |   Левый указатель сдвигаем вправо, I++: {i}.");
            } while (pivot.CompareTo(collection[i]) > 0); // > заменить на < для сортировки по убыванию

            SortLogger.Info($"|   Левый указатель достиг цели (pivot ({pivot}) <= элемент {i} ({collection[i]}) - выполняется).");

            SortLogger.Info($"|   Правый указатель j: {j}.");
            do
            {
                j--;
                SortLogger.Info($"|   |   Правый указатель сдвигаем влево, j--: {j}.");
            } while (collection[j].CompareTo(pivot) > 0); // > заменить на < для сортировки по убыванию

            SortLogger.Info($"|   Правый указатель достиг цели (элемент {j} ({collection[j]}) <= pivot ({pivot}) - выполняется).");

            if (i >= j)
            {
                SortLogger.Info($"|   Разделитель для под массива array[{left}:{right}] найден: {j}.");
                return j;
            }

            SortLogger.Info($"|   {j} элемент ({collection[j]}) и {i} элемент ({collection[i]}), меняем их местами");
            (collection[i], collection[j]) = (collection[j], collection[i]);
        }
    }

    public static void MergeSort<T>(this List<T> array) where T : IComparable
    {
        SortLogger.Info($"Начинается сортировка (Метод: Bubble Sort) массива длинной: {array.Count}.");
        SortLogger.Info($"Исходный массив: {array.GetArrayForLog()}");
        
        SplitAndMergeArray(array, 0, array.Count - 1);
        
        SortLogger.Info("Массив отсортирован!");
        SortLogger.Info($"Результат: {array.GetArrayForLog()}");
    }
    
    private static void SplitAndMergeArray<T>(List<T> array, int left, int right) where T : IComparable
    {
        if (left < right)
        {
            int middle = left + (right - left) / 2;
            SplitAndMergeArray(array, left, middle);
            SplitAndMergeArray(array, middle + 1, right);
            
            SortLogger.Info($"Мерджим подмассивы в массив с {left} элемента по {right} элемент");
            MergeArray(array, left, middle, right);
        }
    }
    
    private static void MergeArray<T>(List<T> array, int left, int middle, int right) where T : IComparable
    {
        var leftArrayLength = middle - left + 1;
        var rightArrayLength = right - middle;
        var leftTempArray = new T[leftArrayLength];
        var rightTempArray = new T[rightArrayLength];
        int i, j;
        
        for (i = 0; i < leftArrayLength; ++i)
            leftTempArray[i] = array[left + i];
        for (j = 0; j < rightArrayLength; ++j)
            rightTempArray[j] = array[middle + 1 + j];
        
        SortLogger.Info($"|   Создаем левый подмассив: {leftTempArray.GetArrayForLog()}");
        SortLogger.Info($"|   Создаем правый подмассив: {rightTempArray.GetArrayForLog()}");
        
        i = 0;
        j = 0;
        int k = left;
        
        while (i < leftArrayLength && j < rightArrayLength)
        {
            SortLogger.Info($"|   Сравниваем {i} элемент в левом подмассиве ({leftTempArray[i]}) и {j} элемент в правом подмассиве ({rightTempArray[j]})");
            if (leftTempArray[i].CompareTo(rightTempArray[j]) <= 0)
            {
                SortLogger.Info($"|   |   {i} элемент в левом подмассиве ({leftTempArray[i]}) меньше или равен, чем {j} элемент в правом подмассиве ({rightTempArray[j]}), присваиваем {k} элементу исходного массива значение ({leftTempArray[i]})");
                array[k++] = leftTempArray[i++];
            }
            else
            {
                SortLogger.Info($"|   |   {i} элемент в левом подмассиве ({leftTempArray[i]}) больше, чем {j} элемент в правом подмассиве ({rightTempArray[j]}), присваиваем {k} элементу исходного массива значение ({rightTempArray[j]})");
                array[k++] = rightTempArray[j++];
            }
        }
        
        while (i < leftArrayLength)
        {
            SortLogger.Info($"|   |   Присваиваем {k} элементу исходного массива {i} элемент левого подмассива ({leftTempArray[i]})");
            array[k++] = leftTempArray[i++];
        }
        
        while (j < rightArrayLength)
        {
            SortLogger.Info($"|   |   Присваиваем {k} элементу исходного массива {j} элемент правого подмассива ({rightTempArray[j]})");
            array[k++] = rightTempArray[j++];
        }
    }
}