namespace CsvConverter;


// TODO: add ///summary for every class/method/property
public class CsvParserOptions
{
    public char Delimiter { get; set; } = ',';
    public string NewLineCharacter { get; set; } = Environment.NewLine;
    public bool Nested { get; set; }
    public char DotOperator { get; set; } = '.';
    public bool TrimValues { get; set; } = true;
    public bool IncludeExtraHeaders { get; set; }
    public bool IncludeExtraValues { get; set; } = true;

    // TODO: public DuplicateKeysHandlingStratergy DuplicateKeysHandlingStratergy { get; set; }
    // TODO: Shall we change the parsers from "(val) => val" to "(val, column) => val"? To take into account column name in transformation

    public Func<byte, object> ByteParser { get; set; } = (i) => i;
    public Func<short, object> ShortParser { get; set; } = (i) => i;
    public Func<int, object> IntParser { get; set; } = (i) => i;
    public Func<long, object> LongParser { get; set; } = (i) => i;
    public Func<float, object> FloatParser { get; set; } = (i) => i;
    public Func<double, object> DoubleParser { get; set; } = (i) => i;
    public Func<decimal, object> DecimalParser { get; set; } = (i) => i;
    public Func<bool, object> BoolParser { get; set; } = (i) => i;
    public Func<char, object> CharParser { get; set; } = (i) => i;
    public Func<string, object> StringParser { get; set; } = (i) => i;
}
