using System.Text;

namespace AlgorythmLab4.Logger;

public static class Extencions
{
    private static int _numberElementsBeginAndEnd = 5;
    
    public static StringBuilder GetArrayForLog<T>(this IList<T> collection)
    {
        var sbLog = new StringBuilder($"array[{collection.Count}] = {{ ");
        if (collection.Count <= _numberElementsBeginAndEnd * 2)
        {
            for (var i = 0; i < collection.Count - 1; i++)
            {
                sbLog.Append($"{collection[i]}, ");
            }

            if (collection.Count > 0) sbLog.Append($"{collection[^1]} ");
        }
        else
        {
            for (var i = 0; i < _numberElementsBeginAndEnd; i++)
            {
                sbLog.Append($"{collection[i]}, ");
            }

            sbLog.Append("..., ");

            for (var i = collection.Count - _numberElementsBeginAndEnd; i < collection.Count - 1; i++)
            {
                sbLog.Append($"{collection[i]}, ");
            }

            sbLog.Append($"{collection[^1]} ");
        }

        sbLog.Append('}');

        return sbLog;
    }
}
