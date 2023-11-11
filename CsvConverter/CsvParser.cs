using Newtonsoft.Json.Linq;
using System.Text;

namespace CsvConverter;

public class CsvParser
{
    private readonly CsvParserOptions _options;

    public CsvParser()
        : this(new CsvParserOptions())
    { }

    public CsvParser(CsvParserOptions parserOptions)
    {
        _options = parserOptions;
    }

    public CsvParser(Action<CsvParserOptions> optionsBuilder)
        : this(new CsvParserOptions())
    {
        optionsBuilder(_options);
    }

    public IEnumerable<JObject> Parse(string csvString, bool hasHeaders)
    {
        if (String.IsNullOrEmpty(csvString))
            yield break;

        string[] lines = csvString.Split(_options.NewLineCharacter);

        if (lines.Length == 0)
            yield break;

        var headers = new List<string>();
        if (hasHeaders)
        {
            if (lines.Length == 1)
                yield break;

            headers.AddRange(lines[0]
                .Split(_options.Delimiter)
                .Select(h => h.Trim()));
        }

        int firstDataRowIndex = hasHeaders ? 1 : 0;

        for (int row = firstDataRowIndex; row < lines.Length; row++)
        {
            var parsedRow = new JObject();
            string line = lines[row];
            string[] rowValues = GetRowValues(line);

            int minValuesToParse = Math.Min(headers.Count, rowValues.Length);

            if (hasHeaders)
            {
                if (_options.IncludeExtraHeaders)
                    minValuesToParse = Math.Max(headers.Count, minValuesToParse);
                if (_options.IncludeExtraValues)
                    minValuesToParse = Math.Max(rowValues.Length, minValuesToParse);
            }
            else
            {
                minValuesToParse = rowValues.Length;
            }

            for (int col = 0; col < minValuesToParse; col++)
            {
                string key = col < headers.Count ? headers[col] : $"Column{col + 1}";   // TODO: Make the text "Column" plugable (ColumnNamingStratergy)

                string? value = col < rowValues.Length
                    ? _options.TrimValues ? rowValues[col].Trim() : rowValues[col]
                    : default;
                object parsedValue;

                if (byte.TryParse(value, out byte byteValue))
                    parsedValue = _options.ByteParser(byteValue);
                else if (short.TryParse(value, out short shortValue))
                    parsedValue = _options.ShortParser(shortValue);
                else if (int.TryParse(value, out int intValue))
                    parsedValue = _options.IntParser(intValue);
                else if (long.TryParse(value, out long longValue))
                    parsedValue = _options.LongParser(longValue);
                else if (float.TryParse(value, out float floatValue))
                    parsedValue = _options.FloatParser(floatValue);
                else if (double.TryParse(value, out double doubleValue))
                    parsedValue = _options.DoubleParser(doubleValue);
                else if (decimal.TryParse(value, out decimal decimalValue))
                    parsedValue = _options.DecimalParser(decimalValue);
                else if (bool.TryParse(value, out bool boolValue))
                    parsedValue = _options.BoolParser(boolValue);
                else if (char.TryParse(value, out char charValue))
                {
                    parsedValue = _options.CharParser(charValue);
                    if (parsedValue is char)
                        parsedValue = parsedValue.ToString()!;  // Need this as json does not have a primitive for characters
                }
                else if (!string.IsNullOrEmpty(value))
                    parsedValue = _options.StringParser(value);
                else
                    parsedValue = value;

                parsedRow.Add(new JProperty(key, parsedValue));
            }

            yield return parsedRow;
        }
    }

    private string[] GetRowValues(string line)
    {
        var values = new List<string>();
        // TODO: compare performance of StringBuilder vs char lsit vs char span vs any other idea

        var value = new StringBuilder();
        int quotationLevel = 0;

        bool startParsingNewValue = true;

        foreach (char c in line)
        {
            if (quotationLevel == 0 && c == _options.Delimiter)
            {
                values.Add(value.ToString());
                startParsingNewValue = true;
                value.Clear();
                continue;
            }
            if (startParsingNewValue)
            {
                if (c == '"')
                {
                    startParsingNewValue = false;
                    quotationLevel++;
                }
                else
                {
                    value.Append(c);
                }
            }
            else
            {
                if (c == '"')
                {
                    quotationLevel--;
                }
                else
                {
                    value.Append(c);
                }
            }
        }
        values.Add(value.ToString());

        return values.ToArray();
    }
}
